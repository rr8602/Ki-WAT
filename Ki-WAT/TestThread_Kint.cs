// ... existing code ...
using Ki_WAT;
using NModbus;
using RollTester;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;

public class TestThread_Kint
{
    Frm_Mainfrm m_MainFrm = null;
    private Thread testThread = null;
    GlobalVal _GV;
    

    private int m_nState = 0;
    
    private readonly object _lock = new object();
    private TestThread_HLT testThread_HLT;
    
    public bool m_SrewLeftOK = false;
    public bool m_SrewRightOK = false;
    public bool m_HLA_Finish = false;
    public bool m_PEV_Finish = false;
    public bool m_SWB_Finish = false;

    public bool m_bPassNext  = false ;

    public bool m_bSendSWA = false;
    public bool m_bTestStop = false;

    //TblCarModel m_Cur_Model = new TblCarModel();
    //TblCarInfo m_Cur_CarInfo = new TblCarInfo();
    
    MeasureData m_measureData = new MeasureData();
    MeasureData m_InitMeasureData = new MeasureData();
    private Task _swaSendTask;
    private CancellationTokenSource _swaCancellationTokenSource;

    private Task _swaCheckTask;
    private CancellationTokenSource _swaCheckCancellationTokenSource;


    private Task _taCheckTask;
    private CancellationTokenSource _taCheckCancellationTokenSource;


    private Random _rand = new Random();  // 클래스 멤버로 선언 (중복 난수 방지)
    
    TblResult _result = new TblResult();

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
    private bool IsShiftEnterPressed()
    {
        return (GetAsyncKeyState(0xA0) & 0x8000) != 0 &&
               (GetAsyncKeyState(0x6B) & 0x8000) != 0;
    }

    // UI 업데이트를 위한 이벤트들
    public static event Action<string> OnStatusUpdate;
    public static event Action<string> OnErrorOccurred;
    public static event Action OnCycleFinished;
    public static event Action OnCycleStart;


    public TestThread_Kint(GlobalVal gv)
    {
        _GV = gv;
        testThread_HLT = new TestThread_HLT(gv);
        TestThread_HLT.OnCycleFinished += TestThread_HLT_OnCycleFinished;



    }

    private void TestThread_HLT_OnCycleFinished()
    {
        m_HLA_Finish = true;
    }

    public void SetMainFrame(Frm_Mainfrm frm)
    {
        m_MainFrm = frm;
        SubscribeToDppDataReceived();
        //m_MainFrm.OnDppDataReceived += OnReceiveDpp;
    }

    

    //public void OnReceiveDpp(MeasureData pData)
    //{
    //    measureData = pData;
    //}

    private void SubscribeToDppDataReceived()
    {
        try
        {
            if (m_MainFrm != null)
            {
                m_MainFrm.OnDppDataReceived += OnDppDataReceived_Handler;
            }
        }
        catch (Exception ex)
        {
            WLog("DppDataReceived 구독 실패: " + ex.Message);
        }
    }

    private void OnDppDataReceived_Handler(MeasureData data)
    {
        try
        {
            m_measureData = data.Clone();
            
        }
        catch (Exception ex)
        {
            WLog("OnDppDataReceived_Handler 오류: " + ex.Message);
        }
    }



    private void WLog(String strLog)
    {
        _GV.Log_PGM.WriteFile(strLog);
        //testLog.WriteLog(strLog);
        GWA.STM(strLog);
    }
    public int StartThread()
    {
        try
        {
            if (testThread != null)
            {
                StopThread();
                Thread.Sleep(1000);
            }
            _GV.m_bTestRun = false;
            m_nState = Constants.STEP_WAIT;
            testThread = new Thread(TestWAB);

            
            testThread.IsBackground = true;
            testThread.Start(this);
            return 1;
        }
        catch (Exception ex)
        {
            WLog("StartThread Error: " + ex.Message);
            return -1;
        }
    }
    private void UnsubscribeFromDppDataReceived()
    {
        try
        {
            var mainForm = _GV._frmMNG.GetForm<Frm_Mainfrm>();
            if (mainForm != null)
            {
                mainForm.OnDppDataReceived -= OnDppDataReceived_Handler;
            }
        }
        catch (Exception ex)
        {
            WLog("DppDataReceived 구독 해제 실패: " + ex.Message);
        }
    }

    public void StopThread()
    {
        try
        {
            UnsubscribeFromDppDataReceived();
            lock (_lock)
            {
                _GV.m_bTestRun = false;
            }
            if (testThread != null && testThread.IsAlive)
            {
                if (!testThread.Join(3000))
                {
                    WLog("TEST Thread did not exit in time. Abort Thread");
                    // 최악의 경우 Abort 고려(비권장)
                    testThread.Abort();
                }
            }
        }
        catch (Exception ex)
        {
            WLog("StopThread Error: " + ex.Message);
        }
    }
    private void UI_Update_Status(string Message)
    {
        try
        {
            WLog(Message);
            OnStatusUpdate?.Invoke(Message);
        }
        catch (Exception ex)
        {
            WLog("UI_Update_Status Error: " + ex.Message);
        }
    }


    public void StartCycle(TblCarInfo pInfo, TblCarModel pModel)
    {
        m_bTestStop = false;
        m_bPassNext = false;
        _GV.m_bTestRun = true;
        m_nState = Constants.STEP_WAIT;

    }

    private bool CheckLoopExit()
    {
        return _GV.m_bNextStep || m_bTestStop || !_GV.m_bTestRun ||  IsShiftEnterPressed() || m_bPassNext;
    }

    private void TestWAB(Object obj)
    {
        try
        {
            UI_Update_Status("ABC");
            while (true)
            {
                Thread.Sleep(10);

                if (m_bTestStop )
                {
                    continue;
                }

                if (m_nState == Constants.STEP_WAIT)
                {
                    DoWait();
                }
                else if (m_nState == Constants.STEP_MOVE_WHEELBASE)
                {
                    DoMoveWheelbase();
                }
                else if (m_nState == Constants.STEP_PEV_START)
                {

                    if (_GV.plcRead.IsPEVDegraded())
                    {
                        SetState(Constants.STEP_DETECT_CAR_WAIT);
                        continue;
                    }
                    Do_PEV_Start();
                }
                else if (m_nState == Constants.STEP_PEV_SEND_PJI)
                {
                    Do_PEV_SendPJI();
                }
                else if (m_nState == Constants.STEP_PEV_MDA_OK)
                {
                    Do_PEV_MDA_OK();
                }
                
                else if (m_nState == Constants.STEP_DETECT_CAR_WAIT)
                {
                    DoDetectCar();
                }
                else if (m_nState == Constants.STEP_PRESS_STARTCYCLE_WAIT)
                {
                    DoPressCycleStartWait();
                }
                else if (m_nState == Constants.STEP_RUNOUT_POS_0)
                {
                    DoPresRunOut_0();
                }
                else if (m_nState == Constants.STEP_RUNOUT_POS_1)
                {
                    DoPresRunOut_1();
                }
                else if (m_nState == Constants.STEP_RUNOUT_POS_2)
                {
                    DoPresRunOut_2();
                }
                else if (m_nState == Constants.STEP_HANDLE_0)
                {
                    if (_GV.plcRead.IsSteeringWheelDegraded())
                    {
                        SetState(Constants.STEP_CHECK_CENTERING);
                    }

                    DoHandle_0();
                }
                else if (m_nState == Constants.STEP_HANDLE_1)
                {
                    DoHandle_1();
                }
                else if (m_nState == Constants.STEP_CHECK_CENTERING)
                {
                    DoCheckCentering();
                }
                else if (m_nState == Constants.STEP_PRESS_MEASURE_WAIT)
                {
                    Do_PressMeasureButton_Wait();
                }
                else if (m_nState == Constants.STEP_WAIT_RUNOUT)
                {
                    Do_WaitRunOutTime();
                }
                else if (m_nState == Constants.STEP_SCREW_SEND_INFO)
                {
                    Do_SrewSendInfo();
                }
                else if (m_nState == Constants.STEP_SCREW_GET_DATA_LEFT)
                {
                    Do_GetSrewLeft();
                }
                else if (m_nState == Constants.STEP_SCREW_GET_DATA_RIGHT)
                {
                    Do_GetSrewRight();
                }
                else if (m_nState == Constants.STEP_PRESS_LEFT_FINISH)
                {
                    Do_PressLeftFinish_Wait();
                }
                else if (m_nState == Constants.STEP_PRESS_RIGHT_FINISH)
                {
                    Do_PressRightFinish_Wait();
                }
                else if (m_nState == Constants.STEP_CHECK_SREW_ALL_HOME)
                {
                    Do_Check_SrewAllHome();
                }
                else if (m_nState == Constants.STEP_PRESS_PIT_OUT_FINISH_WAIT)
                {
                    Do_Press_PitOut_Finish_wait();
                }
                else if (m_nState == Constants.STEP_DISPLAY_RESULT)
                {
                    Do_DisplayResult();
                }
                else if (m_nState == Constants.STEP_CHECK_ALL_FINISH)
                {
                    Do_CheckAllFinish();
                }
                else if (m_nState == Constants.STEP_EXIT_POSITION)
                {
                    Do_Exit_Position();
                }
                
                else if (m_nState == Constants.STEP_SAVE_DATA)
                {
                    Do_SaveData();
                }
                else if (m_nState == Constants.STEP_TICKET_PRINT)
                {
                    Do_TicketPrint();
                }
                else if (m_nState == Constants.STEP_CHECK_GO_OUT1)
                {
                    Do_CheckGoOut1();
                }
              
                else if (m_nState == Constants.STEP_GRET_OK)
                {
                    Do_GretComm(true);
                }
                else if (m_nState == Constants.STEP_FINISH)
                {
                    DoFinish();

                }
            }
        }

        catch (Exception ex)
        {
            WLog("TEST THREAD Exception : " + ex.Message);
        }

        var main = _GV._frmMNG.GetForm<Frm_Main>();
        if (main != null )
        {
            main.FinishCycleUI();
        }
            
        UI_Update_Status("Finish Thread");

    }
    public void SetState(int state)
    {
        try
        {
            
            m_nState = state;
            m_bPassNext = false;
            _GV.m_bNextStep = false;
            
            Thread.Sleep(300);
            _GV.plcWrite.GetPLCOutData();

            if (m_nState == Constants.STEP_WAIT)
            {        
            }
            else if (m_nState == Constants.STEP_PEV_START)
            {
                
            }
            else if (m_nState == Constants.STEP_PEV_SEND_PJI)
            {
                
            }
            else if (m_nState == Constants.STEP_MOVE_WHEELBASE)
            {
              
            }
            else if (m_nState == Constants.STEP_DETECT_CAR_WAIT)
            {
               
            }
            else if (m_nState == Constants.STEP_PRESS_STARTCYCLE_WAIT)
            {
               
            }
            else if (m_nState == Constants.STEP_FINISH)
            {
                UI_Update_Status("STEP_FINISH");
            }
        }

        catch (Exception ex)
        {
            WLog("SetState Error: " + ex.Message);
        }
    }

    private void InitTest()
    {
        m_HLA_Finish = false;
        _GV.Log_PGM.SetName(_GV.m_Cur_Info.CarPJINo);
        _GV.Log_LET.SetFileName(_GV.m_Cur_Info.CarPJINo);
        _result.Clear();
        m_measureData.Clear();
        DppManager.SendToDpp(DppManager.MSG_DPP_WHEELBASE, Int32.Parse(_GV.m_Cur_Model.WhelBase));
        Thread.Sleep(300);
        DppManager.SendToDpp(DppManager.MSG_DPP_VEHICLE_TYPE, Int32.Parse(_GV.m_Cur_Model.Dpp_Code));
        _GV.Log_PGM.WriteFile("Start Test : " + _GV.m_Cur_Info.CarPJINo);

        StopSWASending(); // Task 중단
        StopSWAChecking();
        StopTAChecking();
        _GV.plcWrite.ClearData();


    }
    private void DoWait()
    {
        try
        {
            m_bTestStop = false;
            while (true)
            {
                Thread.Sleep(10);
                if ( _GV.m_bTestRun ) break;
            }
            InitTest();
            SetState(Constants.STEP_MOVE_WHEELBASE);
        }
        catch (Exception ex)
        {
            WLog("DoWait Error: " + ex.Message);
        }
    }


    private int DoMoveWheelbase()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Wait Wheelbase position");
            //_GV._PLCVal.DO._Wheelbase_Start = true;
            //_GV.plcWrite.SetLeftDivDistance(1, Convert.ToInt32(_GV.m_Cur_Model.WhelBase));
            //_GV.plcWrite.SetRightDivDistance(1, Convert.ToInt32(_GV.m_Cur_Model.WhelBase));
            //_GV.plcWrite.SetWheelbaseMoving(true);
            //_GV.plcWrite.SetDiv(ushort.Parse(_GV.m_Cur_Model.WB___Div));

            int nDist = Convert.ToInt32(_GV.m_Cur_Model.WhelBase) - Convert.ToInt32(_GV.Config.Wheelbase.HOME_POS);

            _GV.plcWrite.SetPJI(_GV.m_Cur_Info.CarPJINo);
            _GV.plcWrite.SetLeftDivDistance(1, nDist);
            _GV.plcWrite.SetRightDivDistance(1, nDist);
            _GV.plcWrite.SetDiv(1);
            _GV.plcWrite.SetCodeOK(true);
            _GV.plcWrite.WritePLCData();

            
            //_GV.plcWrite.setPJI(_GV.m_Cur_Info.CarPJINo);

            //while (true)
            //{
            //    if (CheckLoopExit()) break;

            //    int WBDist = Convert.ToInt32(_GV.m_Cur_Model.WhelBase);
            //    if ( _GV.plcRead.GetLeftDistance() == WBDist)
            //    {
            //        break;
            //    }
            //    Thread.Sleep(100);
            //}
            //_GV._PLCVal.DO._Wheelbase_Start = false;
            
            
            SetState(Constants.STEP_PEV_START);
            // 입구 신호등 GREEN;
            //_GV._PLCVal.DO.

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        return nRet;
    }

    #region _PEV 관련
    private void Do_PEV_Start()
    {
        try
        {
			// NEED PEV. Start cycle set Value 1;
			//_GV._VEP_Client.SetAllSyncZero();
			//_GV._VEP_Client.SetStartCycle();
			_GV.vep.B_SetSyncroAllZero();
			_GV.vep.B_StartCycle();




            // NEED PLC. 여기서 휠베이스 이동 명령어도 전송;
           // _GV._PLCVal.DO._Wheelbase_Start = true;
            UI_Update_Status("STEP_PEV_START");

            while (true)
            {
                if (CheckLoopExit()) break;

                if (_GV.vep.IsRequestPJIfromTZ()) break;
                
            }

            SetState(Constants.STEP_PEV_SEND_PJI);
        }
        catch (Exception ex)
        {
            WLog("Do_PEV_Start Error: " + ex.Message);
        }
    }
    private void Do_PEV_SendPJI()
    {
        try
        {
			//_GV._VEP_Data.SendPJI(_GV.m_Cur_Info.CarPJINo);
			_GV.vep.B_SendPJIMultiple(_GV.m_Cur_Info.CarPJINo);

            UI_Update_Status("STEP_PEV_SEND_PJI");
            //_GV._VEP_Data.TransmissionZone.ExchStatus = 2;
            const double TIMEOUT_SECONDS = 0.5;
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                
            }
            SetState(Constants.STEP_PEV_MDA_OK);
        }
        catch (Exception ex)
        {
            WLog("Do_PEV_SendPJI Error: " + ex.Message);
        }
    }

    private void Do_PEV_MDA_OK()
    {
        try
        {
            UI_Update_Status("Wait MDA Connect");
            while (true)
            {
                if (CheckLoopExit()) break;
                //if (_GV._VEP_Data.SynchroZone.GetSyncroValue(0) == 1 ) break;
                if (_GV.vep.vepData.SYData[0] == 1 && _GV.vep.vepData.VEPStatus == 2)
                {
                    Broker.dsBroker.Publish(Topics.PEV.MDA_OK, 0);
                    Broker.dsBroker.Publish(Topics.PEV.Message, "MDA_OK");

                    break;
                }

			}
            //_GV._VEP_Client.SetSync30NewVehicle();
			

            if ( !_GV.plcRead.IsHLADegraded() )testThread_HLT.StartThread();
            SetState(Constants.STEP_DETECT_CAR_WAIT);
        }
        catch (Exception ex)
        {
            WLog("Do_PEV_SendPJI Error: " + ex.Message);
        }
    }
    #endregion  _PEV 관련

    private int DoDetectCar()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_DETECT_CAR_WAIT");
            int nModelWB = Convert.ToInt32(_GV.m_Cur_Model.WhelBase);
            int nHomeWB = Convert.ToInt32(_GV.Config.Wheelbase.HOME_POS);
            int nDist = nModelWB - nHomeWB;
            while (true)
            {
                if (CheckLoopExit()) break;

                int leftDist = _GV.plcRead.GetLeftDistance();
                int rightDist = _GV.plcRead.GetRightDistance();

                bool isLeftInRange = Math.Abs(leftDist - nDist) < 5;
                bool isRightInRange = Math.Abs(rightDist - nDist) < 5;

                if (_GV.plcRead.IsFrontCarDetection() && _GV.plcRead.IsRearCarDetection() &&
                    isLeftInRange && isRightInRange)
                {
                    
                    break;
                }
            }

            _GV.vep.SetSync30NewVehicle();
            SetState(Constants.STEP_PRESS_STARTCYCLE_WAIT);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        
      
        return nRet;
    }

    private int DoPressCycleStartWait()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_PRESS_STARTCYCLE_WAIT");
            while (true)
            {
                if (CheckLoopExit()) break;
                
                if (_GV.plcRead.IsStartCycleButton())
                {
                    break;
                }

                
            }

            
            //_GV._PLCVal.DO._Wheelbase_Start = false;
            OnCycleStart?.Invoke();
            SetState(Constants.STEP_RUNOUT_POS_0);


        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }


        // 헤드라이트 시작 
       
        return nRet;
    }

    private int DoPresRunOut_0()
    {
        int nRet = 0;
        try
        {
           // DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_START);
            UI_Update_Status(" Check Roller Run");

            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsMotorRun())  break;

                
            }
            SetState(Constants.STEP_HANDLE_0);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($" DoPresRunOut_0 : {ex.Message}");
        }
        return nRet;
    }

    private int DoPresRunOut_1()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("RUN-OUT_1");

            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 0.1;
            
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                
            }
            SetState(Constants.STEP_RUNOUT_POS_2);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoPresRunOut_1:  {ex.Message}");
        }

        return nRet;
    }
    private int DoPresRunOut_2()
    {
        int nRet = 0;
        try
        {

            UI_Update_Status("RUN-OUT_2");
            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 0.1;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                
                Thread.Sleep(10);
            }

            SetState(Constants.STEP_HANDLE_0);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoPresRunOut_2:  {ex.Message}");
        }
        
        
        Thread.Sleep(100);
        return nRet;
    }

    private int DoHandle_0()
    {
        int nRet = 0;
        try
        {
          
            UI_Update_Status("Handle_0");
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if (!_GV.plcRead.IsSteeringWheelBalanceHome()) break;

            }
            Thread.Sleep(300);
            SetState(Constants.STEP_HANDLE_1);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoHandle_0:  {ex.Message}");
        }
        
        
        Thread.Sleep(100);
        return nRet;
    }
    private int DoHandle_1()
    {
        int nRet = 0;
        try
        {

            UI_Update_Status("Handle_1");
            DateTime startTime = DateTime.Now;

            double st = Double.Parse(_GV.m_Cur_Model.SWA___ST) / 60;
            double lt = Double.Parse(_GV.m_Cur_Model.SWA___LT) / 60;
    

            while (true)
            {
                if (CheckLoopExit()) break;


                double handle = _GV.dHandle;

                if (handle >= (st - lt) && handle <= (st + lt))
                {
                    _GV.plcWrite.SetTolranceOK(true);
                    _GV.plcWrite.SetCodeOK(false);
                    _GV.plcWrite.WritePLCData();
                    break;
                }
            }

            SetState(Constants.STEP_HANDLE_1);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoHandle_1:  {ex.Message}");
        }
        
        SetState(Constants.STEP_CHECK_CENTERING);
        Thread.Sleep(100);
        return nRet;
    }

    private int DoCheckCentering()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("DoCheckCentering");
            DateTime startTime = DateTime.Now;
            double dStd = 0.5;
            while (true)
            {
                if (CheckLoopExit()) break;
                double dSymm = Math.Abs(_GV.g_MeasureData.dSymm);
                if (dSymm < 0.5 )
                {
                    _GV.plcWrite.SetAccurancyCheck(true);
                    break;
                }
                
            }
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoCheckCentering:  {ex.Message}");
        }

        _GV.plcWrite.SetAccurancyCheck(true);
        _GV.plcWrite.WritePLCData();
        SetState(Constants.STEP_PRESS_MEASURE_WAIT);
        Thread.Sleep(100);
        return nRet;
    }
    
    private int Do_PressMeasureButton_Wait()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_PresstheMeasureButton_Wait");
            _GV.plcWrite.SetCodeOK(false);
            _GV.plcWrite.WritePLCData();

            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsMeasureStart())
                {
                    break;
                }
                Thread.Sleep(10);
            }

            m_InitMeasureData.Clear();
            SetState(Constants.STEP_WAIT_RUNOUT);



            // Measuring start 눌렀을때 여기서 실행
            //DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_START);
            
            

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PresstheMeasureButton_Wait:  {ex.Message}");
        }

        return nRet;
    }

    private void StopSWASending()
    {
        m_bSendSWA = false;
        _swaCancellationTokenSource?.Cancel();
        _swaSendTask?.Wait(3000);
    }

    private void StopTAChecking()
    {

        _taCheckCancellationTokenSource?.Cancel();
        _taCheckTask?.Wait(3000);
    }


    private void StopSWAChecking()
    {

        _swaCheckCancellationTokenSource?.Cancel();
        _swaCheckTask?.Wait(3000);
    }


    private void StartSWASending()
    {
        _swaCancellationTokenSource = new CancellationTokenSource();
        _swaSendTask = Task.Run(() => SendSWALoop(_swaCancellationTokenSource.Token));
    }
    private void StartSWAChecking()
    {
        _swaCheckCancellationTokenSource = new CancellationTokenSource();
        _swaCheckTask = Task.Run(() => SendSWACheck(_swaCheckCancellationTokenSource.Token));
    }
    private void StartTAChecking()
    {
        _taCheckCancellationTokenSource = new CancellationTokenSource();
        _taCheckTask = Task.Run(() => SendTACheck(_taCheckCancellationTokenSource.Token));
    }


    public ushort GetABFromOffset(ushort offset, int nA, int nB, int nC, double dX)
    {
        switch (offset)
        {
            case 1:     // Synchro 12 = 1
                nA = 10;
                nB = 0;
                nC = 1;
                break;

            case 100:   // Synchro 12 = 100
                nA = 1;
                nB = 40;
                nC = 1;
                break;

            case 101:   // Synchro 12 = 101
                nA = 2;
                nB = -20;
                nC = 10;
                break;

            default:
                // 1xx 범위라면 x값으로 계산
                if (offset >= 100 && offset <= 199)
                {
                    // 이부분 물어봐야함;
                    int x = offset % 100;
                    nA = x;
                    nB = x;
                    
                }
                else
                {
                    return 65535;
                }
                    break;
        }


        if (nC == 0)
            nC = 1; // 0으로 나누기 방지

        double result = (nA * dX + nB) / nC;

        // ushort 범위(0 ~ 65535)로 클램핑
        if (result < 0)
            result = 0;
        else if (result > ushort.MaxValue)
            result = ushort.MaxValue;

        return (ushort)Math.Round(result);

    }

    private async Task SendSWALoop(CancellationToken cancellationToken)
    {
        try
        {
            while (m_bSendSWA && !cancellationToken.IsCancellationRequested)
            {

				//ushort isSWB = _GV._VEP_Data.SynchroZone.GetSyncroValue(8);
				ushort isSWB = _GV.vep.vepData.SYData[8];

				if ( isSWB  == 1 || isSWB == 2)
                {
                    int nA = 100;
                    int nB = 0;
                    ushort usData = (ushort)(nA * _GV.dHandle + nB);
					//_GV._VEP_Client.SetSync07SWBAngle(usData);
					_GV.vep.SetSync07SWBAngle(usData);

				}
                // 전송 간격 (예: 100ms)
                await Task.Delay(1000, cancellationToken);

                //if (_GV._VEP_Data.SynchroZone.GetSyncroValue(9) == 1 ||
                //     _GV._VEP_Data.SynchroZone.GetSyncroValue(9) == 2)
                //{
                //    StopSWASending();
                //}
				if (_GV.vep.vepData.SYData[9] == 1 ||
					 _GV.vep.vepData.SYData[9] == 2)
				{
					StopSWASending();
				}

				


			}
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소
        }
        catch (Exception ex)
        {
            WLog($"SWA 전송 중 오류: {ex.Message}");
        }
    }

    private async Task SendTACheck(CancellationToken cancellationToken)
    {
        try
        {
            while (!cancellationToken.IsCancellationRequested)
            {

                if (_GV.vep.vepData.SYData[13] == 2 || _GV.vep.vepData.SYData[13] == 1)
                {
                    StopTAChecking();
                }
                if (_GV.vep.vepData.SYData[12] == 1 || _GV.vep.vepData.SYData[12] == 2 || _GV.vep.vepData.SYData[12] == 3 || _GV.vep.vepData.SYData[12] == 4) 
                {
                    SendThrustangle();
                }

                // 전송 간격 (예: 100ms)
                await Task.Delay(300, cancellationToken);

            }
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소
        }
        catch (Exception ex)
        {
            WLog($"SWA 전송 중 오류: {ex.Message}");
        }
    }

    private async Task SendSWACheck(CancellationToken cancellationToken)
    {
        try
        {
            while (m_bSendSWA && !cancellationToken.IsCancellationRequested)
            {

				if (_GV.vep.vepData.SYData[20] == 3)
				{
					StopSWAChecking();
				}
				if (_GV.vep.vepData.SYData[20] == 1)
				{
					SendFinalSWB();
					StopSWAChecking();
				}

                if (_GV.vep.vepData.SYData[22] == 2 || _GV.vep.vepData.SYData[22] == 1)
                {
                    StopSWAChecking();
                }

				// 전송 간격 (예: 100ms)
				await Task.Delay(300, cancellationToken);

            }
        }
        catch (OperationCanceledException)
        {
            // 정상적인 취소
        }
        catch (Exception ex)
        {
            WLog($"SWA 전송 중 오류: {ex.Message}");
        }
    }

    private int Do_WaitRunOutTime()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Running out.");
            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 10;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;


                // 실제로 값을 받았는지 체크 ;;
                Thread.Sleep(10);
            }

            // RUNOUT 이 끝나고 
            //_GV._VEP_Client.SetSync06RunOutFinish();


            _GV.plcWrite.SetMeasurementFinish(true);
            _GV.plcWrite.WritePLCData();

            _GV.vep.SetSync06RunOutFinish();

			m_bSendSWA = true;
            StartSWASending();
            StartSWAChecking();
            m_InitMeasureData = m_measureData.Clone();
            m_InitMeasureData.dHandle = _GV.dHandle;

            SetState(Constants.STEP_SCREW_SEND_INFO);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PresstheMeasureButton_Wait:  {ex.Message}");
        }
        
        
        Thread.Sleep(100);
        return nRet;
    }
    private int Do_SrewSendInfo()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_SrewSendInfo");
            var main = _GV._frmMNG.GetForm<Frm_Mainfrm>();
            if (main != null && main.ScrewDriverL != null)
            {
                if ( !_GV.plcRead.IsLeftScrewDriverDegraded() )
                    main.ScrewDriverL.Send("asdf");
            }
            if (main != null && main.ScrewDriverR != null)
            {                
                if (!_GV.plcRead.IsRightScrewDriverDegraded())
                    main.ScrewDriverR.Send("asdf");
            }
            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 0.5;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_SCREW_GET_DATA_LEFT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        
        return nRet;
    }

   
    private int Do_GetSrewLeft()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_GetSrewLeft");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;
            //StartTAChecking();
            m_SrewLeftOK = true;

            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                

                if (m_SrewLeftOK) break;
            
                if (_GV.plcRead.IsPitInLeftFinish()) break;

                Thread.Sleep(10);
            }
            SetState(Constants.STEP_PRESS_LEFT_FINISH);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_GetSrewLeft:  {ex.Message}");
        }
        return nRet;
    }

    private int Do_PressLeftFinish_Wait()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_PressLeftFinish_Wait");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;
            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                
                if (_GV.plcRead.IsPitInLeftFinish() || _GV.plcRead.IsPitInRightFinish()) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_SCREW_GET_DATA_RIGHT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressLeftFinish_Wait:  {ex.Message}");
        }
        return nRet;
    }

    private int Do_GetSrewRight()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_GetSrewRight");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;
            m_SrewRightOK = true;

            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                
                if (m_SrewRightOK) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_PRESS_RIGHT_FINISH);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        return nRet;
    }
    private int Do_PressRightFinish_Wait()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_PressRightFinish_Wait");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;
            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                if (_GV.plcRead.IsPitInLeftFinish() || _GV.plcRead.IsPitInRightFinish()) break;
                Thread.Sleep(10);

                //_GV.plcWrite.
            }
            
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_STOP, 0);
            _GV.plcWrite.SetScrewDriverOK(true);
            _GV.plcWrite.WritePLCData();
            SetState(Constants.STEP_CHECK_SREW_ALL_HOME);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressRightFinish_Wait:  {ex.Message}");
        }   
        return nRet;
    }
    private int Do_Check_SrewAllHome()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_Check_SrewAllHome");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;
            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                if (_GV.plcRead.IsScrewDriverLeftHome() &&
                    _GV.plcRead.IsScrewDriverRightHome() &&
                    _GV.plcRead.IsUnWrenchHome()
                    )
                    break;

                Thread.Sleep(10);
            }
            SetState(Constants.STEP_PRESS_PIT_OUT_FINISH_WAIT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressPitOut_Finish:  {ex.Message}");
        }   
        return nRet;
    }
    private void SendThrustangle()
    {   
        //ushort Offset = _GV._VEP_Data.SynchroZone.GetSyncroValue(12);
		ushort Offset = _GV.vep.vepData.SYData[12];


		int nA = 0;
        int nB = 0;
        int nC = 0;
        ushort usData = GetABFromOffset(Offset, nA, nB, nC, _GV.g_MeasureData.dTA);

        if ( usData != 65535 )
        {
            usData = (ushort)_rand.Next(0, 1001);

            //_GV._VEP_Client.SetSync10TAReady();
            //_GV._VEP_Client.SetSync11TAAngle(usData);
            _GV.vep.SetSync11TAAngle(usData);

            _GV.vep.SetSync10TAReady();
			
		}
	}
    private void SendFinalSWB()
    {
        //ushort isSWB = _GV._VEP_Data.SynchroZone.GetSyncroValue(20);
        //if (isSWB == 3)
        {
            int nA = 100;
            int nB = 0;
            ushort usData = (ushort)(nA * _GV.dHandle + nB);

			//_GV._VEP_Client.SetSync04SWB_Imform();
			//_GV._VEP_Client.SetSync07SWBAngle(usData);

			_GV.vep.SetSync04SWB_Imform();
			_GV.vep.SetSync07SWBAngle(usData);

		}

	}
    private int Do_Press_PitOut_Finish_wait()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_Press_PitOut_Finish_wait");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;

            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                if (_GV.plcRead.IsExitPlatform())
                    break;
                Thread.Sleep(10);
            }
            StopSWASending(); // Task 중단
            SendThrustangle();
            Thread.Sleep(100);
            //SendFinalSWB();
            SetState(Constants.STEP_CHECK_ALL_FINISH);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Press_PitOut_Finish_wait:  {ex.Message}");
        }

        
        
        return nRet;
    }

    private int Do_CheckAllFinish()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_CheckAllFinish");
            DateTime startTime = DateTime.Now;

            
            while (true)
            {
                if (CheckLoopExit()) break;

                m_HLA_Finish = _GV.plcRead.IsHLADegraded() || _GV.plcRead.IsHLAHomePosition();
                m_SWB_Finish = _GV.plcRead.IsSteeringWheelDegraded() || _GV.plcRead.IsSteeringWheelBalanceHome();
                if (!_GV.plcRead.IsPEVDegraded())
                {
                    int syValue = _GV.vep.vepData.SYData[1];

                    if (syValue == 2 || syValue == 3 || syValue == 1)
                    {
                        _GV.plcWrite.SetVEPFinish(true);
                        _GV.plcWrite.WritePLCData();
                        m_PEV_Finish = true;

                    }
                   
                }
                if (m_HLA_Finish && m_SWB_Finish && m_PEV_Finish)
                {
                    break;
                }
                
            }
            SetState(Constants.STEP_DISPLAY_RESULT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckAllFinish:  {ex.Message}");
        }
        return nRet;
    }

    private int Do_DisplayResult()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_DisplayResult");
            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 1;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                Thread.Sleep(10);
            }
            SetState(Constants.STEP_EXIT_POSITION);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Press_PitOut_Finish_wait:  {ex.Message}");
        }
        
        return nRet;

    }
   

    private int Do_Exit_Position()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_Exit_Position");
            DateTime startTime = DateTime.Now;

            

            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsExitPosition()) break;
                
            }
            SetState(Constants.STEP_SAVE_DATA);
            //피트 신호등 RED
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Exit_Position:  {ex.Message}");
        }
        
        
        
        return nRet;

    }

    private int Do_SaveData()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_SaveData");
            SaveLocalDB();
            Thread.Sleep(300);
            SaveXmlFile();
            SetState(Constants.STEP_TICKET_PRINT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }

        return nRet;

    }
    private int Do_TicketPrint()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_TicketPrint");
            DateTime startTime = DateTime.Now;


            //ZPrintController _PrintTicket = new ZPrintController();
            //AlignmentReportData data = MakePrintData();
            //_PrintTicket.PrintAlignmentData(data);

            while (true)
            {
                if (CheckLoopExit()) break;

                Thread.Sleep(100);
                _GV.vep.B_BenchCycleEnd();
                break;
            }
            SetState(Constants.STEP_CHECK_GO_OUT1);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_TicketPrint:  {ex.Message}");
        }

        return nRet;

    }


    private int Do_CheckGoOut1()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_CHECK_GO_OUT1");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                if (!_GV.plcRead.IsFrontCarDetection() &&
                     !_GV.plcRead.IsRearCarDetection()) break;

                Thread.Sleep(10);
            }
            SetState(Constants.STEP_GRET_OK);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckGoOut1:  {ex.Message}");
        }

        return nRet;

    }

    private int Do_GretComm(bool bOK)
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_GretComm");
            DateTime startTime = DateTime.Now;

            const double TIMEOUT_SECONDS = 0.5;
            // GRET 성공시
            if (bOK)
            {
                // 성공시 GRET ;
            }
            else
            {
                // NG 시 Gret;
            }
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                //DB Write.
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_FINISH);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_GretComm:  {ex.Message}");
        }
        return nRet;
    }

    private void DoFinish()
    {
        try
        {
            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 5.0;
            while (true)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                {
                    break;
                }
                if (CheckLoopExit()) break;
                Thread.Sleep(10);
            }
            FinishTest();
        }
        catch (Exception ex)
        {
            WLog("DoWait Error: " + ex.Message);
        }
    }


    private bool TotalPanjung()
    {
        bool bResult = true;

        bResult = GetResultOKNG(_result.CarFLToe, _GV.m_Cur_Model.ToeFL_ST, _GV.m_Cur_Model.ToeFL_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarFRToe, _GV.m_Cur_Model.ToeFR_ST, _GV.m_Cur_Model.ToeFR_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarFTToe, _GV.m_Cur_Model.TotToef_ST, _GV.m_Cur_Model.TotToef_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarRLToe, _GV.m_Cur_Model.ToeRL_ST, _GV.m_Cur_Model.ToeRL_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarRRToe, _GV.m_Cur_Model.ToeRR_ST, _GV.m_Cur_Model.ToeRR_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarRTToe, _GV.m_Cur_Model.TotToer_ST, _GV.m_Cur_Model.TotToer_LT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.CarDogRu, _GV.m_Cur_Model.DogRunST, _GV.m_Cur_Model.DogRunLT);
        if (!bResult) return bResult;

        bResult = GetResultOKNG(_result.Car_Hand, _GV.m_Cur_Model.HandleST, _GV.m_Cur_Model.HandleLT);
        if (!bResult) return bResult;
        return bResult;
    }
    private bool GetResultOKNG(string stractual, string strstandard, string strtolerance)
    {

        double actual, standards, tolerance;
        actual  = Convert.ToDouble(stractual);
        standards = Convert.ToDouble(strstandard);
        tolerance = Convert.ToDouble(strtolerance);

        double min = standards - tolerance;
        double max = standards + tolerance;
        return (actual >= min && actual <= max);
        
    }
   private void SaveLocalDB()
    {
        DateTime startTime = DateTime.Now;
        _result.AcceptNo = _GV.m_Cur_Info.AcceptNo;
        _result.PJI_Num  = _GV.m_Cur_Info.CarPJINo;
        _result.Model_NM = _GV.m_Cur_Model.Model_NM;

        _result.CarFLToe = m_measureData.dToeFL.ToString("F1");
        _result.CarFRToe = m_measureData.dToeFR.ToString("F1");
        _result.CarFTToe = (m_measureData.dToeFL + m_measureData.dToeFR).ToString("F1");
        _result.CarRLToe = m_measureData.dToeRL.ToString("F1");
        _result.CarRRToe = m_measureData.dToeRR.ToString("F1");
        _result.CarRTToe = (m_measureData.dToeRL + m_measureData.dToeRR).ToString("F1");
        _result.CarFLCam = m_measureData.dCamFL.ToString("F1");
        _result.CarFRCam = m_measureData.dCamFR.ToString("F1");
        _result.CarFCros = (m_measureData.dCamFL - m_measureData.dCamFR).ToString("F1");
        _result.CarRLCam = m_measureData.dCamRL.ToString("F1");
        _result.CarRRCam = m_measureData.dCamRR.ToString("F1");
        _result.CarRCros = (m_measureData.dCamRL - m_measureData.dCamRR).ToString("F1");
        _result.Car_Hand = _GV.dHandle.ToString("F1");
        _result.CarDogRu = m_measureData.dTA.ToString("F1");
        _result.Car_Symm = m_measureData.dSymm.ToString("F1");
        _result.WTstTime = startTime.ToString("HH:mm:ss");

        if (TotalPanjung())            _result.WAT___PK = "OK";
        else           _result.WAT___PK = "NG";

        Debug.Print("");
        _GV._dbJob.InsertResult(_result);
    }
    

    private void SaveXmlFile()
    {
        try
        {
            TblCarLamp pLamp = _GV._dbJob.SelectCarLamp(_GV.m_Cur_Info.AcceptNo);
            StationSetting setting = XMLWriter.MakeXMLStruct(_GV.m_Cur_Model, _GV.m_Cur_Info, _result, pLamp);

            string basePath = _GV.Config.Program.RESULT_PATH;
            string strPath = Path.Combine(
                basePath,
                DateTime.Now.Year.ToString("D4"),
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2")
            );

            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);
            string filePath = Path.Combine(strPath, _GV.m_Cur_Info.CarPJINo + ".xml");
            XMLWriter.WriteXML(setting, filePath);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"SaveXmlFile:  {ex.Message}");
        }
    }

    private AlignmentReportData MakePrintData()
    {
        AlignmentReportData _Data = new AlignmentReportData();


        bool bResult ;

        bResult = GetResultOKNG(_result.CarFLToe, _GV.m_Cur_Model.ToeFL_ST, _GV.m_Cur_Model.ToeFL_LT);
        _Data.FlToe.After = m_measureData.dToeFL.ToString("F1");
        _Data.FlToe.Before = m_InitMeasureData.dToeFR.ToString("F1");
        _Data.FlToe.Evaluation = bResult ? "OK" : "NOK" ;
        _Data.FlToe.ScrewingDone = "Yes";


        bResult = GetResultOKNG(_result.CarFRToe, _GV.m_Cur_Model.ToeFR_ST, _GV.m_Cur_Model.ToeFR_LT);
        _Data.FrToe.After = m_measureData.dToeFR.ToString("F1");
        _Data.FrToe.Before = m_InitMeasureData.dToeFR.ToString("F1");
        _Data.FrToe.Evaluation = bResult ? "OK" : "NOK";
        _Data.FrToe.ScrewingDone = "Yes";

        bResult = GetResultOKNG(_result.CarFTToe, _GV.m_Cur_Model.TotToef_ST, _GV.m_Cur_Model.TotToef_LT);
        _Data.FrontTotalToe.After = (m_measureData.dToeFL + m_measureData.dToeFR).ToString("F1");
        _Data.FrontTotalToe.Before = (m_InitMeasureData.dToeFR + m_InitMeasureData.dToeFR).ToString("F1") ;
        _Data.FrontTotalToe.Evaluation = bResult ? "OK" : "NOK";
        _Data.FrontTotalToe.ScrewingDone = "";

        bResult = GetResultOKNG(_result.Car_Hand, _GV.m_Cur_Model.HandleST, _GV.m_Cur_Model.HandleLT);
        _Data.SteeringWheelAlignment.After = _result.Car_Hand;
        _Data.SteeringWheelAlignment.Before = m_InitMeasureData.dHandle.ToString("F1");
        _Data.SteeringWheelAlignment.Evaluation = bResult ? "OK" : "NOK";
        _Data.SteeringWheelAlignment.ScrewingDone = "Yes";

        bResult = GetResultOKNG(_result.CarRLToe, _GV.m_Cur_Model.ToeRL_ST, _GV.m_Cur_Model.ToeRL_LT);
        _Data.RlToe.After = m_measureData.dToeRL.ToString("F1");
        _Data.RlToe.Before = m_InitMeasureData.dToeRR.ToString("F1");
        _Data.RlToe.Evaluation = bResult ? "OK" : "NOK";
        _Data.RlToe.ScrewingDone = "";

        bResult = GetResultOKNG(_result.CarRRToe, _GV.m_Cur_Model.ToeRR_ST, _GV.m_Cur_Model.ToeRR_LT);
        _Data.RrToe.After = m_measureData.dToeRR.ToString("F1");
        _Data.RrToe.Before = m_InitMeasureData.dToeRR.ToString("F1");
        _Data.RrToe.Evaluation = bResult ? "OK" : "NOK";
        _Data.RrToe.ScrewingDone = "";

        bResult = GetResultOKNG(_result.CarRTToe, _GV.m_Cur_Model.TotToer_ST, _GV.m_Cur_Model.TotToer_LT);
        _Data.RearTotalToe.After = (m_measureData.dToeRL + m_measureData.dToeRR).ToString("F1");
        _Data.RearTotalToe.Before = (m_InitMeasureData.dToeRR + m_InitMeasureData.dToeRR).ToString("F1");
        _Data.RearTotalToe.Evaluation = bResult ? "OK" : "NOK";
        _Data.RearTotalToe.ScrewingDone = "";

        bResult = GetResultOKNG(_result.CarDogRu, _GV.m_Cur_Model.DogRunST, _GV.m_Cur_Model.DogRunLT);
        _Data.RearThrustAngle.After = m_measureData.dTA.ToString("F1");
        _Data.RearThrustAngle.Before = m_InitMeasureData.dTA.ToString("F1");
        _Data.RearThrustAngle.Evaluation = bResult ? "OK" : "NOK";
        _Data.RearThrustAngle.ScrewingDone = "";

        bResult = true;
        _Data.FlCamber.After = m_measureData.dCamFL.ToString("F1");
        _Data.FlCamber.Before = m_InitMeasureData.dCamFL.ToString("F1");
        _Data.FlCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.FlCamber.ScrewingDone = "";

        _Data.FrCamber.After = m_measureData.dCamFR.ToString("F1");
        _Data.FrCamber.Before = m_InitMeasureData.dCamFR.ToString("F1");
        _Data.FrCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.FrCamber.ScrewingDone = "";

        _Data.FrontDissymmetryOfCamber.After = (m_measureData.dCamFR - m_measureData.dCamFL).ToString("F1");
        _Data.FrontDissymmetryOfCamber.Before = (m_InitMeasureData.dCamFR - m_InitMeasureData.dCamFL).ToString("F1");
        _Data.FrontDissymmetryOfCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.FrontDissymmetryOfCamber.ScrewingDone = "";

        _Data.RlCamber.After = m_measureData.dCamRL.ToString("F1");
        _Data.RlCamber.Before = m_InitMeasureData.dCamRL.ToString("F1");
        _Data.RlCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.RlCamber.ScrewingDone = "";
              
        _Data.RrCamber.After = m_measureData.dCamRR.ToString("F1");
        _Data.RrCamber.Before = m_InitMeasureData.dCamRR.ToString("F1");
        _Data.RrCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.RrCamber.ScrewingDone = "";
              
        _Data.RearDissymmetryOfCamber.After = (m_measureData.dCamRR - m_measureData.dCamRL).ToString("F1");
        _Data.RearDissymmetryOfCamber.Before = (m_InitMeasureData.dCamRR - m_InitMeasureData.dCamRL).ToString("F1");
        _Data.RearDissymmetryOfCamber.Evaluation = bResult ? "OK" : "NOK";
        _Data.RearDissymmetryOfCamber.ScrewingDone = "";


   
        TblCarLamp pLamp = _GV._dbJob.SelectCarLamp(_GV.m_Cur_Info.AcceptNo);

        if (pLamp.AcceptNo != "")
        {
            _Data.LeftHeadlampVertical.After = pLamp.LeftYVal.ToString("F1");
            _Data.LeftHeadlampVertical.Before = pLamp.LeftYVal_F.ToString("F1");
            _Data.LeftHeadlampVertical.Evaluation = pLamp.Left_Res;

            _Data.RightHeadlampVertical.After = pLamp.RightYVal.ToString("F1");
            _Data.RightHeadlampVertical.Before = pLamp.RightYVal_F.ToString("F1");
            _Data.RightHeadlampVertical.Evaluation = pLamp.Left_Res;
        }
        _Data.GlobalEvaluationWAProcess = _result.WAT___PK;
        _Data.GlobalEvaluationHLAProcess = pLamp.HLT___PK;


		//_Data.ResultatPEV = _GV._VEP_Data.SynchroZone.GetSyncroValue(1) == 2 ? "OK" : "NOK";
		_Data.ResultatPEV = _GV.vep.vepData.SYData[1] == 2 ? "OK" : "NOK";

		return _Data;
    }
    

  
    private void FinishTest()
    {

        _GV.plcWrite.ClearData();
        _GV.m_Cur_Info.Clear();
        _GV.m_Cur_Model.Clear();
        OnCycleFinished?.Invoke();
        SetState(Constants.STEP_WAIT);

        StopSWASending(); // Task 중단
        StopSWAChecking();
        StopTAChecking();


    }

    public  void StopTest()
    {
        _GV.plcWrite.ClearData();
        _GV.m_bTestRun = false;
        m_bSendSWA = false;
        StopSWASending(); // Task 중단
        StopSWAChecking();
        StopTAChecking();
        OnCycleFinished?.Invoke();
        SetState(Constants.STEP_WAIT);
        m_bTestStop = true;
        UI_Update_Status("Test Stop!!");
    }

}
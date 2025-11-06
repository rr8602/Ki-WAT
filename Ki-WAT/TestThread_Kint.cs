// ... existing code ...
using Ki_WAT;
using NModbus;
using System;
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
    public bool m_bPassNext  = false ;

    public bool m_bSendSWA = false;
    public bool m_bTestStop = false;

    //TblCarModel m_Cur_Model = new TblCarModel();
    //TblCarInfo m_Cur_CarInfo = new TblCarInfo();
    MeasureData measureData = new MeasureData();
    MeasureData InitMeasureData = new MeasureData();
    private Task _swaSendTask;
    private CancellationTokenSource _swaCancellationTokenSource;

    private Task _swaCheckTask;
    private CancellationTokenSource _swaCheckCancellationTokenSource;


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
            measureData = data.Clone();
            
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
                else if (m_nState == Constants.STEP_PEV_START)
                {

                    if (_GV.g_Substitu_PEV)
                    {
                        SetState(Constants.STEP_MOVE_WHEELBASE);
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
                else if (m_nState == Constants.STEP_MOVE_WHEELBASE)
                {
                    DoMoveWheelbase();
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
                else if (m_nState == Constants.STEP_CHECK_SWB_HOME)
                {
                    Do_CheckSWB_HOME();
                }
                else if (m_nState == Constants.STEP_CHECK_HLA_HOME)
                {
                    Do_CheckHLA_HOME();
                }
                else if ( m_nState == Constants.STEP_CHECK_VEP_FINISH)
                {
                    Do_Check_VEP_Finish();
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
                else if (m_nState == Constants.STEP_CHECK_GO_OUT2)
                {
                    Do_CheckGoOut2();
                }
                else if (m_nState == Constants.STEP_CHECK_GO_OUT3)
                {
                    Do_CheckGoOut3();
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
        measureData.Clear();
        DppManager.SendToDpp(DppManager.MSG_DPP_WHEELBASE, Int32.Parse(_GV.m_Cur_Model.WhelBase));
        Thread.Sleep(300);
        DppManager.SendToDpp(DppManager.MSG_DPP_VEHICLE_TYPE, Int32.Parse(_GV.m_Cur_Model.Dpp_Code));
        _GV.Log_PGM.WriteFile("Start Test : " + _GV.m_Cur_Info.CarPJINo);
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
            SetState(Constants.STEP_PEV_START);
        }
        catch (Exception ex)
        {
            WLog("DoWait Error: " + ex.Message);
        }
    }
    private void Do_PEV_Start()
    {
        try
        {
            // NEED PEV. Start cycle set Value 1;
            _GV._VEP_Client.SetAllSyncZero();
            _GV._VEP_Client.SetStartCycle();
            // NEED PLC. 여기서 휠베이스 이동 명령어도 전송;
           // _GV._PLCVal.DO._Wheelbase_Start = true;
            UI_Update_Status("STEP_PEV_START");

            while (true)
            {
                if (CheckLoopExit()) break;
                Thread.Sleep(10);
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
            _GV._VEP_Data.SendPJI(_GV.m_Cur_Info.CarPJINo);
            UI_Update_Status("STEP_PEV_SEND_PJI");
            //_GV._VEP_Data.TransmissionZone.ExchStatus = 2;
            const double TIMEOUT_SECONDS = 0.5;
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                Thread.Sleep(10);
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
                Thread.Sleep(10);
                if (_GV._VEP_Data.SynchroZone.GetSyncroValue(0) == 1 ) break;
            }
            _GV._VEP_Client.SetSync30NewVehicle();
            testThread_HLT.StartThread();
            SetState(Constants.STEP_MOVE_WHEELBASE);
        }
        catch (Exception ex)
        {
            WLog("Do_PEV_SendPJI Error: " + ex.Message);
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

            _GV.plcWrite.SetDiv(ushort.Parse(_GV.m_Cur_Model.WB___Div));
            _GV.plcWrite.SetCodeOK(true);
            //_GV.plcWrite.setPJI(_GV.m_Cur_Info.CarPJINo);

            while (true)
            {
                if (CheckLoopExit()) break;
                
                int WBDist = Convert.ToInt32(_GV.m_Cur_Model.WhelBase);
                if ( _GV.plcRead.GetLeftDistance() == WBDist)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            //_GV._PLCVal.DO._Wheelbase_Start = false;
            Thread.Sleep(100);

            SetState(Constants.STEP_DETECT_CAR_WAIT);
            // 입구 신호등 GREEN;
            //_GV._PLCVal.DO.

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        return nRet;
    }
    private int DoDetectCar()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_DETECT_CAR_WAIT");
            while (true)
            {
                if (CheckLoopExit()) break;

                if (_GV.plcRead.IsFrontCarDetection() && _GV.plcRead.IsRearCarDetection())
                {
                    break;
                }

                Thread.Sleep(10);
            }
            Thread.Sleep(100);
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

                Thread.Sleep(10);
            }

            
            //_GV._PLCVal.DO._Wheelbase_Start = false;
            OnCycleStart?.Invoke();
            Thread.Sleep(100);
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
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_START);
            UI_Update_Status("RUN-OUT_0");

            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 100;
            while (true)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                {
                    break;
                }

                if (CheckLoopExit()) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_RUNOUT_POS_1);
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
            const double TIMEOUT_SECONDS = 100;
            
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_RUNOUT_POS_2);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoPresRunOut_1:  {ex.Message}");
        }

        
      
        Thread.Sleep(100);
        return nRet;
    }
    private int DoPresRunOut_2()
    {
        int nRet = 0;
        try
        {
            

            UI_Update_Status("RUN-OUT_2");
            DateTime startTime = DateTime.Now;
            const int TIMEOUT_SECONDS = 100;

            while (true)
            {
                if (CheckLoopExit()) break;
                //if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                if ( _GV.g_MeasureData.dSymm < 0.5 && _GV.g_MeasureData.dSymm > -0.5)
                {
                    _GV.plcWrite.SetAccurancyCheck(true);
                    break;
                }
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
            if (_GV.g_Substitu_SWB)
            {
                SetState(Constants.STEP_CHECK_CENTERING);
                return nRet;
            }
            UI_Update_Status("Handle_0");
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if (!_GV.plcRead.IsSteeringWheelBalanceHome() && Math.Abs(_GV.dHandle) > 3.0 ) break;

                Thread.Sleep(10);
            }

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


            while (true)
            {
                if (CheckLoopExit()) break;
                if ( _GV.dHandle > 1)
                {
                    break;
                }

                Thread.Sleep(10);
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

            bool bRoller_Spain = false;
            bool bCenteringOK = false;
            while (true)
            {
                if (CheckLoopExit()) break;
                
                if ( bRoller_Spain && bCenteringOK 
                     && Math.Abs(_GV.g_MeasureData.dSymm) < 0.5)
                {
                    break;
                }

                
                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoCheckCentering:  {ex.Message}");
        }
        
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
            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsMeasureStart())
                {
                    break;
                }
                Thread.Sleep(10);
            }

            InitMeasureData.Clear();
            SetState(Constants.STEP_WAIT_RUNOUT);



            // Measuring start 눌렀을때 여기서 실행

            StartSWAChecking();
            

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

                ushort isSWB = _GV._VEP_Data.SynchroZone.GetSyncroValue(8);
                
                if ( isSWB  == 1 )
                {
                    int nA = 100;
                    int nB = 0;
                    ushort usData = (ushort)(nA * _GV.dHandle + nB);
                    _GV._VEP_Client.SetSync07SWBAngle(usData);
                }
                // 전송 간격 (예: 100ms)
                await Task.Delay(1000, cancellationToken);

                if (_GV._VEP_Data.SynchroZone.GetSyncroValue(9) == 1 ||
                     _GV._VEP_Data.SynchroZone.GetSyncroValue(9) == 2)
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

    private async Task SendSWACheck(CancellationToken cancellationToken)
    {
        try
        {
            while (m_bSendSWA && !cancellationToken.IsCancellationRequested)
            {

                if (_GV._VEP_Data.SynchroZone.GetSyncroValue(20) == 3)
                {
                    StopSWAChecking();
                }
               if (_GV._VEP_Data.SynchroZone.GetSyncroValue(20) == 1)
                {
                    SendFinalSWB();
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
            const double TIMEOUT_SECONDS = 12;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                Thread.Sleep(10);
            }

            // RUNOUT 이 끝나고 
            _GV._VEP_Client.SetSync06RunOutFinish();
            m_bSendSWA = true;
            StartSWASending();
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
                if ( !_GV.g_Substitu_ScrewDriver_L )
                    main.ScrewDriverL.Send("asdf");
            }
            if (main != null && main.ScrewDriverR != null)
            {                
                if (!_GV.g_Substitu_ScrewDriver_R)
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
            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                if (m_SrewLeftOK) break;
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
                
                if (_GV.plcRead.IsPitInLeftFinish()) break;
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
                if (_GV.plcRead.IsPitInRightFinish()) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_PRESS_PIT_OUT_FINISH_WAIT);
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_STOP, 0);
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
        ushort Offset = _GV._VEP_Data.SynchroZone.GetSyncroValue(12);
        int nA = 0;
        int nB = 0;
        int nC = 0;
        ushort usData = GetABFromOffset(Offset, nA, nB, nC, _GV.g_MeasureData.dTA);

        if ( usData != 65535 )
        {
            usData = (ushort)_rand.Next(0, 1001);
            _GV._VEP_Client.SetSync10TAReady();
            _GV._VEP_Client.SetSync11TAAngle(usData);
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
            _GV._VEP_Client.SetSync04SWB_Imform();
            _GV._VEP_Client.SetSync07SWBAngle(usData);
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
            SetState(Constants.STEP_DISPLAY_RESULT);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Press_PitOut_Finish_wait:  {ex.Message}");
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
            const double TIMEOUT_SECONDS = 3;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                Thread.Sleep(10);
            }
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

                if (m_HLA_Finish && m_PEV_Finish)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            SetState(Constants.STEP_CHECK_SWB_HOME);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckAllFinish:  {ex.Message}");
        }
        return nRet;
    }

    private int Do_CheckSWB_HOME()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_CheckSWB_HOME");
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsSteeringWheelBalanceHome()) break;
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_CHECK_HLA_HOME);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckSWB_HOME:  {ex.Message}");
        }
        return nRet;
    }

    private int Do_CheckHLA_HOME()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_CheckHLA_HOME");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsHLAHomePosition()) break;
                Thread.Sleep(10);
            }
            _GV._VEP_Client.SetSync32HLAFinish();
            SetState(Constants.STEP_CHECK_VEP_FINISH);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckHLA_HOME:  {ex.Message}");
        }

        return nRet;
    }

    private int Do_Check_VEP_Finish()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_Exit_Position");
           

            while (true)
            {
                if (CheckLoopExit()) break;

                if ( _GV._VEP_Data.SynchroZone.GetSyncroValue(1) == 2 ||
                    _GV._VEP_Data.SynchroZone.GetSyncroValue(1) == 3
                    )
                {
                    break;
                }
            }
            SetState(Constants.STEP_EXIT_POSITION);
            //피트 신호등 RED
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Exit_Position:  {ex.Message}");
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
                //if ( 모든 조건들이 참일때) break;
                Thread.Sleep(10);
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
   private void MakeResult()
    {
        DateTime startTime = DateTime.Now;
        _result.AcceptNo = _GV.m_Cur_Info.AcceptNo;
        _result.PJI_Num  = _GV.m_Cur_Info.CarPJINo;
        _result.Model_NM = _GV.m_Cur_Model.Model_NM;

        _result.CarFLToe = measureData.dToeFL.ToString("F1");
        _result.CarFRToe = measureData.dToeFR.ToString("F1");
        _result.CarFTToe = (measureData.dToeFL + measureData.dToeFR).ToString("F1");
        _result.CarRLToe = measureData.dToeRL.ToString("F1");
        _result.CarRRToe = measureData.dToeRR.ToString("F1");
        _result.CarRTToe = (measureData.dToeRL + measureData.dToeRR).ToString("F1");
        _result.CarFLCam = measureData.dCamFL.ToString("F1");
        _result.CarFRCam = measureData.dCamFR.ToString("F1");
        _result.CarFCros = (measureData.dCamFL - measureData.dCamFR).ToString("F1");
        _result.CarRLCam = measureData.dCamRL.ToString("F1");
        _result.CarRRCam = measureData.dCamRR.ToString("F1");
        _result.CarRCros = (measureData.dCamRL - measureData.dCamRR).ToString("F1");
        _result.Car_Hand = _GV.dHandle.ToString("F1");
        _result.CarDogRu = measureData.dTA.ToString("F1");
        _result.Car_Symm = measureData.dSymm.ToString("F1");
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
    private int Do_SaveData()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_SaveData");
            DateTime startTime = DateTime.Now;
            //퇴출 신호등 GREEN;
            MakeResult();
            while (true)
            {

                if (m_HLA_Finish && _GV.g_Substitu_HLT )
                {
                    break;
                }
                //if (CheckLoopExit()) break;

                //Thread.Sleep(10);

                break;
            }
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

            while (true)
            {
                if (CheckLoopExit()) break;
                //if (_GV._PLCVal.DI._HLA_Home_position) break;
                //DB Write.
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_CHECK_GO_OUT1);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
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
                if ( !_GV.plcRead.IsFrontCarDetection() &&
                     !_GV.plcRead.IsRearCarDetection()) break;
               
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_CHECK_GO_OUT2);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        
        return nRet;

    }

    private int Do_CheckGoOut2()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_CHECK_GO_OUT2");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV.plcRead.IsFrontCarDetection()) break;
                //DB Write.
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_CHECK_GO_OUT3);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        
        return nRet;

    }

    private int Do_CheckGoOut3()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_CHECK_GO_OUT3");
            DateTime startTime = DateTime.Now;
            while (true)
            {
                if (CheckLoopExit()) break;
                if (!_GV.plcRead.IsFrontCarDetection()) break;
                
                
                Thread.Sleep(10);
            }
            SetState(Constants.STEP_GRET_OK);

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
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
            if ( bOK )
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

    private void FinishTest()
    {
        _GV.m_Cur_Info.Clear();
        _GV.m_Cur_Model.Clear();
        OnCycleFinished?.Invoke();
        SetState(Constants.STEP_WAIT);
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
    public  void StopTest()
    {
        _GV.m_bTestRun = false;
        m_bSendSWA = false;
        StopSWASending(); // Task 중단
        OnCycleFinished?.Invoke();
        SetState(Constants.STEP_WAIT);
        m_bTestStop = true;
        UI_Update_Status("Test Stop!!");
    }

}
// ... existing code ...
using Ki_WAT;
using System.Collections.Generic;
using System.Threading;
using System.Windows.Forms;
using System;
using System.Xml.Linq;

internal class TestThread_Kint
{
    private Thread testThread = null;
    GlobalVal _GV;
    public GPLog testLog = new GPLog();

    private bool m_bRun = false;
    private bool m_bExitStep = false;
    private int m_nState = 0;
    private bool m_bBarcodeRead = false ;
    private readonly object _lock = new object();
    private TestThread_HLT testThread_HLT;
    
    public bool m_SrewLeftOK = false;
    public bool m_SrewRightOK = false;
    public bool m_HLA_Finish = false;
    public bool m_PEV_Finish = false;

    public bool m_bPassNext  = false ;

    TblCarModel m_Cur_Model = new TblCarModel();
    TblCarInfo m_Cur_CarInfo = new TblCarInfo();

    

    [System.Runtime.InteropServices.DllImport("user32.dll")]
    private static extern short GetAsyncKeyState(int vKey);
    private bool IsShiftEnterPressed()
    {
        // Shift + F4 조합으로 변경
        // VK_SHIFT = 0x10, VK_F4 = 0x73

        return (GetAsyncKeyState(0x10) & 0x8000) != 0 &&
               (GetAsyncKeyState(0x73) & 0x8000) != 0;
    }

    // UI 업데이트를 위한 이벤트들
    public static event Action<string> OnStatusUpdate;
    public static event Action<MeasureData> OnDataReceived;
    public static event Action<string> OnErrorOccurred;
    public TestThread_Kint(GlobalVal gv)
    {
        _GV = gv;
        testThread_HLT = new TestThread_HLT(gv);
        
        testLog.SetName("Test Thread");
    }

    private void WLog(String strLog)
    {
        testLog.WriteLog(strLog);
        GWA.STM(strLog);
    }

    public int StartThread(TblCarInfo pInfo, TblCarModel pModel )
    {
        try
        {
            if (m_bRun) return 0;

            if (testThread != null)
            {
                StopThread();
                Thread.Sleep(1000);
            }

            m_Cur_Model = pModel;
            m_Cur_CarInfo = pInfo;

            
            m_bRun = true;
            m_bExitStep = false;

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
    
    public void StopThread()
    {
        try
        {
            lock (_lock)
            {
                m_bRun = false;
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
            OnStatusUpdate?.Invoke(Message);
        }
        catch (Exception ex)
        {
            WLog("UI_Update_Status Error: " + ex.Message);
        }
    }

    public bool BarcodeReading()
    {
        m_bBarcodeRead = true;
        return m_bBarcodeRead;
    }

    private bool CheckLoopExit()
    {
        return !m_bRun || m_bExitStep || IsShiftEnterPressed() || m_bPassNext;
    }

    private void TestWAB(Object obj)
    {
        try
        {
            UI_Update_Status("ABC");
            while (true)
            {
                Thread.Sleep(100);

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
                else if ( m_nState == Constants.STEP_PEV_SEND_PJI)
                {
                    Do_PEV_SendPJI();
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
                else if (m_nState == Constants.STEP_CHECK_CENTERING)
                {
                    DoHandle_0();
                }


                else if (m_nState == Constants.STEP_FINISH)
                {
                    DoFinish();
                    break;
                }

            }

            m_bBarcodeRead = false;
        }
        catch (Exception ex)
        {
            WLog("TEST THREAD Exception : " + ex.Message);
        }

        UI_Update_Status("Finish Thread");

    }

    public void SetState(int state)
    {
        try
        {
            m_nState = state;
            m_bPassNext = false;
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
    private void DoWait()
    {
        try
        {
            while (true)
            {
                if (CheckLoopExit()) 
                    break;

                

                Thread.Sleep(10);
            }

            


            SetState(Constants.STEP_PEV_START);
        }
        catch (Exception ex)
        {
            WLog("DoWait Error: " + ex.Message);
        }
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
            // NEED PLC. 여기서 휠베이스 이동 명령어도 전송;
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
            //m_Cur_CarInfo.CarPJINo
            //SendPJI ;
            UI_Update_Status("STEP_PEV_SEND_PJI");
            while (true)
            {
                if (CheckLoopExit()) break;
                Thread.Sleep(10);
            }

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
            UI_Update_Status(StringDef.WBMove);
            _GV._PLCVal.DO._Wheelbase_Start = true;
            while (true)
            {
                if (CheckLoopExit()) break;
                if ( _GV._PLCVal.DI._Wheelbase_In_Position)
                {
                    break;
                }
                Thread.Sleep(100);
            }
            _GV._PLCVal.DO._Wheelbase_Start = false;

            // 입구 신호등 GREEN;
            //_GV._PLCVal.DO.

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        m_bExitStep = false;
        Thread.Sleep(100);

        SetState(Constants.STEP_DETECT_CAR_WAIT);
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
                if (_GV._PLCVal.DI._Front_Car_Detection_sensor && _GV._PLCVal.DI._Rear_Car_Detection_sensor)
                {
                    break;
                }

                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        m_bExitStep = false;
        Thread.Sleep(100);
        SetState(Constants.STEP_PRESS_STARTCYCLE_WAIT);
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
                if (_GV._PLCVal.DI._Operator_Start_Cycle)
                {
                    break;
                }
                Thread.Sleep(10);
            }

            _GV._PLCVal.DO._Wheelbase_Start = false;

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }


        // 헤드라이트 시작 
        testThread_HLT.StartThread(m_Cur_CarInfo, m_Cur_Model);
        m_bExitStep = false;
        Thread.Sleep(100);
        SetState(Constants.STEP_RUNOUT_POS_0);
        return nRet;
    }

    private int DoPresRunOut_0()
    {
        int nRet = 0;
        try
        {
            _GV._PLCVal.DO._Safety_Roller_Up = true;
            _GV._PLCVal.DO._Safety_Roller_Down = false;

            _GV._PLCVal.DO._RollerBrake_Free = true;
            _GV._PLCVal.DO._RollerBrake_Lock = false;

            _GV._PLCVal.DO._Floating_plate_Free = true;
            _GV._PLCVal.DO._Floating_plate_Lock = false;



            UI_Update_Status("RUN-OUT_0");

            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 0.5;
            while (true)
            {
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS)
                {
                    break;
                }

                if (CheckLoopExit()) break;
                


                Thread.Sleep(10);
            }

            
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($" DoPresRunOut_0 : {ex.Message}");
        }

        m_bExitStep = false;
        SetState(Constants.STEP_RUNOUT_POS_1);
        Thread.Sleep(100);
        return nRet;
    }

    private int DoPresRunOut_1()
    {
        int nRet = 0;
        try
        {
            _GV._PLCVal.DO._Centering_ON = true;
            _GV._PLCVal.DO._Centering_Home = false;

            UI_Update_Status("RUN-OUT_1");

            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 0.5;
            
            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;
                Thread.Sleep(10);
            }

            
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoPresRunOut_1:  {ex.Message}");
        }

        m_bExitStep = false;
        SetState(Constants.STEP_RUNOUT_POS_2);
        Thread.Sleep(100);
        return nRet;
    }
    private int DoPresRunOut_2()
    {
        int nRet = 0;
        try
        {
            _GV._PLCVal.DO._Camera_sensor_Enable = true;
            _GV._PLCVal.DO._Motor_Run = true;
            _GV._PLCVal.DO._Motor_Stop = false;

            // 여기서 visicon sensor ON ;

            UI_Update_Status("RUN-OUT_2");
            DateTime startTime = DateTime.Now;
            const int TIMEOUT_SECONDS = 3;

            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                Thread.Sleep(10);
            }   
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoPresRunOut_2:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_HANDLE_0);
        Thread.Sleep(100);
        return nRet;
    }

    private int DoHandle_0()
    {
        int nRet = 0;
        try
        {
            if (_GV.g_Substitu_HLT)
            {
                SetState(Constants.STEP_CHECK_CENTERING);
                return nRet;
            }

            UI_Update_Status("Handle_0");
            DateTime startTime = DateTime.Now;

            while (true)
            {
                if (CheckLoopExit()) break;
                if (!_GV._PLCVal.DI._Steering_wheel_balance_Home) break;

                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoHandle_0:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_HANDLE_1);
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
                if ( _GV.g_MeasureData.dHandle > 1)
                {
                    break;
                }

                Thread.Sleep(10);
            }
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"DoHandle_0:  {ex.Message}");
        }
        m_bExitStep = false;
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
        m_bExitStep = false;
        SetState(Constants.STEP_PRESS_MEASURE_WAIT);
        Thread.Sleep(100);
        return nRet;
    }

    private int Do_PresstheMeasureButton_Wait()
    {
        int nRet = 0;
        try
        {

            UI_Update_Status("Do_PresstheMeasureButton_Wait");
            
            while (true)
            {
                if (CheckLoopExit()) break;

                if (_GV._PLCVal.DI._Operator_Mearuing_start)
                {
                    break;
                }
                Thread.Sleep(10);
            }


            if ( !_GV.g_Substitu_MovingFlate )
            {
                _GV._PLCVal.DO._Moving_plate_Home = false;
                _GV._PLCVal.DO._Moving_plate_ON = true;
            }
            // Measuring start 눌렀을때
            
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PresstheMeasureButton_Wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_WAIT_RUNOUT);
        Thread.Sleep(100);
        return nRet;
    }

    private int Do_WaitRunOutTime()
    {
        int nRet = 0;
        try
        {

            UI_Update_Status("Do_WaitRunOutTime");

            DateTime startTime = DateTime.Now;
            const double TIMEOUT_SECONDS = 12;

            while (true)
            {
                if (CheckLoopExit()) break;
                if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                Thread.Sleep(10);
            }

            // RUNOUT 이 끝나고 
            _GV._PLCVal.DO._Motor_Run = false;
            _GV._PLCVal.DO._Motor_Stop = true;
            
            // 피트 신호등 GREEN;
            //_GV._PLCVal.DO.
            
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PresstheMeasureButton_Wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_SCREW_SEND_INFO);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_FINISH);
        Thread.Sleep(100);
        return nRet;
    }

    private int Do_SrewLeftTighten()
    {
        int nRet = 0;
        try
        {

            UI_Update_Status("Do_SrewLeftTighten");



            var main = _GV._frmMNG.GetForm<Frm_Mainfrm>();
            if (main != null && main.ScrewDriverL != null)
            {
                if (!_GV.g_Substitu_ScrewDriver_L)
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

      

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_SCREW_GET_DATA_LEFT);
        Thread.Sleep(100);
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



        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_PRESS_LEFT_FINISH);
        Thread.Sleep(100);
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
                if (_GV._PLCVal.DI._OperatorPit_Left_Finish) break;
                Thread.Sleep(10);
            }

            

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressLeftFinish_Wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_SCREW_GET_DATA_RIGHT);
        Thread.Sleep(100);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SrewSendInfo:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_PRESS_RIGHT_FINISH);
        Thread.Sleep(100);
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
                if (_GV._PLCVal.DI._OperatorPit_Right_Finish) break;
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressRightFinish_Wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_PRESS_PIT_OUT_FINISH_WAIT);
        Thread.Sleep(100);
        return nRet;
    }


    private int Do_PressPitOut_Finish()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_PressPitOut_Finish");
            DateTime startTime = DateTime.Now;
            //const double TIMEOUT_SECONDS = 0.5;

            while (true)
            {
                if (CheckLoopExit()) break;
                // if ((DateTime.Now - startTime).TotalSeconds >= TIMEOUT_SECONDS) break;

                if (_GV._PLCVal.DI._Screw_driver_L_HOME &&
                    _GV._PLCVal.DI._Screw_driver_R_HOME &&
                    _GV._PLCVal.DI._UnWrench_Tool_Home &&
                    _GV._PLCVal.DI._OperatorExit_Finish) 
                    
                    break;
                
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressPitOut_Finish:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_SREW_ALL_HOME);
        Thread.Sleep(100);
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

                if (_GV._PLCVal.DI._Screw_driver_L_HOME &&
                    _GV._PLCVal.DI._Screw_driver_R_HOME &&
                    _GV._PLCVal.DI._UnWrench_Tool_Home 
                    )
                    break;

                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_PressPitOut_Finish:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_PRESS_PIT_OUT_FINISH_WAIT);
        Thread.Sleep(100);
        return nRet;
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

                if (_GV._PLCVal.DI._OperatorExit_Finish)
                    break;

                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Press_PitOut_Finish_wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_DISPLAY_RESULT);
        Thread.Sleep(100);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Press_PitOut_Finish_wait:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_ALL_FINISH);
        Thread.Sleep(100);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckAllFinish:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_SWB_HOME);
        Thread.Sleep(100);
        return nRet;

    }
    private int Do_Exit_Position()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_Exit_Position");
            DateTime startTime = DateTime.Now;

            _GV._PLCVal.DO._Moving_plate_Home = true;
            _GV._PLCVal.DO._Moving_plate_ON = false;

            _GV._PLCVal.DO._Safety_Roller_Down = true;
            _GV._PLCVal.DO._Safety_Roller_Up = false;

            _GV._PLCVal.DO._Centering_Home = true;
            _GV._PLCVal.DO._Centering_ON = false;

            _GV._PLCVal.DO._Camera_sensor_Enable = false;

            _GV._PLCVal.DO._Floating_plate_Lock = true;
            _GV._PLCVal.DO._Floating_plate_Free = false;

            _GV._PLCVal.DO._RollerBrake_Lock = true;
            _GV._PLCVal.DO._RollerBrake_Free = false;

            

            while (true)
            {
                if (CheckLoopExit()) break;

                //if ( 모든 조건들이 참일때) break;

                Thread.Sleep(10);
            }

            //피트 신호등 RED
            //퇴출 신호등 GREEN;

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_Exit_Position:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_SWB_HOME);
        Thread.Sleep(100);
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
                if (_GV._PLCVal.DI._Steering_wheel_balance_Home) break;
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckSWB_HOME:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_HLA_HOME);
        Thread.Sleep(100);
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
                if (_GV._PLCVal.DI._HLA_Home_position) break;
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_CheckHLA_HOME:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_SAVE_DATA);
        Thread.Sleep(100);
        return nRet;

    }
    private int Do_SaveData()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("Do_SaveData");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                //if (_GV._PLCVal.DI._HLA_Home_position) break;
                //DB Write.
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_TICKET_PRINT);
        Thread.Sleep(100);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_GO_OUT1);
        Thread.Sleep(100);
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
                if ( !_GV._PLCVal.DI._Front_Car_Detection_sensor &&
                     !_GV._PLCVal.DI._Rear_Car_Detection_sensor) break;
                //DB Write.
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_GO_OUT2);
        Thread.Sleep(100);
        return nRet;

    }

    private int Do_CheckGoOut2()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_CHECK_GO_OUT1");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV._PLCVal.DI._Front_Car_Detection_sensor) break;
                //DB Write.
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_CHECK_GO_OUT3);
        Thread.Sleep(100);
        return nRet;

    }

    private int Do_CheckGoOut3()
    {
        int nRet = 0;
        try
        {
            UI_Update_Status("STEP_CHECK_GO_OUT1");
            DateTime startTime = DateTime.Now;


            while (true)
            {
                if (CheckLoopExit()) break;
                if (_GV._PLCVal.DI._Front_Car_Detection_sensor) break;
                //DB Write.
                Thread.Sleep(10);
            }

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_SaveData:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_GRET_OK);
        Thread.Sleep(100);
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

        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"Do_GretComm:  {ex.Message}");
        }
        m_bExitStep = false;
        SetState(Constants.STEP_FINISH);
        Thread.Sleep(100);
        return nRet;

    }

    


}

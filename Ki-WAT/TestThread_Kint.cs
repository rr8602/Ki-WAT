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
            m_Cur_Model = pModel;
            m_Cur_CarInfo = pInfo;

            if (testThread != null)
            {
                StopThread();
                Thread.Sleep(1000);
            }
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
        return !m_bRun || m_bExitStep || IsShiftEnterPressed();
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

                else if (m_nState == Constants.STEP_FINISH)
                {
                    break;
                }

            }

            m_bBarcodeRead = false;
        }
        catch (Exception ex)
        {
            WLog("TEST THREAD Exception : " + ex.Message);
        }
    }

    public void SetState(int state)
    {
        try
        {
            m_nState = state;

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
            else if ( m_nState == Constants.STEP_RUNOUT_POS)
            {
                _GV._PLCVal.DO._Safety_Roller_Up = true;
                UI_Update_Status(StringDef.Runout);
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

        m_bExitStep = false;
        Thread.Sleep(100);
        SetState(Constants.STEP_FINISH);
        return nRet;
    }

    private int DoPresRunout()
    {
        int nRet = 0;
        try
        {
            _GV._PLCVal.DO._Centering_ON = true;
            _GV._PLCVal.DO._Camera_sensor_Enable = true;
            _GV._PLCVal.DO._Floating_plate_Free = true;
            
            //_GV._PLCVal.DO._RollerBrakeFree = true;

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
            UI_Update_Status(StringDef.WBMoveEnd);
        }
        catch (Exception ex)
        {
            OnErrorOccurred?.Invoke($"WB Move Error :  {ex.Message}");
        }

        m_bExitStep = false;
        Thread.Sleep(100);
        //SetState(Constants.STEP_PC_START);
        return nRet;
    }


}

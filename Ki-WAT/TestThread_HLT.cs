using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_WAT
{
    internal class TestThread_HLT
    {
        private Thread testThread = null;
        GlobalVal _GV;
        //public GPLog testLog = new GPLog();

        private bool m_bRun = false;
        private bool m_bExitStep = false;
        private int m_nState = 0;
        private bool m_bBarcodeRead = false;
        private readonly object _lock = new object();
        private bool m_bStartCycle = false;

        //TblCarModel m_Cur_Model = new TblCarModel();
        //TblCarInfo m_Cur_CarInfo = new TblCarInfo();
        LET_Param m_LetParam = new LET_Param();

        public static event Action<string> OnStatusUpdate;
        public static event Action OnCycleFinished;

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public TestThread_HLT(GlobalVal gv)
        {
            _GV = gv;
            Reset();
            SetLetParam();
            TestThread_Kint.OnCycleStart += OnStartCycle;
        }
        ~TestThread_HLT()
        {
            StopThread();
            TestThread_Kint.OnCycleStart -= OnStartCycle;
        }

        private void OnStartCycle()
        {
            m_bStartCycle = true;
        }

        private void SetLetParam()
        {
            m_LetParam.line = "1";
            m_LetParam.uid = _GV.m_Cur_Info.AcceptNo + "_" + _GV.m_Cur_Info.CarPJINo;
            m_LetParam.vin = _GV.m_Cur_Info.CarPJINo;
            m_LetParam.vsn = Convert.ToInt32(_GV.m_Cur_Info.LetCycle);
            m_LetParam.floorpitch = 1;
        }
        private void Reset()
        {
            m_bRun = false;
            m_bExitStep = false;
            m_nState = Constants.STEP_WAIT;
            m_bBarcodeRead = false;
            m_LetParam.Clear();
            _GV.m_bHLTFinish = false;
            m_bStartCycle = false;
        }

        private bool IsShiftEnterPressed()
        {
            // Shift + F4 조합으로 변경
            // VK_SHIFT = 0x10, VK_F5 = 0x74
            return (GetAsyncKeyState(0x10) & 0x8000) != 0 &&
                   (GetAsyncKeyState(0x72) & 0x8000) != 0;
        }
        

        private bool CheckLoopExit()
        {
            bool bb = IsShiftEnterPressed();
            Debug.Print("");
            return !m_bRun || m_bExitStep || IsShiftEnterPressed();
        }

        public int StartThread()
        {
            try
            {
                if (_GV.g_Substitu_HLT) return 0;
                
                if (testThread != null)
                {
                    StopThread();
                    Thread.Sleep(1000);
                }
                m_bRun = true;
                m_bExitStep = false;
                SetLetParam();
                testThread = new Thread(TestHLT);
                testThread.IsBackground = true;

                SetState(Constants.STEP_HLT_START);
                testThread.Start(this);
                
                

                return 1;
            }
            catch (Exception ex)
            {
                WLog("StartThread Error: " + ex.Message);
                return -1;
            }
        }
        private void WLog(String strLog)
        {
            
            GWA.STM(strLog);
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

        public void SetState(int state)
        {
            try
            {
                
                m_nState = state;


                if (m_nState == Constants.STEP_HLT_START)
                {
                    UI_Update_Status("HLT STEP_WAIT");
                }
                else if (m_nState == Constants.STEP_HLT_VEHICLE_SELECT)
                {
                    UI_Update_Status("HLT STEP_HLT_VEHICLE_SELECT");
                 
                }
                else if (m_nState == Constants.STEP_HLT_PERFORM_TEST)
                {
                    UI_Update_Status("HLT STEP_HLT_PERFORM_TEST");
                }
                else if (m_nState == Constants.STEP_HLT_END_CYCLE)
                {
                    UI_Update_Status("HLT STEP_HLT_END_CYCLE");
                }
                else if (m_nState == Constants.STEP_HLT_GET_RESULT)
                {
                    UI_Update_Status("HLT STEP_HLT_GET_RESULT");
                }
                else if (m_nState == Constants.STEP_HLT_DELETE_ALL)
                {
                    UI_Update_Status("HLT STEP_HLT_DELETE_ALL");
                }
                else if (m_nState == Constants.STEP_HLT_6)
                {
                    Debug.Print("STEP_HLT_6");
                    OnStatusUpdate?.Invoke("HLT STEP_HLT_6");   
                }
                else if (m_nState == Constants.STEP_HLT_7)
                {
                }

            }
            catch (Exception ex)
            {
                WLog("SetState Error: " + ex.Message);
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

        private void TestHLT(Object obj)
        {
            try
            {
                UI_Update_Status("ABC");
                while (true)
                {
                    if ( _GV.m_bTestRun == false)
                    {
                        WLog("TEST Thread Stop by m_bTestRun false");
                        break;
                    }
                    Thread.Sleep(100);
                    if (m_nState == Constants.STEP_HLT_START)
                    {
                        Do_HLT_Start();
                    }
                    else if (m_nState == Constants.STEP_HLT_VEHICLE_SELECT)
                    {
                        Do_HLT_VEHICLE_SELECT();
                    }
                    else if (m_nState == Constants.STEP_HLT_PERFORM_TEST)
                    {
                        Do_HLT_PERFORM_TEST();
                    }
                    else if (m_nState == Constants.STEP_HLT_END_CYCLE)
                    {
                        Do_HLT_END_CYCLE();
                    }
                    else if (m_nState == Constants.STEP_HLT_GET_RESULT)
                    {
                        Do_HLT_GET_RESULT();
                    }
                    else if (m_nState == Constants.STEP_HLT_DELETE_ALL)
                    {
                        Do_HLT_DeleteAllTesk();
                    }
                    else if (m_nState == Constants.STEP_HLT_6)
                    {
                        break;
                    }
                    else if (m_nState == Constants.STEP_HLT_7)
                    {
                        break;
                    }
                }

            }
            catch (Exception ex)
            {
                WLog("TEST THREAD Exception : " + ex.Message);
            }
        }

        private void Do_HLT_Start()
        {
            try
            {
                bool bRes = _GV.LET_Controller.StartCycle(m_LetParam.uid);

                if (bRes == false)
                {
                    WLog("LET Start Cycle Fail");
                    m_bRun = false;
                    return;
                }
                SetState(Constants.STEP_HLT_VEHICLE_SELECT);

                
            }
            catch (Exception ex)
            {
                
            }
        }
        private void Do_HLT_VEHICLE_SELECT()
        {
            try
            {
                bool bRes = _GV.LET_Controller.VehicleSelection(m_LetParam.vsn, m_LetParam.vin);

                if ( bRes == false)
                {
                    WLog("LET Vehicle Selection Fail");
                    m_bRun = false;
                    return;
                }
                SetState(Constants.STEP_HLT_PERFORM_TEST);
            }
            catch (Exception ex)
            {
            }
        }

        private void Do_HLT_PERFORM_TEST()
        {
            try
            {
                //PLC 에서 LET 가출발 됬을때
                if (!_GV._PLCVal.DI._HLA_Home_position && m_bStartCycle)
                {
                    bool bRes = _GV.LET_Controller.PerformTests(m_LetParam.line, m_LetParam.floorpitch);
                    if (bRes == false)
                    {
                        WLog("LET Vehicle Selection Fail");
                        m_bRun = false;
                        return;
                    }
                    SetState(Constants.STEP_HLT_END_CYCLE);
                }
            }
            catch (Exception ex)
            {

            }
        }

        private void Do_HLT_END_CYCLE()
        {
            try
            {
                if (_GV._PLCVal.DI._HLA_Home_position)
                {
                    bool bRes = _GV.LET_Controller.EndCycle();
                    if ( bRes == false) 
                    {
                        WLog("LET ControllerEndCycle Fail");
                        m_bRun = false;
                        return;
                    }

                    _GV._VEP_Client.SetSync32HLAFinish();
                    SetState(Constants.STEP_HLT_GET_RESULT);
                }
            }
            catch (Exception ex)
            {
            }
        }
        // Save Data
        private void Do_HLT_GET_RESULT()
        {
            try
            {
                if (_GV._PLCVal.DI._HLA_Home_position)
                {
                    string strXML = _GV.LET_Controller.GetResult(m_LetParam.uid);
                    SaveXmlToFile(strXML);

                    OnCycleFinished?.Invoke();
                    SetState(Constants.STEP_HLT_DELETE_ALL);
                }
            }
            catch (Exception ex)
            {

            }
        }
        private void Do_HLT_DeleteAllTesk()
        {
            try
            {
                _GV.LET_Controller.DeleteAllTest();

                _GV.m_bHLTFinish = true;
                SetState(Constants.STEP_HLT_6);
                
            }
            catch (Exception ex)
            {
            }
        }
        private void SaveHLTData(string sXML)
        {
            LampInclination ResData = _GV.LET_Controller.ParseInclinationFromXml(sXML);
            TblCarLamp tblCarLamp = new TblCarLamp();

            tblCarLamp.AcceptNo = ResData.Vehicle_UID;
            tblCarLamp.CarPjiNo = ResData.Vehicle_VIN;
            tblCarLamp.HTstTime = ResData.Vehicle_Test_start;
            tblCarLamp.HEndTime = ResData.Vehicle_Test_end;
            tblCarLamp.Model_NM = ResData.Equipment_Model;
            tblCarLamp.HLT___PK = ResData.Vehicle_Overall_result;
            tblCarLamp.LampKind = "L";
            tblCarLamp.LeftXVal = ResData.Low_InclinationXFinal_Left;
            tblCarLamp.LeftYVal = ResData.Low_InclinationYFinal_Left;
            tblCarLamp.Left_Res = ResData.Low_Left_Result;
            tblCarLamp.RightXVal = ResData.Low_InclinationXFinal_Right;
            tblCarLamp.RightYVal = ResData.Low_InclinationYFinal_Right;
            tblCarLamp.Right_Res = ResData.Low_Right_Result;
            
            _GV._dbJob.InsertCarLamp(tblCarLamp);
        }
        private void SaveXmlToFile(string sXML)
        {

            SaveHLTData(sXML);
            string basePath = _GV.Config.Program.RESULT_PATH; ;
            string strPath = Path.Combine(
                basePath,
                DateTime.Now.Year.ToString("D4"),
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2")
            );
            // 폴더가 없으면 생성
            if (!Directory.Exists(strPath))
            {
                Directory.CreateDirectory(strPath);
            }

            // 저장할 파일 경로 지정
            string filePath = Path.Combine(strPath, _GV.m_Cur_Info.CarPJINo + ".xml");
            try
            {
                File.WriteAllText(filePath, sXML);
            }
            catch (Exception ex)
            {
                // 예외 처리 필요 시 여기에 추가
            }
        }


    }
}

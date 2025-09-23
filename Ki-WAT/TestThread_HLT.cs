using System;
using System.Collections.Generic;
using System.Diagnostics;
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
        public GPLog testLog = new GPLog();

        private bool m_bRun = false;
        private bool m_bExitStep = false;
        private int m_nState = 0;
        private bool m_bBarcodeRead = false;
        private readonly object _lock = new object();

        //TblCarModel m_Cur_Model = new TblCarModel();
        //TblCarInfo m_Cur_CarInfo = new TblCarInfo();

        [System.Runtime.InteropServices.DllImport("user32.dll")]
        private static extern short GetAsyncKeyState(int vKey);

        public TestThread_HLT(GlobalVal gv)
        {
            _GV = gv;
            testLog.SetName("Test Thread");
        }

        private bool IsShiftEnterPressed()
        {
            // Shift + F4 조합으로 변경
            // VK_SHIFT = 0x10, VK_F5 = 0x74
            return (GetAsyncKeyState(0x10) & 0x8000) != 0 &&
                   (GetAsyncKeyState(0x72) & 0x8000) != 0;
        }
        public static event Action<string> OnStatusUpdate;

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

                testThread = new Thread(TestHLT);
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
        private void WLog(String strLog)
        {
            testLog.WriteLog(strLog);
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
                    Thread.Sleep(100);
                    if (m_nState == Constants.STEP_WAIT)
                    {
                        Do_HLT_Start();
                    }
                    else if (m_nState == Constants.STEP_HLT_1)
                    {
                        Do_HLT_STEP1();
                    }
                    else if (m_nState == Constants.STEP_HLT_2)
                    {
                        Do_HLT_STEP2();
                    }
                    else if (m_nState == Constants.STEP_HLT_3)
                    {
                        Do_HLT_STEP3();
                    }
                    else if (m_nState == Constants.STEP_HLT_4)
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
                while (true)
                {
                    if (CheckLoopExit())
                        break;
                    Thread.Sleep(100);
                    GWA.STM("HLT_START");
                }
                SetState(Constants.STEP_HLT_1);

                
            }
            catch (Exception ex)
            {
                
            }
        }
        private void Do_HLT_STEP1()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;
                    GWA.STM("STEP1");
                    Thread.Sleep(100);
                }
                SetState(Constants.STEP_HLT_2);


            }
            catch (Exception ex)
            {
            }
        }

        private void Do_HLT_STEP2()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;
                    GWA.STM("STEP2");
                    Thread.Sleep(100);
                }

                SetState(Constants.STEP_HLT_3);
            }
            catch (Exception ex)
            {

            }
        }

        private void Do_HLT_STEP3()
        {
            try
            {
                while (true)
                {
                    if (CheckLoopExit())
                        break;
                    GWA.STM("STEP3");
                    Thread.Sleep(300);
                }

                SetState(Constants.STEP_HLT_4);
            }
            catch (Exception ex)
            {

            }
        }


    }
}

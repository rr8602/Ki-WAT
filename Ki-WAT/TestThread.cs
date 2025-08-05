using DG_ComLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    internal class TestThread : IDisposable
    {
        public GPLog testLog = new GPLog();


        private readonly object _lock = new object();

        private Thread testThread = null;

        private bool m_bRun = false;
        private bool m_bExitStep = false;
        private int m_nStep = 0;



        public TestThread()
        {
            testLog.SetName("Test Thread");
        }

        public void Dispose()
        {

        }

        private void WLog(String strLog)
        {
            testLog.WriteLog(strLog);
            GWA.STM(strLog);
        }

        public void StopThread()
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
        
        public void AddJsno()
        {

        }
        public void TestStart(string Barcode)
        {
            
        }

        public int StartThread()
        {

            if (testThread != null)
            {
                StopThread();
            }
            m_bRun = true;
            m_bExitStep = false;
            testThread = new Thread(TestRun);
            testThread.Start();
            return 1;
        }


        private void SetStep(int nStep)
        {
            m_nStep = nStep;
            if (m_nStep == 100)
            {

            }
            if (m_nStep == 200)
            {
                
            }

        }

        private int _DoTest()
        {
            int nRet = -1;


            //Thread.Sleep(1000);
            ////Progress 초기화를 위해서 사용된 Scop
           
            //WLog("Start Do Test");
            ////WLog(TData.GetDBModel().strModelName);

            //SetStep(0);
            //while (true)
            //{
            //    if (m_nStep == 100)
            //    {
            //        if (m_bRun) _100_Ready();
            //        _Ti.InterfaceProc(Constants.INTERFACE_DATA, "READY DEVICE", "");
            //    }

            //    Thread.Sleep(1000);
            //    _Ti.InterfaceProc(Constants.INTERFACE_DATA, "START TEST", "");
            //    if (m_bRun) _200_DoStart();

            //    Thread.Sleep(1000);
            //    _Ti.InterfaceProc(Constants.INTERFACE_DATA, "START GENBANC", "");
            //    if (m_bRun) _300_DoGenbanc();

            //    Thread.Sleep(1000);
            //    _Ti.InterfaceProc(Constants.INTERFACE_DATA, "END TEST", "");
            //    if (m_bRun) _400_DoFinishi();

            //}



            return nRet;
        }

        private void TestRun()
        {
            //TestData TData = obj as TestData;
            try
            {
                int nRet = _DoTest();
            }
            catch (Exception ex)
            {
                WLog("TEST THREAD Exception : " + ex.Message);
            }
            finally
            {
                WLog("STOP TEST THREAD");

            }


        }



    }
}

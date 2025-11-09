using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;

namespace Ki_WAT
{
    static class Constants
    {
        public const int SubViewWidth = 1850 - 100;
        public const int SubViewHeight = 950 - 50;

        public const int WorkerViewWidth = 1920;
        public const int WorkerViewHeight = 1024;

        public const int WM_COPYDATA = 0x4A;
        public const int WM_USER = 0x0400;
        public const int COPY_MSG_DATA = WM_USER + 101;
        public const int COPY_MSG_TAKEPIC = WM_USER + 1010;

        public const int WGT_KG = WM_USER + 3001;
        public const int CLOSE_WINDOWS = WM_USER + 9999;

        ////////////////////////////// INTERFACE CMD ////////////////////////////////////////////////
        //인터페이스에서 사용되는 MSG 종류
        public const int INTERFACE_DATA = 1001;
        public const int INTERFACE_CMD = 1002;
        public const int INTERFACE_WRITE = 1003;

        public const int BOT_MESSAGE = 2001;
        public const int STA_MESSAGE = 2002;
        public const int PRO_MESSAGE = 2003;

        public const int ERR_MESSAGE = 3000;

        /////////////////////////////////////////////////////////////////////////////////////////////

        public const int STEP_WAIT = 0;

        public const int STEP_BARCODE_READ = 1;
        public const int STEP_MOVE_WHEELBASE = 2;
        public const int STEP_MOVE_WHEELBASE_END = 3;

        public const int STEP_PEV_START = 110;
        public const int STEP_PEV_SEND_PJI = 111;
        public const int STEP_PEV_MDA_OK = 112;


        public const int STEP_DETECT_CAR_WAIT = 120;
        public const int STEP_PRESS_STARTCYCLE_WAIT = 121;

        public const int STEP_RUNOUT_POS_0 = 130;
        public const int STEP_RUNOUT_POS_1 = 131;
        public const int STEP_RUNOUT_POS_2 = 132;

        public const int STEP_HANDLE_0 = 140;
        public const int STEP_HANDLE_1 = 141;

        public const int STEP_CHECK_CENTERING = 142;
        public const int STEP_PRESS_MEASURE_WAIT = 143;


        public const int STEP_WAIT_RUNOUT = 150; //
        public const int STEP_SCREW_SEND_INFO = 160;
        public const int STEP_SCREW_GET_DATA_LEFT = 161;
        public const int STEP_SCREW_GET_DATA_RIGHT = 162;

        public const int STEP_PRESS_LEFT_FINISH = 163;
        public const int STEP_PRESS_RIGHT_FINISH = 164;
        public const int STEP_CHECK_SREW_ALL_HOME = 165;
        public const int STEP_PRESS_PIT_OUT_FINISH_WAIT = 166;

        public const int STEP_DISPLAY_RESULT = 170;

        public const int STEP_CHECK_ALL_FINISH = 171;
        public const int STEP_CHECK_SWB_HOME = 172;
        public const int STEP_CHECK_HLA_HOME = 173;
        public const int STEP_CHECK_VEP_FINISH = 174;
        public const int STEP_EXIT_POSITION = 175;
        


        public const int STEP_SAVE_DATA = 180;
        public const int STEP_TICKET_PRINT = 181;

        public const int STEP_CHECK_GO_OUT1 = 182;
        public const int STEP_CHECK_GO_OUT2 = 183;
        public const int STEP_CHECK_GO_OUT3 = 184;

        public const int STEP_GRET_OK = 190;
        public const int STEP_FINISH = 9999;

        /// <summary>
        /// HLT 관련 define
        /// 
        /// </summary>
        
        public const int STEP_HLT_START = 500;
        public const int STEP_HLT_VEHICLE_SELECT = 501;
        public const int STEP_HLT_PERFORM_TEST = 502;
        public const int STEP_HLT_END_CYCLE = 503;
        public const int STEP_HLT_GET_RESULT = 504;
        public const int STEP_HLT_DELETE_ALL = 505;
        public const int STEP_HLT_6 = 506;
        public const int STEP_HLT_7 = 507;


    }

    internal static class Program
    {
        // 중복 실행 방지를 위한 Mutex
        private static Mutex mutex = null;
        private const string MUTEX_NAME = "Ki_WAT_SingleInstance_Mutex";

        

        /// <summary>
        /// 해당 애플리케이션의 주 진입점입니다.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // 중복 실행 방지
            if (!IsSingleInstance())
            {
                MessageBox.Show("Ki-WAT Already running", "Warning", 
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            try
            {
                // 전역 설정 초기화 (GlobalVal 생성자에서 자동으로 초기화됨)
                // GlobalVal.Instance는 Lazy<T>로 자동 초기화됩니다


                
                Application.EnableVisualStyles();
                Application.SetCompatibleTextRenderingDefault(false);
                Application.Run(new Frm_Mainfrm());
            }
            finally
            {
                // 프로그램 종료 시 Mutex 해제
                ReleaseMutex();
            }
        }

        /// <summary>
        /// 단일 인스턴스 실행을 확인합니다.
        /// </summary>
        /// <returns>true: 단일 인스턴스, false: 이미 실행 중</returns>
        private static bool IsSingleInstance()
        {
            try
            {
                // Mutex 생성 시도
                mutex = new Mutex(true, MUTEX_NAME, out bool createdNew);
                
                if (createdNew)
                {
                    // 새로 생성된 경우 (첫 번째 실행)
                    return true;
                }
                else
                {
                    // 이미 존재하는 경우 (중복 실행)
                    return false;
                }
            }
            catch (Exception ex)
            {
                // Mutex 생성 중 오류 발생 시 로그 기록 (선택사항)
                // MessageBox.Show($"Mutex 생성 중 오류: {ex.Message}");
                return false;
            }
        }

        /// <summary>
        /// Mutex를 해제합니다.
        /// </summary>
        private static void ReleaseMutex()
        {
            try
            {
                if (mutex != null)
                {
                    mutex.ReleaseMutex();
                    mutex.Dispose();
                    mutex = null;
                }
            }
            catch (Exception ex)
            {
                // Mutex 해제 중 오류 발생 시 로그 기록 (선택사항)
                // MessageBox.Show($"Mutex 해제 중 오류: {ex.Message}");
            }
        }
    }
}

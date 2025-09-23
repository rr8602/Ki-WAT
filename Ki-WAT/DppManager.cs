using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{
    static class DppManager
    {
        private struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            [MarshalAs(UnmanagedType.LPStr)]
            public string lpData;
        }
        public const int MSG_DPP_VEHICLE_TYPE = 9000;
        public const int MSG_DPP_WHEELBASE = 9100;
        public const int MSG_DPP_MEASURING_START = 9200;
        public const int MSG_DPP_MEASURING_STOP = 9300;
        public const int MSG_DPP_CALIBRATION = 9400;
        public const int MSG_DPP_VERITY = 9500;
        [DllImport("user32")]
        private static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
        [DllImport("user32")]
        private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, ref COPYDATASTRUCT lParam);
        public  static void SendToDpp(int nType, int nValue = 0)
        {
            string str;
            if (nType == 0 || nType == 1)
            {
                str = $"<CMD/{nType}/{nValue}>";
            }
            else
            {
                str = $"<CMD/{nType}/>";
            }
            Send(str);
        }
        public static void Send(string strMsg)
        {
            COPYDATASTRUCT cds;
            IntPtr hWnd = FindWindow(null, "DppSensor");
            if (hWnd != IntPtr.Zero)
            {
                cds.dwData = (IntPtr)Constants.COPY_MSG_DATA;
                cds.cbData = strMsg.Length + 1;
                cds.lpData = strMsg;
                SendMessage(hWnd, Constants.WM_COPYDATA, 0, ref cds);
            }
        }
    }
}

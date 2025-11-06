using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing;


namespace Ki_WAT
{

	static class GWA
	{

		public struct COPYDATASTRUCT
		{
			public IntPtr dwData;
			public int cbData;
			[MarshalAs(UnmanagedType.LPStr)]
			public string lpData;
		}
		public struct RECT
		{
			public int nLeft ;
			public int nTop ;
			public int nRight	;	
			public int nBottom;	
		}
		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		private static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, uint wParam, ref COPYDATASTRUCT lParam);
		[DllImport("user32")]
		public static extern IntPtr FindWindow(String lpClassName, String lpWindowName);
		[DllImport("user32.dll")]
		public static extern IntPtr FindWindowEx(IntPtr hWnd1, int Hwnd2, string lpClassName, string lpWindowName);
		[DllImport("user32.dll")]
		private static extern bool MoveWindow(IntPtr hWnd, int x, int y, uint nWidth, uint nHeight, bool bRepaint);

		[DllImport("user32.dll", CharSet = CharSet.Auto)]
		public static extern IntPtr SendMessage(IntPtr hWnd, uint Msg, int wParam, int lParam);
		[DllImport("user32")]
		public static extern int GetClientRect(IntPtr hWnd, ref RECT lpRect);
		public static String strName = "";



        public static bool GetBit(byte byData, int nBitPos)
        {
            if (nBitPos < 0 || nBitPos > 7)
                return false;


            return (byData & (1 << nBitPos)) != 0;
        }

        public static void STM(String strMsg)
        {
            COPYDATASTRUCT cds;
            IntPtr hWnd = FindWindow(null, "MessageView");
            if (hWnd != IntPtr.Zero)
            {
                cds.dwData = (IntPtr)Constants.COPY_MSG_DATA;
                cds.cbData = strMsg.Length + 1;
                cds.lpData = strMsg;
                SendMessage(hWnd, Constants.WM_COPYDATA, 0, ref cds);
            }
        }

        public static void TakePicture(String strMsg)
		{
			COPYDATASTRUCT cds;
			IntPtr hWnd = FindWindow(null, "MessageView");
			if (hWnd != IntPtr.Zero)
			{
				cds.dwData = (IntPtr)Constants.COPY_MSG_TAKEPIC;
				cds.cbData = strMsg.Length + 1;
				cds.lpData = strMsg;
				SendMessage(hWnd, Constants.WM_COPYDATA, 0, ref cds);
			}
		}

        public static void STM2(String strMsg)
        {
            COPYDATASTRUCT cds;
			//strMsg = "";
            IntPtr hWnd = FindWindow(null, "LogViewer");
            if (hWnd != IntPtr.Zero)
            {
                cds.dwData = (IntPtr)Constants.COPY_MSG_DATA;
                cds.cbData = strMsg.Length + 1;
                cds.lpData = strMsg;
                SendMessage(hWnd, Constants.WM_COPYDATA, 0, ref cds);
            }
        }

        public static void CaptureToPictureBox(PictureBox pictureBox, int x, int y, int width, int height)
        {
            // 캡처할 영역 크기만큼 비트맵 생성
            Bitmap bmp = new Bitmap(width, height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                // 스크린의 지정된 위치에서 bmp로 복사
                g.CopyFromScreen(x, y, 0, 0, new Size(width, height));
            }

            // PictureBox에 표시
            if (pictureBox.Image != null)
            {
                pictureBox.Image.Dispose(); // 기존 이미지 해제
            }
            pictureBox.Image = bmp;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage; // 자동 맞춤
        }
        public static void CaptureFormToPictureBox(PictureBox pictureBox, Form form)
        {
            Rectangle bounds = form.Bounds; // 폼의 스크린 좌표와 크기 가져오기
            Bitmap bmp = new Bitmap(bounds.Width, bounds.Height);

            using (Graphics g = Graphics.FromImage(bmp))
            {
                g.CopyFromScreen(bounds.X, bounds.Y, 0, 0, bounds.Size);
            }

            if (pictureBox.Image != null)
                pictureBox.Image.Dispose();

            pictureBox.Image = bmp;
            pictureBox.SizeMode = PictureBoxSizeMode.StretchImage;
        }


        public static void MW(IntPtr hWnd, int X, int Y, uint nWidth, uint nHeight, bool bRepaint = true)
		{
			MoveWindow(hWnd, X, Y, nWidth, nHeight, bRepaint);
		}
		//public static void SendMsg(IntPtr hWnd, uint Msg, uint wParam, uint lParam)
		//{
		//	SendMessage(hWnd, Msg, wParam, lParam);
		//}
	}
}

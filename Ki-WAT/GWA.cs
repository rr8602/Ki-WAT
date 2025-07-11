using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

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

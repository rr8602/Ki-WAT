using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace RollTester
{
	internal class GPLog
	{
		private String m_strName = "";
		private String m_strLogPath = "";
		private String m_strLogFileName = "";
		private object wlock = new object();

		public void SetName(String strName)
		{
			m_strName=strName;
			m_strLogFileName = strName + ".log";
		}
		public void SetNewDate()
		{
			m_strLogPath = "";
		}
		public void WriteLog(String strLog)
		{
			if(m_strName.Equals(""))
			{
				m_strName = "PGM";
				m_strLogFileName = m_strName + ".log";
			}
			if(m_strLogPath.Equals(""))
			{
				m_strLogPath = GetLogPath();
			}
			WriteFile(strLog);

		}

		private String GetLogPath()
		{
			
			DateTime dt = DateTime.Now;
			int nYear = dt.Year;
			int nMonth = dt.Month;	
			int nDay = dt.Day;

			String strYear = nYear.ToString();
			String strMonth = nMonth.ToString();
			String strDay = nDay.ToString();

			String strPath = Application.StartupPath + "\\LOG";

			//LOG 폴더를 확인합니다.
			{
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
				if (!di.Exists) { di.Create(); }
			}

			//년도 폴더를 확인합니다.
			strPath = Application.StartupPath + "\\LOG\\" + strYear;
			{
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
				if (!di.Exists) { di.Create(); }
			}

			//월 폴더를 확인합니다.
			strPath = Application.StartupPath + "\\LOG\\" + strYear + "\\" + strMonth;
			{
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
				if (!di.Exists) { di.Create(); }
			}

			//일 폴더를 확인합니다.
			strPath = Application.StartupPath + "\\LOG\\" + strYear + "\\" + strMonth +  "\\" + strDay;
			{
				System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
				if (!di.Exists) { di.Create(); }
			}

			return strPath;
		}

		private bool WriteFile(String strLog)
		{
			String strTime = "";
			DateTime now = DateTime.Now;
			strTime = now.ToString("HH:mm:ss.fff");
			bool bRet = false;
			lock(wlock)
			{
				StreamWriter writer;
				String strFullPath = m_strLogPath + "\\" + m_strLogFileName;
				if(strFullPath.Equals(""))
				{
					bRet = false;
				}
				else
				{
					writer = File.AppendText(strFullPath);
					writer.WriteLine(strTime + " : " +strLog);
					writer.Close();
					bRet = true;
				}
			}

			return bRet;
		}

	}
}

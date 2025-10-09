using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
	public class GPLog
	{
		private String m_strName = "";
		private String m_strLogPath = "";
		private String m_strLogFileName = "";
		private object wlock = new object();

		public void SetName(String strName)
		{
			m_strName=strName;
			m_strLogPath = GetLogPath();
		}
		
        private string GetLogPath()
        {
            DateTime now = DateTime.Now;

            string logPath = Path.Combine(
                Application.StartupPath,
                "LOG",
                "PGM",
                now.Year.ToString(),
                now.Month.ToString("D2"),
                now.Day.ToString("D2")
            );

            Directory.CreateDirectory(logPath);

            return logPath;
        }

        public bool WriteFile(String strLog)
		{
			String strTime = "";
			DateTime now = DateTime.Now;
			strTime = now.ToString("HH:mm:ss.fff");
			bool bRet = false;
			lock(wlock)
			{
				StreamWriter writer;
				String strFullPath = m_strLogPath + "\\" + m_strName + ".log";
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

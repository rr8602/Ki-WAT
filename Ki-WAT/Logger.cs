using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LETInterface
{
    public class Logger
    {
        private string filename;
        private Queue<string> logQueue = new Queue<string>();
        private readonly object writeLock = new object();


        public  void SetFileName(string strPJI)
        {
            string sFolder = CreateFolder();
            filename = sFolder + "\\" + strPJI + ".log";
            Debug.Print(filename);
        }

        private  String CreateFolder()
        {

            DateTime dt = DateTime.Now;
            int nYear = dt.Year;
            int nMonth = dt.Month;
            int nDay = dt.Day;

            String strYear = nYear.ToString();
            String strMonth = nMonth.ToString();
            String strDay = nDay.ToString();

            String strPath = Application.StartupPath + "\\LOG\\LET";

            //LOG 폴더를 확인합니다.
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
                if (!di.Exists) { di.Create(); }
            }

            //년도 폴더를 확인합니다.
            strPath = Application.StartupPath + "\\LOG\\LET\\" + strYear;
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
                if (!di.Exists) { di.Create(); }
            }

            //월 폴더를 확인합니다.
            strPath = Application.StartupPath + "\\LOG\\LET\\" + strYear + "\\" + strMonth;
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
                if (!di.Exists) { di.Create(); }
            }

            //일 폴더를 확인합니다.
            strPath = Application.StartupPath + "\\LOG\\LET\\" + strYear + "\\" + strMonth + "\\" + strDay;
            {
                System.IO.DirectoryInfo di = new System.IO.DirectoryInfo(strPath);
                if (!di.Exists) { di.Create(); }
            }

            return strPath;
        }

        //private static string GetNewLogFilename()
        //{

            
        //    var logDirectory = AppDomain.CurrentDomain.BaseDirectory;
        //    var date = DateTime.Now.ToString("yyyyMMdd");
        //    var sequence = 1;
        //    while (true)
        //    {
        //        var filename = Path.Combine(logDirectory, $"{date}_{sequence:D4}.log");
        //        if (!File.Exists(filename))
        //        {
        //            return filename;
        //        }
        //        sequence++;
        //    }


        //}
        public  void WriteLog(string str, bool bwritenow = false)
        {
            Debug.WriteLine(str);
            logQueue.Enqueue(str);
            if (bwritenow == true)
            {
                lock (writeLock)
                {
                    using (StreamWriter fw = new StreamWriter(filename, true))
                    {
                        while (logQueue.Count > 0)
                        {
                            string logEntry = logQueue.Dequeue();
                            fw.WriteLine(logEntry);
                        }
                    }
                }
            }
        }

        public  string HttpMessageToLogString(object message)
        {
            var sb = new StringBuilder();

            try
            {
                if (message is HttpRequestMessage request)
                {
                    sb.AppendLine("A new connection...\n");
                    sb.AppendLine("client -> server :");

                    // Request Line
                    sb.AppendLine($"{request.Method} {request.RequestUri.PathAndQuery} HTTP/{request.Version}");

                    // Host Header
                    sb.AppendLine($"Host: {request.RequestUri.Host}{(request.RequestUri.Port != 80 && request.RequestUri.Port != 443 ? ":" + request.RequestUri.Port : "")}");

                    // 기타 Header
                    foreach (var header in request.Headers)
                    {
                        sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }

                    if (request.Content != null)
                    {
                        foreach (var header in request.Content.Headers)
                        {
                            sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }
                        var content = request.Content.ReadAsStringAsync().Result;

                        if (!string.IsNullOrEmpty(content))
                        {
                            sb.AppendLine();
                            sb.AppendLine(content);
                        }

                        var mediaType = request.Content.Headers.ContentType?.MediaType;

                        if (mediaType != null)
                        {
                            request.Content = new StringContent(content, Encoding.UTF8, mediaType);
                        }
                        else
                        {
                            request.Content = new StringContent(content, Encoding.UTF8);
                        }
                    }
                }
                else if (message is HttpResponseMessage response)
                {
                    sb.AppendLine("server -> client :");
                    // Status Line
                    sb.AppendLine($"HTTP/{response.Version} {(int)response.StatusCode} {response.ReasonPhrase}");
                    // Header
                    foreach (var header in response.Headers)
                    {
                        sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                    }
                    if (response.Content != null)
                    {
                        foreach (var header in response.Content.Headers)
                        {
                            sb.AppendLine($"{header.Key}: {string.Join(", ", header.Value)}");
                        }
                        var content = response.Content.ReadAsStringAsync().Result;
                        if (!string.IsNullOrEmpty(content))
                        {
                            sb.AppendLine();
                            sb.AppendLine(content);
                        }
                        var mediaType = response.Content.Headers.ContentType?.MediaType;

                        if (mediaType != null)
                        {
                            response.Content = new StringContent(content, Encoding.UTF8, mediaType);
                        }
                        else
                        {
                            response.Content = new StringContent(content, Encoding.UTF8);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                sb.AppendLine($"Error Log: {ex.Message}");
            }

            return sb.ToString();
        }
    }
}

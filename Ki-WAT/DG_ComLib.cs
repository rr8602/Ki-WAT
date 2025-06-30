using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;


namespace DG_ComLib
{

    public class DG_Application
    {
        [DllImport("user32.dll")]
        private static extern IntPtr FindWindow(string SClassName, string SWindowName);

        [DllImport("user32.dll")]
        private static extern bool SetForegroundWindow(IntPtr findname);

        [DllImport("user32.dll")]
        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

        [DllImport("user32.dll")]
        private static extern bool ShowWindowAsync(IntPtr findname, int howShow);
        private const int showNORMAL = 1;
        private const int showMINIMIZED = 2;
        private const int showMAXIMIZED = 3;

        public static bool check_exist_same_pgm()
        {
            // 중복 실행 방지
            Process[] proc = Process.GetProcessesByName(Application.ProductName);
            //매개변수 프로세스 이름
            //반환값 : 지정한 응용 프로그램 또는 파일을 실행 중인 프로세스 리소스를 나타내는 System.Diagnostics.Process 형식의 배열입니다.

            if (proc.Length <= 1)
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public static bool check_same_pgm(string ProgramName)
        {
            Process[] proc = Process.GetProcessesByName(ProgramName);//ProgramName과 같은 이름의 프로세스 불러오는배열
            if (proc.Length < 1)
            {
                return false;
            }
            else
            {
                IntPtr findname = FindWindow(null, "조회 프로그램");//프로그램명(작업관리자에나오는)으로 핸들을 찾음
                //ShowWindowAsync(findname, showMAXIMIZED);//프로그램이 최소화 되어있다면 활성화 시킴
                ShowWindow(findname, 3);//프로그램이 최소화 되어있다면 활성화 시킴(3 : 최대화)
                SetForegroundWindow(findname); //윈도우에 포커스를 줘서 최상위로 끌어올림
                return true;
            }
        }
    }

    public class DG_Log
    {
        public enum LogType { Job, Sys, Parsing }

        public static bool bDebugView = true;

        public static bool isJobLog = true;
        public static bool isParsingLog = false; //메모장에서 불러오게도 만들 수 있다

        private static readonly string sParsingLogDirectory = Application.StartupPath + "\\LOG\\PARSING\\";
        private static readonly string sJobLogDirectory = Application.StartupPath + "\\LOG\\JOB\\";
        //        private static readonly string sSystemLogDirectory = Application.StartupPath + "\\LOG\\SYSTEM\\";
        private static readonly string sSystemLogDirectory = Application.StartupPath + "\\LOG\\";

        public static string sParsingLog = "PARSING";
        public static string sJobLog = "JOB";
        public static string sSysLog = "SYSTEM";        // always 기록


        public static object object_FileLock = new object();



        public DG_Log()
        {
            //
            // TODO: 여기에 생성자 논리를 추가합니다.
            //
        }

        public static void MakeLogFolder()     // 프로그램 실행 관련 폴더들 생성
        {
            //Directory.CreateDirectory(sParsingLogDirectory);
            //Directory.CreateDirectory(sJobLogDirectory);
            Directory.CreateDirectory(sSystemLogDirectory);
        }

        // 일정기간 이전의 Log 파일은 삭제한다.


        public static void DeleteLogFile()
        {
            try
            {
                DateTime dateDelete = DateTime.Now.AddDays(-10);
                string sDateDelete = dateDelete.ToString("yyyy-MM-dd");

                //// Job Process Log
                //if (Directory.Exists(sJobLogDirectory))
                //{
                //    foreach (string fileName in Directory.GetFiles(sJobLogDirectory))
                //    {
                //        string sFileDate = fileName.Replace(sJobLogDirectory, "").Replace(".txt", "");

                //        if (string.Compare(sFileDate, sDateDelete) < 0)
                //        {
                //            File.Delete(fileName);
                //        }
                //    }
                //}

                //if (Directory.Exists(sParsingLogDirectory))
                //{
                //    foreach (string fileName in Directory.GetFiles(sParsingLogDirectory))
                //    {
                //        string sFileDate = fileName.Replace(sParsingLogDirectory, "").Replace(".txt", "");

                //        if (string.Compare(sFileDate, sDateDelete) < 0)
                //        {
                //            File.Delete(fileName);
                //        }
                //    }
                //}


                // 기타 Log
                if (Directory.Exists(sSystemLogDirectory))
                {
                    foreach (string fileName in Directory.GetFiles(sSystemLogDirectory))
                    {
                        string sFileDate = fileName.Replace(sSystemLogDirectory, "").Replace(".txt", "");

                        if (string.Compare(sFileDate, sDateDelete) < 0)
                        {
                            File.Delete(fileName);
                        }
                    }
                }
            }
            catch (Exception err)
            {
                DG_ComLib.DG_Log.SysLog("[DeleteLogFile] [{0}]", err.Message);
            }
        }

        // 기타 통신/JOB에 대한 LOG에 사용된다.
        private static void LogWrite(LogType sLogTypeDir, string logStr)
        {
            try
            {
                DateTime NowDate = DateTime.Now;

                string AppName = null;
                //string LogFileName=null;
                // log 파일을 만든다.
                string sToday = System.DateTime.Now.ToString("yyyy-MM-dd");
                //LogFileName = sToday + ".txt";
                AppName = sToday + ".txt";

                string path = null;

                if (sLogTypeDir == LogType.Job)
                {
                    if (!isJobLog) return;

                    path = sJobLogDirectory;
                }
                else if (sLogTypeDir == LogType.Parsing)
                {
                    if (!isParsingLog) return;

                    path = sParsingLogDirectory;
                }
                else
                {
                    path = sSystemLogDirectory;
                }

                // Directory 없으면 생성
                if (!System.IO.Directory.Exists(path))
                {
                    System.IO.Directory.CreateDirectory(path);
                }

                path = path + AppName;

                // File 내용 채우기
                System.IO.TextWriter logFile;

                logFile = System.IO.File.AppendText(path);

                lock (object_FileLock)
                {
                    logFile.WriteLine("[{0}][{1:HH:mm:ss}] {2}", NowDate.ToShortDateString(), NowDate, logStr);
                }

                logFile.Close();

                path = null;
            }
            catch (Exception err)
            {
                DG_ComLib.DG_Log.SysLog("[LogWrite] [{0}]", err.Message);
            }
        }

        public static void SysLog(StackTrace st, string logStr, params object[] args)
        {
            try
            {
                LogWrite(LogType.Sys, string.Format(logStr, args));

                WriteDebugData(st, string.Format(logStr, args));

                //                Console.WriteLine(string.Format(logStr, args));

            }
            catch { }
        }

        public static void SysLog(string logStr, params object[] args)
        {
            try
            {
                LogWrite(LogType.Sys, string.Format(logStr, args));

                //                Console.WriteLine(string.Format(logStr, args));
            }
            catch { }
        }

        public static void JobLog(string logStr, params object[] args)
        {
            try
            {
                LogWrite(LogType.Job, string.Format(logStr, args));

                //                Console.WriteLine(string.Format(logStr, args));

            }
            catch { }
        }

        public static void ParsingLog(string logStr, params object[] args)
        {
            try
            {
                LogWrite(LogType.Parsing, string.Format(logStr, args));
            }
            catch { }
        }

        public static void WriteDebugData(StackTrace st)
        {
            //            StackTrace st = new StackTrace(new StackFrame(true));
            Trace.WriteLine("[" + Application.ProductName + "] " + st.ToString());

            //StackFrame sf = st.GetFrame(0);

            //Trace.WriteLine(" File: " + sf.GetFileName());
            //Trace.WriteLine(" Method: " + sf.GetMethod().Name);
            //Trace.WriteLine(" Line Number: " + sf.GetFileLineNumber().ToString());
            //Trace.WriteLine(" Column Number: " + sf.GetFileColumnNumber().ToString());
        }

        public static void WriteDebugData(StackTrace st, string strLog)
        {
            if (bDebugView)
            {
                Trace.Write("[" + Application.ProductName + "] " + st.ToString());
                Trace.Write("[" + Application.ProductName + "] " + strLog);
            }
            StackFrame sf = st.GetFrame(0);

            Trace.WriteLine(" File: " + sf.GetFileName());
            Trace.WriteLine(" Method: " + sf.GetMethod().Name);
            Trace.WriteLine(" Line Number: " + sf.GetFileLineNumber().ToString());
            Trace.WriteLine(" Column Number: " + sf.GetFileColumnNumber().ToString());
        }
    }

    public class StringConverter
    {
        public StringConverter()
        {
        }


        static public byte[] ToBytes(string arg)
        {
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            return encoding.GetBytes(arg);
        }

        public static byte[] GetBytes(string hexString, out int discarded)
        {
            discarded = 0;
            string newString = "";
            char c;

            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    newString += c;
                else
                    discarded++;
            }

            if (newString.Length % 2 != 0)
            {
                discarded++;
                newString = newString.Substring(0, newString.Length - 1);
            }

            int byteLength = newString.Length / 2;
            byte[] bytes = new byte[byteLength];
            string hex;
            int j = 0;
            for (int i = 0; i < bytes.Length; i++)
            {
                hex = new String(new Char[] { newString[j], newString[j + 1] });
                bytes[i] = HexToByte(hex);
                j = j + 2;
            }
            return bytes;
        }

        private static byte HexToByte(string hex)
        {
            if (hex.Length > 2 || hex.Length <= 0)
                throw new ArgumentException("hex must be 1 or 2 characters in length");
            byte newByte = byte.Parse(hex, System.Globalization.NumberStyles.HexNumber);
            return newByte;
        }

        public static bool IsHexDigit(Char c)
        {
            int numChar;
            int numA = Convert.ToInt32('A');
            int num1 = Convert.ToInt32('0');
            c = Char.ToUpper(c);
            numChar = Convert.ToInt32(c);
            if (numChar >= numA && numChar < (numA + 6))
                return true;
            if (numChar >= num1 && numChar < (num1 + 10))
                return true;
            return false;
        }

        public static string StringToByteHex(string sOrg, int nByteLength)
        {
            byte[] bin_data = DG_ComLib.StringConverter.ToBytes(sOrg);

            string sTempToHex = DG_ComLib.StringConverter.ToHex(bin_data);

            string sHexString = sTempToHex.PadRight(nByteLength * 2, '0');

            if (sHexString.Length >= (nByteLength * 2))
                sHexString = sHexString.Substring(0, nByteLength * 2);

            return sHexString;

        }

        //private static string TwoCharOneByteTrimString(string s)
        //{
        //    if (s.Length % 2 != 0) return null;

        //    string result = null;

        //    for (int i = 0; i < s.Length; i += 2)
        //    {
        //        byte b1 = StringToByteHex(s.Substring(i, 1));
        //        byte b2 = StringToByteHex(s.Substring(i + 1, 1));

        //        b1 <<= 4;
        //        b2 &= 0x0F;

        //        byte[] b3 = new byte[1];
        //        b3[0] = (byte)(b1 | b2);

        //        if (b3[0] == 0x00) b3[0] = 0x20;

        //        result += Encoding.ASCII.GetString(b3);
        //    }

        //    if (result.Trim() == "")
        //        return "0";
        //    //				return null;

        //    return result.Trim();
        //}

        public static string ToString(byte[] bytes)
        {
            string sData = Encoding.ASCII.GetString(bytes, 0, bytes.Length);


            return sData;

            //string hexString = "";
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    hexString += bytes[i].ToString("X2");
            //}
            //return hexString;
        }

        public static string ToString2(byte bytes)
        {
            string hexString = "";

            hexString += bytes.ToString("X2");

            return hexString;
        }


        public static string ToHex2(byte bin_data)
        {
            string result = "";

            result = string.Format("{0:X2}", bin_data);

            return result;
        }

        public static string ToHex(byte[] bin_data)
        {
            string result = "";
            foreach (byte ch in bin_data)
            {
                result += string.Format("{0:X2}", ch);
            }
            return result;
        }

        public static string ToHex2(byte[] bin_data)
        {
            string result = "";
            foreach (byte ch in bin_data)
            {
                result += string.Format("{0:X2} ", ch);
            }
            return result;
        }

        public static string stringToHex(string sData)
        {
            string sHex = "";

            byte[] bytes = Encoding.Default.GetBytes(sData);

            foreach (byte bData in bytes)
            {
                sHex += string.Format("{0:X2}", bData);
            }

            return sHex;
        }


        public static string ToHexOneByte(byte[] bin_data)
        {
            string result = "";

            result += string.Format("{0:X2}", bin_data[0]);

            return result;
        }

        public static string ToHexTwoByte(byte[] bin_data)
        {
            string result = "";
            result += string.Format("{0:X2}{1:X2}", bin_data[0], bin_data[1]);
            return result;
        }



        public static Int32 PLCToDec(byte[] bin_data)
        {
            Int32 nReturn = 0;

            try
            {
                nReturn = BitConverter.ToInt32(bin_data, 0);
            }
            catch
            { }

            return nReturn;
        }

        public static double PLCToDouble(byte[] bin_data)
        {
            double dReturn = 0.0;

            try
            {
                //PLC real data type => 32bit Single
                dReturn = BitConverter.ToSingle(bin_data, 0);
            }
            catch
            { }

            return dReturn;
        }

        public static string hex2binary(string hexvalue)
        {
            ///C# Convert Hexadecimal to Binary String Conversion
            string binaryval ;
            binaryval = Convert.ToString(Convert.ToInt32(hexvalue, 16), 2);
            return binaryval;
        }

        public static string hex2binarybit(byte bHexvalue)
        {
            //Convert from hexadecimal to bytes
            string bits = String.Format("{0:D8}", int.Parse(Convert.ToString(bHexvalue, 2)));
            return bits;
        }

        //Dec를 2Byte HexString으로 Return
        public static string IntStr2HexWordStr(int nIntStr)
        {
            string sHex = Convert.ToString(nIntStr, 16);
            sHex = sHex.ToUpper();

            return sHex.PadLeft(4, '0');
        }

        public static string GetStrHex2Dec(string hexString)
        {
            string result = "";

            int n;

            byte[] newbyte = GetBytes(hexString, out n);

            int Decimal1 = Convert.ToInt32(newbyte[1]) * 256;
            int Decimal2 = Convert.ToInt32(newbyte[0]);

            result = (Decimal1 + Decimal2).ToString();

            return result;
        }

        public static int TwoByteToInt(byte b1, byte b2)
        {
            int nRtnInt = 0;

            string sTmp = string.Format("{0}", b1);
            string sTmp2 = string.Format("{0}", b2);

            int nTemp1 = Int32.Parse(sTmp);
            int nTemp2 = Int32.Parse(sTmp2);
            nRtnInt = (nTemp1 * 256) + nTemp2;

            return nRtnInt;
        }

        public static int OneByteToInt(byte b1)
        {
            int nRtnInt = 0;

            string sTmp = string.Format("{0}", b1);
            nRtnInt = Int32.Parse(sTmp);

            return nRtnInt;
        }

        public string GetCheckSum(string hexString)
        {
            int n;
            byte[] newbyte = GetBytes(hexString, out n);
            int byteSum = 0;
            int Rem = 0;
            foreach (byte b in newbyte)
            {
                int Decimal = Convert.ToInt32(b);
                byteSum = byteSum + Decimal;
            }
            byteSum = Math.DivRem(byteSum, 256, out Rem);

            return Rem.ToString("x2");
        }

        public string GetConverSion(string hexString, string type)
        {
            string result = "";

            int n;

            byte[] newbyte = GetBytes(hexString, out n);

            if (type == "W")
            {
                int Decimal1 = Convert.ToInt32(newbyte[1]) * 256;
                int Decimal2 = Convert.ToInt32(newbyte[0]);

                result = (Decimal1 + Decimal2).ToString();
            }
            else if (type == "B")
            {
                int Decimal2 = Convert.ToInt32(newbyte[0]);

                result = (Decimal2.ToString());
            }
            else if (type == "b0")
            {
                int Decimal2 = Convert.ToInt32(newbyte[0]);
                int temp = 0;
                int secbit = Math.DivRem(Convert.ToInt32(Decimal2), 2, out temp);

                result = temp.ToString();
            }
            else if (type == "b1")
            {
                int Decimal2 = Convert.ToInt32(newbyte[0]);
                int temp = 0;
                int secbit = Math.DivRem(Convert.ToInt32(Decimal2 >> 1), 2, out temp);

                result = temp.ToString();
            }

            return result;
        }

        public int GetByteCount(string hexString)
        {
            int numHexChars = 0;
            char c;
            // remove all none A-F, 0-9, characters
            for (int i = 0; i < hexString.Length; i++)
            {
                c = hexString[i];
                if (IsHexDigit(c))
                    numHexChars++;
            }
            // if odd number of characters, discard last character
            if (numHexChars % 2 != 0)
            {
                numHexChars--;
            }
            return numHexChars / 2; // 2 characters per byte
        }



        public bool InHexFormat(string hexString)
        {
            bool hexFormat = true;

            foreach (char digit in hexString)
            {
                if (!IsHexDigit(digit))
                {
                    hexFormat = false;
                    break;
                }
            }
            return hexFormat;
        }

        static public string ByteToHex(byte[] bin_data)
        {
            string result = "";
            foreach (byte ch in bin_data)
            {
                //result += string.Format("{0:x2} ", ch); // 2자리의 16진수로 출력, [참고] 링크 읽어볼 것
                result += string.Format("{0:x2}", ch);
            }
            return result;
        }

        static public string ByteToHex2(byte[] bin_data)
        {
            string result = "";
            foreach (byte ch in bin_data)
            {
                //result += string.Format("{0:x2} ", ch); // 2자리의 16진수로 출력, [참고] 링크 읽어볼 것
                result += string.Format("{0:x2} ", ch);
            }
            return result;
        }
    }

    public class DG_INI
    {
        public static string sIniPath = Application.StartupPath;
        public static string sIniName = sIniPath + "\\CarSET.ini";


        public DG_INI()
        {
        }

        [DllImport("kernel32")]
        public static extern bool WritePrivateProfileString(string lpAppName, string lpKeyName, string lpString, string lpFileName);

        [DllImport("kernel32")]
        public static extern uint GetPrivateProfileInt(string lpAppName, string lpKeyName, int nDefault, string lpFileName);

        [DllImport("kernel32")]
        public static extern int GetPrivateProfileString(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName);

        public static int GetPrivateProfileStringEx(string lpAppName, string lpKeyName, string lpDefault, StringBuilder lpReturnedString, int nSize, string lpFileName)
        {
            GetPrivateProfileString(lpAppName, lpKeyName, "NONE", lpReturnedString, nSize, lpFileName);

            if (lpReturnedString.ToString() == "NONE")
            {
                WritePrivateProfileString(lpAppName, lpKeyName, lpDefault, lpFileName);
            }

            return GetPrivateProfileString(lpAppName, lpKeyName, lpDefault, lpReturnedString, nSize, lpFileName);
        }

        public static string ReadValue(string Section, string Key)
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(Section, Key, "", temp, 255, sIniName);
            return temp.ToString();
        }


        public static string ReadValue(string Section, string Key, string sDefaultValue)
        {
            StringBuilder temp = new StringBuilder(255);

            GetPrivateProfileString(Section, Key, "NONE", temp, 255, sIniName);
            if (temp.ToString() == "NONE")
            {
                WritePrivateProfileString(Section, Key, sDefaultValue, sIniName);
            }

            GetPrivateProfileString(Section, Key, "NONE", temp, 255, sIniName);
            return temp.ToString();
        }

        public static void WriteValue(string Section, string Key, string Value)
        {
            WritePrivateProfileString(Section, Key, Value, sIniName);
        }
    }



    public class DG_Util
    {


        public static long GetTotalFreeSpaceMB(string driveLetter)
        {
            long tfs = 0;

            try
            {
                DriveInfo drv = new DriveInfo(driveLetter);
                tfs = drv.TotalFreeSpace / 1024 / 1024;
            }
            catch
            {
            }
            return tfs;
        }

        public static long GetTotalFreeSpaceGB(string driveLetter)
        {
            long tfs = 0;

            try
            {
                DriveInfo drv = new DriveInfo(driveLetter);
                tfs = drv.TotalFreeSpace / 1024 / 1024000;
            }
            catch
            {
            }
            return tfs;
        }

        public static long GetAvailableSpaceRate(string driveLetter)
        {
            long tfs = 0;

            try
            {
                DriveInfo drv = new DriveInfo(driveLetter);
                tfs = (drv.TotalFreeSpace * 100) / drv.TotalSize;
            }
            catch
            {
            }
            return tfs;
        }



    }
}

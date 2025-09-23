using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;

namespace Ki_WAT
{
    /// <summary>
    /// INI 파일 처리를 위한 Win32 API
    /// </summary>
    public class IniFile
    {
        [DllImport("kernel32")]
        private static extern long WritePrivateProfileString(string section, string key, string val, string filePath);

        [DllImport("kernel32")]
        private static extern int GetPrivateProfileString(string section, string key, string def, StringBuilder retVal, int size, string filePath);

        private string filePath;

        public IniFile(string path)
        {
            filePath = path;
        }

        public void WriteValue(string section, string key, string value)
        {
            WritePrivateProfileString(section, key, value, filePath);
        }

        public string ReadValue(string section, string key, string defaultValue = "")
        {
            StringBuilder temp = new StringBuilder(255);
            GetPrivateProfileString(section, key, defaultValue, temp, 255, filePath);
            return temp.ToString();
        }
        public int ReadInteger(string section, string key, int defaultValue = 0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());

            if (int.TryParse(value, out int result))
                return result;

            return defaultValue;
        }

        public double ReadDouble(string section, string key, double defaultValue = 0.0)
        {
            string value = ReadValue(section, key, defaultValue.ToString());

            if (double.TryParse(value, out double result))
                return result;

            return defaultValue;
        }

        public bool ReadBoolean(string section, string key, bool defaultValue = false)
        {
            string value = ReadValue(section, key, defaultValue.ToString());

            if (bool.TryParse(value, out bool result))
                return result;

            return defaultValue;
        }

    }

    /// <summary>
    /// DEVICE 섹션 설정 클래스
    /// </summary>
    public class DeviceConfig
    {
        public string PLC_IP { get; set; } = "";
        public string PLC_PORT { get; set; } = "";
        public string VEP_IP { get; set; } = "";
        public string VEP_PORT { get; set; } = "";
        public string SCREW_IP { get; set; } = "";
        public string SCREW_PORT { get; set; } = "";
        public string SCREW_IP2 { get; set; } = "";
        public string SCREW_PORT2 { get; set; } = "";

        public string PRINT_IP { get; set; } = "";
        public string PRINT_PORT { get; set; } = "";
        public string BARCODE_IP { get; set; } = "";
        public string BARCODE_PORT { get; set; } = "";
        public string LET_URL { get; set; } = "";
        public string LET_PORT { get; set; } = "";
        public string SWB_PORT { get; set; } = "COM3";
        public string SWB_BAUD { get; set; } = "115200";


    }

    /// <summary>
    /// PROGRAM 섹션 설정 클래스
    /// </summary>
    public class ProgramConfig
    {
        public string RESULT_PATH { get; set; } = "";
        public string LANGUAGE { get; set; } = "0";
    }

    /// <summary>
    /// WHEELBASE 섹션 설정 클래스
    /// </summary>
    public class WheelbaseConfig
    {
        public string HOME_POS { get; set; } = "";
        public string MIN_POS { get; set; } = "";
        public string MAX_POS { get; set; } = "";
    }
    public class SWBDataConfig
    {
        public double adZeroPoint { get; set; } = 0;
        public double adPlusPoint { get; set; } = 0;
        public double adMinusPoint { get; set; } = 0;
        
        public int BoardType { get; set; } = 0; // 0: BOARD, 1: PC
    }


    /// <summary>
    /// 전체 설정을 관리하는 메인 클래스
    /// </summary>
    public class AppConfig
    {
        private IniFile iniFile;
        private string configPath;

        public DeviceConfig Device { get; set; }
        public ProgramConfig Program { get; set; }
        public WheelbaseConfig Wheelbase { get; set; }
        public SWBDataConfig SWB { get; set; }
        public AppConfig(string configPath = "System\\config.ini")
        {
            this.configPath = configPath;
            iniFile = new IniFile(configPath);
            
            Device = new DeviceConfig();
            Program = new ProgramConfig();
            Wheelbase = new WheelbaseConfig();
            SWB = new SWBDataConfig();
        }


        
        /// <summary>
        /// INI 파일에서 모든 설정을 읽어옵니다.
        /// </summary>
        public void LoadConfig()
        {
            try
            {
                // DEVICE 섹션 읽기
                Device.PLC_IP = iniFile.ReadValue("DEVICE", "PLC_IP", "");
                Device.PLC_PORT = iniFile.ReadValue("DEVICE", "PLC_PORT", "");
                Device.VEP_IP = iniFile.ReadValue("DEVICE", "VEP_IP", "");
                Device.VEP_PORT = iniFile.ReadValue("DEVICE", "VEP_PORT", "");
                Device.SCREW_IP = iniFile.ReadValue("DEVICE", "SCREW_IP", "");
                Device.SCREW_PORT = iniFile.ReadValue("DEVICE", "SCREW_PORT", "");
                Device.SCREW_IP2 = iniFile.ReadValue("DEVICE", "SCREW_IP2", "127.0.0.1");
                Device.SCREW_PORT2 = iniFile.ReadValue("DEVICE", "SCREW_PORT2", "8500");
                Device.PRINT_IP = iniFile.ReadValue("DEVICE", "PRINT_IP", "");
                Device.PRINT_PORT = iniFile.ReadValue("DEVICE", "PRINT_PORT", "");
                Device.BARCODE_IP = iniFile.ReadValue("DEVICE", "BARCODE_IP", "");
                Device.BARCODE_PORT = iniFile.ReadValue("DEVICE", "BARCODE_PORT", "");
                Device.LET_URL = iniFile.ReadValue("DEVICE", "LET_URL", "");
                Device.LET_PORT = iniFile.ReadValue("DEVICE", "LET_PORT", "");

                Device.SWB_PORT = iniFile.ReadValue("DEVICE", "SWB_PORT", "COM3");
                Device.SWB_BAUD = iniFile.ReadValue("DEVICE", "SWB_BAUD", "115200");


                // PROGRAM 섹션 읽기
                Program.RESULT_PATH = iniFile.ReadValue("PROGRAM", "RESULT_PATH", "");
                Program.LANGUAGE = iniFile.ReadValue("PROGRAM", "LANGUAGE", "0");

                // WHEELBASE 섹션 읽기
                Wheelbase.HOME_POS = iniFile.ReadValue("WHEELBASE", "HOME_POS", "");
                Wheelbase.MIN_POS = iniFile.ReadValue("WHEELBASE", "MIN_POS", "");
                Wheelbase.MAX_POS = iniFile.ReadValue("WHEELBASE", "MAX_POS", "");


                SWB.adZeroPoint = iniFile.ReadDouble("AD_Calibration", "ZeroPoint");
                SWB.adPlusPoint = iniFile.ReadDouble("AD_Calibration", "PlusPoint");
                SWB.adMinusPoint = iniFile.ReadDouble("AD_Calibration", "MinusPoint");

                SWB.BoardType = iniFile.ReadInteger("AD_Calibration", "AngleType");




            }
            catch (Exception ex)
            {
                throw new Exception($"설정 파일 읽기 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        /// <summary>
        /// 현재 설정을 INI 파일에 저장합니다.
        /// </summary>
        public void SaveConfig()
        {
            try
            {
                // DEVICE 섹션 저장
                iniFile.WriteValue("DEVICE", "PLC_IP", Device.PLC_IP);
                iniFile.WriteValue("DEVICE", "PLC_PORT", Device.PLC_PORT);
                iniFile.WriteValue("DEVICE", "VEP_IP", Device.VEP_IP);
                iniFile.WriteValue("DEVICE", "VEP_PORT", Device.VEP_PORT);
                iniFile.WriteValue("DEVICE", "SCREW_IP", Device.SCREW_IP);
                iniFile.WriteValue("DEVICE", "SCREW_PORT", Device.SCREW_PORT);
                iniFile.WriteValue("DEVICE", "SCREW_IP2", Device.SCREW_IP2);
                iniFile.WriteValue("DEVICE", "SCREW_PORT2", Device.SCREW_PORT2);
                iniFile.WriteValue("DEVICE", "PRINT_IP", Device.PRINT_IP);
                iniFile.WriteValue("DEVICE", "PRINT_PORT", Device.PRINT_PORT);
                iniFile.WriteValue("DEVICE", "BARCODE_IP", Device.BARCODE_IP);
                iniFile.WriteValue("DEVICE", "BARCODE_PORT", Device.BARCODE_PORT);
                iniFile.WriteValue("DEVICE", "LET_URL", Device.LET_URL);
                iniFile.WriteValue("DEVICE", "LET_PORT", Device.LET_PORT);

                iniFile.WriteValue("DEVICE", "SWB_PORT", Device.SWB_PORT);
                iniFile.WriteValue("DEVICE", "SWB_BAUD", Device.SWB_BAUD);




                // PROGRAM 섹션 저장
                iniFile.WriteValue("PROGRAM", "RESULT_PATH", Program.RESULT_PATH);
                iniFile.WriteValue("PROGRAM", "LANGUAGE", Program.LANGUAGE);

                // WHEELBASE 섹션 저장
                iniFile.WriteValue("WHEELBASE", "HOME_POS", Wheelbase.HOME_POS);
                iniFile.WriteValue("WHEELBASE", "MIN_POS", Wheelbase.MIN_POS);
                iniFile.WriteValue("WHEELBASE", "MAX_POS", Wheelbase.MAX_POS);
            }
            catch (Exception ex)
            {
                throw new Exception($"설정 파일 저장 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        public void SaveSWB()
        {
            try
            {
                iniFile.WriteValue("AD_Calibration", "ZeroPoint", SWB.adZeroPoint.ToString());
                iniFile.WriteValue("AD_Calibration", "PlusPoint", SWB.adPlusPoint.ToString());
                iniFile.WriteValue("AD_Calibration", "MinusPoint", SWB.adMinusPoint.ToString());
                iniFile.WriteValue("AD_Calibration", "AngleType", SWB.BoardType.ToString());
            }
            catch (Exception ex)
            {
                throw new Exception($"설정 파일 저장 중 오류가 발생했습니다: {ex.Message}");
            }
        }
        /// <summary>
        /// 기본 설정으로 INI 파일을 생성합니다.
        /// </summary>
        public void CreateDefaultConfig()
        {
            try
            {
                // 기본값 설정
                Device.PLC_IP = "1";
                Device.PLC_PORT = "2";
                Device.VEP_IP = "3";
                Device.VEP_PORT = "4";
                Device.SCREW_IP = "5";
                Device.SCREW_PORT = "6";
                Device.PRINT_IP = "7";
                Device.PRINT_PORT = "8";
                Device.BARCODE_IP = "9";
                Device.BARCODE_PORT = "10";
                Device.LET_URL = "11";
                Device.LET_PORT = "12";

                Program.RESULT_PATH = "c:\\abc";
                Program.LANGUAGE = "0";

                Wheelbase.HOME_POS = "1000";
                Wheelbase.MIN_POS = "2000";
                Wheelbase.MAX_POS = "3000";

                // 파일에 저장
                SaveConfig();
            }
            catch (Exception ex)
            {
                throw new Exception($"기본 설정 파일 생성 중 오류가 발생했습니다: {ex.Message}");
            }
        }

        /// <summary>
        /// 설정 파일이 존재하는지 확인합니다.
        /// </summary>
        /// <returns>파일 존재 여부</returns>
        public bool ConfigExists()
        {
            return File.Exists(configPath);
        }

        /// <summary>
        /// 특정 섹션의 특정 키 값을 읽어옵니다.
        /// </summary>
        /// <param name="section">섹션명</param>
        /// <param name="key">키명</param>
        /// <param name="defaultValue">기본값</param>
        /// <returns>읽어온 값</returns>
        public string GetValue(string section, string key, string defaultValue = "")
        {
            return iniFile.ReadValue(section, key, defaultValue);
        }

        /// <summary>
        /// 특정 섹션의 특정 키에 값을 저장합니다.
        /// </summary>
        /// <param name="section">섹션명</param>
        /// <param name="key">키명</param>
        /// <param name="value">저장할 값</param>
        public void SetValue(string section, string key, string value)
        {
            iniFile.WriteValue(section, key, value);
        }
    }
} 
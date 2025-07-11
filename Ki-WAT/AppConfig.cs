using System;
using System.IO;

namespace Ki_WAT
{
    public class AppConfig
    {
        private IniFile iniFile;
        private string configPath;

        public DeviceConfig Device { get; private set; }
        public ProgramConfig Program { get; private set; }
        public WheelbaseConfig Wheelbase { get; private set; }

        public AppConfig()
        {
            // 실행 파일과 같은 디렉토리에 config.ini 파일 생성
            configPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "config.ini");
            iniFile = new IniFile(configPath);
            
            // 설정 객체 초기화
            Device = new DeviceConfig();
            Program = new ProgramConfig();
            Wheelbase = new WheelbaseConfig();
        }

        public void LoadConfig()
        {
            try
            {
                // 파일이 존재하지 않으면 기본값으로 생성
                if (!File.Exists(configPath))
                {
                    CreateDefaultConfig();
                }

                // DEVICE 섹션 로드
                Device.PLC_IP = iniFile.ReadValue("DEVICE", "PLC_IP", Device.PLC_IP);
                Device.PLC_PORT = iniFile.ReadValue("DEVICE", "PLC_PORT", Device.PLC_PORT);
                Device.VEP_IP = iniFile.ReadValue("DEVICE", "VEP_IP", Device.VEP_IP);
                Device.VEP_PORT = iniFile.ReadValue("DEVICE", "VEP_PORT", Device.VEP_PORT);
                Device.SCREW_IP = iniFile.ReadValue("DEVICE", "SCREW_IP", Device.SCREW_IP);
                Device.SCREW_PORT = iniFile.ReadValue("DEVICE", "SCREW_PORT", Device.SCREW_PORT);
                Device.PRINT_IP = iniFile.ReadValue("DEVICE", "PRINT_IP", Device.PRINT_IP);
                Device.PRINT_PORT = iniFile.ReadValue("DEVICE", "PRINT_PORT", Device.PRINT_PORT);
                Device.BARCODE_IP = iniFile.ReadValue("DEVICE", "BARCODE_IP", Device.BARCODE_IP);
                Device.BARCODE_PORT = iniFile.ReadValue("DEVICE", "BARCODE_PORT", Device.BARCODE_PORT);
                Device.LET_URL = iniFile.ReadValue("DEVICE", "LET_URL", Device.LET_URL);
                Device.LET_PORT = iniFile.ReadValue("DEVICE", "LET_PORT", Device.LET_PORT);

                // PROGRAM 섹션 로드
                Program.RESULT_PATH = iniFile.ReadValue("PROGRAM", "RESULT_PATH", Program.RESULT_PATH);
                Program.LANGUAGE = iniFile.ReadValue("PROGRAM", "LANGUAGE", Program.LANGUAGE);

                // WHEELBASE 섹션 로드
                Wheelbase.HOME_POS = iniFile.ReadValue("WHEELBASE", "HOME_POS", Wheelbase.HOME_POS);
                Wheelbase.MIN_POS = iniFile.ReadValue("WHEELBASE", "MIN_POS", Wheelbase.MIN_POS);
                Wheelbase.MAX_POS = iniFile.ReadValue("WHEELBASE", "MAX_POS", Wheelbase.MAX_POS);
            }
            catch (Exception ex)
            {
                // 로드 실패 시 기본값 사용
                System.Diagnostics.Debug.WriteLine($"Config 로드 실패: {ex.Message}");
            }
        }

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
                iniFile.WriteValue("DEVICE", "PRINT_IP", Device.PRINT_IP);
                iniFile.WriteValue("DEVICE", "PRINT_PORT", Device.PRINT_PORT);
                iniFile.WriteValue("DEVICE", "BARCODE_IP", Device.BARCODE_IP);
                iniFile.WriteValue("DEVICE", "BARCODE_PORT", Device.BARCODE_PORT);
                iniFile.WriteValue("DEVICE", "LET_URL", Device.LET_URL);
                iniFile.WriteValue("DEVICE", "LET_PORT", Device.LET_PORT);

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
                System.Diagnostics.Debug.WriteLine($"Config 저장 실패: {ex.Message}");
                throw;
            }
        }

        private void CreateDefaultConfig()
        {
            try
            {
                // 기본 설정으로 저장
                SaveConfig();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"기본 Config 생성 실패: {ex.Message}");
            }
        }
    }
} 
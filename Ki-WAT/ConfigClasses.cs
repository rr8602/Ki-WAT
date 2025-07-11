using System;

namespace Ki_WAT
{
    // DEVICE 섹션 설정 클래스
    public class DeviceConfig
    {
        public string PLC_IP { get; set; } = "192.168.10.10";
        public string PLC_PORT { get; set; } = "4600";
        public string VEP_IP { get; set; } = "";
        public string VEP_PORT { get; set; } = "";
        public string SCREW_IP { get; set; } = "";
        public string SCREW_PORT { get; set; } = "";
        public string PRINT_IP { get; set; } = "";
        public string PRINT_PORT { get; set; } = "";
        public string BARCODE_IP { get; set; } = "";
        public string BARCODE_PORT { get; set; } = "";
        public string LET_URL { get; set; } = "";
        public string LET_PORT { get; set; } = "";
    }

    // PROGRAM 섹션 설정 클래스
    public class ProgramConfig
    {
        public string RESULT_PATH { get; set; } = "C:\\Results";
        public string LANGUAGE { get; set; } = "0"; // 0: English, 1: Português, 2: Korea, 3: Other
    }

    // WHEELBASE 섹션 설정 클래스
    public class WheelbaseConfig
    {
        public string HOME_POS { get; set; } = "0";
        public string MIN_POS { get; set; } = "-100";
        public string MAX_POS { get; set; } = "100";
    }
} 
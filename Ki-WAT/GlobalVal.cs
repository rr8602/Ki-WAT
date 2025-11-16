using LETInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Ki_WAT
{

    struct LET_Param
    {
        public string uid;
        public int vsn;
        public string vin;
        public string line;
        public double floorpitch;

        public void Clear()
        {
            uid = "";
            vsn = 0;
            vin = "";
            line = "";
            floorpitch = 0.0;
        }
    }


    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LampInclination
    {
        // Equipment 정보
        public string Equipment_Name;
        public string Equipment_Manufacturer;
        public string Equipment_Model;
        public string Equipment_Serial_number;
        public string Equipment_Last_calib_date;

        // Vehicle 정보
        public string Vehicle_UID;
        public string Vehicle_Number_plate;
        public string Vehicle_VIN;
        public string Vehicle_Overall_result;
        public string Vehicle_Test_start;
        public string Vehicle_Test_end;

        
        public double Low_InclinationXInit_Left;
        public double Low_InclinationYInit_Left;
        public double Low_InclinationXFinal_Left;
        public double Low_InclinationYFinal_Left;

        public double Low_InclinationXInit_Right;
        public double Low_InclinationYInit_Right;
        public double Low_InclinationXFinal_Right;
        public double Low_InclinationYFinal_Right;

        public double Low_X_Terget_Left;
        public double Low_Y_Terget_Left;
        public double Low_X_Terget_Right;
        public double Low_Y_Terget_Right;

        public double Low_Tolerance_Up_Left;
        public double Low_Tolerance_Down_Left;
        public double Low_Tolerance_Left_Left;
        public double Low_Tolerance_Right_Left;

        public double Low_Tolerance_Up_Right;
        public double Low_Tolerance_Down_Right;
        public double Low_Tolerance_Left_Right;
        public double Low_Tolerance_Right_Right;

        public string Low_Left_Result;
        public string Low_Right_Result;

        // 초기화 메서드 추가
        public void clear()
        {
            
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MeasureData
    {
        public double dToeFL;
        public double dToeFR;
        public double dToeRL;
        public double dToeRR;

        public double dCamFL;
        public double dCamFR;
        public double dCamRL;
        public double dCamRR;

        public double dSymm;
        public double dTA;
        public double dHandle;
        public void Clear()
        {
            dToeFL = 0;
            dToeFR = 0;
            dToeRL = 0;
            dToeRR = 0;
            dCamFL = 0;
            dCamFR = 0;
            dCamRL = 0;
            dCamRR = 0;
            dSymm = 0;
            dTA = 0;
        }
        public MeasureData Clone()
        {
            return (MeasureData)this.MemberwiseClone();
        }

    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct SensorState
    {
        public int nState_FL;
        public int nState_FR;
        public int nState_RL;
        public int nState_RR;
        public void Initialize()
        {
            nState_FL = -1;
            nState_FR = -1;
            nState_RL = -1;
            nState_RR = -1;
        }
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DppData
    {
        public double dToeFL;
        public double dToeFR;
        public double dToeRL;
        public double dToeRR;

        public double dCamFL;
        public double dCamFR;
        public double dCamRL;
        public double dCamRR;

        public double dSymm;
        public double dTA;

        public double dHeightFL;
        public double dHeightFR;
        public double dHeightRL;
        public double dHeightRR;

        
    }
    
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct DppState
    {
        public int nState_FL;
        public int nState_FR;
        public int nState_RL;
        public int nState_RR;
    }

    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct COPYDATASTRUCT
    {
        public IntPtr dwData;
        public int cbData;
        public IntPtr lpData;
    }

    public class GlobalVal
    {
        // private static 필드로 Lazy<T> 사용
        private static readonly Lazy<GlobalVal> _instance = new Lazy<GlobalVal>(() => new GlobalVal());

        // public static property로 인스턴스 접근
        public static GlobalVal Instance => _instance.Value;

        // private 생성자로 외부에서 new 방지
        private GlobalVal()
        {
            g_MeasureData = new MeasureData();
            g_DppState = new SensorState();
            g_DppState.Initialize();
            Config = new AppConfig();
            _frmMNG = new FormManager();
            _TestThread = new TestThread_Kint(this);
            //_PLCVal = new PLCVal();
            //_VEP_Data = new VEPBenchDataManager(this);
            //_VEP_Client = new VEPBenchClient(this, _VEP_Data);
        }
        public void InitData()
        {

        }
        public MeasureData g_MeasureData;
        public SensorState g_DppState;

        // LET Controller variable
        public CycleControl LET_Controller;
        
        // Configuration instance
        public AppConfig Config;

        public FormManager _frmMNG;
        public TestThread_Kint _TestThread;


		public VEPController vep = new VEPController();
		//public PLCVal _PLCVal;

		public PLCReadWAT plcRead = null;
        public PLCWriteWAT plcWrite = null;

        
        public VEPBenchDataManager _VEP_Data;
        public VEPBenchClient     _VEP_Client;

        public TblCarModel m_Cur_Model = new TblCarModel();
        public TblCarInfo  m_Cur_Info  = new TblCarInfo();
        
        public TblCarModel m_Model_Rolling = new TblCarModel();
        public TblCarModel m_Model_Static = new TblCarModel();

        // 전역 변수
        public bool m_bTestRun = false;
        public double dHandle = 0.0f;
        //public bool g_Substitu_PEV = false;
        public bool m_bNextStep = false;
        public bool m_bHLTFinish = false;

        public Logger Log_LET = new Logger();
        public GPLog Log_PGM = new GPLog();

        private LangController langController = new LangController();
        private JsonController json = new JsonController();
        private LangData _currLang = new LangData();
        public DB_LocalWat _dbJob;

        public void SaveLangFile()
        {
            json.SaveLangFile("lang.json", langController);
            langController = json.LoadLangFile("lang.json");
        }
        public LangData GetCurrLang() { return _currLang; }
        public LangData GetLang(String strLang)
        {
            if (strLang == "English") return langController.English;
            else if (strLang == "Portuguese") return langController.Portuguese;
            else if (strLang == "Other") return langController.Other;
            else return langController.desc;
        }

    }
}

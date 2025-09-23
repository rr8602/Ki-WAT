using LETInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;

namespace Ki_WAT
{
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct LampInclination
    {
       public double InclinationXFinal_Left;
       public double InclinationYFinal_Left;
       public double InclinationXFinal_Right;
       public double InclinationYFinal_Right;
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
            Config = new AppConfig();
            _frmMNG = new FormManager();
            _TestThread = new TestThread_Kint(this);
            _PLCVal = new PLCVal();
            _VEP_Data = new VEPBenchDataManager(this);
            _VEP_Client = new VEPBenchClient(this, _VEP_Data);
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

        public PLCVal _PLCVal;

        public bool g_Substitu_PEV = false;
        public bool g_Substitu_HLT = false;
        public bool g_Substitu_SWB = false;
        public bool g_Substitu_MovingFlate = false;
        public bool g_Substitu_WheelAdjust = false;
        public bool g_Substitu_ScrewDriver_L = false;
        public bool g_Substitu_ScrewDriver_R = false;
        public bool g_Substitu_Printer = false;

        public VEPBenchDataManager _VEP_Data;
        public VEPBenchClient     _VEP_Client;

        public TblCarModel m_Cur_Model = new TblCarModel();
        public TblCarInfo  m_Cur_Info  = new TblCarInfo();
        // 전역 변수
        public  bool m_bTestRun = false;
        public double dHandle = 0.0f;
        //public bool g_Substitu_PEV = false;
        public bool m_bNextStep = false;
    }
}

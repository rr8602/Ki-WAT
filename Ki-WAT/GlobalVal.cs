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
        public double dHandle;
        public double dTA;

        public double dHeightFL;
        public double dHeightFR;
        public double dHeightRL;
        public double dHeightRR;
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




    internal class GlobalVal
    {
        // private static 필드로 Lazy<T> 사용
        private static readonly Lazy<GlobalVal> _instance = new Lazy<GlobalVal>(() => new GlobalVal());

        // public static property로 인스턴스 접근
        public static GlobalVal Instance => _instance.Value;

        // private 생성자로 외부에서 new 방지
        private GlobalVal()
        {
            g_DppData = new MeasureData();
            g_DppState = new SensorState();
            Config = new AppConfig();
        }

        public MeasureData g_DppData;
        public SensorState g_DppState;

        // LET Controller variable
        public CycleControl LET_Controller;
        
        // Configuration instance
        public AppConfig Config;
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{
    internal class GlobalVal
    {

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
        public struct SensorState
        {
            public int nState_FL;
            public int nState_FR;
            public int nState_RL;
            public int nState_RR;
        }

        // private static 필드로 Lazy<T> 사용
        private static readonly Lazy<GlobalVal> _instance = new Lazy<GlobalVal>(() => new GlobalVal());

        // public static property로 인스턴스 접근
        public static GlobalVal Instance => _instance.Value;

        // private 생성자로 외부에서 new 방지
        private GlobalVal()
        {
            g_DppData = new MeasureData();
            g_DppState = new SensorState();
        }

        public MeasureData g_DppData;
        public SensorState g_DppState;
    }
}

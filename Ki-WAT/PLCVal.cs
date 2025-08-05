using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{

    public class ST_LOW_DATA
    {
        public byte m_nVal = 0;
        public bool[] bOn = new bool[16];
                
    };

    public class PLC_INPUT
    {

        public bool bHomePos = false;
        public bool bReadyPos = false;
        public bool bRunoutPos = false;
    }

    public class PLC_OUTPUT
    {
        //
        public bool bHomePos = false;
        public bool bReadyPos = false;
        public bool bRunoutPos = false;
    }


    internal class PLCVal
    {
        
        private ST_LOW_DATA[] DI_DATA = new ST_LOW_DATA[100];
        private ST_LOW_DATA[] DO_DATA = new ST_LOW_DATA[100];

        private byte[] BitA = new byte[16];
        public PLC_INPUT DI;
        public PLC_OUTPUT DO;
        public PLCVal()
        {
            for (int i = 1; i < 16; i++)
            {
                BitA[i] = (byte)(BitA[i - 1] * 2);
            }
        }

        public void DIn_Map()
        {
            DI.bHomePos = DI_DATA[0].bOn[0];
        }
        public void DDout_Map()
        {
            DO_DATA[0].bOn[0] = DO.bHomePos;
            DO_DATA[0].bOn[1] = DO.bReadyPos;
        }

    }
}

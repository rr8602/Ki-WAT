using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public class VEPBenchDataManager
    {

        GlobalVal _GV;
        private VEPBenchClient _parent;
        public VEPBenchDescriptionZone DescriptionZone { get; private set; }
        public VEPBenchReceptionZone ReceptionZone { get; private set; }
        public VEPBenchStatusZone StatusZone { get; private set; }
        public VEPBenchSynchroZone SynchroZone { get; internal set; }
        public VEPBenchTransmissionZone TransmissionZone { get; private set; }

        public VEPBenchAddTransmissionZone AddTransmissionZone { get; internal set; }


        public VEPBenchDataManager(GlobalVal gv)
        {
            _GV = gv;
            DescriptionZone = new VEPBenchDescriptionZone();
            ReceptionZone = VEPBenchReceptionZone.Instance;
            StatusZone = VEPBenchStatusZone.Instance;
            SynchroZone = VEPBenchSynchroZone.Instance;
            TransmissionZone = VEPBenchTransmissionZone.Instance;
            AddTransmissionZone = VEPBenchAddTransmissionZone.Instance;


        }

        //public VEPBenchSynchroZone RefreshSynchroZoneFromVEP(VEPBenchClient client)
        //{
        //    if (client != null && client.IsConnected)
        //    {
        //        return VEPBenchSynchroZone.ReadFromVEP((start, count) => client.ReadSynchroZone(start, count));
        //    }

        //    return null;
        //}

        public void UpdateAllZonesFromRegisters(Func<int, int, ushort[]> readRegistersFunc)
        {
            StatusZone.FromRegisters(readRegistersFunc(DescriptionZone.StatusZoneAddr, DescriptionZone.StatusZoneSize));

            ushort[] synchroZone = readRegistersFunc(DescriptionZone.SynchroZoneAddr, DescriptionZone.SynchroZoneSize);
            SynchroZone.FromRegisters(synchroZone);

            ushort[] tzZone = readRegistersFunc(DescriptionZone.TransmissionZoneAddr, DescriptionZone.TransmissionZoneSize);
            TransmissionZone.FromRegisters(tzZone);


            ReceptionZone.FromRegisters(readRegistersFunc(DescriptionZone.ReceptionZoneAddr, DescriptionZone.ReceptionZoneSize));

        }

        public void WriteAllZonesToRegisters(Action<int, ushort[]> writeRegistersFunc)
        {
            writeRegistersFunc(DescriptionZone.StatusZoneAddr, StatusZone.ToRegisters());

            ushort[] synchroRegisters = SynchroZone.ToRegisters();
            writeRegistersFunc(DescriptionZone.SynchroZoneAddr, synchroRegisters);
            writeRegistersFunc(DescriptionZone.TransmissionZoneAddr, TransmissionZone.ToRegisters());
        }
        public void ReadTransmissionZonesFromRegisters(Func<int, int, ushort[]> readRegistersFunc)
        {
            AddTransmissionZone.FromRegisters(readRegistersFunc(DescriptionZone.AdditionalTZAddr, DescriptionZone.AdditionalTZSize));
        }

        public void SetParent(VEPBenchClient parent) { _parent = parent; }
        public int SetSyncroZero()
        {
            int nSize = DescriptionZone.SynchroZoneSize;
            for (int i = 0; i < nSize; i++)
            {

                SynchroZone.SetValue(i, 0);
                _parent.WriteSynchroZone();
            }
            return 0;
        }
        public bool IsRequestPJI()
        {
            bool bRequest = false;

            if (ReceptionZone.AddReSize == 0 &&
                ReceptionZone.FctCode == 6 &&
                ReceptionZone.PCNum == 1 &&
                ReceptionZone.ProcessCode == 1 &&
                ReceptionZone.SubFctCode == 0
            )
            {
                bRequest = true;
            }

            return bRequest;
        }
        public int SendPJI(String strPJI)
        {
            //if (modClient.Connected == false) return -1;

            int nSize = DescriptionZone.ReceptionZoneSize;


            //RZ_Addr + 2 AddReSize		WORD	   0
            //RZ_Addr + 6 FctCode		Low Byte    6
            //			PCNum			High Byte   1
            //RZ_Addr + 8 ProcessCode	Low Byte    1
            //			SubFctCode		High Byte   0
            //RZ_Addr + 12    Data[0]	Low Byte    1
            //							High Byte   7

            ReceptionZone.AddReSize = 0;
            ReceptionZone.FctCode = 6;
            ReceptionZone.PCNum = 1;
            ReceptionZone.ProcessCode = 1;
            ReceptionZone.SubFctCode = 0;

            String strSendPJI = strPJI;
            int nLen = strSendPJI.Length;
            //RZ_Addr + 12    Data[0]	Low Byte    1
            //							High Byte   7
            //High Byte에 Size를 넣어야 함.
            {
                byte cLow = 1;
                byte cHigh = (byte)nLen;
                ReceptionZone.SetWord(12, cHigh, cLow);
            }

            if (nLen % 2 != 0) strSendPJI = strSendPJI + "0";
            // 문자열을 2개씩 짝지어서 WORD 배열에 넣기
            int nStartPJI = 13;
            for (int i = 0; i < strSendPJI.Length / 2; i++)
            {
                byte cLow = (byte)strSendPJI[i * 2];     // 앞 글자
                byte cHigh = (byte)strSendPJI[i * 2 + 1];  // 뒷 글자
                ReceptionZone.SetWord(nStartPJI + i, cHigh, cLow);
                //_GV.vepData.SetValue(pjiIndex + i, (ushort)((high << 8) | low));
            }

            _parent.WriteReceptionZone();

            return 1;
        }

    }
}

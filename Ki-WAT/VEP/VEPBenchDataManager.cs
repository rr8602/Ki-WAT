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

        public VEPBenchDescriptionZone DescriptionZone { get; private set; }
        public VEPBenchReceptionZone ReceptionZone { get; private set; }
        public VEPBenchStatusZone StatusZone { get; private set; }
        public VEPBenchSynchroZone SynchroZone { get; internal set; }
        public VEPBenchTransmissionZone TransmissionZone { get; private set; }

        
        public VEPBenchDataManager(GlobalVal gv)
        {
            _GV = gv;
            DescriptionZone = new VEPBenchDescriptionZone();
            ReceptionZone = VEPBenchReceptionZone.Instance;
            StatusZone = VEPBenchStatusZone.Instance;
            SynchroZone = VEPBenchSynchroZone.Instance;
            TransmissionZone = VEPBenchTransmissionZone.Instance;


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

            ushort[] synchroPart1 = readRegistersFunc(DescriptionZone.SynchroZoneAddr, VEPBenchSynchroZone.SYNCHRO_SIZE_PART1);
            
            SynchroZone.FromRegisters(synchroPart1);
           
            ushort[] tzZone = readRegistersFunc(DescriptionZone.TransmissionZoneAddr, DescriptionZone.TransmissionZoneSize);
            TransmissionZone.FromRegisters(tzZone);
            

            ReceptionZone.FromRegisters(readRegistersFunc(DescriptionZone.ReceptionZoneAddr, DescriptionZone.ReceptionZoneSize));
        }

        public void WriteAllZonesToRegisters(Action<int, ushort[]> writeRegistersFunc)
        {
            writeRegistersFunc(DescriptionZone.StatusZoneAddr, StatusZone.ToRegisters());

            ushort[] synchroRegisters = SynchroZone.ToRegisters();
            writeRegistersFunc(DescriptionZone.SynchroZoneAddr, synchroRegisters.Take(VEPBenchSynchroZone.SYNCHRO_SIZE_PART1).ToArray());
            writeRegistersFunc(DescriptionZone.SynchroZoneAddr + VEPBenchSynchroZone.SYNCHRO_SIZE_PART1, synchroRegisters.Skip(VEPBenchSynchroZone.SYNCHRO_SIZE_PART1).ToArray());

            writeRegistersFunc(DescriptionZone.TransmissionZoneAddr, TransmissionZone.ToRegisters());
        }

        

    }
}

using System;

namespace Ki_WAT
{   
    public interface IVEPBenchZone
    {
        bool IsChanged { get; }
        void ResetChangedState();
        ushort[] ToRegisters();
        void FromRegisters(ushort[] registers);
    }
}

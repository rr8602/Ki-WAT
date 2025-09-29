using Ki_WAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{
    public class VEPBenchSynchroZone : IVEPBenchZone
    {
        private static VEPBenchSynchroZone _instance;
        private static readonly object _lock = new object();

        
        //public const int SYNCHRO_SIZE_PART1 = 90;


        //public const int SYNCHRO_SIZE_PART2 = 67;
        //public const int DEFAULT_SYNCHRO_SIZE = SYNCHRO_SIZE_PART1 + SYNCHRO_SIZE_PART2;

        public ushort GetSyncroValue(int nSync)
        {
            return (ushort)_values[nSync];
        }
        public static VEPBenchSynchroZone Instance
        {
            get
            {
                if (_instance == null)
                {
                    lock (_lock)
                    {
                        if (_instance == null)
                            _instance = new VEPBenchSynchroZone();
                    }
                }
                return _instance;
            }
        }
        
        private ushort[] _values;
        public int Size => _values.Length;
        
        public int this[int index]
        {
            get
            {
                if (index < 0 || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");

                return _values[index];
            }
            set
            {
                if (index < 0 || index >= _values.Length)
                    throw new IndexOutOfRangeException("Index out of range for VEPBenchSynchro values.");

                _values[index] = (ushort)value;
            }
        }

        private bool _isChanged;
        public bool IsChanged => _isChanged;

        public void ResetChangedState()
        {
            _isChanged = false;
        }

        public VEPBenchSynchroZone(int size = 90)
        {
            _values = new ushort[size];
            ResetAllValues();
            _isChanged = false;
        }


        public void FromRegisters(ushort[] registers)
        {

            if (registers == null)
                throw new ArgumentNullException(nameof(registers));

            bool changed = false;

            if (_values.Length != registers.Length)
            {

                _values = new ushort[registers.Length];
                changed = true;
            }

            for (int i = 0; i < registers.Length; i++)
            {
                if (_values[i] != registers[i])
                {
                    _values[i] = registers[i];
                    changed = true;
                }
            }

            if (changed)
            {
                _isChanged = true;
            }
        }

        public ushort[] ToRegisters()
        {
            ushort[] result = new ushort[_values.Length];

            for (int i = 0; i < _values.Length; i++)
            {
                result[i] = Convert.ToUInt16(_values[i]);
            }

            return result;
        }

        public void ResetAllValues()
        {
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = 0;
            }
        }

        public void SetValue(int index, ushort value)
        {
            this[index] = value;
        }

        public int GetValue(int index)
        {
            return this[index];
        }
        public void SetSize(int size)
        {
            if (size <= 0) return;
            _values = new ushort[size];
            for (int i = 0; i < _values.Length; i++)
            {
                _values[i] = 0;
            }
        }

    }
}

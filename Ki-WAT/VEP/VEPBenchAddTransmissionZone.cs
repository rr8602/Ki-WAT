
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace KI_VEP
{
	public class VEPBenchAddTransmissionZone : IVEPBenchZone
	{
		private static VEPBenchAddTransmissionZone _instance;
		private static readonly object _lock = new object();


		public static VEPBenchAddTransmissionZone Instance
		{
			get
			{
				if (_instance == null)
				{
					lock (_lock)
					{
						if (_instance == null)
							_instance = new VEPBenchAddTransmissionZone();
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

		public VEPBenchAddTransmissionZone(int size = 10)
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

		public void ShowData()
		{
			for (int i = 0; i < _values.Length; i++)
			{
				Console.WriteLine("POS : " + i.ToString() + " VALUE + " + _values[i].ToString());
			}
		}
	}
}

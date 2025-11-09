using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{
	public class VEPData
	{
		public ushort Validate = 0;
		public ushort SZAddr = 0;
		public ushort SZSize = 0;
		public ushort SYAddr = 0;
		public ushort SYSize = 0;
		public ushort TZAddr = 0;
		public ushort TZSize = 0;
		public ushort RZAddr = 0;
		public ushort RZSize = 0;
		public ushort AddTZAddr = 0;
		public ushort AddTZSize = 0;
		public ushort AddRZAddr = 0;
		public ushort AddRZSize = 0;

		public ushort[] SZData = null;
		public ushort[] SYData = null;
		public ushort[] TZData = null;
		public ushort[] RZData = null;
		public ushort[] AddTZData = null;
		public ushort[] AddRZData = null;


		public char[] _TZcharBuffer;
		public int _TZdataSize = 0;
		public string TZDataString { get; set; }


		public char[] _RZcharBuffer;
		public int _RZdataSize = 0;
		public string RZDataString { get; set; }


		public char[] _AddTZcharBuffer;
		public int _AddTZdataSize = 0;
		public string AddTZDataString { get; set; }



		public ushort VEPStatus { get { return SZData[0]; } }
		public ushort VEPCycleInterupt { get { return SZData[1]; } }
		public ushort VEPCycleEnd { get { return SZData[2]; } }
		public ushort BenchCycleInterupt { get { return SZData[3]; } }
		public ushort BenchCycleEnd { get { return SZData[4]; } }
		public ushort StartCycle { get { return SZData[5]; } }


		public ushort TZ_AddTZSize { get { return TZData[2]; } }
		public ushort TZ_ExchangeStatus { get { return TZData[3]; } }
		public ushort TZ_FctCode { get { return GetLowByte(TZData[6]); } }
		public ushort TZ_PCNum { get { return GetHighByte(TZData[6]); } }
		public ushort TZ_ProcessCode { get { return GetLowByte(TZData[8]); } }
		public ushort TZ_SubFctCode { get { return GetHighByte(TZData[8]); } }


		public ushort RZ_AddRZSize { get { return RZData[2]; }  }
		public ushort RZ_ExchangeStatus { get { return RZData[3]; } }
		public ushort RZ_FctCode { get { return GetLowByte(RZData[6]); } }
		public ushort RZ_PCNum { get { return GetHighByte(RZData[6]); } }
		public ushort RZ_ProcessCode { get { return GetLowByte(RZData[8]); } }
		public ushort RZ_SubFctCode { get { return GetHighByte(RZData[8]); } }


		public void SetDesZoneData(ushort [] DesZone)
		{
			if (DesZone == null) return;
			if (DesZone.Length != 18) return;
			Validate = DesZone[0];
			SZAddr = DesZone[1];
			SZSize = DesZone[2];
			SYAddr = DesZone[3];
			SYSize = DesZone[4];
			TZAddr = DesZone[10];
			TZSize = DesZone[11];
			RZAddr = DesZone[12];
			RZSize = DesZone[13];
			AddTZAddr = DesZone[14];
			AddTZSize = DesZone[15];
			AddRZAddr = DesZone[16];
			AddRZSize = DesZone[17];
			SetSize();
		}
		private void SetSize()
		{
			if (SZSize > 0) 
			{
				if (SZData == null || SZData.Length != SZSize)				SZData = new ushort[SZSize];
			}
			if (SYSize > 0)
			{
				if (SYData == null || SYData.Length != SYSize) SYData = new ushort[SYSize];
			}
			if (TZSize > 0)
			{
				if (TZData == null || TZData.Length != TZSize)
				{
					TZData = new ushort[TZSize];
					_TZcharBuffer = new char[TZSize];
				}
			}
			if (RZSize > 0)
			{
				if (RZData == null || RZData.Length != RZSize)
				{
					RZData = new ushort[RZSize];
					_RZcharBuffer = new char[RZSize];
				}

			}
			if (AddTZSize > 0)
			{

				if (AddTZData == null || AddTZData.Length != AddTZSize)
				{
					AddTZData = new ushort[AddTZSize];
					_AddTZcharBuffer = new char[AddTZSize];
				}
			}
			if (AddRZSize > 0)
			{
				if (AddRZData == null || AddRZData.Length != AddRZSize) AddRZData = new ushort[AddRZSize];
			}

		}

		public byte GetHighByte(ushort value)
		{
			return (byte)(value >> 8); // 상위 8비트 추출
		}

		public byte GetLowByte(ushort value)
		{
			return (byte)(value & 0xFF); // 하위 8비트 추출
		}
		public ushort CombineBytes(byte low, byte high )
		{
			return (ushort)((high << 8) | low);
		}

	}
}

using RollTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
	public partial class VEPController
	{

		public void V_SZInit()
		{
			DirectWriteSingle((ushort)(vepData.SZAddr + 0), (ushort)1);
			DirectWriteSingle((ushort)(vepData.SZAddr + 1), (ushort)0);
			DirectWriteSingle((ushort)(vepData.SZAddr + 2), (ushort)0);
			DirectWriteSingle((ushort)(vepData.SZAddr + 3), (ushort)0);
			DirectWriteSingle((ushort)(vepData.SZAddr + 4), (ushort)0);
			DirectWriteSingle((ushort)(vepData.SZAddr + 5), (ushort)0);
		}

		public void V_SetVEPStatus(int nStatus)
		{
			DirectWriteSingle((ushort)(vepData.SZAddr + 0), (ushort)nStatus);
		}
		public void V_VEPBreak()
		{
			DirectWriteSingle((ushort)(vepData.SZAddr + 1), (ushort)1);
		}
		public void V_VEPEnd()
		{
			DirectWriteSingle((ushort)(vepData.SZAddr + 2), (ushort)1);
		}

		public int B_BenchCycleBrake()
		{

			int nRet = 0;
			DirectWriteSingle((ushort)(vepData.SZAddr + 3), (ushort)1);
			SetExChange(2);
			return nRet;
		}
		public int B_BenchCycleEnd()
		{
			int nRet = 0;
			DirectWriteSingle((ushort)(vepData.SZAddr + 4), (ushort)1);
			SetExChange(2);
			return nRet;
		}
		public int B_StartCycle()
		{
			int nRet = 0;
			DirectWriteSingle((ushort)(vepData.SZAddr + 5), (ushort)1);
			SetExChange(2);
			return nRet;
		}
		/////////////////////////////////////////////////////////////////////////////////








		/////////////////////////////////////////////////////////////////////////////////

		public void SetSynchro(int nSyncNo, int nValue)
		{
			if (nSyncNo < 0) return;
			if (nSyncNo >= vepData.SYSize) return;
			DirectWriteSingle((ushort)(vepData.SYAddr + nSyncNo), (ushort)nValue);
		}
		public int B_SetSyncroAllZero()
		{

			int nSize = vepData.SYSize;

			for (int i = 0; i < nSize; i++)
			{
				vepData.SYData[i] = 0;
			}
			DirectWriteMultiple(vepData.SYAddr, vepData.SYData);
			SetExChange(2);
			return 0;
		}

		public bool IsSynchroAllZero()
		{
			int nSize = vepData.SYData.Length;

			for (int i = 0; i < nSize; i++)
			{
				if (vepData.SYData[i] != 0) return false;
			}

			return true;
		}
		/////////////////////////////////////////////////////////////////////////////////



		/////////////////////////////////////////////////////////////////////////////////
		public void SetExChange(int nVal)
		{
			SetTZExchange(nVal);
			SetRZExchange(nVal);
		}

		public void SetTZExchange(int nTZEx1)
		{
			DirectWriteSingle((ushort)(vepData.TZAddr + 3), (ushort)nTZEx1);
		}
		public void SetRZExchange(int nRZEx1)
		{
			DirectWriteSingle((ushort)(vepData.RZAddr + 3), (ushort)nRZEx1);
		}

		public int GetTZExchange() { return vepData.TZData[3]; }
		public int GetRZExchange() { return vepData.RZData[3]; }
		/////////////////////////////////////////////////////////////////////////////////




		/////////////////////////////////////////////////////////////////////////////////
		public void V_SetRequestPJItoTZ()
		{
			//TZ_Addr + 2	AddTrSize WORD    0
			//TZ_Addr + 6	FctCode Low Byte    6
			//				PCNum High Byte   1
			//TZ_Addr + 8	ProcessCode Low Byte    1
			//				SubFctCode High Byte   0

			ushort uTZAddr2 = 0;
			ushort uTZAddr6 = 0;
			ushort uTZAddr8 = 0;

			uTZAddr6 = vepData.CombineBytes(6, 1);
			uTZAddr8 = vepData.CombineBytes(1, 0);

			DirectWriteSingle((ushort)(vepData.TZAddr + 2), (ushort)uTZAddr2);
			DirectWriteSingle((ushort)(vepData.TZAddr + 6), (ushort)uTZAddr6);
			DirectWriteSingle((ushort)(vepData.TZAddr + 8), (ushort)uTZAddr8);

			SetExChange(1);
		}
		public bool IsResponsePJIfromRZ()
		{
			if (vepData.RZ_AddRZSize == 0 &&
				vepData.RZ_FctCode == 6 &&
				vepData.RZ_PCNum == 1 &&
				vepData.RZ_ProcessCode == 1 &&
				vepData.RZ_SubFctCode == 0)
			{
				return true;
			}
			else return false;

		}
		public bool IsRequestPJIfromTZ()
		{
			if (vepData.TZ_AddTZSize == 0 &&
				vepData.TZ_FctCode == 6 &&
				vepData.TZ_PCNum == 1 &&
				vepData.TZ_ProcessCode == 1 &&
				vepData.TZ_SubFctCode == 0)
			{
				return true;
			}
			else return false;

		}
		public String GetPJI()
		{
			String PJI = "";

			if (vepData.RZ_AddRZSize == 0 &&
				vepData.RZ_FctCode == 6 &&
				vepData.RZ_PCNum == 1 &&
				vepData.RZ_ProcessCode == 1 &&
				vepData.RZ_SubFctCode == 0)
			{
				PJI = vepData.RZDataString;

			}
			else
			{

			}
			return PJI;
		}



		public int B_SendPJIMultiple(String strPJI)
		{


			int nSize = vepData.RZSize;


			//vepData.RZ_AddRZSize = 0;
			//vepData.RZ_FctCode = 6;
			//vepData.RZ_PCNum = 1;
			//vepData.RZ_ProcessCode = 1;
			//vepData.RZ_SubFctCode = 0;


			ushort uRZAddr2 = 0;
			ushort uRZAddr6 = 0;
			ushort uRZAddr8 = 0;

			uRZAddr6 = vepData.CombineBytes(6, 1);
			uRZAddr8 = vepData.CombineBytes(1, 0);

			DirectWriteSingle((ushort)(vepData.RZAddr + 2), (ushort)uRZAddr2);
			DirectWriteSingle((ushort)(vepData.RZAddr + 6), (ushort)uRZAddr6);
			DirectWriteSingle((ushort)(vepData.RZAddr + 8), (ushort)uRZAddr8);




			String strSendPJI = strPJI;


			if (strSendPJI.Length % 2 != 0) strSendPJI = strSendPJI + "0";

			int nLen = strSendPJI.Length / 2 + 1;

			ushort[] uSendPJI = new ushort[nLen];


			//RZ_Addr + 12    Data[0]	Low Byte    1
			//							High Byte   7
			//High Byte에 Size를 넣어야 함.
			{
				byte cHigh = 1;
				byte cLow = (byte)nLen;
				uSendPJI[0] = vepData.CombineBytes(cLow, cHigh);

			}



			// 문자열을 2개씩 짝지어서 WORD 배열에 넣기

			for (int i = 0; i < strSendPJI.Length / 2; i++)
			{
				byte cLow = (byte)strSendPJI[i * 2];     // 앞 글자
				byte cHigh = (byte)strSendPJI[i * 2 + 1];  // 뒷 글자

				uSendPJI[1 + i] = vepData.CombineBytes(cLow, cHigh);

			}
			int nStartPJI = 12;
			DirectWriteMultiple((ushort)(vepData.RZAddr + nStartPJI), uSendPJI);


			SetExChange(2);

			return 1;
		}
		/////////////////////////////////////////////////////////////////////////////////




		/////////////////////////////////////////////////////////////////////////////////

		public bool IsOPMsgAnsi()
		{
			if (vepData.TZ_AddTZSize == 0 &&
				vepData.TZ_FctCode == 20 &&
				vepData.TZ_PCNum == 1 &&
				(vepData.TZ_ProcessCode == 1 || vepData.TZ_ProcessCode == 2) &&
				vepData.TZ_SubFctCode == 2)
			{
				return true;
			}
			else return false;
		}

		public String GetOPMsgAnsi()
		{
			String OPMsg = "";

			if (vepData.TZ_AddTZSize == 0 &&
				vepData.TZ_FctCode == 20 &&
				vepData.TZ_PCNum == 1 &&
				(vepData.TZ_ProcessCode == 1 || vepData.TZ_ProcessCode == 2) &&
				vepData.TZ_SubFctCode == 2)
			{
				OPMsg = vepData.TZDataString;
			}


			return OPMsg;
		}
		/////////////////////////////////////////////////////////////////////////////////




		public bool IsTicketfromTZ()
		{
			if (vepData.TZ_AddTZSize == 0 &&
				vepData.TZ_FctCode == 15 &&
				vepData.TZ_PCNum == 1 &&
				vepData.TZ_ProcessCode == 20 &&
				vepData.TZ_SubFctCode == 0)
			{
				return true;
			}
			else return false;

		}
		public String GetTicket()
		{
			String Ticket = "";
			if (vepData.TZ_AddTZSize == 0 &&
				vepData.TZ_FctCode == 15 &&
				vepData.TZ_PCNum == 1 &&
				vepData.TZ_ProcessCode == 20 &&
				vepData.TZ_SubFctCode == 0)
			{

				Ticket = vepData.AddTZDataString;

			}
			return Ticket;

		}


		public void V_SetTicketToAddTZMultiple(int nCol, int nLine, String PJI)
		{

			String dateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
			String strTicket = "This is the Ticket. \n PJI : " + PJI + " \n  + VEP : OK \n DateTime : " + dateTime;



			int nSize = vepData.RZSize;

			//TZ에 설정을 기록한다.

			ushort uTZAddr2 = 0;
			ushort uTZAddr6 = 0;
			ushort uTZAddr8 = 0;

			uTZAddr6 = vepData.CombineBytes(15, 1);
			uTZAddr8 = vepData.CombineBytes(20, 0);

			DirectWriteSingle((ushort)(vepData.TZAddr + 2), (ushort)uTZAddr2);
			DirectWriteSingle((ushort)(vepData.TZAddr + 6), (ushort)uTZAddr6);
			DirectWriteSingle((ushort)(vepData.TZAddr + 8), (ushort)uTZAddr8);



			//TZ에 PJI를 기록한다.

			String strSendPJI = PJI;


			if (strSendPJI.Length % 2 != 0) strSendPJI = strSendPJI + "0";

			int nLen = strSendPJI.Length / 2;

			ushort[] uSendPJI = new ushort[nLen];


			// 문자열을 2개씩 짝지어서 WORD 배열에 넣기

			for (int i = 0; i < nLen; i++)
			{
				byte cLow = (byte)strSendPJI[i * 2];     // 앞 글자
				byte cHigh = (byte)strSendPJI[i * 2 + 1];  // 뒷 글자

				uSendPJI[i] = vepData.CombineBytes(cLow, cHigh);

			}
			int nStartPJI = 12;
			DirectWriteMultiple((ushort)(vepData.TZAddr + nStartPJI), uSendPJI);

			DirectWriteSingle((ushort)(vepData.TZAddr + 16), (ushort)nCol);
			DirectWriteSingle((ushort)(vepData.TZAddr + 17), (ushort)nLine);








			//티켓 본문을 AddTZ에 기록한다.
			String strSendTicket = strTicket;


			if (strSendTicket.Length % 2 != 0) strSendTicket = strSendTicket + "0";
			nLen = strSendTicket.Length / 2;
			ushort[] uTicket = new ushort[nLen];
			// 문자열을 2개씩 짝지어서 WORD 배열에 넣기
			int nStartTicket = 0;

			for (int i = 0; i < strSendTicket.Length / 2; i++)
			{
				byte cLow = (byte)strSendTicket[i * 2];     // 앞 글자
				byte cHigh = (byte)strSendTicket[i * 2 + 1];  // 뒷 글자

				uTicket[i] = vepData.CombineBytes(cLow, cHigh);

			}
			DirectWriteMultiple((ushort)(vepData.AddTZAddr + nStartTicket), uTicket);

			SetExChange(1);

		}



		public void SetSync30NewVehicle()
		{
			SetSynchro(30, 1);
			//_vepManager.SynchroZone.SetValue(30, 1);
			//WriteSynchroZone();
		}
		public void SetSync06RunOutFinish()
		{
			SetSynchro(6, 1);
			//_vepManager.SynchroZone.SetValue(6, 1);
			//WriteSynchroZone();
		}
		public void SetSync07SWBAngle(ushort shRes)
		{
			SetSynchro(7, shRes);
			//_vepManager.SynchroZone.SetValue(7, shRes);
			//WriteSynchroZone();
		}
		public void SetSync10TAReady()
		{
			SetSynchro(10, 1);
			//_vepManager.SynchroZone.SetValue(10, 1);
			//WriteSynchroZone();
		}
		public void SetSync11TAAngle(ushort shRes)
		{
			SetSynchro(11, shRes);
			//_vepManager.SynchroZone.SetValue(11, shRes);
			//WriteSynchroZone();
		}

		public void SetSync04SWB_Imform()
		{
			SetSynchro(4, 1);
			//_vepManager.SynchroZone.SetValue(4, 1);
			//WriteSynchroZone();
		}
		public void SetSync21SWBAngle(int nA, int nB, double dX)
		{
			ushort shRes = (ushort)((ushort)(nA * dX) + nB);

			SetSynchro(21, shRes);
			//_vepManager.SynchroZone.SetValue(21, shRes);
			//WriteSynchroZone();
		}

		public void SetSync32HLAFinish()
		{
			SetSynchro(32, 1);
			//_vepManager.SynchroZone.SetValue(32, 1);
			//WriteSynchroZone();
		}





	}
}

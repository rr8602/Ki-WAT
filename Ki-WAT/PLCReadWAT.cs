using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_WAT
{
	public class PLCReadWAT
	{
		uint m_nSize = 230;
		public Byte[] rawDataIn = null;

        GlobalVal _GV = GlobalVal.Instance;
        S7Client client = new S7Client();
		private CancellationTokenSource _cts;

		private String _strIP = "";

		public void Create(String strIP)
		{
			_strIP = strIP;

			CreateData();

			int result = client.ConnectTo(_strIP, 0, 1);

			if (client.Connected )
			{
				//Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.Connect);
				StartPolling();
			}
			else if (!client.Connected )
			{
				//Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.NotConnect);
				
			}

		}
		public void Release()
		{
			CancelPollingToken();
			if (client != null)
			{
				client.Disconnect();
				client = null;


			}
		}

		public bool IsPLCConnect()
		{
			return client.Connected;
        }
		private void CancelPollingToken()
		{
			if (_cts != null && !_cts.IsCancellationRequested)
			{
				try { _cts.Cancel(); }
				catch (ObjectDisposedException) { } // 이미 Dispose된 경우 무시
			}
		}
		public void StartPolling()
		{
			CancelPollingToken();
			_cts = new CancellationTokenSource();
			Task.Run(() => PollingPLCData(_cts.Token));
		}
		private void PollingPLCData(CancellationToken token)
		{

			
			while (!token.IsCancellationRequested)
			{

				if (client.Connected == true)
				{
					int result = client.ReadArea(S7Area.DB, 53000, 0, 230, S7WordLength.Byte, rawDataIn);
					if (result != 0)
					{
						// 예: 로그 남기기, 재연결 시도 등
					}
					Thread.Sleep(100);
				}
				else
				{
					if (!client.Connected)
					{
						int result = client.ConnectTo(_strIP, 0, 1);
						if (result == 0)
						{
							//Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.Connect);
						}
						else
						{
							//Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.NotConnect);
							
							Thread.Sleep(1000);
							continue;
						}
					}
				}

			}
		}




		private void CreateData()
		{
			rawDataIn = new Byte[m_nSize];

			for (int i = 0; i < m_nSize; i++)
			{
				rawDataIn[i] = 0;
			}
		}

		public void SetRawData(int nIndex, int nData)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return;
			if (nData < 0) return;

			rawDataIn[nIndex] = Convert.ToByte(nData);


		}
		public int GetRawData(int nIndex)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return -1;
			return rawDataIn[nIndex];

		}


		public void SetBit(int nIndex, int nBoolPos, bool bOn)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return;
			if (nBoolPos < 0 || nBoolPos >= 16) return;

			ushort data = rawDataIn[nIndex];

			if (bOn)
			{
				data |= (ushort)(1 << nBoolPos);
			}
			else
			{
				data &= (ushort)~(1 << nBoolPos);
			}

			rawDataIn[nIndex] = (Byte)data;
		}
		public bool GetBit(int nIndex, int nBitPos)
		{
			if (rawDataIn == null || nIndex < 0 || nIndex >= rawDataIn.Length)
				return false;

			byte data = rawDataIn[nIndex];
			return (data & (1 << nBitPos)) != 0;
		}





		///////////////////////////////////////////////////////////////////////////////////////////////
		public bool IsHomePosition() { return GetBit(0, 0); }
		public bool IsReadyPosition() { return GetBit(0, 1); }
		public bool IsRunOutPosition() { return GetBit(0, 2); }
		public bool IsStablePosition() { return GetBit(0, 3); }
		public bool IsMeasurementPosition() { return GetBit(0, 4); }
		public bool IsExitPosition() { return GetBit(0, 5); }
		public bool IsRestartPosition() { return GetBit(0, 6); }
		public bool IsByPassStartPosition() { return GetBit(0, 7); }



		public bool IsByPassEndPosition() { return GetBit(1, 0); }
		public bool IsReadyHLAPosition() { return GetBit(1, 1); }
		public bool IsReadyPositionCalibrationStatic() { return GetBit(1, 2); }




		public bool IsPEVDegraded() { return GetBit(2, 0); }
		public bool IsPrinterDegraded() { return GetBit(2, 1); }
		public bool IsBarcodeDegraded() { return GetBit(2, 2); }
		public bool IsLeftScrewDriverDegraded() { return GetBit(2, 3); }
		public bool IsRightScrewDriverDegraded() { return GetBit(2, 4); }
		public bool IsHLADegraded() { return GetBit(2, 5); }
		public bool IsWheelAdjustDegraded() { return GetBit(2, 6); }
		public bool IsSteeringWheelDegraded() { return GetBit(2, 7); }





		public bool IsFloatingPlateDegraded() { return GetBit(3, 0); }





		public bool IsStartCycleButton() { return GetBit(10, 0); }
		public bool IsByPassButton() { return GetBit(10, 1); }
		public bool IsRestartButton() { return GetBit(10, 2); }
		public bool IsRePrintButton() { return GetBit(10, 3); }
		public bool IsSafetyStopButton() { return GetBit(10, 4); }
		public bool IsMeasureStart() { return GetBit(10, 5); }
		public bool IsPitInLeftFinish() { return GetBit(10, 6); }
		public bool IsPitInRightFinish() { return GetBit(10, 7); }
		
		
		
		public bool IsExitPlatform() { return GetBit(11, 0); }
		public bool IsAutoMode() { return GetBit(11, 2); }
		public bool IsManualMode() { return GetBit(11, 3); }
		public bool IsRollingMaster() { return GetBit(11, 4); }
		public bool IsStaticMaster() { return GetBit(11, 5); }
		public bool IsEmegency() { return GetBit(11, 6); }


        public UInt16 GetWord(int nPos)
        {
            if (nPos < 0) return 0;
            if (nPos + 1 >= rawDataIn.Length) return 0;
            byte low = rawDataIn[nPos + 1];
            byte high = rawDataIn[nPos];
            return (ushort)((high << 8) | low);
        }




        public UInt16 GetProg() { return GetWord(22); }
		public UInt16 GetJour() { return GetWord(24); }
		public UInt16 GetIdent() { return GetWord(26); }
		public UInt16 GetDiv() { return GetWord(28); }




		public bool GetAckg1() { return GetBit(60, 0); }
		public bool GetAckg2() { return GetBit(60, 1); }
		public bool GetAckg3() { return GetBit(60, 2); }
		public bool GetAckg4() { return GetBit(60, 3); }
		public bool GetAckg5() { return GetBit(60, 4); }
		public bool GetAckg6() { return GetBit(60, 5); }
		public bool GetAckg7() { return GetBit(60, 6); }
		public bool GetAckg8() { return GetBit(60, 7); }



		public bool GetAckg9() { return GetBit(61, 0); }
		public bool GetAckg10() { return GetBit(61, 1); }
		public bool GetAckg11() { return GetBit(61, 2); }
		public bool GetAckg12() { return GetBit(61, 3); }



		public bool GetAckg13() { return GetBit(62, 0); }
		public bool GetAckg14() { return GetBit(62, 1); }
		public bool GetAckg15() { return GetBit(62, 2); }
		public bool GetAckg16() { return GetBit(62, 3); }
		public bool GetAckg17() { return GetBit(62, 4); }
		public bool GetAckg18() { return GetBit(62, 5); }


		public bool IsCenteringHome() { return GetBit(90, 0); }
		public bool IsCenteringOn() { return GetBit(90, 1); }
		public bool IsFloatingPlateLock() { return GetBit(90, 2); }
		public bool IsFloatingPlateFree() { return GetBit(90, 3); }
		public bool IsMovingPlateHome() { return GetBit(90, 4); }
		public bool IsMovingPlateAdjustPosition() { return GetBit(90, 5); }
		public bool IsSafetyRollerUp() { return GetBit(90, 6); }
		public bool IsSafetyRollerDown() { return GetBit(90, 7); }



		public bool Is3DCameraEnable() { return GetBit(91, 0); }
		public bool IsFrontCarDetection() { return GetBit(91, 1); }
		public bool IsRearCarDetection() { return GetBit(91, 2); }
		public bool IsHLAHomePosition() { return GetBit(91, 3); }
		public bool IsSteeringWheelBalanceHome() { return GetBit(91, 4); }





		public bool IsUnWrenchHome() { return GetBit(100, 0); }
		public bool IsScrewDriverLeftHome() { return GetBit(100, 1); }
		public bool IsScrewDriverRightHome() { return GetBit(100, 2); }
		public bool IsScannerLaserOn() { return GetBit(100, 3); }
		public bool IsSafetyKeyOn() { return GetBit(100, 4); }




		public bool IsWheelbaseMoving() { return GetBit(110, 0); }
		public bool IsWheelbaseHomePosition() { return GetBit(110, 1); }
		public bool IsWheelbaseInPosition() { return GetBit(110, 2); }



		public UInt16 GetLeftDistance() { return GetWord(112); }
		public UInt16 GetRightDistance() { return GetWord(114); }






		public bool IsMotorRun() { return GetBit(181, 6); }
		public bool IsMotorStop() { return GetBit(181, 7); }





		public bool IsSistemaPronto() { return GetBit(220, 0); }
		public bool IsSequnciaOK() { return GetBit(220, 1); }
		public bool IsSequnciaNotOK() { return GetBit(220, 2); }
		public bool IsHLAInitialPosition() { return GetBit(220, 3); }






	}
}

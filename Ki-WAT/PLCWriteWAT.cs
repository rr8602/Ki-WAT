using Sharp7;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_WAT
{
	public class PLCWriteWAT
	{
		uint m_nSize = 230;
		public Byte[] rawDataOut = null;


		S7Client client = new S7Client();
		private CancellationTokenSource _cts;


		private String _strIP = "";

		
		public void Create(String strIP)
		{

			_strIP = strIP;
			CreateData();

			int result = client.ConnectTo(_strIP, 0, 1);

			//if (client.Connected )
			//{
			//	Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.Connect);

			//}
			//else if (!client.Connected )
			//{
			//	Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.NotConnect);

			//}

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

		public bool GetPLCOutData()
		{
			int result = client.ReadArea(S7Area.PA, 0, 0, 229, S7WordLength.Byte, rawDataOut);

			return true;
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

			//_GV.WriteLog("START READ PLC OUT");
			while (!token.IsCancellationRequested)
			{

				if (client.Connected == true)
				{
					int result = client.ReadArea(S7Area.DB, 53001, 0, 230, S7WordLength.Byte, rawDataOut);
					if (result != 0)
					{
						// 예: 로그 남기기, 재연결 시도 등
						//_GV.WriteLog($"OUT ReadArea(PE) Error: {result}");
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
							//_GV.WriteLog("PLC OUT Reconnect Success");
						}
						else
						{
							//Broker.dsBroker.Publish(Topics.DS.PLC, Topics.DS.NotConnect);
							//_GV.WriteLog($"PLC OUT Reconnect Fail: {client.ErrorText(result)}");
							Thread.Sleep(1000);
							continue;
						}
					}
				}

			}
		}
		public int WritePLCData()
		{
			int nRet = 0;

			if (client.Connected)
			{
				// 229바이트만큼 출력 버퍼를 PLC로 전송
				nRet = client.WriteArea(
					S7Area.DB,              // 출력 영역 (Process Outputs)
					53001,                      // DB 번호 (PA는 0)
					0,                      // 시작 오프셋
					230,                    // 전송할 바이트 수
					S7WordLength.Byte,      // 단위: Byte 단위
					rawDataOut      // 보낼 데이터 버퍼
				);

				if (nRet != 0)
				{
                    //_GV.Log_PGM.WriteFile(strLog);
                    //_GV.WriteLog($"WriteArea(PA) Error: {client.ErrorText(nRet)}");
                }
			}
			else
			{
				//_GV.WriteLog("WritePLCData() failed - not connected");
			}

			return nRet;

		}

		public void CreateData()
		{


			rawDataOut = new Byte[m_nSize];

			for (int i = 0; i < m_nSize; i++)
			{
				rawDataOut[i] = 0;
			}
		}

		public void SetRawData(int nIndex, int nData)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return;
			if (nData < 0) return;


			rawDataOut[nIndex] = Convert.ToByte(nData);

		}
		public int GetRawData(int nIndex)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return -1;
			return rawDataOut[nIndex];
		}


		public void SetBit(int nIndex, int nBoolPos, bool bOn)
		{
			if (nIndex < 0 || nIndex >= m_nSize) return;
			if (nBoolPos < 0 || nBoolPos >= 16) return;

			ushort data = rawDataOut[nIndex];

			if (bOn)
			{
				data |= (ushort)(1 << nBoolPos);
			}
			else
			{
				data &= (ushort)~(1 << nBoolPos);
			}

			rawDataOut[nIndex] = (Byte)data;
		}
		public bool GetBit(int nIndex, int nBitPos)
		{
			if (rawDataOut == null || nIndex < 0 || nIndex >= rawDataOut.Length)
				return false;

			byte data = rawDataOut[nIndex];
			return (data & (1 << nBitPos)) != 0;
		}




		public void SetHomePosition(bool bOn) { SetBit(0, 0, bOn); }
		public void SetReadyPosition(bool bOn) { SetBit(0, 1, bOn); }
		public void SetRunOutPosition(bool bOn) { SetBit(0, 2, bOn); }
		public void SetStablePosition(bool bOn) { SetBit(0, 3, bOn); }
		public void SetTestPosition(bool bOn) { SetBit(0, 4, bOn); }
		public void SetExitPosition(bool bOn) { SetBit(0, 5, bOn); }
		public void SetReStartPosition(bool bOn) { SetBit(0, 6, bOn); }
		public void SetByPassStart(bool bOn) { SetBit(0, 7, bOn); }
		
		
		
		public void SetByPassEnd(bool bOn) { SetBit(1, 0, bOn); }
		public void SetReadyHLAPosition(bool bOn) { SetBit(1, 1, bOn); }





		public void SetPEVDegraded(bool bOn) { SetBit(2, 0, bOn); }
		public void SetPrinterDegraded(bool bOn) { SetBit(2, 1, bOn); }
		public void SetBarcodeDegraded(bool bOn) { SetBit(2, 2, bOn); }
		public void SetLeftScrewDriverDegraded(bool bOn) { SetBit(2, 3, bOn); }
		public void SetRightScrewDriverDegraded(bool bOn) { SetBit(2, 4, bOn); }
		public void SetHLADegraded(bool bOn) { SetBit(2, 5, bOn); }
		public void SetWheelAdjustDegraded(bool bOn) { SetBit(2, 6, bOn); }
		public void SetSteeringWheelDegraded(bool bOn) { SetBit(2, 7, bOn); }



		public void IsFloatingPlateDegraded(bool bOn) { SetBit(3, 0, bOn); }







		public void SetPrintCommunicationFail(bool bOn) { SetBit(10, 0, bOn); }
		public void SetBarcodeCommunicationFail(bool bOn) { SetBit(10, 1, bOn); }
		public void SetTraceabilityCommunicationFail(bool bOn) { SetBit(10, 2, bOn); }
		public void SetVEPCommunicationFail(bool bOn) { SetBit(10, 3, bOn); }




		public void SetTestResult(bool bOn) { SetBit(11, 0, bOn); }
		public void SetTestFail(bool bOn) { SetBit(11, 1, bOn); }
		public void Set3DCameraSensorFail(bool bOn) { SetBit(11, 4, bOn); }
		public void SetScrewDriverFail(bool bOn) { SetBit(11, 5, bOn); }
		public void SetHLAFault(bool bOn) { SetBit(11, 6, bOn); }




		public void SetCodeOK(bool bOn) { SetBit(20, 1, bOn); }





		public void SetProg(ushort nProg)
		{
			//ushort value = 0x1234;
			byte b1 = (byte)(nProg & 0xFF);       // 하위 바이트 (34)
			byte b2 = (byte)((nProg >> 8) & 0xFF); // 상위 바이트 (12)
			SetRawData(22, b2);
			SetRawData(23, b1);
		}
		public void SetJour(ushort nJour)
		{
			byte b1 = (byte)(nJour & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nJour >> 8) & 0xFF); // 상위 바이트

			SetRawData(24, b2);
			SetRawData(25, b1);
		}
		public void SetIdent(ushort nIdent)
		{
			byte b1 = (byte)(nIdent & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nIdent >> 8) & 0xFF); // 상위 바이트

			SetRawData(26, b2);
			SetRawData(27, b1);
		}
		public void SetDiv(ushort nDiv)
		{
			byte b1 = (byte)(nDiv & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDiv >> 8) & 0xFF); // 상위 바이트

			SetRawData(28, b2);
			SetRawData(29, b1);
		}




		public void SetTrainAVParallelismeHorsTolerance(bool bOn) { SetBit(60, 0, bOn); }
		public void SetTrainAVAngleDeVolantHorsToler(bool bOn) { SetBit(60, 1, bOn); }
		public void SetTrainAVCarrossageHorsToler(bool bOn) { SetBit(60, 2, bOn); }
		public void SetTrainArriereParallelismeHorsTolerance(bool bOn) { SetBit(60, 3, bOn); }
		public void SetTrainArriereAngleDeVolantHorsToler(bool bOn) { SetBit(60, 4, bOn); }
		public void SetTrainArriereCarrossageHorsToler(bool bOn) { SetBit(60, 5, bOn); }
		public void SetDefaultComPEV(bool bOn) { SetBit(60, 6, bOn); }
		public void SetAbandonCycle(bool bOn) { SetBit(60, 7, bOn); }




		public void SetPanneInstallation(bool bOn) { SetBit(61, 0, bOn); }
		public void SetVissageBancNC(bool bOn) { SetBit(61, 1, bOn); }
		public void SetTrainAVCoubleSerrageParalAvNC(bool bOn) { SetBit(61, 2, bOn); }
		public void SetTrainArriereCoubleSerrageParalAvNC(bool bOn) { SetBit(61, 3, bOn); }







		public void SetProjecteurReglageNoConforme(bool bOn) { SetBit(62, 0, bOn); }
		public void SetProjecteurNonConforme(bool bOn) { SetBit(62, 1, bOn); }
		public void SetProjecteurPreReglNC(bool bOn) { SetBit(62, 2, bOn); }
		public void SetProjecteurMQAutorisRegl(bool bOn) { SetBit(62, 3, bOn); }
		public void SetProjecteurAbandonCycle(bool bOn) { SetBit(62, 4, bOn); }
		public void SetProjecteurPanneInstallation(bool bOn) { SetBit(62, 5, bOn); }




		public void SetCenteringHome(bool bOn) { SetBit(90, 0, bOn); }
		public void SetCenteringOn(bool bOn) { SetBit(90, 1, bOn); }
		public void SetFloatingPlateLock(bool bOn) { SetBit(90, 2, bOn); }
		public void SetFloatingPlateFree(bool bOn) { SetBit(90, 3, bOn); }
		public void SetMovingPlateHome(bool bOn) { SetBit(90, 4, bOn); }
		public void SetMovingPlateAdjustPosition(bool bOn) { SetBit(90, 5, bOn); }
		public void SetSafetyRollerUp(bool bOn) { SetBit(90, 6, bOn); }
		public void SetSafetyRollerDown(bool bOn) { SetBit(90, 7, bOn); }




		public void Set3DCameraSensorEnable(bool bOn) { SetBit(91, 0, bOn); }



		public void SetWheelbaseMoving(bool bOn) { SetBit(110, 0, bOn); }
		public void SetWheelbaseHomePosition(bool bOn) { SetBit(110, 1, bOn); }
		public void SetWheelbaseInPosition(bool bOn) { SetBit(110, 2, bOn); }










		public void SetLeftHome(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트

			int nAdress = 112;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}
		public void SetLeftStaticMaster(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트


			int nAdress = 114;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}
		public void SetLeftRollingMaster(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트


			int nAdress = 116;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}
		//LEFT DIV1 - DIV10
		public void SetLeftDivDistance(int nPos, int nDistance)
		{
			if (nPos < 1) return;
			if (nPos > 10) return;

			if (nDistance < 0) return;
			if (nDistance > 4000) return;

			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트

			int nAdress = 118;
			if (nPos == 1) nAdress = nAdress + 0;
			if (nPos == 2) nAdress = nAdress + 2;
			if (nPos == 3) nAdress = nAdress + 4;
			if (nPos == 4) nAdress = nAdress + 6;
			if (nPos == 5) nAdress = nAdress + 8;
			if (nPos == 6) nAdress = nAdress + 10;
			if (nPos == 7) nAdress = nAdress + 12;
			if (nPos == 8) nAdress = nAdress + 14;
			if (nPos == 9) nAdress = nAdress + 16;
			if (nPos == 10) nAdress = nAdress + 18;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}

		public void SetRightHome(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트
			int nAdress = 138;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}
		public void SetRightStaticMaster(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트
			int nAdress = 140;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}
		public void SetRightRollingMaster(int nDistance)
		{
			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트
			int nAdress = 142;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}


		public void SetRightDivDistance(int nPos, int nDistance)
		{
			if (nPos < 1) return;
			if (nPos > 10) return;

			if (nDistance < 0) return;
			if (nDistance > 4000) return;



			byte b1 = (byte)(nDistance & 0xFF);       // 하위 바이트 
			byte b2 = (byte)((nDistance >> 8) & 0xFF); // 상위 바이트

			int nAdress = 144;
			if (nPos == 1) nAdress = nAdress + 0;
			if (nPos == 2) nAdress = nAdress + 2;
			if (nPos == 3) nAdress = nAdress + 4;
			if (nPos == 4) nAdress = nAdress + 6;
			if (nPos == 5) nAdress = nAdress + 8;
			if (nPos == 6) nAdress = nAdress + 10;
			if (nPos == 7) nAdress = nAdress + 12;
			if (nPos == 8) nAdress = nAdress + 14;
			if (nPos == 9) nAdress = nAdress + 16;
			if (nPos == 10) nAdress = nAdress + 18;

			SetRawData(nAdress + 0, b2);
			SetRawData(nAdress + 1, b1);

		}






		public void SetMotorRun(bool bOn) { SetBit(180, 0, bOn); }
		public void SetMotorStop(bool bOn) { SetBit(180, 1, bOn); }




		public void SetAccurancyCheck(bool bOn) { SetBit(190, 0, bOn); }
		public void SetMeasurementFinish(bool bOn) { SetBit(190, 1, bOn); }
		public void SetCalibrationOK(bool bOn) { SetBit(190, 2, bOn); }



		public void SetTolranceOK(bool bOn) { SetBit(200, 0, bOn); }

		public void SetVEPFinish(bool bOn) { SetBit(210, 0, bOn); }
		public void SetHLAFinish(bool bOn) { SetBit(220, 0, bOn); }

	}
}

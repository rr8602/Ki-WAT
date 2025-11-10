using Microsoft.Win32;
using NModbus;
using NModbus.Device;
using NModbus.Extensions.Enron;
using RollTester;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_WAT
{
	public partial class VEPController
	{
		TcpClient client = null;
		public VEPData vepData = new VEPData();

		ModbusFactory modbusFactory = null;
		IModbusMaster master = null;
		public MsgBroker broker = new MsgBroker();

		CancellationTokenSource readToken;
        String _IP;
        int _Port;
        bool _Connected = false;

        public bool IsConnect() 
		{ 
			if(client == null)  return false;
			if(client.Connected) return true;
			return false;
		}
		private void CancelReadToken()
		{
			if (readToken != null && !readToken.IsCancellationRequested)
			{
				try { readToken.Cancel(); }
				catch (ObjectDisposedException) { } // 이미 Dispose된 경우 무시
			}
		}
        public int Create(String IP, int Port)
        {
            int nRet = 1;
            _IP = IP;
            _Port = Port;
            try
            {

                if (master != null)
                {
                    master.Dispose();
                    master = null;
                }

                if (client != null)
                {
                    client.Close();
                    client.Dispose();
                    client = null;
                }
                modbusFactory = null;

                client = new TcpClient(IP, Port);
                modbusFactory = new ModbusFactory();
                master = modbusFactory.CreateMaster(client);

                if (client.Connected)
                {
                    Console.WriteLine($"✅ Connected to {IP}:{Port}");


                    Broker.dsBroker.Publish(Topics.DS.VEP, Topics.DS.Connect);

                }
                else
                {
                    nRet = -1;
                    Broker.dsBroker.Publish(Topics.DS.VEP, Topics.DS.NotConnect);
                }



            }
            catch (Exception ex)
            {
                Console.WriteLine($"❌ [Create] Connection failed: {ex.Message}");
                Broker.dsBroker.Publish(Topics.DS.VEP, Topics.DS.NotConnect);
                nRet = -1;
            }
            //finally
            {

                CancelReadToken();
                readToken = new CancellationTokenSource();
                Task task = Task.Run(() => TaskReadStart(readToken.Token), readToken.Token);
            }



            return nRet;
        }



        public int TaskReadStart(CancellationToken token)
        {
            int nRet = 0;



            int nSleepTime = 100;

            while (true)
            {
                if (token.IsCancellationRequested) break;

                Thread.Sleep(nSleepTime);
                try
                {


                    Thread.Sleep(nSleepTime);

                    if (client != null && client.Connected)
                    {
                        ReadDZData();

                        Thread.Sleep(nSleepTime);
                        if (vepData.SZAddr != 0 && vepData.SZSize != 0) ReadSZData();
                        Thread.Sleep(nSleepTime);
                        if (vepData.SYAddr != 0 && vepData.SYSize != 0) ReadSYData();
                        Thread.Sleep(nSleepTime);
                        if (vepData.TZAddr != 0 && vepData.TZSize != 0) ReadTZData();
                        Thread.Sleep(nSleepTime);
                        if (vepData.RZAddr != 0 && vepData.RZSize != 0) ReadRZData();

                        if (_Connected == false)
                        {
                            Broker.dsBroker.Publish(Topics.DS.VEP, Topics.DS.Connect);
                            _Connected = true;
                        }

                    }
                    else
                    {

                        if (_Connected == true)
                        {
                            Broker.dsBroker.Publish(Topics.DS.VEP, Topics.DS.NotConnect);
                            _Connected = false;
                        }

                        if (master != null)
                        {
                            master.Dispose();
                            master = null;
                        }

                        if (client != null)
                        {
                            client.Close();
                            client.Dispose();
                            client = null;
                        }
                        modbusFactory = null;
                        Thread.Sleep(500);
                        client = new TcpClient(_IP, _Port);
                        modbusFactory = new ModbusFactory();
                        master = modbusFactory.CreateMaster(client);

                        Thread.Sleep(2000);
                    }


                }
                catch (Exception ex)
                {
                    GWA.STM("READ VEP : " + ex.ToString());
                }
            }

            return nRet;
        }
        public int TaskReadStop()
		{
			int nRet = 0;
			if (readToken != null)
			{
				readToken.Cancel();    // 토큰에 취소 신호 보내기
				readToken.Dispose();   // 메모리 해제
				readToken = null;      // 다음 실행을 위해 초기화
			}
			return nRet;
		}
		private int ReadDZData()
		{
			try
			{
				ushort[] values = _ReadVEP(0, 18);


				if (values == null) return -1;
				vepData.SetDesZoneData(values);


				broker.Publish("VEP", "DZData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}
		private int ReadSZData()
		{
			try
			{
				ushort[] values = _ReadVEP(vepData.SZAddr, vepData.SZSize);


				if (values == null) return -1;


				for (int i = 0; i < values.Length; i++)
				{
					if (vepData.SZData[i] != values[i])
					{
						vepData.SZData[i] = values[i];

					}
				}

				broker.Publish("VEP", "SZData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}
		private int ReadSYData()
		{
			try
			{
				ushort[] values = _ReadVEP(vepData.SYAddr, vepData.SYSize);


				if (values == null) return -1;


				for (int i = 0; i < values.Length; i++)
				{
					if (vepData.SYData[i] != values[i])
					{
						vepData.SYData[i] = values[i];

					}
				}

				broker.Publish("VEP", "SYData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}
		private int ReadTZData()
		{
			try
			{
				ushort[] values = _ReadVEP(vepData.TZAddr, vepData.TZSize);


				if (values == null) return -1;


				for (int i = 0; i < values.Length; i++)
				{
					if (vepData.TZData[i] != values[i])
					{
						vepData.TZData[i] = values[i];

					}
				}


				if (IsOPMsgAnsi()) ReadTZOpMsg();
				else ReadTZPJI();


				broker.Publish("VEP", "TZData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}
		private int ReadTZOpMsg()
		{
			int startIndex = 12;


			int length = ((vepData.TZData[startIndex] >> 8) & 0x00FF); //문자열 길이(Low Byte) LowByte

			if (length > 0)
			{
				vepData._TZdataSize = length;


				int charPos = 0;

				// 첫 글자 (High Byte of startIndex)
				byte firstChar = (byte)(vepData.TZData[startIndex] & 0x00FF);  // 첫 문자
				if (firstChar != 0) vepData._TZcharBuffer[charPos++] = (char)firstChar;

				// 나머지 문자들
				for (int i = 1; i < length; i++)
				{
					ushort value = vepData.TZData[startIndex + i];
					byte low = (byte)(value & 0x00FF);
					byte high = (byte)((value >> 8) & 0x00FF);

					if (low != 0) vepData._TZcharBuffer[charPos++] = (char)low;
					if (high != 0) vepData._TZcharBuffer[charPos++] = (char)high;
				}

				// 버퍼가 부족하면 늘리기
				if (vepData._TZcharBuffer.Length < charPos) vepData._TZcharBuffer = new char[charPos];

				vepData.TZDataString = new string(vepData._TZcharBuffer, 0, charPos);
			}
			else
			{
				return -1;
			}
			return 0;

		}

		private int ReadTZPJI()
		{
			int startIndex = 12;


			int length = 4;

			vepData._TZdataSize = length;


			int charPos = 0;


			// 나머지 문자들
			for (int i = 0; i < length; i++)
			{
				ushort value = vepData.TZData[startIndex + i];
				byte low = (byte)(value & 0x00FF);
				byte high = (byte)((value >> 8) & 0x00FF);

				if (low != 0) vepData._TZcharBuffer[charPos++] = (char)low;
				if (high != 0) vepData._TZcharBuffer[charPos++] = (char)high;
			}

			// 버퍼가 부족하면 늘리기
			if (vepData._TZcharBuffer.Length < charPos) vepData._TZcharBuffer = new char[charPos];

			vepData.TZDataString = new string(vepData._TZcharBuffer, 0, charPos);

			return 0;
		}



		private int ReadRZData()
		{
			try
			{
				ushort[] values = _ReadVEP(vepData.RZAddr, vepData.RZSize);


				if (values == null) return -1;


				for (int i = 0; i < values.Length; i++)
				{
					if (vepData.RZData[i] != values[i])
					{
						vepData.RZData[i] = values[i];

					}
				}

				//RZData를 String으로 만들어 놓는다.
				{
					int startIndex = 12;

					int length = vepData.RZData[startIndex] & 0x00FF;  // 문자열 길이 (Low Byte)

					if (length > 0)
					{
						vepData._RZdataSize = length;

						int charPos = 0;


						// 문자들
						for (int i = 1; i < length; i++)
						{
							ushort value = vepData.RZData[startIndex + i];
							byte low = (byte)(value & 0x00FF);
							byte high = (byte)((value >> 8) & 0x00FF);

							if (low != 0) vepData._RZcharBuffer[charPos++] = (char)low;
							if (high != 0) vepData._RZcharBuffer[charPos++] = (char)high;
						}

						// 버퍼가 부족하면 늘리기
						if (vepData._RZcharBuffer.Length < charPos) vepData._RZcharBuffer = new char[charPos];

						vepData.RZDataString = new string(vepData._RZcharBuffer, 0, charPos);
					}



				}


				broker.Publish("VEP", "RZData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}

		public int ReadAddTZData()
		{
			try
			{
				ushort[] values = _ReadVEP(vepData.AddTZAddr, vepData.AddTZSize);


				if (values == null) return -1;


				for (int i = 0; i < values.Length; i++)
				{
					if (vepData.AddTZData[i] != values[i])
					{
						vepData.AddTZData[i] = values[i];

					}
				}


				//AddTZData를 String으로 만들어 놓는다.
				{
					int startIndex = 0;

					int length = vepData.AddTZData.Length;
					vepData._AddTZdataSize = length;

					int charPos = 0;

					// 첫 글자 (High Byte of startIndex)
					//byte firstChar = (byte)((vepData.AddTZData[startIndex] >> 8) & 0x00FF);
					//if (firstChar != 0) vepData._AddTZcharBuffer[charPos++] = (char)firstChar;

					// 문자들
					for (int i = 0; i < length; i++)
					{
						ushort value = vepData.AddTZData[startIndex + i];
						byte low = (byte)(value & 0x00FF);
						byte high = (byte)((value >> 8) & 0x00FF);

						if (low != 0) vepData._AddTZcharBuffer[charPos++] = (char)low;
						if (high != 0) vepData._AddTZcharBuffer[charPos++] = (char)high;
					}

					// 버퍼가 부족하면 늘리기
					if (vepData._AddTZcharBuffer.Length < charPos) vepData._AddTZcharBuffer = new char[charPos];

					vepData.AddTZDataString = new string(vepData._AddTZcharBuffer, 0, charPos);

				}


				broker.Publish("VEP", "AddTZData");
				return 0;
			}
			catch (Exception ex)
			{

				Console.WriteLine(ex.ToString());
				return -1;
			}
		}

		private ushort[] _ReadVEP(int address, int count)
		{


			int nLimit = 123;
			if (count < nLimit)
			{
				try
				{
					ushort[] registers = master.ReadHoldingRegisters(1, (ushort)address, (ushort)count);

					return registers;
				}
				catch (Exception ex)
				{
					Console.WriteLine(ex.ToString());
					return null;
				}

			}
			else
			{
				int nDoLoop = count / nLimit + 1;
				ushort[] register = new ushort[count];



				for (int i = 0; i < nDoLoop; i++)
				{
					int nPartPos = i * nLimit;

					// 마지막 구간일 경우 남은 데이터 수를 계산
					int nPartSize = Math.Min(nLimit, count - nPartPos);

					try
					{

						ushort[] part = master.ReadHoldingRegisters(1, (ushort)(address + nPartPos), (ushort)nPartSize);
						Array.Copy(part, 0, register, nPartPos, nPartSize);
					}
					catch (Exception ex)
					{
						// 여기서 로깅 또는 재시도 처리
						Console.WriteLine($"Modbus 읽기 실패: {ex.Message}");
						return null;
					}
				}
				return register;
			}

		}

		private int DirectWriteSingle(ushort uAddress, ushort uValue)
		{
			int nRet = 0;
			try
			{

				master.WriteSingleRegister(1, uAddress, uValue);
				return nRet;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}

		}

		private int DirectWriteMultiple(ushort Addr, ushort[] registers)
		{
			int nRet = 0;
			int nSize = registers.Length;

			int nLimit = 123;

			if (nSize < nLimit)
			{
				master.WriteMultipleRegisters(1, Addr, registers);
				return 1;
			}
			int nDoLoop = nSize / nLimit + 1;


			ushort nStartAddr = Addr;
			for (int i = 0; i < nDoLoop; i++)
			{
				int nPartPos = i * nLimit;

				// 마지막 구간일 경우 남은 데이터 수를 계산
				int nPartSize = Math.Min(nLimit, nSize - nPartPos);

				ushort[] part = new ushort[nPartSize];
				Array.Copy(registers, nPartPos, part, 0, nPartSize);

				// Modbus 전송
				master.WriteMultipleRegisters(1, (ushort)(Addr + nPartPos), part);

			}
			return nRet;
		}

		private ushort[] DirectReadSingle(ushort uAddress, ushort uAmount)
		{

			try
			{

				ushort[] values = master.ReadHoldingRegisters(1, uAddress, uAmount);
				return values;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return null;
			}
		}
		private int DirectReadSingle(ushort uAddress)
		{
			try
			{

				ushort[] values = master.ReadHoldingRegisters(1, uAddress, 1);

				return values[0];
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				return -1;
			}
		}


	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace Ki_WAT
{
	public partial class VEPController
	{

		private const int STEP_WAIT = 1000;
		private const int STEP_START_CYCLE = 2000;
		private const int STEP_WORK_START = 3000;



		private const int STEP_V_START = 1;
		private const int STEP_B_SPEED_START = 2;
		private const int STEP_B_SPEED_ACC = 3;
		private const int STEP_B_SPEED_STABLE = 4;
		private const int STEP_V_SPEED_RELEASE = 5;
		private const int STEP_B_SPEED_ZERO = 6;
		private const int STEP_V_FIN = 19;

		private const int STEP_TICKET =4000;





		private String _PJI = "";
		private bool m_bRun = false;
		private int m_nState = STEP_WAIT;
		public int StartSimulate(CancellationToken token)
		{
			int nRet = 0;
			broker.Publish("SIMULATE", "START Simulate");
			try
			{
				int nSleepTime = 100;

				Thread.Sleep(nSleepTime);

				m_bRun = true;

				while (m_bRun)
				{
					if (token.IsCancellationRequested) break;

					Thread.Sleep(nSleepTime);
					if (m_nState == STEP_WAIT)
					{

						nRet = _1000_Idle();
					}
					if (m_nState == STEP_START_CYCLE)
					{

						nRet = _2000_StartCycle();
					}
					if (m_nState == STEP_WORK_START)
					{

						nRet = _3000_WorkStart();
					}
					if (m_nState == STEP_V_START)
					{

						nRet = V_START();
					}
					if (m_nState == STEP_V_SPEED_RELEASE)
					{

						nRet = V_SPEED_RELEASE();
					}
					if (m_nState == STEP_V_FIN)
					{
						nRet = V_FIN();
					}
					if (m_nState == STEP_TICKET)
					{
						nRet = TICKET();
					}
					

				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}

			broker.Publish("SIMULATE", "STOP Simulate");
			return nRet;
		}
		private void SetState(int nStep)
		{
			m_nState = nStep;
		
		}
		private int _1000_Idle()
		{
			int nRet = 0;

			//VEP Status Zone을 초기화 한다.
			V_SZInit();

			//VEP Status를 1로 세팅한다. (VEP WAITING)
			V_SetVEPStatus(1);
			broker.Publish("SIMULATE", "VEP Wating ");
			SetExChange(1);
			
			
			
			//VEP 상태 체크를 한다.
			//TZ, RZExchange가 1이어야 한다.
			//Synchro가 모두 0이어야 한다.
			
			while (true)
			{

				if (m_nState != STEP_WAIT) break;

				if (m_bRun == false) break;

				if (GetTZExchange() == 1 && GetRZExchange() == 1 && IsSynchroAllZero() == true )
				{
					broker.Publish("SIMULATE", "Check End TZ/RZ Exchange = 1, Synchro All Zero ");
					break;
				}
				Thread.Sleep(100);
			}


			broker.Publish("SIMULATE", "Waiting Start Cycle from Bench");
			while (true)
			{

				if (m_nState != STEP_WAIT) break;

				if (m_bRun == false) break;
				Thread.Sleep(100);
				if (vepData.StartCycle == 1)
				{
					Thread.Sleep(100);
					SetState(STEP_START_CYCLE);
					broker.Publish("SIMULATE", "-> Recv Start Cycle");
					return -1;
				}
				Thread.Sleep(100);
			}

			return nRet;
			
		}

		private int _2000_StartCycle()
		{
			int nRet = 0;

			//VEP에 Request PJI를 기록한다.
			//Bench는 이 값을 확인하면 PJI를 보내야한다.
			V_SetRequestPJItoTZ();
			broker.Publish("SIMULATE", "<- Set Request PJI");
			while (true)
			{

				if (m_nState != STEP_WAIT) break;

				if (m_bRun == false) break;

				if (IsResponsePJIfromRZ() == true)
				{
					Thread.Sleep(100);
					_PJI = GetPJI();
					SetState(STEP_WORK_START);
					broker.Publish("SIMULATE", "-> PJI : " + _PJI);
					return -1;
				}
				Thread.Sleep(100);
			}

			return nRet;

		}
		private int _3000_WorkStart()
		{
			int nRet = 0;

			//VEP Status를 2로 세팅한다. (VEP Working)
			V_SetVEPStatus(2);
			broker.Publish("SIMULATE", "<- Set VEP Status = 2");


			while (true)
			{

				if (m_nState != STEP_WAIT) break;

				if (m_bRun == false) break;

				Thread.Sleep(1000 * 5); //5초를 쉬고 다음 스텝으로 넘어간다.
				SetState(STEP_V_START);
				break;
				
			}

			return nRet;

		}

		private int V_START()
		{
			int nRet = 0;

			//VEP Synchro V_START 번지에 값을 1로 세팅한다. 
			SetSynchro(STEP_V_START, 1);
			broker.Publish("SIMULATE", "<- Set Synchro [" + STEP_V_START.ToString() + "] : 1"  );



			//STEP_B_SPEED_START 값을 체크한다.
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;

				if (vepData.SYData[STEP_B_SPEED_START] == 1)
				{
					broker.Publish("SIMULATE", "-> Check End Synchro [" + STEP_B_SPEED_START.ToString() + "] : 1");
					break;
				}
				Thread.Sleep(100); 
				
			}

			//STEP_B_SPEED_ACC 값을 체크한다.
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;

				if (vepData.SYData[STEP_B_SPEED_ACC] == 1)
				{
					broker.Publish("SIMULATE", "-> Check End Synchro [" + STEP_B_SPEED_ACC.ToString() + "] : 1");
					break;
				}
				Thread.Sleep(100);
			}


			//STEP_B_SPEED_STABLE 값을 체크한다.
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;

				if (vepData.SYData[STEP_B_SPEED_STABLE] == 1)
				{
					broker.Publish("SIMULATE", "-> Check End Synchro [" + STEP_B_SPEED_STABLE.ToString() + "] : 1");

					Thread.Sleep(1000 * 5); //5초를 쉬고 다음 스텝으로 넘어간다.
					SetState(STEP_V_SPEED_RELEASE);
					break;
				}
				
				Thread.Sleep(100);

			}

			return nRet;

		}

		private int V_SPEED_RELEASE()
		{
			int nRet = 0;

			//VEP Synchro STEP_V_SPEED_RELEASE 번지에 값을 1로 세팅한다. 
			SetSynchro(STEP_V_START, 1);
			broker.Publish("SIMULATE", "<- Set Synchro [" + STEP_V_SPEED_RELEASE.ToString() + "] : 1");



			//STEP_B_SPEED_ZERO 값을 체크한다.
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;

				if (vepData.SYData[STEP_B_SPEED_ZERO] == 1)
				{
					broker.Publish("SIMULATE", "-> Check End Synchro [" + STEP_B_SPEED_ZERO.ToString() + "] : 1");

					Thread.Sleep(1000 * 5); //5초를 쉬고 다음 스텝으로 넘어간다.
					SetState(STEP_V_FIN);
					break;
				}

				Thread.Sleep(100);

			}

			return nRet;

		}

		private int V_FIN()
		{
			int nRet = 0;

			//VEP Synchro STEP_V_SPEED_RELEASE 번지에 값을 1로 세팅한다. 
			SetSynchro(STEP_V_START, 1);
			broker.Publish("SIMULATE", "<- Set Synchro [" + STEP_V_FIN.ToString() + "] : 1");



			//Bench에서 BecheCycle End를 기록했는지 확인한다.
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;

				if (vepData.BenchCycleEnd == 1)
				{
					broker.Publish("SIMULATE", "-> Check End Bench Cycle End ");

					SetState(STEP_TICKET);
					break;
				}

				Thread.Sleep(100);

			}

			return nRet;

		}
		private int TICKET()
		{
			int nRet = 0;

			//티켓 요청 있었는지 확인한다.

			
			broker.Publish("SIMULATE", "<- Set Ticket");
			//티켓 정보를 만들어 줘서 써야 한다.
			int nCol = vepData.TZData[16];
			int nLine = vepData.TZData[17];
			V_SetTicketToAddTZMultiple(nCol, nLine, _PJI);



			
			while (true)
			{
				if (m_nState != STEP_WAIT) break;
				if (m_bRun == false) break;


				//Ticket이 Bench에서 Print가 완료되고 나면 RZ Exchange 값을 2로 써 줘야 한다.
				if(vepData.RZ_ExchangeStatus == 2)
				{

					broker.Publish("SIMULATE", "Cycle Fin ");
					SetState(STEP_WAIT);
					break;

				}
				Thread.Sleep(100);
			}

			return nRet;

		}

	}
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{

    public struct NOTICE_MSG
    {
        public String Top;
        public String Body;
        public String Bottom;
    }

    public static class Topics
	{
		public const string Notice = "Notice";

		public static class PLC
		{ 
			public const string Data = "PLC.Data";
			public const string Error = "PLC.Error";
        }

		public static class Barcode
		{
			public const string Data = "BC.Data";
			public const string Error = "BC.Error";
		}
		public static class Cali
		{
			public const string Data = "Cali.Data";
			public const string Rate = "Cali.Rate";
			public const string Flow = "Cali.Flow";
			public const string TorqueList = "Cali.TorqueList";
			public const string Msg = "Cali.Msg";
			public const string Index = "Cali.Index";
			public const string Position = "Cali.Position";
			public const string View = "Cali.View";

		}

		public static class Test
		{
			public const string Flow = "Test.Flow";
			public const string Step = "Test.Step";
			public const string PJI = "Test.PJI";
			public const string Model = "Test.Model";
			public const string TestNo = "Test.TestNo";
			public const string GenbancData = "Test.GenbancData";
			public const string BenchMsg = "Test.BenchMsg";
			public const string VEPMsg = "Test.VEPMsg";
            public const string CycleTime = "Test.CycleTime";

        }

        public static class DS
		{
			public const string VEP = "DS.VEP";
			public const string PLC = "DS.PLC";
            public const string SWB = "DS.SWB";

            public const string Barcode = "DS.Barcode";
            public const string Printer = "DS.Printer";
            public const string Screw = "DS.Screw";


            public const string Connect = "DS.Connect";
			public const string NotConnect = "DS.NotConnect";
			public const string Run = "DS.Run";

		}

		public static class PEV
		{
			public const string MDA_OK = "PEV.MDA_OK";
			public const string Message = "PEV.Message";
        }
		

	}
	public static class Broker
	{
		public static MsgBroker barcodeBroker = new MsgBroker();
		public static MsgBroker caliBroker = new MsgBroker();
		public static MsgBroker testBroker = new MsgBroker();
		public static MsgBroker speedBroker = new MsgBroker();
		public static MsgBroker noticeBroker = new MsgBroker();
		public static MsgBroker dsBroker = new MsgBroker();
        public static MsgBroker PEVBroker = new MsgBroker();
        public static void NoticeShow(String strTop = "", String strBody = "", String strBottom = "")
        {
            //GWA.SendMessage(workerHwnd, Constants.HIDE_WINDOW, 0, 0);
            //GWA.SendMessage(Hwnd.noticeHwnd, Constants.SHOW_WINDOW, 0, 0);
            NOTICE_MSG msg = new NOTICE_MSG();
            msg.Top = strTop;
            msg.Body = strBody;
            msg.Bottom = strBottom;
            Broker.noticeBroker.Publish(Topics.Notice, msg);

        }
        public static void NoticeHide(String strTop = "", String strBody = "", String strBottom = "")
        {
            //GWA.SendMessage(workerHwnd, Constants.SHOW_WINDOW, 0, 0);
            //GWA.SendMessage(Hwnd.noticeHwnd, Constants.HIDE_WINDOW, 0, 0);
            NOTICE_MSG msg = new NOTICE_MSG();
            msg.Top = strTop;
            msg.Body = strBody;
            msg.Bottom = strBottom;
            Broker.noticeBroker.Publish(Topics.Notice, msg);

        }

    }



	public class MsgBroker
	{
		private readonly Dictionary<string, List<Action<object>>> subscribers
			= new Dictionary<string, List<Action<object>>>();
		public MsgBroker() { }
		public void Subscribe(string topic, Action<object> handler)
		{
			if (!subscribers.ContainsKey(topic))
				subscribers[topic] = new List<Action<object>>();

			subscribers[topic].Add(handler);
		}

		public void Publish(string topic, object data)
		{
			if (subscribers.ContainsKey(topic))
			{
				foreach (var handler in subscribers[topic])
					handler(data);
			}
		}
		public void Unsubscribe(string topic, Action<object> handler)
		{
			if (subscribers.ContainsKey(topic))
				subscribers[topic].Remove(handler);
		}

	}
}

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
            public const string Write = "PLC.Write";

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

            public const string SWB_ANGLE = "Test.SWB_ANGLE";

            public const string PEV_OK = "Test.PEV_OK";
            public const string SWB_OK = "Test.SWB_OK";
            public const string CENTERING_OK = "Test.CENTERING_OK";
            public const string SCREW_L_OK = "Test.SCREW_LEFT_OK";
            public const string SCREW_R_OK = "Test.CREW_RIGHT_OK";
            public const string ALLOK_INIT = "Test.ALLOK_INIT";
             


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
			//public const string MDA_OK = "PEV.MDA_OK";
   //         public const string MDA_NOK = "PEV.MDA_NOK";
   //         public const string MDA_INIT = "PEV.INIT";

            public const string Message = "PEV.Message";
		}
		public static class Notify
		{
			public const string GetDppData = "Notify.GetData";
        }

	}
	public static class Broker
	{
		public static MsgBroker barcodeBroker = new MsgBroker();
		public static MsgBroker caliBroker = new MsgBroker();
		public static MsgBroker testBroker = new MsgBroker();
		public static MsgBroker speedBroker = new MsgBroker();
		//public static MsgBroker noticeBroker = new MsgBroker();
		public static MsgBroker dsBroker = new MsgBroker();
        public static MsgBroker PEVBroker = new MsgBroker();
		public static MsgBroker NotifyBroker = new MsgBroker();

        public static MsgBroker PLCBroker = new MsgBroker();


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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
//using System.Windows.Media.Media3D;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace Ki_WAT
{

	public class LangController
	{
		public LangData desc { get; set; } = new LangData();
		public LangData Portuguese { get; set; } = new LangData();
		public LangData English { get; set; } = new LangData();
		public LangData Other { get; set; } = new LangData();

		public LangController()
		{
			desc.WAIT = "WAIT";
			desc.Onstandby = "On standby";
			desc.Scanthebarcode					= "Scan the barcode";
			desc.NoVehicleAccess				= "No Vehicle Access";
			desc.ByPass							= "By Pass";
			desc.MoveWheelbase					= "Move Wheelbase";
			desc.Drivein						= "Drive in";
			desc.LiftDown						= "Lift Down";
			desc.LiftUp							= "Lift Up";
			desc.SafetyRollerDown				= "Safety Roller Down";
			desc.SafetyRollerUp					= "Safety Roller Up";
			desc.PresstheStartbutton			= "Pressthe Start button";
			desc.CheckingVEPStation				= "Checking VEP Station";
			desc.Test							= "Test";
			desc.GenbancSequence				= "Genbanc Sequence";
			desc.Finish							= "Finish";
			desc.MoveCar						= "Move Car";
			desc.CLEANUP						= "CLEANUP";
			desc.Re_Test						= "Re-Test";
			desc.RequestPJI						= "Request PJI";
			desc.SendPJI						= "Send PJI";
			desc.SynchroValueZero				= "Synchro Value Zero";
			desc.Ready							= "Ready";
			desc.Start							= "Start";
			desc.Stop							= "Stop";
			desc.Save							= "Save";
			desc.Result							= "Result";
			desc.MotorRun						= "Motor Run";
			desc.StableTargetSpeed				= "Stable Target Speed";
			desc.SpeedReached					= "Speed Reached";
			desc.Exit							= "Exit";
			desc.Entrance						= "Entrance";
			desc.TrafficLightRed				= "Traffic Light Red";
			desc.TrafficLightGreen				= "Traffic Light Green";
			desc.DoorOpen						= "Door Open";
			desc.DoorClose						= "Door Close";
			desc.VEPStatusFault					= "VEP Status Fault";
			desc.UnknownBarcodeData				= "Unknown Barcode Data";
			desc.TIMEOUT						= "TIME OUT";
			desc.BypassOrStart					= "Press the ByPass button or the Start button";
			desc.SET_POSITION					= "SET POSITION";
			desc.READY_POSITION					= "READY POSITION";
			desc.ENTER_POSITION					= "ENTER POSITION";
			desc.TEST_POSITION					= "TEST POSITION";
			desc.EXIT_POSITION					= "EXIT POSITION";
			English = desc;
		}

	}
}



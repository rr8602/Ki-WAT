using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Ki_WAT
{
    public class ST_LOW_DATA
    {
        public byte m_nVal = 0;
        public bool[] bOn = new bool[7];
    };

    public class PLC_INPUT
    {

        // Operating Mode word 0 ~ 1Byte 0~1
        public bool     _HomePos     = false;
        public bool     _ReadyPos    = false;
        public bool     _RunoutPos   = false;
        public bool     _StablePos   = false;
        public bool     _TestPos     = false;
        public bool     _ExitPos     = false;
        public bool     _RestartPos  = false;
        public bool     _ByPassSPos  = false;
        public bool     _ByPassEPos  = false;
        public bool     _Spare_01_01 = false;
        public bool     _Spare_01_02 = false;
        public bool     _Spare_01_03 = false;
        public bool     _Spare_01_04 = false;
        public bool     _Spare_01_05 = false;
        public bool     _Spare_01_06 = false;
        public bool     _Spare_01_07 = false;

        //General mode word 4;
        public bool     _Operator_Start_Cycle;
        public bool     _Operator_ByPass_Cycle;
        public bool     _Operator_Restart_Cycle;
        public bool     _Operator_RePrint;
        public bool     _Operator_Safety_Stop;
        public bool     _Operator_Mearuing_start;
        public bool     _OperatorPit_Left_Finish;
        public bool     _OperatorPit_Right_Finish;
        public bool     _OperatorExit_Finish;
        public bool     _Spare_04_01;
        public bool     _Spare_04_02;
        public bool     _Spare_04_03;
        public bool     _Auto_Mode;
        public bool     _Manual_Mode;
        public bool     _Calibration;
        public bool     _Emergency;
        //PJI
        public byte[] _PJI = new byte[10];
        public string _strPJI = "";
        //Main body sensors
        public bool     _Centering_Home;
        public bool     _Centering_On;
        public bool     _Floating_plate_Lock;
        public bool     _Floating_plate_Free;
        public bool     _Moving_plate_Home;
        public bool     _Moving_plateOn;
        public bool     _Safety_Roller_Up;
        public bool     _Safety_Roller_Down;
        public bool     _Camera_sensor_Enable;
        public bool     _Front_Car_Detection_sensor;
        public bool     _Rear_Car_Detection_sensor;
        public bool     _HLA_Home_position;
        public bool     _Steering_wheel_balance_Home;

        public bool     _UnWrench_Tool_Home;
        public bool     _Screw_driver_L_HOME ;
        public bool     _Screw_driver_R_HOME;
        public bool     _Scanner_laser;
        public bool     _Safety_Keys_;

        public bool     _Wheelbase_Moving;
        public bool     _Wheelbase_Home_Position;
        public bool     _Wheelbase_In_Position;

        public ushort _Distance;
    }
    public class PLC_OUTPUT
    {
        public bool     _Home_pos;
        public bool     _Ready_pos;
        public bool     _Runout_pos;
        public bool     _Stable_pos;
        public bool     _Test_pos;
        public bool     _Exit_Pos;
        public bool     _Restart_pos;
        public bool     _Bypass_start;
        public bool     _Bypass_end;
                        
        public bool     _Fault_Printer;
        public bool     _Fault_Barcode;
        public bool     _Fault_Traceability;
        public bool     _Fault_VEP;
        public bool     _Fault_Test_Failed;

        public bool     _Fault_Camera_sensor;
        public bool     _Fault_ScrewDriver;
        public bool     _Fault_HLA;
        public string   strPJI = "";

        public bool     _Centering_Home;
        public bool     _Centering_ON;
        public bool     _Floating_plate_Lock;
        public bool     _Floating_plate_Free;
        public bool     _Moving_plate_Home;
        public bool     _Moving_plate_ON;
        public bool     _Safety_Roller_Up;
        public bool     _Safety_Roller_Down;
        public bool     _Camera_sensor_Enable;

        public bool     _Wheelbase_Home;
        public bool     _Wheelbase_Start;
        public bool     _Wheelbase_Stop;

        public ushort   _WBDistance;

        public bool     _Motor_Run;
        public bool     _Motor_Stop;

        // 신규 추가
        public bool _RollerBrake_Lock;
        public bool _RollerBrake_Free;

    }
    public class PLCVal
    {
        private ST_LOW_DATA[] DI_DATA = new ST_LOW_DATA[100];
        private ST_LOW_DATA[] DO_DATA = new ST_LOW_DATA[100];
        private byte[] BitA = new byte[16];
        public PLC_INPUT DI = new PLC_INPUT() ;
        public PLC_OUTPUT DO = new PLC_OUTPUT();
        public PLCVal()
        {
            for (int i = 1; i < 16; i++)
            {
                BitA[i] = (byte)(BitA[i - 1] * 2);
            }
        }
        public void DIn_Map()
        {
            DI._HomePos = DI_DATA[0].bOn[0];
            DI._ReadyPos = DI_DATA[0].bOn[1];
            DI._RunoutPos = DI_DATA[0].bOn[2];
            DI._StablePos = DI_DATA[0].bOn[3];
            DI._TestPos = DI_DATA[0].bOn[4];
            DI._ExitPos = DI_DATA[0].bOn[5];
            DI._RestartPos = DI_DATA[0].bOn[6];
            DI._ByPassSPos = DI_DATA[0].bOn[7];
            DI._ByPassEPos = DI_DATA[1].bOn[0];
            DI._Spare_01_01 = DI_DATA[1].bOn[1];
            DI._Spare_01_02 = DI_DATA[1].bOn[2];
            DI._Spare_01_03 = DI_DATA[1].bOn[3];
            DI._Spare_01_04 = DI_DATA[1].bOn[4];
            DI._Spare_01_05 = DI_DATA[1].bOn[5];
            DI._Spare_01_06 = DI_DATA[1].bOn[6];
            DI._Spare_01_07 = DI_DATA[1].bOn[7];
        }
        public void DDout_Map()
        {
            DO_DATA[0].bOn[0] = DO._Home_pos;
            DO_DATA[0].bOn[1] = DO._Ready_pos;
        }
    }
}

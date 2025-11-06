using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Ki_WAT
{
    public struct TblCarInfo
    {
        public string AcceptNo;
        public string CarPJINo;
        public string CarModel;
        public string WatCycle;
        public string LetCycle;
        public string Car_Step;
        public string TotalBar;
        public string Spare__2;
        public string Spare__3;

        public void Clear()
        {
            AcceptNo = "";
            CarPJINo = "";
            CarModel = "";
            WatCycle = "";
            LetCycle = "";
            Car_Step = "";
            TotalBar = "";
            Spare__2 = "";
            Spare__3 = "";
        }
    }

    public struct TblCarLamp
    {
        public string AcceptNo;        // 접수 번호
        public string Model_NM;        // 모델명
        public string CarPjiNo;        // PJI 번호
        public string HTstTime;        // 시험 날짜 및 시간
        public string HEndTime;        // 시험 날짜 및 시간


        public double Target_X_Left;
        public double Target_Y_Left;
        public double Target_X_Right;
        public double Target_Y_Right;

        public double Tolerance_Up_Left;
        public double Tolerance_Down_Left;
        public double Tolerance_Left_Left;
        public double Tolerance_Right_Left;

        public double Tolerance_Up_Right;
        public double Tolerance_Down_Right;
        public double Tolerance_Left_Right;
        public double Tolerance_Right_Right;


        public double LeftXVal;        // X
        public double LeftYVal;        // 좌우 99.9
        public double LeftXVal_F;        // X
        public double LeftYVal_F;        // 좌우 99.9
        public string Left_Res;        // 위치 판정 OK/NG

        public double RightXVal;        // X
        public double RightYVal;        // 좌우 99.9
        public double RightXVal_F;        // X
        public double RightYVal_F;        // 좌우 99.9
        public string Right_Res;        // 위치 판정 OK/NG

        public string LampKind;        // H or L or F
        public string HLT___PK;        // 전체 판정 OK/NG
        
        public void Clear()
        {
            AcceptNo = "";
            Model_NM = "";
            CarPjiNo = "";
            HTstTime = "";
            HEndTime = "";
            LeftXVal = 0.0;
            LeftYVal = 0.0;
            Left_Res = "";
            RightXVal = 0.0;
            RightYVal = 0.0;
            Right_Res = "";
            LampKind = "";
            HLT___PK = "";
        }
    }

    public struct TblResult
    {
        public string AcceptNo;
        public string PJI_Num;
        public string Model_NM;
        public string CycleTime;
        public string WTstTime;
        public string CarFLToe;
        public string CarFRToe;
        public string CarFTToe;
        public string CarRLToe;
        public string CarRRToe;
        public string CarRTToe;
        public string CarFLCam;
        public string CarFRCam;
        public string CarFCros;
        public string CarRLCam;
        public string CarRRCam;
        public string CarRCros;
        public string Car_Hand;
        public string CarDogRu;
        public string Car_Symm;

        public string CarFLToe_F;
        public string CarFRToe_F;
        public string CarFTToe_F;
        public string CarRLToe_F;
        public string CarRRToe_F;
        public string CarRTToe_F;
        public string CarFLCam_F;
        public string CarFRCam_F;
        public string CarFCros_F;
        public string CarRLCam_F;
        public string CarRRCam_F;
        public string CarRCros_F;
        public string Car_Hand_F;
        public string CarDogRu_F;
        public string Car_Symm_F;

        public string WAT___PK;
        public void Clear()
        {
            AcceptNo = "";
            PJI_Num = "";
            Model_NM = "";
            WTstTime = "";
            CarFLToe = "";
            CarFRToe = "";
            CarFTToe = "";
            CarRLToe = "";
            CarRRToe = "";
            CarRTToe = "";
            CarFLCam = "";
            CarFRCam = "";
            CarFCros = "";
            CarRLCam = "";
            CarRRCam = "";
            CarRCros = "";
            Car_Hand = "";
            CarDogRu = "";
            Car_Symm = "";
            WAT___PK = "";
        }

    }

    public struct TblCarModel
    {
        public string Model_NM ;
        public string Bar_Code ;
        public string WhelBase ;
        public string WB___Div ;
        public string Dpp_Code ;
        public string SWARatio;
        public string SWAOffset;
        public string ScrewDriver;
        public string Display_Unit;
        public string Spare_1;
        public string Spare_2;

        public string DogRunST;
        public string DogRunLT;
        public string HandleST;
        public string HandleLT;
            
        public string TotToef_ST;
        public string TotToef_LT;
        public string TotToer_ST;
        public string TotToer_LT;

        public string ToeFL_ST; // 기준
        public string ToeFL_AT; // 조정
        public string ToeFL_LT; // 평가
        public string ToeFR_ST;
        public string ToeFR_AT;
        public string ToeFR_LT;
        public string ToeRL_ST;
        public string ToeRL_AT;
        public string ToeRL_LT;
        public string ToeRR_ST;
        public string ToeRR_AT;
        public string ToeRR_LT;

        public string CamFL_ST; 
        public string CamFL_AT; 
        public string CamFL_LT; 
        public string CamFR_ST;
        public string CamFR_AT;
        public string CamFR_LT;
        public string CamRL_ST;
        public string CamRL_AT;
        public string CamRL_LT;
        public string CamRR_ST;
        public string CamRR_AT;
        public string CamRR_LT;
        public void Clear()
        {
            Model_NM = "";
            Bar_Code = "";
            WhelBase = "";
            Dpp_Code = "";
            SWARatio = "";
            ScrewDriver = "";
            Display_Unit = "";
            Spare_1 = "";
            Spare_2 = "";
            DogRunST = "";
            DogRunLT = "";
            HandleST = "";
            HandleLT = "";
            TotToef_ST = "";
            TotToef_LT = "";
            TotToer_ST = "";
            TotToer_LT = "";
            ToeFL_ST = ""; // 기준
            ToeFL_AT = ""; // 조정
            ToeFL_LT = ""; // 평가
            ToeFR_ST = "";
            ToeFR_AT = "";
            ToeFR_LT = "";
            ToeRL_ST = "";
            ToeRL_AT = "";
            ToeRL_LT = "";
            ToeRR_ST = "";
            ToeRR_AT = "";
            ToeRR_LT = "";
            CamFL_ST = "";
            CamFL_AT = "";
            CamFL_LT = "";
            CamFR_ST = "";
            CamFR_AT = "";
            CamFR_LT = "";
            CamRL_ST = "";
            CamRL_AT = "";
            CamRL_LT = "";
            CamRR_ST = "";
            CamRR_AT = "";
            CamRR_LT = "";
        }
    }
}

using System;
using System.Collections.Generic;
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
    }

    public struct TblResult
    {
        public string AcceptNo;
        public string PJI_Num;
        public string Model_NM;
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
        public string CarDogRun;
        public string Car_Symm;
        public string WAT___PK;
    }

    public struct TblCarModel
    {
        public string Model_NM ;
        public string Bar_Code ;
        public string WhelBase ;
        public string Dpp_Code ;
        public string SWARatio;
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
    }


}

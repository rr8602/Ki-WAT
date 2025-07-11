using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using KINT_Lib;


namespace Ki_WAT
{
    public class DB_LocalWat : DBJob
    {
        private const string TNAME_INFO = "TableCarInfo";


        public DB_LocalWat(string strPath) : base(strPath)
        {

        }

        public List<TblCarModel> SelectAllCarModels()
        {
            List<TblCarModel> list = new List<TblCarModel>();
            try
            {
                string sSQL = "SELECT * FROM TableCarModel";
                DataTable dt = GetDataSet(sSQL);
                //CloseConnection();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        TblCarModel carModel = new TblCarModel
                        {
                            Model_NM = row["Model_NM"].ToString(),
                            Bar_Code = row["Bar_Code"].ToString(),
                            WhelBase = row["WhelBase"].ToString(),
                            Dpp_Code = row["Dpp_Code"].ToString(),
                            SWARatio = row["SWARatio"].ToString(),
                            ScrewDriver = row["ScrewDriver"].ToString(),
                            Display_Unit = row["Display_Unit"].ToString(),


                            Spare_1 = row["Spare_1"].ToString(),
                            Spare_2 = row["Spare_2"].ToString(),

                            DogRunST = row["DogRunST"].ToString(),
                            DogRunLT = row["DogRunLT"].ToString(),
                            HandleST = row["HandleST"].ToString(),
                            HandleLT = row["HandleLT"].ToString(),

                            TotToef_ST = row["TotToef_ST"].ToString(),
                            TotToef_LT = row["TotToef_LT"].ToString(),
                            TotToer_ST = row["TotToer_ST"].ToString(),
                            TotToer_LT = row["TotToer_LT"].ToString(),

                            ToeFL_ST = row["ToeFL_ST"].ToString(),
                            ToeFL_AT = row["ToeFL_AT"].ToString(),
                            ToeFL_LT = row["ToeFL_LT"].ToString(),
                            ToeFR_ST = row["ToeFR_ST"].ToString(),
                            ToeFR_AT = row["ToeFR_AT"].ToString(),
                            ToeFR_LT = row["ToeFR_LT"].ToString(),
                            ToeRL_ST = row["ToeRL_ST"].ToString(),
                            ToeRL_AT = row["ToeRL_AT"].ToString(),
                            ToeRL_LT = row["ToeRL_LT"].ToString(),
                            ToeRR_ST = row["ToeRR_ST"].ToString(),
                            ToeRR_AT = row["ToeRR_AT"].ToString(),
                            ToeRR_LT = row["ToeRR_LT"].ToString(),

                            CamFL_ST = row["CamFL_ST"].ToString(),
                            CamFL_AT = row["CamFL_AT"].ToString(),
                            CamFL_LT = row["CamFL_LT"].ToString(),
                            CamFR_ST = row["CamFR_ST"].ToString(),
                            CamFR_AT = row["CamFR_AT"].ToString(),
                            CamFR_LT = row["CamFR_LT"].ToString(),
                            CamRL_ST = row["CamRL_ST"].ToString(),
                            CamRL_AT = row["CamRL_AT"].ToString(),
                            CamRL_LT = row["CamRL_LT"].ToString(),
                            CamRR_ST = row["CamRR_ST"].ToString(),
                            CamRR_AT = row["CamRR_AT"].ToString(),
                            CamRR_LT = row["CamRR_LT"].ToString(),
                        };

                        list.Add(carModel);
                    }
                }

                return list;
            }
            catch (Exception ex)
            {
                // 예외 처리 또는 로깅 가능
                throw;
            }
        }

        public TblCarModel SelectCarModel(string pModel)
        {
            try
            {
                TblCarModel carModel = new TblCarModel();
                string sSQL = $"SELECT * FROM TableCarModel WHERE Model_NM = '{pModel}'";
                DataTable dt = GetDataSet(sSQL);
                //CloseConnection();

                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];

                    carModel.Model_NM = row["Model_NM"].ToString();
                    carModel.Bar_Code = row["Bar_Code"].ToString();
                    carModel.WhelBase = row["WhelBase"].ToString();
                    carModel.Dpp_Code = row["Dpp_Code"].ToString();
                    carModel.SWARatio = row["SWARatio"].ToString();

                    carModel.ScrewDriver = row["ScrewDriver"].ToString();
                    carModel.Display_Unit = row["Display_Unit"].ToString();

                    carModel.Spare_1 = row["Spare_1"].ToString();
                    carModel.Spare_2 = row["Spare_2"].ToString();

                    carModel.DogRunST = row["DogRunST"].ToString();
                    carModel.DogRunLT = row["DogRunLT"].ToString();
                    carModel.HandleST = row["HandleST"].ToString();
                    carModel.HandleLT = row["HandleLT"].ToString();

                    carModel.TotToef_ST = row["TotToef_ST"].ToString();
                    carModel.TotToef_LT = row["TotToef_LT"].ToString();
                    carModel.TotToer_ST = row["TotToer_ST"].ToString();
                    carModel.TotToer_LT = row["TotToer_LT"].ToString();

                    carModel.ToeFL_ST = row["ToeFL_ST"].ToString();
                    carModel.ToeFL_AT = row["ToeFL_AT"].ToString();
                    carModel.ToeFL_LT = row["ToeFL_LT"].ToString();
                    carModel.ToeFR_ST = row["ToeFR_ST"].ToString();
                    carModel.ToeFR_AT = row["ToeFR_AT"].ToString();
                    carModel.ToeFR_LT = row["ToeFR_LT"].ToString();
                    carModel.ToeRL_ST = row["ToeRL_ST"].ToString();
                    carModel.ToeRL_AT = row["ToeRL_AT"].ToString();
                    carModel.ToeRL_LT = row["ToeRL_LT"].ToString();
                    carModel.ToeRR_ST = row["ToeRR_ST"].ToString();
                    carModel.ToeRR_AT = row["ToeRR_AT"].ToString();
                    carModel.ToeRR_LT = row["ToeRR_LT"].ToString();

                    carModel.CamFL_ST = row["CamFL_ST"].ToString();
                    carModel.CamFL_AT = row["CamFL_AT"].ToString();
                    carModel.CamFL_LT = row["CamFL_LT"].ToString();
                    carModel.CamFR_ST = row["CamFR_ST"].ToString();
                    carModel.CamFR_AT = row["CamFR_AT"].ToString();
                    carModel.CamFR_LT = row["CamFR_LT"].ToString();
                    carModel.CamRL_ST = row["CamRL_ST"].ToString();
                    carModel.CamRL_AT = row["CamRL_AT"].ToString();
                    carModel.CamRL_LT = row["CamRL_LT"].ToString();
                    carModel.CamRR_ST = row["CamRR_ST"].ToString();
                    carModel.CamRR_AT = row["CamRR_AT"].ToString();
                    carModel.CamRR_LT = row["CamRR_LT"].ToString();
                }

                return carModel;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                // 로그 처리 필요 시 여기에 작성
                throw;
            }
        }
        public TblCarInfo SelectCarInfo(string pAcceptNo)
        {
            try
            {
                TblCarInfo tblCarInfo = new TblCarInfo();
                string sSQL;
                sSQL = string.Format($"SELECT * FROM {TNAME_INFO} WHERE AcceptNo = '{pAcceptNo}'");

                DataTable dt = GetDataSet(sSQL);
                //CloseConnection();
                if (dt != null)
                {
                    tblCarInfo.AcceptNo = dt.Rows[0]["AcceptNo"].ToString();
                    tblCarInfo.CarVinNo = dt.Rows[0]["CarVinNo"].ToString();
                    tblCarInfo.CarHandl = dt.Rows[0]["CarHandl"].ToString();
                    tblCarInfo.Car_Rear = dt.Rows[0]["Car_Rear"].ToString();
                    tblCarInfo.TestTime = dt.Rows[0]["TestTime"].ToString();
                    tblCarInfo.VisiconN = dt.Rows[0]["VisiconN"].ToString();
                    tblCarInfo.CarModel = dt.Rows[0]["CarModel"].ToString();
                    tblCarInfo.Car_Mode = dt.Rows[0]["Car_Mode"].ToString();
                    tblCarInfo.Car_Step = dt.Rows[0]["Car_Step"].ToString();
                }
                return tblCarInfo;

            }
            catch (Exception ex)
            {
                throw;
            }
            

            
        }

        public bool UpdateCarModel(TblCarModel model)
        {
            try
            {
                // 업데이트할 데이터를 Dictionary로 구성
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Bar_Code", model.Bar_Code },
                    { "WhelBase", model.WhelBase },
                    { "Dpp_Code", model.Dpp_Code },
                    { "SWARatio", model.SWARatio },
                    { "ScrewDriver", model.ScrewDriver },
                    { "Display_Unit", model.Display_Unit },
                    { "Spare_1", model.Spare_1 },
                    { "Spare_2", model.Spare_2 },
                    { "DogRunST", model.DogRunST },
                    { "DogRunLT", model.DogRunLT },
                    { "HandleST", model.HandleST },
                    { "HandleLT", model.HandleLT },
                    { "TotToef_ST", model.TotToef_ST },
                    { "TotToef_LT", model.TotToef_LT },
                    { "TotToer_ST", model.TotToer_ST },
                    { "TotToer_LT", model.TotToer_LT },
                    { "ToeFL_ST", model.ToeFL_ST },
                    { "ToeFL_AT", model.ToeFL_AT },
                    { "ToeFL_LT", model.ToeFL_LT },
                    { "ToeFR_ST", model.ToeFR_ST },
                    { "ToeFR_AT", model.ToeFR_AT },
                    { "ToeFR_LT", model.ToeFR_LT },
                    { "ToeRL_ST", model.ToeRL_ST },
                    { "ToeRL_AT", model.ToeRL_AT },
                    { "ToeRL_LT", model.ToeRL_LT },
                    { "ToeRR_ST", model.ToeRR_ST },
                    { "ToeRR_AT", model.ToeRR_AT },
                    { "ToeRR_LT", model.ToeRR_LT },
                    { "CamFL_ST", model.CamFL_ST },
                    { "CamFL_AT", model.CamFL_AT },
                    { "CamFL_LT", model.CamFL_LT },
                    { "CamFR_ST", model.CamFR_ST },
                    { "CamFR_AT", model.CamFR_AT },
                    { "CamFR_LT", model.CamFR_LT },
                    { "CamRL_ST", model.CamRL_ST },
                    { "CamRL_AT", model.CamRL_AT },
                    { "CamRL_LT", model.CamRL_LT },
                    { "CamRR_ST", model.CamRR_ST },
                    { "CamRR_AT", model.CamRR_AT },
                    { "CamRR_LT", model.CamRR_LT }
                };
                string whereClause = $"Model_NM = '{model.Model_NM}'";
                bool result = UpdateData("TableCarModel", data, whereClause);
                //CloseConnection();
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 업데이트 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCarModel(string modelName)
        {
            try
            {
                string sSQL = $"DELETE FROM TableCarModel WHERE Model_NM = '{modelName}'";
                
                bool result = ExecuteNonQuery(sSQL);
                //CloseConnection();
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 삭제 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        public bool InsertCarModel(TblCarModel model)
        {
            try
            {
                // 삽입할 데이터를 Dictionary로 구성
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "Model_NM", model.Model_NM },
                    { "Bar_Code", model.Bar_Code },
                    { "WhelBase", model.WhelBase },
                    { "Dpp_Code", model.Dpp_Code },
                    { "SWARatio", model.SWARatio },
                    { "ScrewDriver", model.ScrewDriver },
                    { "Display_Unit", model.Display_Unit },
                    { "Spare_1", model.Spare_1 },
                    { "Spare_2", model.Spare_2 },
                    { "DogRunST", model.DogRunST },
                    { "DogRunLT", model.DogRunLT },
                    { "HandleST", model.HandleST },
                    { "HandleLT", model.HandleLT },
                    { "TotToef_ST", model.TotToef_ST },
                    { "TotToef_LT", model.TotToef_LT },
                    { "TotToer_ST", model.TotToer_ST },
                    { "TotToer_LT", model.TotToer_LT },
                    { "ToeFL_ST", model.ToeFL_ST },
                    { "ToeFL_AT", model.ToeFL_AT },
                    { "ToeFL_LT", model.ToeFL_LT },
                    { "ToeFR_ST", model.ToeFR_ST },
                    { "ToeFR_AT", model.ToeFR_AT },
                    { "ToeFR_LT", model.ToeFR_LT },
                    { "ToeRL_ST", model.ToeRL_ST },
                    { "ToeRL_AT", model.ToeRL_AT },
                    { "ToeRL_LT", model.ToeRL_LT },
                    { "ToeRR_ST", model.ToeRR_ST },
                    { "ToeRR_AT", model.ToeRR_AT },
                    { "ToeRR_LT", model.ToeRR_LT },
                    { "CamFL_ST", model.CamFL_ST },
                    { "CamFL_AT", model.CamFL_AT },
                    { "CamFL_LT", model.CamFL_LT },
                    { "CamFR_ST", model.CamFR_ST },
                    { "CamFR_AT", model.CamFR_AT },
                    { "CamFR_LT", model.CamFR_LT },
                    { "CamRL_ST", model.CamRL_ST },
                    { "CamRL_AT", model.CamRL_AT },
                    { "CamRL_LT", model.CamRL_LT },
                    { "CamRR_ST", model.CamRR_ST },
                    { "CamRR_AT", model.CamRR_AT },
                    { "CamRR_LT", model.CamRR_LT }
                };

                bool result = InsertData("TableCarModel", data);
                //CloseConnection();
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 추가 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        public TblResult SelectCarResult(string pPJI)
        {
            try
            {
                TblResult tblResult = new TblResult();
                string sSQL = $"SELECT * FROM TableCar_WAT WHERE PJI_Num = '{pPJI}'";

                DataTable dt = GetDataSet(sSQL);
                //CloseConnection();
                
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    
                    tblResult.AcceptNo = row["AcceptNo"].ToString();
                    tblResult.PJI_Num = row["PJI_Num"].ToString();
                    tblResult.Model_NM = row["Model_NM"].ToString();
                    tblResult.WTstTime = row["WTstTime"].ToString();
                    tblResult.CarFLToe = row["CarFLToe"].ToString();                    
                    tblResult.CarFRToe = row["CarFRToe"].ToString();
                    tblResult.CarFTToe = row["CarFTToe"].ToString();
                    tblResult.CarRLToe = row["CarRLToe"].ToString();
                    tblResult.CarRRToe = row["CarRRToe"].ToString();
                    tblResult.CarRTToe = row["CarRTToe"].ToString();
                    tblResult.CarFLCam = row["CarFLCam"].ToString();
                    tblResult.CarFRCam = row["CarFRCam"].ToString();
                    tblResult.CarFCros = row["CarFCros"].ToString();
                    tblResult.CarRLCam = row["CarRLCam"].ToString();
                    tblResult.CarRRCam = row["CarRRCam"].ToString();
                    tblResult.CarRCros = row["CarRCros"].ToString();
                    tblResult.Car_Hand = row["Car_Hand"].ToString();
                    tblResult.CarDogRun = row["CarDogRun"].ToString();
                    tblResult.Car_Symm = row["Car_Symm"].ToString();
                    tblResult.WAT___PK = row["WAT___PK"].ToString();
                }

                
                return tblResult;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"결과 조회 중 오류가 발생했습니다: {ex.Message}");
                throw;
            }
        }

        public bool InsertResult(TblResult result)
        {
            try
            {
                // 삽입할 데이터를 Dictionary로 구성
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "AcceptNo", result.AcceptNo },
                    { "PJI_Num", result.PJI_Num },
                    { "Model_NM", result.Model_NM },
                    { "WTstTime", result.WTstTime },
                    { "CarFLToe", result.CarFLToe },
                    { "CarFRToe", result.CarFRToe },
                    { "CarFTToe", result.CarFTToe },
                    { "CarRLToe", result.CarRLToe },
                    { "CarRRToe", result.CarRRToe },
                    { "CarRTToe", result.CarRTToe },
                    { "CarFLCam", result.CarFLCam },
                    { "CarFRCam", result.CarFRCam },
                    { "CarFCros", result.CarFCros },
                    { "CarRLCam", result.CarRLCam },
                    { "CarRRCam", result.CarRRCam },
                    { "CarRCros", result.CarRCros },
                    { "Car_Hand", result.Car_Hand },
                    { "CarDogRun", result.CarDogRun },
                    { "Car_Symm", result.Car_Symm },
                    { "WAT___PK", result.WAT___PK }
                };

                bool success = InsertData("TableResult", data);
                //CloseConnection();
                
                return success;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"결과 추가 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        public bool UpdateResult(TblResult result)
        {
            try
            {
                // 업데이트할 데이터를 Dictionary로 구성
                Dictionary<string, object> data = new Dictionary<string, object>
                {
                    { "AcceptNo", result.AcceptNo },
                    { "Model_NM", result.Model_NM },
                    { "WTstTime", result.WTstTime },
                    { "CarFLToe", result.CarFLToe },
                    { "CarFRToe", result.CarFRToe },
                    { "CarFTToe", result.CarFTToe },
                    { "CarRLToe", result.CarRLToe },
                    { "CarRRToe", result.CarRRToe },
                    { "CarRTToe", result.CarRTToe },
                    { "CarFLCam", result.CarFLCam },
                    { "CarFRCam", result.CarFRCam },
                    { "CarFCros", result.CarFCros },
                    { "CarRLCam", result.CarRLCam },
                    { "CarRRCam", result.CarRRCam },
                    { "CarRCros", result.CarRCros },
                    { "Car_Hand", result.Car_Hand },
                    { "CarDogRun", result.CarDogRun },
                    { "Car_Symm", result.Car_Symm },
                    { "WAT___PK", result.WAT___PK }
                };

                string whereClause = $"PJI_Num = '{result.PJI_Num}'";
                
                bool success = UpdateData("TableResult", data, whereClause);
                //CloseConnection();
                
                return success;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"결과 업데이트 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }

        public bool DeleteResult(string pjiNum)
        {
            try
            {
                string sSQL = $"DELETE FROM TableResult WHERE PJI_Num = '{pjiNum}'";
                
                bool result = ExecuteNonQuery(sSQL);
                //CloseConnection();
                
                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"결과 삭제 중 오류가 발생했습니다: {ex.Message}");
                return false;
            }
        }
    }
}
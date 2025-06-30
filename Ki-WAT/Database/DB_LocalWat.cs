using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using KINT_Lib;


namespace Ki_WAT
{
    internal class DB_LocalWat : DBJob
    {
        private const string TNAME_INFO = "TableCarInfo";

        public DB_LocalWat(string strPath) : base(strPath)
        {

        }

        public TblCarInfo SelectCarInfo(string pAcceptNo)
        {
            try
            {
                TblCarInfo tblCarInfo = new TblCarInfo();
                string sSQL;
                sSQL = string.Format($"SELECT * FROM {TNAME_INFO} WHERE AcceptNo = '{pAcceptNo}'");

                DataTable dt = GetDataSet(sSQL);
                CloseConnection();
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
    }
}
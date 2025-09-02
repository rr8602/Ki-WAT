using RollTester;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using RollTester;
using System.Diagnostics;
namespace Ki_WAT
{
    public partial class Frm_Oper_Test : Form
    {
        Frm_Mainfrm m_Mainfrm;
        private Frm_Operator m_Operator;
        TblCarModel m_CurModel = new TblCarModel();

        public Frm_Oper_Test()
        { 
            InitializeComponent();

        }

        public void SetOperator(Frm_Operator pOper)
        {
            m_Operator = pOper;
        }

        public void SetModel(TblCarModel pModel) 
        {
            m_CurModel = pModel;
            
            GRP_FL_TOE.SetTargetAdjust(float.Parse(pModel.ToeFL_ST), float.Parse(pModel.ToeFL_AT), float.Parse(pModel.ToeFL_LT));
            GRP_FR_TOE.SetTargetAdjust(float.Parse(pModel.ToeFR_ST), float.Parse(pModel.ToeFR_AT), float.Parse(pModel.ToeFR_LT));
            GRP_RL_TOE.SetTargetAdjust(float.Parse(pModel.ToeRL_ST), float.Parse(pModel.ToeRL_AT), float.Parse(pModel.ToeRL_LT));
            GRP_RR_TOE.SetTargetAdjust(float.Parse(pModel.ToeRR_ST), float.Parse(pModel.ToeRR_AT), float.Parse(pModel.ToeRR_LT));

            GRP_FL_CAM.SetTargetAdjust(float.Parse(pModel.CamFL_ST), float.Parse(pModel.CamFL_AT), float.Parse(pModel.CamFL_LT));
            GRP_FR_CAM.SetTargetAdjust(float.Parse(pModel.CamFR_ST), float.Parse(pModel.CamFR_AT), float.Parse(pModel.CamFR_LT));
            GRP_RL_CAM.SetTargetAdjust(float.Parse(pModel.CamRL_ST), float.Parse(pModel.CamRL_AT), float.Parse(pModel.CamRL_LT));
            GRP_RR_CAM.SetTargetAdjust(float.Parse(pModel.CamRR_ST), float.Parse(pModel.CamRR_AT), float.Parse(pModel.CamRR_LT));


            GRP_FL_TOE.SetTarget(float.Parse(pModel.ToeFL_ST), 60);
            GRP_FR_TOE.SetTarget(float.Parse(pModel.ToeFR_ST), 60);
            GRP_RL_TOE.SetTarget(float.Parse(pModel.ToeRL_ST), 60);
            GRP_RR_TOE.SetTarget(float.Parse(pModel.ToeRR_ST), 60);

            GRP_FL_CAM.SetTarget(float.Parse(pModel.CamFL_ST), 60);
            GRP_FR_CAM.SetTarget(float.Parse(pModel.CamFR_ST), 60);
            GRP_RL_CAM.SetTarget(float.Parse(pModel.CamRL_ST), 60);
            GRP_RR_CAM.SetTarget(float.Parse(pModel.CamRR_ST), 60);

            Debug.Print("");
                
        }

        public void SetMainFrm(Frm_Mainfrm pMain)
        {
            m_Mainfrm = pMain;
            m_Mainfrm.OnDppDataReceived += OnReceiveDpp;
        }
        public void OnReceiveDpp(MeasureData pData)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnReceiveDpp(pData)));
                return;
            }

            //lbl_Cam_FL.Text = pData.dCamFL.ToString("F1");
            //lbl_Cam_FR.Text = pData.dCamFR.ToString("F1");
            //lbl_Cam_RL.Text = pData.dCamRL.ToString("F1");
            //lbl_Cam_RR.Text = pData.dCamRR.ToString("F1");

            //lbl_Toe_FL.Text = pData.dToeFL.ToString("F1");
            //lbl_Toe_FR.Text = pData.dToeFR.ToString("F1");
            //lbl_Toe_RL.Text = pData.dToeRL.ToString("F1");
            //lbl_Toe_RR.Text = pData.dToeRR.ToString("F1");

            GRP_FL_TOE.SetValue(pData.dToeFL);
            GRP_FR_TOE.SetValue(pData.dToeFR);
            GRP_RL_TOE.SetValue(pData.dToeRL);
            GRP_RR_TOE.SetValue(pData.dToeRR);

            GRP_FL_CAM.SetValue(pData.dCamFL);
            GRP_FR_CAM.SetValue(pData.dCamFR);
            GRP_RL_CAM.SetValue(pData.dCamRL);
            GRP_RR_CAM.SetValue(pData.dCamRR);


            RedrawGauge(GRP_FL_TOE, (float)pData.dToeFL);
            RedrawGauge(GRP_FR_TOE, (float)pData.dToeFR);
        }
        public void RedrawGauge(CGuage pGauge, float fValue)
        {
            if ( fValue < 13 ) 
            {
                pGauge.SetRange(20);
            }
            
            
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Frm_Operator parent = (Frm_Operator)this.MdiParent;
            parent.ShowFrm(0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            MeasureData pData = new MeasureData();
            pData.dToeFL = 10;
            pData.dToeFR = 20;
            pData.dToeRL = 30;
            pData.dToeRR = 40;

            pData.dCamFL = 15;
            pData.dCamFR = 25;
            pData.dCamRL = 35;
            pData.dCamRR = 45;

            OnReceiveDpp(pData);


        }
    }
}

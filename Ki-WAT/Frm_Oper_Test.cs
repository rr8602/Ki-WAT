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
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
namespace Ki_WAT
{
    public partial class Frm_Oper_Test : Form
    {
        Frm_Mainfrm m_Mainfrm;
        private Frm_Operator m_Operator;
        TblCarModel m_CurModel = new TblCarModel();
        GlobalVal _GV = GlobalVal.Instance;
        private const int GRAPH_TYPE_TOE_FL = 100;
        private const int GRAPH_TYPE_TOE_FR = 200;
        private const int GRAPH_TYPE_TOE_RL = 300;
        private const int GRAPH_TYPE_TOE_RR = 400;

        private const int GRAPH_TYPE_CAM_FL = 500;
        private const int GRAPH_TYPE_CAM_FR = 600;
        private const int GRAPH_TYPE_CAM_RL = 700;
        private const int GRAPH_TYPE_CAM_RR = 800;

        public Frm_Oper_Test()
        { 
            InitializeComponent();
        }

        private void Frm_Oper_Test_Load(object sender, EventArgs e)
        {
            Broker.dsBroker.Subscribe(Topics.PEV.MDA_OK, SetMDA);
            Broker.dsBroker.Subscribe(Topics.PEV.Message, SetPEVMessage);
        }
        private void SetPEVMessage(object data)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => SetPEVMessage(data)));
                return;
            }
            string strMessage = data as string;
            lbl_PEV.Text = strMessage;
        }
        private void SetMDA(object data)
        {
            lbl_PEV.BackColor = Color.LimeGreen;
            //SafeLoad(pic_lamp_io, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
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

            GRP_FL_CAM.SetTarget(float.Parse(pModel.CamFL_ST), 120);
            GRP_FR_CAM.SetTarget(float.Parse(pModel.CamFR_ST), 120);
            GRP_RL_CAM.SetTarget(float.Parse(pModel.CamRL_ST), 120);
            GRP_RR_CAM.SetTarget(float.Parse(pModel.CamRR_ST), 120);
            Debug.Print("");
        }
        public void SetMainFrm(Frm_Mainfrm pMain)
        {
            m_Mainfrm = pMain;
            m_Mainfrm.OnDppDataReceived += OnReceiveDpp;
            TestThread_HLT.OnStatusUpdate += OnStatusUpdate;
        }
        public void OnStatusUpdate(string strStatus)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnStatusUpdate(strStatus)));
                return;
            }
            lbl_hlt_message.Text = strStatus;
        }

        public void OnReceiveDpp(MeasureData pData)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnReceiveDpp(pData)));
                return;
            }
            RedrawGauge((float)pData.dToeFL, GRAPH_TYPE_TOE_FL);
            RedrawGauge((float)pData.dToeFR, GRAPH_TYPE_TOE_FR);
            RedrawGaugeNoMove((float)pData.dToeRL, GRAPH_TYPE_TOE_RL);
            RedrawGaugeNoMove((float)pData.dToeRR, GRAPH_TYPE_TOE_RR);

            RedrawGaugeNoMove((float)pData.dCamFL, GRAPH_TYPE_CAM_FL);
            RedrawGaugeNoMove((float)pData.dCamFR, GRAPH_TYPE_CAM_FR);
            RedrawGaugeNoMove((float)pData.dCamRL, GRAPH_TYPE_CAM_RL);
            RedrawGaugeNoMove((float)pData.dCamRR, GRAPH_TYPE_CAM_RR);
            
            lbl_SWB.Text = _GV.dHandle.ToString("F1");
        }
        public void RedrawGauge(float fValue, int nType)
        {
            CGuage pGauge = null;
            float targetValue = 0 ;
            float nBigScale = 10;
            
            double dST = 0;
            double dLT = 0;
            double dAT = 0;
            switch (nType)
            {
                case GRAPH_TYPE_TOE_FL:
                    pGauge = GRP_FL_TOE;
                    targetValue = float.Parse(m_CurModel.ToeFL_ST);
                    dST = Convert.ToDouble(_GV.m_Cur_Model.ToeFL_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.ToeFL_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.ToeFL_AT);
                    break;
                case GRAPH_TYPE_TOE_FR:
                    pGauge = GRP_FR_TOE;
                    targetValue = float.Parse(m_CurModel.ToeFR_ST);
                    dST = Convert.ToDouble(_GV.m_Cur_Model.ToeFR_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.ToeFR_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.ToeFR_AT);
                    break;
            }
            pGauge.ValueColor = GetColor(fValue, dST, dAT, dLT);
            
            pGauge.SetValue(fValue);
            float minRange = targetValue - nBigScale;
            float maxRange = targetValue + nBigScale;
            pGauge.SetTarget(targetValue, (fValue > minRange && fValue < maxRange) ? nBigScale : 60);
        }
        public Color GetColor(double dValue, double dST, double dAT, double dLT)
        {
            double diff = Math.Abs(dValue - dST);

            if (diff <= dAT)
            {
                return Color.Green;   // ±AT 이내 → Green
            }
            else if (diff <= dLT)
            {
                return Color.Yellow;  // ±LT 이내 → Yellow
            }
            else
            {
                return Color.Red;     // 그 밖은 Red
            }
        }

        public void RedrawGaugeNoMove(float fValue, int nType)
        {
            CGuage pGauge = null;

            
            double dST = 0;
            double dLT = 0;
            double dAT = 0;

            switch (nType)
            {
                case GRAPH_TYPE_CAM_FL:
                    pGauge = GRP_FL_CAM;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.CamFL_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.CamFL_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.CamFL_AT);
                    break;
                case GRAPH_TYPE_CAM_FR:
                    pGauge = GRP_FR_CAM;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.CamFR_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.CamFR_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.CamFR_AT);
                    break;
                case GRAPH_TYPE_CAM_RL:
                    pGauge = GRP_RL_CAM;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.CamRL_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.CamRL_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.CamRL_AT);
                    break;
                case GRAPH_TYPE_CAM_RR:
                    pGauge = GRP_RR_CAM;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.CamRR_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.CamRR_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.CamRR_AT);
                    break;
                case GRAPH_TYPE_TOE_RL:
                    pGauge = GRP_RL_TOE;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.ToeRL_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.ToeRL_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.ToeRL_AT);
                    break;
                case GRAPH_TYPE_TOE_RR:
                    pGauge = GRP_RR_TOE;
                    dST = Convert.ToDouble(_GV.m_Cur_Model.ToeRR_ST);
                    dLT = Convert.ToDouble(_GV.m_Cur_Model.ToeRR_LT);
                    dAT = Convert.ToDouble(_GV.m_Cur_Model.ToeRR_AT);
                    break;
            }

            pGauge.ValueColor = GetColor(fValue, dST, dAT, dLT);
            pGauge.SetValue(fValue);
        }



        private void button1_Click(object sender, EventArgs e)
        {
            Frm_Operator parent = (Frm_Operator)this.MdiParent;
            parent.ShowFrm(0);
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            //float nVal = float.Parse(textBox1.Text);
            RedrawGauge((float)30.5, GRAPH_TYPE_TOE_FL);

            GRP_FL_TOE.ValueColor = Color.Red;
            //MeasureData pData = new MeasureData();
            //pData.dToeFL = 10;
            //pData.dToeFR = 20;
            //pData.dToeRL = 30;
            //pData.dToeRR = 40;

            //pData.dCamFL = 15;
            //pData.dCamFR = 25;
            //pData.dCamRL = 35;
            //pData.dCamRR = 45;

            //OnReceiveDpp(pData);


        }

       
    }
}

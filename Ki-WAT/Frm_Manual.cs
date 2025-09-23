using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Ki_WAT
{
    struct LET_Param
    {
        public string uid ;
        public int    vsn;
        public string vin;
        public string line;
        public double floorpitch;
        
    }

    public partial class Frm_Manual : Form
    {

        GlobalVal _GV = GlobalVal.Instance;
        private Frm_Mainfrm m_frmParent;

        LET_Param m_LetParam = new LET_Param();
        public Frm_Manual()
        {
            InitializeComponent();
        }

        private void Frm_Manual_Load(object sender, EventArgs e)
        {
            
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
            m_frmParent.m_SWBComm._updateDisplayValues += UpdateDisplayValues;
        }

        private void UpdateDisplayValues(string fnd, string sensorAd, string boardAngle, string pcAngle)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateDisplayValues(fnd, sensorAd, boardAngle, pcAngle)));
                return;
            }

            lbl_Type.Text = fnd;
            lbl_Sen_AD.Text = sensorAd;
            lbl_Angle.Text = boardAngle;
            lbl_PC_Angle.Text = pcAngle;
        }

        private void Btn_Last_Error_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Txt_Log.Text = "";
        }


        private void GetLetParamFromUI()
        {
            m_LetParam.uid = Txt_UID.Text;
            m_LetParam.line = "LineA";
            m_LetParam.vin = Txt_VIN.Text;
            m_LetParam.vsn = Int32.Parse(Txt_VSN.Text);
            m_LetParam.floorpitch = double.Parse(Txt_FLOR.Text);

        }
        private void Btn_Startcycle_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.StartCycle(m_LetParam.uid);
        }

        private void Btn_Last_Error_Click_2(object sender, EventArgs e)
        {
            string sRes = _GV.LET_Controller.GetLastResult();
            Txt_Log.Text = sRes;

            LampInclination ResData = _GV.LET_Controller.ParseInclinationFromXml(sRes);

            lbl_Res_LX.Text = ResData.InclinationXFinal_Left.ToString();
            lbl_Res_LY.Text = ResData.InclinationYFinal_Left.ToString();

            lbl_Res_RX.Text = ResData.InclinationXFinal_Right.ToString();
            lbl_Res_RY.Text = ResData.InclinationYFinal_Right.ToString();

        }

        private void Btn_VehicleSel_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.VehicleSelection(m_LetParam.vsn, m_LetParam.vin);

        }

        private void Btn_perfoamTest_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.PerformTests(m_LetParam.line, m_LetParam.floorpitch);
        }

        private void Btn_Endcycle_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.EndCycle();
        }

        private void Btn_Minus_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adMinusPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Zero_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adZeroPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Plus_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adPlusPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            _GV.Config.SaveSWB();
        }

      
    }
}

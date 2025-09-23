using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public partial class FrmSimulator : Form
    {
        GlobalVal _GV = GlobalVal.Instance;
        Frm_Mainfrm m_Parent = null;
        public FrmSimulator(Frm_Mainfrm pMain)
        {
            InitializeComponent();
            m_Parent = pMain;
        }

        private void FrmSimulator_Load(object sender, EventArgs e)
        {
            Txt_Cam_FL.Text = "0";
            Txt_Cam_FR.Text = "0";
            Txt_Cam_RL.Text = "0";
            Txt_Cam_RR.Text = "0";

            Txt_Toe_FL.Text = "0";
            Txt_Toe_FR.Text = "0";
            Txt_Toe_RL.Text = "0";
            Txt_Toe_RR.Text = "0";

            Txt_TA.Text = "0";
            Txt_Symm.Text = "0";
            Txt_Swb.Text = "0";
            
        }


        private void Btn_Toe_Apply_Click(object sender, EventArgs e)
        {
            
            MeasureData dppData = new MeasureData() ;

            dppData.dToeFL = double.Parse(Txt_Toe_FL.Text);
            dppData.dToeFR = double.Parse(Txt_Toe_FR.Text);
            dppData.dToeRL = double.Parse(Txt_Toe_RL.Text);
            dppData.dToeRR = double.Parse(Txt_Toe_RR.Text);

            dppData.dCamFL = double.Parse(Txt_Cam_FL.Text);
            dppData.dCamFR = double.Parse(Txt_Cam_FR.Text);
            dppData.dCamRL = double.Parse(Txt_Cam_RL.Text);
            dppData.dCamRR = double.Parse(Txt_Cam_RR.Text);

            dppData.dTA = double.Parse(Txt_TA.Text);
            dppData.dSymm = double.Parse(Txt_Symm.Text);
            //dppData.dHandle = double.Parse(Txt_Swb.Text);
            _GV.dHandle = double.Parse(Txt_Swb.Text);

            m_Parent.SetDppData(dppData);
        }

        private void Btn_Toe_Zero_Click(object sender, EventArgs e)
        {
            Txt_Cam_FL.Text = "0";
            Txt_Cam_FR.Text = "0";
            Txt_Cam_RL.Text = "0";
            Txt_Cam_RR.Text = "0";

            Txt_Toe_FL.Text = "0";
            Txt_Toe_FR.Text = "0";
            Txt_Toe_RL.Text = "0";
            Txt_Toe_RR.Text = "0";

            Txt_TA.Text = "0";
            Txt_Symm.Text = "0";
            Txt_Swb.Text = "0";


        }

        private void Btn_Next_Step_Click(object sender, EventArgs e)
        {
            _GV.m_bNextStep = true;
        }
    }
}

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
    public partial class Frm_StaticMaster : Form
    {
        Frm_Mainfrm m_MainFrm;
        public Frm_StaticMaster()
        {
            InitializeComponent();
        }

        public void SetParent(Frm_Mainfrm pMain)
        {
            m_MainFrm = pMain;
            m_MainFrm.OnDppDataReceived += OnReceiveDpp;
        }

        public void OnReceiveDpp(MeasureData pData)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnReceiveDpp(pData)));
                return;
            }

            lbl_Cam_FL.Text = pData.dCamFL.ToString("F1");
            lbl_Cam_FR.Text = pData.dCamFR.ToString("F1");
            lbl_Cam_RL.Text = pData.dCamRL.ToString("F1");
            lbl_Cam_RR.Text = pData.dCamRR.ToString("F1");

            lbl_Toe_FL.Text = pData.dToeFL.ToString("F1");
            lbl_Toe_FR.Text = pData.dToeFR.ToString("F1");
            lbl_Toe_RL.Text = pData.dToeRL.ToString("F1");
            lbl_Toe_RR.Text = pData.dToeRR.ToString("F1");

        }

        private void Btn_Calc_Click(object sender, EventArgs e)
        {

        }

        private void Btn_Verify_Click(object sender, EventArgs e)
        {

        }
    }
}

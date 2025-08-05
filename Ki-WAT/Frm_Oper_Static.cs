using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;

namespace Ki_WAT
{
    public partial class Frm_Oper_Static : Form
    {

        Frm_Mainfrm m_Mainfrm;
        Frm_Operator m_Operator;

        public Frm_Oper_Static()
        {
            InitializeComponent();
        }

        public void SetOperator(Frm_Operator pOper)
        {
            m_Operator = pOper;
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

            lbl_Cam_FL.Text = pData.dCamFL.ToString("F1");
            lbl_Cam_FR.Text = pData.dCamFR.ToString("F1");
            lbl_Cam_RL.Text = pData.dCamRL.ToString("F1");
            lbl_Cam_RR.Text = pData.dCamRR.ToString("F1");

            lbl_Toe_FL.Text = pData.dToeFL.ToString("F1");
            lbl_Toe_FR.Text = pData.dToeFR.ToString("F1");
            lbl_Toe_RL.Text = pData.dToeRL.ToString("F1");
            lbl_Toe_RR.Text = pData.dToeRR.ToString("F1");
        }

    }
}

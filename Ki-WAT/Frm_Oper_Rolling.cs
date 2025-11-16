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
    public partial class Frm_Oper_Rolling : Form
    {
        public Frm_Oper_Rolling()
        {
            InitializeComponent();
        }

        private void Frm_Oper_Rolling_Load(object sender, EventArgs e)
        {

            Broker.NotifyBroker.Subscribe(Topics.Notify.GetDppData, GetDppData);
        }

        private void GetDppData(object obj)
        {

            MeasureData pData = (MeasureData)obj;

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => GetDppData(obj)));
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

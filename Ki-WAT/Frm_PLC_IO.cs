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
    public partial class Frm_PLC_IO : Form
    {
        GlobalVal _GV = GlobalVal.Instance;
        public Frm_PLC_IO()
        {
            InitializeComponent();
        }
        private void MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }
        public void RefreshOutPut()
        {
            byte[] byData = _GV.plcRead.rawDataIn;
            for ( int ni = 0; ni < 8; ni++)
            {
                CheckBox chkBox = this.Controls.Find("chk_Output_" + ni.ToString(), true).FirstOrDefault() as CheckBox;
                if (chkBox != null)
                {
                    chkBox.Checked = GWA.GetBit(byData[1], ni);
                }
            }
            GWA.GetBit(byData[0], 0);
        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }
    }
}

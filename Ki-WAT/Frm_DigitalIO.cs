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
    public partial class Frm_DigitalIO : Form
    {

        Frm_DigitalIOIn ioInDlg = null;
        Frm_DigitalIOOut ioOutDlg = null;

        public Frm_DigitalIO()
        {
            InitializeComponent();
        }

        private void Frm_DigitalIO_Load(object sender, EventArgs e)
        {
            CreateDlgs();

            ioInDlg.Show();

        }
        private void CreateDlgs()
        {
            int nWidth = 40;
            ioInDlg = new Frm_DigitalIOIn();
            ioInDlg.TopLevel = false;
            this.Controls.Add(ioInDlg);
            ioInDlg.StartPosition = FormStartPosition.Manual;
            ioInDlg.Location = new Point(10, 50);
            ioInDlg.Width = Constants.SubViewWidth - nWidth;
            ioInDlg.Height = Constants.SubViewHeight - 60;

            ioOutDlg = new Frm_DigitalIOOut();
            ioOutDlg.TopLevel = false;
            this.Controls.Add(ioOutDlg);
            ioOutDlg.StartPosition = FormStartPosition.Manual;
            ioOutDlg.Location = new Point(10, 50);
            ioOutDlg.Width = Constants.SubViewWidth - nWidth;
            ioOutDlg.Height = Constants.SubViewHeight - 60;
        }

        private void btnIn_Click(object sender, EventArgs e)
        {
            if (ioInDlg != null) ioInDlg.Show();
            if (ioOutDlg != null) ioOutDlg.Hide();

        }

        private void btnOut_Click(object sender, EventArgs e)
        {
            if (ioInDlg != null) ioInDlg.Hide();
            if (ioOutDlg != null) ioOutDlg.Show();

        }
    }
}

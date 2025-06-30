using System;
using System.Collections;
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
    public partial class Frm_Config : Form
    {
        Frm_Mainfrm m_frmParent = null;
        public Frm_Config()
        {
            InitializeComponent();
        }
        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void Frm_Config_Load(object sender, EventArgs e)
        {
            modelList.Columns.Add("", 0);
            modelList.Columns.Add("MODEL", modelList.Width , HorizontalAlignment.Center);
        }
    }
}
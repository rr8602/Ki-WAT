﻿using System;
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

        private void button1_Click(object sender, EventArgs e)
        {
            Frm_Operator parent = (Frm_Operator)this.MdiParent;
            parent.ShowFrm(0);


        }
    }
}

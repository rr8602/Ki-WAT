using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DG_ComLib;

namespace Ki_WAT
{
    public partial class Frm_Main : Form
    {
        Frm_Mainfrm m_frmParent;

        public static string LAMP_GREEN = @"Resource\green.png";
        public static string LAMP_GRAY = @"Resource\GRAY.png";
        public static string LAMP_RED = @"Resource\RED.png";

        private delegate void _d_SetStatusListMsg(string strText);

        public Frm_Main()
        {
            InitializeComponent();
        }
        private void Frm_Main_Load(object sender, EventArgs e)
        {

            seqList.Columns.Add("", 0);
            seqList.Columns.Add("BARCODE", seqList.Width /2 , HorizontalAlignment.Center);
            seqList.Columns.Add("PJI", seqList.Width / 2, HorizontalAlignment.Center);
            


            for ( int ni = 0; ni < 4; ni++)
            {
                ListViewItem item1 = new ListViewItem(ni.ToString());

                item1.SubItems.Add("1");
                item1.SubItems.Add("2");

                seqList.Items.Add(item1);
            }
            
            
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _Log_ListBox("asdf");
        }

        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            
            Console.WriteLine("");
        }

        public void _Log_ListBox(string strText)
        {
            try
            {
                if (lb_Message.InvokeRequired)
                {
                    _d_SetStatusListMsg d = new _d_SetStatusListMsg(_Log_ListBox);
                    this.Invoke(d, new object[] { strText });
                }
                else
                {
                    lb_Message.Items.Insert(0, "[" + System.DateTime.Now.ToString("HH:mm:ss") + "] " + strText);

                    DG_Log.SysLog(strText);

                    if (lb_Message.Items.Count >= 1000)
                        lb_Message.Items.RemoveAt(lb_Message.Items.Count - 1);
                }
            }
            catch (Exception ex)
            {
                DG_Log.SysLog("[_Log_ListBox ] <" + strText + ">  ERROR : " + ex.Message);
            }
        }

    }
}

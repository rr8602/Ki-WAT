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
            seqList.Columns.Add("UID", seqList.Width / 2 , HorizontalAlignment.Center);
            seqList.Columns.Add("PJI", seqList.Width / 2, HorizontalAlignment.Center);

            RefreshCarInfoList();
        }

        public void RefreshCarInfoList()
        {
            if (m_frmParent == null || m_frmParent.m_dbJob == null)
                return;

            try
            {
                // 오늘 날짜 yyyyMMdd
                string today = DateTime.Today.ToString("yyyyMMdd");
                string sSQL = $"SELECT * FROM TableCarInfo WHERE AcceptNo LIKE '{today}%' ORDER BY AcceptNo DESC";
                var dt = m_frmParent.m_dbJob.GetDataSet(sSQL);

                seqList.Items.Clear();

                if (dt != null && dt.Rows.Count > 0)
                {
                    foreach (DataRow row in dt.Rows)
                    {
                        string acceptNo = row["AcceptNo"].ToString();
                        string carPjiNo = row["CarPJINo"].ToString();

                        ListViewItem item = new ListViewItem(acceptNo);
                        item.SubItems.Add(acceptNo); // UID
                        item.SubItems.Add(carPjiNo); // PJI
                        seqList.Items.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"CarInfo 리스트 새로고침 중 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
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

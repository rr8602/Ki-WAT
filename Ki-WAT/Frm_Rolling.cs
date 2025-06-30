
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public partial class Frm_Rolling : Form
    {
        private volatile bool m_bTestRun = false ;
        private volatile bool m_bStopRequested = false;
        private volatile int m_nCnt = 0;
        private volatile int m_nRunOutTime = 0;
        private volatile int m_nIdleTime = 5;



        private Thread m_Thread = null;
        public Frm_Rolling()
        {
            InitializeComponent();
        }
        private void Frm_Rolling_Load(object sender, EventArgs e)
        {
            ResList.Columns.Add("", 0);
            ResList.Columns.Add("No.", ResList.Width / 9, HorizontalAlignment.Center);

            ResList.Columns.Add("FL Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("FR Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RL Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RR Toe", ResList.Width / 9, HorizontalAlignment.Center);

            ResList.Columns.Add("FL Camber", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("FR Camber", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RL Camber", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RR Camber", ResList.Width / 9, HorizontalAlignment.Center);


            text_cnt.Text = "3";
            text_time.Text = "15";

            rd_fw.Checked = true;

            LabelTransparent(pictureBox1, lbl_Toe_FL);
            LabelTransparent(pictureBox1, lbl_Toe_FR);
            LabelTransparent(pictureBox1, lbl_Cam_FL);
            

        }

        private void LabelTransparent(PictureBox pPictureBox, Label pLabel)
        {
            Point screenPos = pLabel.PointToScreen(Point.Empty);

            pLabel.Parent = pPictureBox;
            pLabel.BackColor = Color.Transparent;
            pLabel.AutoSize = false;
            pLabel.Location = pPictureBox.PointToClient(screenPos);
            pLabel.BringToFront();
        }

        private void Btn_Init_Click(object sender, EventArgs e)
        {
            ResList.Items.Clear();
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            if (rd_bw.Checked)
            {
                lblMessage.Text = "Backward Measure 1";
            }
            else
            {
                lblMessage.Text = "Foward Measure 1";
            }

            if (text_cnt.Text == "" || text_time.Text == "")
            {
                MessageBox.Show("Check the value");
            }
         
            TestRun(Int32.Parse(text_cnt.Text), Int32.Parse(text_time.Text));

        }
        public void TestRun(int nCnt, int nRunoutTime)
        {
            m_nCnt = nCnt;
            m_nRunOutTime = nRunoutTime;
            if (m_bTestRun) return;

            m_Thread = new Thread(MeasureStart);

            m_Thread.Start();
        }


        private void SleepWithAbortCheck(int milliseconds, IProgress<int> progress)
        {
            int interval = 100;
            int elapsed = 0;

            while (elapsed <= milliseconds)
            {
                if (m_bStopRequested) break;

                Thread.Sleep(interval);
                elapsed += interval;

                progress?.Report(elapsed); // null 체크 후 보고
            }
        }

        private void UpdateTimeRunOutTime(double dTime)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    //strTime

                }));
            }
        }
            



        private void UpdateToeValueOnUI()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => 
                {
                    int index = ResList.Items.Count + 1; // 다음 항목 번호 (1부터 시작)

                    ListViewItem item1 = new ListViewItem(index.ToString());

                    item1.SubItems.Add(index.ToString());

                    item1.SubItems.Add(lbl_Toe_FL.Text);
                    item1.SubItems.Add(lbl_Toe_FR.Text);
                    item1.SubItems.Add(lbl_Toe_RL.Text);
                    item1.SubItems.Add(lbl_Toe_RR.Text);
                                       
                    item1.SubItems.Add(lbl_Cam_FL.Text);
                    item1.SubItems.Add(lbl_Cam_FR.Text);
                    item1.SubItems.Add(lbl_Cam_RL.Text);
                    item1.SubItems.Add(lbl_Cam_RR.Text);

                    ResList.Items.Add(item1);

                }));
            }
           
        }


        private void UpdateMsg(string strMsg)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() =>
                {
                    lblMessage.Text = strMsg;
                }));
            }

        }


        private Progress<int>  RunProgress()
        {
            Progress<int> runoutProgress = new Progress<int>(ms =>
            {
                if (pgs_runout.InvokeRequired)
                {
                    pgs_runout.Invoke(new Action(() =>
                    {
                        pgs_runout.Maximum = m_nRunOutTime * 1000;
                        pgs_runout.Value = Math.Min(ms, pgs_runout.Maximum);
                    }));
                }
                else
                {
                    pgs_runout.Maximum = m_nRunOutTime * 1000;
                    pgs_runout.Value = Math.Min(ms, pgs_runout.Maximum);
                }
            });

            return runoutProgress;


        }

        private Progress<int> RunIdle()
        {
            Progress<int> Idle = new Progress<int>(ms =>
            {
                if (pgs_Idle.InvokeRequired)
                {
                    pgs_Idle.Invoke(new Action(() =>
                    {
                        pgs_Idle.Maximum = m_nIdleTime * 1000;
                        pgs_Idle.Value = Math.Min(ms, pgs_Idle.Maximum);
                    }));
                }
                else
                {
                    pgs_Idle.Maximum = m_nIdleTime * 1000;
                    pgs_Idle.Value = Math.Min(ms, pgs_Idle.Maximum);
                }
            });

            return Idle;


        }

        private void MeasureStart()
        {
            m_bTestRun = true;
            m_bStopRequested = false;
            string strMsg;
            for (int i = 1; i <= m_nCnt ; i++)
            {
                if (m_bStopRequested) break;
                if (rd_bw.Checked)
                {
                    strMsg = "Backward Measure " + i.ToString();
                }
                else
                {
                    strMsg = "Foward Measure " + i.ToString();
                }

                UpdateMsg(strMsg);

                //MotorOn();
                if (pgs_runout.InvokeRequired)
                {
                    pgs_runout.Invoke(new Action(() =>
                    {
                        pgs_runout.Maximum = m_nRunOutTime * 1000;
                        pgs_runout.Value = 0;
                    }));
                }
                else
                {
                    pgs_runout.Maximum = m_nRunOutTime * 1000;
                    pgs_runout.Value = 0;
                }
                if (pgs_Idle.InvokeRequired)
                {
                    pgs_Idle.Invoke(new Action(() =>
                    {       
                        pgs_Idle.Value = 0;
                    }));
                }
                else
                {
                    pgs_Idle.Value = 0;
                }

                Progress<int> runoutProgress = RunProgress();
                SleepWithAbortCheck(m_nRunOutTime * 1000, runoutProgress);
                if (m_bStopRequested) break;


                Thread.Sleep(1000);
                UpdateToeValueOnUI();

                Progress<int> PgrIdle = RunIdle();
                

                // 모터 OFF
                // MotorOff();

                // 다음 사이클 전 3초 대기 (진행률 없이)
                SleepWithAbortCheck(m_nIdleTime *1000 , PgrIdle);

                Thread.Sleep(1000);

            }

            strMsg = "Finish";
            UpdateMsg(strMsg);
            m_bTestRun = false;
        }


    }
}

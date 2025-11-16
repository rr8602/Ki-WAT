
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

        private class StatndardData
        {
            public double dToe_FL_Std;
            public double dToe_FR_Std;
            public double dToe_RL_Std;
            public double dToe_RR_Std;
            public double dCam_FL_Std;
            public double dCam_FR_Std;
            public double dCam_RL_Std;
            public double dCam_RR_Std;

            public double dToe_FL_Tol;
            public double dToe_FR_Tol;
            public double dToe_RL_Tol;
            public double dToe_RR_Tol;
            public double dCam_FL_Tol;
            public double dCam_FR_Tol;
            public double dCam_RL_Tol;
            public double dCam_RR_Tol;

        }
        private volatile bool m_bTestRun = false ;
        private volatile bool m_bStopRequested = false;
        private volatile int m_nCnt = 0;
        private volatile int m_nRunOutTime = 0;
        private volatile int m_nIdleTime = 8;

        private StatndardData m_stdDAta = new StatndardData();

        GlobalVal _GV = GlobalVal.Instance;

        private Thread m_Thread = null;
        public Frm_Rolling()
        {
            InitializeComponent();
        }
        private void Frm_Rolling_Load(object sender, EventArgs e)
        {
            ResList.Columns.Add("", 0);
            ResList.Columns.Add("No.", ResList.Width / 9 - 30, HorizontalAlignment.Center);
            ResList.Columns.Add("Dir", ResList.Width / 9 - 30, HorizontalAlignment.Center);

            ResList.Columns.Add("FL Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("FR Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RL Toe", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RR Toe", ResList.Width / 9, HorizontalAlignment.Center);

            ResList.Columns.Add("FL Cam", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("FR Cam", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RL Cam", ResList.Width / 9, HorizontalAlignment.Center);
            ResList.Columns.Add("RR Cam", ResList.Width / 9, HorizontalAlignment.Center);

            text_cnt.Text = "3";
            text_time.Text = "15";
            rd_fw.Checked = true;

            InitStdData(0);

            Broker.NotifyBroker.Subscribe(Topics.Notify.GetDppData, GetDppData);

        }
        private void InitStdData(int nType)
        {
            if (nType == 0) // Toe
            {
                m_stdDAta.dToe_FL_Std = _GV.m_Model_Rolling.ToeFL_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeFL_ST) : 0.0;
                m_stdDAta.dToe_FR_Std = _GV.m_Model_Rolling.ToeFR_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeFR_ST) : 0.0;
                m_stdDAta.dToe_RL_Std = _GV.m_Model_Rolling.ToeRL_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeRL_ST) : 0.0;
                m_stdDAta.dToe_RR_Std = _GV.m_Model_Rolling.ToeRR_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeRR_ST) : 0.0;
                m_stdDAta.dCam_FL_Std = _GV.m_Model_Rolling.CamFL_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamFL_ST) : 0.0;
                m_stdDAta.dCam_FR_Std = _GV.m_Model_Rolling.CamFR_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamFR_ST) : 0.0;
                m_stdDAta.dCam_RL_Std = _GV.m_Model_Rolling.CamRL_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamRL_ST) : 0.0;
                m_stdDAta.dCam_RR_Std = _GV.m_Model_Rolling.CamRR_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamRR_ST) : 0.0;

                m_stdDAta.dToe_FL_Tol = _GV.m_Model_Rolling.ToeFL_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeFL_LT) : 0.0;
                m_stdDAta.dToe_FR_Tol = _GV.m_Model_Rolling.ToeFR_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeFR_LT) : 0.0;
                m_stdDAta.dToe_RL_Tol = _GV.m_Model_Rolling.ToeRL_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeRL_LT) : 0.0;
                m_stdDAta.dToe_RR_Tol = _GV.m_Model_Rolling.ToeRR_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeRR_LT) : 0.0;
                m_stdDAta.dCam_FL_Tol = _GV.m_Model_Rolling.CamFL_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamFL_LT) : 0.0;
                m_stdDAta.dCam_FR_Tol = _GV.m_Model_Rolling.CamFR_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamFR_LT) : 0.0;
                m_stdDAta.dCam_RL_Tol = _GV.m_Model_Rolling.CamRL_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamRL_LT) : 0.0;
                m_stdDAta.dCam_RR_Tol = _GV.m_Model_Rolling.CamRR_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamRR_LT) : 0.0;
            }
            else // FL <-> RR, FR <-> RL
            {
                m_stdDAta.dToe_FL_Std = _GV.m_Model_Rolling.ToeRR_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeRR_ST) : 0.0;
                m_stdDAta.dToe_FR_Std = _GV.m_Model_Rolling.ToeRL_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeRL_ST) : 0.0;
                m_stdDAta.dToe_RL_Std = _GV.m_Model_Rolling.ToeFR_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeFR_ST) : 0.0;
                m_stdDAta.dToe_RR_Std = _GV.m_Model_Rolling.ToeFL_ST != null ? Double.Parse(_GV.m_Model_Rolling.ToeFL_ST) : 0.0;
                m_stdDAta.dCam_FL_Std = _GV.m_Model_Rolling.CamRR_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamRR_ST) : 0.0;
                m_stdDAta.dCam_FR_Std = _GV.m_Model_Rolling.CamRL_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamRL_ST) : 0.0;
                m_stdDAta.dCam_RL_Std = _GV.m_Model_Rolling.CamFR_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamFR_ST) : 0.0;
                m_stdDAta.dCam_RR_Std = _GV.m_Model_Rolling.CamFL_ST != null ? Double.Parse(_GV.m_Model_Rolling.CamFL_ST) : 0.0;

                m_stdDAta.dToe_FL_Tol = _GV.m_Model_Rolling.ToeRR_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeRR_LT) : 0.0;
                m_stdDAta.dToe_FR_Tol = _GV.m_Model_Rolling.ToeRL_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeRL_LT) : 0.0;
                m_stdDAta.dToe_RL_Tol = _GV.m_Model_Rolling.ToeFR_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeFR_LT) : 0.0;
                m_stdDAta.dToe_RR_Tol = _GV.m_Model_Rolling.ToeFL_LT != null ? Double.Parse(_GV.m_Model_Rolling.ToeFL_LT) : 0.0;
                m_stdDAta.dCam_FL_Tol = _GV.m_Model_Rolling.CamRR_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamRR_LT) : 0.0;
                m_stdDAta.dCam_FR_Tol = _GV.m_Model_Rolling.CamRL_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamRL_LT) : 0.0;
                m_stdDAta.dCam_RL_Tol = _GV.m_Model_Rolling.CamFR_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamFR_LT) : 0.0;
                m_stdDAta.dCam_RR_Tol = _GV.m_Model_Rolling.CamFL_LT != null ? Double.Parse(_GV.m_Model_Rolling.CamFL_LT) : 0.0;
            }

            lbl_TOE_STD_FL.Text = m_stdDAta.dToe_FL_Std.ToString() + "' ±" + m_stdDAta.dToe_FL_Tol.ToString();
            lbl_TOE_STD_FR.Text = m_stdDAta.dToe_FR_Std.ToString() + "' ±" + m_stdDAta.dToe_FR_Tol.ToString();
            lbl_TOE_STD_RL.Text = m_stdDAta.dToe_RL_Std.ToString() + "' ±" + m_stdDAta.dToe_RL_Tol.ToString();
            lbl_TOE_STD_RR.Text = m_stdDAta.dToe_RR_Std.ToString() + "' ±" + m_stdDAta.dToe_RR_Tol.ToString();
            
            lbl_CAM_STD_FL.Text = m_stdDAta.dCam_FL_Std.ToString() + "' ±" + m_stdDAta.dCam_FL_Tol.ToString() ;
            lbl_CAM_STD_FR.Text = m_stdDAta.dCam_FR_Std.ToString() + "' ±" + m_stdDAta.dCam_FR_Tol.ToString() ;
            lbl_CAM_STD_RL.Text = m_stdDAta.dCam_RL_Std.ToString() + "' ±" + m_stdDAta.dCam_RL_Tol.ToString() ;
            lbl_CAM_STD_RR.Text = m_stdDAta.dCam_RR_Std.ToString() + "' ±" + m_stdDAta.dCam_RR_Tol.ToString() ;


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

            lbl_Cam_FL.BackColor = GetColor(pData.dCamFL, m_stdDAta.dCam_FL_Std, m_stdDAta.dCam_FL_Tol);
            lbl_Cam_FR.BackColor = GetColor(pData.dCamFR, m_stdDAta.dCam_FR_Std, m_stdDAta.dCam_FR_Tol);
            lbl_Cam_RL.BackColor = GetColor(pData.dCamRL, m_stdDAta.dCam_RL_Std, m_stdDAta.dCam_RL_Tol);
            lbl_Cam_RR.BackColor = GetColor(pData.dCamRR, m_stdDAta.dCam_RR_Std, m_stdDAta.dCam_RR_Tol);
            
            lbl_Toe_FL.BackColor = GetColor(pData.dToeFL, m_stdDAta.dToe_FL_Std, m_stdDAta.dToe_FL_Tol);
            lbl_Toe_FR.BackColor = GetColor(pData.dToeFR, m_stdDAta.dToe_FR_Std, m_stdDAta.dToe_FR_Tol);
            lbl_Toe_RL.BackColor = GetColor(pData.dToeRL, m_stdDAta.dToe_RL_Std, m_stdDAta.dToe_RL_Tol);
            lbl_Toe_RR.BackColor = GetColor(pData.dToeRR, m_stdDAta.dToe_RR_Std, m_stdDAta.dToe_RR_Tol);

        }

        private Color GetColor(double dMeas, double dStd, double dTol)
        {

         
            if (dMeas >= (dStd - dTol) && dMeas <= (dStd + dTol))
            {
                return Color.LimeGreen;
            }
            else
            {
                return Color.Crimson;
            }
        }

        private void Btn_Init_Click(object sender, EventArgs e)
        {
            ResList.Items.Clear();

            lbl_Cam_FL.Text = "-";
            lbl_Cam_FR.Text = "-";
            lbl_Cam_RL.Text = "-";
            lbl_Cam_RR.Text = "-";

            lbl_Toe_FL.Text = "-";
            lbl_Toe_FR.Text = "-";
            lbl_Toe_RL.Text = "-";
            lbl_Toe_RR.Text = "-";

            lbl_Cam_FL.BackColor = Color.Black;
            lbl_Cam_FR.BackColor = Color.Black;
            lbl_Cam_RL.BackColor = Color.Black;
            lbl_Cam_RR.BackColor = Color.Black;

            lbl_Toe_FL.BackColor = Color.Black;
            lbl_Toe_FR.BackColor = Color.Black;
            lbl_Toe_RL.BackColor = Color.Black;
            lbl_Toe_RR.BackColor = Color.Black;
        }

        private void Btn_Start_Click(object sender, EventArgs e)
        {
            if (!_GV.plcRead.IsRollingMaster() )
            {
                MessageBox.Show("Please change the model to Rolling Master");
            }

            if ( m_bTestRun == true )
            {
                MessageBox.Show("Already Testing.");
                return;
            }

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

                    if (rd_bw.Checked)
                        item1.SubItems.Add("BW");
                    else
                        item1.SubItems.Add("FW");

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

        private void StartCMD()
        {
            _GV.plcWrite.SetFloatingPlateLock(false);
            _GV.plcWrite.SetFloatingPlateFree(true);
            
            _GV.plcWrite.SetRollerBrakeLock(false);
            _GV.plcWrite.SetRollerBrakeFree(true);

            _GV.plcWrite.WritePLCData();
            Thread.Sleep(100);

            _GV.plcWrite.SetCenteringOn(true);
            _GV.plcWrite.SetCenteringHome(false);

            _GV.plcWrite.SetMotorRun(true);
            _GV.plcWrite.SetMotorStop(false);

            _GV.plcWrite.WritePLCData();
        }

        private void StopCMD()
        {
            //_GV.plcWrite.SetFloatingPlateLock(true);
            //_GV.plcWrite.SetFloatingPlateFree(false);

            //_GV.plcWrite.SetRollerBrakeLock(true);
            //_GV.plcWrite.SetRollerBrakeFree(false);

            _GV.plcWrite.SetMotorRun(false);
            _GV.plcWrite.SetMotorStop(true);

            _GV.plcWrite.WritePLCData();
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_STOP, 1);
            Thread.Sleep(500);
            UpdateToeValueOnUI();
            _GV.plcWrite.SetCenteringOn(false);
            _GV.plcWrite.SetCenteringHome(true);
            _GV.plcWrite.WritePLCData();
        }


        private void MeasureStart()
        {
            m_bTestRun = true;
            m_bStopRequested = false;
            string strMsg;

            _GV.plcWrite.SetSafetyRollerUp(true);
            _GV.plcWrite.SetSafetyRollerDown(false);
            _GV.plcWrite.WritePLCData();
            Thread.Sleep(1000);




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
                StartCMD();

                DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_START, 1);

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
                
                Progress<int> PgrIdle = RunIdle();
                StopCMD();
               
               
                SleepWithAbortCheck(m_nIdleTime *1000 , PgrIdle);
                Thread.Sleep(1000);

            }
            
            _GV.plcWrite.SetFloatingPlateLock(true);
            _GV.plcWrite.SetFloatingPlateFree(false);

            _GV.plcWrite.SetRollerBrakeLock(true);
            _GV.plcWrite.SetRollerBrakeFree(false);
            _GV.plcWrite.WritePLCData();
            Thread.Sleep(1000);

            _GV.plcWrite.SetSafetyRollerUp(false);
            _GV.plcWrite.SetSafetyRollerDown(true);
            _GV.plcWrite.WritePLCData();


            strMsg = "Finish";
            UpdateMsg(strMsg);
            m_bTestRun = false;
        }

        private void rd_fw_CheckedChanged(object sender, EventArgs e)
        {

            _GV.m_Model_Rolling = _GV._dbJob.SelectCarModel("RollingMaster");
            if (rd_fw.Checked)
            {
                InitStdData(0);
            }
            else
            {
                InitStdData(1);
            }
            
        }

        private void rd_bw_CheckedChanged(object sender, EventArgs e)
        {
            InitStdData(1);
        }

        private void BtnStop_Click(object sender, EventArgs e)
        {
            m_bTestRun = true;
            m_bStopRequested = true;
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {

        }
    }
}

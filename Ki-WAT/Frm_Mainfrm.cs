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

using KINT_Lib;
using static KINT_Lib.Lib_TcpClient;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Ki_WAT
{
    public partial class Frm_Mainfrm : Form 
    {


        GlobalVal globalData1 = GlobalVal.Instance;
        //globalData1.g_DppData.dToeFL = 10.5;
        

        public Frm_Main m_frmMain = new Frm_Main();
        public Frm_Config m_frmConfig = new Frm_Config();
        public Frm_Operator User_Monitor = null;
        public Frm_PitInMonitor Pit_Monitor = null;
        public Frm_Rolling m_frmRoll = new Frm_Rolling();
        public Frm_StaticMaster m_frmStatic = new Frm_StaticMaster();
        public Frm_Result m_frmResult = new Frm_Result();

        private List<Button> m_NavButtons = new List<Button>();
        private int m_nCurrentFrmIdx = Def.FOM_IDX_MAIN;
        private Form m_ActiveSubForm;
        
        private Lib_TcpClient m_tcpBoardSpeed = new Lib_TcpClient();
        private Lib_Tcp_Server m_tcp_Server = new Lib_Tcp_Server();
        
        private DB_LocalWat m_dbJob;

        private const int WM_COPYDATA = 0x004A;

        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct DppData
        {
            public double dToeFL;
            public double dToeFR;
            public double dToeRL;
            public double dToeRR;

            public double dCamFL;
            public double dCamFR;
            public double dCamRL;
            public double dCamRR;

            public double dSymm;
            public double dTA;

            public double dHeightFL;
            public double dHeightFR;
            public double dHeightRL;
            public double dHeightRR;
        }
        [StructLayout(LayoutKind.Sequential, Pack = 8)]
        public struct DppState
        {
            public int nState_FL;
            public int nState_FR;
            public int nState_RL;
            public int nState_RR;

        }

        [StructLayout(LayoutKind.Sequential)]
        public struct COPYDATASTRUCT
        {
            public IntPtr dwData;
            public int cbData;
            public IntPtr lpData;


        }

        public Frm_Mainfrm()
        {
            InitializeComponent();
        }
        private void Frm_Mainfrm_Load(object sender, EventArgs e)
        {
            m_frmMain.SetParent(this);
            m_frmConfig.SetParent(this);

            InitializeSubForm(m_frmMain);
            InitializeSubForm(m_frmConfig);
            InitializeSubForm(m_frmRoll);
            InitializeSubForm(m_frmStatic);
            InitializeSubForm(m_frmResult);

            m_tcp_Server.Start("127.0.0.1", 8511);

            m_tcpBoardSpeed.Connect("127.0.0.1", 8511);
            m_tcpBoardSpeed.OnDataReceived += new DataReceiveClient(event_GetSpeedData);
            m_dbJob = new DB_LocalWat(Application.StartupPath + "\\System\\WAT-DataDB.mdb");

            m_NavButtons.Add(BtnMain);
            
            m_NavButtons.Add(BtnManual);
            m_NavButtons.Add(btnIo);
            m_NavButtons.Add(BtnConfig);
            m_NavButtons.Add(Btn_Rolling);
            m_NavButtons.Add(Btn_StaticMaster);
            m_NavButtons.Add(Btn_Result);



            if (!this.DesignMode)
            {
                if (User_Monitor == null || User_Monitor.Text == "")
                {
                    User_Monitor = new Frm_Operator();
                    User_Monitor.Show();
                }
                //if (Pit_Monitor == null || Pit_Monitor.Text == "")
                //{
                //    Pit_Monitor = new Frm_PitInMonitor();
                //    Pit_Monitor.Show();
                //}

            }

            ChangeButtonColor(BtnMain);
            ShowFrm(Def.FOM_IDX_MAIN);
            asdfasdf

            //var MeasureData = GlobalVal.Instance;
            //MeasureData.g_DppData.dToeFL = 10.5;
                
        }
        private void ChangeButtonColor(Button pButton)
        {
            foreach (Button btn in m_NavButtons)
            { 
                if (pButton.Text == btn.Text)
                {
                    btn.BackColor = Color.Gray;
                    btn.ForeColor = SystemColors.ControlLightLight;
                }
                else
                {
                    btn.BackColor = Color.Gainsboro;  // SystemColors.Control;
                    btn.ForeColor = Color.Black;      // SystemColors.ControlText; 
                }
            }
        }
        public void event_GetSpeedData(byte[] data)
        {
            Console.WriteLine("");
        }

        private void InitializeSubForm(Form f)
        {
            f.ControlBox = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;
            f.MdiParent = this;
        }


        private void RepositionSubForm(Form fSubform)
        {
            if (fSubform == null)
                return;
            fSubform.Location = new Point(0, 0);
            fSubform.Size = new Size(ClientSize.Width - panelNavBar.Size.Width - 4, ClientSize.Height - 4);
            
        }

        public Form ActiveSubForm
        {
            get
            {
                return m_ActiveSubForm;
            }
            set
            {
                m_ActiveSubForm = value;
                if (m_ActiveSubForm == null)
                    return;
                
                RepositionSubForm(m_ActiveSubForm);
            }
        }


        private void ShowFrm(int nIdx)
        {
            m_nCurrentFrmIdx = nIdx;
            Form f = new Form();
            switch (m_nCurrentFrmIdx)
            {
                case Def.FOM_IDX_MAIN:
                    f = m_frmMain;
                    break;
                case Def.FOM_IDX_CONFIG:
                    f = m_frmConfig;
                    break;
                case Def.FOM_IDX_ROLLING:
                    f = m_frmRoll;
                    break;
                case Def.FOM_IDX_STATIC:
                    f = m_frmStatic;
                    break;
                case Def.FOM_IDX_RESULT:
                    f = m_frmResult;
                    break;

                    
            }

            
            f.Show();
            f.BringToFront();
            ActiveSubForm = f;

            if (User_Monitor != null)
            {
                
                if ( m_nCurrentFrmIdx == Def.FOM_IDX_MAIN || m_nCurrentFrmIdx == Def.FOM_IDX_CONFIG|| 
                     m_nCurrentFrmIdx == Def.FOM_IDX_RESULT )
                {
                    User_Monitor.ShowFrm(Def.FOM_IDX_MAIN);
                }
                else
                {
                    User_Monitor.ShowFrm(m_nCurrentFrmIdx);
                }

            }
        }

        private void BtnConfig_Click(object sender, EventArgs e)
        {

            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_CONFIG);
        }

        private void BtnMain_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_MAIN);
        }

        private void btnParameter_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            
        }

        private void btnManual_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void Btn_T_Click(object sender, EventArgs e)
        {
            m_dbJob.SelectCarInfo("2024061100202");
            m_frmMain._Log_ListBox("asdf");
        }

        private void btnIo_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
        }

        private void BtnCal_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_ROLLING);

        }

        private void Btn_StaticMaster_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_STATIC);
        }

        protected override void WndProc(ref Message m)
        {
            switch (m.Msg)
            {
                case WM_COPYDATA:
                    HandleCopyData(ref m);
                    return;
            }

            base.WndProc(ref m);
        }

        private void HandleCopyData(ref Message m)
        {
            try
            {
                COPYDATASTRUCT cds = (COPYDATASTRUCT)m.GetLParam(typeof(COPYDATASTRUCT));
                if (cds.dwData.ToInt32() == 1) // 구조체 데이터
                {
                    // IntPtr을 구조체로 변환
                    DppData receivedData = (DppData)Marshal.PtrToStructure(cds.lpData, typeof(DppData));

                    // UI 스레드에서 실행
                    this.Invoke(new Action(() => 
                    {
                        var stSensor = GlobalVal.Instance;

                        stSensor.g_DppData.dCamFL = receivedData.dCamFL;
                        stSensor.g_DppData.dCamFR = receivedData.dCamFR;
                        stSensor.g_DppData.dCamRL = receivedData.dCamRL;
                        stSensor.g_DppData.dCamRR = receivedData.dCamRR;

                        stSensor.g_DppData.dToeFL = receivedData.dToeFL;
                        stSensor.g_DppData.dToeFR = receivedData.dToeFR;
                        stSensor.g_DppData.dToeRL = receivedData.dToeRL;
                        stSensor.g_DppData.dToeRR = receivedData.dToeRR;

                        stSensor.g_DppData.dTA = receivedData.dTA;
                        stSensor.g_DppData.dSymm = receivedData.dSymm;

                        stSensor.g_DppData.dHeightFL = receivedData.dHeightFL;
                        stSensor.g_DppData.dHeightFR = receivedData.dHeightFR;
                        stSensor.g_DppData.dHeightRL = receivedData.dHeightRL;
                        stSensor.g_DppData.dHeightRR = receivedData.dHeightRR;

                    }));
                }
                else if ( cds.dwData.ToInt32() == 2 )
                {
                    DppState _dppState = (DppState)Marshal.PtrToStructure(cds.lpData, typeof(DppState));
                    // UI 스레드에서 실행
                    this.Invoke(new Action(() =>
                    {
                        var stSensor = GlobalVal.Instance;
                        stSensor.g_DppState.nState_FL = _dppState.nState_FL;
                        stSensor.g_DppState.nState_FR = _dppState.nState_FR;
                        stSensor.g_DppState.nState_RL = _dppState.nState_RL;
                        stSensor.g_DppState.nState_RR = _dppState.nState_RR;

                    }));
                }
                m.Result = new IntPtr(1); // 성공 반환
            }
            catch (Exception ex)
            {
                MessageBox.Show($"데이터 처리 오류: {ex.Message}");
                m.Result = IntPtr.Zero; // 실패 반환
            }
        }

        private void Btn_Result_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            ShowFrm(Def.FOM_IDX_RESULT);
        }
    }
}

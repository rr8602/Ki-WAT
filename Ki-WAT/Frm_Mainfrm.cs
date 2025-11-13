using HandleLeveler;
using KINT_Lib;
using LETInterface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KINT_Lib.Lib_TcpClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.Rebar;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Ki_WAT
{
    public partial class Frm_Mainfrm : Form 
    {
        GlobalVal _GV = GlobalVal.Instance;
        //globalData1.g_DppData.dToeFL = 10.5;
        public  readonly AngleCalculator m_SWBComm;
        public Frm_Main m_frmMain = new Frm_Main();
        public Frm_Config m_frmConfig = new Frm_Config();
        public Frm_Operator User_Monitor = null;
        public Frm_PitInMonitor m_Frm_PitIn = null;
        public Frm_Rolling m_frmRoll = new Frm_Rolling();
        public Frm_StaticMaster m_frmStatic = new Frm_StaticMaster();
        public Frm_Result m_frmResult = new Frm_Result();
        public Frm_Manual m_frmManual = new Frm_Manual() ;
        public FrmSimulator m_frmSimul;

        private List<Button> m_NavButtons = new List<Button>();
        private int m_nCurrentFrmIdx = Def.FOM_IDX_MAIN;
        private Form m_ActiveSubForm;

        //public Lib_TcpClient m_BarcodeComm = new Lib_TcpClient();
        private Lib_TcpClient m_ScrewDriverL = new Lib_TcpClient();
        private Lib_TcpClient m_ScrewDriverR = new Lib_TcpClient();


        Frm_DigitalIO digtalIO = null;


        // 바코드 통신 연결 상태 체크용 타이머
        private System.Windows.Forms.Timer m_StatusTimer = new System.Windows.Forms.Timer() ;
        private System.Windows.Forms.Timer m_TimerTest;

        private System.Windows.Forms.Timer m_initCheckTimer = new System.Windows.Forms.Timer();
        private System.Windows.Forms.Timer m_StartTimer = new System.Windows.Forms.Timer();


        //public DB_LocalWat m_dbJob;
        private const int WM_COPYDATA = 0x004A;

        public delegate void DppDataReceive(MeasureData pData);
        public event DppDataReceive OnDppDataReceived;

        private Random _rand = new Random();  // 클래스 멤버로 선언 (중복 난수 방지)
        public Lib_TcpClient ScrewDriverL => m_ScrewDriverL;
        public Lib_TcpClient ScrewDriverR => m_ScrewDriverR;

        public BarcodeReader _barcodeReader;

        public Frm_Mainfrm()
        {
            InitializeComponent();
            m_SWBComm = new AngleCalculator( UpdateDisplayValues, UpdateSWBAngle);
        }
        private void InitUI()
        {
            m_frmMain.SetParent(this);
            m_frmConfig.SetParent(this);
            m_frmResult.SetParent(this);
            m_frmManual.SetParent(this);
            m_frmStatic.SetParent(this);
            _GV._TestThread.SetMainFrame(this);

            InitializeSubForm(m_frmMain);
            InitializeSubForm(m_frmConfig);
            InitializeSubForm(m_frmRoll);
            InitializeSubForm(m_frmStatic);
            InitializeSubForm(m_frmResult);
            InitializeSubForm(m_frmManual);

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
                    User_Monitor.SetParent(this);
                    User_Monitor.Show();
                }

                if (m_Frm_PitIn == null )
                {
                    m_Frm_PitIn = new Frm_PitInMonitor(this);
                    m_Frm_PitIn.Show();
                }
            }

            ChangeButtonColor(BtnMain);
            ShowFrm(Def.FOM_IDX_MAIN);

        }
        private void Frm_Mainfrm_Load(object sender, EventArgs e)
        {
            _GV.Config.LoadConfig();
            _GV._dbJob = new DB_LocalWat(Application.StartupPath + "\\System\\WAT-DataDB.mdb");

            InitUI();
            _GV._frmMNG.RegisterForm(this);
            m_frmSimul = new FrmSimulator(this);
            
            m_StatusTimer.Tick += new EventHandler(StatusTimer_Tick);
            m_StatusTimer.Interval = 3000; // 1초마다
            m_StatusTimer.Start();

            
            m_initCheckTimer.Interval = 100; // 0.1초 간격
            m_initCheckTimer.Tick += InitCheckTimer_Tick;
            m_initCheckTimer.Start();

            m_StartTimer.Interval = 1000; // 0.1초 간격
            m_StartTimer.Tick += StartTimer_Tick;
            m_StartTimer.Start();

            

            CreateIODlg();
            //DeviceOpen();
            //Thread.Sleep(5000);

         
        }


        public void CreateIODlg()
        {
            digtalIO = new Frm_DigitalIO();

            digtalIO.StartPosition = FormStartPosition.Manual;
            digtalIO.Location = new Point(120, 20);
            digtalIO.Width = Constants.SubViewWidth;
            digtalIO.Height = Constants.SubViewHeight;

        }
        private void StartTimer_Tick(object sender, EventArgs e)
        {
            picBK.Hide();
            stcLable.Hide();
            m_StartTimer.Stop();
            DeviceOpen();
        }

        private void UpdateSWBAngle(double dSWB)
        {
            _GV.dHandle = dSWB;
        }
        private void UpdateDisplayValues(string fnd, string sensorAd, string boardAngle, string pcAngle)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateDisplayValues(fnd, sensorAd, boardAngle, pcAngle)));
                return;
            }



        }
        private void InitCheckTimer_Tick(object sender, EventArgs e)
        {
            new Thread(() =>
            {
                //_GV.g_DppState.nState_RR = 0;
                if (_GV.vep.IsConnect() && _GV.g_DppState.nState_RR == 0 )
                {
                    this.Invoke(new Action(() =>
                    {
                        picBK.Hide();
                        stcLable.Hide();
                    }));
                    m_initCheckTimer.Stop();
                }
            }).Start();
        }

        private void StatusTimer_Tick(object sender, EventArgs e)
        {
            //lbl_Status_SWB.BackColor = m_ScrewDriverL.IsConnected ? Color.LimeGreen : Color.OrangeRed;
            //lbl_Status_BAR.BackColor = _barcodeReader.IsConnected() ? Color.LimeGreen : Color.OrangeRed;
            lbl_State_VEP.BackColor = _GV.vep.IsConnect() ? Color.LimeGreen : Color.OrangeRed;
            lbl_Status_SWB.BackColor = m_SWBComm.IsConnected ? Color.LimeGreen : Color.OrangeRed;
            Status_PLC.BackColor = _GV.plcRead.IsPLCConnect() ? Color.LimeGreen : Color.OrangeRed;
            lbl_SEN_FL.BackColor = (_GV.g_DppState.nState_FL == 0) ? Color.LimeGreen : Color.OrangeRed;
            lbl_SEN_FR.BackColor = (_GV.g_DppState.nState_FR == 0) ? Color.LimeGreen : Color.OrangeRed;
            lbl_SEN_RL.BackColor = (_GV.g_DppState.nState_RL == 0) ? Color.LimeGreen : Color.OrangeRed;
            lbl_SEN_RR.BackColor = (_GV.g_DppState.nState_RR == 0) ? Color.LimeGreen : Color.OrangeRed;

        }

        public void DeviceOpen()
        {
            string exePath = @"\Dpp\VisiconSampleVC.exe";


            Process[] runningProcesses = Process.GetProcessesByName("VisiconSampleVC");
            if (runningProcesses.Length == 0)
            {
                ProcessStartInfo psi = new ProcessStartInfo();
                psi.FileName = Application.StartupPath + exePath;
                psi.WindowStyle = ProcessWindowStyle.Minimized; // 최소화 실행
                psi.UseShellExecute = true;
                Process.Start(psi);
            }

            _GV.LET_Controller = new CycleControl();

            //Socket 
            //m_ScrewDriverL.Connect(_GV.Config.Device.SCREW_IP, Int32.Parse(_GV.Config.Device.SCREW_PORT));
            //m_ScrewDriverL.OnDataReceived += new DataReceiveClient(event_GetScrewL);

            //m_ScrewDriverR.Connect(_GV.Config.Device.SCREW_IP2, Int32.Parse(_GV.Config.Device.SCREW_PORT2));
            //m_ScrewDriverR.OnDataReceived += new DataReceiveClient(event_GetScrewL);

			int nRet = _GV.vep.Create("172.25.213.11", 502);
      
            m_SWBComm.Connect(_GV.Config.Device.SWB_PORT, Int32.Parse(_GV.Config.Device.SWB_BAUD));
            //StartBarcode();
            _GV.plcRead = new PLCReadWAT();
            _GV.plcWrite = new PLCWriteWAT();
            _GV.plcRead.Create(_GV.Config.Device.PLC_IP);
            _GV.plcWrite.Create(_GV.Config.Device.PLC_IP);
            _GV.plcRead.StartPolling();



            _GV.vep.SetSynchro(88, 1); 
           

        }
        public void event_GetBarcode(byte[] data)
        {
            //PP(2)PJI(7)PARA(3)PROJECTEURS(3)ADAS(3)
            try
            {
                if (data.Length < 18) return;
                string input = System.Text.Encoding.UTF8.GetString(data);
                m_frmMain.GetBarCode(input);
               
            }
            catch (Exception e)
            {
                MessageBox.Show(e.Message);
            }
        }

        public void event_GetScrewL(byte[] data)
        {

        }
        public void event_GetScrewR(byte[] data)
        {

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
 

        private void InitializeSubForm(Form f)
        {
            f.ControlBox = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;
            f.MdiParent = this;
            _GV._frmMNG.RegisterForm(f);
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
        public void ChangeOperMonitor(int nIdx )
        {
            User_Monitor.ShowFrm(nIdx);
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
                case Def.FOM_IDX_MANUAL:
                    f = m_frmManual;
                    break;
            }
            f.Show();
            f.BringToFront();
            ActiveSubForm = f;
            if (User_Monitor != null)
            {
                
                if ( m_nCurrentFrmIdx == Def.FOM_IDX_MAIN || m_nCurrentFrmIdx == Def.FOM_IDX_CONFIG|| 
                     m_nCurrentFrmIdx == Def.FOM_IDX_RESULT || m_nCurrentFrmIdx == Def.FOM_IDX_MANUAL)
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
            ShowFrm(Def.FOM_IDX_MANUAL);
        }
        private void btnIo_Click(object sender, EventArgs e)
        {
            ChangeButtonColor((Button)sender);
            if (digtalIO == null || digtalIO.IsDisposed )
            {
                CreateIODlg();
            }
            digtalIO.Show();
            

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
                    GetDppData(ref m);
                    return;
            }

            base.WndProc(ref m);
        }
        //시뮬레이션에서 사용
        public void SetDppData(MeasureData receivedData)
        {
            this.Invoke(new Action(() =>
            {
                
                double dRatio = Convert.ToDouble(_GV.m_Cur_Model.SWARatio);
                double dHandle = _GV.dHandle;
                if (dRatio == 0) dRatio = 17.5;
                if (dHandle > 1000) dHandle = 0;

                _GV.g_MeasureData.dToeFL = receivedData.dToeFL - (dHandle / dRatio);
                _GV.g_MeasureData.dToeFR = receivedData.dToeFR + (dHandle / dRatio);
                _GV.g_MeasureData.dToeRL = receivedData.dToeRL;
                _GV.g_MeasureData.dToeRR = receivedData.dToeRR;


                _GV.g_MeasureData.dCamFL = receivedData.dCamFL;
                _GV.g_MeasureData.dCamFR = receivedData.dCamFR;
                _GV.g_MeasureData.dCamRL = receivedData.dCamRL;
                _GV.g_MeasureData.dCamRR = receivedData.dCamRR;

                //_GV.g_MeasureData.dToeFL = receivedData.dToeFL;
                //_GV.g_MeasureData.dToeFR = receivedData.dToeFR;
                
                _GV.g_MeasureData.dTA = receivedData.dTA;
                _GV.g_MeasureData.dSymm = receivedData.dSymm;
                OnDppDataReceived?.Invoke(_GV.g_MeasureData);
            }));
        }
        private void GetDppData(ref Message m)
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
                        _GV.g_MeasureData.dCamFL = receivedData.dCamFL;
                        _GV.g_MeasureData.dCamFR = receivedData.dCamFR;
                        _GV.g_MeasureData.dCamRL = receivedData.dCamRL;
                        _GV.g_MeasureData.dCamRR = receivedData.dCamRR;

                        _GV.g_MeasureData.dToeFL = receivedData.dToeFL;
                        _GV.g_MeasureData.dToeFR = receivedData.dToeFR;
                        _GV.g_MeasureData.dToeRL = receivedData.dToeRL;
                        _GV.g_MeasureData.dToeRR = receivedData.dToeRR;

                        _GV.g_MeasureData.dTA = receivedData.dTA;
                        _GV.g_MeasureData.dSymm = receivedData.dSymm;
                        OnDppDataReceived?.Invoke(_GV.g_MeasureData);

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

        private void LogInfo(string err)
        {
            Debug.Print(err);
        }
        private void BarcodeReader_OnError(object sender, Exception e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => LogInfo(e.Message)));
            }
            else
            {
                LogInfo(e.Message);
                //MsgBox.ErrorWithFormat("ErrorInBarcodeReadLoop", "Error", e.Message);
            }
        }

        private void BarcodeReceivedHandler(object sender, BarcodeReader.BarcodeEventArgs e)
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => m_frmMain.GetBarCode(e.BarcodeData)));
            }
            else
            {
                m_frmMain.GetBarCode(e.BarcodeData);
            }
        }

        private void StartBarcode()
        {
            try
            {
                _barcodeReader = new BarcodeReader(_GV.Config.Device.BARCODE_IP);
                _barcodeReader.OnBarcodeReceived += BarcodeReceivedHandler;
                _barcodeReader.OnError += BarcodeReader_OnError;
                _barcodeReader.ConnectBarcodeReader();
            }
            catch (Exception ex)
            {
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {

                _barcodeReader?.Disconnect();

                // 테스트 스레드 정지
                if (_GV != null && _GV._TestThread != null)
                {
                    _GV._TestThread.StopThread();
                }
            }
            catch { }
            base.OnFormClosing(e);
        }

        private void Frm_Mainfrm_FormClosing(object sender, FormClosingEventArgs e)
        {

        }

        private void InitTestTimer()
        {
            m_TimerTest = new System.Windows.Forms.Timer();
            m_TimerTest.Interval = 100; // 1초마다
            m_TimerTest.Tick += new EventHandler(TestTimer_Tick);
            m_TimerTest.Start();
        }

        
        private void TestTimer_Tick(object sender, EventArgs e)
        {
            int nRand = _rand.Next(0, 1001);
			//_GV._VEP_Client.SetTzEtat((ushort)nRand);

			_GV.vep.SetTZExchange((ushort)nRand);

		}

        private void Btn_T_Click(object sender, EventArgs e)
        {
            //DppManager.SendToDpp(DppManager.MSG_DPP_VEHICLE_TYPE, 3);

          
            //InitTestTimer();
            m_frmSimul.Show();
        }

        private void Btn_VEP_Click(object sender, EventArgs e)
        {
            Frm_VEP vep = new Frm_VEP(this);
            vep.ShowVEP();
        }
    }
}

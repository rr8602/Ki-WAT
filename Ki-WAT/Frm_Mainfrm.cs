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
using LETInterface;
using System.Diagnostics;
//using static System.Windows.Forms.VisualStyles.VisualStyleElement.Window;

namespace Ki_WAT
{
    public partial class Frm_Mainfrm : Form 
    {


        GlobalVal _GV = GlobalVal.Instance;
        //globalData1.g_DppData.dToeFL = 10.5;
        

        public Frm_Main m_frmMain = new Frm_Main();
        public Frm_Config m_frmConfig = new Frm_Config();
        public Frm_Operator User_Monitor = null;
        public Frm_PitInMonitor Pit_Monitor = null;
        public Frm_Rolling m_frmRoll = new Frm_Rolling();
        public Frm_StaticMaster m_frmStatic = new Frm_StaticMaster();
        public Frm_Result m_frmResult = new Frm_Result();
        public Frm_Manual m_frmManual = new Frm_Manual() ;

        private List<Button> m_NavButtons = new List<Button>();
        private int m_nCurrentFrmIdx = Def.FOM_IDX_MAIN;
        private Form m_ActiveSubForm;


        public Lib_TcpClient m_BarcodeComm = new Lib_TcpClient();
        private Lib_TcpClient m_ScrewDriverL = new Lib_TcpClient();
        private Lib_TcpClient m_ScrewDriverR = new Lib_TcpClient();

        // 바코드 통신 연결 상태 체크용 타이머
        private Timer m_BarcodeConnectionTimer;
        
        // 바코드 재연결 관련 변수
        private bool m_bReconnecting = false;
        private DateTime m_lastReconnectAttempt = DateTime.MinValue;
        private const int RECONNECT_INTERVAL_SECONDS = 10; // 재연결 시도 간격 (초)

        public DB_LocalWat m_dbJob;
        private const int WM_COPYDATA = 0x004A;

        public delegate void DppDataReceive(MeasureData pData);
        public event DppDataReceive OnDppDataReceived;

        TblCarModel m_Cur_Model = new TblCarModel();

        public Frm_Mainfrm()
        {
            InitializeComponent();
            
            // 바코드 연결 상태 체크 타이머 초기화
            InitializeBarcodeConnectionTimer();
        }

        private void InitUI()
        {
            m_frmMain.SetParent(this);
            m_frmConfig.SetParent(this);
            m_frmResult.SetParent(this);
            m_frmManual.SetParent(this);
            m_frmStatic.SetParent(this);

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


            }

            ChangeButtonColor(BtnMain);
            ShowFrm(Def.FOM_IDX_MAIN);

        }
        private void Frm_Mainfrm_Load(object sender, EventArgs e)
        {
            _GV.Config.LoadConfig();

            DeviceOpen();
            InitUI();

        }

        public void DeviceOpen()
        {
            m_dbJob = new DB_LocalWat(Application.StartupPath + "\\System\\WAT-DataDB.mdb");

            _GV.LET_Controller = new CycleControl();
            //Socket 
            m_BarcodeComm.Connect("192.168.10.114", 8511);
            //m_BarcodeComm.OnDataReceived += new DataReceiveClient(event_GetBarcode);

            // 바코드 연결 상태 체크 타이머 시작
            StartBarcodeConnectionTimer();

            //m_ScrewDriverL.Connect("127.0.0.1", 8515);
            //m_ScrewDriverL.OnDataReceived += new DataReceiveClient(event_GetScrewL);

            //m_ScrewDriverR.Connect("127.0.0.1", 8516);
            //m_ScrewDriverR.OnDataReceived += new DataReceiveClient(event_GetScrewR);
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
            _GV._frmMNG.RegisterForm<Form>(f);
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
        private void Btn_T_Click(object sender, EventArgs e)
        {
            TblCarInfo tblCarInfo = new TblCarInfo();
            tblCarInfo.CarPJINo = "adf";
            tblCarInfo.CarModel = "adf";
            tblCarInfo.WatCycle = "999";
            tblCarInfo.LetCycle = "123";
            tblCarInfo.Car_Step = "0";
            tblCarInfo.TotalBar = "0";
            tblCarInfo.Spare__2 = "0";
            tblCarInfo.Spare__3 = "0";
            m_dbJob.AddNewAcceptNo(ref tblCarInfo);
            m_frmMain.RefreshCarInfoList();
            
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
                    GetDppData(ref m);
                    return;
            }

            base.WndProc(ref m);
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
                        _GV.g_DppData.dCamFL = receivedData.dCamFL;
                        _GV.g_DppData.dCamFR = receivedData.dCamFR;
                        _GV.g_DppData.dCamRL = receivedData.dCamRL;
                        _GV.g_DppData.dCamRR = receivedData.dCamRR;
                        
                        _GV.g_DppData.dToeFL = receivedData.dToeFL;
                        _GV.g_DppData.dToeFR = receivedData.dToeFR;
                        _GV.g_DppData.dToeRL = receivedData.dToeRL;
                        _GV.g_DppData.dToeRR = receivedData.dToeRR;
                        
                        _GV.g_DppData.dTA = receivedData.dTA;
                        _GV.g_DppData.dSymm = receivedData.dSymm;
                        
                        _GV.g_DppData.dHeightFL = receivedData.dHeightFL;
                        _GV.g_DppData.dHeightFR = receivedData.dHeightFR;
                        _GV.g_DppData.dHeightRL = receivedData.dHeightRL;
                        _GV.g_DppData.dHeightRR = receivedData.dHeightRR;

                        OnDppDataReceived?.Invoke(_GV.g_DppData);

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

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            try
            {
                // 바코드 연결 상태 체크 타이머 정지
                StopBarcodeConnectionTimer();
                
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

        #region 바코드 연결 상태 체크 타이머
        private void InitializeBarcodeConnectionTimer()
        {
            m_BarcodeConnectionTimer = new Timer();
            m_BarcodeConnectionTimer.Interval = 1000; // 1초마다
            m_BarcodeConnectionTimer.Tick += new EventHandler(BarcodeConnectionTimer_Tick);
        }

        private void StartBarcodeConnectionTimer()
        {
            if (m_BarcodeConnectionTimer != null)
            {
                m_BarcodeConnectionTimer.Start();
            }
        }

        private void StopBarcodeConnectionTimer()
        {
            if (m_BarcodeConnectionTimer != null)
            {
                m_BarcodeConnectionTimer.Stop();
            }
        }

        private void BarcodeConnectionTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                // 바코드 통신 연결 상태 확인
                bool isConnected = m_BarcodeComm != null && m_BarcodeComm.IsConnected;
                
                // 연결 상태에 따른 처리 (예: UI 업데이트, 로그 등)
                if (isConnected)
                {
                    Status_BAR.BackColor = Color.Lime;
                    m_bReconnecting = false; // 연결 성공 시 재연결 플래그 리셋
                    // 연결됨 - 필요시 UI 업데이트
                    // Debug.Print("바코드 통신 연결됨");
                }
                else
                {
                    Status_BAR.BackColor = Color.Red;
                    
                    // 재연결 시도 조건 확인
                    if (!m_bReconnecting && 
                        (DateTime.Now - m_lastReconnectAttempt).TotalSeconds >= RECONNECT_INTERVAL_SECONDS)
                    {
                        m_bReconnecting = true;
                        m_lastReconnectAttempt = DateTime.Now;
                        
                        // 비동기로 재연결 시도 (UI 스레드 블록 방지)
                        Task.Run(() => AttemptReconnect());
                        
                        Debug.Print("바코드 통신 재연결 시도 - " + DateTime.Now.ToString("HH:mm:ss"));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"바코드 연결 상태 체크 중 오류: {ex.Message}");
            }
        }
        
        private async Task AttemptReconnect()
        {
            try
            {
                // 백그라운드 스레드에서 재연결 시도
                if (m_BarcodeComm != null)
                {
                    m_BarcodeComm.Connect("192.168.10.114", 8511);
                    
                    // 잠시 대기 후 연결 상태 확인
                    await Task.Delay(2000);
                    
                    // UI 스레드에서 상태 업데이트
                    if (this.InvokeRequired)
                    {
                        this.Invoke(new Action(() => 
                        {
                            if (m_BarcodeComm.IsConnected)
                            {
                                Debug.Print("바코드 통신 재연결 성공");
                            }
                            else
                            {
                                Debug.Print("바코드 통신 재연결 실패");
                            }
                        }));
                    }
                }
            }
            catch (Exception ex)
            {
                Debug.Print($"바코드 재연결 시도 중 오류: {ex.Message}");
            }
            finally
            {
                m_bReconnecting = false; // 재연결 시도 완료
            }
        }
        #endregion
    }
}

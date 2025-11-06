using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public partial class Frm_Operator : Form
    {
        public int m_nCurrentFrmIdx = 0;
        public Frm_Oper_Wait m_frm_Oper_Wait = new Frm_Oper_Wait();
        public Frm_Oper_Test m_frm_Oper_Test = new Frm_Oper_Test();
        public Frm_Oper_Rolling m_frm_Oper_Roll = new Frm_Oper_Rolling();
        public Frm_Oper_Static m_frm_Oper_Static = new Frm_Oper_Static();
        private Point mousePoint; // 현재 마우스 포인터의 좌표저장 변수 선언
        GlobalVal _GV = GlobalVal.Instance;
        public List<Form> m_FrmList = new List<Form>();

        private Form m_ActiveSubForm;
        Frm_Mainfrm m_MainFrm;

        // lbl_Time 표시용 타이머와 카운터
        private Timer m_TimeTimer;
        private int m_TimeCounter = 0;



        public Frm_Operator()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();
            MoveFormToSecondMonitor();

        }
        public void SetParent(Frm_Mainfrm pParent)
        {
            m_MainFrm = pParent;
            m_frm_Oper_Test.SetMainFrm(m_MainFrm);
            m_frm_Oper_Test.SetOperator(this);
            m_frm_Oper_Static.SetMainFrm(m_MainFrm);
            m_frm_Oper_Static.SetOperator(this);
        }
        public void OnReceiveDpp(MeasureData pData)
        {
        }

        private void Frm_Operator_Load(object sender, EventArgs e)
        {
            InitializeSubForm(m_frm_Oper_Wait);
            InitializeSubForm(m_frm_Oper_Test);
            InitializeSubForm(m_frm_Oper_Roll);
            InitializeSubForm(m_frm_Oper_Static);
            ShowFrm(2);

            // 1초 타이머 시작 (lbl_Time에 1,2,3... 표시)
            m_TimeTimer = new Timer();
            m_TimeTimer.Interval = 1000;
            m_TimeTimer.Tick += TimeTimer_Tick;

        }
        public void StartTimer()
        {
            m_TimeCounter = 0;
            if (lbl_Time != null && !lbl_Time.IsDisposed)
            {
                lbl_Time.Text = m_TimeCounter.ToString();
            }

            m_TimeTimer.Start();
        }
        public void StopTimer()
        {
            m_TimeCounter = 0;
            m_TimeTimer.Stop();
        }
        public void SetPJIText(string strPJI)
        {
            if (lbl_PJI.InvokeRequired)
            {
                lbl_PJI.Invoke(new Action(() => lbl_PJI.Text = strPJI));
            }
            else
            {
                lbl_PJI.Text = strPJI;
            }
        }
        public void SetNextPJI(string strPJI)
        {
            if (lbl_NextPJI.InvokeRequired)
            {
                lbl_NextPJI.Invoke(new Action(() => lbl_NextPJI.Text = strPJI));
            }
            else
            {
                lbl_NextPJI.Text = strPJI;
            }
        }

        private void OnStatusUpdate_Main(string status)
        {
            try
            {
                if (IsDisposed || !IsHandleCreated) return;
                if (InvokeRequired)
                {
                    BeginInvoke(new Action<string>(OnStatusUpdate_Main), status);
                    return;
                }
                if (lbl_Message == null || lbl_Message.IsDisposed) return;

                lbl_Message.Text = status;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Status 업데이트 처리 중 오류: {ex.Message}");
            }
        }


        private void RepositionSubForm(Form fSubform)
        {
            if (fSubform == null)
                return;
            fSubform.Location = new Point(0, NavTop.Height );
            fSubform.Size = new Size(ClientSize.Width, ClientSize.Height - (NavTop.Height + NavBottom.Height));

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

        public void ShowFrm(int nIdx)
        {
            if (m_nCurrentFrmIdx == nIdx && ActiveSubForm != null)
                return;

            foreach (Form frm in m_FrmList)
            {
                frm.SendToBack();
            }
            m_nCurrentFrmIdx = nIdx;

            Form f = new Form();
            switch (m_nCurrentFrmIdx)
            {
                case Def.FOM_IDX_MAIN:
                    f = m_frm_Oper_Wait;
                    break;
                case Def.FOM_IDX_CONFIG:
                    f = m_frm_Oper_Wait;
                    break;
                case Def.FOM_IDX_ROLLING:
                    f = m_frm_Oper_Roll;
                    break;
                case Def.FOM_IDX_TEST:
                    f = m_frm_Oper_Test;
                    break;
                case Def.FOM_IDX_STATIC:
                    f = m_frm_Oper_Static;
                    break;
                default:
                    return;
            }

            //foreach(Form frm in m_FrmList)
            //{
            //    if ( f != frm)
            //        m_frm_Oper_Wait.SendToBack();
            //}
            f.BringToFront();
            f.Show();
            ActiveSubForm = f;
        }


        private void InitializeSubForm(Form f)
        {
            f.ControlBox = false;
            f.FormBorderStyle = FormBorderStyle.None;
            f.StartPosition = FormStartPosition.Manual;
            f.MdiParent = this;
            m_FrmList.Add(f);
        }

        private void MoveFormToSecondMonitor()
        {
            // 연결된 모든 모니터 가져오기
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 2)
            {
                // 두 번째 모니터 가져오기
                Screen secondScreen = screens[2];

                // 두 번째 모니터의 작업 영역 중앙에 폼 위치
                Rectangle workingArea = secondScreen.WorkingArea;
                this.StartPosition = FormStartPosition.Manual;
                this.Location = new Point(
                    workingArea.Left + (workingArea.Width - this.Width) / 2,
                    workingArea.Top + (workingArea.Height - this.Height) / 2
                );

            }
            else
            {
                MessageBox.Show("두 번째 모니터가 감지되지 않았습니다.");
            }
        }

        private void NavTop_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }

        private void NavTop_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장
        }

        private void Frm_Operator_Shown(object sender, EventArgs e)
        {
           TestThread_Kint.OnStatusUpdate += OnStatusUpdate_Main;
           TestThread_Kint.OnCycleFinished += OnCycleFinished;
            m_MainFrm.m_SWBComm._UpdateAngle += UpdateSWBAngle;
        }

        private void UpdateSWBAngle(double dSWB)
        {
            try
            {

                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() => UpdateSWBAngle(dSWB)));
                    return;
                }
                lbl_Hand.Text = dSWB.ToString("F1");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사이클 완료 처리 중 오류: {ex.Message}");
            }

            
        }

        private void FinishCycleUI()
        {
            lbl_Time.Text = "0";
            lbl_Message.Text = "";
            lbl_NextPJI.Text = "";
            lbl_PJI.Text = "";
        }
        private void OnCycleFinished()
        {
            try
            {
                if (InvokeRequired)
                {
                    Invoke(new Action(OnCycleFinished));
                    return;
                }
                FinishCycleUI();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"사이클 완료 처리 중 오류: {ex.Message}");
            }
        }


        private void TimeTimer_Tick(object sender, EventArgs e)
        {
            try
            {
                m_TimeCounter++;
                if (lbl_Time != null && !lbl_Time.IsDisposed)
                {
                    lbl_Time.Text = m_TimeCounter.ToString();
                }
                //lbl_Hand.Text = _GV.dHandle.ToString("F1");
            }
            catch { }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            try
            {
                if (m_TimeTimer != null)
                {
                    m_TimeTimer.Stop();
                    m_TimeTimer.Tick -= TimeTimer_Tick;
                }
                TestThread_Kint.OnStatusUpdate -= OnStatusUpdate_Main;
            }
            catch { }
            base.OnFormClosed(e);
        }
    }
}

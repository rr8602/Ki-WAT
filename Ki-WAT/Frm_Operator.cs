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

        public List<Form> m_FrmList = new List<Form>();

        private Form m_ActiveSubForm;
        public Frm_Operator()
        {
            InitializeComponent();
            this.SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.UserPaint | ControlStyles.DoubleBuffer, true);
            this.UpdateStyles();

            MoveFormToSecondMonitor();

        }


        private void Frm_Operator_Load(object sender, EventArgs e)
        {
            InitializeSubForm(m_frm_Oper_Wait);
            InitializeSubForm(m_frm_Oper_Test);
            InitializeSubForm(m_frm_Oper_Roll);
            InitializeSubForm(m_frm_Oper_Static);

            ShowFrm(2);
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

            if (screens.Length > 1)
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
    }
}

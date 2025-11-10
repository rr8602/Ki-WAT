using RollTester;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Ki_WAT
{
    public partial class Frm_PitInMonitor : Form
    {
        Frm_Mainfrm m_Parent;
        TblCarModel m_CurModel = new TblCarModel();
        GlobalVal _GV = GlobalVal.Instance;
        public Frm_PitInMonitor(Frm_Mainfrm pMain)
        {
            InitializeComponent();
            m_Parent = pMain;
            m_Parent.OnDppDataReceived += OnReceiveDpp;
            MoveFormToSecondMonitor();
        }
        private void MoveFormToSecondMonitor()
        {
            // 연결된 모든 모니터 가져오기
            Screen[] screens = Screen.AllScreens;
            if (screens.Length > 1)
            {
                // 두 번째 모니터 가져오기
                Screen secondScreen = screens[1];

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
                MessageBox.Show("세번째 모니터가 감지되지 않았습니다.");
            }
        }


        public void OnReceiveDpp(MeasureData pData)
        {

            if (this.InvokeRequired)
            {
                this.Invoke(new Action(() => OnReceiveDpp(pData)));
                return;
            }
   
            SetValueLeft((float)pData.dToeFL);
            SetValueRight((float)pData.dToeFR);
 
        }

        
        public void SetModel(TblCarModel pModel)
        {
            m_CurModel = pModel;

            Bar_Left.SetTargetAdjust(int.Parse(pModel.ToeFL_ST), int.Parse(pModel.ToeFL_AT), int.Parse(pModel.ToeFL_LT));
            Bar_Right.SetTargetAdjust(int.Parse(pModel.ToeFR_ST), int.Parse(pModel.ToeFR_AT), int.Parse(pModel.ToeFR_LT));
            Bar_Left.SetTarget(int.Parse(pModel.ToeFL_ST), 60);
            Bar_Right.SetTarget(int.Parse(pModel.ToeFR_ST), 60);
            Debug.Print("");
        }
        public void SetValueLeft( float fValue )
        {
            float targetValue = Int32.Parse(m_CurModel.ToeFL_ST);
            float yellowTol = Int32.Parse(m_CurModel.ToeFL_LT);
            float greenTol = Int32.Parse(m_CurModel.ToeFL_AT);

            int nBigScale = 5;
            // 바 그래프 값 설정
            Bar_Left.Value = fValue;

            float minRange = targetValue - nBigScale;
            float maxRange = targetValue + nBigScale;

            Bar_Left.SetTarget(targetValue, (fValue > minRange && fValue < maxRange) ? nBigScale : 60);

            

            // 경고등 범위 설정
            float yellowMin = targetValue - yellowTol;
            float yellowMax = targetValue + yellowTol;
            
            float greenMin = targetValue - greenTol;
            float greenMax = targetValue + greenTol;

            // 색상 표시 (라벨 배경)
            if (fValue >= greenMin && fValue <= greenMax)
            {
                lbl_Left_Value.BackColor = Color.Lime;
            }
            else if (fValue >= yellowMin && fValue <= yellowMax)
            {
                lbl_Left_Value.BackColor = Color.Yellow;
            }
            else
            {
                lbl_Left_Value.BackColor = Color.Red;
            }

            // 값 표시
            lbl_Left_Value.Text = fValue.ToString("F1");

        }
        public void SetValueRight(float fValue)
        {
            // 현재 모델 기준값과 허용 오차 파싱
            float targetValue = Int32.Parse(m_CurModel.ToeFR_ST);
            float yellowTol = Int32.Parse(m_CurModel.ToeFR_LT);
            float greenTol = Int32.Parse(m_CurModel.ToeFR_AT);

            // 바 값 설정
            Bar_Right.Value = fValue;
            float nBigScale = 5;
            // 바 우측 기준 범위 설정
            float minRange = targetValue - nBigScale;
            float maxRange = targetValue + nBigScale;

            Bar_Right.SetTarget(targetValue, (fValue > minRange && fValue < maxRange) ? nBigScale : 60);

            // 색상 경계값 설정
            float yellowMin = targetValue - yellowTol;
            float yellowMax = targetValue + yellowTol;

            float greenMin = targetValue - greenTol;
            float greenMax = targetValue + greenTol;

            // 배경 색상 표시
            if (fValue >= greenMin && fValue <= greenMax)
            {
                lbl_Right_Value.BackColor = Color.Lime;
            }
            else if (fValue >= yellowMin && fValue <= yellowMax)
            {
                lbl_Right_Value.BackColor = Color.Yellow;
            }
            else
            {
                lbl_Right_Value.BackColor = Color.Red;
            }

            // 값 표시
            lbl_Right_Value.Text = fValue.ToString("F1");
        }


        private void button1_Click(object sender, EventArgs e)
        {
            float nData = float.Parse(Txt_Left.Text);
            SetValueLeft(nData);

        }
    }
}
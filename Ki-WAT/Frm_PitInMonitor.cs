using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public partial class Frm_PitInMonitor : Form
    {
        public Frm_PitInMonitor()
        {
            InitializeComponent();
            MoveFormToSecondMonitor();
        }
        private void MoveFormToSecondMonitor()
        {
            // 연결된 모든 모니터 가져오기
            Screen[] screens = Screen.AllScreens;

            if (screens.Length > 1)
            {
                // 두 번째 모니터 가져오기
                Screen secondScreen = screens[0];

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
    }
}
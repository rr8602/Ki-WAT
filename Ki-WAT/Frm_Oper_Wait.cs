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
    public partial class Frm_Oper_Wait : Form
    {
        private Point mousePoint; // 현재 마우스 포인터의 좌표저장 변수 선언

        public Frm_Oper_Wait()
        {
            InitializeComponent();
        }
        private void button1_Click(object sender, EventArgs e)
        {
            Frm_Operator parent = (Frm_Operator)this.MdiParent;
            parent.ShowFrm(1);
        }

        private void Frm_Oper_Wait_MouseDown(object sender, MouseEventArgs e)
        {
            mousePoint = new Point(e.X, e.Y); //현재 마우스 좌표 저장
        }

        private void Frm_Oper_Wait_MouseMove(object sender, MouseEventArgs e)
        {
            if ((e.Button & MouseButtons.Left) == MouseButtons.Left) //마우스 왼쪽 클릭 시에만 실행
            {
                //폼의 위치를 드래그중인 마우스의 좌표로 이동 
                Location = new Point(Left - (mousePoint.X - e.X), Top - (mousePoint.Y - e.Y));
            }
        }
    }
}

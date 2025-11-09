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

        public static string LAMP_GREEN = @"Resource\green.png";
        public static string LAMP_GRAY = @"Resource\GRAY.png";
        public static string LAMP_RED = @"Resource\RED.png";
        GlobalVal _GV = GlobalVal.Instance;

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
        public void UpdateLampStatus()
        {
           
        }

        private void Frm_Oper_Wait_Load(object sender, EventArgs e)
        {
           

            Broker.dsBroker.Subscribe(Topics.DS.PLC, SetDevicePLC);
            Broker.dsBroker.Subscribe(Topics.DS.VEP, SetDeviceVEP);
            Broker.dsBroker.Subscribe(Topics.DS.Barcode, SetDeviceBarcode);
            Broker.dsBroker.Subscribe(Topics.DS.Printer, SetDevicePrinter);
            Broker.dsBroker.Subscribe(Topics.DS.Screw, SetDeviceScrew);
            Broker.dsBroker.Subscribe(Topics.DS.SWB, SetDeviceSWB);
            Broker.dsBroker.Subscribe(Topics.PLC.Data, GetPLCData);

        }
        private void SetDeviceScrew(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_ScrewState, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }

        private void SetDeviceSWB(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_swb, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }
        
        private void SetDevicePrinter(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_printer, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }
        private void SetDeviceBarcode(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_bar, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }
        private void SetDeviceVEP(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_vep, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }
        private void SetDevicePLC(object data)
        {
            String strStatus = data.ToString();
            SafeLoad(pic_lamp_io, strStatus == Topics.DS.Connect ? LAMP_GREEN : LAMP_RED);
        }
         

        private void GetPLCData(object data)
        {
            SafeLoad(pic_centering, _GV.plcRead.IsCenteringHome() ? LAMP_GREEN : LAMP_RED);
            SafeLoad(pic_swb, _GV.plcRead.IsSteeringWheelBalanceHome() ? LAMP_GREEN : LAMP_RED);
            SafeLoad(pic_hla, _GV.plcRead.IsHLAHomePosition() ? LAMP_GREEN : LAMP_RED);
            SafeLoad(pic_detect, !_GV.plcRead.IsFrontCarDetection() && !_GV.plcRead.IsRearCarDetection()  ? LAMP_GREEN : LAMP_RED);
            SafeLoad(pic_safety, _GV.plcRead.IsSafetyKeyOn()  ? LAMP_GREEN : LAMP_RED);
            SafeLoad(pic_screw, _GV.plcRead.IsScrewDriverLeftHome() && _GV.plcRead.IsScrewDriverRightHome()  ? LAMP_GREEN : LAMP_RED);


            
        }

        private void SafeLoad(PictureBox pictureBox, string imagePath)
        {
            if (pictureBox.InvokeRequired)
                pictureBox.Invoke((Action)(() => pictureBox.Load(imagePath)));
            else
                pictureBox.Load(imagePath);
        }

 



    }
}

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
    public partial class Frm_DigitalIOIn : Form
    {
        TableLayoutPanel table = new TableLayoutPanel();

        private CancellationTokenSource _cts;
        GlobalVal _GV = GlobalVal.Instance;

        private byte _byteTemp = 0x00;

        public Frm_DigitalIOIn()
        {
            InitializeComponent();
        }

        private void Frm_DigitalIOIn_Load(object sender, EventArgs e)
        {
            SetLable();
            RefreshData();

        }

        private void Frm_DigitalIOIn_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelPollingToken();
        }


        private void SetLable()
        {
            table.Size = new Size(1700, 560); // 
            table.Location = new Point(10, 10); // 원하는 위치 지정

            table.Margin = new Padding(0);
            table.Padding = new Padding(0);
            table.RowCount = 18;
            table.ColumnCount = 9;
            //table.Dock = DockStyle.Fill;
            table.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            table.CellBorderStyle = TableLayoutPanelCellBorderStyle.Single;

            table.BackColor = Color.LightGray;
            for (int i = 0; i < 1; i++)
            {
                for (int j = 0; j < table.ColumnCount; j++)
                {
                    Label lbl = new Label();
                    lbl.Margin = new Padding(0);
                    if (j == 0) lbl.Text = "Address";
                    if (j == 1) lbl.Text = "0";
                    if (j == 2) lbl.Text = "1";
                    if (j == 3) lbl.Text = "2";
                    if (j == 4) lbl.Text = "3";
                    if (j == 5) lbl.Text = "4";
                    if (j == 6) lbl.Text = "5";
                    if (j == 7) lbl.Text = "6";
                    if (j == 8) lbl.Text = "7";

                    lbl.BorderStyle = BorderStyle.FixedSingle;

                    lbl.Size = new Size(186, 30);  // 강제 크기 지정
                    lbl.Dock = DockStyle.Fill;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;
                    lbl.Font = new Font("Arial", 10, FontStyle.Bold);
                    lbl.BackColor = Color.LightGray;

                    // Tag 속성에 좌표 또는 데이터를 저장
                    lbl.Tag = new Point(i, j); // 또는 (i * cols + j) 등 원하는 값
                    table.Controls.Add(lbl, j, i);  // (column, row)
                }
            }
            for (int i = 1; i < table.RowCount; i++)
            {
                for (int j = 0; j < table.ColumnCount; j++)
                {

                    Label lbl = new Label();
                    lbl.Margin = new Padding(0);
                    lbl.BorderStyle = BorderStyle.FixedSingle;

                    lbl.AutoSize = false;
                    lbl.Size = new Size(80, 30);  // 강제 크기 지정
                    lbl.Dock = DockStyle.Fill;
                    lbl.TextAlign = ContentAlignment.MiddleCenter;



                    SetLableDesc(lbl, i, j);
                    // Tag 속성에 좌표 또는 데이터를 저장
                    lbl.Tag = new Point(i, j); // 또는 (i * cols + j) 등 원하는 값
                    table.Controls.Add(lbl, j, i);  // (column, row)
                }
            }




            this.Controls.Add(table);

        }
        private void SetLableDesc(Label lbl, int row, int col)
        {
            String strAddress = "";
            String strDesc = "";
            if (row == 1) strAddress = "Address : 0";
            if (row == 2) strAddress = "Address : 1";
            if (row == 3) strAddress = "Address : 2";
            if (row == 4) strAddress = "Address : 3";
            if (row == 5) strAddress = "Address : 10";
            if (row == 6) strAddress = "Address : 11";
            if (row == 7) strAddress = "Address : 60";
            if (row == 8) strAddress = "Address : 61";
            if (row == 9) strAddress = "Address : 62";
            if (row == 10) strAddress = "Address : 90";
            if (row == 11) strAddress = "Address : 91";
            if (row == 12) strAddress = "Address : 100";
            if (row == 13) strAddress = "Address : 110";
            if (row == 14) strAddress = "Address : 181";
            if (row == 15) strAddress = "Address : 220";
            if (row == 16) strAddress = "Address : ___";
            if (row == 17) strAddress = "Address : ___";


            if (col == 0)
            {
                lbl.Text = strAddress;
                lbl.BackColor = Color.LightGray;
                lbl.Font = new Font("Arial", 10, FontStyle.Bold);
            }
            else
            {
                strDesc = GetDesc(row, col);
                lbl.Text = strDesc;
                lbl.BackColor = Color.White;
                lbl.Font = new Font("Arial", 10, FontStyle.Regular);
            }
        }

        private String GetDesc(int row, int col)
        {


            if (row <= 0) return "";

            row = row - 1;
            col = col - 1;





            if (row == 0 && col == 0) return "Home Position";
            if (row == 0 && col == 1) return "Ready Position";
            if (row == 0 && col == 2) return "Run-out Position";
            if (row == 0 && col == 3) return "Stable Position";
            if (row == 0 && col == 4) return "Measurement Position";
            if (row == 0 && col == 5) return "Exit Position";
            if (row == 0 && col == 6) return "Re-Start Position";
            if (row == 0 && col == 7) return "Bypass Start";
            
            
            if (row == 1 && col == 0) return "Bypass End";
            if (row == 1 && col == 1) return "Ready HLA Position";
            if (row == 1 && col == 2) return "Ready Calibration Static";




            if (row == 2 && col == 0) return "PEV Degraded Mode";
            if (row == 2 && col == 1) return "Printer Degraded Mode";
            if (row == 2 && col == 2) return "Barcode Degraded Mode";
            if (row == 2 && col == 3) return "Left Screw Degraded";
            if (row == 2 && col == 4) return "Right Screw Degraded";
            if (row == 2 && col == 5) return "HLA Degraded";
            if (row == 2 && col == 6) return "Wheel Adjust Degraded";
            if (row == 2 && col == 7) return "Steering Wheel Degraded";


            if (row == 3 && col == 0) return "Floating Plate Degraded";




            if (row == 4 && col == 0) return "Start Cycle";
            if (row == 4 && col == 1) return "ByPass Cycle";
            if (row == 4 && col == 2) return "Restart Cycle";
            if (row == 4 && col == 3) return "Re-Print Cycle";
            if (row == 4 && col == 4) return "Safety Stop";
            if (row == 4 && col == 5) return "Measuring Start";
            if (row == 4 && col == 6) return "Pit In Left Fin";
            if (row == 4 && col == 7) return "Pit In Right Fin";

            if (row == 5 && col == 0) return "Exit Platform";
            
            if (row == 5 && col == 2) return "Auto Mode";
            if (row == 5 && col == 3) return "Manual Mode";
            if (row == 5 && col == 4) return "Cali Rolling Master";
            if (row == 5 && col == 5) return "Cali Static Master";
            if (row == 5 && col == 6) return "Emergency";


            if (row == 6 && col == 0) return "Acknowlege 1";
            if (row == 6 && col == 1) return "Acknowlege 2";
            if (row == 6 && col == 2) return "Acknowlege 3";
            if (row == 6 && col == 3) return "Acknowlege 4";
            if (row == 6 && col == 4) return "Acknowlege 5";
            if (row == 6 && col == 5) return "Acknowlege 6";
            if (row == 6 && col == 6) return "Acknowlege 7";
            if (row == 6 && col == 7) return "Acknowlege 8";


            if (row == 7 && col == 0) return "Acknowlege 9";
            if (row == 7 && col == 1) return "Acknowlege 10";
            if (row == 7 && col == 2) return "Acknowlege 11";
            if (row == 7 && col == 3) return "Acknowlege 12";



            if (row == 8 && col == 0) return "Acknowlege 13";
            if (row == 8 && col == 1) return "Acknowlege 14";
            if (row == 8 && col == 2) return "Acknowlege 15";
            if (row == 8 && col == 3) return "Acknowlege 16";
            if (row == 8 && col == 4) return "Acknowlege 17";
            if (row == 8 && col == 5) return "Acknowlege 18";





            if (row == 9 && col == 0) return "Centering Home";
            if (row == 9 && col == 1) return "Centering On";
            if (row == 9 && col == 2) return "Floating Plate Lock";
            if (row == 9 && col == 3) return "Floationg Plate Free";
            if (row == 9 && col == 4) return "Moving Plate Home";
            if (row == 9 && col == 5) return "Moving Plate Adjust";
            if (row == 9 && col == 6) return "Safety Roller Up";
            if (row == 9 && col == 7) return "Safety Roller Down";


            if (row == 10 && col == 0) return "3D Camera Sensor On";
            if (row == 10 && col == 1) return "Front Car Dectection";
            if (row == 10 && col == 2) return "Rear Car Dectection";
            if (row == 10 && col == 3) return "HLA Home Position";
            if (row == 10 && col == 4) return "Steering Wheel Home";


            if (row == 11 && col == 0) return "UnWrench Tool Home";
            if (row == 11 && col == 1) return "Screw Left Home";
            if (row == 11 && col == 2) return "Screw Right Home";
            if (row == 11 && col == 3) return "Scanner Laser On";
            if (row == 11 && col == 4) return "Safety Key On";




            if (row == 12 && col == 0) return "Wheelbase Moving";
            if (row == 12 && col == 1) return "Wheelbase Home Position";
            if (row == 12 && col == 2) return "Wheelbase In Position";




            if (row == 13 && col == 6) return "Motor Run";
            if (row == 13 && col == 7) return "Motor Stop";

            if (row == 14 && col == 0) return "Sistema pronto";
            if (row == 14 && col == 1) return "Sequenceia OK";
            if (row == 14 && col == 2) return "HLA Position Init";
            if (row == 14 && col == 3) return "HLA Home";


            return "";
        }


        private void CancelPollingToken()
        {
            if (_cts != null && !_cts.IsCancellationRequested)
            {
                try { _cts.Cancel(); }
                catch (ObjectDisposedException) { } // 이미 Dispose된 경우 무시
            }
        }
        private void RefreshData()
        {
            CancelPollingToken();
            _cts = new CancellationTokenSource();
            Task.Run(() => RefreshDataTask(_cts.Token));
        }
        private void RefreshDataTask(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                try
                {
                    ShowStatus();
                }
                catch (Exception ex)
                {
                    GWA.STM("ShowStatus Error: " + ex.Message);
                }
                Thread.Sleep(100);
            }
        }
        private void ShowStatus()
        {
            if (InvokeRequired)
            {
                try
                {
                    Invoke(new Action(ShowStatus)); // UI 스레드에서 다시 실행
                }
                catch (ObjectDisposedException) { }
                return;
            }
            //GWA.STM("SHOW PLC");
            byte[] byData = _GV.plcRead.rawDataIn;

            if (byData[0] != _byteTemp)
            {
                _byteTemp = byData[0];

                //stcTest.Text = _byteTemp.ToString();
            }

            //if (row == 1) strAddress = "Address : 0";
            //if (row == 2) strAddress = "Address : 1";
            //if (row == 3) strAddress = "Address : 2";
            //if (row == 4) strAddress = "Address : 3";
            //if (row == 5) strAddress = "Address : 10";
            //if (row == 6) strAddress = "Address : 11";
            //if (row == 7) strAddress = "Address : 60";
            //if (row == 8) strAddress = "Address : 61";
            //if (row == 9) strAddress = "Address : 62";
            //if (row == 10) strAddress = "Address : 90";
            //if (row == 11) strAddress = "Address : 91";
            //if (row == 12) strAddress = "Address : 100";
            //if (row == 13) strAddress = "Address : 110";
            //if (row == 14) strAddress = "Address : 181";
            //if (row == 15) strAddress = "Address : 220";
            //if (row == 16) strAddress = "Address : ___";
            //if (row == 17) strAddress = "Address : ___";

            SetLabelColor(1, byData[0]);
            SetLabelColor(2, byData[1]);
            SetLabelColor(3, byData[2]);
            SetLabelColor(4, byData[3]);
            SetLabelColor(5, byData[10]);

            SetLabelColor(6, byData[11]);
            SetLabelColor(7, byData[60]);
            SetLabelColor(8, byData[61]);
            SetLabelColor(9, byData[62]);
            SetLabelColor(10, byData[90]);

            SetLabelColor(11, byData[91]);
            SetLabelColor(12, byData[100]);
            SetLabelColor(13, byData[110]);
            SetLabelColor(14, byData[181]);
            SetLabelColor(15, byData[220]);

            
            stcProg.Text = _GV.plcRead.GetProg().ToString();
            stcJour.Text = _GV.plcRead.GetJour().ToString();
            stcIdent.Text = _GV.plcRead.GetIdent().ToString();
            stcDiv.Text = _GV.plcRead.GetDiv().ToString();

        }

        private void SetLabelColor(int row, byte byData)
        {
            if (table.InvokeRequired)
            {
                table.Invoke(new Action(() => SetLabelColor(row, byData)));
                return;
            }

            for (int i = 1; i < 8; i++)
            {
                Control ctrl = table.GetControlFromPosition(i, row);

                bool bOn = GetBit(byData, i - 1);

                if (ctrl is Label lbl)
                {
                    if (bOn) lbl.BackColor = Color.LightGreen;
                    else lbl.BackColor = Color.White;
                }

            }

        }

        private void SetLabelColor(int row, int col, bool bOn)
        {
            Control ctrl = table.GetControlFromPosition(col, row);


            if (ctrl is Label lbl)
            {
                if (bOn) lbl.BackColor = Color.LightGreen;
                else lbl.BackColor = Color.White;
            }
        }

        public bool GetBit(byte byData, int nBitPos)
        {
            if (nBitPos < 0 || nBitPos > 7)
                return false;


            return (byData & (1 << nBitPos)) != 0;
        }
    }
}

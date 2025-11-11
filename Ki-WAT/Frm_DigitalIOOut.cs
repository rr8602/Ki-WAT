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
    public partial class Frm_DigitalIOOut : Form
    {

        TableLayoutPanel table = new TableLayoutPanel();
        private CancellationTokenSource _cts;
        GlobalVal _GV = GlobalVal.Instance;


        public Frm_DigitalIOOut()
        {
            InitializeComponent();
        }

        private void Frm_DigitalIOOut_Load(object sender, EventArgs e)
        {
            SetLable();
            RefreshData();
            ShowStatus();

        }

        private void Frm_DigitalIOOut_FormClosing(object sender, FormClosingEventArgs e)
        {

        }


        private void SetLable()
        {
            table.Size = new Size(1700, 590); // 
            table.Location = new Point(10, 10); // 원하는 위치 지정

            table.Margin = new Padding(0);
            table.Padding = new Padding(0);
            table.RowCount = 19;
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

                    lbl.Click += Label_Click;

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
            if (row == 7) strAddress = "Address : 20";
            if (row == 8) strAddress = "Address : 60";
            if (row == 9) strAddress = "Address : 61";
            if (row == 10) strAddress = "Address : 62";
            if (row == 11) strAddress = "Address : 90";
            if (row == 12) strAddress = "Address : 91";
            if (row == 13) strAddress = "Address : 110";
            if (row == 14) strAddress = "Address : 180";
            if (row == 15) strAddress = "Address : 190";
            if (row == 16) strAddress = "Address : 200";
            if (row == 17) strAddress = "Address : 210";
            if (row == 18) strAddress = "Address : 220";

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
            if (row == 0 && col == 2) return "RunOut Position";
            if (row == 0 && col == 3) return "Stable Position";
            if (row == 0 && col == 4) return "Test Position";
            if (row == 0 && col == 5) return "Exit Position";
            if (row == 0 && col == 6) return "Re-Start Position";
            if (row == 0 && col == 7) return "Bypass Start";


            if (row == 1 && col == 0) return "Bypass End";
            if (row == 1 && col == 1) return "Ready HLA Position";


            if (row == 2 && col == 0) return "PEV Degraded Mode";
            if (row == 2 && col == 1) return "Printer Degraded Mode";
            if (row == 2 && col == 2) return "Barcode Degraded Mode";
            if (row == 2 && col == 3) return "Left Screw Degraded";
            if (row == 2 && col == 4) return "Right Screw Degraded";
            if (row == 2 && col == 5) return "HLA Degraded";
            if (row == 2 && col == 6) return "Wheel Adjust Degraded";
            if (row == 2 && col == 7) return "Steering Wheel Degraded";


            if (row == 3 && col == 0) return "Floating Plate Degraded";


            if (row == 4 && col == 0) return "Priter Fault";
            if (row == 4 && col == 1) return "Barcode Fault";
            if (row == 4 && col == 2) return "Traceability PC Fault";
            if (row == 4 && col == 3) return "VEP Fault";




            if (row == 5 && col == 0) return "Test Result";
            if (row == 5 && col == 1) return "Test Failed";
            if (row == 5 && col == 4) return "3D Camera Sensor Failed";
            if (row == 5 && col == 5) return "ScrewDriver Failed";
            if (row == 5 && col == 6) return "HLA Failed";



            if (row == 6 && col == 0) return "BarCode OK";




            if (row == 7 && col == 0) return "AV - PARALLELISME HORS TOLERANCE";
            if (row == 7 && col == 1) return "AV - ANGLE DE VOLANT HORS TOLER";
            if (row == 7 && col == 2) return "AV - CARROSSAGE HORS TOLER";
            if (row == 7 && col == 3) return "ARRIERE - PARALLELISME HORS TOLERANCE";
            if (row == 7 && col == 4) return "ARRIERE - ANGLE DE POUSSEE HORS TOLER";
            if (row == 7 && col == 5) return "ARRIERE - CARROSSAGE HORS TOLER";
            if (row == 7 && col == 6) return "PARALLELISME - DEFAUT COM PEV";
            if (row == 7 && col == 7) return "PARALLELISME - ABANDON CYCLE";




            if (row == 8 && col == 0) return "PARALLELISME - PANNE INSTALLATION";
            if (row == 8 && col == 1) return "FREINAGE - VISSAGE BANC NC";
            if (row == 8 && col == 2) return "AV - COUPLE SERRAGE PARAL AV NC";
            if (row == 8 && col == 3) return "ARRIERE - COUPLE SERRAGE PARAL AR NC";




            if (row == 9 && col == 0) return "PROJECTEURS - REGLAGE NON-CONFORME";
            if (row == 9 && col == 1) return "PROJECTEURS - NON-CONFORME";
            if (row == 9 && col == 2) return "PROJECTEURS - PRE-REGL NC";
            if (row == 9 && col == 3) return "PROJECTEURS - MQ AUTORIS REGL";
            if (row == 9 && col == 4) return "REGLAGE PROJECTEURS - ABANDON CYCLE";
            if (row == 9 && col == 5) return "REGLAGE PROJECTEURS - PANNE INSTALLATION";





            if (row == 10 && col == 0) return "Centering Home";
            if (row == 10 && col == 1) return "Centering ON";
            if (row == 10 && col == 2) return "Floating plate Lock";
            if (row == 10 && col == 3) return "Floating plate Free";
            if (row == 10 && col == 4) return "Moving plate Home";
            if (row == 10 && col == 5) return "Moving plate Adjust";
            if (row == 10 && col == 6) return "Safety Roller Up";
            if (row == 10 && col == 7) return "Safety Roller Down";


            if (row == 11 && col == 0) return "3D Camera sensor Enable";



            if (row == 12 && col == 0) return "Wheelbase Moving";
            if (row == 12 && col == 1) return "Wheelbase Home Position";
            if (row == 12 && col == 2) return "Wheelbase In Position";

            if (row == 13 && col == 0) return "Motor Run";
            if (row == 13 && col == 1) return "Motor Stop";


            if (row == 14 && col == 0) return "Accurancy Check";
            if (row == 14 && col == 0) return "Measurement Finished";
            if (row == 14 && col == 0) return "Calibration OK";



            if (row == 15 && col == 0) return "Tolerance OK";



            if (row == 16 && col == 0) return "VEP Job Finished";


            if (row == 17 && col == 0) return "HLA Job Finished";


            return "";
        }
        private void Label_Click(object sender, EventArgs e)
        {
            Label lbl = sender as Label;

            if (lbl != null && lbl.Tag is Point pos)
            {
                Point posTemp = (Point)lbl.Tag;

                if (posTemp.Y == 0) return;
                int row = pos.X;
                int col = pos.Y;


                GWA.STM("COL" + col.ToString() + " ROW" + row.ToString());
                int bit = col - 1;
                int dataPos = -1;


                //if (row == 1) strAddress = "Address : 0";
                //if (row == 2) strAddress = "Address : 1";
                //if (row == 3) strAddress = "Address : 2";
                //if (row == 4) strAddress = "Address : 3";
                //if (row == 5) strAddress = "Address : 10";
                //if (row == 6) strAddress = "Address : 11";
                //if (row == 7) strAddress = "Address : 20";
                //if (row == 8) strAddress = "Address : 60";
                //if (row == 9) strAddress = "Address : 61";
                //if (row == 10) strAddress = "Address : 62";
                //if (row == 11) strAddress = "Address : 90";
                //if (row == 12) strAddress = "Address : 91";
                //if (row == 13) strAddress = "Address : 110";
                //if (row == 14) strAddress = "Address : 180";
                //if (row == 15) strAddress = "Address : 190";
                //if (row == 16) strAddress = "Address : 200";
                //if (row == 17) strAddress = "Address : 210";
                //if (row == 18) strAddress = "Address : 220";


                if (row == 1) dataPos = 0;
                if (row == 2) dataPos = 1;
                if (row == 3) dataPos = 2;
                if (row == 4) dataPos = 3;
                if (row == 5) dataPos = 10;
                if (row == 6) dataPos = 11;
                if (row == 7) dataPos = 20;
                if (row == 8) dataPos = 60;
                if (row == 9) dataPos = 61;
                if (row == 10) dataPos = 62;
                if (row == 11) dataPos = 90;
                if (row == 12) dataPos = 91;
                if (row == 13) dataPos = 110;
                if (row == 14) dataPos = 180;
                if (row == 15) dataPos = 190;
                if (row == 16) dataPos = 200;
                if (row == 17) dataPos = 210;
                if (row == 18) dataPos = 220;

                string value = lbl.Text;
                if (lbl.BackColor == Color.White)
                {
                    
                    if (dataPos != -1)
                    {
                        _GV.plcWrite.SetBit(dataPos, bit, true);
                        //_GV.plcWrite.WritePLCData(dataPos);
                        _GV.plcWrite.WritePLCData();
                        RefreshData();
                    }
                }
                else
                {
                    
                    if (dataPos != -1)
                    {
                        _GV.plcWrite.SetBit(dataPos, bit, false);
                        //_GV.plcWrite.WritePLCData(dataPos);
                        _GV.plcWrite.WritePLCData();
                        RefreshData();
                    }
                }
            }
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
            Thread.Sleep(300);
            _GV.plcWrite.GetPLCOutData();
            while (!token.IsCancellationRequested)
            {
                Thread.Sleep(300);
                ShowStatus();
                break;
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
            byte[] byData = _GV.plcWrite.rawDataOut;



            //if (row == 1) strAddress = "Address : 0";
            //if (row == 2) strAddress = "Address : 1";
            //if (row == 3) strAddress = "Address : 2";
            //if (row == 4) strAddress = "Address : 3";
            //if (row == 5) strAddress = "Address : 10";
            //if (row == 6) strAddress = "Address : 11";
            //if (row == 7) strAddress = "Address : 20";
            //if (row == 8) strAddress = "Address : 60";
            //if (row == 9) strAddress = "Address : 61";
            //if (row == 10) strAddress = "Address : 62";
            //if (row == 11) strAddress = "Address : 90";
            //if (row == 12) strAddress = "Address : 91";
            //if (row == 13) strAddress = "Address : 110";
            //if (row == 14) strAddress = "Address : 180";
            //if (row == 15) strAddress = "Address : 190";
            //if (row == 16) strAddress = "Address : 200";
            //if (row == 17) strAddress = "Address : 210";
            //if (row == 18) strAddress = "Address : 220";

            SetLabelColor(1, byData[0]);
            SetLabelColor(2, byData[1]);
            SetLabelColor(3, byData[2]);
            SetLabelColor(4, byData[3]);
            SetLabelColor(5, byData[10]);

            SetLabelColor(6, byData[11]);
            SetLabelColor(7, byData[20]);
            SetLabelColor(8, byData[60]);
            SetLabelColor(9, byData[61]);
            SetLabelColor(10, byData[62]);

            SetLabelColor(11, byData[90]);
            SetLabelColor(12, byData[91]);
            SetLabelColor(13, byData[110]);
            SetLabelColor(14, byData[180]);
            SetLabelColor(15, byData[190]);
            SetLabelColor(16, byData[200]);
            SetLabelColor(17, byData[210]);
            SetLabelColor(18, byData[220]);


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

        private void btnWheelbase_Click(object sender, EventArgs e)
        {
            String str = comWheelbase.SelectedItem.ToString();
            String strDistance = editWheelbase.Text;


            int distance;

            if (int.TryParse(strDistance, out distance))
            {
            }
            else
            {
                MessageBox.Show("입력값이 올바른 숫자가 아닙니다.", "입력 오류", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                distance = 0; // 또는 기본값 설정
                return;
            }

            if (str == "1")
            {
                _GV.plcWrite.SetLeftDivDistance(1, distance);
                _GV.plcWrite.SetRightDivDistance(1, distance);
            }
            else if (str == "2")
            {
                _GV.plcWrite.SetLeftDivDistance(2, distance);
                _GV.plcWrite.SetRightDivDistance(2, distance);
            }
            else if (str == "3")
            {
                _GV.plcWrite.SetLeftDivDistance(3, distance);
                _GV.plcWrite.SetRightDivDistance(3, distance);
            }
            else if (str == "4")
            {
                _GV.plcWrite.SetLeftDivDistance(4, distance);
                _GV.plcWrite.SetRightDivDistance(4, distance);
            }
            else if (str == "5")
            {
                _GV.plcWrite.SetLeftDivDistance(5, distance);
                _GV.plcWrite.SetRightDivDistance(5, distance);
            }
            else if (str == "6")
            {
                _GV.plcWrite.SetLeftDivDistance(6, distance);
                _GV.plcWrite.SetRightDivDistance(6, distance);
            }
            else if (str == "7")
            {
                _GV.plcWrite.SetLeftDivDistance(7, distance);
                _GV.plcWrite.SetRightDivDistance(7, distance);
            }
            else if (str == "8")
            {
                _GV.plcWrite.SetLeftDivDistance(8, distance);
                _GV.plcWrite.SetRightDivDistance(8, distance);
            }
            else if (str == "9")
            {
                _GV.plcWrite.SetLeftDivDistance(9, distance);
                _GV.plcWrite.SetRightDivDistance(8, distance);
            }
            else if (str == "10")
            {
                _GV.plcWrite.SetLeftDivDistance(10, distance);
                _GV.plcWrite.SetRightDivDistance(10, distance);
            }
            else if (str == "HOME")
            {
                _GV.plcWrite.SetLeftHome(distance);
                _GV.plcWrite.SetRightHome(distance);
            }
            _GV.plcWrite.WritePLCData();
        }

        private void btn_PJI_Click(object sender, EventArgs e)
        {
            _GV.plcWrite.SetPJI(txx_PJI.Text);
            _GV.plcWrite.WritePLCData();
            Thread.Sleep(500);
            Refresh();
            ShowStatus();

        }

        private void btn_div_Click(object sender, EventArgs e)
        {
            _GV.plcWrite.SetDiv(ushort.Parse(txt_div.Text));
            _GV.plcWrite.WritePLCData();

            Thread.Sleep(500);
            Refresh();
            ShowStatus();
        }
    }
}

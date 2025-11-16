using DG_ComLib;
using Ki_WAT.Properties;
using KINT_Lib;
using RollTester;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using static KINT_Lib.Lib_TcpClient;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ki_WAT
{
    public partial class Frm_Main : Form
    {
        Frm_Mainfrm m_frmParent;

        public static string LAMP_GREEN = @"Resource\green.png";
        public static string LAMP_GRAY = @"Resource\GRAY.png";
        public static string LAMP_RED = @"Resource\RED.png";
        

        private delegate void _d_SetStatusListMsg(string strText);
        GlobalVal _GV = GlobalVal.Instance;
        //TblCarModel m_Cur_Model = new TblCarModel();
        //TblCarInfo m_Cur_CarInfo = new TblCarInfo();
        System.Windows.Forms.Timer m_timerScreen = new System.Windows.Forms.Timer();
        List<TblCarInfo> m_BarcodeList = new List<TblCarInfo>();

        private Task _barcodePollingTask;
        private CancellationTokenSource _cancellationTokenSource;

      

        public Frm_Main()
        {
            InitializeComponent();
        }
        private void Frm_Main_Load(object sender, EventArgs e)
        {

            seqList.Columns.Add("", 0);
            seqList.Columns.Add("UID", seqList.Width / 2 , HorizontalAlignment.Center);
            seqList.Columns.Add("PJI", seqList.Width / 2, HorizontalAlignment.Center);

            RefreshCarInfoList();
			// 상태 업데이트 이벤트 구독
			
           //m_frmParent.m_BarcodeComm.OnDataReceived += new DataReceiveClient(event_GetBarcode);

            m_timerScreen.Enabled = true;
            m_timerScreen.Interval = 200; // 1초마다
            m_timerScreen.Tick += new EventHandler(CopyScreen_Tick);
            
            Pic_copy.Location = new Point(9, 88);
            Pic_copy.Size = new Size(799, 625);

            StartBarcodePolling();
            _GV._TestThread.StartThread();

            TestThread_Kint.OnStatusUpdate += OnStatusUpdate_Main;
            TestThread_Kint.OnCycleFinished += OnCycleFinished;
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


        public void StartBarcodePolling()
        {
            _cancellationTokenSource = new CancellationTokenSource();
            _barcodePollingTask = Task.Run(() => PollBarcodeList(_cancellationTokenSource.Token));
        }

        private async Task PollBarcodeList(CancellationToken cancellationToken)
        {
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    if (m_BarcodeList != null && m_BarcodeList.Count > 0 && _GV.m_bTestRun == false)
                    {

                        _GV.m_Cur_Info = m_BarcodeList[0];
                        _GV.m_Cur_Model = _GV._dbJob.SelectCarModel(_GV.m_Cur_Info.CarModel);
                        // 사이클 시작
                        RefreshCurrentInfoSafe();
                        StartCycle();
                        m_BarcodeList.Clear(); // 처리 후 리스트 클리어
                    }

                   
                    await Task.Delay(100, cancellationToken); // 100ms 간격으로 폴링
                }
                catch (OperationCanceledException)
                {
                    break;
                }
                catch (Exception ex)
                {
                    // 로그 처리
                    Debug.WriteLine($"Barcode polling error: {ex.Message}");
                }
            }
        }

        public void StopBarcodePolling()
        {
            _cancellationTokenSource?.Cancel();
            _barcodePollingTask?.Wait(3000); // 최대 3초 대기
        }

        private void CopyScreen_Tick(object sender, EventArgs e)
        {
            try
            {
                if (_GV.m_bTestRun)
                {
                    GWA.CaptureFormToPictureBox(Pic_copy, m_frmParent.User_Monitor);
                }
            }
            catch { }
        }
        public void ParseBarcode(string pBarcode)
        {
            string PP = pBarcode.Substring(0, 2);
            string PJI = pBarcode.Substring(2, 7);
            string PARA = pBarcode.Substring(9, 3);
            string PROJECT = pBarcode.Substring(12, 3);
            //string ADAS = pBarcode.Substring(15, 3);
            _GV.m_Cur_Model = _GV._dbJob.SelectCarModelFromBarcode(PARA);
            Debug.Print("");
            _GV.m_Cur_Info.CarPJINo = PJI;
            _GV.m_Cur_Info.CarModel = _GV.m_Cur_Model.Model_NM;
            _GV.m_Cur_Info.WatCycle = PARA;
            _GV.m_Cur_Info.LetCycle = PROJECT;
            _GV.m_Cur_Info.Car_Step = "0";
            _GV.m_Cur_Info.TotalBar = pBarcode;
            _GV.m_Cur_Info.Spare__2 = "0";
            _GV.m_Cur_Info.Spare__3 = "0";
            _GV._dbJob.AddNewAcceptNo(ref _GV.m_Cur_Info);
            RefreshCarInfoList();
        }
        public void GetBarCode(string pBarcode)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => GetBarCode(pBarcode)));
                return;
            }

            try
            {
                if (m_BarcodeList.Count > 1)
                {
                    _Log_ListBox("Can't register new vehicle " + pBarcode);
                    return;
                }

                if (pBarcode.Length < 15)
                {
                    _Log_ListBox("Barcode Length : " + pBarcode);
                    return;
                }

                string PP = pBarcode.Substring(0, 2);
                string PJI = pBarcode.Substring(2, 7);
                string PARA = pBarcode.Substring(9, 3);
                string PROJECT = pBarcode.Substring(12, 3);

                TblCarInfo info = new TblCarInfo();
                TblCarModel model = _GV._dbJob.SelectCarModelFromBarcode(PARA);

                info.CarPJINo = PJI;
                info.CarModel = model.Model_NM;
                info.WatCycle = PARA;
                info.LetCycle = PROJECT;
                info.Car_Step = "0";
                info.TotalBar = pBarcode;
                info.Spare__2 = "0";
                info.Spare__3 = "0";

                _GV._dbJob.AddNewAcceptNo(ref info);
                m_BarcodeList.Add(info);
                RefreshCarInfoList();

                if (_GV.m_bTestRun)
                {
                    lbl_NextPJI.Text = m_BarcodeList[0].CarPJINo;
                    m_frmParent.User_Monitor.SetNextPJI(m_BarcodeList[0].CarPJINo);
                }

                _Log_ListBox("Barcode : " + pBarcode);

            }
            catch (Exception ex)
            {
                _Log_ListBox(ex.Message);
            }
        }
       
        // 폼 종료 시 이벤트 해제
        protected override void OnFormClosed(FormClosedEventArgs e)
		{
			TestThread_Kint.OnStatusUpdate -= OnStatusUpdate_Main;
            StopBarcodePolling();

            base.OnFormClosed(e);
		}
        public void RefreshCarInfoList()
        {
            if (m_frmParent == null || _GV._dbJob == null)
                return;

            try
            {
                // 오늘 날짜 yyyyMMdd
                string today = DateTime.Today.ToString("yyyyMMdd");
                string sSQL = $"SELECT * FROM TableCarInfo WHERE AcceptNo LIKE '{today}%' ORDER BY AcceptNo DESC";
                var dt = _GV._dbJob.GetDataSet(sSQL);

                if (seqList.InvokeRequired)
                {
                    seqList.Invoke(new Action(() => UpdateCarList(dt)));
                }
                else
                {
                    UpdateCarList(dt);
                }
            }
            catch (Exception ex)
            {
                // MessageBox도 UI 스레드에서 실행해야 안전
                if (this.InvokeRequired)
                {
                    this.Invoke(new Action(() =>
                    {
                        MessageBox.Show($"CarInfo 리스트 새로고침 중 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }));
                }
                else
                {
                    MessageBox.Show($"CarInfo 리스트 새로고침 중 오류: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void UpdateCarList(DataTable dt)
        {
            seqList.Items.Clear();

            if (dt != null && dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    string acceptNo = row["AcceptNo"].ToString();
                    string carPjiNo = row["CarPJINo"].ToString();

                    ListViewItem item = new ListViewItem(acceptNo);
                    item.SubItems.Add(acceptNo); // UID
                    item.SubItems.Add(carPjiNo); // PJI
                    seqList.Items.Add(item);
                }
            }
        }
        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }
        private void button1_Click(object sender, EventArgs e)
        {
            _Log_ListBox("asdf");
        }


        public void RefreshCurrentInfoSafe()
        {
            if (this.InvokeRequired)
            {
                this.Invoke(new MethodInvoker(RefreshCurrentInfo));
            }
            else
            {
                RefreshCurrentInfo();
            }
        }

        public void RefreshCurrentInfo()
        {
            lbl_PARA.Text = _GV.m_Cur_Model.Bar_Code;
            lbl_ModelNM.Text = _GV.m_Cur_Model.Model_NM;
            lbl_WB.Text = _GV.m_Cur_Model.WhelBase;
            lbl_LET.Text = _GV.m_Cur_Info.LetCycle;
            lbl_PJI.Text = _GV.m_Cur_Info.CarPJINo;
            lbl_Barcode.Text = _GV.m_Cur_Info.TotalBar;

            int nMin, nMax;

            DogRunST.Text = _GV.m_Cur_Model.DogRunST;
            
            nMin = Int32.Parse(_GV.m_Cur_Model.DogRunST) - Int32.Parse(_GV.m_Cur_Model.DogRunLT);
            DogRunLT_MIN.Text = nMin.ToString();

            nMax = Int32.Parse(_GV.m_Cur_Model.DogRunST) + Int32.Parse(_GV.m_Cur_Model.DogRunLT);
            DogRunLT_MAX.Text = nMax.ToString();

            ToeFL_ST.Text = _GV.m_Cur_Model.ToeFL_ST;
            nMin = Int32.Parse(_GV.m_Cur_Model.ToeFL_ST) - Int32.Parse(_GV.m_Cur_Model.ToeFL_LT);
            ToeFL_LT_MIN.Text = nMin.ToString();

            nMax = Int32.Parse(_GV.m_Cur_Model.ToeFL_ST) + Int32.Parse(_GV.m_Cur_Model.ToeFL_LT);
            ToeFL_LT_MAX.Text = nMax.ToString();

            ToeFR_ST.Text = _GV.m_Cur_Model.ToeFR_ST;
            nMin = Int32.Parse(_GV.m_Cur_Model.ToeFR_ST) - Int32.Parse(_GV.m_Cur_Model.ToeFR_LT);
            ToeFR_LT_MIN.Text = nMin.ToString();

            nMax = Int32.Parse(_GV.m_Cur_Model.ToeFR_ST) + Int32.Parse(_GV.m_Cur_Model.ToeFR_LT);
            ToeFR_LT_MAX.Text = nMax.ToString();



        }
        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = seqList.SelectedItems[0];
                    string acceptNo = selectedItem.Text; // 첫 번째 컬럼 (AcceptNo)
                    
                    // 선택된 AcceptNo로 CarInfo 조회
                    if (m_frmParent != null && _GV._dbJob != null)
                    {
                        _GV.m_Cur_Info = _GV._dbJob.SelectCarInfo(acceptNo);
                        _GV.m_Cur_Model = _GV._dbJob.SelectCarModel(_GV.m_Cur_Info.CarModel);
                        RefreshCurrentInfoSafe();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"차량 정보 선택 중 오류: {ex.Message}");
            }
        }

        public void _Log_ListBox(string strText)
        {
            try
            {
                if (lbl_Message.InvokeRequired)
                {
                    _d_SetStatusListMsg d = new _d_SetStatusListMsg(_Log_ListBox);
                    this.Invoke(d, new object[] { strText });
                }
                else
                {
                    lbl_Message.Items.Insert(0, "[" + System.DateTime.Now.ToString("HH:mm:ss") + "] " + strText);

                    //_GV.Log_PGM.WriteFile(strText);
                    

                    if (lbl_Message.Items.Count >= 1000)
                        lbl_Message.Items.RemoveAt(lbl_Message.Items.Count - 1);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine("[_Log_ListBox ] <" + strText + ">  ERROR : " + ex.Message);
                //_GV.Log_PGM.WriteFile("[_Log_ListBox ] <" + strText + ">  ERROR : " + ex.Message);
            }
        }

		// TestThread_Kint 상태 업데이트 수신 핸들러 (UI 스레드 보장)
		private void OnStatusUpdate_Main(string status)
		{
			try
			{
				if (InvokeRequired)
				{
					Invoke(new Action<string>(OnStatusUpdate_Main), status);
					return;
				}
                lbl_Message.Text = status;
				_Log_ListBox(status);
			}
			catch (Exception ex)
			{
				MessageBox.Show($"Status 업데이트 처리 중 오류: {ex.Message}");
			}
		}

        public void StartCycle()
        {

            try
            {
                // UI 스레드에서 실행
                if (InvokeRequired)
                {
                    Invoke(new Action(StartCycle));
                    return;
                }

                if (_GV.m_bTestRun) return;

                if (_GV.m_Cur_Info.CarPJINo == "" || _GV.m_Cur_Info.CarPJINo == null)
                {
                    MessageBox.Show("No data");
                    return;
                }
                if (_GV.m_Cur_Model.Bar_Code == null)
                {
                    MessageBox.Show("No Model");
                    return;
                }
                m_frmParent.User_Monitor.m_frm_Oper_Test.SetModel(_GV.m_Cur_Model);
                m_frmParent.m_Frm_PitIn.SetModel(_GV.m_Cur_Model);
                

                _GV._TestThread.StartCycle(_GV.m_Cur_Info, _GV.m_Cur_Model);

                StartCycleUI();

                
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Cycle start error: {ex.Message}");
            }


        }
        public void StopCycle()
        {
            _GV._TestThread.StopTest();
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            if ( Btn_Start.Text == "STOP")
            {
                StopCycle();
                return;
            }
            StartCycle();
        }

        private void StartCMD()
        {
            _GV.plcWrite.SetFloatingPlateLock(false);
            _GV.plcWrite.SetFloatingPlateFree(true);

            _GV.plcWrite.SetRollerBrakeLock(false);
            _GV.plcWrite.SetRollerBrakeFree(true);

            Thread.Sleep(500);
            _GV.plcWrite.WritePLCData();

            _GV.plcWrite.SetCenteringOn(true);
            _GV.plcWrite.SetCenteringHome(false);

            _GV.plcWrite.SetMotorRun(true);
            _GV.plcWrite.SetMotorStop(false);

            
            _GV.plcWrite.WritePLCData();
        }

        private void StopCMD()
        {
            _GV.plcWrite.SetFloatingPlateLock(true);
            _GV.plcWrite.SetFloatingPlateFree(false);

            _GV.plcWrite.SetRollerBrakeLock(true);
            _GV.plcWrite.SetRollerBrakeFree(false);

            _GV.plcWrite.SetMotorRun(false);
            _GV.plcWrite.SetMotorStop(true);

            _GV.plcWrite.WritePLCData();
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_STOP, 1);
            Thread.Sleep(500);
            
            _GV.plcWrite.SetCenteringOn(false);
            _GV.plcWrite.SetCenteringHome(true);
            _GV.plcWrite.WritePLCData();
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            DppManager.SendToDpp(DppManager.MSG_DPP_MEASURING_START, 2);

            return;
            //StartCMD();
            Console.WriteLine("Send Wheelbase 3200");

            StopCMD();
            return;

            DppManager.SendToDpp(DppManager.MSG_DPP_WHEELBASE, 3200);
            Console.WriteLine("Send Wheelbase 3200");
            return;
            ZPrintController _PrintTicket = new ZPrintController();
            AlignmentReportData data = new AlignmentReportData();

            data.FlToe.After = "1.1";
            data.FlToe.Before = "1.2";
            data.FlToe.Evaluation = "OK";
            data.FlToe.ScrewingDone = "Yes";

            data.FrToe.After = "2.1";
            data.FrToe.Before = "2.2";
            data.FrToe.Evaluation = "OK";
            data.FrToe.ScrewingDone = "Yes";

            data.FrontTotalToe.After = "3.1";
            data.FrontTotalToe.Before = "3.2";
            data.FrontTotalToe.Evaluation = "OK";
            data.FrontTotalToe.ScrewingDone = "Yes";

            data.FrontTotalToe.After = "3.1";
            data.FrontTotalToe.Before = "3.2";
            data.FrontTotalToe.Evaluation = "OK";
            data.FrontTotalToe.ScrewingDone = "Yes";

            data.SteeringWheelAlignment.After = "4.1";
            data.SteeringWheelAlignment.Before = "4.2";
            data.SteeringWheelAlignment.Evaluation = "OK";
            data.SteeringWheelAlignment.ScrewingDone = "Yes";

            _PrintTicket.PrintAlignmentData(data);

            return;
            //StationSetting setting =  XMLWriter.MakeXMLStruct();

            string basePath = _GV.Config.Program.RESULT_PATH;
            string strPath = Path.Combine(
                basePath,
                DateTime.Now.Year.ToString("D4"),
                DateTime.Now.Month.ToString("D2"),
                DateTime.Now.Day.ToString("D2")
            );

            if (!Directory.Exists(strPath))
                Directory.CreateDirectory(strPath);

            string filePath = Path.Combine(strPath, _GV.m_Cur_Info.CarPJINo + ".xml");

            //XMLWriter.WriteXML(setting, filePath);
        }
        private void StartCycleUI()
        {
            
            Pic_copy.Visible =  true;
            Btn_Start.BackColor = Color.Red;
            Btn_Start.ForeColor = Color.Yellow;
            Btn_Start.Text = "STOP";
            lbl_NextPJI.Text = "-";
            

            m_frmParent.User_Monitor.SetPJIText(_GV.m_Cur_Info.CarPJINo);
            m_frmParent.ChangeOperMonitor(Def.FOM_IDX_TEST);
        }
        public void FinishCycleUI()
        {
            if (this.InvokeRequired) // 현재 스레드가 UI 스레드가 아니면
            {
                this.Invoke(new MethodInvoker(FinishCycleUI)); // UI 스레드에 위임
                return;
            }
            m_frmParent.ChangeOperMonitor(Def.FOM_IDX_MAIN);
            Pic_copy.Visible = false;
            Btn_Start.BackColor = Color.Silver;
            Btn_Start.ForeColor = Color.Teal;
            Btn_Start.Text = "START";
            
            _GV.m_bTestRun = false;
            
        }

        private void Btn_Manual_Click(object sender, EventArgs e)
        {
            if (Txt_Barcode.TextLength < 10) return;

            GetBarCode(Txt_Barcode.Text);
        }

        private void seqList_DoubleClick(object sender, EventArgs e)
        {
            if (seqList.SelectedItems.Count == 1)
            {
                ListViewItem selectedItem = seqList.SelectedItems[0];
                string acceptNo = selectedItem.Text; // 첫 번째 컬럼 (AcceptNo)
                _GV._dbJob.DeleteCarInfo(acceptNo);
                RefreshCarInfoList();
            }
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            _GV._dbJob.DeleteCarInfo(_GV.m_Cur_Info.AcceptNo);
            RefreshCarInfoList();
        }

        private void seqList_MouseDown(object sender, MouseEventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count > 0)
                {
                    ListViewItem selectedItem = seqList.SelectedItems[0];
                    string acceptNo = selectedItem.Text; // 첫 번째 컬럼 (AcceptNo)

                    // 선택된 AcceptNo로 CarInfo 조회
                    if (m_frmParent != null && _GV._dbJob != null)
                    {
                        _GV.m_Cur_Info = _GV._dbJob.SelectCarInfo(acceptNo);
                        _GV.m_Cur_Model = _GV._dbJob.SelectCarModel(_GV.m_Cur_Info.CarModel);
                        RefreshCurrentInfoSafe();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"차량 정보 선택 중 오류: {ex.Message}");
            }
        }
    }
}

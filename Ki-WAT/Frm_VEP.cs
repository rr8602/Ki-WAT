using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace Ki_WAT
{
    public partial class Frm_VEP : Form
    {
        Frm_Mainfrm m_Parent;

        GlobalVal _GV = GlobalVal.Instance;
        
        
        public Frm_VEP(Frm_Mainfrm pParent)
        {
            InitializeComponent();
            m_Parent = pParent;
            _GV._VEP_Client.DescriptionZoneRead += BenchClient_OnDescriptionZoneRead;
            _GV._VEP_Client.StatusZoneChanged += BenchClient_StatusZoneChanged;
            _GV._VEP_Client.SynchroZoneChanged += BenchClient_SynchroZoneChanged;
            _GV._VEP_Client.TransmissionZoneChanged += BenchClient_TransmissionZoneChanged;
            _GV._VEP_Client.ReceptionZoneChanged += BenchClient_ReceptionZoneChanged;
        }
        private void Frm_VEP_Load(object sender, EventArgs e)
        {

        }

        public void ShowVEP()
        {
            InitList();

            InitData();
            this.Show();
        }
        public void SetParent(Frm_Mainfrm frm)
        {
            m_Parent = frm;
        }

        private void InitData()
        {

            // 싱크로
            for ( int ni = 0; ni < 90; ni++)
            {
                List_Sync.Items[ni].SubItems[3].Text = _GV._VEP_Data.SynchroZone.GetValue(ni).ToString();
            }

            Txt_S00.Text = _GV._VEP_Data.SynchroZone.GetValue(0).ToString();
            Txt_S01.Text = _GV._VEP_Data.SynchroZone.GetValue(1).ToString();
            Txt_S03.Text = _GV._VEP_Data.SynchroZone.GetValue(3).ToString();
            Txt_S04.Text= _GV._VEP_Data.SynchroZone.GetValue(4).ToString(); 
            Txt_S06.Text= _GV._VEP_Data.SynchroZone.GetValue(6).ToString();
            Txt_S07.Text= _GV._VEP_Data.SynchroZone.GetValue(7).ToString();
            Txt_S08.Text= _GV._VEP_Data.SynchroZone.GetValue(08).ToString();
            Txt_S09.Text= _GV._VEP_Data.SynchroZone.GetValue(9).ToString();
            Txt_S10.Text= _GV._VEP_Data.SynchroZone.GetValue(10).ToString(); 
            Txt_S11.Text= _GV._VEP_Data.SynchroZone.GetValue(11).ToString(); 
            Txt_S12.Text= _GV._VEP_Data.SynchroZone.GetValue(12).ToString();
            Txt_S13.Text= _GV._VEP_Data.SynchroZone.GetValue(13).ToString();
            Txt_S20.Text= _GV._VEP_Data.SynchroZone.GetValue(20).ToString();
            Txt_S21.Text= _GV._VEP_Data.SynchroZone.GetValue(21).ToString();
            Txt_S22.Text= _GV._VEP_Data.SynchroZone.GetValue(22).ToString();
            Txt_S30.Text= _GV._VEP_Data.SynchroZone.GetValue(30).ToString();
            Txt_S32.Text= _GV._VEP_Data.SynchroZone.GetValue(32).ToString();


        }

        private void InitList()
        {
            List_Sync.View = View.Details; // REPORT 모드
            List_Sync.FullRowSelect = true;
            List_Sync.GridLines = true;

            List_Sync.Columns.Clear();

            // 컬럼 추가
            List_Sync.Columns.Add("Non", 0);
            List_Sync.Columns.Add("Add", List_Sync.Width / 3-20 , HorizontalAlignment.Center);
            List_Sync.Columns.Add("Sync", List_Sync.Width / 3-20,HorizontalAlignment.Center);
            List_Sync.Columns.Add("Val", List_Sync.Width / 3 +15,HorizontalAlignment.Center);

            int nAdd = _GV._VEP_Client.Addr_SynchroZone;

            for (int ni = 0; ni < 90; ni++)
            {
                ListViewItem item = new ListViewItem("1");
                item.SubItems.Add(nAdd.ToString());
                item.SubItems.Add(ni.ToString());
                item.SubItems.Add("");
                List_Sync.Items.Add(item);
                nAdd++;
                cmbValueList.Items.Add("Sync : " + ni.ToString());

            }
            cmbValueList.SelectedIndex = 0;

        }

        public void UpdateSynchroValues(ushort[] pSync)
        {
            if (!this.Visible) return;

            for ( int ni = 0; ni < pSync.Length; ni++)
            {
                List_Sync.Items[ni].SubItems[3].Text = pSync[ni].ToString();
            }

            Txt_S00.Text = pSync[0].ToString();
            Txt_S01.Text = pSync[1].ToString();
            Txt_S03.Text = pSync[3].ToString();
            Txt_S04.Text = pSync[4].ToString();
            Txt_S06.Text = pSync[6].ToString();
            Txt_S07.Text = pSync[7].ToString();
            Txt_S08.Text = pSync[8].ToString();
            Txt_S09.Text = pSync[9].ToString();
            Txt_S10.Text = pSync[10].ToString();
            Txt_S11.Text = pSync[11].ToString();
            Txt_S12.Text = pSync[12].ToString();
            Txt_S13.Text = pSync[13].ToString();
            Txt_S20.Text = pSync[20].ToString();
            Txt_S21.Text = pSync[21].ToString();
            Txt_S22.Text = pSync[22].ToString();
            Txt_S30.Text = pSync[30].ToString();
            Txt_S32.Text = pSync[32].ToString();

        }

        private void BenchClient_OnDescriptionZoneRead(object sender, VEPBenchDescriptionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action<object, VEPBenchDescriptionZone>(BenchClient_OnDescriptionZoneRead), sender, e);
                return;
            }

            txtDesZone.Text = e.ValidityIndicator.ToString();
            txtStatusZoneAddress.Text = e.StatusZoneAddr.ToString();
            txtStatusZoneSize.Text = e.StatusZoneSize.ToString();
            txtSynchroZoneAddress.Text = e.SynchroZoneAddr.ToString();
            txtSynchroZoneSize.Text = e.SynchroZoneSize.ToString();
            txtTzAddress.Text = e.TransmissionZoneAddr.ToString();
            txtTzSize.Text = e.TransmissionZoneSize.ToString();
            txtReAddress.Text = e.ReceptionZoneAddr.ToString();
            txtReSize.Text = e.ReceptionZoneSize.ToString();
            txtAddTzAddress.Text = e.AdditionalTZAddr.ToString();
            txtAddTzSize.Text = e.AdditionalTZSize.ToString();
            txtAddReAddress.Text = e.AdditionalRZAddr.ToString();
            txtAddReSize.Text = e.AdditionalRZSize.ToString();
        }
        private void BenchClient_StatusZoneChanged(object sender, VEPBenchStatusZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateStatusInfo(
                    e.VepStatus,
                    e.VepCycleEnd,
                    e.BenchCycleEnd,
                    e.StartCycle,
                    e.VepCycleInterruption,
                    e.BenchCycleInterruption)));
            }
            else
            {
                UpdateStatusInfo(
                    e.VepStatus,
                    e.VepCycleEnd,
                    e.BenchCycleEnd,
                    e.StartCycle,
                    e.VepCycleInterruption,
                    e.BenchCycleInterruption);
            }
        }
        public void UpdateStatusInfo(ushort vepStatus, ushort vepCycleEnd, ushort benchCycleEnd, ushort startCycle, ushort vepCycleInt, ushort benchCycleInt)
        {
            txtStVepStatus.Text = vepStatus.ToString();
            txtStVepCycleEnd.Text = vepCycleEnd.ToString();
            txtStBenchCycleEnd.Text = benchCycleEnd.ToString();
            txtStStartCycle.Text = startCycle.ToString();
            txtStVepCycleInt.Text = vepCycleInt.ToString();
            txtStBenchCycleInt.Text = benchCycleInt.ToString();
        }

        private void BenchClient_SynchroZoneChanged(object sender, VEPBenchSynchroZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateSynchroValues(e.ToRegisters())));
                    
            }
            else
            {
                UpdateSynchroValues(e.ToRegisters());
            }
        }

        private void BenchClient_TransmissionZoneChanged(object sender, VEPBenchTransmissionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateTransmissionInfo(e.AddTzSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode)));
            }
            else
            {
                UpdateTransmissionInfo(e.AddTzSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode);
            }

            if (e.IsRequest)
            {
                Console.WriteLine($"TransmissionZoneChanged 이벤트: 요청 감지 FctCode={e.FctCode}, PCNum={e.PCNum}");
            }
        }

        public void UpdateTransmissionInfo(ushort size, ushort exchStatus, byte fctCode, byte pcNum, byte processCode, byte subFctCode)
        {
            txtAddrTzSize.Text = size.ToString();
            txtTzExchStatus.Text = exchStatus.ToString();
            txtTzFctCode.Text = fctCode.ToString();
            txtTzPCNum.Text = pcNum.ToString();
            txtTzProcessCode.Text = processCode.ToString();
            txtTzSubFctCode.Text = subFctCode.ToString();
        }

        public void UpdateReceptionInfo(ushort size, ushort exchStatus, byte fctCode, byte pcNum, byte processCode, byte subFctCode)
        {
            txtAddrReSize.Text = size.ToString();
            txtReExchStatus.Text = exchStatus.ToString();
            txtReFctCode.Text = fctCode.ToString();
            txtRePCNum.Text = pcNum.ToString();
            txtReProcessCode.Text = processCode.ToString();
            txtReSubFctCode.Text = subFctCode.ToString();
        }

        private void BenchClient_ReceptionZoneChanged(object sender, VEPBenchReceptionZone e)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new Action(() => UpdateReceptionInfo(e.AddReSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode)));
            }
            else
            {
                UpdateReceptionInfo(e.AddReSize, e.ExchStatus, e.FctCode, e.PCNum, e.ProcessCode, e.SubFctCode);
            }

            string status = e.IsResponseCompleted ? "응답 완료" : "응답 준비";
            Console.WriteLine($"ReceptionZoneChanged 이벤트: {status}, FctCode={e.FctCode}");
        }

        

        private void btnEditValue_Click(object sender, EventArgs e)
        {
            try
            {
                var selectedItem = cmbValueList.SelectedItem?.ToString();
                int nIndex = cmbValueList.SelectedIndex;
                if (string.IsNullOrEmpty(selectedItem))
                {
                    return;
                }

                if (!int.TryParse(txtEditValue.Text, out int valueToSet))
                {
                    MessageBox.Show("Enter a valid integer.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                //_GV._VEP_Data.SynchroZone.SetValue(nIndex, (ushort)valueToSet);
                //_GV._VEP_Client.WriteSynchroZone();
                _GV._VEP_Client.WriteSyncroZone((ushort)nIndex, (ushort)valueToSet);

            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error modifying value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _GV._VEP_Client.SetTzEtat(100);

            if (_GV._VEP_Data.StatusZone.StartCycle == 0 )
            {

            }

        }

    

        private void label47_Click(object sender, EventArgs e)
        {

        }
    }
}

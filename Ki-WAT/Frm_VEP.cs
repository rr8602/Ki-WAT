using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net;
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

        }
        private void Frm_VEP_Load(object sender, EventArgs e)
        {
			List_Sync.View = View.Details; // REPORT 모드
			List_Sync.FullRowSelect = true;
			List_Sync.GridLines = true;

			List_Sync.Columns.Clear();

			// 컬럼 추가
			List_Sync.Columns.Add("Non", 0);
			List_Sync.Columns.Add("Address", List_Sync.Width / 3 - 20, HorizontalAlignment.Center);
			List_Sync.Columns.Add("Sync No", List_Sync.Width / 3 - 20, HorizontalAlignment.Center);
			List_Sync.Columns.Add("Value", List_Sync.Width / 3 + 15, HorizontalAlignment.Center);


			_GV.vep.broker.Subscribe("VEP", UpdateUI);
		}



		private void UpdateUI(object data)
		{
			if (this.InvokeRequired)
			{
				this.BeginInvoke(new Action<object>(UpdateUI), data);
				return;
			}

			string strZone = data as string;

			if (!string.IsNullOrEmpty(strZone))
			{
				switch (strZone)
				{
					case "DZData":
						UpdateDZ();
						break;

					case "SZData":
						UpdateSZ();
						break;

					case "SYData":
						UpdateSY();
						break;
					case "TZData":
						UpdateTZ();
						break;
					case "RZData":
						UpdateRZ();
						break;
					case "AddTZData":
						UpdateAddTZ();
						break;


					default:
						break;
				}
			}
		}

		private void UpdateDZ()
		{
			txtDesZone.Text = _GV.vep.vepData.Validate.ToString();
			txtStatusZoneAddress.Text = _GV.vep.vepData.SZAddr.ToString();
			txtStatusZoneSize.Text = _GV.vep.vepData.SZSize.ToString();
			txtSynchroZoneAddress.Text = _GV.vep.vepData.SYAddr.ToString();
			txtSynchroZoneSize.Text = _GV.vep.vepData.SYSize.ToString();
			txtTzAddress.Text = _GV.vep.vepData.TZAddr.ToString();
			txtTzSize.Text = _GV.vep.vepData.TZSize.ToString();
			txtReAddress.Text = _GV.vep.vepData.RZAddr.ToString();
			txtReSize.Text = _GV.vep.vepData.RZSize.ToString();
			txtAddTzAddress.Text = _GV.vep.vepData.AddTZAddr.ToString();
			txtAddTzSize.Text = _GV.vep.vepData.AddTZSize.ToString();
			txtAddReAddress.Text = _GV.vep.vepData.AddRZAddr.ToString();
			txtAddReSize.Text = _GV.vep.vepData.AddRZSize.ToString();
		}
		private void UpdateSZ()
		{

			txtStVepStatus.Text = _GV.vep.vepData.VEPStatus.ToString();
			txtStVepCycleInt.Text = _GV.vep.vepData.VEPCycleInterupt.ToString();
			txtStVepCycleEnd.Text = _GV.vep.vepData.VEPCycleEnd.ToString();
			txtStBenchCycleInt.Text = _GV.vep.vepData.BenchCycleInterupt.ToString();
			txtStBenchCycleEnd.Text = _GV.vep.vepData.BenchCycleEnd.ToString();
			txtStStartCycle.Text = _GV.vep.vepData.StartCycle.ToString();


		}
		private void UpdateSY()
		{
			if (List_Sync.Items.Count != _GV.vep.vepData.SYData.Length)
			{
				List_Sync.Items.Clear();

				int nAdd = _GV.vep.vepData.SYAddr;

				for (int ni = 0; ni < _GV.vep.vepData.SYData.Length; ni++)
				{
					ListViewItem item = new ListViewItem("1");
					item.SubItems.Add(nAdd.ToString());
					item.SubItems.Add(ni.ToString());
					item.SubItems.Add("");
					List_Sync.Items.Add(item);
					nAdd++;

				}

			}

			if (List_Sync.Items.Count > 0)
			{
				for (int ni = 0; ni < _GV.vep.vepData.SYData.Length; ni++)
				{
					//List_Sync.Items[ni].SubItems[3].Text = vep.vepData.SYData[ni].ToString();
					string newText = _GV.vep.vepData.SYData[ni].ToString();
					if (List_Sync.Items[ni].SubItems[3].Text != newText)
						List_Sync.Items[ni].SubItems[3].Text = newText;
				}
			}

			Txt_S00.Text = _GV.vep.vepData.SYData[0].ToString();
			Txt_S01.Text = _GV.vep.vepData.SYData[1].ToString();
			Txt_S03.Text = _GV.vep.vepData.SYData[3].ToString();
			Txt_S04.Text = _GV.vep.vepData.SYData[4].ToString();
			Txt_S06.Text = _GV.vep.vepData.SYData[6].ToString();
			Txt_S07.Text = _GV.vep.vepData.SYData[7].ToString();
			Txt_S08.Text = _GV.vep.vepData.SYData[8].ToString();
			Txt_S09.Text = _GV.vep.vepData.SYData[9].ToString();
			Txt_S10.Text = _GV.vep.vepData.SYData[10].ToString();
			Txt_S11.Text = _GV.vep.vepData.SYData[11].ToString();
			Txt_S12.Text = _GV.vep.vepData.SYData[12].ToString();
			Txt_S13.Text = _GV.vep.vepData.SYData[13].ToString();
			Txt_S20.Text = _GV.vep.vepData.SYData[20].ToString();
			Txt_S21.Text = _GV.vep.vepData.SYData[21].ToString();
			Txt_S22.Text = _GV.vep.vepData.SYData[22].ToString();
			Txt_S30.Text = _GV.vep.vepData.SYData[30].ToString();
			Txt_S32.Text = _GV.vep.vepData.SYData[32].ToString();


		}

		private void UpdateTZ()
		{

			txtAddrTzSize.Text = _GV.vep.vepData.TZ_AddTZSize.ToString();
			txtTzExchStatus.Text = _GV.vep.vepData.TZ_ExchangeStatus.ToString();
			txtTzFctCode.Text = _GV.vep.vepData.TZ_FctCode.ToString();
			txtTzPCNum.Text = _GV.vep.vepData.TZ_PCNum.ToString();
			txtTzProcessCode.Text = _GV.vep.vepData.TZ_ProcessCode.ToString();
			txtTzSubFctCode.Text = _GV.vep.vepData.TZ_SubFctCode.ToString();
			//editTZData.Text = _GV.vep.vepData.TZDataString;
			richTextBox1.Text = _GV.vep.vepData.TZDataString;
		}

		private void UpdateRZ()
		{

			txtAddrReSize.Text = _GV.vep.vepData.RZ_AddRZSize.ToString();
			txtReExchStatus.Text = _GV.vep.vepData.RZ_ExchangeStatus.ToString();
			txtReFctCode.Text = _GV.vep.vepData.RZ_FctCode.ToString();
			txtRePCNum.Text = _GV.vep.vepData.RZ_PCNum.ToString();
			txtReProcessCode.Text = _GV.vep.vepData.RZ_ProcessCode.ToString();
			txtReSubFctCode.Text = _GV.vep.vepData.RZ_SubFctCode.ToString();
			//editRZData.Text = _GV.vep.vepData.RZDataString;
			richTextBox2.Text = _GV.vep.vepData.RZDataString;
		}

		private void UpdateAddTZ()
		{

		}

		public void ShowVEP()
        {
			
            //InitList();

            //InitData();
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
                List_Sync.Items[ni].SubItems[3].Text = _GV.vep.vepData.SYData[ni].ToString();
            }

            Txt_S00.Text = _GV.vep.vepData.SYData[0].ToString();
            Txt_S01.Text = _GV.vep.vepData.SYData[1].ToString();
            Txt_S03.Text = _GV.vep.vepData.SYData[3].ToString();
            Txt_S04.Text= _GV.vep.vepData.SYData[4].ToString(); 
            Txt_S06.Text= _GV.vep.vepData.SYData[6].ToString();
            Txt_S07.Text= _GV.vep.vepData.SYData[7].ToString();
            Txt_S08.Text= _GV.vep.vepData.SYData[8].ToString();
            Txt_S09.Text= _GV.vep.vepData.SYData[9].ToString();
            Txt_S10.Text= _GV.vep.vepData.SYData[10].ToString(); 
            Txt_S11.Text= _GV.vep.vepData.SYData[11].ToString(); 
            Txt_S12.Text= _GV.vep.vepData.SYData[12].ToString();
            Txt_S13.Text= _GV.vep.vepData.SYData[13].ToString();
            Txt_S20.Text= _GV.vep.vepData.SYData[20].ToString();
            Txt_S21.Text= _GV.vep.vepData.SYData[21].ToString();
            Txt_S22.Text= _GV.vep.vepData.SYData[22].ToString();
            Txt_S30.Text= _GV.vep.vepData.SYData[30].ToString();
            Txt_S32.Text= _GV.vep.vepData.SYData[32].ToString();


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
                //_GV._VEP_Client.WriteSyncroZone((ushort)nIndex, (ushort)valueToSet);


				_GV.vep.SetSynchro(nIndex, (ushort)valueToSet);
			}
            catch (Exception ex)
            {
                MessageBox.Show($"Error modifying value: {ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
			_GV.vep.SetTZExchange(30);

			if (_GV.vep.vepData.StartCycle == 0 )
            {

            }

        }

    

        private void label47_Click(object sender, EventArgs e)
        {

        }

        private void button2_Click(object sender, EventArgs e)
        {
            
			_GV.vep.SetTZExchange(100);
            
        }

		private void Frm_VEP_FormClosing(object sender, FormClosingEventArgs e)
		{
			_GV.vep.broker.Unsubscribe("VEP", UpdateUI);
		}
	}
}

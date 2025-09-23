using KINT_Lib;
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

namespace Ki_WAT
{
    public partial class Frm_Result : Form 
    {

        Frm_Mainfrm m_frmParent = null;

        public Frm_Result()
        {
            InitializeComponent();
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }


        private void Frm_Result_Load(object sender, EventArgs e)
        {
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "yyyy-MM-dd";
            dateTimePicker1.Value = DateTime.Today;   // 오늘 날짜 선택

            seqList.Columns.Add("", 0);
            seqList.Columns.Add("DATE", seqList.Width / 2, HorizontalAlignment.Center);
            seqList.Columns.Add("PJI", seqList.Width / 2, HorizontalAlignment.Center);
            LoadSeqList(dateTimePicker1.Value.ToString("yyyyMMdd"));
        }
        
        public void UpdateUI(TblResult pTable)
        {
            try
            {
                // pTable이 null이거나 유효하지 않은 경우 처리
                if (string.IsNullOrEmpty(pTable.PJI_Num))
                {
                    ClearUI();
                    return;
                }

                // 기본 정보
                // AcceptNo, PJI_Num, ModelNM, WTstTime은 별도 컨트롤이 필요할 수 있음
                // 만약 모델명을 표시할 Label이 있다면: lbl_ModelNM.Text = pTable.ModelNM ?? "";
                
                // Toe 측정값
                lbl_CarFLToe.Text = pTable.CarFLToe ?? "0.0";
                lbl_CarFRToe.Text = pTable.CarFRToe ?? "0.0";
                lbl_CarFTToe.Text = pTable.CarFTToe ?? "0.0";
                lbl_CarRLToe.Text = pTable.CarRLToe ?? "0.0";
                lbl_CarRRToe.Text = pTable.CarRRToe ?? "0.0";
                lbl_CarRTToe.Text = pTable.CarRTToe ?? "0.0";

                // Camber 측정값
                lbl_CarFLCam.Text = pTable.CarFLCam ?? "0.0";
                lbl_CarFRCam.Text = pTable.CarFRCam ?? "0.0";
                lbl_CarFCros.Text = pTable.CarFCros ?? "0.0";
                lbl_CarRLCam.Text = pTable.CarRLCam ?? "0.0";
                lbl_CarRRCam.Text = pTable.CarRRCam ?? "0.0";
                lbl_CarRCros.Text = pTable.CarRCros ?? "0.0";

                // 기타 정보
                lbl_Car_Hand.Text = pTable.Car_Hand ?? "0.0";
                lbl_CarDogRun.Text = pTable.CarDogRu ?? "0.0";
                
                // 추가 필드들 (UI 컨트롤이 있다면)
                // Car_Symm, WAT___PK 등은 별도 컨트롤이 필요할 수 있음
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UI 업데이트 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUI()
        {
            try
            {
                // 모든 Label 컨트롤 초기화
                lbl_CarFLToe.Text = "0.0";
                lbl_CarFRToe.Text = "0.0";
                lbl_CarFTToe.Text = "0.0";
                lbl_CarRLToe.Text = "0.0";
                lbl_CarRRToe.Text = "0.0";
                lbl_CarRTToe.Text = "0.0";
                
                lbl_CarFLCam.Text = "0.0";
                lbl_CarFRCam.Text = "0.0";
                lbl_CarFCros.Text = "0.0";
                lbl_CarRLCam.Text = "0.0";
                lbl_CarRRCam.Text = "0.0";
                lbl_CarRCros.Text = "0.0";
                
                lbl_Car_Hand.Text = "0.0";
                lbl_CarDogRun.Text = "0.0";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UI 초기화 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_PJI_Search_Click(object sender, EventArgs e)
        { 
            try
            {
                // PJI 번호가 입력되었는지 확인
                if (string.IsNullOrEmpty(Txt_PJI.Text.Trim()))
                {
                    MessageBox.Show("PJI 번호를 입력해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 데이터베이스에서 결과 조회
                TblResult tResult = m_frmParent.m_dbJob.SelectCarResult(Txt_PJI.Text.Trim());
                
                // 결과가 있는지 확인
                if (string.IsNullOrEmpty(tResult.PJI_Num))
                {
                    MessageBox.Show($"PJI 번호 '{Txt_PJI.Text.Trim()}'에 해당하는 결과를 찾을 수 없습니다.", "검색 결과 없음", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    ClearUI();
                    return;
                }

                // UI 업데이트
                UpdateUI(tResult);
                
                MessageBox.Show("검색이 완료되었습니다.", "검색 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                return;
            }
        }
        private void cButton1_Click(object sender, EventArgs e)
        {
        }
        private void seqList_SelectedIndexChanged(object sender, EventArgs e)
        {
            try
            {
                if (seqList.SelectedItems.Count > 0)
                {
                    ListViewItem item = seqList.SelectedItems[0];

                    // SubItems[0] → 첫 번째 컬럼
                    // SubItems[1] → 두 번째 컬럼
                    string pji = item.SubItems[2].Text;
                    TblResult _res = m_frmParent.m_dbJob.SelectCarResult(pji);
                    UpdateUI(_res);
                    ///Debug.Print("");
                }
                    
            }
            catch (Exception ex)
            {
                //MessageBox.Show($"날짜별 검색 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        private void LoadSeqList(string pDate)
        {
            // DB 조회
            List<TblCarInfo> _resList = m_frmParent.m_dbJob.SelectCarInfoList(pDate);

            seqList.BeginUpdate();
            try
            {
                seqList.Items.Clear();

                if (_resList == null || _resList.Count == 0)
                    return;

                int idx = 1;
                foreach (var car in _resList)
                {
                    // 0번째(숨김) 컬럼에는 인덱스나 키를 넣어두면 편리
                    var lvi = new ListViewItem(idx.ToString());

                    // 1번째: UID -> AcceptNo
                    lvi.SubItems.Add(car.AcceptNo ?? string.Empty);

                    // 2번째: PJI -> CarPJINo
                    lvi.SubItems.Add(car.CarPJINo ?? string.Empty);

                    seqList.Items.Add(lvi);
                    idx++;
                }
            }
            finally
            {
                seqList.EndUpdate();
            }
        }

        private void Btn_Date_Search_Click(object sender, EventArgs e)
        {
            string pDate = dateTimePicker1.Value.ToString("yyyyMMdd");
            LoadSeqList(pDate);
            
        }
    }
}
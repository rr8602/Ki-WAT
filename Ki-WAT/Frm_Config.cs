using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Ki_WAT
{
    public partial class Frm_Config : Form
    {
        Frm_Mainfrm m_frmParent = null;
        TblCarModel selectModel;
        GlobalVal _GV = GlobalVal.Instance;
        // 중복 실행 방지를 위한 변수들

        public Frm_Config()
        {
            InitializeComponent();
            
        }
        
      
        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
        }

        private void Frm_Config_Load(object sender, EventArgs e)
        {
            modelList.Columns.Add("", 0);
            modelList.Columns.Add("MODEL", modelList.Width , HorizontalAlignment.Center);
            InitializeCustomComponents();

            RefreshListView();
            UpdateGeneralUI();
            
        }

        private void InitializeCustomComponents()
        {
            cbo_SWB.Items.Clear();
            string[] portsArray = SerialPort.GetPortNames();
            cbo_SWB.Items.AddRange(portsArray);

            Debug.Print("");
            // 저장된 포트 가져오기
         

        }


        #region Model Tab
        private void Btn_Refresh_Click(object sender, EventArgs e)
        {
            RefreshListView();
        }

        private void RefreshListView()
        {
            try
            {
                // ListView의 모든 항목 지우기
                modelList.Items.Clear();
                
                // 데이터베이스에서 모델 목록 가져오기
                List<TblCarModel> dtList = m_frmParent.m_dbJob.SelectAllCarModels();

                // ListView에 데이터 추가
                foreach (TblCarModel model in dtList)
                {
                    int index = modelList.Items.Count + 1; // 다음 항목 번호 (1부터 시작)

                    ListViewItem item = new ListViewItem(index.ToString());
                    item.SubItems.Add(model.Model_NM);
                    modelList.Items.Add(item);
                }

            }
            catch (Exception ex)
            {
                MessageBox.Show($"ListView 새로고침 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void UpdateUI()
        {
            try
            {
                // selectModel이 null이거나 유효하지 않은 경우 처리
                if (string.IsNullOrEmpty(selectModel.Model_NM))
                {
                    ClearUI();
                    return;
                }

                // 기본 정보
                Txt_Model.Text = selectModel.Model_NM ?? "";
                Txt_Bar_Code.Text = selectModel.Bar_Code ?? "";
                Txt_WhelBase.Text = selectModel.WhelBase ?? "";
                Txt_Dpp_Code.Text = selectModel.Dpp_Code ?? "";
                Txt_SWARatio.Text = selectModel.SWARatio ?? "";
                
                // ScrewDriver와 Display_Unit은 ComboBox로 처리
                if (!string.IsNullOrEmpty(selectModel.ScrewDriver))
                {
                    int screwIndex;
                    if (int.TryParse(selectModel.ScrewDriver, out screwIndex) && screwIndex >= 0)
                    {
                        if (Cbo_Screw.Items.Count > screwIndex)
                            Cbo_Screw.SelectedIndex = screwIndex;
                        else
                            Cbo_Screw.SelectedIndex = 0;
                    }
                    else
                        Cbo_Screw.SelectedIndex = 0;
                }
                else
                    Cbo_Screw.SelectedIndex = 0;

                if (!string.IsNullOrEmpty(selectModel.Display_Unit))
                {
                    int unitIndex;
                    if (int.TryParse(selectModel.Display_Unit, out unitIndex) && unitIndex >= 0)
                    {
                        if (Cbo_Unit.Items.Count > unitIndex)
                            Cbo_Unit.SelectedIndex = unitIndex;
                        else
                            Cbo_Unit.SelectedIndex = 0;
                    }
                    else
                        Cbo_Unit.SelectedIndex = 0;
                }
                else
                    Cbo_Unit.SelectedIndex = 0;

                // DogRun
                Txt_DogRunST.Text = selectModel.DogRunST ?? "";
                Txt_DogRunLT.Text = selectModel.DogRunLT ?? "";

                // Handle
                HandleST.Text = selectModel.HandleST ?? "";
                HandleLT.Text = selectModel.HandleLT ?? "";

                // TotToe
                TotToeF_ST.Text = selectModel.TotToef_ST ?? "";
                TotToeF_LT.Text = selectModel.TotToef_LT ?? "";
                TotToeR_ST.Text = selectModel.TotToer_ST ?? "";
                TotToeR_LT.Text = selectModel.TotToer_LT ?? "";

                // Toe FL (Front Left)
                Txt_ToeFL_ST.Text = selectModel.ToeFL_ST ?? "";
                Txt_ToeFL_AT.Text = selectModel.ToeFL_AT ?? "";
                Txt_ToeFL_LT.Text = selectModel.ToeFL_LT ?? "";

                // Toe FR (Front Right)
                Txt_ToeFR_ST.Text = selectModel.ToeFR_ST ?? "";
                Txt_ToeFR_AT.Text = selectModel.ToeFR_AT ?? "";
                Txt_ToeFR_LT.Text = selectModel.ToeFR_LT ?? "";

                // Toe RL (Rear Left)
                Txt_ToeRL_ST.Text = selectModel.ToeRL_ST ?? "";
                Txt_ToeRL_AT.Text = selectModel.ToeRL_AT ?? "";
                Txt_ToeRL_LT.Text = selectModel.ToeRL_LT ?? "";

                // Toe RR (Rear Right)
                Txt_ToeRR_ST.Text = selectModel.ToeRR_ST ?? "";
                Txt_ToeRR_AT.Text = selectModel.ToeRR_AT ?? "";
                Txt_ToeRR_LT.Text = selectModel.ToeRR_LT ?? "";

                // Cam FL (Front Left)
                Txt_CamFL_ST.Text = selectModel.CamFL_ST ?? "";
                Txt_CamFL_AT.Text = selectModel.CamFL_AT ?? "";
                Txt_CamFL_LT.Text = selectModel.CamFL_LT ?? "";

                // Cam FR (Front Right)
                Txt_CamFR_ST.Text = selectModel.CamFR_ST ?? "";
                Txt_CamFR_AT.Text = selectModel.CamFR_AT ?? "";
                Txt_CamFR_LT.Text = selectModel.CamFR_LT ?? "";

                // Cam RL (Rear Left)
                Txt_CamRL_ST.Text = selectModel.CamRL_ST ?? "";
                Txt_CamRL_AT.Text = selectModel.CamRL_AT ?? "";
                Txt_CamRL_LT.Text = selectModel.CamRL_LT ?? "";

                // Cam RR (Rear Right)
                Txt_CamRR_ST.Text = selectModel.CamRR_ST ?? "";
                Txt_CamRR_AT.Text = selectModel.CamRR_AT ?? "";
                Txt_CamRR_LT.Text = selectModel.CamRR_LT ?? "";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"UI 업데이트 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void modelList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (modelList.SelectedItems.Count > 0)
            {
                string modelValue = modelList.SelectedItems[0].SubItems[1].Text;
                selectModel = m_frmParent.m_dbJob.SelectCarModel(modelValue);
                UpdateUI(); // UI 업데이트 호출
                Debug.Print("");
            }
        }

        private void Btn_Add_Click(object sender, EventArgs e)
        {
            try
            {
                // UI에서 값을 가져와서 새로운 TblCarModel 객체 생성
                TblCarModel newModel = new TblCarModel
                {
                    // 기본 정보
                    Model_NM = Txt_Model.Text,
                    Bar_Code = Txt_Bar_Code.Text,
                    WhelBase = Txt_WhelBase.Text,
                    Dpp_Code = Txt_Dpp_Code.Text,
                    SWARatio = Txt_SWARatio.Text,
                    
                    // ScrewDriver와 Display_Unit (ComboBox에서 index 값 가져오기)
                    ScrewDriver = Cbo_Screw.SelectedIndex.ToString(),
                    Display_Unit = Cbo_Unit.SelectedIndex.ToString(),
                    
                    // Spare 필드들 (필요시 추가)
                    Spare_1 = "",
                    Spare_2 = "",

                    // DogRun
                    DogRunST = Txt_DogRunST.Text,
                    DogRunLT = Txt_DogRunLT.Text,

                    // Handle
                    HandleST = HandleST.Text,
                    HandleLT = HandleLT.Text,

                    // TotToe
                    TotToef_ST = TotToeF_ST.Text,
                    TotToef_LT = TotToeF_LT.Text,
                    TotToer_ST = TotToeR_ST.Text,
                    TotToer_LT = TotToeR_LT.Text,

                    // Toe FL (Front Left)
                    ToeFL_ST = Txt_ToeFL_ST.Text,
                    ToeFL_AT = Txt_ToeFL_AT.Text,
                    ToeFL_LT = Txt_ToeFL_LT.Text,

                    // Toe FR (Front Right)
                    ToeFR_ST = Txt_ToeFR_ST.Text,
                    ToeFR_AT = Txt_ToeFR_AT.Text,
                    ToeFR_LT = Txt_ToeFR_LT.Text,

                    // Toe RL (Rear Left)
                    ToeRL_ST = Txt_ToeRL_ST.Text,
                    ToeRL_AT = Txt_ToeRL_AT.Text,
                    ToeRL_LT = Txt_ToeRL_LT.Text,

                    // Toe RR (Rear Right)
                    ToeRR_ST = Txt_ToeRR_ST.Text,
                    ToeRR_AT = Txt_ToeRR_AT.Text,
                    ToeRR_LT = Txt_ToeRR_LT.Text,

                    // Cam FL (Front Left)
                    CamFL_ST = Txt_CamFL_ST.Text,
                    CamFL_AT = Txt_CamFL_AT.Text,
                    CamFL_LT = Txt_CamFL_LT.Text,

                    // Cam FR (Front Right)
                    CamFR_ST = Txt_CamFR_ST.Text,
                    CamFR_AT = Txt_CamFR_AT.Text,
                    CamFR_LT = Txt_CamFR_LT.Text,

                    // Cam RL (Rear Left)
                    CamRL_ST = Txt_CamRL_ST.Text,
                    CamRL_AT = Txt_CamRL_AT.Text,
                    CamRL_LT = Txt_CamRL_LT.Text,

                    // Cam RR (Rear Right)
                    CamRR_ST = Txt_CamRR_ST.Text,
                    CamRR_AT = Txt_CamRR_AT.Text,
                    CamRR_LT = Txt_CamRR_LT.Text
                };

                // 데이터베이스에 추가
                bool success = m_frmParent.m_dbJob.InsertCarModel(newModel);
                
                if (success)
                {
                    // 성공 메시지
                    MessageBox.Show("모델이 성공적으로 추가되었습니다.", "추가 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // ListView 새로고침
                    RefreshListView();
                    
                    // UI 초기화
                    ClearUI();
                }
                else
                {
                    MessageBox.Show("모델 추가에 실패했습니다.", "추가 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 추가 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ClearUI()
        {
            // 모든 텍스트박스 초기화
            Txt_Model.Text = "";
            Txt_Bar_Code.Text = "";
            Txt_WhelBase.Text = "";
            Txt_Dpp_Code.Text = "";
            Txt_SWARatio.Text = "";
            
            // ComboBox 초기화
            Cbo_Screw.SelectedIndex = 0;
            Cbo_Unit.SelectedIndex = 0;
            
            // DogRun
            Txt_DogRunST.Text = "";
            Txt_DogRunLT.Text = "";
            
            // Handle
            HandleST.Text = "";
            HandleLT.Text = "";
            
            // TotToe
            TotToeF_ST.Text = "";
            TotToeF_LT.Text = "";
            TotToeR_ST.Text = "";
            TotToeR_LT.Text = "";
            
            // Toe
            Txt_ToeFL_ST.Text = "";
            Txt_ToeFL_AT.Text = "";
            Txt_ToeFL_LT.Text = "";
            Txt_ToeFR_ST.Text = "";
            Txt_ToeFR_AT.Text = "";
            Txt_ToeFR_LT.Text = "";
            Txt_ToeRL_ST.Text = "";
            Txt_ToeRL_AT.Text = "";
            Txt_ToeRL_LT.Text = "";
            Txt_ToeRR_ST.Text = "";
            Txt_ToeRR_AT.Text = "";
            Txt_ToeRR_LT.Text = "";
            
            // Cam
            Txt_CamFL_ST.Text = "";
            Txt_CamFL_AT.Text = "";
            Txt_CamFL_LT.Text = "";
            Txt_CamFR_ST.Text = "";
            Txt_CamFR_AT.Text = "";
            Txt_CamFR_LT.Text = "";
            Txt_CamRL_ST.Text = "";
            Txt_CamRL_AT.Text = "";
            Txt_CamRL_LT.Text = "";
            Txt_CamRR_ST.Text = "";
            Txt_CamRR_AT.Text = "";
            Txt_CamRR_LT.Text = "";
        }

        private void Btn_Modify_Click(object sender, EventArgs e)
        {
            try
            {
                // 선택된 모델이 있는지 확인
                if ( string.IsNullOrEmpty(selectModel.Model_NM))
                {
                    MessageBox.Show("수정할 모델을 먼저 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // UI에서 값을 가져와서 TblCarModel 객체 업데이트
                selectModel.Model_NM = Txt_Model.Text;
                selectModel.Bar_Code = Txt_Bar_Code.Text;
                selectModel.WhelBase = Txt_WhelBase.Text;
                selectModel.Dpp_Code = Txt_Dpp_Code.Text;
                selectModel.SWARatio = Txt_SWARatio.Text;
                
                // ScrewDriver와 Display_Unit (ComboBox에서 index 값 가져오기)
                selectModel.ScrewDriver = Cbo_Screw.SelectedIndex.ToString();
                selectModel.Display_Unit = Cbo_Unit.SelectedIndex.ToString();
                
                // DogRun
                selectModel.DogRunST = Txt_DogRunST.Text;
                selectModel.DogRunLT = Txt_DogRunLT.Text;

                // Handle
                selectModel.HandleST = HandleST.Text;
                selectModel.HandleLT = HandleLT.Text;

                // TotToe
                selectModel.TotToef_ST = TotToeF_ST.Text;
                selectModel.TotToef_LT = TotToeF_LT.Text;
                selectModel.TotToer_ST = TotToeR_ST.Text;
                selectModel.TotToer_LT = TotToeR_LT.Text;

                // Toe FL (Front Left)
                selectModel.ToeFL_ST = Txt_ToeFL_ST.Text;
                selectModel.ToeFL_AT = Txt_ToeFL_AT.Text;
                selectModel.ToeFL_LT = Txt_ToeFL_LT.Text;

                // Toe FR (Front Right)
                selectModel.ToeFR_ST = Txt_ToeFR_ST.Text;
                selectModel.ToeFR_AT = Txt_ToeFR_AT.Text;
                selectModel.ToeFR_LT = Txt_ToeFR_LT.Text;

                // Toe RL (Rear Left)
                selectModel.ToeRL_ST = Txt_ToeRL_ST.Text;
                selectModel.ToeRL_AT = Txt_ToeRL_AT.Text;
                selectModel.ToeRL_LT = Txt_ToeRL_LT.Text;

                // Toe RR (Rear Right)
                selectModel.ToeRR_ST = Txt_ToeRR_ST.Text;
                selectModel.ToeRR_AT = Txt_ToeRR_AT.Text;
                selectModel.ToeRR_LT = Txt_ToeRR_LT.Text;

                // Cam FL (Front Left)
                selectModel.CamFL_ST = Txt_CamFL_ST.Text;
                selectModel.CamFL_AT = Txt_CamFL_AT.Text;
                selectModel.CamFL_LT = Txt_CamFL_LT.Text;

                // Cam FR (Front Right)
                selectModel.CamFR_ST = Txt_CamFR_ST.Text;
                selectModel.CamFR_AT = Txt_CamFR_AT.Text;
                selectModel.CamFR_LT = Txt_CamFR_LT.Text;

                // Cam RL (Rear Left)
                selectModel.CamRL_ST = Txt_CamRL_ST.Text;
                selectModel.CamRL_AT = Txt_CamRL_AT.Text;
                selectModel.CamRL_LT = Txt_CamRL_LT.Text;

                // Cam RR (Rear Right)
                selectModel.CamRR_ST = Txt_CamRR_ST.Text;
                selectModel.CamRR_AT = Txt_CamRR_AT.Text;
                selectModel.CamRR_LT = Txt_CamRR_LT.Text;

                // 데이터베이스에 업데이트
                bool success = m_frmParent.m_dbJob.UpdateCarModel(selectModel);
                
                if (success)
                {
                    // 성공 메시지
                    MessageBox.Show("모델이 성공적으로 수정되었습니다.", "수정 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    
                    // ListView 새로고침
                    RefreshListView();
                }
                else
                {
                    MessageBox.Show("모델 수정에 실패했습니다.", "수정 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 수정 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Btn_Delete_Click(object sender, EventArgs e)
        {
            try
            {
                // 선택된 모델이 있는지 확인
                if (string.IsNullOrEmpty(selectModel.Model_NM))
                {
                    MessageBox.Show("삭제할 모델을 먼저 선택해주세요.", "알림", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }

                // 삭제 확인 메시지
                DialogResult result = MessageBox.Show(
                    $"모델 '{selectModel.Model_NM}'을(를) 삭제하시겠습니까?\n이 작업은 되돌릴 수 없습니다.", 
                    "삭제 확인", 
                    MessageBoxButtons.YesNo, 
                    MessageBoxIcon.Question);

                if (result == DialogResult.Yes)
                {
                    // 데이터베이스에서 삭제
                    bool success = m_frmParent.m_dbJob.DeleteCarModel(selectModel.Model_NM);
                    
                    if (success)
                    {
                        // 성공 메시지
                        MessageBox.Show("모델이 성공적으로 삭제되었습니다.", "삭제 완료", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        
                        // ListView 새로고침
                        RefreshListView();
                        
                        // UI 초기화
                        ClearUI();
                        
                        // selectModel 초기화
                        selectModel = new TblCarModel();
                    }
                    else
                    {
                        MessageBox.Show("모델 삭제에 실패했습니다.", "삭제 실패", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"모델 삭제 중 오류가 발생했습니다: {ex.Message}", "오류", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        #endregion 

        #region General Tab
        private void BTN_SAVE_Click(object sender, EventArgs e)
        {
            try
            {
                // UI에서 설정 값을 가져와서 Config 객체에 저장
                
                // DEVICE 섹션 - PLC 설정
                _GV.Config.Device.PLC_IP = editPLCIP.Text;
                _GV.Config.Device.PLC_PORT = editPLCPort.Text;
                
                // DEVICE 섹션 - VEP 설정
                _GV.Config.Device.VEP_IP = editVEPIP.Text;
                _GV.Config.Device.VEP_PORT = editVEPPort.Text;
                
                // DEVICE 섹션 - SCREW 설정
                _GV.Config.Device.SCREW_IP = editScrewIP.Text;
                _GV.Config.Device.SCREW_PORT = editScrewPort.Text;

                _GV.Config.Device.SCREW_IP2 = editScrewIP2.Text;
                _GV.Config.Device.SCREW_PORT2 = editScrewPort2.Text;

                // DEVICE 섹션 - PRINT 설정
                _GV.Config.Device.PRINT_IP = editPrintIP.Text;
                _GV.Config.Device.PRINT_PORT = editPrintPort.Text;
                
                // DEVICE 섹션 - BARCODE 설정
                _GV.Config.Device.BARCODE_IP = editBarcodeIP.Text;
                _GV.Config.Device.BARCODE_PORT = editBarcodePort.Text;
                
                // DEVICE 섹션 - LET 설정
                _GV.Config.Device.LET_URL = editLetURL.Text;
                _GV.Config.Device.LET_PORT = editLETPort.Text;
                
                // PROGRAM 섹션 - 결과 경로
                _GV.Config.Program.RESULT_PATH = lblResultPath.Text;
                
                // PROGRAM 섹션 - 언어 설정
                _GV.Config.Program.LANGUAGE = comLang.SelectedIndex.ToString();
                
                // WHEELBASE 섹션 - 휠베이스 설정
                _GV.Config.Wheelbase.HOME_POS = editWBHome.Text;
                _GV.Config.Wheelbase.MIN_POS = editWBMin.Text;
                _GV.Config.Wheelbase.MAX_POS = editWBMax.Text;

                _GV.Config.Device.SWB_PORT = cbo_SWB.Text;
                _GV.Config.Device.SWB_BAUD = Txt_SWB_Baud.Text;

                _GV.Config.SWB.BoardType = rd_swb_type_board.Checked ? 0 : 1;

                // INI 파일에 저장
                _GV.Config.SaveConfig();
                _GV.Config.SaveSWB();

                m_frmParent.m_SWBComm.SetFndDisplay(_GV.Config.SWB.BoardType);

                MessageBox.Show("설정이 성공적으로 저장되었습니다.", "저장 완료", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 저장 중 오류가 발생했습니다: {ex.Message}", "저장 실패", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void UpdateGeneralUI()
        {
            try
            {
                // DEVICE 섹션 - PLC 설정
                editPLCIP.Text = _GV.Config.Device.PLC_IP;
                editPLCPort.Text = _GV.Config.Device.PLC_PORT;
                
                // DEVICE 섹션 - VEP 설정
                editVEPIP.Text = _GV.Config.Device.VEP_IP;
                editVEPPort.Text = _GV.Config.Device.VEP_PORT;
                
                // DEVICE 섹션 - SCREW 설정
                editScrewIP.Text = _GV.Config.Device.SCREW_IP;
                editScrewPort.Text = _GV.Config.Device.SCREW_PORT;

                // DEVICE 섹션 - SCREW 설정
                editScrewIP2.Text = _GV.Config.Device.SCREW_IP2;
                editScrewPort2.Text = _GV.Config.Device.SCREW_PORT2;

                // DEVICE 섹션 - PRINT 설정
                editPrintIP.Text = _GV.Config.Device.PRINT_IP;
                editPrintPort.Text = _GV.Config.Device.PRINT_PORT;
                
                // DEVICE 섹션 - BARCODE 설정
                editBarcodeIP.Text = _GV.Config.Device.BARCODE_IP;
                editBarcodePort.Text = _GV.Config.Device.BARCODE_PORT;
                
                // DEVICE 섹션 - LET 설정
                editLetURL.Text = _GV.Config.Device.LET_URL;
                editLETPort.Text = _GV.Config.Device.LET_PORT;
                
                // PROGRAM 섹션 - 결과 경로
                lblResultPath.Text = _GV.Config.Program.RESULT_PATH;
                
                // PROGRAM 섹션 - 언어 설정
                if (int.TryParse(_GV.Config.Program.LANGUAGE, out int langIndex))
                {
                    if (langIndex >= 0 && langIndex < comLang.Items.Count)
                    {
                        comLang.SelectedIndex = langIndex;
                    }
                }
                
                // WHEELBASE 섹션 - 휠베이스 설정
                editWBHome.Text = _GV.Config.Wheelbase.HOME_POS;
                editWBMin.Text = _GV.Config.Wheelbase.MIN_POS;
                editWBMax.Text = _GV.Config.Wheelbase.MAX_POS;

                Txt_SWB_Baud.Text = _GV.Config.Device.SWB_BAUD;

                string savedPort = _GV.Config.Device.SWB_PORT;


                rd_swb_type_board.Checked = _GV.Config.SWB.BoardType == 0;
                rd_swb_type_pc.Checked = _GV.Config.SWB.BoardType == 1;

                // 일치하는 항목이 있으면 선택
                if (!string.IsNullOrEmpty(savedPort))
                {
                    int index = cbo_SWB.Items.IndexOf(savedPort);
                    if (index >= 0)
                    {
                        cbo_SWB.SelectedIndex = index;
                    }
                }


            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 UI 업데이트 중 오류가 발생했습니다: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        private void Btn_ResPath_Click(object sender, EventArgs e)
        {
            try
            {
                using (FolderBrowserDialog folderDialog = new FolderBrowserDialog())
                {
                    // 다이얼로그 설정
                    folderDialog.Description = "결과 파일 저장 경로를 선택하세요";
                    folderDialog.ShowNewFolderButton = true;

                    // 현재 경로가 있으면 초기 경로로 설정
                    if (!string.IsNullOrEmpty(lblResultPath.Text) && Directory.Exists(lblResultPath.Text))
                    {
                        folderDialog.SelectedPath = lblResultPath.Text;
                    }
                    else
                    {
                        // 기본 경로 설정 (C 드라이브 루트)
                        folderDialog.SelectedPath = "C:\\";
                    }

                    // 다이얼로그 표시
                    if (folderDialog.ShowDialog() == DialogResult.OK)
                    {
                        // 선택된 경로를 lblResultPath에 설정
                        lblResultPath.Text = folderDialog.SelectedPath;

                        // Config 객체에도 즉시 반영
                        _GV.Config.Program.RESULT_PATH = folderDialog.SelectedPath;

                        // 선택 완료 메시지 (선택사항)
                        // MessageBox.Show($"경로가 설정되었습니다: {folderDialog.SelectedPath}", "경로 설정", 
                        //     MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"경로 선택 중 오류가 발생했습니다: {ex.Message}", "오류",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        #endregion

 
    }
}
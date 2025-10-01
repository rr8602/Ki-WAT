using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Net.Mime.MediaTypeNames;

namespace Ki_WAT
{
    
    public partial class Frm_Manual : Form
    {

        GlobalVal _GV = GlobalVal.Instance;
        private Frm_Mainfrm m_frmParent;

        LET_Param m_LetParam = new LET_Param();
        public Frm_Manual()
        {
            InitializeComponent();
        }

        private void Frm_Manual_Load(object sender, EventArgs e)
        {
            
        }

        public void SetParent(Frm_Mainfrm f)
        {
            m_frmParent = f;
            m_frmParent.m_SWBComm._updateDisplayValues += UpdateDisplayValues;
        }

        private void UpdateDisplayValues(string fnd, string sensorAd, string boardAngle, string pcAngle)
        {
            if (InvokeRequired)
            {
                Invoke(new Action(() => UpdateDisplayValues(fnd, sensorAd, boardAngle, pcAngle)));
                return;
            }

            lbl_Type.Text = fnd;
            lbl_Sen_AD.Text = sensorAd;
            lbl_Angle.Text = boardAngle;
            lbl_PC_Angle.Text = pcAngle;
        }

        private void Btn_Last_Error_Click(object sender, EventArgs e)
        {
            
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Txt_Log.Text = "";
        }


        private void GetLetParamFromUI()
        {
            m_LetParam.uid = Txt_UID.Text;
            m_LetParam.line = "LineA";
            m_LetParam.vin = Txt_VIN.Text;
            m_LetParam.vsn = Int32.Parse(Txt_VSN.Text);
            m_LetParam.floorpitch = double.Parse(Txt_FLOR.Text);

        }
        private void Btn_Startcycle_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            bool bRes = _GV.LET_Controller.StartCycle(m_LetParam.uid);

            Debug.Print("");
        }

        private void SaveXmlToFile(string sXML)
        {
            // 현재 실행 중인 EXE의 폴더 경로 가져오기
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;

            // 로그용 서브 폴더 경로 지정
            string folderPath = Path.Combine(baseDir, "LOG", "XML");

            // 폴더가 없으면 생성
            if (!Directory.Exists(folderPath))
            {
                Directory.CreateDirectory(folderPath);
            }

            // 저장할 파일 경로 지정
            string filePath = Path.Combine(folderPath, "output.xml");

            try
            {
                File.WriteAllText(filePath, sXML);
                MessageBox.Show("XML 파일이 저장되었습니다:\n" + filePath);
            }
            catch (Exception ex)
            {
                MessageBox.Show("파일 저장 중 오류 발생: " + ex.Message);
            }
        }

        private void RefreshLetXML(string sXML)
        {

            Txt_Log.Text = sXML;

            LampInclination ResData = _GV.LET_Controller.ParseInclinationFromXml(sXML);

            lbl_left_init_X.Text = ResData.Low_InclinationXInit_Left.ToString();
            lbl_left_init_Y.Text = ResData.Low_InclinationYInit_Left.ToString();
            lbl_Right_init_X.Text = ResData.Low_InclinationXInit_Right.ToString();
            lbl_Right_init_Y.Text = ResData.Low_InclinationYInit_Right.ToString();


            lbl_left_Final_X.Text = ResData.Low_InclinationXFinal_Left.ToString();
            lbl_left_Final_Y.Text = ResData.Low_InclinationYFinal_Left.ToString();
            lbl_Right_Final_X.Text = ResData.Low_InclinationXFinal_Right.ToString();
            lbl_Right_Final_Y.Text = ResData.Low_InclinationYFinal_Right.ToString();




            SaveXmlToFile(sXML);
        }
        private void Btn_Last_Error_Click_2(object sender, EventArgs e)
        {
            string sRes = _GV.LET_Controller.GetLastResult();
            

            RefreshLetXML(sRes);
            

        }

        private void Btn_VehicleSel_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            bool bRes = _GV.LET_Controller.VehicleSelection(m_LetParam.vsn, m_LetParam.vin);

            Debug.Print("");

        }

        private void Btn_perfoamTest_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.PerformTests(m_LetParam.line, m_LetParam.floorpitch);
        }

        private void Btn_Endcycle_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            _GV.LET_Controller.EndCycle();
        }

        private void Btn_Minus_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adMinusPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Zero_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adZeroPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Plus_Click(object sender, EventArgs e)
        {
            _GV.Config.SWB.adPlusPoint = double.Parse(lbl_Sen_AD.Text);
        }

        private void Btn_Save_Click(object sender, EventArgs e)
        {
            _GV.Config.SaveSWB();
        }

        private void Btn_XML_Click(object sender, EventArgs e)
        {
            GetLetParamFromUI();
            string sRes = _GV.LET_Controller.GetResult(m_LetParam.uid);
            RefreshLetXML(sRes);
        }
    }
}

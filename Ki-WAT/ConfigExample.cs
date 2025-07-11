using System;
using System.Windows.Forms;

namespace Ki_WAT
{
    /// <summary>
    /// INI 설정 클래스 사용 예제
    /// </summary>
    public class ConfigExample
    {
        /// <summary>
        /// 설정 사용 예제
        /// </summary>
        public static void ShowUsageExample()
        {
            try
            {
                // 전역 설정 인스턴스 사용
                var config = GlobalVal.Instance.Config;
                
                // 설정 값 읽기
                string plcIp = config.Device.PLC_IP;
                string resultPath = config.Program.RESULT_PATH;
                string homePos = config.Wheelbase.HOME_POS;
                
                // 개별 값 읽기
                string language = config.GetValue("PROGRAM", "LANGUAGE", "0");
                
                // 설정 값 변경
                config.Device.PLC_IP = "192.168.1.100";
                config.Program.RESULT_PATH = "D:\\Results";
                config.Wheelbase.HOME_POS = "1500";
                
                // 개별 값 설정
                config.SetValue("PROGRAM", "LANGUAGE", "1");
                
                // 변경된 설정을 파일에 저장
                config.SaveConfig();
                
                MessageBox.Show("설정이 성공적으로 저장되었습니다.", "성공", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 처리 중 오류가 발생했습니다: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 새로운 설정 파일 생성 예제
        /// </summary>
        public static void CreateNewConfigExample()
        {
            try
            {
                // 새로운 설정 파일 생성
                var newConfig = new AppConfig("new_config.ini");
                
                // 사용자 정의 값 설정
                newConfig.Device.PLC_IP = "192.168.1.200";
                newConfig.Device.PLC_PORT = "502";
                newConfig.Program.RESULT_PATH = "C:\\KiWAT\\Results";
                newConfig.Program.LANGUAGE = "1";
                newConfig.Wheelbase.HOME_POS = "1200";
                newConfig.Wheelbase.MIN_POS = "1800";
                newConfig.Wheelbase.MAX_POS = "3200";
                
                // 파일에 저장
                newConfig.SaveConfig();
                
                MessageBox.Show("새로운 설정 파일이 생성되었습니다.", "성공", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"새 설정 파일 생성 중 오류가 발생했습니다: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
        
        /// <summary>
        /// 설정 값 검증 예제
        /// </summary>
        public static void ValidateConfigExample()
        {
            try
            {
                var config = GlobalVal.Instance.Config;
                
                // 필수 설정 값 검증
                if (string.IsNullOrEmpty(config.Device.PLC_IP))
                {
                    MessageBox.Show("PLC IP 주소가 설정되지 않았습니다.", "경고", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                if (string.IsNullOrEmpty(config.Program.RESULT_PATH))
                {
                    MessageBox.Show("결과 저장 경로가 설정되지 않았습니다.", "경고", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                // 숫자 값 검증
                if (!int.TryParse(config.Wheelbase.HOME_POS, out int homePos))
                {
                    MessageBox.Show("HOME_POS 값이 유효하지 않습니다.", "경고", 
                        MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return;
                }
                
                MessageBox.Show("모든 설정 값이 유효합니다.", "검증 완료", 
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"설정 검증 중 오류가 발생했습니다: {ex.Message}", "오류", 
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
} 
using Ki_WAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace RollTester
{


    public class WheelAlignmentValue
    {
        public string Before { get; set; }
        public string After { get; set; }
        public string Evaluation { get; set; }
        public string ScrewingDone { get; set; }
    }

    public class HeadlampValue
    {
        public string Before { get; set; }
        public string After { get; set; }
        public string Evaluation { get; set; }
    }


    public class AlignmentReportData
    {
        // Wheel alignment process
        public WheelAlignmentValue FlToe { get; set; }
        public WheelAlignmentValue FrToe { get; set; }
        public WheelAlignmentValue FrontTotalToe { get; set; }
        public WheelAlignmentValue SteeringWheelAlignment { get; set; }
        public WheelAlignmentValue RlToe { get; set; }
        public WheelAlignmentValue RrToe { get; set; }
        public WheelAlignmentValue RearTotalToe { get; set; }
        public WheelAlignmentValue RearThrustAngle { get; set; }
        public WheelAlignmentValue FlCamber { get; set; }
        public WheelAlignmentValue FrCamber { get; set; }
        public WheelAlignmentValue FrontDissymmetryOfCamber { get; set; }
        public WheelAlignmentValue RlCamber { get; set; }
        public WheelAlignmentValue RrCamber { get; set; }
        public WheelAlignmentValue RearDissymmetryOfCamber { get; set; }

        // Headlamp aimer process
        public HeadlampValue LeftHeadlampVertical { get; set; }
        public HeadlampValue RightHeadlampVertical { get; set; }

        // Global evaluation
        public string GlobalEvaluationWAProcess { get; set; }
        public string GlobalEvaluationHLAProcess { get; set; }
        public string ResultatPEV { get; set; }


        public AlignmentReportData()
        {
            FlToe = new WheelAlignmentValue();
            FrToe = new WheelAlignmentValue();
            FrontTotalToe = new WheelAlignmentValue();
            SteeringWheelAlignment = new WheelAlignmentValue();
            RlToe = new WheelAlignmentValue();
            RrToe = new WheelAlignmentValue();
            RearTotalToe = new WheelAlignmentValue();
            RearThrustAngle = new WheelAlignmentValue();
            FlCamber = new WheelAlignmentValue();
            FrCamber = new WheelAlignmentValue();
            FrontDissymmetryOfCamber = new WheelAlignmentValue();
            RlCamber = new WheelAlignmentValue();
            RrCamber = new WheelAlignmentValue();
            RearDissymmetryOfCamber = new WheelAlignmentValue();

            LeftHeadlampVertical = new HeadlampValue();
            RightHeadlampVertical = new HeadlampValue();
        }
    }

    public class ZPrintController
	{

        GlobalVal _GV = GlobalVal.Instance;
        public void PrintAlignmentData(AlignmentReportData data, string ipAddress = "172.25.213.30", int port = 9100)
        {
            try
            {

                StringBuilder zplBuilder = new StringBuilder();

                zplBuilder.AppendLine("^XA");
                zplBuilder.AppendLine("^CI28");

                // --- Layout Variables ---
                int yPos = 30;
                int nOffset = 30;
                int nOffset2 = 200;
                int col1 = 20 + nOffset;  // Parameter Name
                int col2 = 320 + nOffset2 ; // Before
                int col3 = 460 + nOffset2 ; // After
                int col4 = 600 + nOffset2 ; // Evaluation
                int col5 = 740 + nOffset2 ; // Screwing Done
                int headerFontHeight = 38;
                int headerFontWidth = 38;
                int rowFontHeight = 35;
                int rowFontWidth = 35;
                int rowHeight = 50;
                int sectionSpacing = 20;
                string strText;
                // 날짜/시간
                string strDateTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
                strText = $"Date & Time : {strDateTime} ";
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{strText}^FS");
                yPos += rowHeight;

                strText = $"Bench Number : 1 ";
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{strText}^FS");
                yPos += rowHeight;

                strText = $"Vehicle type : {_GV.m_Cur_Info.CarModel} ";
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{strText}^FS");
                yPos += rowHeight;

                strText = $"PJI  : {_GV.m_Cur_Info.CarPJINo} ";
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{strText}^FS");
                yPos += rowHeight;


                // --- Helper Actions ---
                Action<string, WheelAlignmentValue> addWheelRow = (param, values) => {
                    zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{param}^FS");
                    zplBuilder.AppendLine($"^FO{col2},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.Before}^FS");
                    zplBuilder.AppendLine($"^FO{col3},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.After}^FS");
                    zplBuilder.AppendLine($"^FO{col4},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.Evaluation}^FS");
                    zplBuilder.AppendLine($"^FO{col5},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.ScrewingDone}^FS");
                    yPos += rowHeight;
                };

                Action<string, HeadlampValue> addHeadlampRow = (param, values) => {
                    zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{param}^FS");
                    zplBuilder.AppendLine($"^FO{col2},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.Before}^FS");
                    zplBuilder.AppendLine($"^FO{col3},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.After}^FS");
                    zplBuilder.AppendLine($"^FO{col4},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{values.Evaluation}^FS");
                    yPos += rowHeight;
                };

                Action<string, string> addGlobalRow = (param, value) => {
                    zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{param}^FS");
                    zplBuilder.AppendLine($"^FO{col2},{yPos}^A0N,{rowFontHeight},{rowFontWidth}^FD{value}^FS");
                    yPos += rowHeight;
                };

                // --- Wheel Alignment Section ---
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDWheel alignment process^FS");
                yPos += 35;
                zplBuilder.AppendLine($"^FO{col2},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDBefore^FS");
                zplBuilder.AppendLine($"^FO{col3},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDAfter^FS");
                zplBuilder.AppendLine($"^FO{col4},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDEval^FS");
                zplBuilder.AppendLine($"^FO{col5},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDScrewing^FS");
                yPos += 35;

                addWheelRow("FL toe", data.FlToe);
                addWheelRow("FR toe", data.FrToe);
                addWheelRow("Front total toe", data.FrontTotalToe);
                addWheelRow("Steering wheel alignment", data.SteeringWheelAlignment);
                addWheelRow("RL toe", data.RlToe);
                addWheelRow("RR toe", data.RrToe);
                addWheelRow("Rear total toe", data.RearTotalToe);
                addWheelRow("Rear thrust angle", data.RearThrustAngle);
                addWheelRow("FL camber", data.FlCamber);
                addWheelRow("FR camber", data.FrCamber);
                addWheelRow("Front dissymmetry of camber", data.FrontDissymmetryOfCamber);
                addWheelRow("RL camber", data.RlCamber);
                addWheelRow("RR camber", data.RrCamber);
                addWheelRow("Rear dissymmetry of camber", data.RearDissymmetryOfCamber);

                yPos += sectionSpacing;

                // --- Headlamp Aimer Section ---
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDHeadlamp aimer process^FS");
                yPos += 35;
                zplBuilder.AppendLine($"^FO{col2},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDBefore^FS");
                zplBuilder.AppendLine($"^FO{col3},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDAfter^FS");
                zplBuilder.AppendLine($"^FO{col4},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDEval^FS");
                yPos += 35;

                addHeadlampRow("Left headlamp (vertical)", data.LeftHeadlampVertical);
                addHeadlampRow("Right headlamp (vertical)", data.RightHeadlampVertical);

                yPos += sectionSpacing;

                // --- Global Evaluation Section ---
                zplBuilder.AppendLine($"^FO{col1},{yPos}^A0N,{headerFontHeight},{headerFontWidth}^FDGlobal Evaluation^FS");
                yPos += 35;
                addGlobalRow("Global evaluation WA process", data.GlobalEvaluationWAProcess);
                addGlobalRow("Global evaluation HLA process", data.GlobalEvaluationHLAProcess);
                addGlobalRow("Resultat PEV", data.ResultatPEV);

                zplBuilder.AppendLine("^XZ");

                string zplString = zplBuilder.ToString();
                DoPrint(zplString, ipAddress, port);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error sending to printer: " + ex.Message);     
            }
        }
        private bool DoPrint(string strPrintData, string ipAddress="172.25.213.30", int port=9100 )
		{
			if (String.IsNullOrWhiteSpace(strPrintData)) return false;
			//String strZebraString = GenerateZpl(strPrintData);
			try
			{
				using (TcpClient client = new TcpClient())
				{
					client.Connect(ipAddress, port);

					using (NetworkStream stream = client.GetStream())
					{
						byte[] data = Encoding.ASCII.GetBytes(strPrintData);
						stream.Write(data, 0, data.Length);
						stream.Close();
					}

					client.Close();
				}
				return true;
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error sending to printer: " + ex.Message);
				return false;
			}
		}
		private string GenerateZpl(string printString)
		{
			const int labelHeightDots = 610;
			const int margin = 80;

			var allLines = printString.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries);

			StringBuilder zpl = new StringBuilder();
			zpl.AppendLine("^XA");
			zpl.AppendLine("^CI28");

			const int fontHeight = 40;
			const int fontWidth = 40;
			const int lineHeight = 45;

			int yPosition = margin;

			foreach (var line in allLines)
			{
				if (yPosition + fontHeight > labelHeightDots - margin)
				{
					break;
				}

				// AON : 기본 폰트 출력
				// A@N : 다운로드 폰트 출력 (ex: 한글)
				zpl.AppendLine($"^FO{margin},{yPosition}^A0N,{fontHeight},{fontWidth}^FD{line.Trim()}^FS");
				yPosition += lineHeight;
			}

			zpl.AppendLine("^XZ");

			return zpl.ToString();
		}
	}


}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Ki_WAT
{

    public class XMLWriter
    {
        public static void WriteXML(StationSetting data, string filePath)
        {
            var serializer = new XmlSerializer(typeof(StationSetting));
            var ns = new XmlSerializerNamespaces();
            ns.Add("", ""); // No namespace

            using (var writer = new StreamWriter(filePath))
            {
                serializer.Serialize(writer, data, ns);
            }
        }

        private static bool GetResultOKNG(string stractual, string strstandard, string strtolerance)
        {

            double actual, standards, tolerance;
            actual = Convert.ToDouble(stractual);
            standards = Convert.ToDouble(strstandard);
            tolerance = Convert.ToDouble(strtolerance);

            double min = standards - tolerance;
            double max = standards + tolerance;
            return (actual >= min && actual <= max);

        }

        public static StationSetting MakeXMLStruct(TblCarModel pModel, TblCarInfo pInfo, TblResult pResult, TblCarLamp pLamp)
        {

            
            StationSetting setting = new StationSetting
            {
                DataModel = new DataModel { Version = "1.2.I" },
                FileFormat = new FileFormat { Version = "1.2.I" },
                MainPart_ID = pInfo.AcceptNo,
                TopStartCyclePart = true,
                TopPart = true,
                ParallelismSettingProcessType = new ParallelismSettingProcessType
                {
                    
                    VerdictOK = (pResult.WAT___PK == "OK"),
                    DiversityCode = "DIV1",
                    BenchNumber = "1",
                    CmdSignConvention = "ISO",
                    SwaOffset = Convert.ToDouble(pModel.SWAOffset),
                    GearRatio = Convert.ToDouble(pModel.SWARatio),
                    CycleTime = Convert.ToInt32(pResult.CycleTime),
                    InitialSteeringWheelAngle = Convert.ToInt32(pResult.Car_Hand_F),
                    FinalSteeringWheelAngle = Convert.ToInt32(pResult.Car_Hand),
                    CmdSteeringWheelAngle = Convert.ToDouble(pModel.HandleST),
                    CmdSteeringWheelAngleToleranceMin = Convert.ToDouble(pModel.HandleST) - Convert.ToDouble(pModel.HandleLT),
                    CmdSteeringWheelAngleToleranceMax = Convert.ToDouble(pModel.HandleST) + Convert.ToDouble(pModel.HandleLT),
                    IsSettingsAdjustReference = true,
                    AxleTools = new List<ParallelizationAxleToolType>
            {
                // FRONT
                new ParallelizationAxleToolType
                {
                    Name = "ParallelizationAxleTool_FRONT",
                    AxleType = "FRONT",
                    InitialDissymetry = Convert.ToDouble(pResult.CarFCros_F),
                    FinalDissymetry = Convert.ToDouble(pResult.CarFCros),
                    CmdDissymetry = 0,
                    CmdDissymetryToleranceMin = -999,
                    CmdDissymetryToleranceMax = 999,
                    TotalToeVerdictOK = true,
                    InitialTotalToe = Convert.ToDouble(pResult.CarFTToe_F),
                    FinalTotalToe = Convert.ToDouble(pResult.CarFTToe),
                    CmdTotalToe = Convert.ToDouble(pModel.TotToef_ST),
                    CmdTotalToeToleranceMin = Convert.ToDouble(pModel.TotToef_ST) - Convert.ToDouble(pModel.TotToef_LT),
                    CmdTotalToeToleranceMax = Convert.ToDouble(pModel.TotToef_ST) + Convert.ToDouble(pModel.TotToef_LT),
                    ThrustAngleVerdictOK = GetResultOKNG(pResult.CarDogRu, pModel.DogRunST, pModel.DogRunLT),
                    InitialThrustAngle = Convert.ToDouble(pResult.CarDogRu_F),
                    FinalThrustAngle = Convert.ToDouble(pResult.CarDogRu),
                    CmdThrustAngle = Convert.ToDouble(pModel.DogRunST),
                    CmdThrustAngleToleranceMin = Convert.ToDouble(pModel.DogRunST) - Convert.ToDouble(pModel.DogRunLT),
                    CmdThrustAngleToleranceMax = Convert.ToDouble(pModel.DogRunST) + Convert.ToDouble(pModel.DogRunLT),
                    WheelTools = new List<ParallelizationWheelToolType>
                    {
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_RIGHT",
                            SideType = "RIGHT",
                            CamberVerdictOK = true,
                            InitialCamber = Convert.ToDouble(pResult.CarFRCam_F),
                            FinalCamber = Convert.ToDouble(pResult.CarFRCam),
                            CmdCamber = 0,
                            CmdCamberToleranceMin = -999,
                            CmdCamberToleranceMax = +999,
                            ToeVerdictOK = GetResultOKNG(pResult.CarFRToe, pModel.ToeFR_ST, pModel.ToeFR_LT),
                            InitialToe = Convert.ToDouble(pResult.CarFRToe_F),
                            FinalToe = Convert.ToDouble(pResult.CarFRToe),
                            CmdToe = Convert.ToDouble(pModel.ToeFR_ST),
                            CmdToeToleranceMin = Convert.ToDouble(pModel.ToeFR_ST) - Convert.ToDouble(pModel.ToeFR_LT),
                            CmdToeToleranceMax = Convert.ToDouble(pModel.ToeFR_ST) + Convert.ToDouble(pModel.ToeFR_LT)
                        },
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_LEFT",
                            SideType = "LEFT",
                            CamberVerdictOK = true,
                            InitialCamber = Convert.ToDouble(pResult.CarFLCam_F),
                            FinalCamber = Convert.ToDouble(pResult.CarFLCam),
                            CmdCamber = 0,
                            CmdCamberToleranceMin = -999,
                            CmdCamberToleranceMax = +999,
                            ToeVerdictOK = GetResultOKNG(pResult.CarFLToe, pModel.ToeFL_ST, pModel.ToeFL_LT),
                            InitialToe = Convert.ToDouble(pResult.CarFLToe_F),
                            FinalToe = Convert.ToDouble(pResult.CarFLToe),
                            CmdToe = Convert.ToDouble(pModel.ToeFL_ST),
                            CmdToeToleranceMin = Convert.ToDouble(pModel.ToeFL_ST) - Convert.ToDouble(pModel.ToeFL_LT),
                            CmdToeToleranceMax = Convert.ToDouble(pModel.ToeFL_ST) + Convert.ToDouble(pModel.ToeFL_LT)
                        }
                    }
                },
                // REAR
                new ParallelizationAxleToolType
                {
                    Name = "ParallelizationAxleTool_REAR",
                    AxleType = "REAR",
                    InitialDissymetry = Convert.ToDouble(pResult.CarRCros_F),
                    FinalDissymetry = Convert.ToDouble(pResult.CarRCros),
                    CmdDissymetry = 0,
                    CmdDissymetryToleranceMin = -999,
                    CmdDissymetryToleranceMax = 999,
                    TotalToeVerdictOK = true,
                    InitialTotalToe = Convert.ToDouble(pResult.CarRTToe_F),
                    FinalTotalToe = Convert.ToDouble(pResult.CarRTToe),
                    CmdTotalToe = Convert.ToDouble(pModel.TotToer_ST),
                    CmdTotalToeToleranceMin = Convert.ToDouble(pModel.TotToer_ST) - Convert.ToDouble(pModel.TotToer_LT),
                    CmdTotalToeToleranceMax = Convert.ToDouble(pModel.TotToer_ST) + Convert.ToDouble(pModel.TotToer_LT),
                    ThrustAngleVerdictOK = GetResultOKNG(pResult.CarDogRu, pModel.DogRunST, pModel.DogRunLT),
                    InitialThrustAngle = Convert.ToDouble(pResult.CarDogRu_F),
                    FinalThrustAngle = Convert.ToDouble(pResult.CarDogRu),
                    CmdThrustAngle = Convert.ToDouble(pModel.DogRunST),
                    CmdThrustAngleToleranceMin = Convert.ToDouble(pModel.DogRunST) - Convert.ToDouble(pModel.DogRunLT),
                    CmdThrustAngleToleranceMax = Convert.ToDouble(pModel.DogRunST) + Convert.ToDouble(pModel.DogRunLT),
                    WheelTools = new List<ParallelizationWheelToolType>
                    {
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_RIGHT",
                            SideType = "RIGHT",
                            CamberVerdictOK = true,
                            InitialCamber = Convert.ToDouble(pResult.CarRRCam_F),
                            FinalCamber = Convert.ToDouble(pResult.CarRRCam),
                            CmdCamber = 0,
                            CmdCamberToleranceMin = -999,
                            CmdCamberToleranceMax = +999,
                            ToeVerdictOK = GetResultOKNG(pResult.CarRRToe, pModel.ToeRR_ST, pModel.ToeRR_LT),
                            InitialToe = Convert.ToDouble(pResult.CarRRToe_F),
                            FinalToe = Convert.ToDouble(pResult.CarRRToe),
                            CmdToe = Convert.ToDouble(pModel.ToeRR_ST),
                            CmdToeToleranceMin = Convert.ToDouble(pModel.ToeRR_ST) - Convert.ToDouble(pModel.ToeRR_LT),
                            CmdToeToleranceMax = Convert.ToDouble(pModel.ToeRR_ST) + Convert.ToDouble(pModel.ToeRR_LT)
                        },
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_LEFT",
                            SideType = "LEFT",
                            CamberVerdictOK = true,
                            InitialCamber = Convert.ToDouble(pResult.CarRLCam_F),
                            FinalCamber = Convert.ToDouble(pResult.CarRLCam),
                            CmdCamber = 0,
                            CmdCamberToleranceMin = -999,
                            CmdCamberToleranceMax = +999,
                            ToeVerdictOK = GetResultOKNG(pResult.CarRLToe, pModel.ToeRL_ST, pModel.ToeRL_LT),
                            InitialToe = Convert.ToDouble(pResult.CarRLToe_F),
                            FinalToe = Convert.ToDouble(pResult.CarRLToe),
                            CmdToe = Convert.ToDouble(pModel.ToeRL_ST),
                            CmdToeToleranceMin = Convert.ToDouble(pModel.ToeRL_ST) - Convert.ToDouble(pModel.ToeRL_LT),
                            CmdToeToleranceMax = Convert.ToDouble(pModel.ToeRL_ST) + Convert.ToDouble(pModel.ToeRL_LT)
                        }
                    }
                }
            }
                },
                LightingSettingProcessType = new LightingSettingProcessType
                {
                    VerdictOK = (pLamp.HLT___PK == "Pass"),
                    LightingTools = new List<LightingSettingToolType>
            {
                new LightingSettingToolType
                {
                    SideType = "LEFT",
                    BeamType = "LowBeam",
                    LightingToolType = "Elevation",
                    VerdictOK = (pLamp.Left_Res == "Pass"),
                    InitialMeasure = pLamp.LeftYVal_F,
                    FinalMeasure = pLamp.LeftYVal,
                    CmdTarget = pLamp.Target_Y_Left,
                    CmdToleranceMin = pLamp.Tolerance_Up_Left,
                    CmdToleranceMax = pLamp.Tolerance_Down_Left
                },
                new LightingSettingToolType
                {
                    SideType = "LEFT",
                    BeamType = "LowBeam",
                    LightingToolType = "Azimuth",
                    VerdictOK = (pLamp.Left_Res == "Pass"),
                    InitialMeasure = pLamp.LeftXVal_F,
                    FinalMeasure = pLamp.LeftXVal,
                    CmdTarget = pLamp.Target_X_Left,
                    CmdToleranceMin = pLamp.Tolerance_Left_Left,
                    CmdToleranceMax = pLamp.Tolerance_Right_Left
                },
                new LightingSettingToolType
                {
                    SideType = "RIGHT",
                    BeamType = "LowBeam",
                    LightingToolType = "Elevation",
                    VerdictOK = (pLamp.Right_Res == "Pass"),
                    InitialMeasure = pLamp.RightYVal_F,
                    FinalMeasure = pLamp.RightYVal,
                    CmdTarget = pLamp.Target_Y_Right,
                    CmdToleranceMin = pLamp.Tolerance_Up_Right,
                    CmdToleranceMax = pLamp.Tolerance_Down_Right
                },
                new LightingSettingToolType
                {
                    SideType = "RIGHT",
                    BeamType = "LowBeam",
                    LightingToolType = "Azimuth",
                    VerdictOK = (pLamp.Right_Res == "Pass"),
                    InitialMeasure = pLamp.RightXVal_F,
                    FinalMeasure = pLamp.RightXVal,
                    CmdTarget = pLamp.Target_X_Right,
                    CmdToleranceMin = pLamp.Tolerance_Left_Right,
                    CmdToleranceMax = pLamp.Tolerance_Right_Right
                }
            }
                },
                PEVProcessType = new PEVProcessType { VerdictOK = true }
            };

            return setting;
        }
    }
    [XmlRoot("STATION_SETTING")]
    public class StationSetting
    {
        public DataModel DataModel { get; set; }
        public FileFormat FileFormat { get; set; }
        public string MainPart_ID { get; set; }
        public bool TopStartCyclePart { get; set; }
        public bool TopPart { get; set; }
        public ParallelismSettingProcessType ParallelismSettingProcessType { get; set; }
        public LightingSettingProcessType LightingSettingProcessType { get; set; }
        public PEVProcessType PEVProcessType { get; set; }
    }

    public class DataModel
    {
        [XmlAttribute("version")]
        public string Version { get; set; }
    }

    public class FileFormat
    {
        [XmlAttribute("version")]
        public string Version { get; set; }
    }

    public class ParallelismSettingProcessType
    {
        public bool VerdictOK { get; set; }
        public string DiversityCode { get; set; }
        public string BenchNumber { get; set; }
        public string CmdSignConvention { get; set; }
        public double SwaOffset { get; set; }
        public double GearRatio { get; set; }
        public double CycleTime { get; set; }
        public double InitialSteeringWheelAngle { get; set; }
        public double FinalSteeringWheelAngle { get; set; }
        public double CmdSteeringWheelAngle { get; set; }
        public double CmdSteeringWheelAngleToleranceMin { get; set; }
        public double CmdSteeringWheelAngleToleranceMax { get; set; }
        public bool IsSettingsAdjustReference { get; set; }

        [XmlArray("ParallelizationAxleToolTypesList")]
        [XmlArrayItem("ParallelizationAxleToolType")]
        public List<ParallelizationAxleToolType> AxleTools { get; set; }
    }

    public class ParallelizationAxleToolType
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("AxleType")]
        public string AxleType { get; set; }

        public double InitialDissymetry { get; set; }
        public double FinalDissymetry { get; set; }
        public double CmdDissymetry { get; set; }
        public double CmdDissymetryToleranceMin { get; set; }
        public double CmdDissymetryToleranceMax { get; set; }
        public bool TotalToeVerdictOK { get; set; }
        public double InitialTotalToe { get; set; }
        public double FinalTotalToe { get; set; }
        public double CmdTotalToe { get; set; }
        public double CmdTotalToeToleranceMin { get; set; }
        public double CmdTotalToeToleranceMax { get; set; }
        public bool ThrustAngleVerdictOK { get; set; }
        public double InitialThrustAngle { get; set; }
        public double FinalThrustAngle { get; set; }
        public double CmdThrustAngle { get; set; }
        public double CmdThrustAngleToleranceMin { get; set; }
        public double CmdThrustAngleToleranceMax { get; set; }

        [XmlArray("ParallelizationWheelToolTypesList")]
        [XmlArrayItem("ParallelizationWheelToolType")]
        public List<ParallelizationWheelToolType> WheelTools { get; set; }
    }

    public class ParallelizationWheelToolType
    {
        [XmlAttribute("Name")]
        public string Name { get; set; }

        [XmlAttribute("SideType")]
        public string SideType { get; set; }

        public bool CamberVerdictOK { get; set; }
        public double InitialCamber { get; set; }
        public double FinalCamber { get; set; }
        public double CmdCamber { get; set; }
        public double CmdCamberToleranceMin { get; set; }
        public double CmdCamberToleranceMax { get; set; }

        public bool ToeVerdictOK { get; set; }
        public double InitialToe { get; set; }
        public double FinalToe { get; set; }
        public double CmdToe { get; set; }
        public double CmdToeToleranceMin { get; set; }
        public double CmdToeToleranceMax { get; set; }
    }

    public class LightingSettingProcessType
    {
        public bool VerdictOK { get; set; }

        [XmlArray("LightingSettingToolTypesList")]
        [XmlArrayItem("LightingSettingToolType")]
        public List<LightingSettingToolType> LightingTools { get; set; }
    }

    public class LightingSettingToolType
    {
        [XmlAttribute("SideType")]
        public string SideType { get; set; }

        [XmlAttribute("BeamType")]
        public string BeamType { get; set; }

        [XmlAttribute("LightingToolType")]
        public string LightingToolType { get; set; }

        public bool VerdictOK { get; set; }
        public double InitialMeasure { get; set; }
        public double FinalMeasure { get; set; }
        public double CmdTarget { get; set; }
        public double CmdToleranceMin { get; set; }
        public double CmdToleranceMax { get; set; }
    }

    public class PEVProcessType
    {
        public bool VerdictOK { get; set; }
    }
}

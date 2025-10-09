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


        public StationSetting MakeXMLStruct(TblCarModel pModel, TblCarInfo pInfo, TblResult pResult)
        {
            
            StationSetting setting = new StationSetting
            {
                DataModel = new DataModel { Version = "1.2.I" },
                FileFormat = new FileFormat { Version = "1.2.I" },
                MainPart_ID = "201215836921B",
                TopStartCyclePart = true,
                TopPart = true,
                ParallelismSettingProcessType = new ParallelismSettingProcessType
                {
                    VerdictOK = true,
                    DiversityCode = "DIV1",
                    BenchNumber = "1",
                    CmdSignConvention = "ISO",
                    SwaOffset = 1.2,
                    GearRatio = 1.0,
                    CycleTime = 11,
                    InitialSteeringWheelAngle = 1.3,
                    FinalSteeringWheelAngle = 1.4,
                    CmdSteeringWheelAngle = 2.4,
                    CmdSteeringWheelAngleToleranceMin = 1.5,
                    CmdSteeringWheelAngleToleranceMax = 1.6,
                    IsSettingsAdjustReference = true,
                    AxleTools = new List<ParallelizationAxleToolType>
            {
                // FRONT
                new ParallelizationAxleToolType
                {
                    Name = "ParallelizationAxleTool_FRONT",
                    AxleType = "FRONT",
                    InitialDissymetry = 1.1,
                    FinalDissymetry = 1.2,
                    CmdDissymetry = 1.22,
                    CmdDissymetryToleranceMin = 1.3,
                    CmdDissymetryToleranceMax = 1.4,
                    TotalToeVerdictOK = true,
                    InitialTotalToe = 1.5,
                    FinalTotalToe = 1.6,
                    CmdTotalToe = 1.62,
                    CmdTotalToeToleranceMin = 1.7,
                    CmdTotalToeToleranceMax = 1.8,
                    ThrustAngleVerdictOK = true,
                    InitialThrustAngle = 1.9,
                    FinalThrustAngle = 1.10,
                    CmdThrustAngle = 1.12,
                    CmdThrustAngleToleranceMin = 1.2,
                    CmdThrustAngleToleranceMax = 1.3,
                    WheelTools = new List<ParallelizationWheelToolType>
                    {
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_RIGHT",
                            SideType = "RIGHT",
                            CamberVerdictOK = true,
                            InitialCamber = 1.11,
                            FinalCamber = 1.22,
                            CmdCamber = 1.23,
                            CmdCamberToleranceMin = 1.33,
                            CmdCamberToleranceMax = 1.44,
                            ToeVerdictOK = true,
                            InitialToe = 1.55,
                            FinalToe = 1.66,
                            CmdToe = 1.67,
                            CmdToeToleranceMin = 1.77,
                            CmdToeToleranceMax = 1.88
                        },
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_LEFT",
                            SideType = "LEFT",
                            CamberVerdictOK = true,
                            InitialCamber = 2.11,
                            FinalCamber = 2.22,
                            CmdCamber = 2.23,
                            CmdCamberToleranceMin = 2.33,
                            CmdCamberToleranceMax = 2.44,
                            ToeVerdictOK = true,
                            InitialToe = 2.55,
                            FinalToe = 2.66,
                            CmdToe = 2.67,
                            CmdToeToleranceMin = 2.77,
                            CmdToeToleranceMax = 2.88
                        }
                    }
                },
                // REAR
                new ParallelizationAxleToolType
                {
                    Name = "ParallelizationAxleTool_REAR",
                    AxleType = "REAR",
                    InitialDissymetry = 3.1,
                    FinalDissymetry = 3.2,
                    CmdDissymetry = 3.22,
                    CmdDissymetryToleranceMin = 3.3,
                    CmdDissymetryToleranceMax = 3.4,
                    TotalToeVerdictOK = true,
                    InitialTotalToe = 3.5,
                    FinalTotalToe = 3.6,
                    CmdTotalToe = 3.62,
                    CmdTotalToeToleranceMin = 3.7,
                    CmdTotalToeToleranceMax = 3.8,
                    ThrustAngleVerdictOK = true,
                    InitialThrustAngle = 3.9,
                    FinalThrustAngle = 3.10,
                    CmdThrustAngle = 3.12,
                    CmdThrustAngleToleranceMin = 3.2,
                    CmdThrustAngleToleranceMax = 3.3,
                    WheelTools = new List<ParallelizationWheelToolType>
                    {
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_RIGHT",
                            SideType = "RIGHT",
                            CamberVerdictOK = true,
                            InitialCamber = 3.11,
                            FinalCamber = 3.22,
                            CmdCamber = 3.23,
                            CmdCamberToleranceMin = 3.33,
                            CmdCamberToleranceMax = 3.44,
                            ToeVerdictOK = true,
                            InitialToe = 3.55,
                            FinalToe = 3.66,
                            CmdToe = 3.67,
                            CmdToeToleranceMin = 3.77,
                            CmdToeToleranceMax = 3.88
                        },
                        new ParallelizationWheelToolType {
                            Name = "ParallelizationWheelTool_LEFT",
                            SideType = "LEFT",
                            CamberVerdictOK = true,
                            InitialCamber = 4.11,
                            FinalCamber = 4.22,
                            CmdCamber = 4.23,
                            CmdCamberToleranceMin = 4.33,
                            CmdCamberToleranceMax = 4.44,
                            ToeVerdictOK = true,
                            InitialToe = 4.55,
                            FinalToe = 4.66,
                            CmdToe = 4.67,
                            CmdToeToleranceMin = 4.77,
                            CmdToeToleranceMax = 4.88
                        }
                    }
                }
            }
                },
                LightingSettingProcessType = new LightingSettingProcessType
                {
                    VerdictOK = true,
                    LightingTools = new List<LightingSettingToolType>
            {
                new LightingSettingToolType
                {
                    SideType = "LEFT",
                    BeamType = "LowBeam",
                    LightingToolType = "Elevation",
                    VerdictOK = true,
                    InitialMeasure = 2.1,
                    FinalMeasure = 2.2,
                    CmdTarget = 2.3,
                    CmdToleranceMin = 2.4,
                    CmdToleranceMax = 2.5
                },
                new LightingSettingToolType
                {
                    SideType = "LEFT",
                    BeamType = "LowBeam",
                    LightingToolType = "Azimuth",
                    VerdictOK = true,
                    InitialMeasure = 2.6,
                    FinalMeasure = 2.7,
                    CmdTarget = 2.8,
                    CmdToleranceMin = 2.9,
                    CmdToleranceMax = 3.0
                },
                new LightingSettingToolType
                {
                    SideType = "RIGHT",
                    BeamType = "LowBeam",
                    LightingToolType = "Elevation",
                    VerdictOK = true,
                    InitialMeasure = 3.1,
                    FinalMeasure = 3.2,
                    CmdTarget = 3.3,
                    CmdToleranceMin = 3.4,
                    CmdToleranceMax = 3.5
                },
                new LightingSettingToolType
                {
                    SideType = "RIGHT",
                    BeamType = "LowBeam",
                    LightingToolType = "Azimuth",
                    VerdictOK = true,
                    InitialMeasure = 3.6,
                    FinalMeasure = 3.7,
                    CmdTarget = 3.8,
                    CmdToleranceMin = 3.9,
                    CmdToleranceMax = 4.0
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

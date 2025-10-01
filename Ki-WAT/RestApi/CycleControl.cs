using Ki_WAT;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using static Ki_WAT.GlobalVal;

namespace LETInterface
{
    public class CycleControl
    {
        private readonly TaskService _service;
        GlobalVal _GV = GlobalVal.Instance;
        

        

        public CycleControl()
        {
            _service = new TaskService(_GV.Config.Device.LET_URL, Int32.Parse(_GV.Config.Device.LET_PORT));

        }

        public CycleControl(TaskService service)
        {
            _service = service;
            //_service = new TaskService(_GV.Config.Device.LET_URL, Int32.Parse(_GV.Config.Device.LET_PORT));

        }

        public bool StartCycle(string uid)
        {
            try
            {
                var req = new { context = "System", activity = "start-cycle", uid };
                (bool success, string message) = TasksProcessTask(req);

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"StartCycle 예외: {ex.Message}");
                return false;
            }
        }

        public bool VehicleSelection(int? vsn, string vin)
        {
            try
            {
                var req = new { context = "System", activity = "vehicle-selection", vehicle = vsn, identification = vin };
                (bool success, string message) = TasksProcessTask(req);
                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"VehicleSelection 예외: {ex.Message}");
                return false;
            }
        }

        public bool PerformTests(string line, double? floorpitch)
        {
            try
            {
                var req = new { context = "System", activity = "perform-tests", line, floorpitch };
                (bool success, string message) = TasksProcessTask(req);

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"PerformTests 예외: {ex.Message}");

                return false;
            }
        }

        public bool EndCycle()
        {
            try
            {
                var req = new { context = "System", activity = "end-cycle" };
                (bool success, string message) = TasksProcessTask(req);

                return success;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"EndCycle 예외: {ex.Message}");
                return false;
            }
        }

        public void DeleteAllTest()
        {
            try
            {
                _service.TaskDeleteAll();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"DeleteAllTest 예외: {ex.Message}");
            }
        }
        public (bool Success, string Message) TasksProcessTask(object content, int timeoutMs = 60000)
        {
            try
            {
                var response = _service.TaskAddNew();
                if (response.StatusCode != System.Net.HttpStatusCode.Created)
                    return (false, "Error: adding new task");

                string location = response.Headers.Location.ToString();
                response = _service.TaskSpecify(location, content);
                if (response.StatusCode != System.Net.HttpStatusCode.Accepted)
                    return (false, "Error: specifying task");

                var sw = System.Diagnostics.Stopwatch.StartNew();
                while (true)
                {
                    if (sw.ElapsedMilliseconds > timeoutMs)
                        return (false, "Timeout: task did not finish in time");

                    response = _service.TaskGet(location);
                    if (response.StatusCode != System.Net.HttpStatusCode.OK)
                        return (false, "Error: inspecting task state");

                    var result = JsonDocument.Parse(response.Content.ReadAsStringAsync().Result);
                    string state = result.RootElement.GetProperty("state").GetString();

                    if (state == "finished") break;
                    Thread.Sleep(500);
                }
                return (true, "Success");
            }
            catch (Exception ex)
            {
                // TODO: 필요시 로그 기록 추가
                return (false, $"Exception: {ex.Message}");
            }
        }

        public string GetLastResult()
        {
            try
            {
                string strLastRes = _service.GetLastResult();
                return strLastRes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLastResult 예외: {ex.Message}");
                return string.Empty;
            }
        }

        public string GetResult(string uID)
        {
            try
            {
                string strLastRes = _service.GetResultByUid(uID);
                return strLastRes;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GetLastResult 예외: {ex.Message}");
                return string.Empty;
            }
        }


        public LampInclination ParseInclinationFromXml(string xmlContent)
        {
            var result = new LampInclination();

            try
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlContent);

                XmlNodeList lamps = doc.GetElementsByTagName("Lamp");

                foreach (XmlNode lamp in lamps)
                {
                    var side = lamp.Attributes?["side"]?.InnerText;
                    var Type = lamp.Attributes?["beam_type"]?.InnerText;


                    if (string.IsNullOrWhiteSpace(side)) continue;

                    string xFinalStr = lamp["Inclination_X_final"]?.InnerText;
                    string yFinalStr = lamp["Inclination_Y_final"]?.InnerText;

                    string xInitStr = lamp["Inclination_X_initial"]?.InnerText;
                    string yInitStr = lamp["Inclination_Y_initial"]?.InnerText;

                    double? xFinal = double.TryParse(xFinalStr, out var x) ? x : (double?)null;
                    double? yFinal = double.TryParse(yFinalStr, out var y) ? y : (double?)null;

                    double? xInit = double.TryParse(xInitStr, out var x1) ? x1 : (double?)null;
                    double? yInit = double.TryParse(yInitStr, out var y1) ? y1 : (double?)null;


                    if ( Type == "Low")
                    {
                        if (side == "Left")
                        {
                            result.Low_InclinationXFinal_Left = (double)xFinal;
                            result.Low_InclinationYFinal_Left = (double)yFinal;
                            result.Low_InclinationXInit_Left = (double)xInit;
                            result.Low_InclinationYInit_Left = (double)yInit;
                        }
                        else if (side == "Right")
                        {
                            result.Low_InclinationXFinal_Right = (double)xFinal;
                            result.Low_InclinationYFinal_Right = (double)yFinal;
                                   
                            result.Low_InclinationXInit_Right = (double)xInit;
                            result.Low_InclinationYInit_Right = (double)yInit;
                        }
                    }

                    if (Type == "High")
                    {
                        if (side == "Left")
                        {
                            result.High_InclinationXFinal_Left = (double)xFinal;
                            result.High_InclinationYFinal_Left = (double)yFinal;
                            result.High_InclinationXInit_Left = (double)xInit;
                            result.High_InclinationYInit_Left = (double)yInit;
                        }
                        else if (side == "Right")
                        {
                            result.High_InclinationXFinal_Right = (double)xFinal;
                            result.High_InclinationYFinal_Right = (double)yFinal;
                            result.High_InclinationXInit_Right = (double)xInit;
                            result.High_InclinationYInit_Right = (double)yInit;
                        }
                    }

                    if (Type == "Fog")
                    {
                        if (side == "Left")
                        {
                            result.Fog_InclinationXFinal_Left = (double)xFinal;
                            result.Fog_InclinationYFinal_Left = (double)yFinal;
                            result.Fog_InclinationXInit_Left = (double)xInit;
                            result.Fog_InclinationYInit_Left = (double)yInit;
                        }
                        else if (side == "Right")
                        {
                            result.Fog_InclinationXFinal_Right = (double)xFinal;
                            result.Fog_InclinationYFinal_Right = (double)yFinal;
                            result.Fog_InclinationXInit_Right = (double)xInit;
                            result.Fog_InclinationYInit_Right = (double)yInit;
                        }
                    }




                }
            }
            catch (XmlException xe)
            {
                Console.WriteLine("XML 구문 오류: " + xe.Message);
            }
            catch (Exception ex)
            {
                Console.WriteLine("처리 중 예외 발생: " + ex.Message);
            }
            return result;
        }
    }
}

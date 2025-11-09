using NModbus;
using NModbus.Serial;
using System;
using System.IO.Ports;
using System.Threading;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Windows.Forms;
using Ki_WAT;

namespace HandleLeveler
{
    public class AngleCalculator : IDisposable
    {

        GlobalVal _GV = GlobalVal.Instance;
        private IModbusMaster master;
        private SerialPort port;
        private readonly byte slaveId = 1;

        public bool IsConnected { get; private set; } = false;
        private readonly object _modbusLock = new object();
        private readonly object _queueLock = new object();
        private double overrideAngle = 0;
        private CancellationTokenSource resetOverrideTimerCts = null;

        private CancellationTokenSource _cancellationTokenSource;
        private Task _communicationTask;
        private readonly Queue<Tuple<ushort, ushort>> _writeQueue = new Queue<Tuple<ushort, ushort>>();

        // INI File AD points
        private double adZeroPoint;
        private double adPlusPoint;
        private double adMinusPoint;

        // UI Update Actions
        //private readonly Action<string> _updateStatusMessage;
        public Action<string, string, string, string> _updateDisplayValues;
        //private readonly Action<bool> _updateConnectionStateUI;
        public Action<double> _UpdateAngle;

        public AngleCalculator(Action<string, string, string, string> updateDisplay, Action<double> UpdateAngle)
        {
            _updateDisplayValues = updateDisplay;
            _UpdateAngle = UpdateAngle;
            
        }


        public void Connect(string portName, int baudRate)
        {
            if (IsConnected) return;
            if (portName == "") return;
            try
            {

                adZeroPoint = _GV.Config.SWB.adZeroPoint;
                adPlusPoint = _GV.Config.SWB.adPlusPoint;
                adMinusPoint = _GV.Config.SWB.adMinusPoint;

                port = new SerialPort(portName, baudRate)
                {
                    ReadTimeout = 50,
                    WriteTimeout = 50
                };
                port.Open();

                var factory = new ModbusFactory();
                var adapter = new SerialPortAdapter(port);
                master = factory.CreateRtuMaster(adapter);

                IsConnected = true;

                //SetFndDisplay(_GV.Config.SWB.BoardType);
                _cancellationTokenSource = new CancellationTokenSource();
                _communicationTask = Task.Run(() => CommunicationLoop(_cancellationTokenSource.Token), _cancellationTokenSource.Token);

                Broker.dsBroker.Publish(Topics.DS.SWB, Topics.DS.Connect);

            }
            catch (Exception ex)
            {
                //_updateStatusMessage($"연결 오류: {ex.Message}");
                port?.Close();
                IsConnected = false;
                Broker.dsBroker.Publish(Topics.DS.SWB, Topics.DS.NotConnect);

            }
        }

        public void Disconnect(bool resetPcAngle = false)
        {
            if (!IsConnected) return;

            IsConnected = false;

            try
            {
                _cancellationTokenSource?.Cancel();
                _communicationTask?.Wait(500);
            }
            catch (Exception ex)
            {
                //_updateStatusMessage($"Cancellation-Token 해제 오류: {ex.Message}");
            }

            if (resetPcAngle && master != null)
            {
                try
                {
                    master.WriteSingleRegister(slaveId, 5, 0);
                    //_updateStatusMessage("PC 각도를 0으로 초기화했습니다.");
                    Thread.Sleep(50);
                }
                catch (Exception ex)
                {
                    //_updateStatusMessage($"PC 각도 초기화 오류: {ex.Message}");
                }
            }
            
            Dispose();
            //_updateConnectionStateUI(false);
            //_updateStatusMessage("통신 정지됨");
        }

        private void CommunicationLoop(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                if (!Monitor.TryEnter(_modbusLock, 50))
                {
                    try { Task.Delay(50, token).Wait(token); } catch (OperationCanceledException) { break; }
                    continue;
                }

                try
                {
                    if (_writeQueue.Count > 0)
                    {
                        Tuple<ushort, ushort> writeRequest;
                        lock (_queueLock)
                        {
                            writeRequest = _writeQueue.Dequeue();
                        }
                        master.WriteSingleRegister(slaveId, writeRequest.Item1, writeRequest.Item2);
                    }
                    else
                    {
                        ushort[] registers = master.ReadHoldingRegisters(slaveId, 1, 5);
                        if (registers.Length == 5)
                        {
                            ProcessAndDisplayData(registers);
                        }
                    }
                }
                catch (Exception ex)
                {
                    if (!token.IsCancellationRequested)
                    {
                        MessageBox.Show(ex.Message);
                         
                        //_updateStatusMessage($"통신 오류: {ex.Message}. 연결을 종료합니다.");
                        Disconnect(false);
                    }
                    break;
                }
                finally
                {
                    Monitor.Exit(_modbusLock);
                }
            }
        }

        private void ProcessAndDisplayData(ushort[] registers)
        {
            double dAngle;
            string fndDisplay = registers[0] == 0 ? "보드" : "PC";
            int sensorAdValue = (registers[1] * 0x10000) + registers[2];
            
            int boardAngleInt = (int)registers[3];

            if (boardAngleInt >= 0x8000)
            {
                boardAngleInt = (boardAngleInt - 0x8000) * -1;
            }
            double boardAngle = boardAngleInt / 100.0;
            double pcAngle = 0;
            if (registers[0] == 1) // PC Control
            {
                pcAngle =  CalculatePcAngle(sensorAdValue);
                
                int writeValue = (int)(pcAngle * 100);
                ushort writeAngle = (ushort)(writeValue >= 0 ? writeValue : (Math.Abs(writeValue) | 0x8000));

                boardAngle = pcAngle;
                WriteRegister(5, writeAngle);
            }
            _UpdateAngle?.Invoke(boardAngle);
            _updateDisplayValues?.Invoke(fndDisplay, sensorAdValue.ToString(), boardAngle.ToString("F2"), pcAngle.ToString("F2"));
        }

        public double CalculatePcAngle(int sensorAdValue)
        {
            if (adZeroPoint == 0) return 0;

            const double angleZeroPoint = 0.0;
            const double anglePlusPoint = 10.0;
            const double angleMinusPoint = -10.0;

            double plusSensitivity = (anglePlusPoint - angleZeroPoint) / (adPlusPoint - adZeroPoint);
            double plusOffset = angleZeroPoint - plusSensitivity * adZeroPoint;

            double minusSensitivity = (angleMinusPoint - angleZeroPoint) / (adMinusPoint - adZeroPoint);
            double minusOffset = angleZeroPoint - minusSensitivity * adZeroPoint;

            double calculatedAngle = sensorAdValue >= adZeroPoint
                ? (sensorAdValue * plusSensitivity) + plusOffset
                : (sensorAdValue * minusSensitivity) + minusOffset;

            return Math.Max(-15.00, Math.Min(15.00, calculatedAngle));
        }

        public void WriteRegister(ushort address, ushort value)
        {
            if (!IsConnected || master == null) return;
            lock (_queueLock)
            {
                _writeQueue.Enqueue(Tuple.Create(address, value));
            }
        }
        
        public void SetFndDisplay(int isPcControl)
        {
            WriteRegister(1, (ushort)(isPcControl));
        }

        public void SetOverrideAngle(double angle)
        {
            resetOverrideTimerCts?.Cancel();
            resetOverrideTimerCts?.Dispose();

            overrideAngle = angle;

            if (angle != 0 )
            {
                resetOverrideTimerCts = new CancellationTokenSource();
                CancellationToken token = resetOverrideTimerCts.Token;

                Task.Delay(5000, token).ContinueWith(t =>
                {
                    if (!t.IsCanceled)
                    {
                        overrideAngle = 0;
                    }
                }, TaskScheduler.FromCurrentSynchronizationContext());
            }
        }

        public void Dispose()
        {
            _cancellationTokenSource?.Dispose();
            master?.Dispose();
            port?.Dispose();
            _communicationTask = null;
            _cancellationTokenSource = null;
            master = null;
            port = null;
            GC.SuppressFinalize(this);
        }
    }
}
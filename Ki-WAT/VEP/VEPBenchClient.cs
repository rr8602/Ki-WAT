
using Microsoft.Win32;

using NModbus;
using NModbus.Extensions;
using NModbus.Extensions.Enron;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Sockets;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ki_WAT
{
    public class VEPBenchClient
    {
        private string _ip;
        private int _port;
        private GlobalVal _GV;
        private TcpClient _tcpClient;
        private IModbusMaster _modbusMaster;
        private bool _isConnected = false;

        // 시작 주소 값
        public ushort Addr_Validity ;
        public ushort Addr_StatusZone;
        public ushort Addr_SynchroZone;
        public ushort Addr_TransmissionZone;
        public ushort Addr_ReceptionZone;

        public bool IsConnected => _isConnected && _tcpClient != null && _tcpClient.Connected;

        // 연결 실패 시 재시도 설정
        private const int MaxRetryCount = 3;
        private const int RetryDelayMs = 1000;

        private Task _pollingTask = null;
        private CancellationTokenSource _cancellationTokenSouce = null;
        private int _pollingIntervalMs = 300;

        private VEPBenchDataManager _vepManager;

        // 폴링 주기 설정
        public int PollingIntervalMs
        {
            get { return _pollingIntervalMs; }
            set { _pollingIntervalMs = Math.Max(100, value); }
        }

        // Zone 변경 이벤트
        private EventHandler<VEPBenchDescriptionZone> _descriptionZoneRead;
        public event EventHandler<VEPBenchDescriptionZone> DescriptionZoneRead
        {
            add
            {
                _descriptionZoneRead += value;
                OnDescriptionZoneRead();
            }

            remove { _descriptionZoneRead -= value; }
        }

        private EventHandler<VEPBenchStatusZone> _statusZoneChanged;
        public event EventHandler<VEPBenchStatusZone> StatusZoneChanged
        {
            add
            {
                _statusZoneChanged += value;
                OnStatusZoneChanged();
            }

            remove { _statusZoneChanged -= value; }
        }

        private EventHandler<VEPBenchSynchroZone> _synchroZoneChanged;
        public event EventHandler<VEPBenchSynchroZone> SynchroZoneChanged
        {
            add
            {
                _synchroZoneChanged += value;
                OnSynchroZoneChanged();
            }

            remove { _synchroZoneChanged -= value; }
        }

        private EventHandler<VEPBenchTransmissionZone> _transmissionZoneChanged;
        public event EventHandler<VEPBenchTransmissionZone> TransmissionZoneChanged
        {
            add
            {
                _transmissionZoneChanged += value;
                OnTransmissionZoneChanged();
            }

            remove { _transmissionZoneChanged -= value; }
        }

        private EventHandler<VEPBenchReceptionZone> _receptionZoneChanged;
        public event EventHandler<VEPBenchReceptionZone> ReceptionZoneChanged
        {
            add
            {
                _receptionZoneChanged += value;
                OnReceptionZoneChanged();
            }

            remove { _receptionZoneChanged -= value; }
        }

        public VEPBenchClient(string ip, int port)
        {
            _ip = ip;
            _port = port;
        }
        public VEPBenchClient(GlobalVal pGlobal, VEPBenchDataManager pData)
        {
            _GV = pGlobal;
            _vepManager = pData;
            pData.SetParent(this);
        }

        private void InitializeAddresses()
        {
            Addr_Validity = _vepManager.DescriptionZone.ValidityIndicator;
            Addr_StatusZone = _vepManager.DescriptionZone.StatusZoneAddr;
            Addr_SynchroZone = _vepManager.DescriptionZone.SynchroZoneAddr;
            Addr_TransmissionZone = _vepManager.DescriptionZone.TransmissionZoneAddr;
            Addr_ReceptionZone = _vepManager.DescriptionZone.ReceptionZoneAddr;

            _vepManager.StatusZone.SetSize(_vepManager.DescriptionZone.StatusZoneSize);
            _vepManager.SynchroZone.SetSize(_vepManager.DescriptionZone.SynchroZoneSize);
            _vepManager.ReceptionZone.SetSize(_vepManager.DescriptionZone.ReceptionZoneSize);
            _vepManager.TransmissionZone.SetSize(_vepManager.DescriptionZone.TransmissionZoneSize);
            _vepManager.AddTransmissionZone.SetSize(_vepManager.DescriptionZone.AdditionalTZSize);

        }

        public void PerformInitialRead()
        {
            try
            {
                if (!IsConnected)
                {
                    Connect();
                }

                PollAllZones();
                LogMessage("초기 Modbus 데이터 읽기 완료.");
            }
            catch (Exception ex)
            {
                LogMessage($"초기 Modbus 데이터 읽기 오류: {ex.Message}");
            }
        }
        private void Connect()
        {
            Connect(_ip, _port);
        }

        public void InitializeAndReadDescriptionZone()
        {
            try
            {
                if (!IsConnected)
                    Connect();

                ReadDescriptionZone();
                OnDescriptionZoneRead();
                InitializeAddresses();
                LogMessage("Description Zone 초기 읽기 완료");
            }
            catch (Exception ex)
            {
                LogMessage($"Description Zone 초기 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void Connect(string pIP, int pPort)
        {
            try
            {
                _ip = pIP;
                _port = pPort;
                if (_isConnected || (_tcpClient != null && _tcpClient.Connected))
                {
                    DisConnect();
                }

                int retryCount = 0;
                bool connected = false;

                while (!connected && retryCount < MaxRetryCount)
                {
                    try
                    {
                        _tcpClient = new TcpClient();

                        var connectTask = _tcpClient.ConnectAsync(_ip, _port);

                        if (!connectTask.Wait(5000))
                        {
                            throw new TimeoutException("연결 시간이 초과되었습니다.");
                        }

                        var factory = new ModbusFactory();
                        _modbusMaster = factory.CreateMaster(_tcpClient);

                        // 타임아웃 설정
                        _modbusMaster.Transport.ReadTimeout = 3000;  // 읽기 타임아웃 3초
                        _modbusMaster.Transport.WriteTimeout = 3000; // 쓰기 타임아웃 3초

                        connected = true;
                        _isConnected = true;
                        LogMessage("Modbus 서버에 연결되었습니다.");
                    }
                    catch (Exception ex)
                    {
                        retryCount++;
                        LogMessage($"연결 시도 {retryCount}/{MaxRetryCount} 실패: {ex.Message}");

                        if (_tcpClient != null)
                        {
                            _tcpClient.Dispose();
                            _tcpClient = null;
                        }

                        if (retryCount < MaxRetryCount)
                        {
                            Thread.Sleep(RetryDelayMs);
                        }
                        else
                        {
                            throw new Exception($"최대 재시도 횟수({MaxRetryCount})를 초과했습니다. 연결 실패: {ex.Message}");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                LogMessage($"연결 오류: {ex.Message}");
                _isConnected = false;
                throw;
            }
        }

        public void DisConnect()
        {
            try
            {
                if (_modbusMaster != null)
                {
                    _modbusMaster.Dispose();
                    _modbusMaster = null;
                }

                if (_tcpClient != null)
                {
                    if (_tcpClient.Connected)
                    {
                        _tcpClient.Close();
                    }

                    _tcpClient.Dispose();
                    _tcpClient = null;
                }
            }
            catch (Exception ex)
            {
                LogMessage($"연결 해제 오류: {ex.Message}");
            }
            finally
            {
                _isConnected = false;
                LogMessage("Modbus 연결이 해제되었습니다.");
            }
        }

        // 주기적 Read 시작
        public void StartMonitoring()
        {
            if (_pollingTask != null && !_pollingTask.IsCompleted)
            {
                LogMessage("이미 모니터링이 실행 중입니다.");

                return;
            }

            _cancellationTokenSouce = new CancellationTokenSource();
            var token = _cancellationTokenSouce.Token;

            _pollingTask = Task.Run(() =>
            {
                LogMessage("모니터링 시작");

                try
                {
                    InitializeAndReadDescriptionZone();
                    PollAllZones();

                    while (!token.IsCancellationRequested)
                    {
                        Thread.Sleep(_pollingIntervalMs);

                        if (token.IsCancellationRequested)
                            break;

                        if (IsConnected)
                        {
                            PollAllZones();
                        }
                        else
                        {
                            try
                            {
                                LogMessage("연결이 끊어졌습니다. 재연결 시도 중...");
                                Connect();
                            }
                            catch (Exception ex)
                            {
                                LogMessage($"재연결 시도 중 오류 발생: {ex.Message}");
                                Thread.Sleep(5000);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"모니터링 중 오류 발생: {ex.Message}");
                }

                LogMessage("모니터링 종료");
            }, token);
        }

        // Read 중지
        public void StopMonitoring()
        {
            if (_cancellationTokenSouce != null)
            {
                _cancellationTokenSouce.Cancel();

                try
                {
                    if (_pollingTask != null)
                    {
                        bool completed = _pollingTask.Wait(3000);

                        if (!completed)
                        {
                            LogMessage("모니터링 작업이 시간 내에 완료되지 않았습니다. 강제 종료합니다.");
                        }
                    }
                }
                catch (Exception ex)
                {
                    LogMessage($"모니터링 중지 오류: {ex.Message}");
                }
                finally
                {
                    _pollingTask = null;
                    _cancellationTokenSouce.Dispose();
                    _cancellationTokenSouce = null;

                    LogMessage("모니터링이 중지되었습니다.");
                }
            }
        }

        // 모든 Zone 폴링
        private void PollAllZones()
        {
            try
            {
                //PollValidityIndicator();
                _vepManager.UpdateAllZonesFromRegisters(ReadAllRegisters);
                
                if (_vepManager.DescriptionZone.IsChanged)
                {
                    OnDescriptionZoneRead();
                    _vepManager.DescriptionZone.ResetChangedState();
                }

                if (_vepManager.StatusZone.IsChanged)
                {
                    OnStatusZoneChanged();
                    _vepManager.StatusZone.ResetChangedState();
                }

                if (_vepManager.SynchroZone.IsChanged)
                {
                    OnSynchroZoneChanged();
                    _vepManager.SynchroZone.ResetChangedState();
                }

                if (_vepManager.TransmissionZone.IsChanged)
                {
                    OnTransmissionZoneChanged();
                    _vepManager.TransmissionZone.ResetChangedState();
                }

                if (_vepManager.ReceptionZone.IsChanged)
                {
                    OnReceptionZoneChanged();
                    _vepManager.ReceptionZone.ResetChangedState();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"모든 Zone 폴링 중 오류 발생: {ex.Message}");
            }
        }

        private void PollValidityIndicator()
        {
            try
            {
                ushort validity = ReadValidityIndicator();

                if (validity == 0)
                {
                    LogMessage("경고: VEP 소프트웨어가 준비되지 않았습니다.");
                }
            }
            catch (Exception ex)
            {
                LogMessage($"유효성 표시기 폴링 오류: {ex.Message}");
            }
        }

        private ushort[] ReadAllRegisters(int address, int count)
        {
            CheckConnection();

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, (ushort)address, (ushort)count);

                return registers;
            }
            catch (Exception ex)
            {
                LogMessage($"ReadAllRegisters 오류 발생: Addr={address}, Count={count}, Error={ex.Message}");
                throw;
            }
        }

        public ushort ReadValidityIndicator()
        {
            CheckConnection();

            try
            {
                var values = _modbusMaster.ReadHoldingRegisters(1, Addr_Validity, 1); // VEP 서버

                if (values[0] == 0)
                {
                    throw new Exception("VEP 소프트웨어가 준비되지 않았습니다.");
                }

                LogMessage($"유효성 표시기 읽기: {values[0]} " +
                    $"(0=VEP 소프트웨어 미준비, 1=VEP 소프트웨어 준비됨)");

                return values[0];
            }
            catch (Exception ex)
            {
                LogMessage($"유효성 표시기 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public VEPBenchDescriptionZone ReadDescriptionZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, _vepManager.DescriptionZone.ValidityIndicator, _vepManager.DescriptionZone.Length);

                _vepManager.DescriptionZone.FromRegisters(registers);

                LogMessage($"Description Zone 읽기 성공: ValidityIndicator={_vepManager.DescriptionZone.ValidityIndicator}, " +
                  $"StatusZoneAddr={_vepManager.DescriptionZone.StatusZoneAddr}, StatusZoneSize={_vepManager.DescriptionZone.StatusZoneSize}");

                return _vepManager.DescriptionZone;
            }
            catch (Exception ex)
            {
                LogMessage($"Description Zone 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteStatusZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _vepManager.StatusZone.ToRegisters();
                //_modbusMaster.WriteMultipleRegisters(1, Addr_StatusZone, registers);
                WriteMultiple(1, Addr_StatusZone, registers);
                LogMessage($"Status Zone 쓰기 성공: VepStatus={_vepManager.StatusZone.GetVepStatusString()}, " +
                 $"StartCycle={_vepManager.StatusZone.StartCycle}, " +
                 $"VepCycleEnd={_vepManager.StatusZone.VepCycleEnd}, " +
                 $"BenchCycleEnd={_vepManager.StatusZone.BenchCycleEnd}");

                OnStatusZoneChanged();
            }
            catch (Exception ex)
            {
                LogMessage($"상태 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        public ushort[] ReadSynchroZone(int startIndex, int count)
        {
            CheckConnection();

            ushort modbusStartAddr = (ushort)(Addr_SynchroZone + startIndex);

            if (count < 1 || count > 123)
                throw new ArgumentOutOfRangeException(nameof(count), "numberOfPoints must be between 1 and 123 inclusive.");

            try
            {
                ushort[] registers = _modbusMaster.ReadHoldingRegisters(1, modbusStartAddr, (ushort)count);
                LogMessage($"동기화 영역 부분 읽기: Start={modbusStartAddr}, Count={count}, Data={string.Join(",", registers)}");

                return registers;
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 읽기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteSyncroZone(ushort pSyncro, ushort nVal)
        {
            CheckConnection();

            try
            {
                //_vepManager.SynchroZone.SetValue(pSyncro, nVal);
                ushort addr = (ushort)(Addr_SynchroZone + pSyncro);
                _modbusMaster.WriteSingleRegister(1, addr, nVal);
                OnSynchroZoneChanged();


            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }

        }
        public void WriteSynchroZone()
        {
            CheckConnection();

            try
            {
                ushort[] data = _vepManager.SynchroZone.ToRegisters();
                //_modbusMaster.WriteMultipleRegisters(1, Addr_SynchroZone, data);   
                WriteMultiple(1, Addr_SynchroZone, data);
                OnSynchroZoneChanged();
                
            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        // Bench -> VEP에 응답 Write
        public void WriteReceptionZone()
        {
            CheckConnection();

            try
            {
                ushort[] data = _vepManager.ReceptionZone.ToRegisters();
                //_modbusMaster.WriteMultipleRegisters(1, Addr_ReceptionZone, data);
                WriteMultiple(1, Addr_ReceptionZone, data);
                OnReceptionZoneChanged();
                LogMessage($"수신 영역 쓰기: {string.Join(", ", data)}");
            }
            catch (Exception ex)
            {
                LogMessage($"수신 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteReceptionZone(ushort pAdd, ushort nVal)
        {
            CheckConnection();

            try
            {
                //_vepManager.SynchroZone.SetValue(pSyncro, nVal);
                ushort addr = (ushort)(Addr_ReceptionZone + pAdd);
                _modbusMaster.WriteSingleRegister(1, addr, nVal);
                OnReceptionZoneChanged();


            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }

        }

        public void WriteTransmissionZone()
        {
            CheckConnection();

            try
            {
                ushort[] registers = _vepManager.TransmissionZone.ToRegisters();
                //_modbusMaster.WriteMultipleRegisters(1, Addr_TransmissionZone, registers);
                WriteMultiple(1, Addr_TransmissionZone, registers);

                LogMessage($"전송 영역 쓰기 성공: " +
                  $"AddTSize={_vepManager.TransmissionZone.AddTzSize}, " +
                  $"ExchStatus={_vepManager.TransmissionZone.ExchStatus}, " +
                  $"FctCode={_vepManager.TransmissionZone.FctCode}, " +
                  $"PCNum={_vepManager.TransmissionZone.PCNum}, " +
                  $"DataSize={_vepManager.TransmissionZone.Data.Length}");

                OnTransmissionZoneChanged();

            }
            catch (Exception ex)
            {
                LogMessage($"전송 영역 쓰기 오류: {ex.Message}");
                throw;
            }
        }

        public void WriteTransmissionZone(ushort pAdd, ushort nVal)
        {
            CheckConnection();

            try
            {
                //_vepManager.SynchroZone.SetValue(pSyncro, nVal);
                ushort addr = (ushort)(Addr_TransmissionZone + pAdd);
                _modbusMaster.WriteSingleRegister(1, addr, nVal);
                OnTransmissionZoneChanged();


            }
            catch (Exception ex)
            {
                LogMessage($"동기화 영역 쓰기 오류: {ex.Message}");
                throw;
            }

        }

        // 연결 상태 확인
        private void CheckConnection()
        {
            if (!IsConnected)
            {
                LogMessage("Modbus 서버에 연결되어 있지 않습니다. 연결 시도 중...");
                Connect();
            }
        }

        protected virtual void OnDescriptionZoneRead()
        {
            _descriptionZoneRead?.Invoke(this, _vepManager.DescriptionZone);
        }

        protected virtual void OnStatusZoneChanged()
        {
            _statusZoneChanged?.Invoke(this, _vepManager.StatusZone);
        }

        protected virtual void OnSynchroZoneChanged()
        {
            _synchroZoneChanged?.Invoke(this, _vepManager.SynchroZone);
        }

        protected virtual void OnTransmissionZoneChanged()
        {
            _transmissionZoneChanged?.Invoke(this, _vepManager.TransmissionZone);
        }

        protected virtual void OnReceptionZoneChanged()
        {
            _receptionZoneChanged?.Invoke(this, _vepManager.ReceptionZone);
        }

        // 로깅
        private void LogMessage(string message)
        {
            Console.WriteLine($"[Modbus Client] {DateTime.Now:HH:mm:ss.fff} - {message}");
        }

        // Modbus 서버 테스트 - 단순히 연결 확인
        public bool TestConnection()
        {
            try
            {
                CheckConnection();
                // 단순히 레지스터 하나를 읽어서 연결이 잘 되는지 확인
                _modbusMaster.ReadHoldingRegisters(1, 0, 1);
                LogMessage("Modbus 서버 연결 테스트 성공");
                return true;
            }
            catch (Exception ex)
            {
                LogMessage($"Modbus 서버 연결 테스트 실패: {ex.Message}");
                return false;
            }
        }



        public void SetStartCycle()
        {
            _vepManager.StatusZone.StartCycle = 1;
            WriteStatusZone();
        }


        public void SetAllSyncZero()
        {
            _vepManager.SynchroZone.ResetAllValues();
            WriteSynchroZone();
        }

        public void SetSync30NewVehicle()
        {
            _vepManager.SynchroZone.SetValue(30, 1);
            WriteSynchroZone();
        }
        public void SetSync06RunOutFinish()
        {
            _vepManager.SynchroZone.SetValue(6, 1);
            WriteSynchroZone();
        }
        public void SetSync07SWBAngle(ushort shRes)
        {
            _vepManager.SynchroZone.SetValue(7, shRes);
            WriteSynchroZone();
        }
        public void SetSync10TAReady()
        {
            _vepManager.SynchroZone.SetValue(10, 1);
            WriteSynchroZone();
        }
        public void SetSync11TAAngle(ushort shRes)
        {
            _vepManager.SynchroZone.SetValue(11, shRes);
            WriteSynchroZone();
        }

        public void SetSync04SWB_Imform()
        {
            _vepManager.SynchroZone.SetValue(4, 1);
            WriteSynchroZone();
        }
        public void SetSync21SWBAngle(int nA, int nB, double dX)
        {
            ushort shRes = (ushort)((ushort)(nA * dX) + nB);

            _vepManager.SynchroZone.SetValue(21, shRes);
            WriteSynchroZone();
        }

        public void SetSync32HLAFinish()
        {
            _vepManager.SynchroZone.SetValue(32, 1);
            WriteSynchroZone();
        }

        public void SetSyncro(ushort pSync, ushort pVal)
        {
            _GV._VEP_Client.WriteSyncroZone(pSync, pVal);
        }

        public void SetTzEtat(ushort pVal)
        {
            //_vepManager.TransmissionZone.ExchStatus = pVal;
            WriteTransmissionZone(3, pVal);
        }
        public void SetRzEtat(ushort pVal)
        {
            //_vepManager.ReceptionZone.ExchStatus = pVal;
            WriteReceptionZone(3, pVal);
        }
        private int WriteMultiple(int nSlaveID, ushort Addr, ushort[] registers)
        {
            int nRet = 0;
            int nSize = registers.Length;

            int nLimit = 123;

            if (nSize < nLimit)
            {
                _modbusMaster.WriteMultipleRegisters(1, Addr, registers);
                return 1;
            }
            int nDoLoop = nSize / nLimit + 1;


            ushort nStartAddr = Addr;
            for (int i = 0; i < nDoLoop; i++)
            {
                int nPartPos = i * nLimit;

                // 마지막 구간일 경우 남은 데이터 수를 계산
                int nPartSize = Math.Min(nLimit, nSize - nPartPos);

                ushort[] part = new ushort[nPartSize];
                Array.Copy(registers, nPartPos, part, 0, nPartSize);

                // Modbus 전송
                _modbusMaster.WriteMultipleRegisters(1, (ushort)(Addr + nPartPos), part);

            }
            return nRet;
        }

        public void ReadAddTransmissionZone()
        {
            try
            {

                _vepManager.ReadTransmissionZonesFromRegisters(ReadAllRegisters);

                if (_vepManager.AddTransmissionZone.IsChanged)
                {
                    //OnAddTransmissionZoneChanged();
                    _vepManager.AddTransmissionZone.ResetChangedState();
                }
            }
            catch (Exception ex)
            {
                LogMessage($"ReadAddTransmissionZone 오류 발생: {ex.Message}");
            }
        }



    }
}
using Keyence.AutoID.SDK;

using System;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace Ki_WAT
{
    public class BarcodeReader
    {
        public class BarcodeEventArgs : EventArgs
        {
            public string BarcodeData { get; }

            public BarcodeEventArgs(string barcodeData)
            {
                BarcodeData = barcodeData;
            }
        }

        public  ReaderAccessor m_reader = new ReaderAccessor();
        private Thread _readThread;
        private volatile bool _run = false;

        public event EventHandler<BarcodeEventArgs> OnBarcodeReceived;
        public event EventHandler<Exception> OnError;
        private bool m_bIsConnect = false;

        public BarcodeReader(string ipAddress)
        {
            m_reader.IpAddress = ipAddress;
            
        }
        public bool IsConnected()
        {
            return m_bIsConnect;
        }

        public void ConnectBarcodeReader()
        {
             m_bIsConnect = m_reader.Connect();

            if (_readThread == null || !_readThread.IsAlive)
            {
                _run = true;
                _readThread = new Thread(ReadLoop);
                _readThread.IsBackground = true;
                _readThread.Start();

                Broker.dsBroker.Publish(Topics.DS.Barcode, Topics.DS.Connect);
            }
        }

        private void ReadLoop()
        {
            while (_run)
            {
                try
                {
                     string data = m_reader.ExecCommand("LON");
                    
                    if (!_run) break;

                    if (!string.IsNullOrEmpty(data))
                    {
                        OnBarcodeReceived?.Invoke(this, new BarcodeEventArgs(data));
                    }
                }
                catch (Exception ex)
                {
                    if (_run)
                    {
                        _run = false;
                        OnError?.Invoke(this, ex);
                    }
                }

                Thread.Sleep(500); 
            }
        }

        public void Disconnect()
        {
            _run = false;
            Thread.Sleep(300);
            string data = m_reader.ExecCommand("LOFF");

            m_reader.Disconnect();
            m_bIsConnect = false;
            
            if (_readThread != null && _readThread.IsAlive)
            {
                _readThread.Join(500);
            }
        }
    }
}

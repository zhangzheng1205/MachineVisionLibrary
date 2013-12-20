using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading;
using System.Windows.Forms;

namespace ComCommunicator
{
    class SerialPortManager : INotifyPropertyChanged
    {
        #region Events

        public event PropertyChangedEventHandler PropertyChanged;

        public delegate bool NotifyAnswerInBatchMode(string message);
        public delegate bool NotifyAnswerInNormalMode(string message);

        #endregion

        #region Private Fields

        bool _IsOpen = false;
        bool _IsBatchMode = false;
        bool _IsNormalTransfer = false;

        private NotifyAnswerInBatchMode _notifyBatch = null;
        private NotifyAnswerInNormalMode _notifynormalMode = null;
        private Form1 _parentForm = null;

        private string _messageReceived;

        public static readonly string[] BaudRates = new string[]{
            "300",
            "600",
            "1200",
            "2400",
            "4800",
            "9600",
            "14400",
            "28800",
            "36000",
            "115200"};

        private SerialPort _serialPort = new SerialPort();

        #endregion

        #region Properties

        public RichTextBox Display { set; get; }

        public string PortName
        {
            set
            {
                _serialPort.PortName = value;
            }

            get
            {
                return _serialPort.PortName;
            }
        }

        public bool SetNotifyForBatch(NotifyAnswerInBatchMode answer)
        {
            if (answer == null)
            {
                return false;
            }

            _notifyBatch = answer;
            return true;
        }

        public bool SetNotifyForNormalTransfer(NotifyAnswerInNormalMode answer)
        {
            if (answer == null)
            {
                return false;
            }

            _notifynormalMode = answer;
            return true;
        }

        public int BaudRate
        {
            set
            {
                _serialPort.BaudRate = value;
            }

            get
            {
                return _serialPort.BaudRate;
            }
        }

        public bool IsBatchMode
        {
            set { _IsBatchMode = value; }
            get { return _IsBatchMode; }
        }

        public bool IsNormalTransfer
        {
            set { _IsNormalTransfer = value; }
            get { return _IsNormalTransfer; }
        }

        public string Parity
        {
            set
            {
                switch (value)
                {
                    case "N":
                        _serialPort.Parity = System.IO.Ports.Parity.None;
                        break;

                    case "O":
                        _serialPort.Parity = System.IO.Ports.Parity.Odd;
                        break;

                    case "E":
                        _serialPort.Parity = System.IO.Ports.Parity.Even;
                        break;
                }
            }

            get
            {
                switch (_serialPort.Parity)
                {
                    case System.IO.Ports.Parity.Even:
                        return "E";

                    case System.IO.Ports.Parity.Odd:
                        return "O";

                    case System.IO.Ports.Parity.None:
                    default:
                        return "N";
                }
            }
        }

        public int DataBits
        {
            set
            {
                _serialPort.DataBits = value;
            }

            get
            {
                return _serialPort.DataBits;
            }
        }

        public int StopBits
        {
            set
            {
                switch (value)
                {
                    case 1:
                        _serialPort.StopBits = System.IO.Ports.StopBits.One;
                        break;

                    case 2:
                        _serialPort.StopBits = System.IO.Ports.StopBits.Two;
                        break;
                }
            }

            get
            {
                switch (_serialPort.StopBits)
                {
                    case System.IO.Ports.StopBits.One:
                        return 1;

                    case System.IO.Ports.StopBits.Two:
                        return 2;

                    default:
                        return 0;
                }
            }
        }


        public bool IsOpen
        {
            get
            {
                return _IsOpen;
            }

            private set
            {
                _IsOpen = value;
                OnPropertyChanged("IsOpen");
            }
        }

        #endregion

        #region Constructor

        public SerialPortManager(Form1 form)
        {
            BaudRate = 115200;
            PortName = "COM1";

            Parity = "N";
            DataBits = 8;
            StopBits = 1;

            _serialPort.DataReceived += _serialPort_DataReceived;
            _serialPort.ReadBufferSize = 2000000;
            _serialPort.WriteTimeout = 200;
            _parentForm = form;
        }

        ~SerialPortManager()
        {
            Close();
        }

        #endregion

        #region Private Methods

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            string msg = null;
            string sMessage = null;
            bool bTransfer = true;

            int bytesToRead = _serialPort.BytesToRead;
            byte[] byteBuffer = new byte[bytesToRead];

            _serialPort.Read(byteBuffer, 0, bytesToRead);

            msg = ASCIIEncoding.ASCII.GetString(byteBuffer);

            _messageReceived += msg;
            if ((_IsBatchMode == true) || (_IsNormalTransfer == true))
            {
                if (msg.IndexOf(';') < 0)
                {
                    bTransfer = false;
                }
            }
            else
            {
                _messageReceived = "";
            }

            sMessage = _messageReceived;

            msg = msg.Replace("\r", "\n").Replace("\n", Environment.NewLine);

            DisplayString(msg, Color.Black);

            if (bTransfer == true)
            {
                // Notify answer
                if ((_IsBatchMode == true) && (this._notifyBatch != null))
                {
                    _parentForm.Invoke(this._notifyBatch, (object)(sMessage));
                }

                if ((_IsNormalTransfer == true) && (this._notifynormalMode != null))
                {
                    _parentForm.Invoke(this._notifynormalMode, (object)sMessage);
                }

                _messageReceived = "";
            }
        }

        private void DisplayString(string newMsg, Color color)
        {
            String errorMsg = string.Empty;

            if (Display != null)
            {
                Display.Invoke(new EventHandler(delegate
                {
                    Display.SelectionColor = color;
                    Display.AppendText(newMsg + errorMsg);

                    Display.ScrollToCaret();
                }));
            }
        }

        #endregion

        #region Event Handlers

        protected void OnPropertyChanged(PropertyChangedEventArgs e)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null)
                handler(this, e);
        }

        protected void OnPropertyChanged(string propertyName)
        {
            OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
        }

        #endregion

        #region Public Interface

        public void Open()
        {
            if (_serialPort.IsOpen == false)
            {
                try
                {
                    _serialPort.Open();
                    IsOpen = true;
                }
                catch (Exception excp)
                {
                    MessageBox.Show(excp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        public void Close()
        {
            if (_serialPort.IsOpen == true)
            {
                _serialPort.Close();
                IsOpen = false;
            }
        }

        public void Write(string text)
        {
            _serialPort.Write(text);

            text += "\r";
            DisplayString(text + "\n", Color.Green);
        }

        #endregion
    }
}

using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Timers;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO.Pipes;
using System.Globalization;
using System.Reflection;
using System.Runtime.InteropServices;
using ComCommunicator.Properties;
using System.Diagnostics;

namespace ComCommunicator
{
    #region MVL definitions

    public enum MVL_RETURN
    {
        MVL_SUCCESS = 0,
        MVL_FAILURE
    };

    public enum MVL_ERROR_CODE
    {
        MVL_ERROR_UNDEFINED = -1,
        MVL_ERROR_NOT_PRESENT = 0,
        MVL_ERROR_PROPERTY_NOT_PRESENT = 1,
        MVL_ERROR_ANALIZER_WORK = 2
    };

    public enum MVL_VALUE_TYPE
    {
        MVL_VALUE_ENABLED = 0,
        MVL_VALUE_DISABLED = 1
    };

    public enum MVL_PROPERTY
    {
        MVL_UNDEFINED_PROPERTY = -1,

        MVL_SELECT_ANALYSIS,
        MVL_VERSION_ID,
        MVL_PROPERTY_IMAGE_ADDRESS,
        MVL_PROPERTY_GET_IMAGE_WIDTH,
	    MVL_PROPERTY_GET_IMAGE_HEIGHT,

        // Result properties
        MVL_PROPERTY_RESULT_ANALIZER_ID,
        MVL_PROPERTY_RESULT_ANALIZER_NUM_POINTS,
        MVL_PROPERTY_RESULT_ANALIZER_NUM_BYTES,
        MVL_PROPERTY_RESULT_ANALIZER_DATA,

        MVL_NUMBER_OF_PROPERTY
    };

    public enum MVL_ANALYSIS
    {
        MVL_UNDEFINED_ANALISYS = -1,

        MVL_CANNY_ANALYSIS = 0,
        MVL_GRADIENTE_ANALYSIS,
	    MVL_SIFT_ANALYSIS,

        MVL_NUMBER_OF_ANALISYS
    };

    #endregion


    public partial class Form1 : Form
    {
        private SerialPortManager _serialManager = null;
        private string[] _portNames;
        private delegate void WriteToSerialPort(string text);
        private delegate void TransferFinishedDelegate(bool bValue);
        private delegate void BatchUpdateCommandsTxDelegate(int[] nCommands);

        #region MVL_Library_Function_Handles

        private IntPtr _dllLibrary = IntPtr.Zero;
        private IntPtr _createMVL = IntPtr.Zero;
        private IntPtr _destroyMVL = IntPtr.Zero;
        private IntPtr _setParameterMVL = IntPtr.Zero;
        private IntPtr _getParameterMVL = IntPtr.Zero;
        private IntPtr _analyzeMVL = IntPtr.Zero;
        private IntPtr _getLastErrorMVL = IntPtr.Zero;
        private string _edgeSelected;
        private bool _IsLibraryCreated = false;

        #endregion

        #region Batch Parameters

        private CreateBatch _batchForm = null;
        private static System.Timers.Timer _batchAnswerTimer;

        // For batch transmission
        private Thread _batchThread = null;
        private EventWaitHandle[] _hadleBatch = { new EventWaitHandle(false, EventResetMode.AutoReset), 
                                                  new EventWaitHandle(false, EventResetMode.AutoReset),
                                                  new EventWaitHandle(false, EventResetMode.AutoReset),
                                                  new EventWaitHandle(false, EventResetMode.AutoReset),
                                                  new EventWaitHandle(false, EventResetMode.AutoReset)};
        private string[] _commandBatch = null;
        private Queue _batchResponseQueue = Queue.Synchronized(new Queue());

        private enum EBATCH_EVENT
        {
            START_EVENT = 0,
            RESPONSE_RECEIVED_EVENT = 1,
            TIMER_ELAPSED_EVENT = 2,
            PROCEED_NEXT_COMMAND = 3,
            END_TRANSFER = 4
        };

        #endregion

        #region Parameters for normal commands

        private CNC_Parameters _CNCParameters = null;
        private static System.Timers.Timer _normalTransferAnswerTimer;

        // For batch transmission
        private Thread _NormalTransferThread = null;
        private EventWaitHandle[] _hadleNormalTransfer = { new EventWaitHandle(false, EventResetMode.AutoReset), 
                                                           new EventWaitHandle(false, EventResetMode.AutoReset),
                                                           new EventWaitHandle(false, EventResetMode.AutoReset),
                                                           new EventWaitHandle(false, EventResetMode.AutoReset)};
        private string[] _commandNormalTransfer = null;
        private ArrayList _responseToNormalCommands = new ArrayList();
        private string[] _responseString = null;
        private Queue _normalTransferResponseQueue = Queue.Synchronized(new Queue());
        private EndNormalTransferDelegate _currentClientForNormalTransfer = null;

        private enum ENORMAL_COMMANDS
        {
            START_SEQUENCE  = 0,
            RESPONSE_RECEIVED   = 1,
            TIMER_ELAPSED_EVENT = 2,
            END_TRANSFER    =  3
        };

        #endregion

        #region MVL delegate definitions

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_RETURN Create(IntPtr pImage);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_RETURN Destroy();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_RETURN SetProperty(int nPropertyID, IntPtr pData, IntPtr pData2, IntPtr pData3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_RETURN GetProperty(int nPropertyID, IntPtr pData, IntPtr pData2, IntPtr pData3);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_RETURN Analyze();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        private delegate MVL_ERROR_CODE GetLastError();

        #endregion

        #region MVL functions pointer

        private Create MVL_Create = null;

        private Destroy MVL_Destroy = null;

        private SetProperty MVL_SetProperty = null;

        private GetProperty MVL_GetProperty = null;

        private Analyze MVL_Analyze = null;

        private GetLastError MVL_GetLastError = null;

        #endregion

        private static readonly FieldInfo[] MVLFunctions = Type.GetType("ComCommunicator.Form1")
                                            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
                                            .Where(t => t.Name.StartsWith("MVL_")).ToArray();

        private int _nXMaxVal = -1;
        private int _nYMaxVal = -1;
        private int _nZMaxVal = -1;

        private MVL_ANALYSIS _eCurrentAnalysis = MVL_ANALYSIS.MVL_UNDEFINED_ANALISYS;
        private int _nCurrentImageWidth = -1;
        private int _nCurrentImageHeight = -1;

        public Form1()
        {
            InitializeComponent();

            _serialManager = new SerialPortManager(this);

            button3.Enabled = false;
            _IsLibraryCreated = false;

            // Create thread used to send batch commands
            _batchThread = new Thread(new ThreadStart(Thread_SendBatchCommands));
            _batchThread.Name = "Batch Thread";
            _batchThread.Start();

            // Create thread used for normal transfer
            _NormalTransferThread = new Thread(new ThreadStart(ThreadSendNormalCommands));
            _NormalTransferThread.Name = "Normal Transfer Thread";
            _NormalTransferThread.Start();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            _portNames = SerialPort.GetPortNames();
            Array.Sort<string>(_portNames);
            comboPort.Items.AddRange(_portNames);
            comboPort.SelectedIndex = 0;

            comboBaud.Items.AddRange(SerialPortManager.BaudRates);
            comboBaud.SelectedIndex = SerialPortManager.BaudRates.Length - 1;

            comboParity.SelectedIndex = comboParity.FindStringExact("N");
            comboDataBits.SelectedIndex = comboDataBits.FindStringExact("8");
            comboStopBits.SelectedIndex = comboStopBits.FindStringExact("1");

            _serialManager.Display = textReceive;
            _serialManager.BaudRate = int.Parse(comboBaud.Text);
            _serialManager.PortName = comboPort.Text;
            _serialManager.PropertyChanged += serialManager_PropertyChanged;
            _serialManager.SetNotifyForBatch(AnswerReceivedInBatch);
            _serialManager.SetNotifyForNormalTransfer(AnswerReceivedInNormalMode);
        }

        public bool SaveEdgeSelected(ref string edge, MVL_ANALYSIS nPropertyValue)
        {
            _edgeSelected = edge;
            this.Enabled = true;

            if (_IsLibraryCreated == true)
            {
               // Set property to library
                unsafe
                {
                    MVL_VALUE_TYPE value = MVL_VALUE_TYPE.MVL_VALUE_ENABLED;
                    IntPtr pAdddress = (IntPtr)(&nPropertyValue);
                    IntPtr pValueType = (IntPtr)(&value);
                    CheckMVLReturnValue( this.MVL_SetProperty((int)MVL_PROPERTY.MVL_SELECT_ANALYSIS, pAdddress, pValueType, IntPtr.Zero) );

                    // Save current analysis
                    _eCurrentAnalysis = nPropertyValue;
                }
            }
            return true;
        }

        public bool SetMVLEdgeSelection(MVL_ANALYSIS nPropertyValue, MVL_VALUE_TYPE eValueType)
        {
            if (_IsLibraryCreated == true)
            {
                // Set property to library
                unsafe
                {
                    MVL_VALUE_TYPE value = eValueType;
                    IntPtr pAdddress = (IntPtr)(&nPropertyValue);
                    IntPtr pValueType = (IntPtr)(&value);
                    CheckMVLReturnValue( this.MVL_SetProperty((int)MVL_PROPERTY.MVL_SELECT_ANALYSIS, pAdddress, pValueType, IntPtr.Zero) );
                }
            }
            return true;
        }

        public void CheckMVLReturnValue(MVL_RETURN returnType)
        {
            switch (returnType)
            {
                case MVL_RETURN.MVL_FAILURE:

                    MessageBox.Show("Failure setting properties to MVL", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;

                default:
                    break;
            }
        }

        public int X_Max_Val
        {
            set { _nXMaxVal = value; }
            get { return _nXMaxVal; }
        }

        public int Y_Max_Val
        {
            set { _nYMaxVal = value; }
            get { return _nYMaxVal; }
        }

        public int Z_Max_Val
        {
            set { _nZMaxVal = value; }
            get { return _nZMaxVal; }
        }

        public bool AnswerReceivedInBatch(string message)
        {
            string tempMessage;
            int nVal = 0;

            // pre process string, remove empty characters
            tempMessage = message;

            nVal = message.IndexOf(';');
            while (nVal > -1)
            {
                if ((message.IndexOf('@') != -1) && (message.IndexOf(';') != -1))
                {
                    tempMessage = message.Substring(message.IndexOf('@'), message.IndexOf(';') - message.IndexOf('@') + 1);

                    _batchResponseQueue.Enqueue(tempMessage);

                    if (message.IndexOf(';') + 2 < message.Length)
                    {
                        message = message.Substring(message.IndexOf(';') + 1, message.Length - message.IndexOf(';') - 1);
                        nVal = message.IndexOf(';');
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Signal event
            _hadleBatch[(int)EBATCH_EVENT.RESPONSE_RECEIVED_EVENT].Set();

            return true;
        }

        public bool SendBatchCommands(string[] sCommands)
        {
            if (sCommands.Length == 0)
            {
                MessageBox.Show("No command present in list", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // End of transfer
                BatchEnd(false);

                return false;
            }

            if (_serialManager.IsOpen == false)
            {
                MessageBox.Show("Port closed - Open port before writing to device", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // End of transfer
                BatchEnd(false);

                return false;
            }

            // Save command list
            _commandBatch = sCommands;

            // Set serial manager in batch mode
            _serialManager.IsBatchMode = true;

            // Waitinf for thread is up
            while (!_batchThread.IsAlive) ;

            // Signal to start batch
            _hadleBatch[(int)EBATCH_EVENT.START_EVENT].Set();
            return true;
        }

        private void Thread_SendBatchCommands()
        {
            int nIndex = 0;
            int nCommandIdx = 0;
            int nNumcommands = 0;
            WriteToSerialPort delegateWrite = _serialManager.Write;
            TransferFinishedDelegate delegateEndBatch = this.BatchEnd;
            BatchUpdateCommandsTxDelegate delegateUpdateCommandTx = this.SetCommandsTransferred;
            int[] Commands = new int[] { nCommandIdx, nNumcommands };

            while (true)
            {
                nIndex = EventWaitHandle.WaitAny(_hadleBatch);

                switch (nIndex)
                {
                    case (int)EBATCH_EVENT.START_EVENT:

                        // Disable commands
                        buttonSend.Enabled = false;
                        buttonClear.Enabled = false;

                        // Initialize timer for transmission
                        _batchAnswerTimer = new System.Timers.Timer(200000);
                        _batchAnswerTimer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
                        _batchAnswerTimer.Interval = 200000;
                        // Prevent garbage collector to remove timer in a long time preocess
                        GC.KeepAlive(_batchAnswerTimer);
                        _batchAnswerTimer.Stop();

                        // Save number of commands
                        nNumcommands = _commandBatch.Length;

                        // Current index command
                        nCommandIdx = 0;

                        // Used to skip null commands
                        while ((nCommandIdx < nNumcommands) && (String.IsNullOrEmpty(_commandBatch[nCommandIdx]) == true))
                        {
                            ++nCommandIdx;
                        }

                        if (nCommandIdx < nNumcommands)
                        {
                            // Start Timer
                            _batchAnswerTimer.Enabled = true;
                            _batchAnswerTimer.Start();

                            // Write command
                            this.Invoke(delegateWrite, _commandBatch[nCommandIdx]);
                        }
                        else
                        {
                            // Signal to end transfer
                            _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();
                        }
                        break;

                    case (int)EBATCH_EVENT.PROCEED_NEXT_COMMAND:

                        // Increment command
                        ++nCommandIdx;

                        // Used to skip null commands
                        while ((nCommandIdx < nNumcommands) && (String.IsNullOrEmpty(_commandBatch[nCommandIdx]) == true))
                        {
                            ++nCommandIdx;
                        }

                        if (nCommandIdx < nNumcommands)
                        {
                            Thread.Sleep(30);

                            // Start Timer
                            _batchAnswerTimer.Enabled = true;
                            _batchAnswerTimer.Start();

                            // Write command
                            this.Invoke(delegateWrite, _commandBatch[nCommandIdx]);
                        }
                        else
                        {
                            // Signal to end transfer
                            _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();
                        }
                        break;

                    case (int)EBATCH_EVENT.RESPONSE_RECEIVED_EVENT:

                        string message;

                        // Get element from queue
                        while (_batchResponseQueue.Count > 0)
                        {
                            message = (string)_batchResponseQueue.Dequeue();

                            // Process response

                            // Command right
                            if (String.Equals(message, "@OK;") == true)
                            {
                                // OK answer
                                Commands[0] = nCommandIdx + 1;
                                Commands[1] = nNumcommands;
                                this.Invoke(delegateUpdateCommandTx, Commands);
                            }
                            // Current point ended to process
                            else if (String.Equals(message, "@NEXT;") == true)
                            {
                                // Proceed with next command
                                _hadleBatch[(int)EBATCH_EVENT.PROCEED_NEXT_COMMAND].Set();
                            }
                            // Command send was wrong
                            else if (String.Equals(message, "@KO;") == true)
                            {
                                MessageBox.Show("Error in answer by CNC", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                // Signal to end transfer
                                _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();
                            }
                            // For others answers
                            else
                            {
                                MessageBox.Show("Error in answer by CNC", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                // Signal to end transfer
                                _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();
                            }
                            
                        }

                        break;

                    case (int)EBATCH_EVENT.TIMER_ELAPSED_EVENT:

                        MessageBox.Show("Timer elapsed waiting for answer", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);

                        // Signal to end transfer
                        _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();
                        break;

                    case (int)EBATCH_EVENT.END_TRANSFER:

                        // Stop timer
                        _batchAnswerTimer.Stop();
                        _batchAnswerTimer.Enabled = false;
                        _batchAnswerTimer.Dispose();

                        this.Invoke(delegateEndBatch, true);
                        break;

                    default:
                        // abort thread
                        break;
                }
            }
        }

        private void SetCommandsTransferred(int[] nCommands)
        {
            _batchForm.UpdateCommandsTransfered(nCommands[0], nCommands[1]);
        }

        private void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            // Signal to end transfer
            _hadleBatch[(int)EBATCH_EVENT.END_TRANSFER].Set();

            // Stop timer
            _batchAnswerTimer.Stop();
            _batchAnswerTimer.Enabled = false;
        }

        private void BatchEnd(bool bValue)
        {
            // Reset command list
            _commandBatch = null;

            // Exit serial manager from batch mode
            _serialManager.IsBatchMode = false;

            // End of batch
            _batchForm.BatchTerminated(bValue);

            // Enable commands
            buttonSend.Enabled = true;
            buttonClear.Enabled = true;
        }



        public bool AnswerReceivedInNormalMode(string message)
        {
            string tempMessage;
            int nVal = 0;

            // pre process string, remove empty characters
            tempMessage = message;

            nVal = message.IndexOf(';');
            while (nVal > -1)
            {
                if ((message.IndexOf('@') != -1) && (message.IndexOf(';') != -1))
                {
                    tempMessage = message.Substring(message.IndexOf('@'), message.IndexOf(';') - message.IndexOf('@') + 1);

                    _normalTransferResponseQueue.Enqueue(tempMessage);

                    if (message.IndexOf(';') + 2 < message.Length)
                    {
                        message = message.Substring(message.IndexOf(';') + 1, message.Length - message.IndexOf(';') - 1);
                        nVal = message.IndexOf(';');
                    }
                    else
                    {
                        break;
                    }
                }
            }

            // Signal event
            _hadleNormalTransfer[(int)ENORMAL_COMMANDS.RESPONSE_RECEIVED].Set();

            return true;
        }

        public bool SendNormalCommands(string[] sCommands, string[] sResponse, EndNormalTransferDelegate func)
        {
            // Save delegate
            _currentClientForNormalTransfer = func;

            // Save response
            _responseString = sResponse;

            if (sCommands.Length == 0)
            {
                MessageBox.Show("No command present in list", "Error",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);

                // End of transfer
                EndNormalTransfer(false);

                return false;
            }

            if (_serialManager.IsOpen == false)
            {
                MessageBox.Show("Port closed - Open port before writing to device", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);

                // End of transfer
                EndNormalTransfer(false);

                return false;
            }

            // Save command list
            _commandNormalTransfer = sCommands;

            // Set serial manager in batch mode
            _serialManager.IsNormalTransfer = true;

            // Waiting for thread is up
            while (!_NormalTransferThread.IsAlive) ;

            // Signal to start batch
            _hadleNormalTransfer[(int)ENORMAL_COMMANDS.START_SEQUENCE].Set();
            return true;
        }

        void ThreadSendNormalCommands()
        {
            int nIndex = 0;
            int nCommandIdx = 0;
            int nNumcommands = 0;
            WriteToSerialPort delegateWrite = _serialManager.Write;
            TransferFinishedDelegate delegateEndTx = this.EndNormalTransfer;
            bool bErrorOccurred = false;

            while (true)
            {
                nIndex = EventWaitHandle.WaitAny(_hadleNormalTransfer);

                switch (nIndex)
                {
                    case (int)ENORMAL_COMMANDS.START_SEQUENCE:

                        // Disable commands
                        Invoke(new Action(() => buttonSend.Enabled = false));
                        Invoke(new Action(() => buttonClear.Enabled = false));

                        // Initialize timer for transmission
                        _normalTransferAnswerTimer = new System.Timers.Timer(90000);
                        _normalTransferAnswerTimer.Elapsed += new ElapsedEventHandler(OnTimedNormalEvent);
                        _normalTransferAnswerTimer.Stop();

                        // Save number of commands
                        nNumcommands = _commandNormalTransfer.Length;

                        // Current index command
                        nCommandIdx = 0;

                        // Used to skip null commands
                        while ((nCommandIdx < nNumcommands) && (String.IsNullOrEmpty(_commandNormalTransfer[nCommandIdx]) == true))
                        {
                            ++nCommandIdx;
                        }

                        if (nCommandIdx < nNumcommands)
                        {
                            // Start Timer
                            _normalTransferAnswerTimer.Enabled = true;
                            _normalTransferAnswerTimer.Start();

                            // Write command
                            this.Invoke(delegateWrite, _commandNormalTransfer[nCommandIdx]);
                        }
                        else
                        {
                            // Signal to end transfer
                            _hadleNormalTransfer[(int)ENORMAL_COMMANDS.END_TRANSFER].Set();
                        }
                        break;

                    case (int)ENORMAL_COMMANDS.RESPONSE_RECEIVED:

                        string message;

                        // Get element from queue
                        while (_normalTransferResponseQueue.Count > 0)
                        {
                            message = (string)_normalTransferResponseQueue.Dequeue();

                            // Process response
                            bool bProceed = true;

                            _normalTransferAnswerTimer.Stop();

                            // Command right
                            if (String.Equals(message, "@OK;") == true)
                            {
                                // OK answer

                                // Save response
                                _responseToNormalCommands.Add(message);
                            }
                            // Command send was wrong
                            else if (String.Equals(message, "@KO;") == true)
                            {
                                MessageBox.Show("Error in answer by CNC", "Warning",
                                MessageBoxButtons.OK, MessageBoxIcon.Warning);

                                bProceed = false;

                                bErrorOccurred = true;
                            }
                            // For others answers
                            else
                            {
                                // Save response
                                _responseToNormalCommands.Add(message);
                            }

                            if (bProceed == false)
                            {
                                // Signal to end transfer
                                _hadleNormalTransfer[(int)ENORMAL_COMMANDS.END_TRANSFER].Set();
                            }
                            else
                            {
                                // Increment command
                                ++nCommandIdx;

                                // Used to skip null commands
                                while ((nCommandIdx < nNumcommands) && (String.IsNullOrEmpty(_commandNormalTransfer[nCommandIdx]) == true))
                                {
                                    ++nCommandIdx;
                                }

                                // Continue to transfer
                                if (nCommandIdx < nNumcommands)
                                {
                                    // Start Timer
                                    _normalTransferAnswerTimer.Enabled = true;
                                    _normalTransferAnswerTimer.Start();

                                    Thread.Sleep(20);

                                    // Write command
                                    this.Invoke(delegateWrite, _commandNormalTransfer[nCommandIdx]);
                                }
                                else
                                {
                                    // Signal to end transfer
                                    _hadleNormalTransfer[(int)ENORMAL_COMMANDS.END_TRANSFER].Set();
                                }
                            }

                        }
                        break;

                    case (int)ENORMAL_COMMANDS.TIMER_ELAPSED_EVENT:

                        _normalTransferAnswerTimer.Stop();

                        MessageBox.Show("Timer elapsed waiting for answer", "Error",
                                  MessageBoxButtons.OK, MessageBoxIcon.Error);

                        bErrorOccurred = true;

                        // Signal to end transfer
                        _hadleNormalTransfer[(int)ENORMAL_COMMANDS.END_TRANSFER].Set();
                        break;

                    case (int)ENORMAL_COMMANDS.END_TRANSFER:

                        // Stop timer
                        _normalTransferAnswerTimer.Stop();
                        _normalTransferAnswerTimer.Enabled = false;
                        _normalTransferAnswerTimer.Dispose();

                        this.Invoke(delegateEndTx, !bErrorOccurred);
                        bErrorOccurred = false;
                        break;

                    default:
                        // abort thread
                        break;
                }
            }
        }

        private void EndNormalTransfer(bool bValue)
        {
            // Reset command list
            _commandNormalTransfer = null;

            // Exit serial manager from batch mode
            _serialManager.IsNormalTransfer = false;

            // Save response vector
            _responseString = (string[])_responseToNormalCommands.ToArray(typeof(string));

            // End of normal transfer
            _currentClientForNormalTransfer(bValue, ref _responseString);
            
            // Clear response list
            _responseToNormalCommands.Clear();
            _responseString = null;
            
            // Enable commands
            buttonSend.Enabled = true;
            buttonClear.Enabled = true;
        }

        private void OnTimedNormalEvent(object source, ElapsedEventArgs e)
        {
            // Signal to end transfer
            _hadleNormalTransfer[(int)ENORMAL_COMMANDS.TIMER_ELAPSED_EVENT].Set();
            
            // Stop timer
            _normalTransferAnswerTimer.Stop();
            _normalTransferAnswerTimer.Enabled = false;
        }

        private void serialManager_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            switch (e.PropertyName)
            {
                case "IsOpen":
                    buttonOpen.Text = _serialManager.IsOpen ? "Close Port" : "Open Port";

                    buttonOpen.Enabled = true;
                    comboPort.Enabled = !comboPort.Enabled;
                    break;
            }
        }

        private void comboPort_SelectedIndexChanged(object sender, EventArgs e)
        {
            _serialManager.PortName = comboPort.Text;
        }

        private void comboBaud_SelectedIndexChanged(object sender, EventArgs e)
        {
            _serialManager.BaudRate = int.Parse(comboBaud.Text);
        }

        private void comboParity_SelectedIndexChanged(object sender, EventArgs e)
        {
            _serialManager.Parity = comboParity.Text;
        }

        private void comboDataBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            _serialManager.DataBits = int.Parse(comboDataBits.Text);
        }

        private void comboStopBits_SelectedIndexChanged(object sender, EventArgs e)
        {
            _serialManager.StopBits = int.Parse(comboStopBits.Text);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (String.IsNullOrEmpty(textBox1.Text))
            {
                return;
            }

            try
            {
                _serialManager.Write(textBox1.Text);
            }
            catch (InvalidOperationException)
            {
                MessageBox.Show("Port closed - Open port before writing to device", "Warning",
                    MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }

            textBox1.Clear();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            if (_serialManager.IsOpen == false)
            {
                _serialManager.Open();
                textBox1.Focus();
            }
            else
            {
                _serialManager.Close();
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            textReceive.Clear();
            textReceive.Invalidate();
            textReceive.Update();
        }

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void textBox1_KeyPress(object sender, KeyPressEventArgs e)
        {
            if (e.KeyChar == '\r')
            {
                buttonSend.PerformClick();
            }
        }

        private void label6_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            if (textBox2.TextLength > 0)
            {
                DialogResult result = System.Windows.Forms.DialogResult.Retry;
                while ((_dllLibrary == IntPtr.Zero) && (result == System.Windows.Forms.DialogResult.Retry))
                {
                    _dllLibrary = DLLImporter.LoadLibrary(textBox2.Text);
                    if (_dllLibrary != IntPtr.Zero)
                    {
                        // Load function address
                        
                        _createMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_Create");
                        _destroyMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_Destroy");
                        _setParameterMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_SetProperty");
                        _getParameterMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_GetProperty");
                        _analyzeMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_Analyze");
                        _getLastErrorMVL = DLLImporter.GetProcAddress(_dllLibrary, "MVL_GetLastError");

                        if ((_createMVL != IntPtr.Zero) && (_destroyMVL != IntPtr.Zero) &&
                            (_setParameterMVL != IntPtr.Zero) && (_getParameterMVL != IntPtr.Zero) &&
                            (_analyzeMVL != IntPtr.Zero))
                        {
                            CreateLibrary();
                        }
                        else
                        {
                            result = MessageBox.Show("Error occurred", "Retry",
                                 MessageBoxButtons.RetryCancel);

                            if (result == System.Windows.Forms.DialogResult.Retry)
                            {
                                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                                {
                                    textBox2.Clear();
                                    textBox2.AppendText(openFileDialog1.FileName);
                                }
                            }
                            else
                            {
                                textBox2.Clear();
                            }
                        }
                    }
                    else
                    {
                        result = MessageBox.Show("Error occurred", "Retry",
                             MessageBoxButtons.RetryCancel);

                        if (result == System.Windows.Forms.DialogResult.Retry)
                        {
                            if (openFileDialog1.ShowDialog() == DialogResult.OK)
                            {
                                textBox2.Clear();
                                textBox2.AppendText(openFileDialog1.FileName);
                            }
                        }
                        else
                        {
                            textBox2.Clear();
                        }
                    }
                }
            }
        }

        private void button2_Click_1(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                textBox2.Clear();
                textBox2.AppendText(openFileDialog1.FileName);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            if (_dllLibrary != IntPtr.Zero)
            {
                if (_IsLibraryCreated == true)
                {
                    DestroyLibrary();
                }

                DLLImporter.FreeLibrary(_dllLibrary);
                textBox2.Clear();
                textBox2.ReadOnly = false;
                button1.Enabled = true;
                button3.Enabled = false;
                _dllLibrary = IntPtr.Zero;
                label8.Visible = false;

                textBox3.Clear();
            }
            else
            {
                MessageBox.Show("No library loaded!", "Warning", MessageBoxButtons.OK);
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            if (openFileDialog2.ShowDialog() == DialogResult.OK)
            {
                textBox3.Clear();
                textBox3.AppendText(openFileDialog2.FileName);

                if (_IsLibraryCreated == true)
                {
                    IntPtr temp = Marshal.StringToHGlobalAnsi(openFileDialog2.FileName);

                    unsafe
                    {
                        // Every time an image is changed, send new information to library
                        CheckMVLReturnValue(this.MVL_SetProperty((int)MVL_PROPERTY.MVL_PROPERTY_IMAGE_ADDRESS, temp, IntPtr.Zero, IntPtr.Zero));
                    }
                }
            }
        }

        private void edgeDetectionToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            SelectEdgeDetection form = new SelectEdgeDetection(this, ref _edgeSelected);
            form.Text = "Select edge analysis";
            form.Focus();
            form.Show();
        }

        private void button5_Click(object sender, EventArgs e)
        {
            if (textBox3.TextLength > 0)
            {
                this.Enabled = false;
                ImageViewer form = new ImageViewer(this, textBox3.Text);
                form.Text = textBox3.Text;
                form.Focus();
                form.Show();
                form.ShowImage();
            }
        }

        private bool CreateLibrary()
        {
            MessageBox.Show("Library loaded succesfully");
            DialogResult result = System.Windows.Forms.DialogResult.Cancel;
            
            textBox2.ReadOnly = true;
            button1.Enabled = false;
            button3.Enabled = true;

            // Load function address
            MVLFunctions.LoadFunctions(this._dllLibrary, this);
            
            // Convert image pointer
            IntPtr temp = IntPtr.Zero;
            if (String.IsNullOrEmpty(textBox3.Text) == true)
            {
                temp = IntPtr.Zero;
            }
            else
            {
                temp = Marshal.StringToHGlobalAnsi(textBox3.Text);
            }

            if (this.MVL_Create != null)
            {
                // Create library
                if (this.MVL_Create(temp) == MVL_RETURN.MVL_SUCCESS)
                {
                    _IsLibraryCreated = true;

                    // Get library ID
                    string sLibraryID;
                    IntPtr ptr = Marshal.AllocHGlobal(50);
                    this.MVL_GetProperty((int)MVL_PROPERTY.MVL_VERSION_ID, ptr, IntPtr.Zero, IntPtr.Zero);
                    sLibraryID = Marshal.PtrToStringAnsi(ptr);
                    Marshal.FreeHGlobal(ptr);

                    label8.Visible = true;
                    label8.Text = sLibraryID;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private bool DestroyLibrary()
        {
            if (this.MVL_Destroy != null)
            {
                // Destroy library
                if (this.MVL_Destroy() == MVL_RETURN.MVL_SUCCESS)
                {
                    _IsLibraryCreated = false;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                return false;
            }

            return true;
        }

        private void AnalyzeButton_Click(object sender, EventArgs e)
        {
            if (_IsLibraryCreated == true)
            {
                CheckMVLReturnValue( this.MVL_Analyze() );

                // Get image parameters
                unsafe
                {
                    int nVal = 0;
                    _nCurrentImageWidth = -1;
                    _nCurrentImageHeight = -1;

                    // Get image width
                    IntPtr ptr = IntPtr.Zero;
                    ptr = (IntPtr)(&nVal);
                    this.MVL_GetProperty((int)MVL_PROPERTY.MVL_PROPERTY_GET_IMAGE_WIDTH, ptr, IntPtr.Zero, IntPtr.Zero);
                    _nCurrentImageWidth = nVal;

                    // Get image height
                    ptr = (IntPtr)(&nVal);
                    this.MVL_GetProperty((int)MVL_PROPERTY.MVL_PROPERTY_GET_IMAGE_HEIGHT, ptr, IntPtr.Zero, IntPtr.Zero);
                    _nCurrentImageHeight = nVal;
                }
            }
        }

        private void GetMVLResults(MVL_ANALYSIS eTypeAnalysis, out int numPoints, out IntPtr pData)
        {
            unsafe
            {
                int nVal = (int)eTypeAnalysis;
                IntPtr ptr = (IntPtr)(&nVal);

                // Select analyzer ID
                this.MVL_SetProperty((int)MVL_PROPERTY.MVL_PROPERTY_RESULT_ANALIZER_ID, ptr, IntPtr.Zero, IntPtr.Zero);

                // Get number of bytes
                int nNumBytes = 0;
                IntPtr ptrNumBytes = (IntPtr)(&nNumBytes);
                this.MVL_GetProperty((int)MVL_PROPERTY.MVL_PROPERTY_RESULT_ANALIZER_NUM_BYTES, ptrNumBytes, IntPtr.Zero, IntPtr.Zero);

                // Get number of points
                int nNumPoints = 0;
                IntPtr ptrNumPoints = (IntPtr)(&nNumPoints);
                this.MVL_GetProperty((int)MVL_PROPERTY.MVL_PROPERTY_RESULT_ANALIZER_NUM_POINTS, ptrNumPoints, IntPtr.Zero, IntPtr.Zero);
                numPoints = nNumPoints;

                // Get results
                IntPtr ptrResults = Marshal.AllocHGlobal(nNumBytes);
                this.MVL_GetProperty((int)MVL_PROPERTY.MVL_PROPERTY_RESULT_ANALIZER_DATA, ptrResults, IntPtr.Zero, IntPtr.Zero);
                pData = ptrResults;               
            }
        }

        private void Form1_Deactivate(object sender, EventArgs e)
        {
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_IsLibraryCreated == true)
            {
                DestroyLibrary();
                DLLImporter.FreeLibrary(_dllLibrary);
            }

            try
            {
                foreach (Process proc in Process.GetProcessesByName("ComCommunicator"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        
        private void newToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _batchForm = new CreateBatch(this);
            this.Enabled = false;
            _batchForm.Text = "Create Batch";
            _batchForm.Focus();
            _batchForm.Show();
        }

        private void loadToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFile = new OpenFileDialog();
            if (openFile.ShowDialog() == DialogResult.OK)
            {
                string[] lines = System.IO.File.ReadAllLines(openFile.FileName);

                _batchForm = new CreateBatch(this, lines);
                this.Enabled = false;
                _batchForm.Text = "Create Batch";
                _batchForm.Focus();
                _batchForm.Show();
            }
            else
            {
                MessageBox.Show("Impossible load this file", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void commonToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _CNCParameters = new CNC_Parameters(this);
            this.Enabled = false;
            _CNCParameters.Show();
            _CNCParameters.Focus();
        }

        private void manageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ServerForm server = new ServerForm(this);
        }

        private void buttonGetResults_Click(object sender, EventArgs e)
        {
            IntPtr pData;
            int nNumPoints = 0;

            GetMVLResults(_eCurrentAnalysis, out nNumPoints, out pData);

            DialogResult result = DialogResult.No;

            result = MessageBox.Show("Send results to CNC?", "Select choice", MessageBoxButtons.YesNo);
            if (result == DialogResult.Yes)
            {
                ExtractResults pSendResults = new ExtractResults(this, nNumPoints, pData);
                pSendResults.SendDataToCNC(_nCurrentImageWidth, _nCurrentImageHeight);
            }

            Marshal.FreeHGlobal(pData);
        }

        private void designToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            //CNC_Design_Objects obj = new CNC_Design_Objects(this, _nXMaxVal, _nYMaxVal);
            // Valori di prova
            CNC_Design_Objects obj = new CNC_Design_Objects(this, 2000, 1000, 5);
        }
    }

    public static class ExtensionMethods
    {
        public static void LoadFunctions(this FieldInfo[] functions, IntPtr library, object target)
        {
            foreach (var field in functions)
            {
                string functionName = field.Name;
                IntPtr functionAddress = DLLImporter.GetProcAddress(library, functionName);

                var fieldType = field.FieldType;
                field.SetValue(target, Marshal.GetDelegateForFunctionPointer(functionAddress, fieldType));
            }
        }
    }
}

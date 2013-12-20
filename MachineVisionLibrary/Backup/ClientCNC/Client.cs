using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.Windows.Forms;
using System.IO;

namespace ClientCommunicationCNC
{
    public class Client
    {
        // Thread that manages client
        private Thread _threadClient = null;

        // Thread that listen messages from server
        private Thread _threadListenFromServer = null;

        private string _clientPublicAddress;

        private int _numberOfPort = 13457; // Port used to communicate with server
        private int _numberOfPortToListen = 13458; // Port used to receive commands from server

        // Semaphore list for TCP thread client
        private EventWaitHandle[] _eventHandle = {  new EventWaitHandle(false, EventResetMode.AutoReset),
                                                    new EventWaitHandle(false, EventResetMode.AutoReset),
                                                    new EventWaitHandle(false, EventResetMode.AutoReset),
                                                    new EventWaitHandle(false, EventResetMode.AutoReset)};

        private string _messaggeReceived = null;
        private string _messageToSend = null;

        // IP address of server
        private string _serverIPAddress = null;

        // Parent form information
        private Form1 _parentForm = null;
        private DisplayMessageTx _displayDelegate = null;
        private EndTransferToServer _endTransferDelegate = null;
        private EndConnectToServer _connectDelegate = null;
        private SetServerIPAddress _setServerIP = null;

        // Tcp client
        private TcpClient _clientTcp = null;

        // Udp client
        private UdpClient _clientUdp = null;
        private IPEndPoint _serverEndPoint = null;

        private TcpListener _listener = null;

        private int _nMillisecondsTimeout = 10000;

        private string _messageReceivedUDP = null;

        private EventWaitHandle _eventReceiveDoneUDP = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _eventSendDoneUDP = new EventWaitHandle(false, EventResetMode.AutoReset);

        // Represents port mapping
        private NATUPNPLib.IStaticPortMappingCollection _mappingsPort;

        private string _directoryWork = null;

        private void ClientListenServerThread()
        {
            bool bContinue = true;
            int nNumBytesRead = 0;
            Byte[] bytes = new Byte[1024000];
            String data = null;
            byte[] msgResponse = null;
            NetworkStream stream = null;
            TcpClient client = null;

            _listener = new TcpListener(IPAddress.Any, _numberOfPortToListen);
            _listener.Start();

            while (bContinue == true)
            {
                try
                {
                    client = _listener.AcceptTcpClient();

                    // Get stream
                    stream = client.GetStream();                    

                    // Read data
                    nNumBytesRead = stream.Read(bytes, 0, bytes.Length);

                    if (nNumBytesRead > 0)
                    {
                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, nNumBytesRead);

                        // Check if a file has been sent
                        if (String.Compare(data, 0, "FILE_TO_SEND_", 0, 13) == 0)
                        {
                            // Save file
                            int nIndex = -1;
                            int nFileLength;
                            int nFileTypeIndex = 0;
                            string fileData = String.Empty;
                            string fileType = String.Empty;

                            // Search file type
                            nFileTypeIndex = data.IndexOf("***");
                            if (nFileTypeIndex <= -1)
                            {
                                // abort
                                // Send response
                                msgResponse = System.Text.Encoding.ASCII.GetBytes("File type Not Found!");

                                // Send back a response.
                                stream.Write(msgResponse, 0, msgResponse.Length);

                                // Shutdown and end connection
                                client.Close();

                                continue;
                            }
                            fileType = data.Substring(13, nFileTypeIndex - 13);

                            nIndex = data.IndexOf("___");
                            if (nIndex <= -1)
                            {
                                // abort
                                // Send response
                                msgResponse = System.Text.Encoding.ASCII.GetBytes("Separator Not Found!");

                                // Send back a response.
                                stream.Write(msgResponse, 0, msgResponse.Length);

                                // Shutdown and end connection
                                client.Close();

                                continue;
                            }

                            nFileLength = Convert.ToInt32(data.Substring(nFileTypeIndex + 3, nIndex - (nFileTypeIndex + 3)));
                            if (nFileLength <= 0)
                            {
                                // abort
                                // Send response
                                msgResponse = System.Text.Encoding.ASCII.GetBytes("Error in File Length !");

                                // Send back a response.
                                stream.Write(msgResponse, 0, msgResponse.Length);

                                // Shutdown and end connection
                                client.Close();

                                continue;
                            }

                            fileData = data.Substring(nIndex + 3, data.Length - (nIndex + 3));

                            string clientSubDirectory = ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString();
                            string path = null;

                            clientSubDirectory = clientSubDirectory.Replace('.', '_');
                            path = System.IO.Path.Combine(_directoryWork, clientSubDirectory);

                            // Check if sub directory for client exists
                            if (Directory.Exists(path) == false)
                            {
                                // Create directory
                                Directory.CreateDirectory(path);
                            }

                            string tempPath = null;
                            tempPath = System.IO.Path.Combine(path, fileType);

                            // Display messages received
                            _parentForm.Invoke(this._displayDelegate,
                                (object)("Received a file from server: " +
                                _serverIPAddress +
                                " File name: " + fileType));

                            // Save file for current client
                            File.WriteAllText(tempPath, fileData);

                            // Send response
                            msgResponse = System.Text.Encoding.ASCII.GetBytes("File Well Saved!");

                            // Send back a response.
                            stream.Write(msgResponse, 0, msgResponse.Length);

                            // Shutdown and end connection
                            client.Close();

                            continue;
                        }

                        // Translate data bytes to a ASCII string.
                        data = System.Text.Encoding.ASCII.GetString(bytes, 0, nNumBytesRead);

                        // Display messages received
                        _parentForm.Invoke(this._displayDelegate, (object)(data));

                        msgResponse = System.Text.Encoding.ASCII.GetBytes(data);

                        // Send back a response.
                        stream.Write(msgResponse, 0, msgResponse.Length);

                        // Display messages received
                        _parentForm.Invoke(this._displayDelegate, (object)(data));

                        client.Close();
                    }
                }
                catch (Exception excp)
                {
                }
            }

        }

        private bool ReceiveUDPMessage(IPAddress address, int timeout)
        {
            try
            {
                _serverEndPoint = new IPEndPoint(address, _numberOfPortToListen);
                _clientUdp = new UdpClient(_serverEndPoint);

                UDPStateObject stateUDP = new UDPStateObject();
                stateUDP._udpClient = _clientUdp;
                stateUDP._endPoint = _serverEndPoint;

                _clientUdp.BeginReceive(new AsyncCallback(ReceiveCallbackUDP), stateUDP);
            }
            catch (Exception excp)
            {
            }

            if (timeout == 0)
            {
                return _eventReceiveDoneUDP.WaitOne(_nMillisecondsTimeout);
            }
            else
            {
                return _eventReceiveDoneUDP.WaitOne(timeout);
            }
        }

        private void ReceiveCallbackUDP(IAsyncResult ar)
        {
            try
            {
                UdpClient u = (UdpClient)((UDPStateObject)(ar.AsyncState))._udpClient;
                IPEndPoint e = (IPEndPoint)((UDPStateObject)(ar.AsyncState))._endPoint;

                Byte[] receiveBytes = u.EndReceive(ar, ref e);
                _messageReceivedUDP = Encoding.ASCII.GetString(receiveBytes);
            }
            catch(Exception excp)
            {
            }

            // Set event
            _eventReceiveDoneUDP.Set();
        }

        private bool SendUDPMessage(string message, IPAddress address)
        {
            _clientUdp = new UdpClient();
            _serverEndPoint = new IPEndPoint(address, _numberOfPortToListen);

            try
            {
                // Connect to remote end point
                _clientUdp.Connect(_serverEndPoint);

                Byte[] sendBytes = Encoding.ASCII.GetBytes(message);

                _clientUdp.BeginSend(sendBytes, sendBytes.Length,
                            new AsyncCallback(SendCallbackUDP), _clientUdp);
            }
            catch (Exception excp)
            {
            }

            return _eventSendDoneUDP.WaitOne(_nMillisecondsTimeout);
        }

        private void SendCallbackUDP(IAsyncResult ar)
        {
            UdpClient u = (UdpClient)ar.AsyncState;

            try
            {
                u.EndSend(ar);
            }
            catch (Exception excp)
            {
            }

            _eventSendDoneUDP.Set();
        }


        private void ClientThreadFunction()
        {
            int nIndex = 0;
            string data = null;
            bool bContinue = true;

            // Initialization //

            while ( bContinue == true )
            {
                nIndex = EventWaitHandle.WaitAny(_eventHandle);

                switch (nIndex)
                {
                    // For initialization
                    case 0:

                        // Make a request to server to know its IP address
                        data = "Connect Client";

                        if (_serverIPAddress != null)
                        {
                            // Send data
                            if (ClientTCPSendData(data, _serverIPAddress, _numberOfPort) == false)
                            {
                                // Send information to form
                                _parentForm.Invoke(this._connectDelegate, (object)false);
                                break;
                            }
                            
                            // Get data
                            data = ClientTCPGetData();
                            if (data == null)
                            {
                                // Send information to form
                                _parentForm.Invoke(this._connectDelegate, (object)false);
                                break;
                            }

                            // Close connection
                            CloseClientTCP();

                            // Send data to display
                            _parentForm.Invoke(this._displayDelegate, (object)data);

                            // Send information to form
                            _parentForm.Invoke(this._connectDelegate, (object)true);

                            // Set server ip address
                            _parentForm.Invoke(this._setServerIP, (object)_serverIPAddress);

                            break;
                        }
                        else 
                        {
                            MessageBox.Show("Set Server IP address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Send information to form
                            _parentForm.Invoke(this._connectDelegate, (object)false);
                            break;
                        }
                        break;

                    // Close client
                    case 1:

                        // Send information to form
                        _parentForm.Invoke(this._connectDelegate, (object)false);
                        break;

                    // Send a request to server
                    case 2:

                        data = _messageToSend;

                        if (_serverIPAddress != null)
                        {
                            // Send data
                            if (ClientTCPSendData(data, _serverIPAddress, _numberOfPort) == false)
                            {
                                // Send information to form
                                _parentForm.Invoke(this._connectDelegate, (object)false);
                                break;
                            }

                            // Get data
                            _messaggeReceived = ClientTCPGetData();
                            if (data == null)
                            {
                                // Send information to form
                                _parentForm.Invoke(this._connectDelegate, (object)false);
                                break;
                            }

                            // Close connection
                            CloseClientTCP();

                            // Send data to display
                            _parentForm.Invoke(this._displayDelegate, (object)_messaggeReceived);

                            // Send information to form
                            _parentForm.Invoke(this._connectDelegate, (object)true);

                            // Send information to form
                            _parentForm.Invoke(this._endTransferDelegate, (object)true);

                            break;
                        }
                        else
                        {
                            MessageBox.Show("Set Server IP address", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);

                            // Send information to form
                            _parentForm.Invoke(this._connectDelegate, (object)false);

                            // Send information to form
                            _parentForm.Invoke(this._endTransferDelegate, (object)false);
                            break;
                        }
                        break;

                    // End thread
                    case 3:

                        _listener.Stop();

                        // Send information to form
                        _parentForm.Invoke(this._connectDelegate, (object)false);
                        bContinue = false;
                        break;

                    default:
                        break;
                }
            }
        }

        private bool ClientTCPSendData(string message, string ip, int numPort)
        {
            try
            {
                if (_clientTcp == null)
                {
                    _clientTcp = new TcpClient(ip, numPort);
                }

                // Translate the passed message into ASCII and store it as a Byte array.
                Byte[] data = System.Text.Encoding.ASCII.GetBytes(message);

                NetworkStream stream = _clientTcp.GetStream();

                // Send the message to the connected TcpServer. 
                stream.Write(data, 0, data.Length);

                return true;
            }
            catch (Exception excp)
            {
                return false;
            }
        }

        private string ClientTCPGetData()
        {
            Byte[] data;
            // String to store the response ASCII representation.
            String responseData = String.Empty;

            if (_clientTcp == null)
            {
                return null;
            }

            try
            {
                NetworkStream stream = _clientTcp.GetStream();

                // Buffer to store the response bytes.
                data = new Byte[10240];

                // Read the first batch of the TcpServer response bytes.
                Int32 bytes = stream.Read(data, 0, data.Length);
                responseData = System.Text.Encoding.ASCII.GetString(data, 0, bytes);

                stream.Close();
            }
            catch (Exception excp)
            {
                
            }

            return responseData;
        }

        private void CloseClientTCP()
        {
            if (_clientTcp != null)
            {
                _clientTcp.Close();
                _clientTcp = null;
            }
        }

        private string GetPublicIPAddress()
        {
            string url = "http://checkip.dyndns.org";
            System.Net.WebRequest req = System.Net.WebRequest.Create(url);
            System.Net.WebResponse resp = req.GetResponse();
            System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
            string response = sr.ReadToEnd().Trim();
            string[] a = response.Split(':');
            string a2 = a[1].Substring(1);
            string[] a3 = a2.Split('<');
            string a4 = a3[0];
            return a4;
        }

        private string GetLocalIPAddress()
        {
            /*
            IPHostEntry host;
            string localIP = "";
            host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (IPAddress ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    localIP = ip.ToString();
                    break;
                }
            }
            return localIP;
            */
            string strHostName = "";
            strHostName = System.Net.Dns.GetHostName();

            IPHostEntry ipEntry = System.Net.Dns.GetHostEntry(strHostName);

            IPAddress[] addr = ipEntry.AddressList;

            strHostName = addr[addr.Length - 2].ToString();

            return strHostName;
        }

        public Client(Form1 form, string serverIP, DisplayMessageTx displayDelegate, EndTransferToServer endDelegate,
                      EndConnectToServer connectDelegate, SetServerIPAddress serverIPDelegate, bool bDiscover)
        {
            _parentForm = form;
            _displayDelegate = displayDelegate;
            _endTransferDelegate = endDelegate;
            _connectDelegate = connectDelegate;
            _setServerIP = serverIPDelegate;

            _serverIPAddress = serverIP;

            // Get directory of job
            _directoryWork = Directory.GetCurrentDirectory();

            // Create thread
            _threadClient = new Thread(new ThreadStart(ClientThreadFunction));
            _threadClient.Name = "Client Thread";
            _threadClient.Start();

            // Create thread that listen for server remote commands
            _threadListenFromServer = new Thread(new ThreadStart(ClientListenServerThread));
            _threadListenFromServer.Name = "Client Listen from Server";
            _threadListenFromServer.Start();

            try
            {
                // How enable port forwarding
                NATUPNPLib.UPnPNATClass upnpnat = new NATUPNPLib.UPnPNATClass();
                _mappingsPort = upnpnat.StaticPortMappingCollection;

                // Open a UDP Port to forward to a specific Computer on the Private Network
                _mappingsPort.Add(_numberOfPortToListen, "TCP", _numberOfPortToListen, GetLocalIPAddress(), true, "Client TCP");
            }
            catch (Exception excp)
            {
                MessageBox.Show("Impossible discover server because port " + _numberOfPortToListen + " is already occupied", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Send information to form
                _parentForm.Invoke(this._connectDelegate, (object)false);
                return;
            }

            // Get client public ip
            _clientPublicAddress = GetPublicIPAddress();

            if (bDiscover == true)
            {
                
            }
            else
            {
                // Start initialization
                _eventHandle[0].Set();
            }
        }

        public void DestroyClient()
        {
            if (_threadClient != null)
            {
                _messageToSend = "Disconnect Client";
                _eventHandle[2].Set();

                // Remove UDP forwarding for Port _numberOfPortToListen
                try
                {
                    _mappingsPort.Remove(_numberOfPortToListen, "TCP");
                }
                catch (Exception excp)
                {
                }

                _eventHandle[3].Set();
            }
        }

        public bool SendMessage(string message)
        {
            _messageToSend = message;

            _eventHandle[2].Set();
            return true;
        }

        public void CloseClient()
        {
            _eventHandle[1].Set();
        }
    }

    public class UDPStateObject
    {
        public UdpClient _udpClient;
        public IPEndPoint _endPoint;
    }
}

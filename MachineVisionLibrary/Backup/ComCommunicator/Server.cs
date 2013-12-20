using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows.Forms;
using System.Net;
using System.Net.Sockets;
//using System.Net.Mail;
using System.Linq;
using System.Text;
using System.Threading;
using System.Diagnostics;
using System.ComponentModel;
using System.Management;
using System.IO;

namespace ComCommunicator
{
    public enum ShutDown
    {
        LogOff = 0,
        Shutdown = 1,
        Reboot = 2,
        ForcedLogOff = 4,
        ForcedShutdown = 5,
        ForcedReboot = 6,
        PowerOff = 8,
        ForcedPowerOff = 12
    }

    public class Server
    {
        private Thread _serverThread = null;
        private Queue _messageReceivedQueue = Queue.Synchronized(new Queue());
        private static EventWaitHandle[] _eventsToWait = { new EventWaitHandle(false, EventResetMode.AutoReset),
                                                           new EventWaitHandle(false, EventResetMode.AutoReset),
                                                           new EventWaitHandle(false, EventResetMode.AutoReset), 
                                                           new EventWaitHandle(false, EventResetMode.AutoReset),
                                                           new EventWaitHandle(false, EventResetMode.AutoReset)};
        private int _numberOfPortToListen = 13457;
        private int _numPortForSend = 13458;
        private string _serverIPAddress = null;

        // Parent form variables
        private ServerForm _parentForm = null;
        private StartStopServer _endActionDelegate = null;
        private ServerMessageReceived _messageReceivedAction = null;
        private ServerMessageResponse _messageResponseAction = null;
        private AddClientsToServer _addClientDelegate = null;
        private RemoveClientsToServer _removeClientDelegate = null;
        private SetServerIPAddress _displayServerIP = null;
        private static bool _bIsServerOpened = false;


        private StringBuilder _messageComposed = new StringBuilder();
        private static string _responseToClient = null;
        private static bool _bIsServerInAnswer = false;

        // TCP listener for all connections
        private TcpListener _serverListenerTCP = null;
        private Thread _serverListenThread = null;

        // TCP client list
        private List<TcpClient> _clientList = null;

        // TCP client threads
        private List<Thread> _clientThreads = null;

        // Used to know if server is started up
        EventWaitHandle _eventServerStatus = new EventWaitHandle(false, EventResetMode.AutoReset);

        // Used to access to client list
        private object _manageClientList = new object();

        // Used to send email
        MailWrapper _mailWrapper = new MailWrapper();

        private NATUPNPLib.IStaticPortMappingCollection _mappingsPort;

        // Udp client
        UdpClient _clientUdp = null;
        IPEndPoint _serverEndPoint = null;

        private string _messageReceivedUDP = null;

        private EventWaitHandle _eventReceiveDoneUDP = new EventWaitHandle(false, EventResetMode.AutoReset);
        private EventWaitHandle _eventSendDoneUDP = new EventWaitHandle(false, EventResetMode.AutoReset);

        // Semaphore list for UDP thread client
        private EventWaitHandle[] _eventHandleSendToClient = {  new EventWaitHandle(false, EventResetMode.AutoReset),
                                                                new EventWaitHandle(false, EventResetMode.AutoReset)};
        private Thread _threadSendToClient = null;

        private string _fileTosend = null;
        private string _clientIPAddress = null;

        private TcpClient _clientTcp = null;

        private string _directoryWork = null;

        private void ThreadReceiveResponse()
        {
            int nIndex = 0;
            bool bContinue = true;
            string totalData = String.Empty;

            while (bContinue == true)
            {
                nIndex = EventWaitHandle.WaitAny(_eventHandleSendToClient);


                switch (nIndex)
                {
                    case 0:

                        if (TCPSendData(_fileTosend, _clientIPAddress, _numPortForSend) == false)
                        {
                            MessageBox.Show("TCP Connection failed !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                            break;
                        }

                        // Get response
                        totalData = TCPGetData();

                        if (totalData != null)
                        {
                            // Signal that a message has been received
                            _parentForm.Invoke(this._messageReceivedAction, (Object)(totalData));
                        }

                        // Close communication
                        CloseTCP();
                        break;

                    case 1:
                        bContinue = false;
                        break;
                }
            }

        }

        private bool TCPSendData(string message, string ip, int numPort)
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

        private string TCPGetData()
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

        private void CloseTCP()
        {
            if (_clientTcp != null)
            {
                _clientTcp.Close();
                _clientTcp = null;
            }
        }

        private bool ReceiveUDPMessage(IPAddress address)
        {
            try
            {
                _serverEndPoint = new IPEndPoint(address, _numPortForSend);
                _clientUdp = new UdpClient(_serverEndPoint);

                UDPStateObject stateUDP = new UDPStateObject();
                stateUDP._udpClient = _clientUdp;
                stateUDP._endPoint = _serverEndPoint;

                _clientUdp.BeginReceive(new AsyncCallback(ReceiveCallbackUDP), stateUDP);
            }
            catch (Exception excp)
            {
            }

            return _eventReceiveDoneUDP.WaitOne();
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
            catch (Exception excp)
            {
            }

            // Set event
            _eventReceiveDoneUDP.Set();
        }

        private bool SendUDPMessage(string message, IPAddress address)
        {
            _clientUdp = new UdpClient();
            _serverEndPoint = new IPEndPoint(address, _numPortForSend);

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

            return _eventSendDoneUDP.WaitOne();
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

        // Used to get messages from clients
        private void ServerMainThreadFunction()
        {
            int nIndex = 0;
            bool bContinue = true;

            while (bContinue)
            {
                nIndex = EventWaitHandle.WaitAny(_eventsToWait);

                switch (nIndex)
                {
                    // Start server
                    case 0:

                        _clientList = new List<TcpClient>();
                        _clientThreads = new List<Thread>();

                        _serverListenThread = new Thread(new ThreadStart(ServerListenConnections));
                        _serverListenThread.Start();

                        // Waiting for server is up
                        if (_eventServerStatus.WaitOne(2000) == false)
                        {
                            // Send action
                            _parentForm.Invoke(this._endActionDelegate, (object)false);
                            break;
                        }

                        // Server is opend
                        _bIsServerOpened = true;

                        // Send action
                        _parentForm.Invoke(this._endActionDelegate, (object)true);

                        //SendMailServerStatus(true);
                        break;

                    // Stop server
                    case 1:

                        _bIsServerOpened = false;

                        _serverListenerTCP.Stop();

                        // Send action
                        _parentForm.Invoke(this._endActionDelegate, (object)false);

                        //SendMailServerStatus(false);
                        break;

                    // Received data
                    case 2:
                        string message = String.Empty;
                        bool bValue = false;

                        while (_messageReceivedQueue.Count > 0)
                        {
                            message = (string)_messageReceivedQueue.Dequeue();

                            // Signal that a message has been received
                            _parentForm.Invoke(this._messageReceivedAction, (Object)(_messageComposed.ToString()));

                            if (_bIsServerInAnswer == false)
                            {
                                // Send answer to client
                                _responseToClient = ParseClientMessage(_messageComposed.ToString());

                                _eventsToWait[4].Set();
                            }
                            _messageComposed.Length = 0;

                            if (bValue == false)
                            {
                                bValue = true;
                            }
                        }

                        break;

                    // Destroy thread
                    case 3:

                        bContinue = false;
                        break;

                    // Send answer to client
                    case 4:

                        if (_responseToClient != null && _bIsServerInAnswer == false)
                        {
                        }
                        break;

                    default:
                        break;
                }
            }
        }

        private void ServerListenConnections()
        {
            bool bContinue = true;

            // Create TCP listener
            _serverListenerTCP = new TcpListener(IPAddress.Any, _numberOfPortToListen);

            // Start listening for client requests
            _serverListenerTCP.Start();

            // Now server is up
            _eventServerStatus.Set();

            while ( bContinue == true )
            {
                // Waiting for clients
                try
                {
                    TcpClient client = _serverListenerTCP.AcceptTcpClient();

                    lock (_manageClientList)
                    {
                        // Create thread used to manage clients
                        Thread clientThread = new Thread(ClientManagementThread);
                        _clientThreads.Add(clientThread);

                        // Start client thread
                        clientThread.Start(client);
                    }
                }
                catch (Exception excp)
                {
                    
                }
            }
        }

        private void ClientManagementThread(object tcpClient)
        {
            TcpClient client = (TcpClient)tcpClient;

            int nNumBytesRead = 0;
            Byte[] bytes = new Byte[1024000];
            String data = null;
            byte[] msgResponse = null;

            // If client must be disconnected from server
            bool bDisconnect = false;

            // Get stream
            NetworkStream stream = client.GetStream();

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

                        return;
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

                        return;
                    }

                    nFileLength = Convert.ToInt32(data.Substring(nFileTypeIndex + 3, nIndex - (nFileTypeIndex+3)));
                    if (nFileLength <= 0)
                    {
                        // abort
                        // Send response
                        msgResponse = System.Text.Encoding.ASCII.GetBytes("Error in File Length !");

                        // Send back a response.
                        stream.Write(msgResponse, 0, msgResponse.Length);

                        // Shutdown and end connection
                        client.Close();

                        return;
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

                    // Search first index valid to save file
                    
                    string tempPath = null;
                    tempPath = System.IO.Path.Combine(path, fileType);

                    // Display messages received
                    _parentForm.Invoke(this._messageReceivedAction, 
                        (object)("Received a file from client: " +
                        ((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString()) +
                        " File name: " + fileType);

                    // Save file for current client
                    File.WriteAllText(tempPath, fileData);

                    // Send response
                    msgResponse = System.Text.Encoding.ASCII.GetBytes("File Well Saved!");

                    // Send back a response.
                    stream.Write(msgResponse, 0, msgResponse.Length);

                    lock (_manageClientList)
                    {
                        _clientThreads.Remove(Thread.CurrentThread);
                    }

                    // Shutdown and end connection
                    client.Close();

                    return;
                }

                // Display messages received
                _parentForm.Invoke(this._messageReceivedAction, (object)(data));

                if (String.Compare(data, "Disconnect Client") == 0)
                {
                    bDisconnect = true;
                }
                else if (String.Compare(data, "Connect Client") == 0)
                {
                    lock (_manageClientList)
                    {
                        // Look for if client is already registered
                        if (_clientList.Find(
                            delegate(TcpClient myClient)
                            {
                                for (int i = 0; i < _clientList.Count; i++)
                                {
                                    if (_clientList.ElementAt(i) == myClient)
                                    {
                                        return true;
                                    }
                                }
                                return false;
                            }
                        ) != client)
                        {
                            // Add element to list
                            _clientList.Add(client);

                            _parentForm.Invoke(this._addClientDelegate, (object)((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                        }
                    }
                }

                // Process the data sent by the client
                data = ParseClientMessage(data);

                msgResponse = System.Text.Encoding.ASCII.GetBytes(data);

                // Send back a response.
                stream.Write(msgResponse, 0, msgResponse.Length);

                // Display messages received
                _parentForm.Invoke(this._messageResponseAction, (object)(data));
            }

            // Remove client from lists
            if (bDisconnect == true)
            {
                lock (_manageClientList)
                {
                    _clientList.Remove(client);
                    _clientThreads.Remove(Thread.CurrentThread);

                    _parentForm.Invoke(this._removeClientDelegate, (object)((IPEndPoint)client.Client.RemoteEndPoint).Address.ToString());
                }
            }
            else
            {
                lock (_manageClientList)
                {
                    _clientThreads.Remove(Thread.CurrentThread);
                }
            }

            // Shutdown and end connection
            client.Close();
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

        private void shutDown(ShutDown flag)
        {
            ManagementBaseObject outParam = null;
            ManagementClass sysOS = new ManagementClass("Win32_OperatingSystem");
            sysOS.Get();
            // enables required security privilege.
            sysOS.Scope.Options.EnablePrivileges = true;
            // get our in parameters
            ManagementBaseObject inParams = sysOS.GetMethodParameters("Win32Shutdown");
            // pass the flag of 0 = System Shutdown
            inParams["Flags"] = flag;
            inParams["Reserved"] = "0";
            foreach (ManagementObject manObj in sysOS.GetInstances())
            {
                outParam = manObj.InvokeMethod("Win32Shutdown", inParams, null);
            }
        }

        private string ParseClientMessage(string message)
        {
            if (String.Compare(message, "Request IP Server") == 0)
            {
                return _serverIPAddress;
            }
            else if (String.Compare(message, "CNC Server?") == 0)
            {
                return "Server CNC ON with IP Address: " + _serverIPAddress;
            }
            else if (String.Compare(message, "Disconnect Client") == 0)
            {
                return "OK Client Disconnected";
            }
            else if (String.Compare(message, "Connect Client") == 0)
            {
                return "OK Client Connected";
            }
            else if (String.Compare(message, "Reboot") == 0)
            {
                shutDown(ShutDown.ForcedReboot);
                return "OK";
            }
            else if (String.Compare(message, "Shutdown") == 0)
            {
                shutDown(ShutDown.ForcedShutdown);
                return "OK";
            }
            else if (String.Compare(message, "LogOff") == 0)
            {
                shutDown(ShutDown.ForcedLogOff);
                return "OK";
            }
            else
            {
                return "Messsage Unknown";
            }
        }

        private void SendMailServerStatus(bool bValue)
        {
            string subjectMail = "CNC Server Address";
            string textMail;

            if (bValue == true)
            {
                textMail = "CNC Server is UP with IP: " + _serverIPAddress;
            }
            else
            {
                textMail = "CNC Server is DOWN";
            }

            _mailWrapper.SendEmailToClients(subjectMail, textMail);
        }

        // Constructor
        public Server(ServerForm form, StartStopServer del, ServerMessageReceived action,
            ServerMessageResponse actionResponse, AddClientsToServer addClient, 
            RemoveClientsToServer removeClient, SetServerIPAddress serverIPDelegate)
        {
            // Create thread
            _serverThread = new Thread(new ThreadStart(ServerMainThreadFunction));
            _serverThread.Name = "Server Main Thread";
            _serverThread.Start();

            // Create thread for UDP connection
            _threadSendToClient = new Thread(new ThreadStart(ThreadReceiveResponse));
            _threadSendToClient.Name = "Server Thread Listener";
            _threadSendToClient.Start();

            _parentForm = form;
            _endActionDelegate = del;
            _messageReceivedAction = action;
            _messageResponseAction = actionResponse;
            _addClientDelegate = addClient;
            _removeClientDelegate = removeClient;
            _displayServerIP = serverIPDelegate;

            // Get directory of job
            _directoryWork = Directory.GetCurrentDirectory();

            // Get local IP address
            _serverIPAddress = GetPublicIPAddress();

            _parentForm.Invoke(_displayServerIP, (object)_serverIPAddress);

            // How enable port forwarding
            NATUPNPLib.UPnPNATClass upnpnat = new NATUPNPLib.UPnPNATClass();
            _mappingsPort = upnpnat.StaticPortMappingCollection;

            GetLocalIPAddress();

            try
            {
                _mappingsPort.Add(_numberOfPortToListen, "TCP", _numberOfPortToListen, GetLocalIPAddress(), true, "CNC TCP Server");
            }
            catch (Exception excp)
            {
                MessageBox.Show(excp.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                // Send action
                _parentForm.Invoke(this._endActionDelegate, (object)false);
                return;
            }
        }

        public bool DestroyServer()
        {
            _eventsToWait[3].Set();
            _serverThread.Join();

            _mappingsPort.Remove(_numberOfPortToListen, "TCP");
            return true;
        }

        // Public interface
        public bool StartServer()
        {
            if (_serverThread == null)
            {
                return false;
            }

            // Signal to start server
            _eventsToWait[0].Set();

            return true;
        }

        public bool StopServer()
        {
            if (_serverThread == null)
            {
                return false;
            }

            // Signal to stop server
            _eventsToWait[1].Set();

            return true;
        }

        public bool SendFileToClient(string fileData, string clientIP)
        {
            IPAddress address;

            if ((fileData == null) || (clientIP == null) || (IPAddress.TryParse(clientIP, out address) == false))
            {
                return false;
            }

            _fileTosend = fileData;
            _clientIPAddress = clientIP;

            _eventHandleSendToClient[0].Set();

            return true;
        }
    }

    public class UDPStateObject
    {
        public UdpClient _udpClient;
        public IPEndPoint _endPoint;
    }
}

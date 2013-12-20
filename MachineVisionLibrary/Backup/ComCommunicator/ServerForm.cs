using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;

namespace ComCommunicator
{
    public delegate void StartStopServer(bool bValue);
    public delegate void ServerMessageReceived(string message);
    public delegate void ServerMessageResponse(string message);
    public delegate void AddClientsToServer(string clientIP);
    public delegate void RemoveClientsToServer(string clientIP);
    public delegate void SetServerIPAddress(string serverIP);

    public partial class ServerForm : Form
    {
        private Form1 _parentForm = null;
        private bool _bIsServerStarted = false;
        private Server _server = null;

        public ServerForm(Form1 parentForm)
        {
            InitializeComponent();

            _parentForm = parentForm;

            this.Show();
            this.Focus();
        }

        public void DisplayServerIP(string serverIP)
        {
            labelServerIP.Text = "Server IP: ";
            textBoxServerIP.Text = serverIP;
        }

        public void ActionStartStop(bool bValue)
        {
            if (bValue == true)
            {
                _bIsServerStarted = true;

                buttonStart.Text = "Stop Server";
            }
            else
            {
                _bIsServerStarted = false;

                buttonStart.Text = "Start Server";
            }

            this.Enabled = true;
        }

        public void ServerCommandReceived(string message)
        {
            richTextBoxCommandReceived.Invoke(new EventHandler(delegate
                {
                    richTextBoxCommandReceived.SelectionColor = Color.Black;
                    richTextBoxCommandReceived.AppendText("Received From Client: ");
                    richTextBoxCommandReceived.SelectionColor = Color.Red;
                    richTextBoxCommandReceived.AppendText(message + "\r");
                    richTextBoxCommandReceived.ScrollToCaret();

                }));
        }

        public void ServerCommandResponse(string message)
        {
            richTextBoxCommandReceived.Invoke(new EventHandler(delegate
            {
                richTextBoxCommandReceived.SelectionColor = Color.Black;
                richTextBoxCommandReceived.AppendText("Response to Client: ");
                richTextBoxCommandReceived.SelectionColor = Color.Blue;
                richTextBoxCommandReceived.AppendText(message + "\r");
                richTextBoxCommandReceived.ScrollToCaret();
            }));
        }

        public void AddClientToServer(string clientIPAddress)
        {
            int nIndex = comboBoxClients.FindString(clientIPAddress);
            
            if (nIndex < 0)
            {
                comboBoxClients.BeginUpdate();
                comboBoxClients.Items.Add(clientIPAddress);
                comboBoxClients.EndUpdate();

                labelNumClients.Text = "Number of Clients: " + comboBoxClients.Items.Count;
            }
        }

        public void RemoveClientToServer(string clientIPAddress)
        {
            int nIndex = comboBoxClients.FindString(clientIPAddress);

            if (nIndex >= 0)
            {
                comboBoxClients.BeginUpdate();
                comboBoxClients.Items.Remove(clientIPAddress);
                comboBoxClients.EndUpdate();

                labelNumClients.Text = "Number of Clients: " + comboBoxClients.Items.Count;
            }
        }

        private void buttonStart_Click(object sender, EventArgs e)
        {
            this.Enabled = false;
            
            if (_bIsServerStarted == false)
            {
                // Start server
                _server.StartServer();
            }
            else
            {
                // Stop server
                _server.StopServer();
            }
        }

        private void ServerForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _server.DestroyServer();
            _parentForm.Focus();
        }

        private void ServerForm_Load(object sender, EventArgs e)
        {
            this.Enabled = false;
            StartStopServer dele = this.ActionStartStop;
            ServerMessageReceived messageReceived = this.ServerCommandReceived;
            ServerMessageResponse messageResponse = this.ServerCommandResponse;
            AddClientsToServer addClient = this.AddClientToServer;
            RemoveClientsToServer removeClient = this.RemoveClientToServer;
            SetServerIPAddress serverIP = this.DisplayServerIP;
            _server = new Server(this, dele, messageReceived, messageResponse, addClient, removeClient, serverIP);
            this.Enabled = true;
        }

        private void ServerForm_Shown(object sender, EventArgs e)
        {
            
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxCommandReceived.Clear();
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == DialogResult.OK)
            {
                textBoxSendData.Text = file.FileName;
            }
        }

        private void textBoxSendData_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (File.Exists(textBoxSendData.Text) == false)
                {
                    textBoxSendData.Clear();
                }
            }
        }

        private void buttonSendFile_Click(object sender, EventArgs e)
        {
            string clientIP = null;
            if (comboBoxClients.Items.Count > 0)
            {
                clientIP = comboBoxClients.GetItemText(comboBoxClients.SelectedItem);
            }
            
            if ((textBoxSendData.TextLength > 0) && (clientIP != null))
            {
                if (File.Exists(textBoxSendData.Text) == true)
                {
                    byte[] fileDataRaw = File.ReadAllBytes(textBoxSendData.Text);
                    string totalData = "FILE_TO_SEND_";
                    int nVal = fileDataRaw.Length;

                    string fileType = textBoxSendData.Text.Substring(textBoxSendData.Text.LastIndexOf('.'),
                        textBoxSendData.TextLength - textBoxSendData.Text.LastIndexOf('.'));

                    string filename = textBoxSendData.Text.Substring(textBoxSendData.Text.LastIndexOf('\\') + 1,
                        textBoxSendData.TextLength - textBoxSendData.Text.LastIndexOf('\\') -
                        fileType.Length - 1);

                    totalData += filename + fileType + "***";

                    totalData += Convert.ToString(nVal) + "___";
                    foreach (var item in fileDataRaw)
                    {
                        totalData += Convert.ToString(Convert.ToChar(item));
                    }
                    _server.SendFileToClient(totalData, clientIP);
                }
                else
                {
                    MessageBox.Show("File Not Exists !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

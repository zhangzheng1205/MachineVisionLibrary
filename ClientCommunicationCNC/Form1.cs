using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Net;
using System.Net.Sockets;
using System.Diagnostics;
using System.Threading;
using System.IO;
using System.Windows.Forms;

namespace ClientCommunicationCNC
{
    public delegate void DisplayMessageTx(string message);
    public delegate void SetServerIPAddress(string ipAddress);
    public delegate void EndTransferToServer(bool bValue);
    public delegate void EndConnectToServer(bool bValue);

    public partial class Form1 : Form
    {
        private Client _client = null;
        private bool _bIsConnected = false;
        private string _serverIPAddress = null;

        public Form1()
        {
            InitializeComponent();
            this.Text = "Client Application";
        }

        public void DisplayMessage(string message)
        {
            richTextBoxMessage.Invoke(new EventHandler(delegate
            {
                richTextBoxMessage.SelectionColor = Color.Black;
                richTextBoxMessage.AppendText(message + "\r");
                richTextBoxMessage.ScrollToCaret();
            }));
        }

        public void SetServerIP(string IP)
        {
        }

        public void EndTransferToServer(bool bValue)
        {
            this.Enabled = true;

            if (bValue == true)
            {
            }
            else
            {
                MessageBox.Show("Wrong Trasmission", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                this._bIsConnected = false;
                _client.DestroyClient();
                _client = null;
                buttonConnect.Text = "Connect To Server";
            }
        }

        public void ConnectDisconnect(bool bValue)
        {
            this.Enabled = true;

            if (bValue == true)
            {
                _bIsConnected = true;
                buttonConnect.Text = "Disconnect From Server";
            }
            else
            {

                if (_bIsConnected == true)
                {
                    _bIsConnected = false;
                    buttonConnect.Text = "Connect To Server";

                    MessageBox.Show("Connection losed from Server", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
                else
                {
                    MessageBox.Show("Error Connecting To Server", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

        private void buttonSend_Click(object sender, EventArgs e)
        {
            if (textBoxCommands.TextLength > 0 && this._bIsConnected == true)
            {
                _client.SendMessage(textBoxCommands.Text);
            }
        }

        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            if (_client != null)
            {
                _client.DestroyClient();
            }

            try
            {
                foreach (Process proc in Process.GetProcessesByName("ClientCNC"))
                {
                    proc.Kill();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }

        private void buttonClear_Click(object sender, EventArgs e)
        {
            richTextBoxMessage.Clear();
            textBoxCommands.Clear();
        }

        private void Form1_Shown(object sender, EventArgs e)
        {
            
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            _serverIPAddress = textBox1.Text;
        }

        private void buttonConnect_Click(object sender, EventArgs e)
        {
            if (this._bIsConnected == false)
            {
                DisplayMessageTx del = DisplayMessage;
                EndTransferToServer endDel = EndTransferToServer;
                EndConnectToServer connectDel = ConnectDisconnect;
                SetServerIPAddress delSetServerIP = SetServerIP;

                this.Enabled = false;
                IPAddress address;
                if ((_serverIPAddress != null) && (IPAddress.TryParse(_serverIPAddress, out address) == true))
                {
                    _client = new Client(this, _serverIPAddress, del, endDel, connectDel, delSetServerIP, false);
                }
                else
                {
                    this.Enabled = true;
                    return;
                }
            }
            else
            {
                if (_client != null)
                {
                    this.Enabled = false;
                    _client.DestroyClient();
                }
                
                richTextBoxMessage.Clear();
                textBoxCommands.Clear();            
            }
        }

        private void buttonSelectFile_Click(object sender, EventArgs e)
        {
            OpenFileDialog file = new OpenFileDialog();

            if (file.ShowDialog() == DialogResult.OK)
            {
                textBoxSendDataPath.Text = file.FileName;
            }
        }

        private void textBoxSendDataPath_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                if (File.Exists(textBoxSendDataPath.Text) == false)
                {
                    textBoxSendDataPath.Clear();
                }
            }
        }

        private void buttonSendData_Click(object sender, EventArgs e)
        {
            if ((_bIsConnected == true) && (textBoxSendDataPath.TextLength > 0))
            {
                if (File.Exists(textBoxSendDataPath.Text) == true)
                {
                    byte[] fileDataRaw = File.ReadAllBytes(textBoxSendDataPath.Text);
                    string totalData = "FILE_TO_SEND_";
                    int nVal = fileDataRaw.Length;

                    string fileType = textBoxSendDataPath.Text.Substring(textBoxSendDataPath.Text.LastIndexOf('.'),
                        textBoxSendDataPath.TextLength - textBoxSendDataPath.Text.LastIndexOf('.'));

                    string filename = textBoxSendDataPath.Text.Substring(textBoxSendDataPath.Text.LastIndexOf('\\') + 1,
                        textBoxSendDataPath.TextLength - textBoxSendDataPath.Text.LastIndexOf('\\') -
                        fileType.Length - 1);

                    totalData += filename + fileType + "***";

                    totalData += Convert.ToString(nVal) + "___";
                    foreach (var item in fileDataRaw)
                    {
                        totalData += Convert.ToString(Convert.ToChar(item));
                    }
                    _client.SendMessage(totalData);
                }
                else
                {
                    MessageBox.Show("File Not Exists !", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
    }
}

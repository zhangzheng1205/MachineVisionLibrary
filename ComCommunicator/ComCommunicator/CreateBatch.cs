using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Windows;
using System.IO;

namespace ComCommunicator
{
    public partial class CreateBatch : Form
    {
        private Form1 _parentForm;
        private ArrayList _commandList;
        private bool _bDirty = false;
        private delegate bool SendBatchCommandsDelegate(string[] sCommands);

        public CreateBatch(Form1 parentForm)
        {
            InitializeComponent();

            _parentForm = parentForm;
            _commandList = new ArrayList();

            TransferLabel.Visible = false;
            progressBarBatch.Visible = false;
            progressBarBatch.Minimum = 0;
            progressBarBatch.Value = 0;
            progressBarBatch.Step = 1;
            this.TransferLabel.Text = "Transferred Commands 0 di 0";
        }

        public CreateBatch(Form1 parentForm, string[] commands)
        {
            InitializeComponent();

            _parentForm = parentForm;
            _commandList = new ArrayList();

            TransferLabel.Visible = false;
            progressBarBatch.Visible = false;
            progressBarBatch.Minimum = 0;
            progressBarBatch.Value = 0;
            progressBarBatch.Step = 1;
            this.TransferLabel.Text = "Transferred Commands 0 di 0";

            foreach (var item in commands)
            {
                CommandrichTextBox.AppendText(item);
                _commandList.Add(item);
                CommandrichTextBox.AppendText("\r");
            }

            _bDirty = false;
        }

        private void CreateBatch_FormClosed(object sender, FormClosedEventArgs e)
        {
            DialogResult result = DialogResult.Cancel;

            if (_bDirty == true)
            {
                result = MessageBox.Show("Save before exit ?", "Warning",
                             MessageBoxButtons.YesNo);

                if (result == DialogResult.Yes)
                {
                    Savebutton_Click(this, EventArgs.Empty);
                }
            }

            _parentForm.Enabled = true;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            if (progressBarBatch.Visible == true)
            {
                this.Close();
            }
        }

        private void AddPointbutton_Click(object sender, EventArgs e)
        {
            AddPointForm pointForm = new AddPointForm(this);
            this.Enabled = false;
        }

        private string FormatPointCoordinates(uint x, uint y, uint z)
        {
            string xCoord;
            string yCoord;
            string zCoord;
            string finalString;

            if (x > 999999)
            {
                xCoord = "999999";
            }
            else
            {
                xCoord = Convert.ToString(x);
                xCoord = xCoord.PadLeft(6, '0');
            }
            if (y > 999999)
            {
                yCoord = "999999";
            }
            else
            {
                yCoord = Convert.ToString(y);
                yCoord = yCoord.PadLeft(6, '0');
            }
            if (z > 999999)
            {
                zCoord = "999999";
            }
            else
            {
                zCoord = Convert.ToString(z);
                zCoord = zCoord.PadLeft(6, '0');
            }

            finalString = "@wPX" + xCoord + "Y" + yCoord + "Z" + zCoord + ";";

            return finalString;
        }

        private string FormatLineCoordinates(uint x1, uint y1, uint x2, uint y2)
        {
            string xCoord1;
            string yCoord1;
            string xCoord2;
            string yCoord2;
            string finalString;

            if (x1 > 999999)
            {
                xCoord1 = "999999";
            }
            else
            {
                xCoord1 = Convert.ToString(x1);
                xCoord1 = xCoord1.PadLeft(6, '0');
            }

            if (y1 > 999999)
            {
                yCoord1 = "999999";
            }
            else
            {
                yCoord1 = Convert.ToString(y1);
                yCoord1 = yCoord1.PadLeft(6, '0');
            }

            if (x2 > 999999)
            {
                xCoord2 = "999999";
            }
            else
            {
                xCoord2 = Convert.ToString(x2);
                xCoord2 = xCoord2.PadLeft(6, '0');
            }

            if (y2 > 999999)
            {
                yCoord2 = "999999";
            }
            else
            {
                yCoord2 = Convert.ToString(y2);
                yCoord2 = yCoord2.PadLeft(6, '0');
            }

            finalString = "@L" + xCoord1 + yCoord1 + xCoord2 + yCoord2 + ";";

            return finalString;
        }

        public bool AddPointCoordinates(uint x, uint y, uint z)
        {
            string data;

            data = FormatPointCoordinates(x, y, z);

            CommandrichTextBox.AppendText(data);
            _commandList.Add(data);
            CommandrichTextBox.AppendText("\r");
            return true;
        }

        public bool AddLineCoordinates(uint x1, uint y1, uint x2, uint y2)
        {
            string data;

            data = FormatLineCoordinates(x1, y1, x2, y2);

            CommandrichTextBox.AppendText(data);
            _commandList.Add(data);
            CommandrichTextBox.AppendText("\r");
            return true;
        }

        public void UpdateCommandsTransfered(int nCurrentCommandIdx, int nNumCommands)
        {
            //this.TransferLabel.Text = "Transferred Commands " + nCurrentCommandIdx + " di " + nNumCommands;
            progressBarBatch.PerformStep();
        }

        private void AddLinebutton_Click(object sender, EventArgs e)
        {
            AddLineForm lineForm = new AddLineForm(this);
            this.Enabled = false;
        }

        private void Executebutton_Click(object sender, EventArgs e)
        {
            SendBatchCommandsDelegate batchDelegate = _parentForm.SendBatchCommands;
            string[] stringCommands = (string[])_commandList.ToArray(typeof(string));

            //TransferLabel.Visible = true;
            progressBarBatch.Visible = true;
            progressBarBatch.Maximum = stringCommands.Length;
            Executebutton.Enabled = false;
            Cancelbutton.Enabled = false;

            _parentForm.Invoke(batchDelegate, new object[] { stringCommands });
        }

        public void BatchTerminated(bool bValue)
        {
            Executebutton.Enabled = true;
            Cancelbutton.Enabled = true;
            //TransferLabel.Visible = false;
            progressBarBatch.Visible = false;

            if (bValue == true)
            {
                MessageBox.Show("Batch conclused", "Information",
                   MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                MessageBox.Show("Batch bad exit", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Savebutton_Click(object sender, EventArgs e)
        {
            SaveFileDialog saveScriptDialog = new SaveFileDialog();
            DialogResult result = saveScriptDialog.ShowDialog();
            if (result == DialogResult.OK)
            {
                string mydocpath = saveScriptDialog.FileName;

                string[] stringCommands = (string[])_commandList.ToArray(typeof(string));
                StringBuilder sb = new StringBuilder();

                foreach (var item in stringCommands)
                {
                    sb.AppendLine(item);
                }

                // Save file
                using (StreamWriter outfile = new StreamWriter(mydocpath, true))
                {
                    outfile.Write(sb.ToString());
                }
            }
            else if (result == DialogResult.Cancel)
            {
            }
            else
            {
                MessageBox.Show("Impossible save on this file", "Error",
                   MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void CommandrichTextBox_TextChanged(object sender, EventArgs e)
        {
            _bDirty = true;
        }
    }
}

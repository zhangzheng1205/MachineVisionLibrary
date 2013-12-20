namespace ClientCommunicationCNC
{
    partial class Form1
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.buttonSend = new System.Windows.Forms.Button();
            this.richTextBoxMessage = new System.Windows.Forms.RichTextBox();
            this.textBoxCommands = new System.Windows.Forms.TextBox();
            this.buttonClear = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.labelServerIP = new System.Windows.Forms.Label();
            this.labelSendCommand = new System.Windows.Forms.Label();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonSendData = new System.Windows.Forms.Button();
            this.textBoxSendDataPath = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonSend
            // 
            this.buttonSend.Location = new System.Drawing.Point(36, 567);
            this.buttonSend.Name = "buttonSend";
            this.buttonSend.Size = new System.Drawing.Size(125, 63);
            this.buttonSend.TabIndex = 0;
            this.buttonSend.Text = "Send data";
            this.buttonSend.UseVisualStyleBackColor = true;
            this.buttonSend.Click += new System.EventHandler(this.buttonSend_Click);
            // 
            // richTextBoxMessage
            // 
            this.richTextBoxMessage.Location = new System.Drawing.Point(12, 119);
            this.richTextBoxMessage.Name = "richTextBoxMessage";
            this.richTextBoxMessage.ReadOnly = true;
            this.richTextBoxMessage.Size = new System.Drawing.Size(753, 435);
            this.richTextBoxMessage.TabIndex = 1;
            this.richTextBoxMessage.Text = "";
            // 
            // textBoxCommands
            // 
            this.textBoxCommands.Location = new System.Drawing.Point(140, 39);
            this.textBoxCommands.Name = "textBoxCommands";
            this.textBoxCommands.Size = new System.Drawing.Size(625, 22);
            this.textBoxCommands.TabIndex = 2;
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(215, 567);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(127, 63);
            this.buttonClear.TabIndex = 3;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(191, 3);
            this.textBox1.Name = "textBox1";
            this.textBox1.Size = new System.Drawing.Size(574, 22);
            this.textBox1.TabIndex = 6;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // labelServerIP
            // 
            this.labelServerIP.AutoSize = true;
            this.labelServerIP.Location = new System.Drawing.Point(13, 9);
            this.labelServerIP.Name = "labelServerIP";
            this.labelServerIP.Size = new System.Drawing.Size(122, 17);
            this.labelServerIP.TabIndex = 7;
            this.labelServerIP.Text = "Server IP Address";
            // 
            // labelSendCommand
            // 
            this.labelSendCommand.AutoSize = true;
            this.labelSendCommand.Location = new System.Drawing.Point(13, 37);
            this.labelSendCommand.Name = "labelSendCommand";
            this.labelSendCommand.Size = new System.Drawing.Size(104, 17);
            this.labelSendCommand.TabIndex = 8;
            this.labelSendCommand.Text = "SendCommand";
            // 
            // buttonConnect
            // 
            this.buttonConnect.Location = new System.Drawing.Point(399, 567);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(133, 63);
            this.buttonConnect.TabIndex = 9;
            this.buttonConnect.Text = "Connect To Server";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonSendData
            // 
            this.buttonSendData.Location = new System.Drawing.Point(604, 567);
            this.buttonSendData.Name = "buttonSendData";
            this.buttonSendData.Size = new System.Drawing.Size(133, 62);
            this.buttonSendData.TabIndex = 10;
            this.buttonSendData.Text = "Send File";
            this.buttonSendData.UseVisualStyleBackColor = true;
            this.buttonSendData.Click += new System.EventHandler(this.buttonSendData_Click);
            // 
            // textBoxSendDataPath
            // 
            this.textBoxSendDataPath.Location = new System.Drawing.Point(95, 82);
            this.textBoxSendDataPath.Name = "textBoxSendDataPath";
            this.textBoxSendDataPath.Size = new System.Drawing.Size(670, 22);
            this.textBoxSendDataPath.TabIndex = 11;
            this.textBoxSendDataPath.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSendDataPath_KeyDown);
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.BackgroundImage = global::ClientCommunicationCNC.Properties.Resources.Library_load_icon;
            this.buttonSelectFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSelectFile.Location = new System.Drawing.Point(12, 69);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(54, 38);
            this.buttonSelectFile.TabIndex = 12;
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
            this.ClientSize = new System.Drawing.Size(781, 642);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.textBoxSendDataPath);
            this.Controls.Add(this.buttonSendData);
            this.Controls.Add(this.buttonConnect);
            this.Controls.Add(this.labelSendCommand);
            this.Controls.Add(this.labelServerIP);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.textBoxCommands);
            this.Controls.Add(this.richTextBoxMessage);
            this.Controls.Add(this.buttonSend);
            this.Name = "Form1";
            this.Text = "Client Application";
            this.Shown += new System.EventHandler(this.Form1_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form1_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSend;
        private System.Windows.Forms.RichTextBox richTextBoxMessage;
        private System.Windows.Forms.TextBox textBoxCommands;
        private System.Windows.Forms.Button buttonClear;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.Label labelServerIP;
        private System.Windows.Forms.Label labelSendCommand;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonSendData;
        private System.Windows.Forms.TextBox textBoxSendDataPath;
        private System.Windows.Forms.Button buttonSelectFile;
    }
}


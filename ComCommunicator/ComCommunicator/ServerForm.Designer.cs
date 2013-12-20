namespace ComCommunicator
{
    partial class ServerForm
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
            this.richTextBoxCommandReceived = new System.Windows.Forms.RichTextBox();
            this.buttonStart = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxClients = new System.Windows.Forms.ComboBox();
            this.labelNumClients = new System.Windows.Forms.Label();
            this.labelServerIP = new System.Windows.Forms.Label();
            this.textBoxServerIP = new System.Windows.Forms.TextBox();
            this.textBoxSendData = new System.Windows.Forms.TextBox();
            this.buttonSelectFile = new System.Windows.Forms.Button();
            this.buttonSendFile = new System.Windows.Forms.Button();
            this.buttonClear = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // richTextBoxCommandReceived
            // 
            this.richTextBoxCommandReceived.Location = new System.Drawing.Point(17, 78);
            this.richTextBoxCommandReceived.Name = "richTextBoxCommandReceived";
            this.richTextBoxCommandReceived.ReadOnly = true;
            this.richTextBoxCommandReceived.Size = new System.Drawing.Size(860, 524);
            this.richTextBoxCommandReceived.TabIndex = 0;
            this.richTextBoxCommandReceived.Text = "";
            // 
            // buttonStart
            // 
            this.buttonStart.Location = new System.Drawing.Point(121, 650);
            this.buttonStart.Name = "buttonStart";
            this.buttonStart.Size = new System.Drawing.Size(158, 59);
            this.buttonStart.TabIndex = 1;
            this.buttonStart.Text = "Start Server";
            this.buttonStart.UseVisualStyleBackColor = true;
            this.buttonStart.Click += new System.EventHandler(this.buttonStart_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(884, 206);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(115, 17);
            this.label1.TabIndex = 2;
            this.label1.Text = "Client registered:";
            // 
            // comboBoxClients
            // 
            this.comboBoxClients.FormattingEnabled = true;
            this.comboBoxClients.Location = new System.Drawing.Point(887, 241);
            this.comboBoxClients.Name = "comboBoxClients";
            this.comboBoxClients.Size = new System.Drawing.Size(148, 24);
            this.comboBoxClients.TabIndex = 3;
            // 
            // labelNumClients
            // 
            this.labelNumClients.AutoSize = true;
            this.labelNumClients.Location = new System.Drawing.Point(883, 170);
            this.labelNumClients.Name = "labelNumClients";
            this.labelNumClients.Size = new System.Drawing.Size(136, 17);
            this.labelNumClients.TabIndex = 4;
            this.labelNumClients.Text = "Number of Clients: 0";
            // 
            // labelServerIP
            // 
            this.labelServerIP.AutoSize = true;
            this.labelServerIP.Location = new System.Drawing.Point(884, 78);
            this.labelServerIP.Name = "labelServerIP";
            this.labelServerIP.Size = new System.Drawing.Size(74, 17);
            this.labelServerIP.TabIndex = 5;
            this.labelServerIP.Text = "Server IP: ";
            // 
            // textBoxServerIP
            // 
            this.textBoxServerIP.Location = new System.Drawing.Point(887, 112);
            this.textBoxServerIP.Name = "textBoxServerIP";
            this.textBoxServerIP.ReadOnly = true;
            this.textBoxServerIP.Size = new System.Drawing.Size(176, 22);
            this.textBoxServerIP.TabIndex = 6;
            // 
            // textBoxSendData
            // 
            this.textBoxSendData.Location = new System.Drawing.Point(111, 39);
            this.textBoxSendData.Name = "textBoxSendData";
            this.textBoxSendData.Size = new System.Drawing.Size(766, 22);
            this.textBoxSendData.TabIndex = 7;
            this.textBoxSendData.KeyDown += new System.Windows.Forms.KeyEventHandler(this.textBoxSendData_KeyDown);
            // 
            // buttonSelectFile
            // 
            this.buttonSelectFile.BackgroundImage = global::ComCommunicator.Properties.Resources.Library_load_icon;
            this.buttonSelectFile.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.buttonSelectFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.buttonSelectFile.Location = new System.Drawing.Point(17, 23);
            this.buttonSelectFile.Name = "buttonSelectFile";
            this.buttonSelectFile.Size = new System.Drawing.Size(62, 43);
            this.buttonSelectFile.TabIndex = 8;
            this.buttonSelectFile.UseVisualStyleBackColor = true;
            this.buttonSelectFile.Click += new System.EventHandler(this.buttonSelectFile_Click);
            // 
            // buttonSendFile
            // 
            this.buttonSendFile.Location = new System.Drawing.Point(433, 651);
            this.buttonSendFile.Name = "buttonSendFile";
            this.buttonSendFile.Size = new System.Drawing.Size(149, 58);
            this.buttonSendFile.TabIndex = 10;
            this.buttonSendFile.Text = "Send File";
            this.buttonSendFile.UseVisualStyleBackColor = true;
            this.buttonSendFile.Click += new System.EventHandler(this.buttonSendFile_Click);
            // 
            // buttonClear
            // 
            this.buttonClear.Location = new System.Drawing.Point(714, 651);
            this.buttonClear.Name = "buttonClear";
            this.buttonClear.Size = new System.Drawing.Size(148, 58);
            this.buttonClear.TabIndex = 11;
            this.buttonClear.Text = "Clear";
            this.buttonClear.UseVisualStyleBackColor = true;
            this.buttonClear.Click += new System.EventHandler(this.buttonClear_Click);
            // 
            // ServerForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1086, 721);
            this.Controls.Add(this.buttonClear);
            this.Controls.Add(this.buttonSendFile);
            this.Controls.Add(this.buttonSelectFile);
            this.Controls.Add(this.textBoxSendData);
            this.Controls.Add(this.textBoxServerIP);
            this.Controls.Add(this.labelServerIP);
            this.Controls.Add(this.labelNumClients);
            this.Controls.Add(this.comboBoxClients);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.buttonStart);
            this.Controls.Add(this.richTextBoxCommandReceived);
            this.Name = "ServerForm";
            this.Text = "ServerForm";
            this.Load += new System.EventHandler(this.ServerForm_Load);
            this.Shown += new System.EventHandler(this.ServerForm_Shown);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ServerForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBoxCommandReceived;
        private System.Windows.Forms.Button buttonStart;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxClients;
        private System.Windows.Forms.Label labelNumClients;
        private System.Windows.Forms.Label labelServerIP;
        public System.Windows.Forms.TextBox textBoxServerIP;
        private System.Windows.Forms.TextBox textBoxSendData;
        private System.Windows.Forms.Button buttonSelectFile;
        private System.Windows.Forms.Button buttonSendFile;
        private System.Windows.Forms.Button buttonClear;
    }
}
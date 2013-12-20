namespace ComCommunicator
{
    partial class CNC_Parameters
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
            this.buttonSetParameters = new System.Windows.Forms.Button();
            this.buttonRestoreDefault = new System.Windows.Forms.Button();
            this.labelMaxX = new System.Windows.Forms.Label();
            this.labelMaxY = new System.Windows.Forms.Label();
            this.labelMaxZ = new System.Windows.Forms.Label();
            this.textBoxMaxX = new System.Windows.Forms.TextBox();
            this.textBoxMaxY = new System.Windows.Forms.TextBox();
            this.textBoxMaxZ = new System.Windows.Forms.TextBox();
            this.buttonGetImageSize = new System.Windows.Forms.Button();
            this.textBoxSlowFrequency = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textBoxHighFrequency = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxMotorID = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.groupBoxMotorParameters = new System.Windows.Forms.GroupBox();
            this.label8 = new System.Windows.Forms.Label();
            this.textBoxDecDefault = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.textBoxHighDefault = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.textBoxMicroStepDefault = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.textBoxAccDefault = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.textBoxSlowDefault = new System.Windows.Forms.TextBox();
            this.labelMicroStep = new System.Windows.Forms.Label();
            this.textBoxMicroStep = new System.Windows.Forms.TextBox();
            this.labelDecStep = new System.Windows.Forms.Label();
            this.textBoxDecStep = new System.Windows.Forms.TextBox();
            this.labelAccStep = new System.Windows.Forms.Label();
            this.textBoxAcceleration = new System.Windows.Forms.TextBox();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textBoxDrillDiameter = new System.Windows.Forms.TextBox();
            this.buttonDrill = new System.Windows.Forms.Button();
            this.groupBoxMotorParameters.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonSetParameters
            // 
            this.buttonSetParameters.Location = new System.Drawing.Point(79, 384);
            this.buttonSetParameters.Name = "buttonSetParameters";
            this.buttonSetParameters.Size = new System.Drawing.Size(127, 49);
            this.buttonSetParameters.TabIndex = 14;
            this.buttonSetParameters.Text = "Set Parameters";
            this.buttonSetParameters.UseVisualStyleBackColor = true;
            this.buttonSetParameters.Click += new System.EventHandler(this.buttonSetParameters_Click);
            // 
            // buttonRestoreDefault
            // 
            this.buttonRestoreDefault.Location = new System.Drawing.Point(283, 384);
            this.buttonRestoreDefault.Name = "buttonRestoreDefault";
            this.buttonRestoreDefault.Size = new System.Drawing.Size(119, 49);
            this.buttonRestoreDefault.TabIndex = 15;
            this.buttonRestoreDefault.Text = "Restore Default";
            this.buttonRestoreDefault.UseVisualStyleBackColor = true;
            this.buttonRestoreDefault.Click += new System.EventHandler(this.buttonRestoreDefault_Click);
            // 
            // labelMaxX
            // 
            this.labelMaxX.AutoSize = true;
            this.labelMaxX.Location = new System.Drawing.Point(64, 36);
            this.labelMaxX.Name = "labelMaxX";
            this.labelMaxX.Size = new System.Drawing.Size(46, 17);
            this.labelMaxX.TabIndex = 16;
            this.labelMaxX.Text = "X Max";
            // 
            // labelMaxY
            // 
            this.labelMaxY.AutoSize = true;
            this.labelMaxY.Location = new System.Drawing.Point(317, 36);
            this.labelMaxY.Name = "labelMaxY";
            this.labelMaxY.Size = new System.Drawing.Size(46, 17);
            this.labelMaxY.TabIndex = 17;
            this.labelMaxY.Text = "Y Max";
            // 
            // labelMaxZ
            // 
            this.labelMaxZ.AutoSize = true;
            this.labelMaxZ.Location = new System.Drawing.Point(579, 36);
            this.labelMaxZ.Name = "labelMaxZ";
            this.labelMaxZ.Size = new System.Drawing.Size(46, 17);
            this.labelMaxZ.TabIndex = 18;
            this.labelMaxZ.Text = "Z Max";
            // 
            // textBoxMaxX
            // 
            this.textBoxMaxX.Enabled = false;
            this.textBoxMaxX.Location = new System.Drawing.Point(138, 36);
            this.textBoxMaxX.Name = "textBoxMaxX";
            this.textBoxMaxX.Size = new System.Drawing.Size(116, 22);
            this.textBoxMaxX.TabIndex = 19;
            // 
            // textBoxMaxY
            // 
            this.textBoxMaxY.Enabled = false;
            this.textBoxMaxY.Location = new System.Drawing.Point(383, 35);
            this.textBoxMaxY.Name = "textBoxMaxY";
            this.textBoxMaxY.Size = new System.Drawing.Size(116, 22);
            this.textBoxMaxY.TabIndex = 20;
            // 
            // textBoxMaxZ
            // 
            this.textBoxMaxZ.Enabled = false;
            this.textBoxMaxZ.Location = new System.Drawing.Point(644, 34);
            this.textBoxMaxZ.Name = "textBoxMaxZ";
            this.textBoxMaxZ.Size = new System.Drawing.Size(116, 22);
            this.textBoxMaxZ.TabIndex = 21;
            // 
            // buttonGetImageSize
            // 
            this.buttonGetImageSize.Location = new System.Drawing.Point(471, 385);
            this.buttonGetImageSize.Name = "buttonGetImageSize";
            this.buttonGetImageSize.Size = new System.Drawing.Size(123, 48);
            this.buttonGetImageSize.TabIndex = 22;
            this.buttonGetImageSize.Text = "Update Image Size";
            this.buttonGetImageSize.UseVisualStyleBackColor = true;
            this.buttonGetImageSize.Click += new System.EventHandler(this.buttonGetImageSize_Click);
            // 
            // textBoxSlowFrequency
            // 
            this.textBoxSlowFrequency.Location = new System.Drawing.Point(169, 32);
            this.textBoxSlowFrequency.Name = "textBoxSlowFrequency";
            this.textBoxSlowFrequency.Size = new System.Drawing.Size(110, 22);
            this.textBoxSlowFrequency.TabIndex = 27;
            this.textBoxSlowFrequency.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(8, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(108, 17);
            this.label1.TabIndex = 28;
            this.label1.Text = "Slow Frequency";
            // 
            // textBoxHighFrequency
            // 
            this.textBoxHighFrequency.Location = new System.Drawing.Point(137, 114);
            this.textBoxHighFrequency.Name = "textBoxHighFrequency";
            this.textBoxHighFrequency.Size = new System.Drawing.Size(114, 22);
            this.textBoxHighFrequency.TabIndex = 29;
            this.textBoxHighFrequency.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 113);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(108, 17);
            this.label2.TabIndex = 30;
            this.label2.Text = "High Frequency";
            // 
            // comboBoxMotorID
            // 
            this.comboBoxMotorID.FormattingEnabled = true;
            this.comboBoxMotorID.Location = new System.Drawing.Point(147, 329);
            this.comboBoxMotorID.Name = "comboBoxMotorID";
            this.comboBoxMotorID.Size = new System.Drawing.Size(116, 24);
            this.comboBoxMotorID.TabIndex = 31;
            this.comboBoxMotorID.SelectedIndexChanged += new System.EventHandler(this.comboBoxMotorID_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(18, 332);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(104, 17);
            this.label3.TabIndex = 32;
            this.label3.Text = "Select Motor ID";
            // 
            // groupBoxMotorParameters
            // 
            this.groupBoxMotorParameters.Controls.Add(this.label8);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxDecDefault);
            this.groupBoxMotorParameters.Controls.Add(this.label7);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxHighDefault);
            this.groupBoxMotorParameters.Controls.Add(this.label6);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxMicroStepDefault);
            this.groupBoxMotorParameters.Controls.Add(this.label5);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxAccDefault);
            this.groupBoxMotorParameters.Controls.Add(this.label4);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxSlowDefault);
            this.groupBoxMotorParameters.Controls.Add(this.labelMicroStep);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxMicroStep);
            this.groupBoxMotorParameters.Controls.Add(this.labelDecStep);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxDecStep);
            this.groupBoxMotorParameters.Controls.Add(this.labelAccStep);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxAcceleration);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxSlowFrequency);
            this.groupBoxMotorParameters.Controls.Add(this.label1);
            this.groupBoxMotorParameters.Controls.Add(this.textBoxHighFrequency);
            this.groupBoxMotorParameters.Controls.Add(this.label2);
            this.groupBoxMotorParameters.Location = new System.Drawing.Point(12, 110);
            this.groupBoxMotorParameters.Name = "groupBoxMotorParameters";
            this.groupBoxMotorParameters.Size = new System.Drawing.Size(832, 201);
            this.groupBoxMotorParameters.TabIndex = 33;
            this.groupBoxMotorParameters.TabStop = false;
            this.groupBoxMotorParameters.Text = "Motor Parameters";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(283, 152);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(170, 17);
            this.label8.TabIndex = 46;
            this.label8.Text = "Num Deceleration Default";
            // 
            // textBoxDecDefault
            // 
            this.textBoxDecDefault.Location = new System.Drawing.Point(459, 147);
            this.textBoxDecDefault.Name = "textBoxDecDefault";
            this.textBoxDecDefault.ReadOnly = true;
            this.textBoxDecDefault.Size = new System.Drawing.Size(120, 22);
            this.textBoxDecDefault.TabIndex = 45;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(12, 146);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(86, 17);
            this.label7.TabIndex = 44;
            this.label7.Text = "High Default";
            // 
            // textBoxHighDefault
            // 
            this.textBoxHighDefault.Location = new System.Drawing.Point(139, 150);
            this.textBoxHighDefault.Name = "textBoxHighDefault";
            this.textBoxHighDefault.ReadOnly = true;
            this.textBoxHighDefault.Size = new System.Drawing.Size(111, 22);
            this.textBoxHighDefault.TabIndex = 43;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(589, 72);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(124, 17);
            this.label6.TabIndex = 42;
            this.label6.Text = "Micro Step Default";
            // 
            // textBoxMicroStepDefault
            // 
            this.textBoxMicroStepDefault.Location = new System.Drawing.Point(715, 69);
            this.textBoxMicroStepDefault.Name = "textBoxMicroStepDefault";
            this.textBoxMicroStepDefault.ReadOnly = true;
            this.textBoxMicroStepDefault.Size = new System.Drawing.Size(107, 22);
            this.textBoxMicroStepDefault.TabIndex = 41;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(285, 66);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(168, 17);
            this.label5.TabIndex = 40;
            this.label5.Text = "Num Acceleration Default";
            // 
            // textBoxAccDefault
            // 
            this.textBoxAccDefault.Location = new System.Drawing.Point(458, 67);
            this.textBoxAccDefault.Name = "textBoxAccDefault";
            this.textBoxAccDefault.ReadOnly = true;
            this.textBoxAccDefault.Size = new System.Drawing.Size(122, 22);
            this.textBoxAccDefault.TabIndex = 39;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 63);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(157, 17);
            this.label4.TabIndex = 38;
            this.label4.Text = "Slow Frequency Default";
            // 
            // textBoxSlowDefault
            // 
            this.textBoxSlowDefault.Location = new System.Drawing.Point(169, 63);
            this.textBoxSlowDefault.Name = "textBoxSlowDefault";
            this.textBoxSlowDefault.ReadOnly = true;
            this.textBoxSlowDefault.Size = new System.Drawing.Size(110, 22);
            this.textBoxSlowDefault.TabIndex = 37;
            // 
            // labelMicroStep
            // 
            this.labelMicroStep.AutoSize = true;
            this.labelMicroStep.Location = new System.Drawing.Point(622, 38);
            this.labelMicroStep.Name = "labelMicroStep";
            this.labelMicroStep.Size = new System.Drawing.Size(75, 17);
            this.labelMicroStep.TabIndex = 36;
            this.labelMicroStep.Text = "Micro Step";
            // 
            // textBoxMicroStep
            // 
            this.textBoxMicroStep.Location = new System.Drawing.Point(715, 35);
            this.textBoxMicroStep.Name = "textBoxMicroStep";
            this.textBoxMicroStep.Size = new System.Drawing.Size(107, 22);
            this.textBoxMicroStep.TabIndex = 35;
            this.textBoxMicroStep.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelDecStep
            // 
            this.labelDecStep.AutoSize = true;
            this.labelDecStep.Location = new System.Drawing.Point(281, 115);
            this.labelDecStep.Name = "labelDecStep";
            this.labelDecStep.Size = new System.Drawing.Size(161, 17);
            this.labelDecStep.TabIndex = 34;
            this.labelDecStep.Text = "Num Deceleration Steps";
            // 
            // textBoxDecStep
            // 
            this.textBoxDecStep.Location = new System.Drawing.Point(458, 110);
            this.textBoxDecStep.Name = "textBoxDecStep";
            this.textBoxDecStep.Size = new System.Drawing.Size(123, 22);
            this.textBoxDecStep.TabIndex = 33;
            this.textBoxDecStep.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // labelAccStep
            // 
            this.labelAccStep.AutoSize = true;
            this.labelAccStep.Location = new System.Drawing.Point(285, 32);
            this.labelAccStep.Name = "labelAccStep";
            this.labelAccStep.Size = new System.Drawing.Size(159, 17);
            this.labelAccStep.TabIndex = 32;
            this.labelAccStep.Text = "Num Acceleration Steps";
            // 
            // textBoxAcceleration
            // 
            this.textBoxAcceleration.Location = new System.Drawing.Point(458, 32);
            this.textBoxAcceleration.Name = "textBoxAcceleration";
            this.textBoxAcceleration.Size = new System.Drawing.Size(124, 22);
            this.textBoxAcceleration.TabIndex = 31;
            this.textBoxAcceleration.TextChanged += new System.EventHandler(this.textBox_TextChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.textBoxMaxZ);
            this.groupBox1.Controls.Add(this.textBoxMaxY);
            this.groupBox1.Controls.Add(this.textBoxMaxX);
            this.groupBox1.Controls.Add(this.labelMaxZ);
            this.groupBox1.Controls.Add(this.labelMaxY);
            this.groupBox1.Controls.Add(this.labelMaxX);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(809, 76);
            this.groupBox1.TabIndex = 34;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Max CNC Size";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(318, 332);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(93, 17);
            this.label9.TabIndex = 35;
            this.label9.Text = "Drill Diameter";
            // 
            // textBoxDrillDiameter
            // 
            this.textBoxDrillDiameter.Location = new System.Drawing.Point(440, 332);
            this.textBoxDrillDiameter.Name = "textBoxDrillDiameter";
            this.textBoxDrillDiameter.Size = new System.Drawing.Size(115, 22);
            this.textBoxDrillDiameter.TabIndex = 36;
            // 
            // buttonDrill
            // 
            this.buttonDrill.Location = new System.Drawing.Point(665, 384);
            this.buttonDrill.Name = "buttonDrill";
            this.buttonDrill.Size = new System.Drawing.Size(120, 48);
            this.buttonDrill.TabIndex = 37;
            this.buttonDrill.Text = "Set Drill Diameter";
            this.buttonDrill.UseVisualStyleBackColor = true;
            this.buttonDrill.Click += new System.EventHandler(this.buttonDrill_Click);
            // 
            // CNC_Parameters
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(859, 458);
            this.Controls.Add(this.buttonDrill);
            this.Controls.Add(this.textBoxDrillDiameter);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.groupBoxMotorParameters);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.comboBoxMotorID);
            this.Controls.Add(this.buttonGetImageSize);
            this.Controls.Add(this.buttonRestoreDefault);
            this.Controls.Add(this.buttonSetParameters);
            this.Name = "CNC_Parameters";
            this.Text = "CNC_Parameters";
            this.Load += new System.EventHandler(this.CNC_Parameters_Load);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.CNC_Parameters_FormClosing);
            this.groupBoxMotorParameters.ResumeLayout(false);
            this.groupBoxMotorParameters.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonSetParameters;
        private System.Windows.Forms.Button buttonRestoreDefault;
        private System.Windows.Forms.Label labelMaxX;
        private System.Windows.Forms.Label labelMaxY;
        private System.Windows.Forms.Label labelMaxZ;
        private System.Windows.Forms.TextBox textBoxMaxX;
        private System.Windows.Forms.TextBox textBoxMaxY;
        private System.Windows.Forms.TextBox textBoxMaxZ;
        private System.Windows.Forms.Button buttonGetImageSize;
        private System.Windows.Forms.TextBox textBoxSlowFrequency;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textBoxHighFrequency;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxMotorID;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.GroupBox groupBoxMotorParameters;
        private System.Windows.Forms.TextBox textBoxAcceleration;
        private System.Windows.Forms.Label labelMicroStep;
        private System.Windows.Forms.TextBox textBoxMicroStep;
        private System.Windows.Forms.Label labelDecStep;
        private System.Windows.Forms.TextBox textBoxDecStep;
        private System.Windows.Forms.Label labelAccStep;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox textBoxMicroStepDefault;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox textBoxAccDefault;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox textBoxSlowDefault;
        private System.Windows.Forms.TextBox textBoxHighDefault;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox textBoxDecDefault;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textBoxDrillDiameter;
        private System.Windows.Forms.Button buttonDrill;
    }
}
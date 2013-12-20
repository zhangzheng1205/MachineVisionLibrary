namespace ComCommunicator
{
    partial class CreateBatch
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
            this.CommandrichTextBox = new System.Windows.Forms.RichTextBox();
            this.Savebutton = new System.Windows.Forms.Button();
            this.Executebutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.AddPointbutton = new System.Windows.Forms.Button();
            this.AddLinebutton = new System.Windows.Forms.Button();
            this.TransferLabel = new System.Windows.Forms.Label();
            this.progressBarBatch = new System.Windows.Forms.ProgressBar();
            this.SuspendLayout();
            // 
            // CommandrichTextBox
            // 
            this.CommandrichTextBox.Location = new System.Drawing.Point(4, 5);
            this.CommandrichTextBox.Name = "CommandrichTextBox";
            this.CommandrichTextBox.Size = new System.Drawing.Size(1013, 537);
            this.CommandrichTextBox.TabIndex = 0;
            this.CommandrichTextBox.Text = "";
            this.CommandrichTextBox.UseWaitCursor = true;
            this.CommandrichTextBox.TextChanged += new System.EventHandler(this.CommandrichTextBox_TextChanged);
            // 
            // Savebutton
            // 
            this.Savebutton.Location = new System.Drawing.Point(610, 559);
            this.Savebutton.Name = "Savebutton";
            this.Savebutton.Size = new System.Drawing.Size(130, 53);
            this.Savebutton.TabIndex = 1;
            this.Savebutton.Text = "Save Script";
            this.Savebutton.UseVisualStyleBackColor = true;
            this.Savebutton.Click += new System.EventHandler(this.Savebutton_Click);
            // 
            // Executebutton
            // 
            this.Executebutton.Location = new System.Drawing.Point(755, 560);
            this.Executebutton.Name = "Executebutton";
            this.Executebutton.Size = new System.Drawing.Size(127, 52);
            this.Executebutton.TabIndex = 2;
            this.Executebutton.Text = "Execute";
            this.Executebutton.UseVisualStyleBackColor = true;
            this.Executebutton.Click += new System.EventHandler(this.Executebutton_Click);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Location = new System.Drawing.Point(900, 559);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(117, 53);
            this.Cancelbutton.TabIndex = 3;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // AddPointbutton
            // 
            this.AddPointbutton.Location = new System.Drawing.Point(12, 559);
            this.AddPointbutton.Name = "AddPointbutton";
            this.AddPointbutton.Size = new System.Drawing.Size(139, 52);
            this.AddPointbutton.TabIndex = 4;
            this.AddPointbutton.Text = "Add Point";
            this.AddPointbutton.UseVisualStyleBackColor = true;
            this.AddPointbutton.Click += new System.EventHandler(this.AddPointbutton_Click);
            // 
            // AddLinebutton
            // 
            this.AddLinebutton.Location = new System.Drawing.Point(182, 561);
            this.AddLinebutton.Name = "AddLinebutton";
            this.AddLinebutton.Size = new System.Drawing.Size(130, 50);
            this.AddLinebutton.TabIndex = 5;
            this.AddLinebutton.Text = "Add Line";
            this.AddLinebutton.UseVisualStyleBackColor = true;
            this.AddLinebutton.Click += new System.EventHandler(this.AddLinebutton_Click);
            // 
            // TransferLabel
            // 
            this.TransferLabel.AutoSize = true;
            this.TransferLabel.Location = new System.Drawing.Point(380, 571);
            this.TransferLabel.Name = "TransferLabel";
            this.TransferLabel.Size = new System.Drawing.Size(0, 17);
            this.TransferLabel.TabIndex = 6;
            // 
            // progressBarBatch
            // 
            this.progressBarBatch.Location = new System.Drawing.Point(366, 566);
            this.progressBarBatch.Name = "progressBarBatch";
            this.progressBarBatch.Size = new System.Drawing.Size(185, 45);
            this.progressBarBatch.TabIndex = 7;
            // 
            // CreateBatch
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(120F, 120F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.ClientSize = new System.Drawing.Size(1030, 624);
            this.Controls.Add(this.progressBarBatch);
            this.Controls.Add(this.TransferLabel);
            this.Controls.Add(this.AddLinebutton);
            this.Controls.Add(this.AddPointbutton);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.Executebutton);
            this.Controls.Add(this.Savebutton);
            this.Controls.Add(this.CommandrichTextBox);
            this.Name = "CreateBatch";
            this.Text = "CreateBatch";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CreateBatch_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox CommandrichTextBox;
        private System.Windows.Forms.Button Savebutton;
        private System.Windows.Forms.Button Executebutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Button AddPointbutton;
        private System.Windows.Forms.Button AddLinebutton;
        private System.Windows.Forms.Label TransferLabel;
        private System.Windows.Forms.ProgressBar progressBarBatch;
    }
}
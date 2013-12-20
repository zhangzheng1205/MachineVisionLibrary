namespace ComCommunicator
{
    partial class AddPointForm
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
            this.xCoordtextBox = new System.Windows.Forms.TextBox();
            this.yCoordtextBox = new System.Windows.Forms.TextBox();
            this.Addbutton = new System.Windows.Forms.Button();
            this.Cancelbutton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.zCoordtextBox = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // xCoordtextBox
            // 
            this.xCoordtextBox.Location = new System.Drawing.Point(27, 50);
            this.xCoordtextBox.Name = "xCoordtextBox";
            this.xCoordtextBox.Size = new System.Drawing.Size(81, 22);
            this.xCoordtextBox.TabIndex = 0;
            this.xCoordtextBox.Text = "0";
            this.xCoordtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.xCoordtextBox.TextChanged += new System.EventHandler(this.xCoordtextBox_TextChanged);
            this.xCoordtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.xCoordtextBox_KeyDown);
            // 
            // yCoordtextBox
            // 
            this.yCoordtextBox.Location = new System.Drawing.Point(149, 50);
            this.yCoordtextBox.Name = "yCoordtextBox";
            this.yCoordtextBox.Size = new System.Drawing.Size(89, 22);
            this.yCoordtextBox.TabIndex = 1;
            this.yCoordtextBox.Text = "0";
            this.yCoordtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.yCoordtextBox.TextChanged += new System.EventHandler(this.yCoordtextBox_TextChanged);
            this.yCoordtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.yCoordtextBox_KeyDown);
            // 
            // Addbutton
            // 
            this.Addbutton.Location = new System.Drawing.Point(59, 110);
            this.Addbutton.Name = "Addbutton";
            this.Addbutton.Size = new System.Drawing.Size(90, 43);
            this.Addbutton.TabIndex = 2;
            this.Addbutton.Text = "Add";
            this.Addbutton.UseVisualStyleBackColor = true;
            this.Addbutton.Click += new System.EventHandler(this.Addbutton_Click);
            this.Addbutton.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Addbutton_KeyDown);
            // 
            // Cancelbutton
            // 
            this.Cancelbutton.Location = new System.Drawing.Point(220, 112);
            this.Cancelbutton.Name = "Cancelbutton";
            this.Cancelbutton.Size = new System.Drawing.Size(94, 39);
            this.Cancelbutton.TabIndex = 3;
            this.Cancelbutton.Text = "Cancel";
            this.Cancelbutton.UseVisualStyleBackColor = true;
            this.Cancelbutton.Click += new System.EventHandler(this.Cancelbutton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(62, 20);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(17, 17);
            this.label1.TabIndex = 4;
            this.label1.Text = "X";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(184, 20);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(17, 17);
            this.label2.TabIndex = 5;
            this.label2.Text = "Y";
            // 
            // zCoordtextBox
            // 
            this.zCoordtextBox.Location = new System.Drawing.Point(279, 50);
            this.zCoordtextBox.Name = "zCoordtextBox";
            this.zCoordtextBox.Size = new System.Drawing.Size(90, 22);
            this.zCoordtextBox.TabIndex = 6;
            this.zCoordtextBox.Text = "0";
            this.zCoordtextBox.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.zCoordtextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.zCoordtextBox.KeyDown += new System.Windows.Forms.KeyEventHandler(this.zCoordtextBox_KeyDown);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(311, 20);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(17, 17);
            this.label3.TabIndex = 7;
            this.label3.Text = "Z";
            // 
            // AddPointForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(404, 180);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.zCoordtextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Cancelbutton);
            this.Controls.Add(this.Addbutton);
            this.Controls.Add(this.yCoordtextBox);
            this.Controls.Add(this.xCoordtextBox);
            this.Name = "AddPointForm";
            this.Text = "AddPointForm";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.AddPointForm_FormClosed);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox xCoordtextBox;
        private System.Windows.Forms.TextBox yCoordtextBox;
        private System.Windows.Forms.Button Addbutton;
        private System.Windows.Forms.Button Cancelbutton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox zCoordtextBox;
        private System.Windows.Forms.Label label3;
    }
}
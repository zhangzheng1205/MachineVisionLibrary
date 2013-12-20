namespace ComCommunicator
{
    partial class CNC_Design_Objects
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
            this.components = new System.ComponentModel.Container();
            this.buttonDrawLine = new System.Windows.Forms.Button();
            this.pictureBoxMain = new System.Windows.Forms.PictureBox();
            this.labelCurrentMousePosition = new System.Windows.Forms.Label();
            this.labelCurrentAction = new System.Windows.Forms.Label();
            this.buttonRectangle = new System.Windows.Forms.Button();
            this.buttonCircle = new System.Windows.Forms.Button();
            this.buttonBorderStyle = new System.Windows.Forms.Button();
            this.textBoxThickness = new System.Windows.Forms.TextBox();
            this.labelThickness = new System.Windows.Forms.Label();
            this.labelX = new System.Windows.Forms.Label();
            this.labelY = new System.Windows.Forms.Label();
            this.labelThicknessMin = new System.Windows.Forms.Label();
            this.textBoxMinThickness = new System.Windows.Forms.TextBox();
            this.labelStartPosition = new System.Windows.Forms.Label();
            this.contextMenuStrip1 = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.deleteAllToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.deleteElementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.treeViewObjects = new System.Windows.Forms.TreeView();
            this.labelObjects = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).BeginInit();
            this.contextMenuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // buttonDrawLine
            // 
            this.buttonDrawLine.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonDrawLine.Location = new System.Drawing.Point(58, 10);
            this.buttonDrawLine.Margin = new System.Windows.Forms.Padding(2);
            this.buttonDrawLine.Name = "buttonDrawLine";
            this.buttonDrawLine.Size = new System.Drawing.Size(49, 32);
            this.buttonDrawLine.TabIndex = 1;
            this.buttonDrawLine.Text = "Line";
            this.buttonDrawLine.UseVisualStyleBackColor = true;
            this.buttonDrawLine.Click += new System.EventHandler(this.buttonDrawLine_Click);
            // 
            // pictureBoxMain
            // 
            this.pictureBoxMain.BackColor = System.Drawing.SystemColors.Window;
            this.pictureBoxMain.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pictureBoxMain.Location = new System.Drawing.Point(62, 46);
            this.pictureBoxMain.Margin = new System.Windows.Forms.Padding(2);
            this.pictureBoxMain.Name = "pictureBoxMain";
            this.pictureBoxMain.Size = new System.Drawing.Size(699, 373);
            this.pictureBoxMain.TabIndex = 0;
            this.pictureBoxMain.TabStop = false;
            this.pictureBoxMain.MouseMove += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMain_MouseMove);
            this.pictureBoxMain.MouseClick += new System.Windows.Forms.MouseEventHandler(this.pictureBoxMain_MouseClick);
            // 
            // labelCurrentMousePosition
            // 
            this.labelCurrentMousePosition.AutoSize = true;
            this.labelCurrentMousePosition.Location = new System.Drawing.Point(674, 444);
            this.labelCurrentMousePosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentMousePosition.Name = "labelCurrentMousePosition";
            this.labelCurrentMousePosition.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentMousePosition.TabIndex = 2;
            // 
            // labelCurrentAction
            // 
            this.labelCurrentAction.AutoSize = true;
            this.labelCurrentAction.Location = new System.Drawing.Point(63, 443);
            this.labelCurrentAction.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelCurrentAction.Name = "labelCurrentAction";
            this.labelCurrentAction.Size = new System.Drawing.Size(0, 13);
            this.labelCurrentAction.TabIndex = 3;
            // 
            // buttonRectangle
            // 
            this.buttonRectangle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonRectangle.Location = new System.Drawing.Point(117, 10);
            this.buttonRectangle.Margin = new System.Windows.Forms.Padding(2);
            this.buttonRectangle.Name = "buttonRectangle";
            this.buttonRectangle.Size = new System.Drawing.Size(48, 32);
            this.buttonRectangle.TabIndex = 4;
            this.buttonRectangle.Text = "Rect";
            this.buttonRectangle.UseVisualStyleBackColor = true;
            this.buttonRectangle.Click += new System.EventHandler(this.buttonRectangle_Click);
            // 
            // buttonCircle
            // 
            this.buttonCircle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonCircle.Location = new System.Drawing.Point(176, 10);
            this.buttonCircle.Margin = new System.Windows.Forms.Padding(2);
            this.buttonCircle.Name = "buttonCircle";
            this.buttonCircle.Size = new System.Drawing.Size(50, 32);
            this.buttonCircle.TabIndex = 5;
            this.buttonCircle.Text = "Circle";
            this.buttonCircle.UseVisualStyleBackColor = true;
            this.buttonCircle.Click += new System.EventHandler(this.buttonCircle_Click);
            // 
            // buttonBorderStyle
            // 
            this.buttonBorderStyle.FlatStyle = System.Windows.Forms.FlatStyle.System;
            this.buttonBorderStyle.Location = new System.Drawing.Point(604, 11);
            this.buttonBorderStyle.Margin = new System.Windows.Forms.Padding(2);
            this.buttonBorderStyle.Name = "buttonBorderStyle";
            this.buttonBorderStyle.Size = new System.Drawing.Size(51, 31);
            this.buttonBorderStyle.TabIndex = 6;
            this.buttonBorderStyle.Text = "Border";
            this.buttonBorderStyle.UseVisualStyleBackColor = true;
            this.buttonBorderStyle.Click += new System.EventHandler(this.buttonBorderStyle_Click);
            // 
            // textBoxThickness
            // 
            this.textBoxThickness.Location = new System.Drawing.Point(345, 17);
            this.textBoxThickness.Name = "textBoxThickness";
            this.textBoxThickness.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.textBoxThickness.Size = new System.Drawing.Size(48, 20);
            this.textBoxThickness.TabIndex = 7;
            this.textBoxThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.textBoxThickness.TextChanged += new System.EventHandler(this.textBoxThickness_TextChanged);
            // 
            // labelThickness
            // 
            this.labelThickness.AutoSize = true;
            this.labelThickness.Location = new System.Drawing.Point(258, 21);
            this.labelThickness.Name = "labelThickness";
            this.labelThickness.Size = new System.Drawing.Size(81, 13);
            this.labelThickness.TabIndex = 8;
            this.labelThickness.Text = "Thickness (mm)";
            // 
            // labelX
            // 
            this.labelX.AutoSize = true;
            this.labelX.Location = new System.Drawing.Point(743, 421);
            this.labelX.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelX.Name = "labelX";
            this.labelX.Size = new System.Drawing.Size(13, 13);
            this.labelX.TabIndex = 9;
            this.labelX.Text = "0";
            // 
            // labelY
            // 
            this.labelY.AutoSize = true;
            this.labelY.Location = new System.Drawing.Point(30, 43);
            this.labelY.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelY.Name = "labelY";
            this.labelY.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.labelY.Size = new System.Drawing.Size(13, 13);
            this.labelY.TabIndex = 10;
            this.labelY.Text = "0";
            // 
            // labelThicknessMin
            // 
            this.labelThicknessMin.AutoSize = true;
            this.labelThicknessMin.Location = new System.Drawing.Point(428, 21);
            this.labelThicknessMin.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelThicknessMin.Name = "labelThicknessMin";
            this.labelThicknessMin.Size = new System.Drawing.Size(101, 13);
            this.labelThicknessMin.TabIndex = 11;
            this.labelThicknessMin.Text = "Min Thickness (mm)";
            // 
            // textBoxMinThickness
            // 
            this.textBoxMinThickness.Location = new System.Drawing.Point(538, 20);
            this.textBoxMinThickness.Margin = new System.Windows.Forms.Padding(2);
            this.textBoxMinThickness.Name = "textBoxMinThickness";
            this.textBoxMinThickness.ReadOnly = true;
            this.textBoxMinThickness.Size = new System.Drawing.Size(42, 20);
            this.textBoxMinThickness.TabIndex = 12;
            this.textBoxMinThickness.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            // 
            // labelStartPosition
            // 
            this.labelStartPosition.AutoSize = true;
            this.labelStartPosition.Location = new System.Drawing.Point(44, 418);
            this.labelStartPosition.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.labelStartPosition.Name = "labelStartPosition";
            this.labelStartPosition.Size = new System.Drawing.Size(31, 13);
            this.labelStartPosition.TabIndex = 13;
            this.labelStartPosition.Text = "(0, 0)";
            // 
            // contextMenuStrip1
            // 
            this.contextMenuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.deleteAllToolStripMenuItem,
            this.deleteElementToolStripMenuItem});
            this.contextMenuStrip1.Name = "contextMenuStrip1";
            this.contextMenuStrip1.Size = new System.Drawing.Size(154, 48);
            // 
            // deleteAllToolStripMenuItem
            // 
            this.deleteAllToolStripMenuItem.Name = "deleteAllToolStripMenuItem";
            this.deleteAllToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteAllToolStripMenuItem.Text = "Delete all";
            this.deleteAllToolStripMenuItem.Click += new System.EventHandler(this.deleteAllToolStripMenuItem_Click);
            // 
            // deleteElementToolStripMenuItem
            // 
            this.deleteElementToolStripMenuItem.Name = "deleteElementToolStripMenuItem";
            this.deleteElementToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.deleteElementToolStripMenuItem.Text = "Delete Element";
            this.deleteElementToolStripMenuItem.Click += new System.EventHandler(this.deleteElementToolStripMenuItem_Click);
            // 
            // treeViewObjects
            // 
            this.treeViewObjects.Location = new System.Drawing.Point(783, 46);
            this.treeViewObjects.Name = "treeViewObjects";
            this.treeViewObjects.Size = new System.Drawing.Size(219, 373);
            this.treeViewObjects.TabIndex = 14;
            // 
            // labelObjects
            // 
            this.labelObjects.AutoSize = true;
            this.labelObjects.Location = new System.Drawing.Point(780, 24);
            this.labelObjects.Name = "labelObjects";
            this.labelObjects.Size = new System.Drawing.Size(94, 13);
            this.labelObjects.TabIndex = 15;
            this.labelObjects.Text = "Objects created: 0";
            // 
            // CNC_Design_Objects
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1014, 461);
            this.Controls.Add(this.labelObjects);
            this.Controls.Add(this.labelStartPosition);
            this.Controls.Add(this.textBoxMinThickness);
            this.Controls.Add(this.treeViewObjects);
            this.Controls.Add(this.labelThicknessMin);
            this.Controls.Add(this.labelY);
            this.Controls.Add(this.labelX);
            this.Controls.Add(this.labelThickness);
            this.Controls.Add(this.textBoxThickness);
            this.Controls.Add(this.buttonBorderStyle);
            this.Controls.Add(this.buttonCircle);
            this.Controls.Add(this.buttonRectangle);
            this.Controls.Add(this.labelCurrentAction);
            this.Controls.Add(this.labelCurrentMousePosition);
            this.Controls.Add(this.buttonDrawLine);
            this.Controls.Add(this.pictureBoxMain);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "CNC_Design_Objects";
            this.Text = "CNC_Design_Objects";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.CNC_Design_Objects_FormClosed);
            this.ResizeEnd += new System.EventHandler(this.CNC_Design_Objects_ResizeEnd);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBoxMain)).EndInit();
            this.contextMenuStrip1.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button buttonDrawLine;
        private System.Windows.Forms.PictureBox pictureBoxMain;
        private System.Windows.Forms.Label labelCurrentMousePosition;
        private System.Windows.Forms.Label labelCurrentAction;
        private System.Windows.Forms.Button buttonRectangle;
        private System.Windows.Forms.Button buttonCircle;
        private System.Windows.Forms.Button buttonBorderStyle;
        private System.Windows.Forms.TextBox textBoxThickness;
        private System.Windows.Forms.Label labelThickness;
        private System.Windows.Forms.Label labelX;
        private System.Windows.Forms.Label labelY;
        private System.Windows.Forms.Label labelThicknessMin;
        private System.Windows.Forms.TextBox textBoxMinThickness;
        private System.Windows.Forms.Label labelStartPosition;
        private System.Windows.Forms.ContextMenuStrip contextMenuStrip1;
        private System.Windows.Forms.ToolStripMenuItem deleteAllToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem deleteElementToolStripMenuItem;
        private System.Windows.Forms.TreeView treeViewObjects;
        private System.Windows.Forms.Label labelObjects;
    }
}
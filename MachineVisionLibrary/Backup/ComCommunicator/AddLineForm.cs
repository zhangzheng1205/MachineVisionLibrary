using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComCommunicator
{
    public partial class AddLineForm : Form
    {
        private CreateBatch _parentForm;
        private uint _x1 = 0, _y1 = 0;
        private uint _x2 = 0, _y2 = 0;

        public AddLineForm(CreateBatch form)
        {
            InitializeComponent();

            _parentForm = form;
            _parentForm.Enabled = false;
            this.Focus();
            this.Show();
        }

        private void Coordinates_TextChanged(object sender, EventArgs e)
        {
            uint data = 0;
            TextBox textBox;

            if (sender == textBox1)
            {
                textBox = textBox1;
            }
            else if (sender == textBox2)
            {
                textBox = textBox2;
            }
            else if (sender == textBox3)
            {
                textBox = textBox3;
            }
            else if (sender == textBox4)
            {
                textBox = textBox4;
            }
            else
            {
                return;
            }

            try
            {
                data = Convert.ToUInt32(textBox.Text);
            }
            catch (FormatException excp)
            {
                textBox.Text = "0";
            }
            catch (OverflowException excp)
            {
                textBox.Text = "0";
            }
            finally
            {
                if (data > Int32.MaxValue)
                {
                    textBox.Text = "0";
                }
            }

            if (sender == textBox1)
            {
                _x1 = data;
            }
            else if (sender == textBox2)
            {
                _y1 = data;
            }
            else if (sender == textBox3)
            {
                _x2 = data;
            }
            else if (sender == textBox4)
            {
                _y2 = data;
            }

        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            _parentForm.AddLineCoordinates(_x1, _y1, _x2, _y2);
            _parentForm.Enabled = true;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            _parentForm.Enabled = true;
        }

        private void AddLineForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parentForm.Enabled = true;
        }

        private void textBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parentForm.AddLineCoordinates(_x1, _y1, _x2, _y2);
                _parentForm.Enabled = true;
            }
        }
    }
}

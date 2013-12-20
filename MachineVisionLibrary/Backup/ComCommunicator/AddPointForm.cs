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
    public partial class AddPointForm : Form
    {
        private CreateBatch _parentForm;
        private uint _xCoord = 0;
        private uint _yCoord = 0;
        private uint _zCoord = 0;

        public AddPointForm(CreateBatch form)
        {
            InitializeComponent();

            _parentForm = form;
            _parentForm.Enabled = false;
            this.Focus();
            this.Show();
        }

        private void xCoordtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _xCoord = Convert.ToUInt32(xCoordtextBox.Text);
            }
            catch (FormatException excp)
            {
                xCoordtextBox.Text = "0";
            }
            catch (OverflowException excp)
            {
                xCoordtextBox.Text = "0";
            }
            finally
            {
                if (_xCoord > Int32.MaxValue)
                {
                    xCoordtextBox.Text = "0";
                }
            }
        }

        private void Addbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            _parentForm.AddPointCoordinates(_xCoord, _yCoord, _zCoord);
            _parentForm.Enabled = true;
        }

        private void Cancelbutton_Click(object sender, EventArgs e)
        {
            this.Close();
            _parentForm.Enabled = true;
        }

        private void AddPointForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parentForm.Enabled = true;
        }

        private void yCoordtextBox_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _yCoord = Convert.ToUInt32(yCoordtextBox.Text);
            }
            catch (FormatException excp)
            {
                yCoordtextBox.Text = "0";
            }
            catch (OverflowException excp)
            {
                yCoordtextBox.Text = "0";
            }
            finally
            {
                if (_yCoord > Int32.MaxValue)
                {
                    yCoordtextBox.Text = "0";
                }
            }
        }

        private void yCoordtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parentForm.AddPointCoordinates(_xCoord, _yCoord, _zCoord);
                _parentForm.Enabled = true;
            }
        }

        private void xCoordtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parentForm.AddPointCoordinates(_xCoord, _yCoord, _zCoord);
                _parentForm.Enabled = true;
            }
        }

        private void Addbutton_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parentForm.AddPointCoordinates(_xCoord, _yCoord, _zCoord);
                _parentForm.Enabled = true;
            }
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            try
            {
                _zCoord = Convert.ToUInt32(zCoordtextBox.Text);
            }
            catch (FormatException excp)
            {
                zCoordtextBox.Text = "0";
            }
            catch (OverflowException excp)
            {
                zCoordtextBox.Text = "0";
            }
            finally
            {
                if (_zCoord > Int32.MaxValue)
                {
                    zCoordtextBox.Text = "0";
                }
            }
        }

        private void zCoordtextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                this.Close();
                _parentForm.AddPointCoordinates(_xCoord, _yCoord, _zCoord);
                _parentForm.Enabled = true;
            }
        }
    }
}

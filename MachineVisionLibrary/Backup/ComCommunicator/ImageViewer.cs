using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ComCommunicator
{
    public partial class ImageViewer : Form
    {
        private Form1 _parentForm;
        private string _imagePath;
        private Bitmap _imageBitMap;
        private int _xLeft, _yUp;
        private int _xRight, _yDown;
        private List<Bitmap> BitmapHistory = new List<Bitmap>();
        private Graphics _graphic;
        private GraphicsState _graphicState;
        private bool _bDrawing = false;

        private CRectangle _Rectangle = new CRectangle();

        public ImageViewer(Form1 form, string sImage)
        {
            InitializeComponent();

            _parentForm = form;
            _imagePath = sImage;
        }

        public void ShowImage()
        {
            if (_imageBitMap != null)
            {
                _imageBitMap.Dispose();
            }
            //pictureBox1.SizeMode = PictureBoxSizeMode.StretchImage;
            _imageBitMap = new Bitmap(_imagePath);
            pictureBox1.ClientSize = new Size(_imageBitMap.Width, _imageBitMap.Height);
            pictureBox1.Image = (Image)_imageBitMap;

            BitmapHistory.Add(_imageBitMap);

            _graphic = pictureBox1.CreateGraphics();
        }

        private void ImageViewer_Deactivate(object sender, EventArgs e)
        {
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }

        private void ResetZoomHistory()
        {
            _xLeft = -1;
            _yUp = -1;
            _xRight = -1;
            _yDown = -1;
        }

        private void ZoomImage(ref Bitmap bmp, int xLeft, int YUp, int xRight, int YDown)
        {
            float zoomFactorX = (float)bmp.Width / (xRight - xLeft);
            float zoomFactorY = (float)bmp.Height / (YDown - YUp);

            _bDrawing = true;

            Size newSize = new Size((int)(bmp.Width * zoomFactorX), (int)(bmp.Height * zoomFactorY));
            try
            {
                Bitmap Newbmp = new Bitmap(bmp, newSize);
                pictureBox1.Image = (Image)Newbmp;
                _imageBitMap = Newbmp;
                BitmapHistory.Add(Newbmp);
            }
            catch (Exception exception)
            {
                //MessageBox.Show("No zoom available");
            }

            _bDrawing = false;
        }


        private void pictureBox1_MouseClick(object sender, MouseEventArgs e)
        {

        }

        private void pictureBox1_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _xLeft = e.X;
                _yUp = e.Y;
                _Rectangle._first._nX = e.X;
                _Rectangle._first._nY = e.Y;
            }
            else if (e.Button == MouseButtons.Right)
            {
                if (BitmapHistory.Count > 1)
                {
                    BitmapHistory.RemoveAt(BitmapHistory.Count - 1);
                    _imageBitMap = BitmapHistory.ElementAt(BitmapHistory.Count - 1);
                    pictureBox1.Image = _imageBitMap;
                }
            }
        }

        private void pictureBox1_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                _xRight = e.X;
                _yDown = e.Y;

                if (_xLeft != -1 &&
                _yUp != -1 &&
                _xRight != -1 &&
                _yDown != -1)
                {
                    // Create pen.
                    Pen blackPen = new Pen(Color.Black, 3);

                    // Create location and size of rectangle.
                    float x = _xLeft;
                    float y = _yUp;
                    float width = e.X - _xLeft;
                    float height = e.Y - _yUp;

                    // Draw rectangle to screen.
                    if (_graphicState != null)
                    {
                        //_graphic.Restore(_graphicState);
                    }

                    //ZoomImage(ref _imageBitMap, _xLeft, _yUp, _xRight, _yDown);
                    ZoomImage(ref _imageBitMap, _Rectangle._first._nX, _Rectangle._first._nY, _Rectangle._second._nX, _Rectangle._second._nY);
                }
            }
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {

        }

        private void pictureBox1_MouseMove(object sender, MouseEventArgs e)
        {
            if (_bDrawing == true)
            {
                return;
            }

            if (e.Button == MouseButtons.None)
            {
                label1.Text = "X=" + e.X + " Y=" + e.Y + " Pixel Value=" + " " + _imageBitMap.GetPixel(e.X, e.Y);
            }
            else
            {
                if ((e.Button == MouseButtons.Left) && (_xLeft > -1) && (_yUp > -1))
                {
                    // Create pen.
                    _graphicState = _graphic.Save();

                    pictureBox1.Image = (Image)_imageBitMap;

                    // Draw rectangle to screen.
                    _Rectangle._second._nX = e.X;
                    _Rectangle._second._nY = e.Y;
                    _Rectangle.Draw(_graphic, Color.Green, new Pen(Color.Green, 5));

                    label1.Text = "Draw Rectangle: X1 = " + Convert.ToString(_Rectangle._first._nX) + ", Y1 = " + Convert.ToString(_Rectangle._first._nY)
                                                + ", X2 = " + Convert.ToString(e.X) + ", Y2 = " + Convert.ToString(e.Y);
                }
            }
        }

        private void ImageViewer_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Enabled = false;
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }
    }
}

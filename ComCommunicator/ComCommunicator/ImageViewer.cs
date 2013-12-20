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

        // this tracks the transformation applied to the PictureBox's Graphics for wheelmouse scroll
        private Matrix transform = new Matrix();
        public static float s_dScrollValue = (float) .01;

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
            //pictureBox1.SizeMode = PictureBoxSizeMode.AutoSize;
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

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void panel1_Paint(object sender, PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.Transform = transform;
            SetScrollBarValues();
        }

        private void pictureBox1_Paint(object sender, PaintEventArgs e)
        {
            
        }

        protected override void OnMouseWheel(MouseEventArgs mea)
        {
            pictureBox1.Focus();
            if (pictureBox1.Focused == true && mea.Delta != 0)
            {
                ZoomScroll(mea.Location, mea.Delta > 0);
            }
        }

        //FUNCTION FOR MOUSE SCROL ZOOM-IN
        private void ZoomScroll(Point location, bool zoomIn)
        {
            // make zoom-point (cursor location) our origin
            transform.Translate(-location.X, -location.Y);

            // perform zoom (at origin)
            if (zoomIn)
            {
                transform.Scale( s_dScrollValue,  s_dScrollValue);
            }
            else
            {
                transform.Scale( -s_dScrollValue,  -s_dScrollValue);
            }

            // translate origin back to cursor
            transform.Translate(location.X, location.Y);

            panel1.Invalidate();
        }

        //Function to manage the scroll bar event
        private void ScrollBar_Scroll(object sender, System.Windows.Forms.ScrollEventArgs e)
        {
            //Create a graphics object and draw a portion of the image in the PictureBox.
            Graphics g = pictureBox1.CreateGraphics();

            int xWidth = pictureBox1.Width;
            int yHeight = pictureBox1.Height;

            int x;
            int y;

            if (e.ScrollOrientation == ScrollOrientation.HorizontalScroll)
            {
                x = e.NewValue;
                y = vScrollBar1.Value;
            }
            else //e.ScrollOrientation == ScrollOrientation.VerticalScroll
            {
                y = e.NewValue;
                x = hScrollBar1.Value;
            }

            g.DrawImage(pictureBox1.Image,
             new Rectangle(0, 0, xWidth, yHeight),  //where to draw the image 
             new Rectangle(x, y, xWidth, yHeight),  //the portion of the image to draw
            GraphicsUnit.Pixel);

            panel1.Invalidate();
        }

        //funtion to set the scrollbar max value
        private void SetScrollBarValues()
        {
            //Set the following scrollbar properties: 

            //Minimum: Set to 0 

            //SmallChange and LargeChange: Per UI guidelines, these must be set 
            //    relative to the size of the view that the user sees, not to 
            //    the total size including the unseen part.  In this example, 
            //    these must be set relative to the picture box, not to the image. 

            //Maximum: Calculate in steps: 
            //Step 1: The maximum to scroll is the size of the unseen part. 
            //Step 2: Add the size of visible scrollbars if necessary. 
            //Step 3: Add an adjustment factor of ScrollBar.LargeChange. 


            //Configure the horizontal scrollbar 
            //--------------------------------------------- 
            if (this.hScrollBar1.Visible)
            {
                this.hScrollBar1.Minimum = 0;
                this.hScrollBar1.SmallChange = this.pictureBox1.Width / 20;
                this.hScrollBar1.LargeChange = this.pictureBox1.Width / 10;

                this.hScrollBar1.Maximum = this.pictureBox1.Image.Size.Width - pictureBox1.ClientSize.Width;  //step 1 

                if (this.vScrollBar1.Visible) //step 2
                {
                    this.hScrollBar1.Maximum += this.vScrollBar1.Width;
                }

                this.hScrollBar1.Maximum += this.hScrollBar1.LargeChange; //step 3
            }

            //Configure the vertical scrollbar 
            //--------------------------------------------- 
            if (this.vScrollBar1.Visible)
            {
                this.vScrollBar1.Minimum = 0;
                this.vScrollBar1.SmallChange = this.pictureBox1.Height / 20;
                this.vScrollBar1.LargeChange = this.pictureBox1.Height / 10;

                this.vScrollBar1.Maximum = this.pictureBox1.Image.Size.Height - pictureBox1.ClientSize.Height; //step 1 

                if (this.hScrollBar1.Visible) //step 2
                {
                    this.vScrollBar1.Maximum += this.hScrollBar1.Height;
                }

                this.vScrollBar1.Maximum += this.vScrollBar1.LargeChange; //step 3
            }
        }

        
        private void panel1_MouseWheel(object sender, MouseEventArgs e)
        {
            /*
            if (e.Delta != 0)
            {
                if (e.Delta <= 0)
                {
                    //set minimum size to zoom
                    if (pictureBox1.Width < 50)
                        return;
                }
                else
                {
                    //set maximum size to zoom
                    if (pictureBox1.Width > 500)
                        return;
                }
                pictureBox1.Width += Convert.ToInt32(pictureBox1.Width * e.Delta / 1000);
                pictureBox1.Height += Convert.ToInt32(pictureBox1.Height * e.Delta / 1000);
            }
             */
        }
             
    }
}

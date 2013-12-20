using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Collections;
using System.Windows.Forms;

namespace ComCommunicator
{
    public enum EOBJECT_DRAW
    {
        EUNDEFINED_OBJECT = -1,
        ELINE_OBJECT = 0,
        ERECTANGLE_BORDER_OBJECT,
        ERECTANGLE_FILLER_OBJECT,
        ECIRCLE_OBJECT,

        // Last one
        ENUM_OBJECTS
    }

    public partial class CNC_Design_Objects : Form
    {
        private Form1 _parentForm = null;
        private Graphics _Graphics = null;

        private EOBJECT_DRAW _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;

        // Current mouse position
        private int _nCurrentMouseXPosition = -1;
        private int _nCurrentMouseYPosition = -1;

        // Points saved on mouse event
        private CPoint _mouseSingleClickDown = new CPoint();
        private int _nNumMouseSingleClick = 0;

        // Defines thickness of lines
        private int _nThicknessLine = 5; // 5 millimeters as default
        private int _nMinThicknessLine = 5; // minimum thickness value

        // Image size
        private int _nImageWidth = 0;
        private int _nImageHeight = 0;

        // Button used to select border or filler
        private bool _bBorderFiller = false; // at the beginning is border

        // Objects parameters
        private CLine _Line = new CLine();
        private CRectangle _Rectangle = new CRectangle();
        private CRectangleFill _RectangleFiller = new CRectangleFill();
        private CCircle _Circle = new CCircle();

        // Contains all objects created
        private CContainer _Container = new CContainer();

        public CNC_Design_Objects(Form1 form, int nWidth, int nHeight, int nMinThickness)
        {
            InitializeComponent();

            _parentForm = form;
            _Graphics = pictureBoxMain.CreateGraphics();

            ResetCurrentActionLabel();
            ResetMouseEvents();

            // Save image dimension
            _nImageWidth = nWidth;
            _nImageHeight = nHeight;

            _nThicknessLine = nMinThickness;
            textBoxThickness.Text = Convert.ToString(_nThicknessLine);
            textBoxMinThickness.Text = _nMinThicknessLine.ToString();

            labelX.Text = _nImageWidth.ToString();
            labelY.Text = _nImageHeight.ToString();

            treeViewObjects.BeginUpdate();

            treeViewObjects.Nodes.Add("Lines");
            treeViewObjects.Nodes.Add("Rectangles");
            treeViewObjects.Nodes.Add("Circles");

            treeViewObjects.EndUpdate();

            // Show current form
            _parentForm.Enabled = false;
            this.Enabled = true;
            this.Show();
            this.Focus();
        }

        private void ResetMouseEvents()
        {
            _nNumMouseSingleClick = 0;
        }

        private void ResetCurrentActionLabel()
        {
            labelCurrentAction.Text = "";
            labelCurrentAction.Visible = false;
            _nNumMouseSingleClick = 0;

            labelObjects.Text = "Objects Created: " + _Container.GetNumObjects().ToString();
        }

        private void ShowCurrentActionLabel(string data)
        {
            labelCurrentAction.Text = data;
            labelCurrentAction.Visible = true;
        }

        private void pictureBoxMain_MouseMove(object sender, MouseEventArgs e)
        {
            _nCurrentMouseXPosition = e.X;
            _nCurrentMouseYPosition = e.Y;

            labelCurrentMousePosition.Text = "X = " + Convert.ToString(_nCurrentMouseXPosition) +
                                             ", Y = " + Convert.ToString(_nCurrentMouseYPosition);

            // Check an object is being drawing
            switch (_currentObject)
            {
                case EOBJECT_DRAW.ELINE_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _Line._second._nX = e.X;
                        _Line._second._nY = e.Y;
                        _Line.DrawTempLine(_Graphics, Color.Black);
                        ShowCurrentActionLabel("Draw Line: X1 = " + Convert.ToString(_Line._first._nX) + ", Y1 = " + Convert.ToString(_Line._first._nY)
                                                + ", X2 = " + Convert.ToString(e.X) + ", Y2 = " + Convert.ToString(e.Y));

                        _Container.DrawAllObjects(_Graphics);
                    }
                    break;

                case EOBJECT_DRAW.ERECTANGLE_BORDER_OBJECT:
                    
                    if (_nNumMouseSingleClick == 1)
                    {
                        _Rectangle._second._nX = e.X;
                        _Rectangle._second._nY = e.Y;
                        _Rectangle.DrawTempRectangle(_Graphics, Color.Black);
                        ShowCurrentActionLabel("Draw Rectangle: X1 = " + Convert.ToString(_Rectangle._first._nX) + ", Y1 = " + Convert.ToString(_Rectangle._first._nY)
                                                + ", X2 = " + Convert.ToString(e.X) + ", Y2 = " + Convert.ToString(e.Y));

                        _Container.DrawAllObjects(_Graphics);
                    }
                    break;

                case EOBJECT_DRAW.ERECTANGLE_FILLER_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _RectangleFiller._second._nX = e.X;
                        _RectangleFiller._second._nY = e.Y;
                        _RectangleFiller.DrawTempRectangle(_Graphics, Color.Black);
                        ShowCurrentActionLabel("Fill Rectangle: X1 = " + Convert.ToString(_Rectangle._first._nX) + ", Y1 = " + Convert.ToString(_Rectangle._first._nY)
                                                + ", X2 = " + Convert.ToString(e.X) + ", Y2 = " + Convert.ToString(e.Y));

                        _Container.DrawAllObjects(_Graphics);
                    }
                    break;

                case EOBJECT_DRAW.ECIRCLE_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _Circle._nRange = Math.Max(Math.Abs(_Circle._nCenterX - e.X), Math.Abs(_Circle._nCenterY - e.Y));
                        _Circle.DrawTempCircle(_Graphics, Color.Black);

                        if (_Circle._bIsFill == false)
                        {
                            ShowCurrentActionLabel("Draw Circle: Center X = " + Convert.ToString(_Circle._nCenterX) + ", Center Y = " + Convert.ToString(_Circle._nCenterY) + ", Range = " + _Circle._nRange);
                        }
                        else
                        {
                            ShowCurrentActionLabel("Fill Circle: Center X = " + Convert.ToString(_Circle._nCenterX) + ", Center Y = " + Convert.ToString(_Circle._nCenterY) + ", Range = " + _Circle._nRange);
                        }
                        _Container.DrawAllObjects(_Graphics);
                    }
                    break;
            }
        }

        private void CNC_Design_Objects_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }

        private void pictureBoxMain_MouseClick(object sender, MouseEventArgs e)
        {
            // Only left button can be used
            if (e.Button != MouseButtons.Left)
            {
                // Check if right button has been pressed
                if (e.Button == MouseButtons.Right)
                {
                    if (_nNumMouseSingleClick >= 1)
                    {
                        switch (_currentObject)
                        {
                            case EOBJECT_DRAW.ELINE_OBJECT:

                                _Line._second._nX = e.X;
                                _Line._second._nY = e.Y;
                                _Line.Draw(_Graphics, Color.White);
                                break;

                            case EOBJECT_DRAW.ERECTANGLE_BORDER_OBJECT:

                                _Rectangle._second._nX = e.X;
                                _Rectangle._second._nY = e.Y;
                                _Rectangle.Draw(_Graphics, Color.White);
                                break;

                            case EOBJECT_DRAW.ERECTANGLE_FILLER_OBJECT:

                                _RectangleFiller._second._nX = e.X;
                                _RectangleFiller._second._nY = e.Y;
                                _RectangleFiller.Draw(_Graphics, Color.White);
                                break;

                            case EOBJECT_DRAW.ECIRCLE_OBJECT:

                                _Circle._nRange = Math.Max(Math.Abs(_Circle._nCenterX - e.X), Math.Abs(_Circle._nCenterY - e.Y));
                                _Circle.Draw(_Graphics, Color.White);
                                break;
                        }

                        ResetCurrentActionLabel();
                        _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;
                    }
                    else
                    {
                        _mouseSingleClickDown._nX = e.X;
                        _mouseSingleClickDown._nY = e.Y;
                        contextMenuStrip1.Show(Cursor.Position.X, Cursor.Position.Y);
                    }
                }
                return;
            }

            _nNumMouseSingleClick++;
            _mouseSingleClickDown._nX = e.X;
            _mouseSingleClickDown._nY = e.Y;

            // Look action to do
            switch (_currentObject)
            {
                case EOBJECT_DRAW.ELINE_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _Line._first._nX = e.X;
                        _Line._first._nY = e.Y;

                        ShowCurrentActionLabel("Draw Line: X1 = " + Convert.ToString(_Line._first._nX) + ", Y1 = " + Convert.ToString(_Line._first._nY));
                    }
                    else if (_nNumMouseSingleClick == 2)
                    {
                        _Line._second._nX = e.X;
                        _Line._second._nY = e.Y;
                        _Line.Draw(_Graphics);

                        // Add element
                        _Container.AddLine(_Line, treeViewObjects);

                        ResetCurrentActionLabel();
                        _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;
                    }
                    break;

                case EOBJECT_DRAW.ERECTANGLE_BORDER_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _Rectangle._first._nX = e.X;
                        _Rectangle._first._nY = e.Y;

                        ShowCurrentActionLabel("Draw Rectangle: X1 = " + Convert.ToString(_Rectangle._first._nX) + ", Y1 = " + Convert.ToString(_Rectangle._first._nY));
                    }
                    else if (_nNumMouseSingleClick == 2)
                    {
                        _Rectangle._second._nX = e.X;
                        _Rectangle._second._nY = e.Y;
                        _Rectangle.Draw(_Graphics);

                        // Add element
                        _Container.AddRectangle(_Rectangle, treeViewObjects);

                        ResetCurrentActionLabel();
                        _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;
                    }
                    break;

                case EOBJECT_DRAW.ERECTANGLE_FILLER_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _RectangleFiller._first._nX = e.X;
                        _RectangleFiller._first._nY = e.Y;

                        ShowCurrentActionLabel("Fill Rectangle: X1 = " + Convert.ToString(_RectangleFiller._first._nX) + ", Y1 = " + Convert.ToString(_RectangleFiller._first._nY));
                    }
                    else if (_nNumMouseSingleClick == 2)
                    {
                        _RectangleFiller._second._nX = e.X;
                        _RectangleFiller._second._nY = e.Y;
                        _RectangleFiller.Draw(_Graphics);

                        // Add element
                        _Container.AddRectangleFill(_RectangleFiller, treeViewObjects);

                        ResetCurrentActionLabel();
                        _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;
                    }
                    break;

                case EOBJECT_DRAW.ECIRCLE_OBJECT:

                    if (_nNumMouseSingleClick == 1)
                    {
                        _Circle._nCenterX = e.X;
                        _Circle._nCenterY = e.Y;

                        if (_Circle._bIsFill == false)
                        {
                            ShowCurrentActionLabel("Draw Circle: Center X = " + Convert.ToString(_Circle._nCenterX) + ", Center Y = " + Convert.ToString(_Circle._nCenterY));
                        } 
                        else
                        {
                            ShowCurrentActionLabel("Fill Circle: Center X = " + Convert.ToString(_Circle._nCenterX) + ", Center Y = " + Convert.ToString(_Circle._nCenterY));
                        }
                    }
                    else if (_nNumMouseSingleClick == 2)
                    {
                        _Circle._nRange = Math.Max(Math.Abs(_Circle._nCenterX - e.X), Math.Abs(_Circle._nCenterY - e.Y));
                        _Circle.Draw(_Graphics);

                        // Add element
                        _Container.AddCircle(_Circle, treeViewObjects);

                        ResetCurrentActionLabel();
                        _currentObject = EOBJECT_DRAW.EUNDEFINED_OBJECT;
                    }
                    break;
            }
        }

        private void buttonDrawLine_Click(object sender, EventArgs e)
        {
            _currentObject = EOBJECT_DRAW.ELINE_OBJECT;

            ResetMouseEvents();
            ShowCurrentActionLabel("Draw Line");

            _Line._fWidth = (float)_nThicknessLine / _nMinThicknessLine;
        }

        private void buttonRectangle_Click(object sender, EventArgs e)
        {
            if (_bBorderFiller == false)
            {
                _currentObject = EOBJECT_DRAW.ERECTANGLE_BORDER_OBJECT;
                ShowCurrentActionLabel("Draw Rectangle");

                _Rectangle._fWidth = (float)_nThicknessLine / _nMinThicknessLine;
            } 
            else
            {
                _currentObject = EOBJECT_DRAW.ERECTANGLE_FILLER_OBJECT;
                ShowCurrentActionLabel("Fill Rectangle");
            }
            
            ResetMouseEvents();
        }

        private void buttonCircle_Click(object sender, EventArgs e)
        {
            if (_bBorderFiller == true)
            {
                _Circle._bIsFill = true;
                ShowCurrentActionLabel("Fill Circle");
            }
            else
            {
                _Circle._bIsFill = false;
                _Circle._fWidth = (float)_nThicknessLine / _nMinThicknessLine;
                ShowCurrentActionLabel("Draw Circle");
            }

            _currentObject = EOBJECT_DRAW.ECIRCLE_OBJECT;
            ResetMouseEvents();
        }

        private void buttonBorderStyle_Click(object sender, EventArgs e)
        {
            _bBorderFiller = !_bBorderFiller;

            if (_bBorderFiller == true)
            {
                buttonBorderStyle.Text = "Filler";
            } 
            else
            {
                buttonBorderStyle.Text = "Border";
            }
        }

        private void textBoxThickness_TextChanged(object sender, EventArgs e)
        {
            try
            {
                if ((Convert.ToInt32(textBoxThickness.Text) <= 0) || (Convert.ToInt32(textBoxThickness.Text) < _nMinThicknessLine))
                {
                    textBoxThickness.Text = Convert.ToString(_nThicknessLine);
                }
                else
                {
                    _nThicknessLine = Convert.ToInt32(textBoxThickness.Text);
                }
            }
            catch (System.Exception ex)
            {
                textBoxThickness.Text = Convert.ToString(_nThicknessLine);
            }
        }

        private void CNC_Design_Objects_ResizeEnd(object sender, EventArgs e)
        {
            _Container.DrawAllObjects(_Graphics);
        }

        private void deleteAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Container.DestroyAll(treeViewObjects);
            _Graphics.Clear(Color.White);
        }

        private void deleteElementToolStripMenuItem_Click(object sender, EventArgs e)
        {
            _Container.DestroyElementAtPosition(_mouseSingleClickDown._nX, _mouseSingleClickDown._nY, _Graphics, treeViewObjects);
            _Container.DrawAllObjects(_Graphics);
        }
    }

    public class CPoint
    {
        public int _nX = -1;
        public int _nY = -1;
    }

    public class CLine
    {
        public CPoint _first = new CPoint();
        public CPoint _second = new CPoint();
        public CPoint _secondTemp = new CPoint();
        public float _fWidth = 1;
        public int _nNumTempLines = 0;

        public void Draw(Graphics graphic)
        {
            graphic.DrawLine(new Pen(Color.Black, _fWidth), new Point(_first._nX, _first._nY), new Point(_second._nX, _second._nY));
            _nNumTempLines = 0;
        }

        public void Draw(Graphics graphic, Color color)
        {
            graphic.DrawLine(new Pen(color, _fWidth), new Point(_first._nX, _first._nY), new Point(_second._nX, _second._nY));
            _nNumTempLines = 0;
        }

        public void DrawTempLine(Graphics graphic, Color color)
        {
            if (_nNumTempLines > 0)
            {
                graphic.DrawLine(new Pen(Color.White, _fWidth), new Point(_first._nX, _first._nY), new Point(_secondTemp._nX, _secondTemp._nY));
                graphic.DrawLine(new Pen(color, _fWidth), new Point(_first._nX, _first._nY), new Point(_second._nX, _second._nY));
            }
            else
            {
                graphic.DrawLine(new Pen(color, _fWidth), new Point(_first._nX, _first._nY), new Point(_second._nX, _second._nY));
            }

            // Save last point added
            _secondTemp._nX = _second._nX;
            _secondTemp._nY = _second._nY;

            _nNumTempLines++;
        }
    }

    public class CRectangle
    {
        public CPoint _first = new CPoint();
        public CPoint _second = new CPoint();
        public CPoint _secondTemp = new CPoint();
        public float _fWidth = 1;
        private int _nNumTemp = 0;

        private void GetRectangleCoordinates(CPoint second, out int nX, out int nY, out int nWidth, out int nHeight)
        {
            if (_first._nX < second._nX)
            {
                if (_first._nY < second._nY)
                {
                    nX = _first._nX;
                    nY = _first._nY;
                    nWidth = second._nX - _first._nX;
                    nHeight = second._nY - _first._nY;
                }
                else
                {
                    nX = _first._nX;
                    nY = second._nY;
                    nWidth = second._nX - _first._nX;
                    nHeight = _first._nY - second._nY;
                }
            }
            else
            {
                if (_first._nY < second._nY)
                {
                    nX = second._nX;
                    nY = _first._nY;
                    nWidth = _first._nX - second._nX;
                    nHeight = second._nY - _first._nY;
                }
                else
                {
                    nX = second._nX;
                    nY = second._nY;
                    nWidth = _first._nX - second._nX;
                    nHeight = _first._nY - second._nY;
                }
            }
        }

        public bool ContainThisPoint(int nX, int nY)
        {
            if ((nX >= _first._nX) && (nX <= _second._nX) &&
                (nY >= _first._nY) && (nY <= _second._nY))
            {
                return true;
            } 
            else
            {
                return false;
            }
        }

        public void Draw(Graphics graphic)
        {
            int nX, nY, nWidth, nHeight;

            GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);

            graphic.DrawRectangle(new Pen(Color.Black, _fWidth), nX, nY, nWidth, nHeight);
            _nNumTemp = 0;
        }

        public void Draw(Graphics graphic, Color color)
        {
            int nX, nY, nWidth, nHeight;

            GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);

            graphic.DrawRectangle(new Pen(color, _fWidth), nX, nY, nWidth, nHeight);
            _nNumTemp = 0;
        }

        public void Draw(Graphics graphic, Color color, Pen pen)
        {
            int nX, nY, nWidth, nHeight;

            GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);

            graphic.DrawRectangle(pen, nX, nY, nWidth, nHeight);
            _nNumTemp = 0;
        }

        public void DrawTempRectangle(Graphics graphic, Color color)
        {
            int nX, nY, nWidth, nHeight;

            if (_nNumTemp > 0)
            {
                GetRectangleCoordinates(_secondTemp, out nX, out nY, out nWidth, out nHeight);
                graphic.DrawRectangle(new Pen(Color.White, _fWidth), nX, nY, nWidth, nHeight);

                GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);
                graphic.DrawRectangle(new Pen(color, _fWidth), nX, nY, nWidth, nHeight);
            }
            else
            {
                GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);
                graphic.DrawRectangle(new Pen(color, _fWidth), nX, nY, nWidth, nHeight);
            }

            // Save last point added
            _secondTemp._nX = _second._nX;
            _secondTemp._nY = _second._nY;

            _nNumTemp++;
        }
    }

    public class CRectangleFill
    {
        public CPoint _first = new CPoint();
        public CPoint _second = new CPoint();
        public CPoint _secondTemp = new CPoint();
        private int _nNumTemp = 0;

        private void GetRectangleCoordinates(CPoint second, out int nX, out int nY, out int nWidth, out int nHeight)
        {
            if (_first._nX < second._nX)
            {
                if (_first._nY < second._nY)
                {
                    nX = _first._nX;
                    nY = _first._nY;
                    nWidth = second._nX - _first._nX;
                    nHeight = second._nY - _first._nY;
                }
                else
                {
                    nX = _first._nX;
                    nY = second._nY;
                    nWidth = second._nX - _first._nX;
                    nHeight = _first._nY - second._nY;
                }
            }
            else
            {
                if (_first._nY < second._nY)
                {
                    nX = second._nX;
                    nY = _first._nY;
                    nWidth = _first._nX - second._nX;
                    nHeight = second._nY - _first._nY;
                }
                else
                {
                    nX = second._nX;
                    nY = second._nY;
                    nWidth = _first._nX - second._nX;
                    nHeight = _first._nY - second._nY;
                }
            }
        }

        public bool ContainThisPoint(int nX, int nY)
        {
            if ((nX >= _first._nX) && (nX <= _second._nX) &&
                (nY >= _first._nY) && (nY <= _second._nY))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(Graphics graphic)
        {
            int nX, nY, nWidth, nHeight;

            GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);

            graphic.FillRectangle(new SolidBrush(Color.Black), nX, nY, nWidth, nHeight);
            _nNumTemp = 0;
        }

        public void Draw(Graphics graphic, Color color)
        {
            int nX, nY, nWidth, nHeight;

            GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);

            graphic.FillRectangle(new SolidBrush(color), nX, nY, nWidth, nHeight);
            _nNumTemp = 0;
        }

        public void DrawTempRectangle(Graphics graphic, Color color)
        {
            int nX, nY, nWidth, nHeight;

            if (_nNumTemp > 0)
            {
                GetRectangleCoordinates(_secondTemp, out nX, out nY, out nWidth, out nHeight);
                graphic.FillRectangle(new SolidBrush(Color.White), nX, nY, nWidth, nHeight);

                GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);
                graphic.FillRectangle(new SolidBrush(color), nX, nY, nWidth, nHeight);
            }
            else
            {
                GetRectangleCoordinates(_second, out nX, out nY, out nWidth, out nHeight);
                graphic.FillRectangle(new SolidBrush(color), nX, nY, nWidth, nHeight);
            }

            // Save last point added
            _secondTemp._nX = _second._nX;
            _secondTemp._nY = _second._nY;

            _nNumTemp++;
        }
    }

    public class CCircle
    {
        public int _nCenterX = -1;
        public int _nCenterY = -1;
        public int _nRange = -1;
        private int _nTempRange = -1;
        public bool _bIsFill = false;
        public float _fWidth = 1;
        private int _nNumTemp = 0;

        public void Draw(Graphics graphic, Color color)
        {
            if (_bIsFill == false)
            {
                graphic.DrawEllipse(new Pen(color, _fWidth), _nCenterX, _nCenterY, _nRange, _nRange);
            } 
            else
            {
                graphic.FillEllipse(new SolidBrush(color), _nCenterX, _nCenterY, _nRange, _nRange);
            }
            _nNumTemp = 0;
        }

        public bool ContainThisPoint(int nX, int nY)
        {
            if ((nX >= _nCenterX - _nRange) && (nX <= _nCenterX + _nRange) &&
                (nY >= _nCenterY - _nRange) && (nY <= _nCenterY + _nRange))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public void Draw(Graphics graphic)
        {
            if (_bIsFill == true)
            {
                graphic.FillEllipse(new SolidBrush(Color.Black), _nCenterX, _nCenterY, _nRange, _nRange);
            }
            else
            {
                graphic.DrawEllipse(new Pen(Color.Black, _fWidth), _nCenterX, _nCenterY, _nRange, _nRange);
            }
            _nNumTemp = 0;
        }

        public void DrawTempCircle(Graphics graphic, Color color)
        {
            if (_bIsFill == true)
            {
                if (_nNumTemp > 0)
                {
                    graphic.FillEllipse(new SolidBrush(Color.White), _nCenterX, _nCenterY, _nTempRange, _nTempRange);
                    graphic.FillEllipse(new SolidBrush(Color.Black), _nCenterX, _nCenterY, _nRange, _nRange);
                }
                else
                {
                    graphic.FillEllipse(new SolidBrush(Color.Black), _nCenterX, _nCenterY, _nRange, _nRange);
                }
            }
            else
            {
                if (_nNumTemp > 0)
                {
                    graphic.DrawEllipse(new Pen(Color.White, _fWidth), _nCenterX, _nCenterY, _nTempRange, _nTempRange);
                    graphic.DrawEllipse(new Pen(Color.Black, _fWidth), _nCenterX, _nCenterY, _nRange, _nRange);
                }
                else
                {
                    graphic.DrawEllipse(new Pen(Color.Black, _fWidth), _nCenterX, _nCenterY, _nRange, _nRange);
                }
            }

            // Save last point added
            _nTempRange = _nRange;

            _nNumTemp++;
        }
    }

    public class CContainer
    {
        private CLine[] _Lines = new CLine[1000];
        private CRectangle[] _Rectangles = new CRectangle[1000];
        private CRectangleFill[] _RectanglesFiller = new CRectangleFill[1000];
        private CCircle[] _Circles = new CCircle[1000];
        private int _nNumLines = 0;
        private int _nNumRectangles = 0;
        private int _nNumRectanglesFill = 0;
        private int _nNumCircles = 0;

        public int GetNumObjects()
        {
            return _nNumLines + _nNumRectangles + _nNumRectanglesFill + _nNumCircles;
        }

        public void AddLine(CLine line, TreeView tree)
        {
            tree.Nodes[0].Nodes.Add(
                "X1: " + line._first._nX.ToString() + " Y1: " + line._first._nY.ToString() +
                " X2: " + line._second._nX.ToString() + " Y2: " + line._second._nY.ToString() +
                " Thi: " + line._fWidth.ToString());

            tree.ExpandAll();

            _Lines[_nNumLines] = new CLine();
            _Lines[_nNumLines]._first = line._first;
            _Lines[_nNumLines]._second = line._second;
            _Lines[_nNumLines]._fWidth = line._fWidth;
            _nNumLines++;
        }

        public void AddRectangle(CRectangle rect, TreeView tree)
        {
            tree.Nodes[1].Nodes.Add(
                "X1: " + rect._first._nX.ToString() + " Y1: " + rect._first._nY.ToString() +
                " X2: " + rect._second._nX.ToString() + " Y2: " + rect._second._nY.ToString() +
                " Thi: " + rect._fWidth.ToString());

            tree.ExpandAll();

            _Rectangles[_nNumRectangles] = new CRectangle();
            _Rectangles[_nNumRectangles]._first = rect._first;
            _Rectangles[_nNumRectangles]._second = rect._second;
            _Rectangles[_nNumRectangles]._fWidth = rect._fWidth;
            _nNumRectangles++;
        }

        public void AddRectangleFill(CRectangleFill rect, TreeView tree)
        {
            tree.Nodes[1].Nodes.Add(
                "X1: " + rect._first._nX.ToString() + " Y1: " + rect._first._nY.ToString() +
                " X2: " + rect._second._nX.ToString() + " Y2: " + rect._second._nY.ToString() +
                " Fill");

            tree.ExpandAll();

            _RectanglesFiller[_nNumRectanglesFill] = new CRectangleFill();
            _RectanglesFiller[_nNumRectanglesFill]._first = rect._first;
            _RectanglesFiller[_nNumRectanglesFill]._second = rect._second;
            _nNumRectanglesFill++;
        }

        public void AddCircle(CCircle circle, TreeView tree)
        {
            if (circle._bIsFill == true)
            {
                tree.Nodes[2].Nodes.Add(
                   "X: " + circle._nCenterX.ToString() + " Y: " + circle._nCenterY.ToString() +
                   " Range: " + circle._nRange.ToString() + " Fill");
            } 
            else
            {
                tree.Nodes[2].Nodes.Add(
                   "X: " + circle._nCenterX.ToString() + " Y: " + circle._nCenterY.ToString() +
                   " Range: " + circle._nRange.ToString() + " Thick: " + circle._fWidth.ToString());
            }

            tree.ExpandAll();

            _Circles[_nNumCircles] = new CCircle();
            _Circles[_nNumCircles]._nCenterX = circle._nCenterX;
            _Circles[_nNumCircles]._nCenterY = circle._nCenterY;
            _Circles[_nNumCircles]._nRange = circle._nRange;
            _Circles[_nNumCircles]._fWidth = circle._fWidth;
            _Circles[_nNumCircles]._bIsFill = circle._bIsFill;
            _nNumCircles++;
        }

        public void DestroyAll(TreeView tree)
        {
            tree.Nodes.Clear();

            tree.BeginUpdate();

            tree.Nodes.Add("Lines");
            tree.Nodes.Add("Rectangles");
            tree.Nodes.Add("Circles");

            tree.EndUpdate();

            _nNumLines = 0;
            _nNumRectangles = 0;
            _nNumRectanglesFill = 0;
            _nNumCircles = 0;
        }

        public bool DestroyElementAtPosition(int nX, int nY, Graphics graphic, TreeView tree)
        {
            bool bRetVal = false;

            // Search first in rectangles and circles
            for (int i = 0; i < _nNumRectangles; i++)
            {
                if (_Rectangles[i].ContainThisPoint(nX, nY) == true)
                {
                    tree.Nodes[1].Nodes[i].Remove();
                    _Rectangles[i].Draw(graphic, Color.White);
                    Array.ConstrainedCopy(_Rectangles, i + 1, _Rectangles, i, _nNumRectangles - (i + 1));
                    --_nNumRectangles;
                    return true;
                }
            }

            for (int i = 0; i < _nNumRectanglesFill; i++)
            {
                if (_RectanglesFiller[i].ContainThisPoint(nX, nY) == true)
                {
                    tree.Nodes[1].Nodes[i].Remove();
                    _RectanglesFiller[i].Draw(graphic, Color.White);
                    Array.ConstrainedCopy(_RectanglesFiller, i + 1, _RectanglesFiller, i, _nNumRectanglesFill - (i + 1));
                    --_nNumRectanglesFill;
                    return true;
                }
            }

            for (int i = 0; i < _nNumCircles; i++)
            {
                if (_Circles[i].ContainThisPoint(nX, nY) == true)
                {
                    tree.Nodes[2].Nodes[i].Remove();
                    _Circles[i].Draw(graphic, Color.White);
                    Array.ConstrainedCopy(_Circles, i + 1, _Circles, i, _nNumCircles - (i + 1));
                    --_nNumCircles;
                    return true;
                }
            }

            return bRetVal;
        }

        public void DrawAllObjects(Graphics graphic)
        {
            for (int i = 0; i < _nNumLines; i++ )
            {
                _Lines[i].Draw(graphic, Color.Black);
            }

            for (int i = 0; i < _nNumRectangles; i++)
            {
                _Rectangles[i].Draw(graphic, Color.Black);
            }

            for (int i = 0; i < _nNumRectanglesFill; i++)
            {
                _RectanglesFiller[i].Draw(graphic, Color.Black);
            }

            for (int i = 0; i < _nNumCircles; i++ )
            {
                _Circles[i].Draw(graphic, Color.Black);
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComCommunicator
{
    public class Warper
    {
        private Form1 _parentForm = null;
        private int[] _nxCooridinates = null;
        private int[] _nyCooridinates = null;
        private int[] _nzCooridinates = null;

        private int _nXImageSize = -1;
        private int _nYImageSize = -1;
        private int _nZImageSize = -1;

        private int _nXImageAnalyzed = -1;
        private int _nYImageAnalyzed = -1;
        private int _nZImageAnalyzed = -1;

        public Warper(Form1 form, int nXMax, int nYMax, int nZMax)
        {
            _parentForm = form;

            _nXImageSize = nXMax;
            _nYImageSize = nYMax;
            _nZImageSize = nZMax;
        }

        public Warper(Form1 form, int[] xCoord, int[] yCoord, int[] zCoord, int nXMax, int nYMax, int nZMax, 
                      int nImageWidth, int nImageHeigth, int nDepth)
        {
            _parentForm = form;
            _nxCooridinates = xCoord;
            _nyCooridinates = yCoord;
            _nzCooridinates = zCoord;

            _nXImageSize = nXMax;
            _nYImageSize = nYMax;
            _nZImageSize = nZMax;

            _nXImageAnalyzed = nImageWidth;
            _nYImageAnalyzed = nImageHeigth;
            _nZImageAnalyzed = nDepth;
        }

        public bool WarpImagePoints(out int[] xCoordWarped, out int[] yCoordWarped, out int[] zCoordWarped)
        {
            xCoordWarped = _nxCooridinates;
            yCoordWarped = _nyCooridinates;
            zCoordWarped = _nzCooridinates;

            if ((_parentForm.X_Max_Val <= 0) || (_parentForm.Y_Max_Val <= 0) || (_parentForm.Z_Max_Val <= 0) ||
                (_nXImageSize <= 0) || (_nYImageSize <= 0) || (_nZImageSize <= 0) ||
                (_nxCooridinates.Length != _nyCooridinates.Length) || (_nyCooridinates.Length != _nzCooridinates.Length)
                || (_nxCooridinates.Length != _nzCooridinates.Length))
            {
                return false;
            }

            // Coordinates are proportionally: Ximm : Xmaximm = Xpiano : Xmaxpiano
            for (int i = 0; i < _nxCooridinates.Length; ++i)
            {
                // X coordinates
                try
                {
                    xCoordWarped[i] = (_nxCooridinates[i] * (_parentForm.X_Max_Val - 1)) / (_nXImageAnalyzed - 1);
                }
                catch (DivideByZeroException e)
                {
                    xCoordWarped[i] = _parentForm.X_Max_Val - 1;
                }

                // Y coordinates
                try
                {
                    yCoordWarped[i] = (_nyCooridinates[i] * (_parentForm.Y_Max_Val - 1)) / (_nYImageAnalyzed - 1);
                }
                catch (DivideByZeroException e)
                {
                    yCoordWarped[i] = _parentForm.Y_Max_Val - 1;
                }

                // Z coordinates
                try
                {
                    zCoordWarped[i] = (_nzCooridinates[i] * (_parentForm.Z_Max_Val - 1)) / (_nZImageAnalyzed - 1);
                }
                catch (DivideByZeroException e)
                {
                    zCoordWarped[i] = _parentForm.Z_Max_Val - 1;
                }                             
            }

            return true;
        }
    }
}

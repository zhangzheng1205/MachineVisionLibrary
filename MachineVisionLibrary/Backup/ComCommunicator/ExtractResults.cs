using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ComCommunicator
{
    class ExtractResults
    {
        private Form1 _parentForm = null;
        private IntPtr _pResults = IntPtr.Zero;
        private int _nNumPoints = 0;

        private string[] _responseCommands = null;

        public ExtractResults(Form1 form, int nNumPoints, IntPtr pData)
        {
            _parentForm = form;
            _nNumPoints = nNumPoints;
            _pResults = pData;
        }

        public bool SendDataToCNC(int nImageWidth, int nImageHeigth)
        {
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;
            IntPtr pCurrent = _pResults;

            // For each point add other two points, one to get down, the other to get up drill
            string[] sData = new string[_nNumPoints*3];

            unsafe
            {
                int* pnValues = (int*)_pResults.ToPointer();

                int[] pnXCoord = new int[_nNumPoints * 3];
                int[] pnYCoord = new int[_nNumPoints * 3];
                int[] pnZCoord = new int[_nNumPoints * 3];

                int[] pnXCoordOut = new int[_nNumPoints * 3];
                int[] pnYCoordOut = new int[_nNumPoints * 3];
                int[] pnZCoordOut = new int[_nNumPoints * 3];

                int nXVal = 0;
                int nYVal = 0;

                for (int i = 0; i < _nNumPoints*3; i+=3)
                {
                    nXVal = pnValues[(i / 3) * 2];
                    nYVal = pnValues[(i / 3) * 2 + 1];

                    pnXCoord[i] = nXVal;
                    pnYCoord[i] = nYVal;
                    pnZCoord[i] = 0;

                    // go down
                    pnXCoord[i + 1] = nXVal;
                    pnYCoord[i + 1] = nYVal;
                    pnZCoord[i + 1] = 20;

                    // go up
                    pnXCoord[i + 2] = nXVal;
                    pnYCoord[i + 2] = nYVal;
                    pnZCoord[i + 2] = 0;
                }

                Warper pWarper = new Warper(_parentForm, pnXCoord, pnYCoord, pnZCoord, 
                                            _parentForm.X_Max_Val, _parentForm.Y_Max_Val,
                                            _parentForm.Z_Max_Val, nImageWidth, nImageHeigth, 100);
                if (pWarper.WarpImagePoints(out pnXCoordOut, out pnYCoordOut, out pnZCoordOut) == true)
                {
                    for (int i = 0; i < _nNumPoints; i++)
                    {
                        sData[i] = "@wPX" + Convert.ToString(pnXCoordOut[i]).PadLeft(6, '0') + "Y" + Convert.ToString(pnYCoordOut[i]).PadLeft(6, '0') + "Z" + Convert.ToString(pnZCoordOut[i]).PadLeft(6, '0') + ";";
                    }
                }
                else
                {
                    return false;
                }
            }

            _parentForm.Enabled = false;
            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
            return true;
        }

        public void EndOfSettingsParameters(bool bValue, ref string[] response)
        {
            _parentForm.Enabled = true;
        }
    }
}

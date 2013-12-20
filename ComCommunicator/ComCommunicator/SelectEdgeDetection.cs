using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Windows.Forms;

namespace ComCommunicator
{
    public partial class SelectEdgeDetection : Form
    {
        private Form1 _parentForm;
        private string _radioSelected;
        private MVL_ANALYSIS _propertyAnalysis = MVL_ANALYSIS.MVL_UNDEFINED_ANALISYS;

        public SelectEdgeDetection(Form1 form, ref string previousAnalysis)
        {
            InitializeComponent();

            _parentForm = form;

            if (previousAnalysis == null)
            {
                _radioSelected = "Canny";
                _propertyAnalysis = MVL_ANALYSIS.MVL_CANNY_ANALYSIS;
            }
            else
            {
                if (String.Compare(previousAnalysis, "Canny") == 0)
                {
                    _radioSelected = "Canny";
                    _propertyAnalysis = MVL_ANALYSIS.MVL_CANNY_ANALYSIS;
                    radioButton1.Checked = true;
                }
                else if (String.Compare(previousAnalysis, "Gradiente") == 0)
                {
                    _radioSelected = "Gradiente";
                    _propertyAnalysis = MVL_ANALYSIS.MVL_GRADIENTE_ANALYSIS;
                    radioButton2.Checked = true;
                }
                else if (String.Compare(previousAnalysis, "Sift") == 0)
                {
                    _radioSelected = "Sift";
                    _propertyAnalysis = MVL_ANALYSIS.MVL_SIFT_ANALYSIS;
                    radioButton3.Checked = true;
                }
            }
        }

        private void radioButton1_CheckedChanged(object sender, EventArgs e)
        {
            _radioSelected = "Canny";
            if (_propertyAnalysis != MVL_ANALYSIS.MVL_CANNY_ANALYSIS)
            {
                _parentForm.SetMVLEdgeSelection(_propertyAnalysis, MVL_VALUE_TYPE.MVL_VALUE_DISABLED);
            }
            _propertyAnalysis = MVL_ANALYSIS.MVL_CANNY_ANALYSIS;
        }

        private void radioButton2_CheckedChanged(object sender, EventArgs e)
        {
            _radioSelected = "Gradiente";
            if (_propertyAnalysis != MVL_ANALYSIS.MVL_GRADIENTE_ANALYSIS)
            {
                _parentForm.SetMVLEdgeSelection(_propertyAnalysis, MVL_VALUE_TYPE.MVL_VALUE_DISABLED);
            }
            _propertyAnalysis = MVL_ANALYSIS.MVL_GRADIENTE_ANALYSIS;
        }

        private void radioButton3_CheckedChanged(object sender, EventArgs e)
        {
            _radioSelected = "Sift";
            if (_propertyAnalysis != MVL_ANALYSIS.MVL_SIFT_ANALYSIS)
            {
                _parentForm.SetMVLEdgeSelection(_propertyAnalysis, MVL_VALUE_TYPE.MVL_VALUE_DISABLED);
            }
            _propertyAnalysis = MVL_ANALYSIS.MVL_SIFT_ANALYSIS;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _parentForm.SaveEdgeSelected(ref _radioSelected, _propertyAnalysis);
            this.Close();
        }

        private void SelectEdgeDetection_Deactivate(object sender, EventArgs e)
        {
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }

        private void SelectEdgeDetection_FormClosed(object sender, FormClosedEventArgs e)
        {
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }
    }
}

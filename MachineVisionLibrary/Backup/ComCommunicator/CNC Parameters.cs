using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace ComCommunicator
{
    public delegate void EndNormalTransferDelegate(bool bValue, ref string[] response);

    public partial class CNC_Parameters : Form
    {
        private int _nMaxXVal = -1;
        private int _nMaxYVal = -1;
        private int _nMaxZVal = -1;

        private uint _nDrillDiameter = 0;

        private int _nMotorID = -1;
        private MotorSettings[] _pMotorSettings = new MotorSettings[3];

        private bool _bAskMaxImageSize = false;
        private bool _bSettingsParameters = false;
        private bool _bGetMotorParameters = false;
        private bool _bGetDrillDiameter = false;
        private bool _bSetDrillDiameter = false;

        private string[] _responseCommands = null;
        private bool _bIsInitializing = false;

        private Form1 _parentForm = null;

        private enum CNC_PARAMETERS_ID
        {
            UNDEFINED_SETTINGS   = -1,
            SLOW_FREQUENCY = 0,
            HIGH_FREQUENCY,
            NUM_ACCELERATION_STEP,
            NUM_DECELERATION_STEP,
            MICRO_STEP_VALUE
        };

        public CNC_Parameters(Form1 form)
        {
            InitializeComponent();

            _parentForm = form;
            _parentForm.Enabled = false;

            this.Enabled = true;
            this.Show();
            this.Focus();

            _bIsInitializing = true;

            for (int i = 0; i < _pMotorSettings.Length; i++ )
            {
                _pMotorSettings[i] = new MotorSettings();
            }

            // Set Motor Identification
            comboBoxMotorID.BeginUpdate();
            for (int i = 0; i < 3; i++ )
            {
                comboBoxMotorID.Items.Add(i);
            }
            comboBoxMotorID.EndUpdate();
            comboBoxMotorID.SelectedIndex = comboBoxMotorID.FindString("0");

            // At the beginning obtain initial parameters from CNC
            GetMotorParameters();
        }

        private void RestoreDefaultParameters()
        {
            for (int i = 0; i < 3; i++ )
            {
                _pMotorSettings[i]._unSlowFrequency = _pMotorSettings[i]._unSlowFrequencyDefaultVal;

                _pMotorSettings[i]._unHighFrequency = _pMotorSettings[i]._unHighFrequencyDefaultVal;

                _pMotorSettings[i]._unNumAccelerationSteps = _pMotorSettings[i]._unNumAccelerationStepsDefaultVal;

                _pMotorSettings[i]._unNumDecelerationStep = _pMotorSettings[i]._unNumDecelerationStepDefaultVal;

                _pMotorSettings[i]._unMicroStepValue = _pMotorSettings[i]._unMicroStepValueDefaultVal;
            }
        }

        private void UpdateParametersOnForm()
        {
            textBoxMaxX.Text = Convert.ToString(_nMaxXVal);
            textBoxMaxY.Text = Convert.ToString(_nMaxYVal);
            textBoxMaxZ.Text = Convert.ToString(_nMaxZVal);

            textBoxDrillDiameter.Text = Convert.ToString(_nDrillDiameter);

            // Set max coordinates to parent form
            _parentForm.X_Max_Val = _nMaxXVal;
            _parentForm.Y_Max_Val = _nMaxYVal;
            _parentForm.Z_Max_Val = _nMaxZVal;

            textBoxSlowFrequency.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unSlowFrequency);
            textBoxHighFrequency.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unHighFrequency);
            textBoxAcceleration.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unNumAccelerationSteps);
            textBoxDecStep.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unNumDecelerationStep);
            textBoxMicroStep.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unMicroStepValue);

            textBoxSlowDefault.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unSlowFrequencyDefaultVal);
            textBoxHighDefault.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unHighFrequencyDefaultVal);
            textBoxAccDefault.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unNumAccelerationStepsDefaultVal);
            textBoxDecDefault.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unNumDecelerationStepDefaultVal);
            textBoxMicroStepDefault.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unMicroStepValueDefaultVal);
        }

        private void GetMotorParameters()
        {
            string[] sData = new string[15];
            int nMotorID = 0;
            int nCount = 0;
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

            this.Enabled = false;

            for (nMotorID = 0; nMotorID < 3; nMotorID++ )
            {
                sData[nCount++] = "@gS" + Convert.ToString(nMotorID) + ";";
                sData[nCount++] = "@gR" + Convert.ToString(nMotorID) + ";";
                sData[nCount++] = "@gA" + Convert.ToString(nMotorID) + ";";
                sData[nCount++] = "@gD" + Convert.ToString(nMotorID) + ";";
                sData[nCount++] = "@gM" + Convert.ToString(nMotorID) + ";";
            }

            _bGetMotorParameters = true;
            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
        }

        private void ChangeCNCParameter(CNC_PARAMETERS_ID eID)
        {
            string[] sData = new string[1];
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

            this.Enabled = false;

            switch (eID)
            {
                case CNC_PARAMETERS_ID.SLOW_FREQUENCY:
                    sData[0] = "@iP" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unSlowFrequency).PadLeft(6, '0') + ";";
                    break;

                case CNC_PARAMETERS_ID.HIGH_FREQUENCY:
                    sData[0] = "@iR" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unHighFrequency).PadLeft(6, '0') + ";";
                    break;

                case CNC_PARAMETERS_ID.NUM_ACCELERATION_STEP:
                    sData[0] = "@iA" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unNumAccelerationSteps).PadLeft(6, '0') + ";";
                    break;

                case CNC_PARAMETERS_ID.NUM_DECELERATION_STEP:
                    sData[0] = "@iD" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unNumDecelerationStep).PadLeft(6, '0') + ";";
                    break;

                case CNC_PARAMETERS_ID.MICRO_STEP_VALUE:
                    sData[0] = "@iM" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unMicroStepValue).PadLeft(6, '0') + ";";
                    break;

                default:
                    return;
            }

            _bGetMotorParameters = true;
            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
        }

        private void CheckParametersValidityAndApply(TextBox sender)
        {
            uint nValue = 0;
            CNC_PARAMETERS_ID ID = CNC_PARAMETERS_ID.UNDEFINED_SETTINGS;

            nValue = Convert.ToUInt32(sender.Text);

            if (sender == textBoxSlowFrequency)
            {
                ID = CNC_PARAMETERS_ID.SLOW_FREQUENCY;
            }
            else if (sender == textBoxHighFrequency)
            {
                ID = CNC_PARAMETERS_ID.HIGH_FREQUENCY;
            }
            else if (sender == textBoxAcceleration)
            {
                ID = CNC_PARAMETERS_ID.NUM_ACCELERATION_STEP;
            }
            else if (sender == textBoxDecStep)
            {
                ID = CNC_PARAMETERS_ID.NUM_DECELERATION_STEP;
            }
            else if (sender == textBoxMicroStep)
            {
                ID = CNC_PARAMETERS_ID.MICRO_STEP_VALUE;
            }

            switch (ID)
            {
                case CNC_PARAMETERS_ID.SLOW_FREQUENCY:
                    if ((nValue != _pMotorSettings[_nMotorID]._unSlowFrequency) && (nValue < _pMotorSettings[_nMotorID]._unHighFrequency))
                    {
                        _pMotorSettings[_nMotorID]._unSlowFrequency = nValue;
                    }
                    else
                    {
                        textBoxSlowFrequency.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unSlowFrequency);
                        return;
                    }
                    break;

                case CNC_PARAMETERS_ID.HIGH_FREQUENCY:
                    if ((nValue != _pMotorSettings[_nMotorID]._unHighFrequency) && (nValue > _pMotorSettings[_nMotorID]._unSlowFrequency))
                    {
                        _pMotorSettings[_nMotorID]._unHighFrequency = nValue;
                    }
                    else
                    {
                        textBoxHighFrequency.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unHighFrequency);
                        return;
                    }
                    break;

                case CNC_PARAMETERS_ID.NUM_ACCELERATION_STEP:
                    if (nValue != _pMotorSettings[_nMotorID]._unNumAccelerationSteps)
                    {
                        _pMotorSettings[_nMotorID]._unNumAccelerationSteps = nValue;
                    }
                    break;

                case CNC_PARAMETERS_ID.NUM_DECELERATION_STEP:
                    if (nValue != _pMotorSettings[_nMotorID]._unNumDecelerationStep)
                    {
                        _pMotorSettings[_nMotorID]._unNumDecelerationStep = nValue;
                    }
                    break;

                case CNC_PARAMETERS_ID.MICRO_STEP_VALUE:
                    if ((nValue != _pMotorSettings[_nMotorID]._unMicroStepValue) && (nValue % 2 == 0))
                    {
                        _pMotorSettings[_nMotorID]._unMicroStepValue = nValue;
                    }
                    else
                    {
                        textBoxMicroStep.Text = Convert.ToString(_pMotorSettings[_nMotorID]._unMicroStepValue);
                        return;
                    }
                    break;

                default:
                    return;
            }

            // Apply new value
            //ChangeCNCParameter(ID);
        }

        private void textBox_TextChanged(object sender, EventArgs e)
        {
            TextBox textBox = (TextBox)sender;

            if (_bIsInitializing == true)
            {
                return;
            }

            try
            {
                
            }
            catch (FormatException excp)
            {

            }
            catch (OverflowException excp)
            {

            }
            finally
            {
                CheckParametersValidityAndApply(textBox);
            }
        }

        private void CNC_Parameters_FormClosing(object sender, FormClosingEventArgs e)
        {
            this.Enabled = false;
            _parentForm.Enabled = true;
            _parentForm.Focus();
        }

        private void GetMaxImageSize()
        {
            string[] svImageSize = new string[1];
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

            this.Enabled = false;

            svImageSize[0] = "@gPMAX;";
            _bAskMaxImageSize = true;

            _parentForm.SendNormalCommands(svImageSize, _responseCommands, endNormalTrasferDelegate);
        }

        private void GetDrillDiameter()
        {
            string[] svImageSize = new string[1];
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

            this.Enabled = false;

            svImageSize[0] = "@gT;";
            _bGetDrillDiameter = true;

            _parentForm.SendNormalCommands(svImageSize, _responseCommands, endNormalTrasferDelegate);
        }

        private void buttonSetParameters_Click(object sender, EventArgs e)
        {
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;
            this.Enabled = false;
            _bSettingsParameters = true;

            string[] sData = new string[5];
                
            sData[0] = "@iP" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unSlowFrequency).PadLeft(6, '0') + ";";
            
            sData[1] = "@iR" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unHighFrequency).PadLeft(6, '0') + ";";
            
            sData[2] = "@iA" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unNumAccelerationSteps).PadLeft(6, '0') + ";";
            
            sData[3] = "@iD" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unNumDecelerationStep).PadLeft(6, '0') + ";";

            sData[4] = "@iM" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[_nMotorID]._unMicroStepValue).PadLeft(6, '0') + ";";

            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
        }

        private void SetAllMotorParameters()
        {
            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;
            this.Enabled = false;
            _bSettingsParameters = true;

            string[] sData = new string[15];
            int nCount = 0;

            for (int i = 0; i < 3; i++ )
            {
                sData[nCount++] = "@iP" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[i]._unSlowFrequency).PadLeft(6, '0') + ";";

                sData[nCount++] = "@iR" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[i]._unHighFrequency).PadLeft(6, '0') + ";";

                sData[nCount++] = "@iA" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[i]._unNumAccelerationSteps).PadLeft(6, '0') + ";";

                sData[nCount++] = "@iD" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[i]._unNumDecelerationStep).PadLeft(6, '0') + ";";

                sData[nCount++] = "@iM" + Convert.ToString(_nMotorID) + Convert.ToString(_pMotorSettings[i]._unMicroStepValue).PadLeft(6, '0') + ";";
            }

            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
        }

        public void EndOfSettingsParameters(bool bValue, ref string[] response)
        {
            this.Enabled = true;
            DialogResult result = DialogResult.Cancel;

            if (bValue == true)
            {
                // Check response vector
                if (_bAskMaxImageSize == true)
                {
                    _bAskMaxImageSize = false;

                    if ((response.Length > 0) && (response[0].StartsWith("@pMX") == true))
                    {
                        _nMaxXVal = Convert.ToInt32(response[0].Substring(4, 6));
                        _nMaxYVal = Convert.ToInt32(response[0].Substring(5 + 6, 6));
                        _nMaxZVal = Convert.ToInt32(response[0].Substring(6 + 12, 6));

                        _parentForm.X_Max_Val = _nMaxXVal;
                        _parentForm.Y_Max_Val = _nMaxYVal;
                        _parentForm.Z_Max_Val = _nMaxZVal;

                        if (_bIsInitializing == true)
                        {
                            _bIsInitializing = false;
                            GetDrillDiameter();
                        }
                        else
                        {
                            UpdateParametersOnForm();
                        }
                    }
                    else
                    {
                        result = MessageBox.Show("It's not possible obtain max image size", "Retry",
                                MessageBoxButtons.RetryCancel);
                        if (result == DialogResult.Retry)
                        {
                            GetMaxImageSize();
                        }
                    }
                }
                else if (_bSettingsParameters == true)
                {
                    _bSettingsParameters = false;

                    if (response.Length > 0)
                    {
                        foreach (var item in response)
                        {
                            if (String.Compare("@OK;", item) != 0)
                            {
                                result = MessageBox.Show("Error occurred during settings", "Retry",
                                MessageBoxButtons.RetryCancel);
                                if (result == DialogResult.Retry)
                                {
                                    buttonSetParameters_Click(this, EventArgs.Empty);
                                }
                                break;
                            }
                        }
                    }
                }
                else if (_bGetMotorParameters == true)
                {
                    _bGetMotorParameters = false;

                    foreach (var item in response)
                    {
                        uint nMotorID = 0;
                        string sMotorID = item.Substring(2, 1);
                        uint unValue = 0;
                        string sDataValue = item.Substring(3, 6);

                        if (UInt32.TryParse(sMotorID, out nMotorID) == false)
                        {
                            return;
                        }

                        if (UInt32.TryParse(sDataValue, out unValue) == false)
                        {
                            return;
                        }

                        if (item.Contains("@S") == true)
                        {
                            _pMotorSettings[nMotorID]._unSlowFrequency = unValue;
                        }
                        else if (item.Contains("@R") == true)
                        {
                            _pMotorSettings[nMotorID]._unHighFrequency = unValue;
                        }
                        else if (item.Contains("@A") == true)
                        {
                            _pMotorSettings[nMotorID]._unNumAccelerationSteps = unValue;
                        }
                        else if (item.Contains("@D") == true)
                        {
                            _pMotorSettings[nMotorID]._unNumDecelerationStep = unValue;
                        }
                        else if (item.Contains("@M") == true)
                        {
                            _pMotorSettings[nMotorID]._unMicroStepValue = unValue;
                        }
                    }

                    if (_bIsInitializing == true)
                    {
                        SetMotorDefaultParameters();
                        GetMaxImageSize();
                    } 
                    else
                    {
                        UpdateParametersOnForm();
                    }
                }
                else if (_bGetDrillDiameter == true)
                {
                    _bGetDrillDiameter = false;

                    if ((response.Length > 0) && (response[0].StartsWith("@T") == true))
                    {
                        _nDrillDiameter = Convert.ToUInt32(response[0].Substring(2, 6));

                        if (_bIsInitializing == true)
                        {
                            _bIsInitializing = false;
                            UpdateParametersOnForm();
                        }
                        else
                        {
                            UpdateParametersOnForm();
                        }
                    }
                    else
                    {
                        result = MessageBox.Show("It's not possible obtain max image size", "Retry",
                                MessageBoxButtons.RetryCancel);
                        if (result == DialogResult.Retry)
                        {
                            GetDrillDiameter();
                        }
                    }
                }
                else if (_bSetDrillDiameter == true)
                {
                    _bSetDrillDiameter = false;

                    if (response[0].StartsWith("@OK;") != true)
                    {
                        result = MessageBox.Show("It's not possible set drill diameter", "Retry",
                                MessageBoxButtons.RetryCancel);
                        if (result == DialogResult.Retry)
                        {
                            string[] sData = new string[1];
                            EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

                            this.Enabled = false;

                            sData[0] = "@iT" + Convert.ToString(_nDrillDiameter).PadLeft(6, '0') + ";";
                            _bSetDrillDiameter = true;

                            _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
                        }
                    }
                }
            }
            else
            {
                // Check response vector
                if (_bAskMaxImageSize == true)
                {
                    _bAskMaxImageSize = false;

                    result = MessageBox.Show("It's not possible obtain max image size", "Retry",
                                MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        GetMaxImageSize();
                    }
                }
                else if (_bSettingsParameters == true)
                {
                    _bSettingsParameters = false;

                    result = MessageBox.Show("It's not possible settings the parameters", "Retry",
                                MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        buttonSetParameters_Click(this, EventArgs.Empty);
                    }
                }
                else if (_bGetMotorParameters == true)
                {
                    _bGetMotorParameters = false;

                    result = MessageBox.Show("It's not possible obtain parameters", "Retry",
                                MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        GetMotorParameters();
                    }
                }
                else if (_bGetDrillDiameter == true)
                {
                    _bGetDrillDiameter = false;

                    result = MessageBox.Show("It's not possible obtain drill diameter", "Retry",
                                MessageBoxButtons.RetryCancel);
                    if (result == DialogResult.Retry)
                    {
                        GetDrillDiameter();
                    }
                }
            }
        }

        private void SetMotorDefaultParameters()
        {
            for (int i = 0; i < 3; i++ )
            {
                _pMotorSettings[i]._unSlowFrequencyDefaultVal = _pMotorSettings[i]._unSlowFrequency;
                _pMotorSettings[i]._unHighFrequencyDefaultVal = _pMotorSettings[i]._unHighFrequency;
                _pMotorSettings[i]._unNumAccelerationStepsDefaultVal = _pMotorSettings[i]._unNumAccelerationSteps;
                _pMotorSettings[i]._unNumDecelerationStepDefaultVal = _pMotorSettings[i]._unNumDecelerationStep;
                _pMotorSettings[i]._unMicroStepValueDefaultVal = _pMotorSettings[i]._unMicroStepValue;
            }
        }

        private void buttonRestoreDefault_Click(object sender, EventArgs e)
        {
            RestoreDefaultParameters();
            UpdateParametersOnForm();
            SetAllMotorParameters();
        }

        private void buttonGetImageSize_Click(object sender, EventArgs e)
        {
            GetMaxImageSize();
        }

        private void CNC_Parameters_Load(object sender, EventArgs e)
        {

        }

        private void comboBoxMotorID_SelectedIndexChanged(object sender, EventArgs e)
        {
            _nMotorID = Convert.ToInt32(comboBoxMotorID.GetItemText(comboBoxMotorID.SelectedIndex));

            // Change parameters showed on form
            UpdateParametersOnForm();
        }

        private void buttonDrill_Click(object sender, EventArgs e)
        {
            uint nValue = 0;
            nValue = Convert.ToUInt32(textBoxDrillDiameter.Text);
            if (nValue > 0)
            {
                _nDrillDiameter = nValue;

                string[] sData = new string[1];
                EndNormalTransferDelegate endNormalTrasferDelegate = EndOfSettingsParameters;

                this.Enabled = false;

                sData[0] = "@iT" + Convert.ToString(_nDrillDiameter).PadLeft(6, '0') + ";";
                _bSetDrillDiameter = true;

                _parentForm.SendNormalCommands(sData, _responseCommands, endNormalTrasferDelegate);
            }
        }
    }

    public class MotorSettings
    {
        public uint _unSlowFrequency = 0;
        public uint _unSlowFrequencyDefaultVal = 0;
        public uint _unHighFrequency = 0;
        public uint _unHighFrequencyDefaultVal = 0;
        public uint _unNumAccelerationSteps = 0;
        public uint _unNumAccelerationStepsDefaultVal = 0;
        public uint _unNumDecelerationStep = 0;
        public uint _unNumDecelerationStepDefaultVal = 0;
        public uint _unMicroStepValue = 0;
        public uint _unMicroStepValueDefaultVal = 0;
    }
}

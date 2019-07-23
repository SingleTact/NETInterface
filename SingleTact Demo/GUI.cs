//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using ZedGraph;
using System.Threading;
using System.Globalization;
using SingleTactLibrary;
using System.Diagnostics;

namespace SingleTact_Demo
{
    public partial class GUI : Form
    {
        private bool backgroundIsFinished_ = false;  // Flag to check background thread is finished
        private double measuredFrequency_ = 50;  // Sensor update rate
        private int timerItr_ = 0;  // Some things are slower that the timer frequency
        private bool isFirstFrame_ = true; // Is first frame after boot
        private const int graphXRange_ = 30; // 30 seconds
        private const int reservedAddresses = 4; // Don't use I2C addresses 0 to 3
        private Object workThreadLock = new Object(); //Thread synchronization
        List<string> serialPortNames = new List<string>();
        private List<USBdevice> USBdevices = new List<USBdevice>();
        private SingleTact activeSingleTact;
        private double lastReadingTime = 0.0;
        private delegate void CloseMainFormDelegate(); //Used to close the program if hardware is not connected

        public GUI()
        {
            bool sensorStarted = false;
            string serialPortName = null;
            string exceptionMessage = null;

            InitializeComponent();

            // Get available serial ports.
            string[] ports = SerialPort.GetPortNames();
            if (0 != ports.Length)
            {
                // Assume Arduino is on the first port during startup.
                serialPortName = ports[0];
                try
                {
                    // if there's more than one device connected
                    if (ports.Length > 1)
                    {
                        // Ask user to select from multiple serial ports.
                        SerialPortSelector selector = new SerialPortSelector(ports);
                        selector.ShowDialog();
                        serialPortNames = selector.SelectedPorts;

                        foreach (string portName in serialPortNames)
                        {
                            USBdevice USB = new USBdevice();
                            USB.Initialise(portName);
                            USBdevices.Add(USB);
                        }
                    }
                    else  // there is only one device connected
                    {
                        serialPortNames.Add(serialPortName);
                        USBdevice USB = new USBdevice();
                        USB.Initialise(serialPortName);
                        USBdevices.Add(USB);
                    }

                }
                catch (Exception ex)
                {
                    exceptionMessage = ex.Message;
                }

                PopulateSetComboBoxes();

            }

            try
            {
                foreach (USBdevice USB in USBdevices)
                {
                    activeSingleTact = USB.singleTact;
                    RefreshFlashSettings_Click(this, null); //Get the settings from flash
                }
                CreateStripChart();
                AcquisitionWorker.RunWorkerAsync(); //Start the acquisition thread

                guiTimer_.Start();
            }
            catch
            {
                string summary = "Failed to start sensor";

                if (serialPortName == null)
                    summary += ": no serial ports detected.";
                else
                    summary += " on " + serialPortName + ".";

                summary += "\n\n";

                if (exceptionMessage != null)
                    summary += exceptionMessage;
                else
                    summary += "Please connect the Arduino then restart this application.";

                MessageBox.Show(
                    summary,
                    "Hardware initialisation failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                // There's no point showing the GUI.  Force the app to auto-close.
                this.Shown += new EventHandler(this.CloseOnStart);
            }
        }


        private void CloseOnStart(object sender, EventArgs e)
        {
            this.Close();
        }


        /// <summary>
        /// Fill appropriate Values into GUI Comboboxes
        /// </summary>
        private void PopulateSetComboBoxes()
        {
            //Populate active sensor combobox
            foreach (string port in serialPortNames)
                ActiveSensor.Items.Add(port);

            // Populate i2c addresses
            i2cAddressInputComboBox_.Items.Clear();

            for (int i = reservedAddresses; i < 128; i++)
            {
                i2cAddressInputComboBox_.Items
                                        .Add("0x" + i.ToString("X2"));
            }

            ScaleInputValueLabel.Text = (scaleInputTrackBar_.Value/100.0)
                                        .ToString("#0.00");

            sensorTypeSelector.Items.Add("8mm XX PSI");
            sensorTypeSelector.Items.Add("8mm XX PSI");
            sensorTypeSelector.Items.Add("8mm XX PSI");
            sensorTypeSelector.Items.Add("15mm XX PSI");
            sensorTypeSelector.Items.Add("15mm XX PSI");
            sensorTypeSelector.Items.Add("15mm XX PSI");
            sensorTypeSelector.SelectedIndex = 0;
        }

        /// <summary>
        /// Using ZedGraph, create a stripchart
        /// More info on Zedgraph can be found at: http://zedgraph.sourceforge.net/index.html
        /// </summary>
        void CreateStripChart()
        {
            // This is to remove all plots
            graph_.GraphPane.CurveList.Clear();

            // GraphPane object holds one or more Curve objects (or plots)
            GraphPane myPane = graph_.GraphPane;

            // Draw a box item to highlight the valid range
            BoxObj box = new BoxObj(0, 512, 30, 512, Color.Empty,
                                    Color.FromArgb(150, Color.LightGreen));
            box.Fill = new Fill(Color.FromArgb(200, Color.LightGreen),
                                Color.FromArgb(200, Color.LightGreen), 45.0F);
            // Use the BehindGrid zorder to draw the highlight beneath the grid lines
            box.ZOrder = ZOrder.F_BehindGrid;
            myPane.GraphObjList.Add(box);

            //Lables
            myPane.XAxis.Title.Text = "Time (s)";
            myPane.YAxis.Title.Text = "Output (511 = Full Scale Range)";
            myPane.Title.IsVisible = false;
            myPane.Legend.IsVisible = true;

            //Set scale
            myPane.XAxis.Scale.Max = 20; //Show 30s of data
            myPane.XAxis.Scale.Min = -10;

            //Set scale
            myPane.YAxis.Scale.Max = 768; //Valid range
            myPane.YAxis.Scale.Min = -255;

            //Make grid visible
            myPane.YAxis.MajorGrid.IsVisible = true;
            myPane.YAxis.MinorGrid.IsVisible = true;

            // Refeshes the plot.
            graph_.AxisChange();
            graph_.Invalidate();
            graph_.Refresh();
        }

        /// <summary>
        /// Add new measurements to the graph
        /// </summary>
        /// <param name="time"></param>
        /// <param name="measurements"></param>
        private void AddData(double time, double[] measurements, USBdevice USB)
        {
            USB.dataBuffer.AddData(measurements, time);  // update data
        }

        private void updateGraph(USBdevice USB)
        {
            int index = USBdevices.IndexOf(USB); // get current sensor number
            SingleTactData data_pt = USB.dataBuffer;

            // start graphing
            GraphPane graphPane = graph_.GraphPane;
            Color[] colours = { Color.Blue, Color.Orange, Color.DeepPink, Color.Olive, Color.ForestGreen };
            while (graphPane.CurveList.Count <= index)
            {
                string name = "Sensor " + (graphPane.CurveList.Count + 1).ToString() + " - " + serialPortNames[index].ToString();
                LineItem myCurve = new LineItem(
                    name,
                    data_pt.data[0],
                    colours[index],
                    SymbolType.None,
                    3.0f);
                graphPane.CurveList.Add(myCurve);
            }

            // This is to update the max and min value
            if (isFirstFrame_)
            {
                graphPane.XAxis.Scale.Min = data_pt.MostRecentTime;
                graphPane.XAxis.Scale.Max = graphPane.XAxis.Scale.Min + graphXRange_;
                isFirstFrame_ = false;
            }

            if (data_pt.MostRecentTime >= graphPane.XAxis.Scale.Max)
            {
                graphPane.XAxis.Scale.Max = data_pt.MostRecentTime;
                graphPane.XAxis.Scale.Min = graphPane.XAxis.Scale.Max - graphXRange_;
            }

            //Update green valid region box
            BoxObj b = (BoxObj)graphPane.GraphObjList[0];
            b.Location.X = graphPane.XAxis.Scale.Max - graphXRange_;
            b.Location.Y = Math.Min(graphPane.YAxis.Scale.Max, 512);
            b.Location.Height = Math.Min(graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min, 512);

            graph_.Refresh();
            graph_.AxisChange();
        }

        /// <summary>
        /// Finds a character to separate exported values.
        /// </summary>
        /// <returns>A semi-colon for locales where comma is the decimal 
        /// separator; otherwise a comma.</returns>
        private string safeSeparator()
        {
            double testValue = 3.14;
            string testText = testValue.ToString();

            if (testText.IndexOf(',') < 0)
                return ",";

            return ";";
        }

        //Save data to CSV
        private void buttonSave_Click(object sender, EventArgs e)
        {
            bool backgroundWasRunning = AcquisitionWorker.IsBusy;

            if (backgroundWasRunning)
            {
                StopAcquisitionThread();
            }

            // Initialize the Dialog to save the file
            SaveFileDialog saveDataDialog = new SaveFileDialog();
            saveDataDialog.Filter = "*.csv|*.csv";
            saveDataDialog.RestoreDirectory = true;
            saveDataDialog.FileName = "SingleTactSampleData";

            if (saveDataDialog.ShowDialog() == DialogResult.OK)
            {
                // To fix Issue 3, separate exported values by semi-colon instead
                // of comma if current locale's decimal separator is comma.
                string separator = safeSeparator();
                string saveFileName = saveDataDialog.FileName;
                StreamWriter dataWriter = new StreamWriter(saveFileName, false, Encoding.Default);

                // write column headers
                string columnNames = "Time(s)" + separator;
                // populate columns with serial port names
                foreach(string portName in serialPortNames)
                {
                    columnNames += portName + " (PSI)" + separator;
                }
                columnNames += "NB (0 = 0 PSI;  511 = Full Scale Range)";
                dataWriter.WriteLine(columnNames);

                // write data
                string row = "";
                int data_length = USBdevices[0].dataBuffer.data[0].Count;
                for (int i = 0; i < data_length; i++)  // for each sensor reading
                {
                    bool first = true;
                    foreach (USBdevice USB in USBdevices)
                    {
                        try
                        {
                            SingleTactData data = USB.dataBuffer;
                            PointPair dataPoint = data.data[0][i];
                            if (first) // only save the time the first sensor's reading was taken
                            {
                                // round the time value to mitigate any uncertainty around sampling time
                                row += Math.Round(dataPoint.X, 3) + separator + dataPoint.Y + separator;
                                first = false;
                            }
                            else
                            {
                                row += dataPoint.Y + separator;
                            }
                        }
                        catch  // index out of range, unequal number of sensor readings
                        {
                            row += "null" + separator;
                        }
                    }
                    dataWriter.WriteLine(row);
                    row = "";
                }

                dataWriter.Close();
            }

            if (backgroundWasRunning)
            {
                StartAcquisitionThread();
            }
        }

        private void tareAllButton_Click(object sender, EventArgs e)
        {
            StopAcquisitionThread();
            foreach (USBdevice USB in USBdevices)
            {
                SingleTact singletact = USB.singleTact;
                if (singletact.Tare())
                {
                    RefreshFlashSettings_Click(null, null);
                }
            }
            StartAcquisitionThread();
        }

        /// <summary>
        /// Stop acquisition thread
        /// </summary>
        private void StopAcquisitionThread()
        {
            AcquisitionWorker.CancelAsync();
            while (false == backgroundIsFinished_)
            {
                System.Windows.Forms.Application.DoEvents(); //Wait for us to finish
                Thread.Sleep(1);
            }
        }

        /// <summary>
        /// Stop the Acquisition Thread
        /// </summary>
        private void StartAcquisitionThread()
        {
            AcquisitionWorker.RunWorkerAsync();
        }

        /// <summary>
        /// Do work - process sensor data
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcquisitionWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            backgroundIsFinished_ = false;
            BackgroundWorker worker = sender as BackgroundWorker;

            while (!worker.CancellationPending) //Do the work
            {
                foreach (USBdevice USB in USBdevices)
                {
                    SingleTact singleTact = USB.singleTact;
                    SingleTactFrame newFrame = singleTact.ReadSensorData(); //Get sensor data

                    if (null != newFrame) //If we have data
                    {
                        lock (workThreadLock)
                        {
                            USB.addFrame(newFrame.DeepClone());
                        }

                        //Calculate rate
                        double delta = newFrame.TimeStamp - lastReadingTime; // calculate delta relative to previous sensor's last reading
                        if (delta > 0)
                            measuredFrequency_ = measuredFrequency_ * 0.95 + 0.05 * (1.0 / (delta));  //Averaging
                        lastReadingTime = newFrame.TimeStamp;
                        USB.setTimestamp(newFrame.TimeStamp);
                    }
                }
            }
        }

        /// <summary>
        /// Update plot
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void guiTimer__Tick(object sender, EventArgs e)
        {
            try
            {
                lock (workThreadLock)
                {
                    foreach (USBdevice USB in USBdevices)
                    {
                        List<SingleTactFrame> newFrames_ = USB.frameList;
                        SingleTactFrame localCopy = null;

                        localCopy = newFrames_[0];
                        newFrames_.RemoveAt(0);
                        newFrames_.TrimExcess();

                        if (localCopy != null)
                            // use first timestamp only to quantise readings and match csv output
                            AddData(USBdevices[0].lastTimeStamp, localCopy.SensorData, USB); //Add to stripchart

                        updateGraph(USB);
                    }
                }
            }
            catch (Exception) {}

            //Update update rate
            timerItr_++;
            if (0 == timerItr_ % 5)
            this.Text = "PPS SingleTact Demo [ " + measuredFrequency_.ToString("##0") + " Hz ]";
        }

        /// <summary>
        /// Done - we are closing down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcquisitionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundIsFinished_ = true;
        }

        private void GUI_FormClosing(object sender, FormClosingEventArgs e)
        {
            if(AcquisitionWorker.IsBusy)
                StopAcquisitionThread();
        }

        private void scaleInputTrackBar__Scroll(object sender, EventArgs e)
        {
            ScaleInputValueLabel.Text = (scaleInputTrackBar_.Value/100.0).ToString("#0.00");
        }

        /// <summary>
        /// Get settings from sensor flash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void RefreshFlashSettings_Click(object sender, EventArgs e)
        {
            bool backgroundWasRunning = AcquisitionWorker.IsBusy;

            if (backgroundWasRunning)
            StopAcquisitionThread();

            if (!activeSingleTact.PullSettingsFromHardware())
            {
                MessageBox.Show(
                    "Failed to retrieve current settings.",
                    "Error!",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error);
            }
            else
            {
                try
                {
                    textAddress.Text = "0x" + activeSingleTact.Settings.I2CAddress.ToString("X2");
                    textGain.Text = activeSingleTact.Settings.ReferenceGain.ToString("00") + " (" + (activeSingleTact.Settings.ReferenceGain + 1).ToString("0") + "x)";
                    textScale.Text = activeSingleTact.Settings.Scaling.ToString("00") + " (" + ((activeSingleTact.Settings.Scaling) / 100.0).ToString("#0.00") + ")";
                    textTare.Text = activeSingleTact.Settings.Baselines.ElementAt(0).ToString("0000");

                    i2cAddressInputComboBox_.SelectedIndex = activeSingleTact.Settings.I2CAddress - reservedAddresses;

                    scaleInputTrackBar_.Value = activeSingleTact.Settings.Scaling;
                    ScaleInputValueLabel.Text = (scaleInputTrackBar_.Value / 100.0).ToString("#0.00");
                }
                catch (Exception)
                {
                    MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }

            ResetSensorButton.Enabled = (activeSingleTact.Settings.Reserved == 0) ? true : false;
            scaleInputTrackBar_.Enabled = (activeSingleTact.Settings.Reserved == 0) ? true : false;
            
            LockButton.Text = (activeSingleTact.Settings.Reserved == 0) ? "Lock" : "Unlock";

            if (backgroundWasRunning)
            {
                StartAcquisitionThread();
            }
        }

        /// <summary>
        /// Set settings to sensor flash
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetSettingsButton_Click(object sender, EventArgs e)
        {
            bool backgroundWasRunning = AcquisitionWorker.IsBusy;

            if (backgroundWasRunning)
            StopAcquisitionThread();

            try
            {
                // ReferenceGain still has value from PullSettingsFromHardware
                // which is OK because the firmware fully controls this anyway.
                activeSingleTact.Settings.Scaling = (UInt16)(scaleInputTrackBar_.Value);
                activeSingleTact.Settings.I2CAddress = (byte)(i2cAddressInputComboBox_.SelectedIndex + reservedAddresses);
                activeSingleTact.Settings.Accumulator = 5;
                //singleTact_.Settings.Baselines =

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            activeSingleTact.PushSettingsToHardware();
            if (activeSingleTact.Tare())
            {
                RefreshFlashSettings_Click(this, null);
            }

            if (backgroundWasRunning)
            {
                StartAcquisitionThread();
            }
        }

        /// <summary>
        /// Reset sensor to a default configuration
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ResetSensorButton_Click(object sender, EventArgs e)
        {
            string sensorType = (String)(sensorTypeSelector.Items[sensorTypeSelector.SelectedIndex]);

            DialogResult result = MessageBox.Show("Warning: this will reset your sensor to the default value for a "
                                + sensorType
                                + " SingleTact.  Do you want to proceed?",
                                "Reset Sensor",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Exclamation);

            if (result == DialogResult.Yes)
            {
                MessageBox.Show("ToDo...");
                //ToDo
            }
        }

        /// <summary>
        /// Update the sensor baseline
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SetBaselineButton_Click(object sender, EventArgs e)
        {
            StopAcquisitionThread();
            if (activeSingleTact.Tare())
            {
                RefreshFlashSettings_Click(null, null);
            }
            StartAcquisitionThread();
        }

        /// <summary>
        /// Lock the sensor if needed
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void LockButton_Click(object sender, EventArgs e)
        {
            // Get the Info that whether the settings in the MainRegister is locked or not
            bool backgroundWasRunning = AcquisitionWorker.IsBusy;

            if (backgroundWasRunning)
            StopAcquisitionThread();

            try
            {
                activeSingleTact.Settings.Reserved = (activeSingleTact.Settings.Reserved == 0) ? (byte)1 : (byte)0;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            activeSingleTact.PushSettingsToHardware();
            RefreshFlashSettings_Click(this, null);

            ResetSensorButton.Enabled = (activeSingleTact.Settings.Reserved == 0) ? true : false;
            scaleInputTrackBar_.Enabled = (activeSingleTact.Settings.Reserved == 0) ? true : false;

            LockButton.Text = (activeSingleTact.Settings.Reserved == 0) ? "Lock" : "Unlock";

            if (backgroundWasRunning)
            StartAcquisitionThread();
        }

        private void Send_Calibration_Click(object sender, EventArgs e)
        {
            // Get the Info that whether the settings in the MainRegister is locked or not
            bool backgroundWasRunning = AcquisitionWorker.IsBusy;
            int[] calibrationTable = new int[1024];

            if (backgroundWasRunning)
            StopAcquisitionThread();

            activeSingleTact.PushCalibrationToHardware(calibrationTable);

            if (backgroundWasRunning)
            StartAcquisitionThread();
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeSingleTact = USBdevices[ActiveSensor.SelectedIndex].singleTact;
            RefreshFlashSettings_Click(this, null); //Update display
        }

        private void label2_Click(object sender, EventArgs e)
        {

        }
    }
}

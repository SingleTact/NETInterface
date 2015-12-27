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

namespace SingleTact_Demo
{
    public partial class GUI : Form
    {
        private SingleTactData dataBuffer_ = new SingleTactData();  // Stripchart data
        private bool backgroundIsFinished_ = false;  // Flag to check background thread is finished
        private double measuredFrequency_ = 50;  // Sensor update rate
        private double lastTimestamp_ = 0; // Used to calculate update rate
        private int timerItr_ = 0;  // Some things are slower that the timer frequency
        private bool isFirstFrame_ = true; // Is first frame after boot
        private const int graphXRange_ = 30; // 30 seconds
        private Object workThreadLock = new Object(); //Thread synchronization
        List<SingleTactFrame> newFrames_ = new List<SingleTactFrame>();
        private delegate void CloseMainFormDelegate(); //Used to close the program if hardware is not connected

        public GUI()
        {
            InitializeComponent();
            PopulateSetComboBoxes();

            //Get available serial prot
            string[] ports = SerialPort.GetPortNames();
            if (0 != ports.Length)
            {
                try
                {
                    string serialPortName = ports[0];
                    if (ports.Length > 1)
                    {
                        SerialPortSelector selector = new SerialPortSelector(ports);
                        selector.ShowDialog();

                        serialPortName = selector.SelectedPort;
                    }

                    //Assume arduino is on the first COM port
                    arduinoSingleTactDriver.Initialise(serialPortName); //Start Arduino driver
                    singleTact_.Initialise(arduinoSingleTactDriver); //Attach to sensor
                }
                catch (Exception)
                {
                    MessageBox.Show("Failed to start sensor", "Hardware Initialisation Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    this.Shown += new EventHandler(this.CloseOnStart);
                }

                RefreshFlashSettings_Click(this, null); //Get the settings from flash
                CreateStripChart();

                AcquisitionWorker.RunWorkerAsync(); //Start the acquisition thread

                guiTimer_.Start();

            }
            else
            {
                MessageBox.Show("Error: No Serial Port.  Please ensure the Arduino is plugged in and restart the application.", "No Serial Port", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
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
            i2cAddressInputComboBox_.Items.Clear();
            refGainInputComboBox_.Items.Clear();

            for (int i = 0; i < 12; i++)
            {
                i2cAddressInputComboBox_.Items
                                        .Add("0x" + (i + 4).ToString("00"));
            }

            for (int i = 1; i <= 8; i++)
            {
                refGainInputComboBox_.Items.Add(i.ToString("0") + "x");
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
            myPane.YAxis.Title.Text = "Output (512 = Full Scale Range)";
            myPane.Title.IsVisible = false;
            myPane.Legend.IsVisible = false;

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
        public void AddData(double time, double[] measurements)
        {
            dataBuffer_.AddData(measurements, time);
            GraphPane graphPane = graph_.GraphPane;

            while (graphPane.CurveList.Count < measurements.Length)
            {
                string name = "Sensor " + (graphPane.CurveList.Count + 1).ToString();
                LineItem myCurve = new LineItem(
                    name,
                    dataBuffer_.data[graphPane.CurveList.Count],
                    Color.Blue,
                    SymbolType.None,
                    3.0f);
                graphPane.CurveList.Add(myCurve);
            }

            for (int i = 0; i < measurements.Length; i++)
            {
                graphPane.CurveList[i].Points = dataBuffer_.data[i];
            }

            // This is to update the max and min value
            if (isFirstFrame_)
            {
                graphPane.XAxis.Scale.Min = dataBuffer_.MostRecentTime;
                graphPane.XAxis.Scale.Max = graphPane.XAxis.Scale.Min + graphXRange_;
                isFirstFrame_ = false;
            }

            if (dataBuffer_.MostRecentTime >= graphPane.XAxis.Scale.Max)
            {
                graphPane.XAxis.Scale.Max = dataBuffer_.MostRecentTime;
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
                string saveFileName = saveDataDialog.FileName;
                StreamWriter dataWriter = new StreamWriter(saveFileName, false, Encoding.Default);

                //Write a header
                dataWriter.WriteLine("Time (s)" + "," + "Value (0 = 0 PSI  512 = Full Scale Range)");

                //Just export first trace (we only support 1 sensor)
                for (int i = 0; i < dataBuffer_.data[0].Count; i++)
                {
                    dataWriter.WriteLine(dataBuffer_.data[0][i].X + "," + dataBuffer_.data[0][i].Y);
                }

                dataWriter.Close();
            }

            if (backgroundWasRunning)
            {
                StartAcquisitionThread();
            }
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
                SingleTactFrame newFrame = singleTact_.ReadSensorData(); //Get sensor data

                if (null != newFrame) //If we have data
                {
                    lock (workThreadLock)
                    {
                        newFrames_.Add(newFrame.DeepClone());
                    }

                    //Calculate rate
                    double delta = newFrame.TimeStamp - lastTimestamp_;
                    measuredFrequency_ = measuredFrequency_ * 0.95 + 0.05 * (1.0 / (delta));  //Averaging
                    lastTimestamp_ = newFrame.TimeStamp;
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
                    SingleTactFrame localCopy = null;

                    while (newFrames_.Count != 0)
                    {
                        localCopy = newFrames_[0];
                        newFrames_.RemoveAt(0);
                        newFrames_.TrimExcess();

                        if (localCopy != null)
                        AddData(localCopy.TimeStamp, localCopy.SensorData); //Add to stripchart
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

            singleTact_.PullSettingsFromHardware();
            try
            {
                textAddress.Text = "0x" + singleTact_.Settings.I2CAddress.ToString("X2");
                textGain.Text = singleTact_.Settings.ReferenceGain.ToString("00") + " (" + (singleTact_.Settings.ReferenceGain + 1).ToString("0") + "x)";
                textScale.Text = singleTact_.Settings.Scaling.ToString("00") + " (" + ((singleTact_.Settings.Scaling) / 100.0).ToString("#0.00") + ")";
                textTare.Text = singleTact_.Settings.Baselines.ElementAt(0).ToString("0000");

                if (singleTact_.Settings.Baselines.ElementAt(0) > 65000)
                MessageBox.Show("Warning: Sensor raw baseline is really high, please reduce the reference gain.", "Warning: Baseline too high", MessageBoxButtons.OK);

                i2cAddressInputComboBox_.SelectedIndex = singleTact_.Settings.I2CAddress - 4;

                refGainInputComboBox_.SelectedIndex = singleTact_.Settings.ReferenceGain;

                scaleInputTrackBar_.Value = singleTact_.Settings.Scaling;
                ScaleInputValueLabel.Text = (scaleInputTrackBar_.Value/100.0).ToString("#0.00");
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            SetSettingsButton.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            i2cAddressInputComboBox_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            refGainInputComboBox_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            ResetSensorButton.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            scaleInputTrackBar_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;

            LockButton.Text = (singleTact_.Settings.Reserved == 0) ? "Lock" : "Unlock";

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
                singleTact_.Settings.ReferenceGain = (byte)(refGainInputComboBox_.SelectedIndex);
                singleTact_.Settings.Scaling = (UInt16)(scaleInputTrackBar_.Value);
                singleTact_.Settings.I2CAddress = (byte)(i2cAddressInputComboBox_.SelectedIndex + 4);
                singleTact_.Settings.Accumulator = 5;
                //singleTact_.Settings.Baselines =

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            singleTact_.PushSettingsToHardware();
            singleTact_.Tare();
            RefreshFlashSettings_Click(this, null);

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
            singleTact_.Tare();
            RefreshFlashSettings_Click(null, null);
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
                singleTact_.Settings.Reserved = (singleTact_.Settings.Reserved == 0) ? (byte)1 : (byte)0;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            singleTact_.PushSettingsToHardware();
            RefreshFlashSettings_Click(this, null);

            SetSettingsButton.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            i2cAddressInputComboBox_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            refGainInputComboBox_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            ResetSensorButton.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;
            scaleInputTrackBar_.Enabled = (singleTact_.Settings.Reserved == 0) ? true : false;

            LockButton.Text = (singleTact_.Settings.Reserved == 0) ? "Lock" : "Unlock";

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

            singleTact_.PushCalibrationToHardware(calibrationTable);

            if (backgroundWasRunning)
            StartAcquisitionThread();
        }
    }
}

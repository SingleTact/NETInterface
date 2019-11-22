//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO.Ports;
using System.IO;
using ZedGraph;
using System.Threading;
using SingleTactLibrary;
using System.Management;

namespace SingleTact_Demo
{
    public partial class GUI : Form
    {
        private string version = "GitHub Source";
        private bool backgroundIsFinished_ = false;  // Flag to check background thread is finished
        private double measuredFrequency_ = 50;  // Sensor update rate
        private int timerItr_ = 0;  // Some things are slower that the timer frequency
        private bool isFirstFrame_ = true; // Is first frame after boot
        private const int graphXRange_ = 30; // 30 seconds
        private const int reservedAddresses = 4; // Don't use I2C addresses 0 to 3
        private Object workThreadLock = new Object(); //Thread synchronization
        private List<string> serialPortNames = new List<string>();
        private  List<String> prettyPorts = new List<string>();
        private List<USBdevice> USBdevices = new List<USBdevice>();
        private SingleTact activeSingleTact;
        private delegate void CloseMainFormDelegate(); //Used to close the program if hardware is not connected

        public GUI()
        {

            string serialPortName = null;
            string exceptionMessage = null;

            InitializeComponent();

            // Get available serial ports.
            String[] ports = SerialPort.GetPortNames();
            prettyPorts = getPrettyPortNames(ports);

            if (0 != ports.Length)
            {
                // Assume Arduino is on the first port during startup.
                serialPortName = ports[0];
                bool portSelected = false;
                while (!portSelected)
                {
                    try
                    {
                        // if there's more than one PPS/Arduino device connected
                        if (prettyPorts.Count > 1)
                        {
                            // Ask user to select from multiple serial ports.
                            SerialPortSelector selector = new SerialPortSelector(prettyPorts);
                            selector.ShowDialog();
                            serialPortNames = selector.SelectedPorts;
                            prettyPorts = serialPortNames;  // remove unselected ports
                            List<String> comPorts = new List<string>();
                            foreach (String portName in serialPortNames)
                            {
                                comPorts.Add(prettyToComPort(portName));
                            }
                            serialPortNames = comPorts;

                            if (serialPortNames.Count == 0) // no port selected, loop
                            {
                                MessageBox.Show("Please select at least one port");
                            }

                            else
                            {

                                portSelected = true;

                                if (serialPortNames.Count == 1) // User selected one port only
                                {
                                    //hide GUI elements intended for multiple USBs
                                    updateUIforOneDevice();
                                }

                                foreach (string portName in serialPortNames)
                                {
                                    USBdevice USB = new USBdevice();
                                    USB.Initialise(portName);
                                    USBdevices.Add(USB);
                                }
                            }
                        }
                        else if (prettyPorts.Count == 1)  // there is only one device connected
                        {
                            portSelected = true;
                            serialPortNames.Add(serialPortName);
                            USBdevice USB = new USBdevice();
                            USB.Initialise(serialPortName);
                            USBdevices.Add(USB);
                            updateUIforOneDevice();
                        }
                    }
                    catch (Exception ex)
                    {
                        exceptionMessage = ex.Message;
                    }
                }
                PopulateGUIFields();
            }

            try
            {
                var usb = USBdevices[0];  // Force exception to occur if there are no USB devices
                foreach (USBdevice USB in USBdevices)
                {
                    USB.singleTact.PushSettingsToHardware();
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

        private void updateUIforOneDevice()
        {
            //hide GUI elements intended for multiple USBs
            tareAll.Text = "Tare";
            tareAll.Top -= 60;
            buttonSave.Top -= 60;
            Settings.Top -= 60;
            linkLabel1.Top -= 60;
            Settings.Height += 60;
            SetBaselineButton.Visible = false;
            ActiveSensorLabel.Visible = false;
            ActiveSensor.Visible = false;
        }

        private void CloseOnStart(object sender, EventArgs e)
        {
            this.Close();
        }

        private List<String> getPrettyPortNames(String[] serialPorts)
        {  // Get a human readable version of comport similar to device manager
            String name = "";
            List<String> details;
            List<String> names = new List<string>();

            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();
                details = (from n in serialPorts
                           join p in ports on n equals p["DeviceID"].ToString()
                           select n + " - " + p["Caption"]).ToList();
            }

            foreach (string detail in details)
            {
                if (detail.Contains("Arduino"))
                {
                    name = detail.Replace("Arduino Leonardo", "PPS Sensor");
                    name = name.Split(new string[] { " (" }, StringSplitOptions.None)[0];
                    names.Add(name);
                }
            }
            return names;
        }


        private string prettyToComPort(String pretty)
        {
            // get just comport to initialise devices
            return pretty.Split(new string[] { " -" }, StringSplitOptions.None)[0];
        }


        /// <summary>
        /// Fill appropriate Values into GUI Comboboxes
        /// </summary>
        private void PopulateGUIFields()
        {

            // Populate i2c addresses
            i2cAddressInputComboBox_.Items.Clear();

            //TODO appears to be race condition to populate combo box
            // This causes the invalid settings warning
            for (int i = reservedAddresses; i < 128; i++)
            {
                i2cAddressInputComboBox_.Items
                                        .Add("0x" + i.ToString("X2"));
            }

            //Populate active sensor combobox
            int j = 0;
            foreach (string port in prettyPorts)
            {
                string[] portSplit = port.Split('-');
                // replace COM port with index
                String name = portSplit[1] + " " + (prettyPorts.IndexOf(port) + 1).ToString();

                if (USBdevices[j].singleTact.firmwareVersion > 0)
                {
                    if (!USBdevices[j].isCalibrated)
                    {
                        name = name + "(calibrated)";
                    }
                    else
                    {
                        name = name + "(uncalibrated)";
                    }
                }

                ActiveSensor.Items.Add(name);
                j++;
            }
            ActiveSensor.SelectedIndex = 0;
            activeSingleTact = USBdevices[0].singleTact;

            if(activeSingleTact.Settings.Scaling < 100)
            {
                activeSingleTact.Settings.Scaling = 100;
            }

            int maxWidth = 0;
            System.Windows.Forms.Label dummy = new System.Windows.Forms.Label();
            // find widest label to resize dropdown dynamically
            foreach (var obj in ActiveSensor.Items)
            {
                dummy.Text = obj.ToString();
                int temp = dummy.PreferredWidth;
                if (temp > maxWidth)
                {
                    maxWidth = temp;
                }
            }
            ActiveSensor.DropDownWidth = maxWidth;

            // Get firmware version
            firmwareLabel.Text = activeSingleTact.firmwareVersion.ToString();

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


            // Set font sizes
            myPane.XAxis.Title.FontSpec.Size = 10;
            myPane.YAxis.Title.FontSpec.Size = 10;
            myPane.XAxis.Scale.FontSpec.Size = 7;
            myPane.YAxis.Scale.FontSpec.Size = 7;

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
            USB.dataBuffer.AddData(measurements, time);  // update 
        }

        private void updateGraph(USBdevice USB)
        {
            int index = USBdevices.IndexOf(USB); // get current sensor number
            SingleTactData data_pt = USB.dataBuffer;
            Color[] colours = { Color.Blue, Color.Orange, Color.DarkViolet, Color.Red, Color.DeepPink, Color.DarkSlateGray  };

            if (data_pt.data.Count > 0 && index < colours.Length)
            {
                // start graphing
                GraphPane graphPane = graph_.GraphPane;

                if (graphPane.CurveList.Count <= index)  // initialise curves
                {
                    string name = prettyPorts[index].ToString().Split('-')[1] + " " + (index+1).ToString();
                    if (USBdevices[index].singleTact.firmwareVersion > 0)
                    {
                        if (USBdevices[index].isCalibrated)
                        {
                            name = name + "(calibrated)";
                        }else
                        {
                            name = name + "(uncalibrated)";
                        }
                    }                   
                    LineItem myCurve = new LineItem(
                        name,
                        data_pt.data[0],
                        colours[index],
                        SymbolType.None,
                        3.0f);
                    graphPane.CurveList.Add(myCurve);
                }
                else
                {
                    // update curve data with new readings
                    graphPane.CurveList[index].Points = data_pt.data[0];
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
                    //Update green valid region box
                    BoxObj b = (BoxObj)graphPane.GraphObjList[0];
                    b.Location.X = graphPane.XAxis.Scale.Max - graphXRange_;
                    b.Location.Y = Math.Min(graphPane.YAxis.Scale.Max, 512);
                    b.Location.Height = Math.Min(graphPane.YAxis.Scale.Max - graphPane.YAxis.Scale.Min, 512);
                    graph_.AxisChange();

                }
                graph_.Refresh();
            }
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
            Invoke((Action)(() => { 
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
                    foreach(string portName in prettyPorts)
                    {
                        int index = prettyPorts.IndexOf(portName);
                        string name = portName.ToString().Split('-')[1] + " " + (index + 1).ToString();
                        if (USBdevices[index].singleTact.firmwareVersion > 0)
                        {
                            if (USBdevices[index].isCalibrated)
                            {
                                name = name + "(calibrated)";
                            }
                            else
                            {
                                name = name + "(uncalibrated)";
                            }
                        }
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
            }));

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
        /// Start the Acquisition Thread
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
                            USB.addFrame(newFrame);

                            // use first timestamp only to quantise readings and match csv output
                            AddData(USBdevices[0].lastTimeStamp, newFrame.SensorData, USB); //Add to stripchart
                    
                    }
                    else  // USB has been unplugged
                    {
                        new Thread(() =>
                        {
                            guiTimer_.Stop();
                            backgroundIsFinished_ = true;
                            var index = USBdevices.IndexOf(USB);
                            var comPort = serialPortNames[index];
                            var result = MessageBox.Show(
                                comPort.ToString() + " has been unplugged.\nWould you like to save your data before exiting?",
                                "Error!",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error);
                            if (result == DialogResult.Yes)
                            {
                                buttonSave_Click(this, null);
                            }
                            Application.Exit();
                        }).Start();
                        backgroundIsFinished_ = false;
                        StopAcquisitionThread();
                        break;
                    }

                    //Calculate rate
                    double delta = newFrame.TimeStamp - USB.lastTimeStamp; // calculate delta relative to previous sensor's last reading
                        if (delta != 0)
                            measuredFrequency_ = measuredFrequency_ * 0.95 + 0.05 * (1.0 / (delta));  //Averaging
                            //measuredFrequency_ = 1/delta;
                        USB.setTimestamp(newFrame.TimeStamp);
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

            foreach (USBdevice USB in USBdevices)
            {
                if(!backgroundIsFinished_)
                    updateGraph(USB);
            }                

            //Update update rate
            timerItr_++;
            if (0 == timerItr_ % 5)
                this.Text = "PPS SingleTact Demo - Version " + version + " [ " + measuredFrequency_.ToString("##0") + " Hz ]";
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
                    textTare.Text = activeSingleTact.Settings.Baselines.ElementAt(0).ToString("0000");
                    i2cAddressInputComboBox_.SelectedIndex = activeSingleTact.Settings.I2CAddress - reservedAddresses;

                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.ToString());
                    MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            

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
                activeSingleTact.Settings.I2CAddress = (byte)(i2cAddressInputComboBox_.SelectedIndex + reservedAddresses);
                activeSingleTact.Settings.Accumulator = 5;
                //singleTact_.Settings.Baselines =

            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            activeSingleTact.PushSettingsToHardware();

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
                activeSingleTact.Settings.Calibrated = (activeSingleTact.Settings.Calibrated == 0) ? (byte)1 : (byte)0;
            }
            catch (Exception)
            {
                MessageBox.Show("Invalid settings", "Error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            activeSingleTact.PushSettingsToHardware();
            RefreshFlashSettings_Click(this, null);
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
       

        private void LinkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            var text = linkLabel1.Text;
            if (linkLabel1.Text.Contains((char)0x25BC))
            {
                linkLabel1.Text = text.Replace((char)0x25BC, (char)0x25B2);
            }
            else
            {
                linkLabel1.Text = text.Replace((char)0x25B2, (char)0x25BC);
            }
            Settings.Visible = !Settings.Visible;
        }

        private void ActiveSensor_SelectedIndexChanged(object sender, EventArgs e)
        {
            activeSingleTact = USBdevices[ActiveSensor.SelectedIndex].singleTact;
            if (activeSingleTact.isUSB && activeSingleTact.isCalibrated)
            {
                 linkLabel1.Visible = true;
                if (Settings.TabPages.Count > 1)
                {
                    Settings.TabPages.RemoveAt(Settings.TabPages.Count - 1);
                }
            }
            else if (activeSingleTact.isUSB && !activeSingleTact.isCalibrated)
            {
                linkLabel1.Visible = true;
                if (Settings.TabPages.Count > 1)
                {
                    Settings.TabPages.RemoveAt(Settings.TabPages.Count - 1);
                }
            }
            else if (!activeSingleTact.isUSB && activeSingleTact.isCalibrated)
            {
                linkLabel1.Visible = true;
                SetSettingsButton.Enabled = true;
            }
            else
            {
                linkLabel1.Visible = true;
                SetSettingsButton.Enabled = true;
            }
            RefreshFlashSettings_Click(this, null); //Update display
        }

        private void GUI_Load(object sender, EventArgs e)
        {

        }
       
    
    }

}

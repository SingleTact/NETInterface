using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using System.Threading;
using SingleTactLibrary;

namespace SingleTact_Barebones
{
    public partial class Form1 : Form
    {
        private bool backgroundIsFinished_ = false;  // Flag to check background thread is finished
        private double measuredFrequency_ = 50;  // Sensor update rate
        private List<USBdevice> USBdevices = new List<USBdevice>();
        private List<string> comPortList = new List<string>();
        private SingleTact activeSingleTact;
        private delegate void CloseMainFormDelegate(); //Used to close the program if hardware is not connected
        public Form1()
        {
            string exceptionMessage = null;

            InitializeComponent();
            var finder = new ComPortFinder();
            // Get available serial ports.
            comPortList = finder.findSingleTact();
            if (comPortList.Count == 0)
            {
                MessageBox.Show(
                "Failed to start sensor: no serial ports found.\n\nPlease ensure Arduino drivers are installed.\nThis can be checked by looking if the Arduino is identified in Device Manager.\n\nPlease connect the device then restart this application.",
               "Hardware initialisation failed",
               MessageBoxButtons.OK,
               MessageBoxIcon.Exclamation);
                // There's no point showing the GUI.  Force the app to auto-close.
                Environment.Exit(-1);
            }
            else
            {
                for (int i = 0; i < 1; i++)
                {
                    USBdevice USB = new USBdevice();
                    USB.Initialise(finder.prettyToComPort(comPortList[i]));
                    USBdevices.Add(USB);
                    this.Text = comPortList[i];
                }
            }
            activeSingleTact = USBdevices[0].singleTact;
            try
            {
                //PopulateGUIFields();
                foreach (USBdevice USB in USBdevices)
                {
                    USB.singleTact.PushSettingsToHardware();
                    RefreshFlashSettings_Click(this, null); //Get the settings from flash
                }
                //CreateStripChart();
                AcquisitionWorker.RunWorkerAsync(); //Start the acquisition thread

                guiTimer_.Start();
            }
            catch
            {
                string summary = "Failed to start sensor";

                if (comPortList.Count == 0)
                    summary += ": no serial ports detected.";
                else
                    summary += " on " + comPortList[0] + ".";

                summary += "\n\n";

                if (exceptionMessage != null)
                    summary += exceptionMessage;
                else
                    summary += "Please connect the device then restart this application.";

                MessageBox.Show(
                    summary,
                    "Hardware initialisation failed",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Exclamation);

                // There's no point showing the GUI.  Force the app to auto-close.
                Environment.Exit(-1);
            }
        }
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
                    }
                    else  // USB has been unplugged
                    {
                        new Thread(() =>
                        {
                            guiTimer_.Stop();
                            backgroundIsFinished_ = true;
                            var index = USBdevices.IndexOf(USB);
                            var comPort = comPortList[index];
                            var result = MessageBox.Show(
                                comPort.ToString() + " has been unplugged.\nWould you like to save your data before exiting?",
                                "Error!",
                                MessageBoxButtons.YesNo,
                                MessageBoxIcon.Error);
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
        /// Done - we are closing down
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AcquisitionWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            backgroundIsFinished_ = true;
        }
        private void guiTimer__Tick(object sender, EventArgs e)
        {
            foreach (USBdevice USB in USBdevices)
            {
                if (!backgroundIsFinished_)
                    updateGraph(USB);
            }
        }

        private void updateGraph(USBdevice USB)
        {
            if (USB.frameList.Count > 0)
            {
                var point = USB.frameList.Last();
                ForceOuptut.Text = point.SensorData[0] + "";
            }
        }
    }
}

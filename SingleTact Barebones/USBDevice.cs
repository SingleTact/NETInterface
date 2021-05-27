using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SingleTactLibrary;


namespace SingleTact_Barebones
{
    /// <summary>
    /// Class to contain all aspects of a Singletact USB connection
    /// Including data, frames, and serial port
    /// </summary>
    class USBdevice
    {
        private ArduinoSingleTactDriver _arduino;
        private List<SingleTactFrame> _frameList;
        private SingleTact _singleTact;
        private double _lastTimestamp = 0.0;
        public bool isCalibrated = false;


        public bool Initialise(string portName)
        {
            try
            {
                _frameList = new List<SingleTactFrame>();
                _singleTact = new SingleTact();
                _arduino = new ArduinoSingleTactDriver();
                _arduino.Initialise(portName); //Start Arduino driver
                _singleTact.Initialise(_arduino);
                _singleTact.I2cAddressForCommunications = ((byte)(4));
                isCalibrated = _singleTact.isCalibrated;
                return true;
            }
            catch
            {
                return false;
            }
        }


        /// <summary>
        /// Add frame to frame list
        /// </summary>
        public void addFrame(SingleTactFrame frame)
        { 
            _frameList.Add(frame);
        }

        /// <summary>
        /// Remove all frame to frame list
        /// </summary>
        public void removeAllFrame()
        {
            _frameList.Clear();
        }


        /// <summary>
        /// Update last timestamp
        /// </summary>
        public void setTimestamp(double time)
        {
            _lastTimestamp = time;
        }

        /// <summary>
        /// Get copy of USB device's last data timestamp
        /// </summary>
        public double lastTimeStamp
        { get { return _lastTimestamp; } }


        /// <summary>
        /// Get copy of USB device's frame list
        /// </summary>
        public List<SingleTactFrame> frameList
        { get {
                return _frameList;
            }
        }


        /// <summary>
        /// Get USB device's Singletact object
        /// </summary>
        public SingleTact singleTact
        { get { return _singleTact; } }
    }
}

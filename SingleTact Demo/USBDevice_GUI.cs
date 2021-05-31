using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SingleTactLibrary;


namespace SingleTact_Demo
{
    /// <summary>
    /// Class to contain all aspects of a Singletact USB connection
    /// Including data, frames, and serial port
    /// </summary>
    public class USBdevice_GUI : USBdevice
    {
        private SingleTactData _dataBuffer;

        public new bool Initialise(string portName)
        {
            try
            {
                _dataBuffer = new SingleTactData();
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
        /// Get copy of USB device's data buffer
        /// </summary>
        public SingleTactData dataBuffer
        { get { return _dataBuffer; } }
    }
}

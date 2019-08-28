//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading;

namespace SingleTactLibrary
{
    public partial class SingleTact : Component
    {
        private ArduinoSingleTactDriver arduino_;
        private SingleTactFrame lastFrame_;
        private bool isFirst = true;
        private UInt32 startTime = 0;
        private byte i2cAddress_ = 0x04;
        private int itr_ = 0;
        public bool isConnected;
        public bool isCalibrated;

        public SingleTact()
        {
            InitializeComponent();
        }

        public SingleTact(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Start the SingleTact, using an Arduino Interface Object
        /// </summary>
        /// <param name="arduino">Arduino Interface</param>
        /// <returns>true if successful; false otherwise.</returns>
        public bool Initialise(ArduinoSingleTactDriver arduino)
        {
            arduino_ = arduino;

            if (!PullSettingsFromHardware())
                return false;

            isConnected  = false;
            isCalibrated = false;
            return true;
        }

        /// <summary>
        /// Write local copy of settings to sensor's flash
        /// </summary>
        public void PushSettingsToHardware()
        {
            for (int i = 0; i < 7; i++)  //We need to do this over 7 transfers
            {
                const int PacketSize = 16;

                byte[] toSend = new byte[PacketSize];

                for (int j = 0; j < PacketSize; j++)
                {
                    toSend[j] = Settings.SettingsRaw[i * PacketSize + j];
                }

                if (!arduino_.WriteToMainRegister(toSend, (byte)(i * PacketSize), i2cAddress_))
                {
                    MessageBox.Show("Failed to write settings", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }

                Thread.Sleep(50); //Give the sensor time to write to flash
            }
        }

        /// <summary>
        /// Write local copy of settings to sensor's flash
        /// </summary>
        public void PushCalibrationToHardware(int[] calibrationTable)
        {
            for (int i = 0; i < 32; i++)  //We need to do this over 32 transfers
            {
                const int PacketSize = 16;

                byte[] toSend = new byte[PacketSize];

                for (int j = 0; j < PacketSize / 2; j++)
                {
                    // Comment for now
                    toSend[j * 2] = (byte)(calibrationTable[i * 8 + j] >> 8);
                    toSend[j * 2 + 1] = (byte)(calibrationTable[i * 8 + j] & 0xFF);
                }

                if (!arduino_.WriteToCalibrationRegister(toSend, (byte)(i), i2cAddress_))
                {
                    MessageBox.Show("Failed to write calibration", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    isCalibrated = false;
                    System.Environment.Exit(0);
                }

                Thread.Sleep(100); //Give the sensor time to write to flash
            }

            isCalibrated = true;
        }

        /// <summary>
        /// Push Toggle GPIO CMD to Arduino
        /// </summary>
        //
        public void PushToggleToArduino(byte ToggleGPIO)
        {
            if (!arduino_.WriteToggleCommand(ToggleGPIO))
            {
                MessageBox.Show("Failed to Toggle the GPIO", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Thread.Sleep(50);
        }

        /// <summary>
        /// Pull settings from sensor flash (updating local copy)
        /// </summary>
        /// <returns>true if completely successful; otherwise false.</returns>
        public bool PullSettingsFromHardware()
        {
            byte[] settings = new byte[SingleTactParameters.ParamLocation];
            byte[] parameters = new byte[128 - SingleTactParameters.ParamLocation];

            for (int i = 0; i < 4; i++)  //We need to do this over 4 transfers
            {
                byte[] newByteData = arduino_.ReadFromMainRegister((byte)(i * 32), 32, i2cAddress_);  //Read 32 bytes from main register

                if (null == newByteData)
                    return false;

                for (int j = 0; j < 32; j++)
                {
                    if (i * 32 + j < SingleTactParameters.ParamLocation)
                        settings[i * 32 + j] = newByteData[j + ArduinoSingleTactDriver.TIMESTAMP_SIZE];
                    else
                        parameters[j - 16] = newByteData[j + ArduinoSingleTactDriver.TIMESTAMP_SIZE]; //Last half packet is parameters
                }
            }

            Settings.SettingsRaw = settings;
            Parameters.ParametersRaw = parameters;

            return true;
        }

        /// <summary>
        /// Read sensor for new pressure measurement
        /// </summary>
        /// <returns>New frame if one is available</returns>
        public SingleTactFrame ReadSensorData()
        {
            //Sample read code - reading sensor data
            byte[] newByteData = arduino_.ReadFromMainRegister(128, 6, i2cAddress_);  //Read 6 bytes of sensor from location 128 in main register
            if (null != newByteData)
            {
                UInt16 itr = (UInt16)((newByteData[0 + ArduinoSingleTactDriver.TIMESTAMP_SIZE] << 8) + newByteData[1 + ArduinoSingleTactDriver.TIMESTAMP_SIZE]);

                if (itr_ != itr)
                {
                    itr_ = itr;
                    
                    UInt32 timeStampRaw = (UInt32)((newByteData[0] << 24) + (newByteData[1] << 16) + (newByteData[2] << 8) + newByteData[3]);
                    if (isFirst == true)
                    {
                        isFirst = false;
                        startTime = timeStampRaw;
                    }
                    timeStampRaw -= startTime;
                    double timeStamp = (double)timeStampRaw / 10000.0; //10kHz clock

                    UInt16[] sensorData = new UInt16[(int)(newByteData.Length - ArduinoSingleTactDriver.TIMESTAMP_SIZE - 4) / 2];

                    //Do it manually to avoid confusion with Endianess - Windows is little, sensor is big.
                    //ToDo find a more elegent solution!
                    for (int i = 0; i < sensorData.Length; i++)
                    {
                        sensorData[i] = (UInt16)((newByteData[2 * i + 4 + ArduinoSingleTactDriver.TIMESTAMP_SIZE] << 8) + newByteData[2 * i + 5 + ArduinoSingleTactDriver.TIMESTAMP_SIZE]);
                    }

                    SingleTactFrame toReturn = new SingleTactFrame(sensorData, timeStamp);
                    lastFrame_ = toReturn.DeepClone(); // keep a local copy of the last frame, used in taring
                    return toReturn;
                }
                else
                {
                    return null;
                }
            }

            return null;
        }

        /// <summary>
        /// Reset baseline of all elements
        /// </summary>
        /// <returns>true if successful; false otherwise.</returns>
        public bool Tare()
        {
            ushort scaling = Settings.Scaling;

            if (lastFrame_ == null)
                return false;

            Settings.Baselines = new UInt16[lastFrame_.nSensors]; //Zeros
            Settings.Scaling = 100;

            PushSettingsToHardware();

            Thread.Sleep(10); // Give it time to capture a new frame

            SingleTactFrame newFrame = ReadSensorData();

            UInt16[] newBaselines = new UInt16[newFrame.SensorDataRaw.Length];
            for (int i = 0; i < newFrame.SensorDataRaw.Length; i++)
            {
                newBaselines[i] = (UInt16)(newFrame.SensorDataRaw[i] - 0xFF);
            }

            Settings.Baselines = newBaselines;
            Settings.Scaling = scaling;
            PushSettingsToHardware();

            return true;
        }

        /// <summary>
        /// I2C address used for communication - all sensors respond to 0x04 and their own specific address.
        /// </summary>
        public byte I2cAddressForCommunications
        {
            get
            {
                return i2cAddress_;
            }
            set
            {
                i2cAddress_ = value;
            }
        }


        /// <summary>
        /// Sensors settings
        /// </summary>
        public SingleTactSettings Settings = new SingleTactSettings();

        /// <summary>
        /// Sensor parameters
        /// </summary>
        public SingleTactParameters Parameters = new SingleTactParameters();


        /// <summary>
        /// Returns copy of SingleTact object
        /// </summary>
        public SingleTact Clone()
        {
            SingleTact clone = new SingleTact();
            clone.arduino_ = this.arduino_;
            clone.lastFrame_ = this.lastFrame_;
            clone.isFirst = this.isFirst;
            clone.startTime = this.startTime;
            clone.i2cAddress_ = this.i2cAddress_;
            clone.itr_ = this.itr_;
            clone.isConnected = this.isConnected;
            clone.isCalibrated = this.isCalibrated;
            return clone;
        }
    }
}

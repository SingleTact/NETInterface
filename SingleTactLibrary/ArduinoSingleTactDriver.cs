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
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace SingleTactLibrary
{
    public partial class ArduinoSingleTactDriver : Component
    {
        SerialPort serialPort_;
        List<byte> incommingSerialBuffer_ = new List<byte>();

        byte cmdItr_ = 0;
        private UInt16 lastItr_ = 0;

        public const int TIMESTAMP_SIZE = 4;
        const int I2C_ID_BYTE = 6;
        const int I2C_TIMESTAMP = 7;
        const int I2C_TOPC_NBYTES = 11;
        const int I2C_START_OF_DATA = 12;

        //Minimum packet length is 15 (header + info + footer)
        const int MINIMUM_FROMARDUINO_PACKET_LENGTH = 15;

        public ArduinoSingleTactDriver()
        {
            InitializeComponent();
        }

        public ArduinoSingleTactDriver(IContainer container)
        {
            container.Add(this);
            InitializeComponent();
        }

        /// <summary>
        /// Initialise connection
        /// </summary>
        /// <param name="serialPort">Serial port name (i.e. COM1)</param>
        public void Initialise(string serialPort)
        {
            serialPort_ = new SerialPort(serialPort);
            //serialPort_.BaudRate = 115200*4;
            serialPort_.BaudRate = 115200;
            serialPort_.ReadBufferSize = 48;
            serialPort_.WriteBufferSize = 16;
            serialPort_.ErrorReceived += new System.IO.Ports.SerialErrorReceivedEventHandler(this.SerialErrorReceived);
            serialPort_.Open();

            //Reset the Arduino
            serialPort_.DtrEnable = true;
            Thread.Sleep(10);
            serialPort_.DtrEnable = false;
            Thread.Sleep(2000); //Give Arduino time to boot after reset
<<<<<<< HEAD

            serialPort_.RtsEnable = true;
=======
>>>>>>> refs/remotes/origin/master
        }

        public void ResetArduino()
        {
            if (serialPort_.IsOpen)
            serialPort_.Close();

            serialPort_.Open();
            //Reset the Arduino
            serialPort_.DtrEnable = true;
            Thread.Sleep(10);
            serialPort_.DtrEnable = false;
            Thread.Sleep(2000); //Give Arduino time to boot after reset
        }

        private void SerialErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Serial General Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        private void ReadSerialBuffer()
        {
            while (serialPort_.BytesToRead > 0)
            {
                incommingSerialBuffer_.Add((byte)serialPort_.ReadByte());
            }
        }

        /// <summary>
        /// Write to sensor's Main Register
        /// </summary>
        /// <param name="toSend">Data to write (max 28 bytes)</param>
        /// <param name="location">Main register location (can write upto byte 128)</param>
        /// <param name="i2CAddress">I2C Address</param>
        /// <returns>Was successful?</returns>
        public bool WriteToMainRegister(byte[] toSend, byte location, byte i2CAddress)
        {
            if (toSend.Length > 28) //Max write length (limited by 32 byte i2c transfer length = 28 bytes of data + header info)
            {
                MessageBox.Show("Trying to write a packet that is too large", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (toSend.Length + location > 128)
            {
                MessageBox.Show("Trying to write into read only region", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            byte[] cmdToArduino = SerialCommand.GenerateWriteCommand(i2CAddress, cmdItr_++, location, toSend);

            serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);

            bool acknowledged = false;
            int attempts = 20;
            //Typically takes 4 - 6 attempts as the chip needs to
            //write the settings to flash which takes about 50ms.

            Thread.Sleep(10); //Give comms time to happen

            while (false == acknowledged && attempts > 0)
            {
                byte[] cmdFromArduino = ProcessSerialBuffer();

                if (null != cmdFromArduino)
                {
                    if (cmdToArduino[I2C_ID_BYTE] == cmdFromArduino[I2C_ID_BYTE])
                    {
                        acknowledged = true;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Comms Error - Failed Acknoledge Write Command", "Communications Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        acknowledged = true; //Move on anyway
                        return false;
                    }
                }
                else
                {
                    Thread.Sleep(10); //Keep waiting
                    attempts--;
                }
            }

            return false; // no ack, give up
        }

        /// <summary>
        /// Write to sensor's Calibration Register
        /// </summary>
        /// <param name="toSend">Data to write (max 28 bytes)</param>
        /// <param name="location">Main register location (can write upto byte 128)</param>
        /// <param name="i2CAddress">I2C Address</param>
        /// <returns>Was successful?</returns>
        public bool WriteToCalibrationRegister(byte[] toSend, byte location, byte i2CAddress)
        {
            if (toSend.Length > 28) //Max write length (limited by 32 byte i2c transfer length = 28 bytes of data + header info)
            {
                MessageBox.Show("Trying to write a packet that is too large", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (toSend.Length + location > 1024)
            {
                MessageBox.Show("Trying to write into read only region", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            byte[] cmdToArduino = SerialCommand.GenerateWriteCalCommand(i2CAddress, cmdItr_++, location, toSend);

            serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);

            bool acknowledged = false;
            int attempts = 20;
            //Typically takes 4 - 6 attempts as the chip needs to
            //write the settings to flash which takes about 50ms.

            Thread.Sleep(10); //Give comms time to happen

            while (false == acknowledged && attempts > 0)
            {
                byte[] cmdFromArduino = ProcessSerialBuffer();

                if (null != cmdFromArduino)
                {
                    if (cmdToArduino[I2C_ID_BYTE] == cmdFromArduino[I2C_ID_BYTE])
                    {
                        acknowledged = true;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Comms Error - Failed Acknoledge Write Command", "Communications Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        acknowledged = true; //Move on anyway
                        return false;
                    }
                }
                else
                {
                    Thread.Sleep(10); //Keep waiting
                    attempts--;
                }
            }

            return false; // no ack, give up
        }

        /// <summary>
        /// Read from sensor's Main Register
        /// </summary>
        /// <param name="location">Location to read (sensor data starts at 128)</param>
        /// <param name="nBytes">Number of bytes to read (max = 32)</param>
        /// <param name="i2CAddress">I2C Address</param>
        /// <returns>4 byte timestamp followed by nBytes of data</returns>
        public byte[] ReadFromMainRegister(byte location, byte nBytes, byte i2CAddress)
        {
            if (nBytes > 32)
            {
                MessageBox.Show("Error - Trying to read too much");
                return null;
            }

            if (location + nBytes > 191) //0 - 191 are valid memory locations
            {
                MessageBox.Show("Error - Trying to read off the end of the main register");
                return null;
            }

            byte[] cmdToArduino = SerialCommand.GenerateReadCommand(i2CAddress, cmdItr_++, location, nBytes);

            serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);

            bool acknowledged = false;
            long attempts = 50;

            Thread.Sleep(10); //Give comms time to happen

            while (false == acknowledged && attempts > 0)
            {
                byte[] cmdFromArduino = ProcessSerialBuffer();

                if (null != cmdFromArduino)
                {
                    if (cmdToArduino[I2C_ID_BYTE] == cmdFromArduino[I2C_ID_BYTE])
                    {
                        acknowledged = true;
                        byte[] toReturn = new byte[nBytes + TIMESTAMP_SIZE];
                        Array.Copy(cmdFromArduino, I2C_TIMESTAMP, toReturn, 0, TIMESTAMP_SIZE);
                        Array.Copy(cmdFromArduino, I2C_START_OF_DATA, toReturn, TIMESTAMP_SIZE, nBytes);
                        return toReturn;
                    }
                    else
                    {
                        MessageBox.Show("Comms error - failed ack");
                        acknowledged = true; //Move on anyway
                        return null;
                    }
                }
                else
                {
                    Thread.Sleep(10);
                    attempts--;
                }
            }

            return null; // no ack
        }

        /// <summary>
        /// Write to Toggle the PIN
        /// </summary>
        /// <param name="toSend">Pins to Toggle</param>
        /// <returns>Was successful?</returns>
        public bool WriteToggleCommand(byte toSend)
        {
            byte[] cmdToArduino = SerialCommand.GenerateToggleCommand(4, cmdItr_++, 0, toSend);

            serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);

            bool acknowledged = false;
            long attempts = 50;

            Thread.Sleep(10); //Give comms time to happen

            while (false == acknowledged && attempts > 0)
            {
                byte[] cmdFromArduino = ProcessSerialBuffer();

                if (null != cmdFromArduino)
                {
                    if (cmdToArduino[I2C_ID_BYTE] == cmdFromArduino[I2C_ID_BYTE])
                    {
                        acknowledged = true;
                        return true;
                    }
                    else
                    {
                        MessageBox.Show("Comms Error - Failed Acknoledge Write Command", "Communications Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        acknowledged = true; //Move on anyway
                        return false;
                    }
                }
                else
                {
                    Thread.Sleep(10); //Keep waiting
                    attempts--;
                }
            }

            return false; // no ack, give up
        }

        /// <summary>
        /// Check the full footer
        /// </summary>
        /// <param name="endOfPacket"></param>
        /// <returns></returns>
        private bool CheckUartFooter(int endOfPacket)
        {
            for (int i = 0; i < 4; i++)
            {
                if (incommingSerialBuffer_[endOfPacket - i] != 0xFE)
                return false;  //Footer corrupt
            }

            return true; //Footer all good
        }

        /// <summary>
        /// Check available header bytes
        /// </summary>
        /// <returns></returns>
        private bool CheckUartHeader()
        {
            for (int i = 0; i < 4; i++)
            {
                if (incommingSerialBuffer_[i] != 0xFF)
                return false; //Header corrupt
            }
            return true; //Header all good
        }

        /// <summary>
        /// Process incomming serial data
        /// </summary>
        /// <returns></returns>
        private byte[] ProcessSerialBuffer()
        {
            ReadSerialBuffer();

            if (incommingSerialBuffer_.Count > MINIMUM_FROMARDUINO_PACKET_LENGTH)
            {
                if (false == CheckUartHeader())
                {
                    incommingSerialBuffer_.RemoveAt(0);
                    incommingSerialBuffer_.TrimExcess();
                }

                int i2cPacketLength = incommingSerialBuffer_[I2C_TOPC_NBYTES];

                if (incommingSerialBuffer_.Count > (i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH))
                {
                    if (CheckUartFooter(i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH))
                    {
                        byte[] toReturn = new byte[i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH + 1];
                        incommingSerialBuffer_.GetRange(0, i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH + 1).CopyTo(toReturn);
                        incommingSerialBuffer_.RemoveRange(0, i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH + 1);
                        return toReturn; //We have a good packet
                    }
                    else
                    {
                        incommingSerialBuffer_.RemoveRange(0, i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH + 1); //Bad data
                        return null;
                    }
                }
            }

            return null;
        }
    }
}

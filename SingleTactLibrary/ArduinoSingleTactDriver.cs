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
using System.IO.Ports;
using System.Windows.Forms;
using System.Threading;

namespace SingleTactLibrary
{
    public partial class ArduinoSingleTactDriver : Component
    {
        SerialPort serialPort_;

        // Raw UART bytes are stored here first. The serial receive event only adds
        // bytes to this buffer, then the parser moves complete packets out of it.
        private readonly List<byte> rawSerialBuffer_ = new List<byte>(8192);

        // Complete, validated Arduino packets are stored here. Read/write commands
        // search this buffer for the matching command ID instead of reading UART.
        private readonly List<byte[]> packetBuffer_ = new List<byte[]>();

        private readonly object rawLock_ = new object();
        private readonly object packetLock_ = new object();
        private readonly object commandLock_ = new object();

        private const int MAX_RAW_BUFFER_BYTES = 8192;
        private const int MAX_PACKET_BUFFER_COUNT = 200;
        private const int WRITE_ACK_TIMEOUT_MS = 500;
        private const int READ_ACK_TIMEOUT_MS = 500;

        byte cmdItr_ = 0;
        public bool isUSB = false;

        public const int TIMESTAMP_SIZE = 4;
        const int I2C_ID_BYTE = 6;
        const int I2C_TIMESTAMP = 7;
        const int I2C_TOPC_NBYTES = 11;
        const int I2C_START_OF_DATA = 12;

        // Minimum packet length is 15 (header + info + footer)
        const int MINIMUM_FROMARDUINO_PACKET_LENGTH = 15;

        public ArduinoSingleTactDriver()
        {
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

            // Make the .NET receive buffer bigger than before. The old value of 48
            // bytes can overflow easily if the application thread is busy.
            serialPort_.ReadBufferSize = 8192;
            serialPort_.WriteBufferSize = 256;

            serialPort_.ErrorReceived += new SerialErrorReceivedEventHandler(this.SerialErrorReceived);
            serialPort_.DataReceived += new SerialDataReceivedEventHandler(this.SerialDataReceived);

            serialPort_.Open();

            ClearReceiveBuffers();

            // Reset the Arduino
            serialPort_.DtrEnable = true;
            Thread.Sleep(10);
            serialPort_.DtrEnable = false;
            Thread.Sleep(2000); // Give Arduino time to boot after reset
            serialPort_.RtsEnable = true;

            ClearReceiveBuffers();
        }

        private void SerialErrorReceived(object sender, SerialErrorReceivedEventArgs e)
        {
            MessageBox.Show("Serial General Error", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }

        /// <summary>
        /// UART receive side. This is the only place that reads from the SerialPort.
        /// It saves bytes to a large raw buffer, then moves complete packets to packetBuffer_.
        /// </summary>
        private void SerialDataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                ReadAvailableSerialBytes();
            }
            catch
            {
                // USB may have been unplugged or the port may have been closed.
                // Command methods will return false/null on timeout or write failure.
            }
        }

        /// <summary>
        /// Reads all currently available UART bytes into rawSerialBuffer_, then parses
        /// any complete packets into packetBuffer_.
        /// </summary>
        private void ReadAvailableSerialBytes()
        {
            if (serialPort_ == null || !serialPort_.IsOpen)
                return;

            int bytesToRead = serialPort_.BytesToRead;
            if (bytesToRead <= 0)
                return;

            byte[] temp = new byte[bytesToRead];
            int bytesRead = serialPort_.Read(temp, 0, bytesToRead);

            if (bytesRead <= 0)
                return;

            lock (rawLock_)
            {
                for (int i = 0; i < bytesRead; i++)
                    rawSerialBuffer_.Add(temp[i]);

                TrimRawBufferIfNeeded();
                MoveRawBytesToPacketsLocked();
            }
        }

        private void ClearReceiveBuffers()
        {
            lock (rawLock_)
            {
                rawSerialBuffer_.Clear();
            }

            lock (packetLock_)
            {
                packetBuffer_.Clear();
            }

            try
            {
                if (serialPort_ != null && serialPort_.IsOpen)
                    serialPort_.DiscardInBuffer();
            }
            catch
            {
                // Ignore discard errors during reset/disconnect.
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
            if (toSend.Length > 28) // Max write length (limited by 32 byte i2c transfer length = 28 bytes of data + header info)
            {
                MessageBox.Show("Trying to write a packet that is too large", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            if (toSend.Length + location > 128)
            {
                MessageBox.Show("Trying to write into read only region", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            lock (commandLock_)
            {
                byte[] cmdToArduino = SerialCommand.GenerateWriteCommand(i2CAddress, cmdItr_++, location, toSend);
                byte expectedId = cmdToArduino[I2C_ID_BYTE];

                RemovePacketsWithId(expectedId);

                try
                {
                    serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);
                }
                catch
                {
                    return false;
                }

                byte[] cmdFromArduino = WaitForPacket(expectedId, WRITE_ACK_TIMEOUT_MS);
                return cmdFromArduino != null;
            }
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

            if (location + nBytes > 191) // 0 - 191 are valid memory locations
            {
                MessageBox.Show("Error - Trying to read off the end of the main register");
                return null;
            }

            lock (commandLock_)
            {
                byte[] cmdToArduino = SerialCommand.GenerateReadCommand(i2CAddress, cmdItr_++, location, nBytes);
                byte expectedId = cmdToArduino[I2C_ID_BYTE];

                RemovePacketsWithId(expectedId);

                try
                {
                    serialPort_.Write(cmdToArduino, 0, cmdToArduino.Length);
                }
                catch
                {
                    return null;
                }

                byte[] cmdFromArduino = WaitForPacket(expectedId, READ_ACK_TIMEOUT_MS);

                if (cmdFromArduino == null)
                    return null;

                if (cmdFromArduino.Length < I2C_START_OF_DATA + nBytes)
                    return null;

                byte[] toReturn = new byte[nBytes + TIMESTAMP_SIZE];
                Array.Copy(cmdFromArduino, I2C_TIMESTAMP, toReturn, 0, TIMESTAMP_SIZE);
                Array.Copy(cmdFromArduino, I2C_START_OF_DATA, toReturn, TIMESTAMP_SIZE, nBytes);
                return toReturn;
            }
        }

        /// <summary>
        /// Moves complete packets from rawSerialBuffer_ to packetBuffer_.
        /// rawLock_ must already be held when this method is called.
        /// </summary>
        private void MoveRawBytesToPacketsLocked()
        {
            while (rawSerialBuffer_.Count > MINIMUM_FROMARDUINO_PACKET_LENGTH)
            {
                if (false == CheckUartHeader(rawSerialBuffer_))
                {
                    rawSerialBuffer_.RemoveAt(0);
                    continue;
                }

                if (rawSerialBuffer_.Count <= I2C_TOPC_NBYTES)
                    return;

                int i2cPacketLength = rawSerialBuffer_[I2C_TOPC_NBYTES];
                int fullPacketLength = i2cPacketLength + MINIMUM_FROMARDUINO_PACKET_LENGTH + 1;

                if (fullPacketLength <= 0)
                {
                    rawSerialBuffer_.RemoveAt(0);
                    continue;
                }

                if (rawSerialBuffer_.Count < fullPacketLength)
                    return; // Not enough bytes yet. Wait for the next UART receive event.

                if (CheckUartFooter(rawSerialBuffer_, fullPacketLength - 1))
                {
                    byte[] packet = rawSerialBuffer_.GetRange(0, fullPacketLength).ToArray();
                    rawSerialBuffer_.RemoveRange(0, fullPacketLength);
                    AddPacket(packet);
                }
                else
                {
                    // Footer is corrupt. Drop only one byte so we can resync to the next header.
                    rawSerialBuffer_.RemoveAt(0);
                }
            }
        }

        /// <summary>
        /// Check the full footer in the supplied buffer.
        /// </summary>
        private bool CheckUartFooter(List<byte> buffer, int endOfPacket)
        {
            if (endOfPacket < 3 || endOfPacket >= buffer.Count)
                return false;

            for (int i = 0; i < 4; i++)
            {
                if (buffer[endOfPacket - i] != 0xFE)
                    return false; // Footer corrupt
            }

            return true; // Footer all good
        }

        /// <summary>
        /// Check available header bytes in the supplied buffer.
        /// </summary>
        private bool CheckUartHeader(List<byte> buffer)
        {
            if (buffer.Count < 4)
                return false;

            for (int i = 0; i < 4; i++)
            {
                if (buffer[i] != 0xFF && buffer[i] != 0xAA)
                    return false; // Header corrupt
            }

            if (buffer[0] == 0xAA)
                isUSB = true;

            return true; // Header all good
        }

        private void AddPacket(byte[] packet)
        {
            lock (packetLock_)
            {
                packetBuffer_.Add(packet);

                while (packetBuffer_.Count > MAX_PACKET_BUFFER_COUNT)
                    packetBuffer_.RemoveAt(0);

                Monitor.PulseAll(packetLock_);
            }
        }

        /// <summary>
        /// Search the parsed packet buffer for a packet with the requested command ID.
        /// This does not read the SerialPort directly; UART receive is handled separately.
        /// </summary>
        private byte[] WaitForPacket(byte expectedCommandId, int timeoutMs)
        {
            Stopwatch sw = Stopwatch.StartNew();

            lock (packetLock_)
            {
                while (sw.ElapsedMilliseconds < timeoutMs)
                {
                    for (int i = 0; i < packetBuffer_.Count; i++)
                    {
                        byte[] packet = packetBuffer_[i];

                        if (packet.Length > I2C_ID_BYTE && packet[I2C_ID_BYTE] == expectedCommandId)
                        {
                            packetBuffer_.RemoveAt(i);
                            return packet;
                        }
                    }

                    int remainingMs = timeoutMs - (int)sw.ElapsedMilliseconds;
                    if (remainingMs <= 0)
                        break;

                    Monitor.Wait(packetLock_, Math.Min(remainingMs, 20));
                }
            }

            return null;
        }

        private void RemovePacketsWithId(byte commandId)
        {
            lock (packetLock_)
            {
                for (int i = packetBuffer_.Count - 1; i >= 0; i--)
                {
                    byte[] packet = packetBuffer_[i];
                    if (packet.Length > I2C_ID_BYTE && packet[I2C_ID_BYTE] == commandId)
                        packetBuffer_.RemoveAt(i);
                }
            }
        }

        private void TrimRawBufferIfNeeded()
        {
            if (rawSerialBuffer_.Count <= MAX_RAW_BUFFER_BYTES)
                return;

            int bytesToRemove = rawSerialBuffer_.Count - MAX_RAW_BUFFER_BYTES;
            rawSerialBuffer_.RemoveRange(0, bytesToRemove);
        }
    }
}

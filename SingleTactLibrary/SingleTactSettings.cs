//-----------------------------------------------------------------------------
//                                                                            
//  Copyright (c) 2015 All Right Reserved                                      
//  Pressure Profile Systems                                                   
//  www.pressureprofile.com                                                    
//  V1.0                                                         
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleTactLibrary
{
   public class SingleTactSettings
   {
         //Main register index
         const int INDEX_SMBUSADDRESS = 0;
         const int INDEX_SERIAL_NUMBER_MSB = 1;
         const int INDEX_SERIAL_NUMBER_LSB = 2;
         const int INDEX_PWM_PIN_MODE = 3;
         const int INDEX_SENSOR_MAPPING_PWM_ANALOG = 4;
         const int INDEX_Accumulator = 5;
         const int INDEX_REFERENCE_GAIN = 6;
         const int INDEX_CONVERSION_SIZE = 7;
         const int INDEX_DISCHARGE_TIMER = 8;
         const int INDEX_OUTPUT_CURRENT = 9;
         const int INDEX_SCALING_MSB = 10;
         const int INDEX_SCALING_LSB = 11;
         const int INDEX_NUMBER_ELEMENTS = 12;
         const int INDEX_RESERVED = 13;
         const int INDEX_INDEX_OF_SCANLIST = 15;
         const int INDEX_OF_BASELINES = 41;

         public enum PWMPinModes { PWM, SYNC};


         public byte NumberElements
         {
            get { return settingsRaw_[INDEX_NUMBER_ELEMENTS]; }
            set { settingsRaw_[INDEX_NUMBER_ELEMENTS] = value; }
         }

      /// <summary>
      /// The raw settings array as found on sensor memory
      /// </summary>
      public byte[] SettingsRaw
      {
         get { return settingsRaw_; }
         set { settingsRaw_ = value; }
      }
      private byte[] settingsRaw_;

      //Sensors I2C address
      public byte I2CAddress
      {
         get { return settingsRaw_[INDEX_SMBUSADDRESS]; }
         set { settingsRaw_[INDEX_SMBUSADDRESS] = value; }
      }

      /// <summary>
      /// Sensors serial number
      /// </summary>
      public UInt16 SerialNumberMsb
      {
         get { return (UInt16)(settingsRaw_[INDEX_SERIAL_NUMBER_MSB] * 256 + settingsRaw_[INDEX_SERIAL_NUMBER_LSB]); }
         set { settingsRaw_[INDEX_SERIAL_NUMBER_MSB] = (byte)(value >> 8); settingsRaw_[INDEX_SERIAL_NUMBER_LSB] = (byte)(value & 0xFF); }
      }

      /// <summary>
      /// PWM Pin mode
      /// </summary>
      public PWMPinModes PWMPinMode
      {
         get { return (PWMPinModes)settingsRaw_[INDEX_PWM_PIN_MODE]; }
         set { settingsRaw_[INDEX_PWM_PIN_MODE] = (byte)(value); }
      }

      /// <summary>
      /// Which sensor element is used for PWM output and analogue output
      /// </summary>
      public byte SensorMappingPWM_Analogue
      {
         get { return settingsRaw_[INDEX_SENSOR_MAPPING_PWM_ANALOG]; }
         set { settingsRaw_[INDEX_SENSOR_MAPPING_PWM_ANALOG] = value; }
      }

      /// <summary>
      /// Capacitive sensing accumulator - See CY8051F70x datasheet, chapter 15
      /// </summary>
      public byte Accumulator
      {
         get { return settingsRaw_[INDEX_Accumulator]; }
         set { settingsRaw_[INDEX_Accumulator] = value; }
      }

      /// <summary>
      /// Capacitive sensing reference gain  - See CY8051F70x datasheet, chapter 15
      /// </summary>
      public byte ReferenceGain
      {
         get { return settingsRaw_[INDEX_REFERENCE_GAIN]; }
         set { settingsRaw_[INDEX_REFERENCE_GAIN] = value; }
      }

      /// <summary>
      /// See CY8051F70x datasheet, chapter 15
      /// </summary>
      public byte ConversionSize  
      {
         get { return settingsRaw_[INDEX_CONVERSION_SIZE]; }
         set { settingsRaw_[INDEX_CONVERSION_SIZE] = value; }
      }

      /// <summary>
      /// See CY8051F70x datasheet, chapter 15
      /// </summary>
      public byte DischargeTimer
      {
         get { return settingsRaw_[INDEX_DISCHARGE_TIMER]; }
         set { settingsRaw_[INDEX_DISCHARGE_TIMER] = value; }
      }

      /// <summary>
      /// Scaling factor to go from raw 16 bit to 10 bit output
      /// </summary>
      public UInt16 Scaling
      {
         get { return (UInt16)(settingsRaw_[INDEX_SCALING_MSB] * 256 + settingsRaw_[INDEX_SCALING_LSB]); }
         set { settingsRaw_[INDEX_SCALING_MSB] = (byte)(value >> 8); settingsRaw_[INDEX_SCALING_LSB] = (byte)(value & 0xFF); }
      }

      public byte OutputCurrent
      {
         get { return settingsRaw_[INDEX_OUTPUT_CURRENT]; }
         set { settingsRaw_[INDEX_OUTPUT_CURRENT] = value; }
      }

      //Reserved Register, use the first bit to determine Lock/Unlock
      public byte Reserved
      {
         get { return settingsRaw_[INDEX_RESERVED]; }
         set { settingsRaw_[INDEX_RESERVED] = value; }
      }

      /// <summary>
      /// Element scan list
      /// </summary>
      public byte[] ScanList
      {
         get
         {
            byte[] toReturn = new byte[settingsRaw_[INDEX_NUMBER_ELEMENTS]];
            for (int i = 0; i < toReturn.Length; i++)
               toReturn[i] = settingsRaw_[INDEX_INDEX_OF_SCANLIST + i];
            return toReturn;
         }
         set
         {
            if(value.Length > 25)
            {
               MessageBox.Show("Error: Too many elements", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
               return;
            }

            settingsRaw_[INDEX_NUMBER_ELEMENTS] = (byte)(value.Length);

            for (int i = 0; i < value.Length; i++)
               settingsRaw_[INDEX_INDEX_OF_SCANLIST + i] = value[i];

            for (int i = value.Length; i < 25; i++)
               settingsRaw_[INDEX_INDEX_OF_SCANLIST + i] = 0;

         }
      }

      /// <summary>
      /// Element scan list
      /// </summary>
      public UInt16[] Baselines
      {
         get
         {
            UInt16[] toReturn = new UInt16[settingsRaw_[INDEX_NUMBER_ELEMENTS]];
            for (int i = 0; i < toReturn.Length; i++)
            {
               toReturn[i] = (UInt16)((UInt16)(settingsRaw_[INDEX_OF_BASELINES + 2 * i] << 8) + (UInt16)(settingsRaw_[INDEX_OF_BASELINES + 2 * i + 1]));
            }
            return toReturn;
         }
         set
         {
            if (value.Length != NumberElements)
            {
               MessageBox.Show("Error: Does not match number of elements", "Error", MessageBoxButtons.OK, MessageBoxIcon.Asterisk);
               return;
            }

            for (int i = 0; i < value.Length*2; i+=2)
            {
               settingsRaw_[INDEX_OF_BASELINES + i] = (byte)(value[i] >> 8);
               settingsRaw_[INDEX_OF_BASELINES + i + 1] = (byte)(value[i]);
               
            }
             
            for (int i = value.Length * 2; i < 50; i++)
               settingsRaw_[INDEX_OF_BASELINES + i] = 0;
         }
      }


   }
}

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

namespace SingleTactLibrary
{
   public class SingleTactFrame 
   {
      /// <summary>
      /// Create a new frame
      /// </summary>
      /// <param name="nSensors">N Sensors</param>
      /// <param name="sensorData">Raw values from UART</param>
      /// <param name="timeStamp">The timestamp</param>
      public SingleTactFrame(UInt16[] sensorData, double timeStamp)
      {
         nSensors_ = sensorData.Length;

         sensorDataRaw_ = sensorData;
         sensorData_ = new double[sensorData.Length];
         for (int i = 0; i < sensorData.Length; i++)
            sensorData_[i] = sensorData[i] - 0xFF; //Subtract offset
         timeStamp_ = timeStamp;
      }

      /// <summary>
      /// Default constructor - do not use
      /// </summary>
      public SingleTactFrame() { }

      public SingleTactFrame DeepClone()
      {
         SingleTactFrame toReturn = new SingleTactFrame();
         toReturn.nSensors_ = this.nSensors;
         toReturn.timeStamp_ = this.timeStamp_;
         toReturn.sensorData_ = new double[nSensors_];

         for (int i = 0; i < sensorData_.Length; i++)
            toReturn.sensorData_[i] = sensorData_[i];

         return toReturn;
      }

      /// <summary>
      /// N Sensors in Frame
      /// </summary>
      public int nSensors
      {
         get { return nSensors_; }
      }
      private int nSensors_ = 0;

      /// <summary>
      /// Sensor Data
      /// </summary>
      public double[] SensorData
      {
         get { return sensorData_; }
      }
      private double[] sensorData_;

      /// <summary>
      /// Sensor Raw
      /// </summary>
      public UInt16[] SensorDataRaw
      {
         get { return sensorDataRaw_; }
      }
      private UInt16[] sensorDataRaw_;

      /// <summary>
      /// Timestamp in seconds
      /// </summary>
      public double TimeStamp
      {
         get { return timeStamp_; }
      }
      private double timeStamp_;


   }
}

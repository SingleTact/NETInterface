//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using ZedGraph;

namespace SingleTact_Demo
{
    /// <summary>
    /// Class to buffer SingleTact Data
    /// </summary>
    public class SingleTactData
    {
        /// <summary>
        /// Most recent time
        /// </summary>
        public double MostRecentTime
        { get { return mostRecentTime_; } }
        private double mostRecentTime_ = 0;

        const int MAX_NUMBER_MESUREMENTS = 40 * 1000;// ~1000s of data

        /// <summary>
        /// Stripchart Data
        /// </summary>
        public List<RollingPointPairList> data = new List<RollingPointPairList>();

        /// <summary>
        /// Add data to container
        /// </summary>
        /// <param name="measurements">Measurements</param>
        /// <param name="time">Time in ms</param>
        public int AddData(double[] measurements, double time)
        {
            //Resize data store to fit number of measurements
            while (measurements.Length > data.Count)
            {
                data.Add(new RollingPointPairList(MAX_NUMBER_MESUREMENTS));
                data.TrimExcess();
            }

            //Add the data to the buffer
            for (int i = 0; i < measurements.Length; i++)
            {
                data[i].Add(time, measurements[i]);  //Convert to seconds
            }

            mostRecentTime_ = time;
            return (data[0].Count * 100 / MAX_NUMBER_MESUREMENTS);
        }

        public SingleTactData Clone()
        {
            SingleTactData clone = new SingleTactData();
            clone.mostRecentTime_ = this.mostRecentTime_;
            clone.data = this.data;

            return clone;
        }
    }
}

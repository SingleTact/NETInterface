//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;
using ZedGraph;

namespace SingleTact_Demo
{
    /// <summary>
    /// Class to buffer SingleTact Data.
    /// Long-duration history is stored in Local AppData so the RAM buffer can stay small.
    /// </summary>
    public class SingleTactData
    {
        /// <summary>
        /// Most recent time
        /// </summary>
        public double MostRecentTime
        { get { return mostRecentTime_; } }
        private double mostRecentTime_ = 0;

        // Keep up to four hours in the disk-backed buffer. Time comes from the sensor timestamp in seconds.
        private const double MAX_BUFFER_SECONDS = 4 * 60 * 60;

        // Only keep enough points in memory for live charting. The full export history is read from AppData.
        private const int MAX_POINTS_IN_MEMORY = 100 * 60;
        private const int FLUSH_INTERVAL_ROWS = 100;
        private const int CLEANUP_OLD_BUFFER_DAYS = 7;

        private readonly object bufferLock_ = new object();
        private readonly string bufferFilePath_;
        private FileStream bufferFileStream_;
        private StreamWriter bufferWriter_;
        private int rowsSinceFlush_ = 0;
        private bool hasFirstBufferedTime_ = false;
        private double firstBufferedTime_ = 0;

        /// <summary>
        /// Stripchart Data. This now intentionally contains only a small recent window.
        /// Use SnapshotPoints() when exporting the full two-hour AppData buffer.
        /// </summary>
        public List<RollingPointPairList> data = new List<RollingPointPairList>();


        public SingleTactData()
        {
            string bufferDirectory = GetBufferDirectory();
            bufferFilePath_ = Path.Combine(
                bufferDirectory,
                "SingleTact_" + DateTime.Now.ToString("yyyyMMdd_HHmmss", CultureInfo.InvariantCulture) + "_" + Guid.NewGuid().ToString("N") + ".buffer.csv");

            OpenWriter();
        }

        /// <summary>
        /// Add data to container
        /// </summary>
        /// <param name="measurements">Measurements</param>
        /// <param name="time">Time in seconds</param>
        public int AddData(double[] measurements, double time)
        {
            lock (bufferLock_)
            {
                // Resize data store to fit number of measurements.
                while (measurements.Length > data.Count)
                {
                    data.Add(new RollingPointPairList(MAX_POINTS_IN_MEMORY));
                    data.TrimExcess();
                }

                // Add the latest data to the small in-memory chart buffer.
                for (int i = 0; i < measurements.Length; i++)
                {
                    data[i].Add(time, measurements[i]);
                }

                // Append the full history to the AppData disk buffer.
                WriteRowToDisk(measurements, time);

                if (!hasFirstBufferedTime_)
                {
                    firstBufferedTime_ = time;
                    hasFirstBufferedTime_ = true;
                }

                mostRecentTime_ = time;

                double bufferSeconds = mostRecentTime_ - firstBufferedTime_;
                if (bufferSeconds < 0)
                    bufferSeconds = 0;

                int percentFull = (int)Math.Round(bufferSeconds * 100.0 / MAX_BUFFER_SECONDS);
                if (percentFull < 0)
                    percentFull = 0;
                if (percentFull > 100)
                    percentFull = 100;

                return percentFull;
            }
        }

        /// <summary>
        /// Read a snapshot of one measurement column from the two-hour AppData buffer.
        /// </summary>
        public List<PointPair> SnapshotPoints(int measurementIndex)
        {
            List<PointPair> points = new List<PointPair>();
            double cutoffTime;

            lock (bufferLock_)
            {
                FlushWriter();
                cutoffTime = mostRecentTime_ - MAX_BUFFER_SECONDS;
            }

            if (!File.Exists(bufferFilePath_))
                return points;

            using (FileStream stream = new FileStream(bufferFilePath_, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
            using (StreamReader reader = new StreamReader(stream, Encoding.UTF8))
            {
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    string[] columns = line.Split(',');
                    int valueColumn = measurementIndex + 1;
                    if (columns.Length <= valueColumn)
                        continue;

                    double time;
                    double value;
                    if (!double.TryParse(columns[0], NumberStyles.Float, CultureInfo.InvariantCulture, out time))
                        continue;
                    if (!double.TryParse(columns[valueColumn], NumberStyles.Float, CultureInfo.InvariantCulture, out value))
                        continue;

                    if (time >= cutoffTime)
                    {
                        points.Add(new PointPair(time, value));
                    }
                }
            }

            return points;
        }

        /// <summary>
        /// Clear both the small RAM chart buffer and the AppData history file.
        /// </summary>
        public void Clear()
        {
            lock (bufferLock_)
            {
                foreach (RollingPointPairList series in data)
                {
                    series.Clear();
                }

                CloseWriter();
                OpenWriter();

                mostRecentTime_ = 0;
                firstBufferedTime_ = 0;
                hasFirstBufferedTime_ = false;
                rowsSinceFlush_ = 0;
            }
        }

        /// <summary>
        /// Copy the latest chart point while acquisition and clearing are excluded.
        /// </summary>
        public bool TryGetLatestPoint(int measurementIndex, out PointPair point)
        {
            lock (bufferLock_)
            {
                point = null;
                if (measurementIndex < 0 || measurementIndex >= data.Count)
                    return false;

                RollingPointPairList series = data[measurementIndex];
                if (series.Count == 0)
                    return false;

                PointPair latest = series.Peek();
                if (latest == null)
                    return false;

                point = new PointPair(latest);
                return true;
            }
        }

        public SingleTactData Clone()
        {
            SingleTactData clone = new SingleTactData();
            clone.mostRecentTime_ = this.mostRecentTime_;
            clone.data = this.data;

            return clone;
        }

        private void WriteRowToDisk(double[] measurements, double time)
        {
            StringBuilder row = new StringBuilder();
            row.Append(time.ToString("R", CultureInfo.InvariantCulture));

            for (int i = 0; i < measurements.Length; i++)
            {
                row.Append(',');
                row.Append(measurements[i].ToString("R", CultureInfo.InvariantCulture));
            }

            bufferWriter_.WriteLine(row.ToString());

            rowsSinceFlush_++;
            if (rowsSinceFlush_ >= FLUSH_INTERVAL_ROWS)
            {
                FlushWriter();
            }
        }

        private void FlushWriter()
        {
            if (bufferWriter_ != null)
            {
                bufferWriter_.Flush();
                rowsSinceFlush_ = 0;
            }
        }

        private void OpenWriter()
        {
            bufferFileStream_ = new FileStream(bufferFilePath_, FileMode.Create, FileAccess.Write, FileShare.Read);
            bufferWriter_ = new StreamWriter(bufferFileStream_, Encoding.UTF8);
        }

        private void CloseWriter()
        {
            if (bufferWriter_ != null)
            {
                bufferWriter_.Flush();
                bufferWriter_.Dispose();
                bufferWriter_ = null;
                bufferFileStream_ = null;
            }
        }

        private static string GetBufferDirectory()
        {
            string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
            string bufferDirectory = Path.Combine(localAppData, "PPS", "SingleTact Demo", "Buffers");
            Directory.CreateDirectory(bufferDirectory);

            CleanupOldBufferFiles(bufferDirectory);

            return bufferDirectory;
        }

        private static void CleanupOldBufferFiles(string bufferDirectory)
        {
            try
            {
                string[] oldFiles = Directory.GetFiles(bufferDirectory, "*.buffer.csv");
                DateTime cutoff = DateTime.Now.AddDays(-CLEANUP_OLD_BUFFER_DAYS);

                foreach (string oldFile in oldFiles)
                {
                    if (File.GetLastWriteTime(oldFile) < cutoff)
                    {
                        File.Delete(oldFile);
                    }
                }
            }
            catch
            {
                // Buffer cleanup is best-effort only. Do not block acquisition if cleanup fails.
            }
        }
    }
}

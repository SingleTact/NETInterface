using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SingleTactLibrary
{
    public class ComPortFinder
    {
        private (List<String>, List<String>) EnumUSBComPort()
        {  // Get a human readable version of comport similar to device manager
            List<String> names = new List<string>();
            List<String> COMNumber = new List<string>();
            using (var searcher = new ManagementObjectSearcher("SELECT * FROM WIN32_SerialPort"))
            {
                var ports = searcher.Get().Cast<ManagementBaseObject>().ToList();

                ManagementObjectCollection collection;
                collection = searcher.Get();

                foreach (var device in collection)
                {
                    String deviceID = device.GetPropertyValue("DeviceID").ToString();
                    String caption = device.GetPropertyValue("Caption").ToString();
                    String DeviceID = device.GetPropertyValue("PNPDeviceID").ToString();
                    caption = caption.Replace("Arduino Leonardo", "PPS Sensor");
                    caption = caption.Split(new string[] { " (" }, StringSplitOptions.None)[0];

                    int VIDIndex = DeviceID.IndexOf("VID_");
                    int PIDIndex = DeviceID.IndexOf("PID_");

                    String PID = DeviceID.Substring(PIDIndex + 4, 4);
                    String VID = DeviceID.Substring(VIDIndex + 4, 4);

                    if (caption.Contains("Arduino"))
                    {
                        names.Add(deviceID + " - " + caption);
                        COMNumber.Add(deviceID);
                    }
                    else if (PID.Equals("8036") && VID.Equals("2341"))
                    {
                        names.Add(deviceID + " - PPS Sensor");
                        COMNumber.Add(deviceID);
                    }
                }
            }
            return (names, COMNumber);
        }

        public List<string> findSingleTact()
        {
            List<string> SingleTactUSBList = new List<string>();
            List<string> serialPortNames = new List<string>();
            (serialPortNames, _) = EnumUSBComPort();
            if (serialPortNames.Count == 1)
            {
                SingleTactUSBList.Add(serialPortNames[0]);
            }
            else if (serialPortNames.Count > 1)
            {
                // if there's more than one PPS/Arduino device connected
                bool portSelected = false;
                while (!portSelected)
                {
                    if (serialPortNames.Count > 1)
                    {
                        // Ask user to select from multiple serial ports.
                        SerialPortSelector selector = new SerialPortSelector(serialPortNames);
                        selector.ShowDialog();
                        serialPortNames = selector.SelectedPorts;
                        foreach (String portName in serialPortNames)
                        {
                            SingleTactUSBList.Add(portName);
                        }
                        portSelected = true;
                    }
                }
            }
            return SingleTactUSBList;
        }
        public string prettyToComPort(String pretty)
        {
            // get just comport to initialise devices
            return pretty.Split(new string[] { " -" }, StringSplitOptions.None)[0];
        }

    }
}

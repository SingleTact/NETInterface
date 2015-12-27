using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace SingleTact_Demo
{
   public partial class SerialPortSelector : Form
   {
      public SerialPortSelector(String[] serialPorts)
      {
         InitializeComponent();

         foreach (string port in serialPorts)
            SerialPortComboBox.Items.Add(port);

         SerialPortComboBox.SelectedIndex = 0;
      }

      private void OkButton_Click(object sender, EventArgs e)
      {
         selectedPort_ = (String) SerialPortComboBox.SelectedItem;
         this.Close();
      }

      private string selectedPort_ = "COM1";
      public string SelectedPort
      {
         get{ return selectedPort_;}
      }
   }
}

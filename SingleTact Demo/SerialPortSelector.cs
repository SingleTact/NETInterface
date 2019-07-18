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
            SerialPortCheckBoxes.Items.Add(port);

         SerialPortCheckBoxes.SelectedIndex = 0;
      }

      private void OkButton_Click(object sender, EventArgs e)
      {
            foreach (object itemChecked in SerialPortCheckBoxes.CheckedItems)
            {
                selectedPorts_.Add(itemChecked.ToString());
            }
            this.Close();
      }

      private List<string> selectedPorts_ = new List<string>();
      public List<string> SelectedPorts
      {
         get{ return selectedPorts_;}
      }

        private void label1_Click(object sender, EventArgs e)
        {

        }

        private void SerialPortSelector_Load(object sender, EventArgs e)
        {

        }

    }
}

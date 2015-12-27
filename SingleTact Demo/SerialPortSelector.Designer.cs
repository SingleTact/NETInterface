namespace SingleTact_Demo
{
   partial class SerialPortSelector
   {
      /// <summary>
      /// Required designer variable.
      /// </summary>
      private System.ComponentModel.IContainer components = null;

      /// <summary>
      /// Clean up any resources being used.
      /// </summary>
      /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
      protected override void Dispose(bool disposing)
      {
         if (disposing && (components != null))
         {
            components.Dispose();
         }
         base.Dispose(disposing);
      }

      #region Windows Form Designer generated code

      /// <summary>
      /// Required method for Designer support - do not modify
      /// the contents of this method with the code editor.
      /// </summary>
      private void InitializeComponent()
      {
         System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(SerialPortSelector));
         this.OkButton = new System.Windows.Forms.Button();
         this.SerialPortComboBox = new System.Windows.Forms.ComboBox();
         this.label1 = new System.Windows.Forms.Label();
         this.SuspendLayout();
         // 
         // OkButton
         // 
         this.OkButton.Location = new System.Drawing.Point(62, 129);
         this.OkButton.Name = "OkButton";
         this.OkButton.Size = new System.Drawing.Size(75, 23);
         this.OkButton.TabIndex = 0;
         this.OkButton.Text = "OK";
         this.OkButton.UseVisualStyleBackColor = true;
         this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
         // 
         // SerialPortComboBox
         // 
         this.SerialPortComboBox.FormattingEnabled = true;
         this.SerialPortComboBox.Location = new System.Drawing.Point(30, 91);
         this.SerialPortComboBox.Name = "SerialPortComboBox";
         this.SerialPortComboBox.Size = new System.Drawing.Size(145, 21);
         this.SerialPortComboBox.TabIndex = 1;
         // 
         // label1
         // 
         this.label1.AutoSize = true;
         this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
         this.label1.Location = new System.Drawing.Point(27, 21);
         this.label1.MaximumSize = new System.Drawing.Size(180, 0);
         this.label1.Name = "label1";
         this.label1.Size = new System.Drawing.Size(171, 56);
         this.label1.TabIndex = 2;
         this.label1.Text = "You have mulitple serial devices attached to your computer.  Please select the Ar" +
    "duino COM port.";
         // 
         // SerialPortSelector
         // 
         this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
         this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
         this.ClientSize = new System.Drawing.Size(212, 164);
         this.ControlBox = false;
         this.Controls.Add(this.label1);
         this.Controls.Add(this.SerialPortComboBox);
         this.Controls.Add(this.OkButton);
         this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
         this.Name = "SerialPortSelector";
         this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
         this.Text = "Please Select Serial Port";
         this.ResumeLayout(false);
         this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button OkButton;
      private System.Windows.Forms.ComboBox SerialPortComboBox;
      private System.Windows.Forms.Label label1;
   }
}
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
            this.label1 = new System.Windows.Forms.Label();
            this.SerialPortCheckBoxes = new System.Windows.Forms.CheckedListBox();
            this.SuspendLayout();
            // 
            // OkButton
            // 
            this.OkButton.Location = new System.Drawing.Point(191, 610);
            this.OkButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.OkButton.Name = "OkButton";
            this.OkButton.Size = new System.Drawing.Size(238, 65);
            this.OkButton.TabIndex = 0;
            this.OkButton.Text = "OK";
            this.OkButton.UseVisualStyleBackColor = true;
            this.OkButton.Click += new System.EventHandler(this.OkButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Tai Le", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(86, 60);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label1.MaximumSize = new System.Drawing.Size(570, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(541, 168);
            this.label1.TabIndex = 2;
            this.label1.Text = "You have mulitple serial devices attached to your computer.  Please select the Ar" +
    "duino/USB Board COM ports.";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // SerialPortCheckBoxes
            // 
            this.SerialPortCheckBoxes.FormattingEnabled = true;
            this.SerialPortCheckBoxes.Location = new System.Drawing.Point(63, 274);
            this.SerialPortCheckBoxes.Name = "SerialPortCheckBoxes";
            this.SerialPortCheckBoxes.Size = new System.Drawing.Size(539, 277);
            this.SerialPortCheckBoxes.TabIndex = 3;
            // 
            // SerialPortSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(660, 749);
            this.ControlBox = false;
            this.Controls.Add(this.SerialPortCheckBoxes);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.OkButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.Name = "SerialPortSelector";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Please Select Serial Port";
            this.Load += new System.EventHandler(this.SerialPortSelector_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

      }

      #endregion

      private System.Windows.Forms.Button OkButton;
      private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckedListBox SerialPortCheckBoxes;
    }
}
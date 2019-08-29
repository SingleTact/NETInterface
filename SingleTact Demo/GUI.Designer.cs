//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

using System;

namespace SingleTact_Demo
{
    partial class GUI
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(GUI));
            this.graph_ = new ZedGraph.ZedGraphControl();
            this.AcquisitionWorker = new System.ComponentModel.BackgroundWorker();
            this.guiTimer_ = new System.Windows.Forms.Timer(this.components);
            this.picPpsLogo = new System.Windows.Forms.PictureBox();
            this.Settings = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.textGain = new System.Windows.Forms.TextBox();
            this.textTare = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.textScale = new System.Windows.Forms.TextBox();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.label2 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.i2cAddressInputComboBox_ = new System.Windows.Forms.ComboBox();
            this.SetSettingsButton = new System.Windows.Forms.Button();
            this.ActiveSensor = new System.Windows.Forms.ComboBox();
            this.ActiveSensorLabel = new System.Windows.Forms.Label();
            this.tareAll = new System.Windows.Forms.Button();
            this.SetBaselineButton = new System.Windows.Forms.Button();
            this.buttonSave = new System.Windows.Forms.Button();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            ((System.ComponentModel.ISupportInitialize)(this.picPpsLogo)).BeginInit();
            this.Settings.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.SuspendLayout();
            // 
            // graph_
            // 
            this.graph_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graph_.IsEnableHPan = false;
            this.graph_.IsEnableHZoom = false;
            this.graph_.Location = new System.Drawing.Point(191, 18);
            this.graph_.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.graph_.Name = "graph_";
            this.graph_.ScrollGrace = 0D;
            this.graph_.ScrollMaxX = 0D;
            this.graph_.ScrollMaxY = 0D;
            this.graph_.ScrollMaxY2 = 0D;
            this.graph_.ScrollMinX = 0D;
            this.graph_.ScrollMinY = 0D;
            this.graph_.ScrollMinY2 = 0D;
            this.graph_.Size = new System.Drawing.Size(675, 409);
            this.graph_.TabIndex = 1;
            // 
            // AcquisitionWorker
            // 
            this.AcquisitionWorker.WorkerReportsProgress = true;
            this.AcquisitionWorker.WorkerSupportsCancellation = true;
            this.AcquisitionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AcquisitionWorker_DoWork);
            this.AcquisitionWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AcquisitionWorker_RunWorkerCompleted);
            // 
            // guiTimer_
            // 
            this.guiTimer_.Interval = 1;
            this.guiTimer_.Tick += new System.EventHandler(this.guiTimer__Tick);
            // 
            // picPpsLogo
            // 
            this.picPpsLogo.Image = ((System.Drawing.Image)(resources.GetObject("picPpsLogo.Image")));
            this.picPpsLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("picPpsLogo.InitialImage")));
            this.picPpsLogo.Location = new System.Drawing.Point(7, 22);
            this.picPpsLogo.Margin = new System.Windows.Forms.Padding(2, 2, 2, 2);
            this.picPpsLogo.Name = "picPpsLogo";
            this.picPpsLogo.Size = new System.Drawing.Size(172, 55);
            this.picPpsLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPpsLogo.TabIndex = 46;
            this.picPpsLogo.TabStop = false;
            // 
            // Settings
            // 
            this.Settings.AccessibleName = "Settings";
            this.Settings.AccessibleRole = System.Windows.Forms.AccessibleRole.None;
            this.Settings.Controls.Add(this.tabPage1);
            this.Settings.Controls.Add(this.tabPage2);
            this.Settings.Location = new System.Drawing.Point(7, 275);
            this.Settings.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.Settings.Name = "Settings";
            this.Settings.SelectedIndex = 0;
            this.Settings.Size = new System.Drawing.Size(177, 152);
            this.Settings.TabIndex = 56;
            this.Settings.Visible = false;
            // 
            // tabPage1
            // 
            this.tabPage1.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage1.Controls.Add(this.textAddress);
            this.tabPage1.Controls.Add(this.label1);
            this.tabPage1.Controls.Add(this.textGain);
            this.tabPage1.Controls.Add(this.textTare);
            this.tabPage1.Controls.Add(this.label9);
            this.tabPage1.Controls.Add(this.textScale);
            this.tabPage1.Controls.Add(this.label8);
            this.tabPage1.Controls.Add(this.label7);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tabPage1.Size = new System.Drawing.Size(169, 126);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            // 
            // textAddress
            // 
            this.textAddress.BackColor = System.Drawing.SystemColors.Control;
            this.textAddress.Enabled = false;
            this.textAddress.Location = new System.Drawing.Point(90, 15);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(77, 20);
            this.textAddress.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(4, 17);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 13);
            this.label1.TabIndex = 38;
            this.label1.Text = "I2C Address:";
            // 
            // textGain
            // 
            this.textGain.BackColor = System.Drawing.SystemColors.Control;
            this.textGain.Enabled = false;
            this.textGain.Location = new System.Drawing.Point(90, 41);
            this.textGain.Name = "textGain";
            this.textGain.Size = new System.Drawing.Size(77, 20);
            this.textGain.TabIndex = 40;
            // 
            // textTare
            // 
            this.textTare.BackColor = System.Drawing.SystemColors.Control;
            this.textTare.Enabled = false;
            this.textTare.Location = new System.Drawing.Point(90, 68);
            this.textTare.Name = "textTare";
            this.textTare.Size = new System.Drawing.Size(77, 20);
            this.textTare.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(4, 95);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(83, 13);
            this.label9.TabIndex = 45;
            this.label9.Text = "Scale Factor:";
            // 
            // textScale
            // 
            this.textScale.BackColor = System.Drawing.SystemColors.Control;
            this.textScale.Enabled = false;
            this.textScale.Location = new System.Drawing.Point(90, 93);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(77, 20);
            this.textScale.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(4, 69);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(59, 13);
            this.label8.TabIndex = 44;
            this.label8.Text = "Baseline:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(4, 43);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(100, 13);
            this.label7.TabIndex = 43;
            this.label7.Text = "Reference Gain:";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.label2);
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.i2cAddressInputComboBox_);
            this.tabPage2.Controls.Add(this.SetSettingsButton);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tabPage2.Size = new System.Drawing.Size(169, 126);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Update Flash";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(7, 13);
            this.label2.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(157, 58);
            this.label2.TabIndex = 55;
            this.label2.Text = "Warning: changing the I2C Address can damage the hardware. Only update this if yo" +
    "u are sure it is necessary.";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(7, 74);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(79, 13);
            this.label5.TabIndex = 53;
            this.label5.Text = "I2C Address:";
            // 
            // i2cAddressInputComboBox_
            // 
            this.i2cAddressInputComboBox_.FormattingEnabled = true;
            this.i2cAddressInputComboBox_.Location = new System.Drawing.Point(86, 74);
            this.i2cAddressInputComboBox_.Name = "i2cAddressInputComboBox_";
            this.i2cAddressInputComboBox_.Size = new System.Drawing.Size(77, 21);
            this.i2cAddressInputComboBox_.TabIndex = 54;
            // 
            // SetSettingsButton
            // 
            this.SetSettingsButton.Location = new System.Drawing.Point(6, 96);
            this.SetSettingsButton.Name = "SetSettingsButton";
            this.SetSettingsButton.Size = new System.Drawing.Size(153, 23);
            this.SetSettingsButton.TabIndex = 51;
            this.SetSettingsButton.Text = "Update I2C Address";
            this.SetSettingsButton.UseVisualStyleBackColor = true;
            this.SetSettingsButton.Click += new System.EventHandler(this.SetSettingsButton_Click);
            // 
            // ActiveSensor
            // 
            this.ActiveSensor.FormattingEnabled = true;
            this.ActiveSensor.Location = new System.Drawing.Point(92, 128);
            this.ActiveSensor.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.ActiveSensor.Name = "ActiveSensor";
            this.ActiveSensor.Size = new System.Drawing.Size(91, 21);
            this.ActiveSensor.TabIndex = 69;
            this.ActiveSensor.SelectedIndexChanged += new System.EventHandler(this.ActiveSensor_SelectedIndexChanged);
            // 
            // ActiveSensorLabel
            // 
            this.ActiveSensorLabel.AutoSize = true;
            this.ActiveSensorLabel.Location = new System.Drawing.Point(5, 130);
            this.ActiveSensorLabel.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.ActiveSensorLabel.Name = "ActiveSensorLabel";
            this.ActiveSensorLabel.Size = new System.Drawing.Size(85, 13);
            this.ActiveSensorLabel.TabIndex = 70;
            this.ActiveSensorLabel.Text = "Selected Sensor";
            // 
            // tareAll
            // 
            this.tareAll.Location = new System.Drawing.Point(7, 178);
            this.tareAll.Margin = new System.Windows.Forms.Padding(1, 1, 1, 1);
            this.tareAll.Name = "tareAll";
            this.tareAll.Size = new System.Drawing.Size(173, 23);
            this.tareAll.TabIndex = 68;
            this.tareAll.Text = "Tare All Sensors";
            this.tareAll.UseVisualStyleBackColor = true;
            this.tareAll.Click += new System.EventHandler(this.tareAllButton_Click);
            // 
            // SetBaselineButton
            // 
            this.SetBaselineButton.Location = new System.Drawing.Point(7, 151);
            this.SetBaselineButton.Name = "SetBaselineButton";
            this.SetBaselineButton.Size = new System.Drawing.Size(173, 23);
            this.SetBaselineButton.TabIndex = 67;
            this.SetBaselineButton.Text = "Tare Selected Sensor";
            this.SetBaselineButton.UseVisualStyleBackColor = true;
            this.SetBaselineButton.Click += new System.EventHandler(this.SetBaselineButton_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(7, 206);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(173, 23);
            this.buttonSave.TabIndex = 63;
            this.buttonSave.Text = "Export Chart Data";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // linkLabel1
            // 
            this.linkLabel1.ActiveLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.LinkColor = System.Drawing.Color.Black;
            this.linkLabel1.Location = new System.Drawing.Point(5, 253);
            this.linkLabel1.Margin = new System.Windows.Forms.Padding(1, 0, 1, 0);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(68, 13);
            this.linkLabel1.TabIndex = 71;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "▼ Advanced";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 437);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.ActiveSensor);
            this.Controls.Add(this.ActiveSensorLabel);
            this.Controls.Add(this.tareAll);
            this.Controls.Add(this.SetBaselineButton);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.Settings);
            this.Controls.Add(this.picPpsLogo);
            this.Controls.Add(this.graph_);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MinimumSize = new System.Drawing.Size(415, 264);
            this.Name = "GUI";
            this.Text = "PPS SingleTact Demo [XXHz]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picPpsLogo)).EndInit();
            this.Settings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }


        #endregion

        private ZedGraph.ZedGraphControl graph_;
        private System.ComponentModel.BackgroundWorker AcquisitionWorker;
        private System.Windows.Forms.Timer guiTimer_;
        private System.Windows.Forms.PictureBox picPpsLogo;
        private System.Windows.Forms.TabControl Settings;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.ComboBox i2cAddressInputComboBox_;
        private System.Windows.Forms.Button SetSettingsButton;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textGain;
        private System.Windows.Forms.TextBox textTare;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.TextBox textScale;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.ComboBox ActiveSensor;
        private System.Windows.Forms.Label ActiveSensorLabel;
        private System.Windows.Forms.Button tareAll;
        private System.Windows.Forms.Button SetBaselineButton;
        private System.Windows.Forms.Button buttonSave;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label2;
    }
}

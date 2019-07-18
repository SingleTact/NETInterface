//-----------------------------------------------------------------------------
//  Copyright (c) 2015 Pressure Profile Systems
//
//  Licensed under the MIT license. This file may not be copied, modified, or
//  distributed except according to those terms.
//-----------------------------------------------------------------------------

﻿namespace SingleTact_Demo
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
            this.RefreshFlashSettings = new System.Windows.Forms.Button();
            this.SetSettingsButton = new System.Windows.Forms.Button();
            this.SetBaselineButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.textAddress = new System.Windows.Forms.TextBox();
            this.label9 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.textScale = new System.Windows.Forms.TextBox();
            this.textTare = new System.Windows.Forms.TextBox();
            this.textGain = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.buttonSave = new System.Windows.Forms.Button();
            this.AcquisitionWorker = new System.ComponentModel.BackgroundWorker();
            this.i2cAddressInputComboBox_ = new System.Windows.Forms.ComboBox();
            this.scaleInputTrackBar_ = new System.Windows.Forms.TrackBar();
            this.ScaleInputValueLabel = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.sensorTypeSelector = new System.Windows.Forms.ComboBox();
            this.ResetSensorButton = new System.Windows.Forms.Button();
            this.guiTimer_ = new System.Windows.Forms.Timer(this.components);
            this.LockButton = new System.Windows.Forms.Button();
            this.arduinoSingleTactDriver = new SingleTactLibrary.ArduinoSingleTactDriver(this.components);
            this.picPpsLogo = new System.Windows.Forms.PictureBox();
            this.ActiveSensor = new System.Windows.Forms.ComboBox();
            this.ActiveSensorLabel = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.scaleInputTrackBar_)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPpsLogo)).BeginInit();
            this.SuspendLayout();
            // 
            // graph_
            // 
            this.graph_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graph_.IsEnableHPan = false;
            this.graph_.IsEnableHZoom = false;
            this.graph_.Location = new System.Drawing.Point(827, 63);
            this.graph_.Margin = new System.Windows.Forms.Padding(32, 26, 32, 26);
            this.graph_.Name = "graph_";
            this.graph_.ScrollGrace = 0D;
            this.graph_.ScrollMaxX = 0D;
            this.graph_.ScrollMaxY = 0D;
            this.graph_.ScrollMaxY2 = 0D;
            this.graph_.ScrollMinX = 0D;
            this.graph_.ScrollMinY = 0D;
            this.graph_.ScrollMinY2 = 0D;
            this.graph_.Size = new System.Drawing.Size(1777, 1423);
            this.graph_.TabIndex = 1;
            // 
            // RefreshFlashSettings
            // 
            this.RefreshFlashSettings.Location = new System.Drawing.Point(184, 364);
            this.RefreshFlashSettings.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.RefreshFlashSettings.Name = "RefreshFlashSettings";
            this.RefreshFlashSettings.Size = new System.Drawing.Size(329, 65);
            this.RefreshFlashSettings.TabIndex = 4;
            this.RefreshFlashSettings.Text = "Refresh";
            this.RefreshFlashSettings.UseVisualStyleBackColor = true;
            this.RefreshFlashSettings.Click += new System.EventHandler(this.RefreshFlashSettings_Click);
            // 
            // SetSettingsButton
            // 
            this.SetSettingsButton.Location = new System.Drawing.Point(29, 384);
            this.SetSettingsButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.SetSettingsButton.Name = "SetSettingsButton";
            this.SetSettingsButton.Size = new System.Drawing.Size(323, 65);
            this.SetSettingsButton.TabIndex = 5;
            this.SetSettingsButton.Text = "Set Configuration";
            this.SetSettingsButton.UseVisualStyleBackColor = true;
            this.SetSettingsButton.Click += new System.EventHandler(this.SetSettingsButton_Click);
            // 
            // SetBaselineButton
            // 
            this.SetBaselineButton.Location = new System.Drawing.Point(361, 384);
            this.SetBaselineButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.SetBaselineButton.Name = "SetBaselineButton";
            this.SetBaselineButton.Size = new System.Drawing.Size(323, 65);
            this.SetBaselineButton.TabIndex = 8;
            this.SetBaselineButton.Text = "Update Baseline";
            this.SetBaselineButton.UseVisualStyleBackColor = true;
            this.SetBaselineButton.Click += new System.EventHandler(this.SetBaselineButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(41, 63);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 38);
            this.label1.TabIndex = 9;
            this.label1.Text = "I2C Address:";
            // 
            // textAddress
            // 
            this.textAddress.BackColor = System.Drawing.SystemColors.Control;
            this.textAddress.Location = new System.Drawing.Point(367, 54);
            this.textAddress.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(314, 44);
            this.textAddress.TabIndex = 10;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(41, 285);
            this.label9.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(223, 38);
            this.label9.TabIndex = 36;
            this.label9.Text = "Scale Factor:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(41, 211);
            this.label8.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 38);
            this.label8.TabIndex = 35;
            this.label8.Text = "Baseline:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(41, 137);
            this.label7.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(270, 38);
            this.label7.TabIndex = 34;
            this.label7.Text = "Reference Gain:";
            // 
            // textScale
            // 
            this.textScale.BackColor = System.Drawing.SystemColors.Control;
            this.textScale.Location = new System.Drawing.Point(367, 276);
            this.textScale.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(314, 44);
            this.textScale.TabIndex = 33;
            // 
            // textTare
            // 
            this.textTare.BackColor = System.Drawing.SystemColors.Control;
            this.textTare.Location = new System.Drawing.Point(367, 205);
            this.textTare.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textTare.Name = "textTare";
            this.textTare.Size = new System.Drawing.Size(314, 44);
            this.textTare.TabIndex = 32;
            // 
            // textGain
            // 
            this.textGain.BackColor = System.Drawing.SystemColors.Control;
            this.textGain.Location = new System.Drawing.Point(367, 128);
            this.textGain.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textGain.Name = "textGain";
            this.textGain.Size = new System.Drawing.Size(314, 44);
            this.textGain.TabIndex = 31;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(54, 80);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 38);
            this.label5.TabIndex = 43;
            this.label5.Text = "I2C Address:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(48, 239);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 38);
            this.label4.TabIndex = 39;
            this.label4.Text = "Scale Factor:";
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(73, 1412);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(656, 65);
            this.buttonSave.TabIndex = 45;
            this.buttonSave.Text = "Export Chart Data";
            this.buttonSave.UseVisualStyleBackColor = true;
            this.buttonSave.Click += new System.EventHandler(this.buttonSave_Click);
            // 
            // AcquisitionWorker
            // 
            this.AcquisitionWorker.WorkerReportsProgress = true;
            this.AcquisitionWorker.WorkerSupportsCancellation = true;
            this.AcquisitionWorker.DoWork += new System.ComponentModel.DoWorkEventHandler(this.AcquisitionWorker_DoWork);
            this.AcquisitionWorker.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.AcquisitionWorker_RunWorkerCompleted);
            // 
            // i2cAddressInputComboBox_
            // 
            this.i2cAddressInputComboBox_.FormattingEnabled = true;
            this.i2cAddressInputComboBox_.Location = new System.Drawing.Point(367, 71);
            this.i2cAddressInputComboBox_.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.i2cAddressInputComboBox_.Name = "i2cAddressInputComboBox_";
            this.i2cAddressInputComboBox_.Size = new System.Drawing.Size(314, 45);
            this.i2cAddressInputComboBox_.TabIndex = 47;
            // 
            // scaleInputTrackBar_
            // 
            this.scaleInputTrackBar_.Location = new System.Drawing.Point(348, 239);
            this.scaleInputTrackBar_.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.scaleInputTrackBar_.Maximum = 6400;
            this.scaleInputTrackBar_.Minimum = 100;
            this.scaleInputTrackBar_.Name = "scaleInputTrackBar_";
            this.scaleInputTrackBar_.Size = new System.Drawing.Size(358, 136);
            this.scaleInputTrackBar_.TabIndex = 49;
            this.scaleInputTrackBar_.TickFrequency = 640;
            this.scaleInputTrackBar_.Value = 100;
            this.scaleInputTrackBar_.Scroll += new System.EventHandler(this.scaleInputTrackBar__Scroll);
            // 
            // ScaleInputValueLabel
            // 
            this.ScaleInputValueLabel.AutoSize = true;
            this.ScaleInputValueLabel.Location = new System.Drawing.Point(124, 290);
            this.ScaleInputValueLabel.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ScaleInputValueLabel.Name = "ScaleInputValueLabel";
            this.ScaleInputValueLabel.Size = new System.Drawing.Size(53, 37);
            this.ScaleInputValueLabel.TabIndex = 50;
            this.ScaleInputValueLabel.Text = "##";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.ScaleInputValueLabel);
            this.groupBox1.Controls.Add(this.SetBaselineButton);
            this.groupBox1.Controls.Add(this.scaleInputTrackBar_);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.i2cAddressInputComboBox_);
            this.groupBox1.Controls.Add(this.SetSettingsButton);
            this.groupBox1.Location = new System.Drawing.Point(44, 817);
            this.groupBox1.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.groupBox1.Size = new System.Drawing.Size(725, 495);
            this.groupBox1.TabIndex = 51;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Update Flash";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.textAddress);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.textGain);
            this.groupBox2.Controls.Add(this.textTare);
            this.groupBox2.Controls.Add(this.label9);
            this.groupBox2.Controls.Add(this.RefreshFlashSettings);
            this.groupBox2.Controls.Add(this.textScale);
            this.groupBox2.Controls.Add(this.label8);
            this.groupBox2.Controls.Add(this.label7);
            this.groupBox2.Location = new System.Drawing.Point(38, 282);
            this.groupBox2.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Padding = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.groupBox2.Size = new System.Drawing.Size(725, 475);
            this.groupBox2.TabIndex = 52;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Settings On Flash";
            // 
            // sensorTypeSelector
            // 
            this.sensorTypeSelector.FormattingEnabled = true;
            this.sensorTypeSelector.Location = new System.Drawing.Point(393, 1503);
            this.sensorTypeSelector.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.sensorTypeSelector.Name = "sensorTypeSelector";
            this.sensorTypeSelector.Size = new System.Drawing.Size(375, 45);
            this.sensorTypeSelector.TabIndex = 53;
            this.sensorTypeSelector.Visible = false;
            // 
            // ResetSensorButton
            // 
            this.ResetSensorButton.Location = new System.Drawing.Point(41, 1497);
            this.ResetSensorButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.ResetSensorButton.Name = "ResetSensorButton";
            this.ResetSensorButton.Size = new System.Drawing.Size(333, 63);
            this.ResetSensorButton.TabIndex = 54;
            this.ResetSensorButton.Text = "Reset Sensor As:";
            this.ResetSensorButton.UseVisualStyleBackColor = true;
            this.ResetSensorButton.Visible = false;
            this.ResetSensorButton.Click += new System.EventHandler(this.ResetSensorButton_Click);
            // 
            // guiTimer_
            // 
            this.guiTimer_.Interval = 50;
            this.guiTimer_.Tick += new System.EventHandler(this.guiTimer__Tick);
            // 
            // LockButton
            // 
            this.LockButton.Location = new System.Drawing.Point(73, 1329);
            this.LockButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.LockButton.Name = "LockButton";
            this.LockButton.Size = new System.Drawing.Size(656, 65);
            this.LockButton.TabIndex = 55;
            this.LockButton.Text = "Lock";
            this.LockButton.UseVisualStyleBackColor = true;
            this.LockButton.Click += new System.EventHandler(this.LockButton_Click);
            // 
            // picPpsLogo
            // 
            this.picPpsLogo.Image = ((System.Drawing.Image)(resources.GetObject("picPpsLogo.Image")));
            this.picPpsLogo.InitialImage = ((System.Drawing.Image)(resources.GetObject("picPpsLogo.InitialImage")));
            this.picPpsLogo.Location = new System.Drawing.Point(73, 63);
            this.picPpsLogo.Margin = new System.Windows.Forms.Padding(6);
            this.picPpsLogo.Name = "picPpsLogo";
            this.picPpsLogo.Size = new System.Drawing.Size(545, 157);
            this.picPpsLogo.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picPpsLogo.TabIndex = 46;
            this.picPpsLogo.TabStop = false;
            // 
            // ActiveSensor
            // 
            this.ActiveSensor.FormattingEnabled = true;
            this.ActiveSensor.Location = new System.Drawing.Point(398, 238);
            this.ActiveSensor.Name = "ActiveSensor";
            this.ActiveSensor.Size = new System.Drawing.Size(321, 45);
            this.ActiveSensor.TabIndex = 56;
            this.ActiveSensor.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // label2
            // 
            this.ActiveSensorLabel.AutoSize = true;
            this.ActiveSensorLabel.Location = new System.Drawing.Point(79, 241);
            this.ActiveSensorLabel.Name = "label2";
            this.ActiveSensorLabel.Size = new System.Drawing.Size(213, 37);
            this.ActiveSensorLabel.TabIndex = 57;
            this.ActiveSensorLabel.Text = "Active Sensor";
            this.ActiveSensorLabel.Click += new System.EventHandler(this.label2_Click);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2641, 1600);
            this.Controls.Add(this.ActiveSensorLabel);
            this.Controls.Add(this.ActiveSensor);
            this.Controls.Add(this.LockButton);
            this.Controls.Add(this.ResetSensorButton);
            this.Controls.Add(this.sensorTypeSelector);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.picPpsLogo);
            this.Controls.Add(this.buttonSave);
            this.Controls.Add(this.graph_);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.MinimumSize = new System.Drawing.Size(2598, 1227);
            this.Name = "GUI";
            this.Text = "PPS SingleTact Demo [XXHz]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.scaleInputTrackBar_)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.picPpsLogo)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ZedGraph.ZedGraphControl graph_;
        private System.Windows.Forms.Button RefreshFlashSettings;
        private System.Windows.Forms.Button SetSettingsButton;
        private System.Windows.Forms.Button SetBaselineButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox textAddress;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.TextBox textScale;
        private System.Windows.Forms.TextBox textTare;
        private System.Windows.Forms.TextBox textGain;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button buttonSave;
        private System.ComponentModel.BackgroundWorker AcquisitionWorker;
        private SingleTactLibrary.ArduinoSingleTactDriver arduinoSingleTactDriver;
        private System.Windows.Forms.ComboBox i2cAddressInputComboBox_;
        private System.Windows.Forms.TrackBar scaleInputTrackBar_;
        private System.Windows.Forms.Label ScaleInputValueLabel;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox sensorTypeSelector;
        private System.Windows.Forms.Button ResetSensorButton;
        private System.Windows.Forms.Timer guiTimer_;
        private System.Windows.Forms.Button LockButton;
        private System.Windows.Forms.PictureBox picPpsLogo;
        private System.Windows.Forms.ComboBox ActiveSensor;
        private System.Windows.Forms.Label ActiveSensorLabel;
    }
}

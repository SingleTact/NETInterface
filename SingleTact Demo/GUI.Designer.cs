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
            this.label5 = new System.Windows.Forms.Label();
            this.ScaleInputValueLabel = new System.Windows.Forms.Label();
            this.scaleInputTrackBar_ = new System.Windows.Forms.TrackBar();
            this.label4 = new System.Windows.Forms.Label();
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
            ((System.ComponentModel.ISupportInitialize)(this.scaleInputTrackBar_)).BeginInit();
            this.SuspendLayout();
            // 
            // graph_
            // 
            this.graph_.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.graph_.IsEnableHPan = false;
            this.graph_.IsEnableHZoom = false;
            this.graph_.Location = new System.Drawing.Point(606, 63);
            this.graph_.Margin = new System.Windows.Forms.Padding(32, 26, 32, 26);
            this.graph_.Name = "graph_";
            this.graph_.ScrollGrace = 0D;
            this.graph_.ScrollMaxX = 0D;
            this.graph_.ScrollMaxY = 0D;
            this.graph_.ScrollMaxY2 = 0D;
            this.graph_.ScrollMinX = 0D;
            this.graph_.ScrollMinY = 0D;
            this.graph_.ScrollMinY2 = 0D;
            this.graph_.Size = new System.Drawing.Size(1998, 1154);
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
            this.picPpsLogo.Location = new System.Drawing.Point(23, 63);
            this.picPpsLogo.Margin = new System.Windows.Forms.Padding(6);
            this.picPpsLogo.Name = "picPpsLogo";
            this.picPpsLogo.Size = new System.Drawing.Size(545, 157);
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
            this.Settings.Location = new System.Drawing.Point(23, 783);
            this.Settings.Name = "Settings";
            this.Settings.SelectedIndex = 0;
            this.Settings.Size = new System.Drawing.Size(561, 434);
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
            this.tabPage1.Location = new System.Drawing.Point(12, 58);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(537, 364);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Settings";
            // 
            // textAddress
            // 
            this.textAddress.BackColor = System.Drawing.SystemColors.Control;
            this.textAddress.Enabled = false;
            this.textAddress.Location = new System.Drawing.Point(284, 43);
            this.textAddress.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textAddress.Name = "textAddress";
            this.textAddress.Size = new System.Drawing.Size(236, 44);
            this.textAddress.TabIndex = 39;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.Location = new System.Drawing.Point(14, 49);
            this.label1.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(218, 38);
            this.label1.TabIndex = 38;
            this.label1.Text = "I2C Address:";
            // 
            // textGain
            // 
            this.textGain.BackColor = System.Drawing.SystemColors.Control;
            this.textGain.Enabled = false;
            this.textGain.Location = new System.Drawing.Point(284, 117);
            this.textGain.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textGain.Name = "textGain";
            this.textGain.Size = new System.Drawing.Size(236, 44);
            this.textGain.TabIndex = 40;
            // 
            // textTare
            // 
            this.textTare.BackColor = System.Drawing.SystemColors.Control;
            this.textTare.Enabled = false;
            this.textTare.Location = new System.Drawing.Point(284, 194);
            this.textTare.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textTare.Name = "textTare";
            this.textTare.Size = new System.Drawing.Size(236, 44);
            this.textTare.TabIndex = 41;
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label9.Location = new System.Drawing.Point(14, 271);
            this.label9.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(223, 38);
            this.label9.TabIndex = 45;
            this.label9.Text = "Scale Factor:";
            // 
            // textScale
            // 
            this.textScale.BackColor = System.Drawing.SystemColors.Control;
            this.textScale.Enabled = false;
            this.textScale.Location = new System.Drawing.Point(284, 265);
            this.textScale.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.textScale.Name = "textScale";
            this.textScale.Size = new System.Drawing.Size(236, 44);
            this.textScale.TabIndex = 42;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label8.Location = new System.Drawing.Point(14, 197);
            this.label8.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(160, 38);
            this.label8.TabIndex = 44;
            this.label8.Text = "Baseline:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label7.Location = new System.Drawing.Point(14, 123);
            this.label7.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(270, 38);
            this.label7.TabIndex = 43;
            this.label7.Text = "Reference Gain:";
            // 
            // tabPage2
            // 
            this.tabPage2.BackColor = System.Drawing.Color.WhiteSmoke;
            this.tabPage2.Controls.Add(this.label5);
            this.tabPage2.Controls.Add(this.ScaleInputValueLabel);
            this.tabPage2.Controls.Add(this.scaleInputTrackBar_);
            this.tabPage2.Controls.Add(this.label4);
            this.tabPage2.Controls.Add(this.i2cAddressInputComboBox_);
            this.tabPage2.Controls.Add(this.SetSettingsButton);
            this.tabPage2.Location = new System.Drawing.Point(12, 58);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(537, 364);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Update Flash";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(18, 44);
            this.label5.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(218, 38);
            this.label5.TabIndex = 53;
            this.label5.Text = "I2C Address:";
            // 
            // ScaleInputValueLabel
            // 
            this.ScaleInputValueLabel.AutoSize = true;
            this.ScaleInputValueLabel.Location = new System.Drawing.Point(89, 187);
            this.ScaleInputValueLabel.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.ScaleInputValueLabel.Name = "ScaleInputValueLabel";
            this.ScaleInputValueLabel.Size = new System.Drawing.Size(53, 37);
            this.ScaleInputValueLabel.TabIndex = 56;
            this.ScaleInputValueLabel.Text = "##";
            // 
            // scaleInputTrackBar_
            // 
            this.scaleInputTrackBar_.Location = new System.Drawing.Point(256, 136);
            this.scaleInputTrackBar_.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.scaleInputTrackBar_.Maximum = 6400;
            this.scaleInputTrackBar_.Minimum = 100;
            this.scaleInputTrackBar_.Name = "scaleInputTrackBar_";
            this.scaleInputTrackBar_.Size = new System.Drawing.Size(260, 136);
            this.scaleInputTrackBar_.TabIndex = 55;
            this.scaleInputTrackBar_.TickFrequency = 640;
            this.scaleInputTrackBar_.Value = 100;
            this.scaleInputTrackBar_.Scroll += new System.EventHandler(this.scaleInputTrackBar__Scroll);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label4.Location = new System.Drawing.Point(13, 136);
            this.label4.Margin = new System.Windows.Forms.Padding(10, 0, 10, 0);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(223, 38);
            this.label4.TabIndex = 52;
            this.label4.Text = "Scale Factor:";
            // 
            // i2cAddressInputComboBox_
            // 
            this.i2cAddressInputComboBox_.FormattingEnabled = true;
            this.i2cAddressInputComboBox_.Location = new System.Drawing.Point(269, 43);
            this.i2cAddressInputComboBox_.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.i2cAddressInputComboBox_.Name = "i2cAddressInputComboBox_";
            this.i2cAddressInputComboBox_.Size = new System.Drawing.Size(235, 45);
            this.i2cAddressInputComboBox_.TabIndex = 54;
            // 
            // SetSettingsButton
            // 
            this.SetSettingsButton.Location = new System.Drawing.Point(19, 273);
            this.SetSettingsButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.SetSettingsButton.Name = "SetSettingsButton";
            this.SetSettingsButton.Size = new System.Drawing.Size(485, 65);
            this.SetSettingsButton.TabIndex = 51;
            this.SetSettingsButton.Text = "Save Configuration";
            this.SetSettingsButton.UseVisualStyleBackColor = true;
            this.SetSettingsButton.Click += new System.EventHandler(this.SetSettingsButton_Click);
            // 
            // ActiveSensor
            // 
            this.ActiveSensor.FormattingEnabled = true;
            this.ActiveSensor.Location = new System.Drawing.Point(292, 363);
            this.ActiveSensor.Name = "ActiveSensor";
            this.ActiveSensor.Size = new System.Drawing.Size(280, 45);
            this.ActiveSensor.TabIndex = 69;
            // 
            // ActiveSensorLabel
            // 
            this.ActiveSensorLabel.AutoSize = true;
            this.ActiveSensorLabel.Location = new System.Drawing.Point(16, 371);
            this.ActiveSensorLabel.Name = "ActiveSensorLabel";
            this.ActiveSensorLabel.Size = new System.Drawing.Size(213, 37);
            this.ActiveSensorLabel.TabIndex = 70;
            this.ActiveSensorLabel.Text = "Active Sensor";
            // 
            // tareAll
            // 
            this.tareAll.Location = new System.Drawing.Point(23, 508);
            this.tareAll.Name = "tareAll";
            this.tareAll.Size = new System.Drawing.Size(549, 65);
            this.tareAll.TabIndex = 68;
            this.tareAll.Text = "Tare All Sensors";
            this.tareAll.UseVisualStyleBackColor = true;
            this.tareAll.Click += new System.EventHandler(this.tareAllButton_Click);
            // 
            // SetBaselineButton
            // 
            this.SetBaselineButton.Location = new System.Drawing.Point(23, 431);
            this.SetBaselineButton.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.SetBaselineButton.Name = "SetBaselineButton";
            this.SetBaselineButton.Size = new System.Drawing.Size(549, 65);
            this.SetBaselineButton.TabIndex = 67;
            this.SetBaselineButton.Text = "Tare Active Sensor";
            this.SetBaselineButton.UseVisualStyleBackColor = true;
            this.SetBaselineButton.Click += new System.EventHandler(this.SetBaselineButton_Click);
            // 
            // buttonSave
            // 
            this.buttonSave.Location = new System.Drawing.Point(23, 585);
            this.buttonSave.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.buttonSave.Name = "buttonSave";
            this.buttonSave.Size = new System.Drawing.Size(549, 65);
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
            this.linkLabel1.Location = new System.Drawing.Point(16, 721);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(189, 37);
            this.linkLabel1.TabIndex = 71;
            this.linkLabel1.TabStop = true;
            this.linkLabel1.Text = "▼ Advanced";
            this.linkLabel1.VisitedLinkColor = System.Drawing.Color.Black;
            this.linkLabel1.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.LinkLabel1_LinkClicked);
            // 
            // GUI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(19F, 37F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(2641, 1278);
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
            this.Margin = new System.Windows.Forms.Padding(10, 9, 10, 9);
            this.MinimumSize = new System.Drawing.Size(2598, 1227);
            this.Name = "GUI";
            this.Text = "PPS SingleTact Demo [XXHz]";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.GUI_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.picPpsLogo)).EndInit();
            this.Settings.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage1.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.scaleInputTrackBar_)).EndInit();
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
        private System.Windows.Forms.Label ScaleInputValueLabel;
        private System.Windows.Forms.TrackBar scaleInputTrackBar_;
        private System.Windows.Forms.Label label4;
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
    }
}

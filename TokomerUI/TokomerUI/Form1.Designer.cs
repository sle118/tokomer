﻿using System.Collections.Generic;
using System.ComponentModel;

namespace CerealPotter
{
    partial class Form1
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
            CerealPotter.Properties.Settings settings4 = new CerealPotter.Properties.Settings();
            CerealPotter.Properties.Settings settings5 = new CerealPotter.Properties.Settings();
            CerealPotter.Properties.Settings settings6 = new CerealPotter.Properties.Settings();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.chkShowDataRead = new System.Windows.Forms.CheckBox();
            this.zedGraphControl1 = new ZedGraph.ZedGraphControl();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.flowLayoutPanelMainControls = new System.Windows.Forms.FlowLayoutPanel();
            this.label2 = new System.Windows.Forms.Label();
            this.comboBoxAvailableSerial = new System.Windows.Forms.ComboBox();
            this.buttonConnect = new System.Windows.Forms.Button();
            this.buttonStartPlot = new System.Windows.Forms.Button();
            this.textBoxReceive = new System.Windows.Forms.TextBox();
            this.tableLayoutPanelStates = new System.Windows.Forms.TableLayoutPanel();
            this.checkBoxPower = new System.Windows.Forms.CheckBox();
            this.checkBoxDigital = new System.Windows.Forms.CheckBox();
            this.panelScaleCombo = new System.Windows.Forms.Panel();
            this.comboBoxScale = new System.Windows.Forms.ComboBox();
            this.sensitivityValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.statusValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label6 = new System.Windows.Forms.Label();
            this.panelGraphCombo = new System.Windows.Forms.Panel();
            this.label7 = new System.Windows.Forms.Label();
            this.comboBoxGraph = new System.Windows.Forms.ComboBox();
            this.graphValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.panel1 = new System.Windows.Forms.Panel();
            this.comboBoxSerial = new System.Windows.Forms.ComboBox();
            this.serialValuesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label8 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.tableLayoutPanel2 = new System.Windows.Forms.TableLayoutPanel();
            this.label1 = new System.Windows.Forms.Label();
            this.numericUpDown1 = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.numBufferLength = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.numReadsPerSec = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.numPreThreshold = new System.Windows.Forms.NumericUpDown();
            this.label9 = new System.Windows.Forms.Label();
            this.numTriggerLag_ms = new System.Windows.Forms.NumericUpDown();
            this.label10 = new System.Windows.Forms.Label();
            this.numTriggerAvgWindowMs = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.numTriggerDeltamA = new System.Windows.Forms.NumericUpDown();
            this.checkBoxLogAxis = new System.Windows.Forms.CheckBox();
            this.splitContainer2 = new System.Windows.Forms.SplitContainer();
            this.tableGraphPanel = new System.Windows.Forms.TableLayoutPanel();
            this.flowLayoutPanel1 = new System.Windows.Forms.FlowLayoutPanel();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripProgressBar1 = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.nouveauToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.ouvrirToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonImport = new System.Windows.Forms.ToolStripButton();
            this.enregistrerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.imprimerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator = new System.Windows.Forms.ToolStripSeparator();
            this.couperToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.copierToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.collerToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.ToolStripButton = new System.Windows.Forms.ToolStripButton();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.circularProgressBar1 = new CircularProgressBar.CircularProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.rangescalesBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.label12 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.labelElementsPerSec = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.flowLayoutPanelMainControls.SuspendLayout();
            this.tableLayoutPanelStates.SuspendLayout();
            this.panelScaleCombo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityValuesBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusValuesBindingSource)).BeginInit();
            this.panelGraphCombo.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphValuesBindingSource)).BeginInit();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serialValuesBindingSource)).BeginInit();
            this.tabPage2.SuspendLayout();
            this.tableLayoutPanel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferLength)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadsPerSec)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreThreshold)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerLag_ms)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerAvgWindowMs)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerDeltamA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).BeginInit();
            this.splitContainer2.Panel1.SuspendLayout();
            this.splitContainer2.Panel2.SuspendLayout();
            this.splitContainer2.SuspendLayout();
            this.tableGraphPanel.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangescalesBindingSource)).BeginInit();
            this.panel2.SuspendLayout();
            this.SuspendLayout();
            // 
            // chkShowDataRead
            // 
            this.chkShowDataRead.AutoSize = true;
            this.chkShowDataRead.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.chkShowDataRead.Dock = System.Windows.Forms.DockStyle.Top;
            this.chkShowDataRead.Location = new System.Drawing.Point(3, 3);
            this.chkShowDataRead.Name = "chkShowDataRead";
            this.chkShowDataRead.Size = new System.Drawing.Size(348, 24);
            this.chkShowDataRead.TabIndex = 6;
            this.chkShowDataRead.Text = "Show Data Read";
            this.chkShowDataRead.UseVisualStyleBackColor = true;
            this.chkShowDataRead.CheckedChanged += new System.EventHandler(this.chkShowDataRead_CheckedChanged);
            this.chkShowDataRead.Click += new System.EventHandler(this.chkShowDataRead_Click);
            // 
            // zedGraphControl1
            // 
            this.zedGraphControl1.AutoSize = true;
            this.zedGraphControl1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.zedGraphControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zedGraphControl1.IsAntiAlias = true;
            this.zedGraphControl1.IsEnableHEdit = true;
            this.zedGraphControl1.IsEnableSelection = true;
            this.zedGraphControl1.IsEnableVEdit = true;
            this.zedGraphControl1.IsShowCursorValues = true;
            this.zedGraphControl1.IsShowHScrollBar = true;
            this.zedGraphControl1.IsShowVScrollBar = true;
            this.zedGraphControl1.IsSynchronizeXAxes = true;
            this.zedGraphControl1.IsZoomOnMouseCenter = true;
            this.zedGraphControl1.Location = new System.Drawing.Point(3, 3);
            this.zedGraphControl1.Margin = new System.Windows.Forms.Padding(0);
            this.zedGraphControl1.Name = "zedGraphControl1";
            this.zedGraphControl1.ScrollGrace = 0D;
            this.zedGraphControl1.ScrollMaxX = 0D;
            this.zedGraphControl1.ScrollMaxY = 0D;
            this.zedGraphControl1.ScrollMaxY2 = 0D;
            this.zedGraphControl1.ScrollMinX = 0D;
            this.zedGraphControl1.ScrollMinY = 0D;
            this.zedGraphControl1.ScrollMinY2 = 0D;
            this.zedGraphControl1.Size = new System.Drawing.Size(1508, 492);
            this.zedGraphControl1.TabIndex = 0;
            this.zedGraphControl1.UseExtendedPrintDialog = true;
            this.zedGraphControl1.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.zedGraphControl1_KeyPress);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(0, 33);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.tabControl1);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.splitContainer2);
            this.splitContainer1.Size = new System.Drawing.Size(1882, 656);
            this.splitContainer1.SplitterDistance = 362;
            this.splitContainer1.TabIndex = 21;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(362, 656);
            this.tabControl1.TabIndex = 16;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.flowLayoutPanelMainControls);
            this.tabPage1.Location = new System.Drawing.Point(4, 29);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(354, 623);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Execution";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // flowLayoutPanelMainControls
            // 
            this.flowLayoutPanelMainControls.Controls.Add(this.label2);
            this.flowLayoutPanelMainControls.Controls.Add(this.comboBoxAvailableSerial);
            this.flowLayoutPanelMainControls.Controls.Add(this.buttonConnect);
            this.flowLayoutPanelMainControls.Controls.Add(this.buttonStartPlot);
            this.flowLayoutPanelMainControls.Controls.Add(this.textBoxReceive);
            this.flowLayoutPanelMainControls.Controls.Add(this.tableLayoutPanelStates);
            this.flowLayoutPanelMainControls.Location = new System.Drawing.Point(3, 3);
            this.flowLayoutPanelMainControls.Name = "flowLayoutPanelMainControls";
            this.flowLayoutPanelMainControls.Size = new System.Drawing.Size(365, 650);
            this.flowLayoutPanelMainControls.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(4, 0);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(82, 20);
            this.label2.TabIndex = 5;
            this.label2.Text = "Serial Port";
            // 
            // comboBoxAvailableSerial
            // 
            this.comboBoxAvailableSerial.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.comboBoxAvailableSerial.FormattingEnabled = true;
            this.comboBoxAvailableSerial.Location = new System.Drawing.Point(94, 5);
            this.comboBoxAvailableSerial.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.comboBoxAvailableSerial.Name = "comboBoxAvailableSerial";
            this.comboBoxAvailableSerial.Size = new System.Drawing.Size(223, 28);
            this.comboBoxAvailableSerial.TabIndex = 1;
            // 
            // buttonConnect
            // 
            this.buttonConnect.AutoSize = true;
            this.buttonConnect.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonConnect.Dock = System.Windows.Forms.DockStyle.Fill;
            this.buttonConnect.Location = new System.Drawing.Point(4, 43);
            this.buttonConnect.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonConnect.Name = "buttonConnect";
            this.buttonConnect.Size = new System.Drawing.Size(79, 30);
            this.buttonConnect.TabIndex = 2;
            this.buttonConnect.Text = "Connect";
            this.buttonConnect.UseVisualStyleBackColor = true;
            this.buttonConnect.Click += new System.EventHandler(this.buttonConnect_Click);
            // 
            // buttonStartPlot
            // 
            this.buttonStartPlot.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.buttonStartPlot.AutoSize = true;
            this.buttonStartPlot.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.buttonStartPlot.Location = new System.Drawing.Point(91, 43);
            this.buttonStartPlot.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.buttonStartPlot.Name = "buttonStartPlot";
            this.buttonStartPlot.Size = new System.Drawing.Size(85, 30);
            this.buttonStartPlot.TabIndex = 5;
            this.buttonStartPlot.Text = "Start Plot";
            this.buttonStartPlot.UseVisualStyleBackColor = true;
            this.buttonStartPlot.Click += new System.EventHandler(this.buttonStartPlot_Click_1);
            // 
            // textBoxReceive
            // 
            this.textBoxReceive.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBoxReceive.Location = new System.Drawing.Point(4, 83);
            this.textBoxReceive.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.textBoxReceive.Multiline = true;
            this.textBoxReceive.Name = "textBoxReceive";
            this.textBoxReceive.Size = new System.Drawing.Size(209, 42);
            this.textBoxReceive.TabIndex = 8;
            // 
            // tableLayoutPanelStates
            // 
            this.tableLayoutPanelStates.AutoSize = true;
            this.tableLayoutPanelStates.ColumnCount = 1;
            this.tableLayoutPanelStates.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 100F));
            this.tableLayoutPanelStates.Controls.Add(this.panel2, 0, 6);
            this.tableLayoutPanelStates.Controls.Add(this.checkBoxPower, 0, 1);
            this.tableLayoutPanelStates.Controls.Add(this.checkBoxDigital, 0, 3);
            this.tableLayoutPanelStates.Controls.Add(this.panelScaleCombo, 0, 4);
            this.tableLayoutPanelStates.Controls.Add(this.panelGraphCombo, 0, 5);
            this.tableLayoutPanelStates.Controls.Add(this.panel1, 0, 0);
            this.tableLayoutPanelStates.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableLayoutPanelStates.Location = new System.Drawing.Point(3, 133);
            this.tableLayoutPanelStates.Name = "tableLayoutPanelStates";
            this.tableLayoutPanelStates.RowCount = 7;
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStates.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanelStates.Size = new System.Drawing.Size(296, 197);
            this.tableLayoutPanelStates.TabIndex = 17;
            // 
            // checkBoxPower
            // 
            this.checkBoxPower.AutoSize = true;
            this.checkBoxPower.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxPower.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxPower.Location = new System.Drawing.Point(3, 43);
            this.checkBoxPower.Name = "checkBoxPower";
            this.checkBoxPower.Size = new System.Drawing.Size(290, 24);
            this.checkBoxPower.TabIndex = 10;
            this.checkBoxPower.Tag = "poweroff,poweron";
            this.checkBoxPower.Text = "Power Enable";
            this.checkBoxPower.UseVisualStyleBackColor = true;
            // 
            // checkBoxDigital
            // 
            this.checkBoxDigital.AutoSize = true;
            this.checkBoxDigital.CheckAlign = System.Drawing.ContentAlignment.MiddleRight;
            this.checkBoxDigital.Dock = System.Windows.Forms.DockStyle.Fill;
            this.checkBoxDigital.Location = new System.Drawing.Point(3, 73);
            this.checkBoxDigital.Name = "checkBoxDigital";
            this.checkBoxDigital.Size = new System.Drawing.Size(290, 24);
            this.checkBoxDigital.TabIndex = 13;
            this.checkBoxDigital.Tag = "digitaloff,digitalon";
            this.checkBoxDigital.Text = "Digital Input Enable";
            this.checkBoxDigital.UseVisualStyleBackColor = true;
            // 
            // panelScaleCombo
            // 
            this.panelScaleCombo.AutoSize = true;
            this.panelScaleCombo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelScaleCombo.Controls.Add(this.comboBoxScale);
            this.panelScaleCombo.Controls.Add(this.label6);
            this.panelScaleCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelScaleCombo.Location = new System.Drawing.Point(3, 103);
            this.panelScaleCombo.Name = "panelScaleCombo";
            this.panelScaleCombo.Size = new System.Drawing.Size(290, 34);
            this.panelScaleCombo.TabIndex = 18;
            // 
            // comboBoxScale
            // 
            this.comboBoxScale.DataSource = this.sensitivityValuesBindingSource;
            this.comboBoxScale.DisplayMember = "Value";
            this.comboBoxScale.FormattingEnabled = true;
            this.comboBoxScale.Location = new System.Drawing.Point(166, 3);
            this.comboBoxScale.Name = "comboBoxScale";
            this.comboBoxScale.Size = new System.Drawing.Size(121, 28);
            this.comboBoxScale.TabIndex = 12;
            this.comboBoxScale.Tag = "scale0,scale1,scale2,scale3";
            this.comboBoxScale.ValueMember = "Key";
            // 
            // sensitivityValuesBindingSource
            // 
            this.sensitivityValuesBindingSource.DataMember = "SensitivityValues";
            this.sensitivityValuesBindingSource.DataSource = this.statusValuesBindingSource;
            // 
            // statusValuesBindingSource
            // 
            this.statusValuesBindingSource.DataSource = typeof(CerealPotter.StatusValues);
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(3, 6);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(79, 20);
            this.label6.TabIndex = 14;
            this.label6.Text = "Sensitivity";
            // 
            // panelGraphCombo
            // 
            this.panelGraphCombo.AutoSize = true;
            this.panelGraphCombo.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panelGraphCombo.Controls.Add(this.label7);
            this.panelGraphCombo.Controls.Add(this.comboBoxGraph);
            this.panelGraphCombo.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panelGraphCombo.Location = new System.Drawing.Point(3, 143);
            this.panelGraphCombo.Name = "panelGraphCombo";
            this.panelGraphCombo.Size = new System.Drawing.Size(290, 31);
            this.panelGraphCombo.TabIndex = 18;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(3, 3);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(54, 20);
            this.label7.TabIndex = 15;
            this.label7.Text = "Graph";
            // 
            // comboBoxGraph
            // 
            this.comboBoxGraph.DataSource = this.graphValuesBindingSource;
            this.comboBoxGraph.DisplayMember = "Value";
            this.comboBoxGraph.FormattingEnabled = true;
            this.comboBoxGraph.Location = new System.Drawing.Point(166, 0);
            this.comboBoxGraph.Name = "comboBoxGraph";
            this.comboBoxGraph.Size = new System.Drawing.Size(121, 28);
            this.comboBoxGraph.TabIndex = 15;
            this.comboBoxGraph.Tag = "graphvolt,graphcurr";
            this.comboBoxGraph.ValueMember = "Key";
            // 
            // graphValuesBindingSource
            // 
            this.graphValuesBindingSource.DataMember = "GraphValues";
            this.graphValuesBindingSource.DataSource = this.statusValuesBindingSource;
            // 
            // panel1
            // 
            this.panel1.AutoSize = true;
            this.panel1.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.panel1.Controls.Add(this.comboBoxSerial);
            this.panel1.Controls.Add(this.label8);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel1.Location = new System.Drawing.Point(3, 3);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(290, 34);
            this.panel1.TabIndex = 19;
            // 
            // comboBoxSerial
            // 
            this.comboBoxSerial.DataSource = this.serialValuesBindingSource;
            this.comboBoxSerial.DisplayMember = "Value";
            this.comboBoxSerial.FormattingEnabled = true;
            this.comboBoxSerial.Location = new System.Drawing.Point(166, 3);
            this.comboBoxSerial.Name = "comboBoxSerial";
            this.comboBoxSerial.Size = new System.Drawing.Size(121, 28);
            this.comboBoxSerial.TabIndex = 12;
            this.comboBoxSerial.Tag = "serial0,serial1,serial2";
            this.comboBoxSerial.ValueMember = "Key";
            // 
            // serialValuesBindingSource
            // 
            this.serialValuesBindingSource.DataMember = "SerialValues";
            this.serialValuesBindingSource.DataSource = this.statusValuesBindingSource;
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(3, 6);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(93, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Serial Mode";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.tableLayoutPanel2);
            this.tabPage2.Controls.Add(this.checkBoxLogAxis);
            this.tabPage2.Controls.Add(this.chkShowDataRead);
            this.tabPage2.Location = new System.Drawing.Point(4, 29);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(354, 623);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Parameters";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // tableLayoutPanel2
            // 
            this.tableLayoutPanel2.ColumnCount = 2;
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle());
            this.tableLayoutPanel2.Controls.Add(this.label1, 0, 3);
            this.tableLayoutPanel2.Controls.Add(this.numericUpDown1, 1, 2);
            this.tableLayoutPanel2.Controls.Add(this.label5, 0, 2);
            this.tableLayoutPanel2.Controls.Add(this.numBufferLength, 1, 1);
            this.tableLayoutPanel2.Controls.Add(this.label4, 0, 1);
            this.tableLayoutPanel2.Controls.Add(this.numReadsPerSec, 1, 0);
            this.tableLayoutPanel2.Controls.Add(this.label3, 0, 0);
            this.tableLayoutPanel2.Controls.Add(this.numPreThreshold, 1, 3);
            this.tableLayoutPanel2.Controls.Add(this.label9, 0, 4);
            this.tableLayoutPanel2.Controls.Add(this.numTriggerLag_ms, 1, 4);
            this.tableLayoutPanel2.Controls.Add(this.label10, 0, 5);
            this.tableLayoutPanel2.Controls.Add(this.numTriggerAvgWindowMs, 1, 5);
            this.tableLayoutPanel2.Controls.Add(this.label11, 0, 6);
            this.tableLayoutPanel2.Controls.Add(this.numTriggerDeltamA, 1, 6);
            this.tableLayoutPanel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.tableLayoutPanel2.Location = new System.Drawing.Point(3, 51);
            this.tableLayoutPanel2.Name = "tableLayoutPanel2";
            this.tableLayoutPanel2.RowCount = 8;
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle());
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 30F));
            this.tableLayoutPanel2.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20F));
            this.tableLayoutPanel2.Size = new System.Drawing.Size(348, 503);
            this.tableLayoutPanel2.TabIndex = 24;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(3, 96);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(203, 32);
            this.label1.TabIndex = 29;
            this.label1.Text = "Pre-Threshold Window (ms)";
            // 
            // numericUpDown1
            // 
            this.numericUpDown1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numericUpDown1.AutoSize = true;
            settings4.BufferLenSeconds = new decimal(new int[] {
            100,
            0,
            0,
            0});
            settings4.curve_multipliers = "0.001,0.01";
            settings4.curve_names = "mA,V";
            settings4.curves_titles = "Current,Voltage";
            settings4.DataFileInitialDirectory = "";
            settings4.PeakDetectionLag_ms = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings4.PeakDetectionSignalInfluence = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            settings4.PeakDetectionThreshold = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            settings4.Precision = "";
            settings4.PreThreshold_ms = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings4.ReadsPerSec = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            settings4.regex_pattern = "(\\w+),(\\d+(\\.\\d+)?)";
            settings4.regex_pattern_noname = "(\\d+([\\.]\\d+)?)[,](\\d+([\\.]\\d+)?)";
            settings4.serial_port = "COM5";
            settings4.SettingsKey = "";
            settings4.Threshold_mA = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            settings4.TriggerAvgWindowMs = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings4.TriggerDelta_mA = new decimal(new int[] {
            10,
            0,
            0,
            0});
            settings4.TrigThresholdPct = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings4.WindowLocation = new System.Drawing.Point(245, 258);
            settings4.WindowSize = new System.Drawing.Size(1463, 803);
            this.numericUpDown1.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings4, "Threshold_mA", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numericUpDown1.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Location = new System.Drawing.Point(212, 67);
            this.numericUpDown1.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numericUpDown1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numericUpDown1.Name = "numericUpDown1";
            this.numericUpDown1.Size = new System.Drawing.Size(133, 26);
            this.numericUpDown1.TabIndex = 28;
            this.numericUpDown1.Value = settings4.Threshold_mA;
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(3, 64);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(117, 32);
            this.label5.TabIndex = 27;
            this.label5.Text = "Threshold (mA)";
            // 
            // numBufferLength
            // 
            this.numBufferLength.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numBufferLength.AutoSize = true;
            settings5.BufferLenSeconds = new decimal(new int[] {
            100,
            0,
            0,
            0});
            settings5.curve_multipliers = "0.001,0.01";
            settings5.curve_names = "mA,V";
            settings5.curves_titles = "Current,Voltage";
            settings5.DataFileInitialDirectory = "";
            settings5.PeakDetectionLag_ms = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings5.PeakDetectionSignalInfluence = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            settings5.PeakDetectionThreshold = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            settings5.Precision = "";
            settings5.PreThreshold_ms = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings5.ReadsPerSec = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            settings5.regex_pattern = "(\\w+),(\\d+(\\.\\d+)?)";
            settings5.regex_pattern_noname = "(\\d+([\\.]\\d+)?)[,](\\d+([\\.]\\d+)?)";
            settings5.serial_port = "COM5";
            settings5.SettingsKey = "";
            settings5.Threshold_mA = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            settings5.TriggerAvgWindowMs = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings5.TriggerDelta_mA = new decimal(new int[] {
            10,
            0,
            0,
            0});
            settings5.TrigThresholdPct = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings5.WindowLocation = new System.Drawing.Point(245, 258);
            settings5.WindowSize = new System.Drawing.Size(1463, 803);
            this.numBufferLength.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings5, "BufferLenSeconds", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numBufferLength.Location = new System.Drawing.Point(212, 35);
            this.numBufferLength.Maximum = new decimal(new int[] {
            600,
            0,
            0,
            0});
            this.numBufferLength.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numBufferLength.Name = "numBufferLength";
            this.numBufferLength.Size = new System.Drawing.Size(133, 26);
            this.numBufferLength.TabIndex = 26;
            this.numBufferLength.Value = settings5.BufferLenSeconds;
            // 
            // label4
            // 
            this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(3, 32);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(117, 32);
            this.label4.TabIndex = 25;
            this.label4.Text = "Buffer len (sec)";
            // 
            // numReadsPerSec
            // 
            this.numReadsPerSec.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numReadsPerSec.AutoSize = true;
            settings6.BufferLenSeconds = new decimal(new int[] {
            100,
            0,
            0,
            0});
            settings6.curve_multipliers = "0.001,0.01";
            settings6.curve_names = "mA,V";
            settings6.curves_titles = "Current,Voltage";
            settings6.DataFileInitialDirectory = "";
            settings6.PeakDetectionLag_ms = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings6.PeakDetectionSignalInfluence = new decimal(new int[] {
            5,
            0,
            0,
            65536});
            settings6.PeakDetectionThreshold = new decimal(new int[] {
            35,
            0,
            0,
            65536});
            settings6.Precision = "";
            settings6.PreThreshold_ms = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings6.ReadsPerSec = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            settings6.regex_pattern = "(\\w+),(\\d+(\\.\\d+)?)";
            settings6.regex_pattern_noname = "(\\d+([\\.]\\d+)?)[,](\\d+([\\.]\\d+)?)";
            settings6.serial_port = "COM5";
            settings6.SettingsKey = "";
            settings6.Threshold_mA = new decimal(new int[] {
            15,
            0,
            0,
            65536});
            settings6.TriggerAvgWindowMs = new decimal(new int[] {
            50,
            0,
            0,
            0});
            settings6.TriggerDelta_mA = new decimal(new int[] {
            10,
            0,
            0,
            0});
            settings6.TrigThresholdPct = new decimal(new int[] {
            200,
            0,
            0,
            0});
            settings6.WindowLocation = new System.Drawing.Point(245, 258);
            settings6.WindowSize = new System.Drawing.Size(1463, 803);
            this.numReadsPerSec.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings6, "ReadsPerSec", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numReadsPerSec.Increment = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numReadsPerSec.Location = new System.Drawing.Point(212, 3);
            this.numReadsPerSec.Maximum = new decimal(new int[] {
            5000,
            0,
            0,
            0});
            this.numReadsPerSec.Minimum = new decimal(new int[] {
            100,
            0,
            0,
            0});
            this.numReadsPerSec.Name = "numReadsPerSec";
            this.numReadsPerSec.Size = new System.Drawing.Size(133, 26);
            this.numReadsPerSec.TabIndex = 24;
            this.numReadsPerSec.Value = settings6.ReadsPerSec;
            // 
            // label3
            // 
            this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(3, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(85, 32);
            this.label3.TabIndex = 23;
            this.label3.Text = "Reads/sec";
            // 
            // numPreThreshold
            // 
            this.numPreThreshold.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numPreThreshold.AutoSize = true;
            this.numPreThreshold.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings4, "PreThreshold_ms", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numPreThreshold.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPreThreshold.Location = new System.Drawing.Point(212, 99);
            this.numPreThreshold.Maximum = new decimal(new int[] {
            500,
            0,
            0,
            0});
            this.numPreThreshold.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numPreThreshold.Name = "numPreThreshold";
            this.numPreThreshold.Size = new System.Drawing.Size(133, 26);
            this.numPreThreshold.TabIndex = 30;
            this.numPreThreshold.Value = settings4.PreThreshold_ms;
            // 
            // label9
            // 
            this.label9.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(3, 128);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(71, 30);
            this.label9.TabIndex = 31;
            this.label9.Text = "Lag (ms)";
            // 
            // numTriggerLag_ms
            // 
            this.numTriggerLag_ms.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTriggerLag_ms.AutoSize = true;
            this.numTriggerLag_ms.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings4, "PeakDetectionLag_ms", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numTriggerLag_ms.Location = new System.Drawing.Point(212, 131);
            this.numTriggerLag_ms.Maximum = new decimal(new int[] {
            10000,
            0,
            0,
            0});
            this.numTriggerLag_ms.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTriggerLag_ms.Name = "numTriggerLag_ms";
            this.numTriggerLag_ms.Size = new System.Drawing.Size(133, 26);
            this.numTriggerLag_ms.TabIndex = 32;
            this.numTriggerLag_ms.Value = settings4.PeakDetectionLag_ms;
            // 
            // label10
            // 
            this.label10.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(3, 158);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(123, 30);
            this.label10.TabIndex = 33;
            this.label10.Text = "Signal Influence";
            // 
            // numTriggerAvgWindowMs
            // 
            this.numTriggerAvgWindowMs.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTriggerAvgWindowMs.AutoSize = true;
            this.numTriggerAvgWindowMs.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings4, "PeakDetectionSignalInfluence", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numTriggerAvgWindowMs.DecimalPlaces = 2;
            this.numTriggerAvgWindowMs.Increment = new decimal(new int[] {
            1,
            0,
            0,
            65536});
            this.numTriggerAvgWindowMs.Location = new System.Drawing.Point(212, 161);
            this.numTriggerAvgWindowMs.Maximum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.numTriggerAvgWindowMs.Name = "numTriggerAvgWindowMs";
            this.numTriggerAvgWindowMs.Size = new System.Drawing.Size(133, 26);
            this.numTriggerAvgWindowMs.TabIndex = 34;
            this.numTriggerAvgWindowMs.Value = settings4.PeakDetectionSignalInfluence;
            // 
            // label11
            // 
            this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left)));
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(3, 188);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(79, 30);
            this.label11.TabIndex = 35;
            this.label11.Text = "Threshold";
            // 
            // numTriggerDeltamA
            // 
            this.numTriggerDeltamA.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.numTriggerDeltamA.AutoSize = true;
            this.numTriggerDeltamA.DataBindings.Add(new System.Windows.Forms.Binding("Value", settings4, "PeakDetectionThreshold", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
            this.numTriggerDeltamA.DecimalPlaces = 2;
            this.numTriggerDeltamA.Increment = new decimal(new int[] {
            1,
            0,
            0,
            131072});
            this.numTriggerDeltamA.Location = new System.Drawing.Point(212, 191);
            this.numTriggerDeltamA.Maximum = new decimal(new int[] {
            50,
            0,
            0,
            0});
            this.numTriggerDeltamA.Name = "numTriggerDeltamA";
            this.numTriggerDeltamA.Size = new System.Drawing.Size(133, 26);
            this.numTriggerDeltamA.TabIndex = 36;
            this.numTriggerDeltamA.Value = settings4.PeakDetectionThreshold;
            // 
            // checkBoxLogAxis
            // 
            this.checkBoxLogAxis.AutoSize = true;
            this.checkBoxLogAxis.CheckAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.checkBoxLogAxis.Dock = System.Windows.Forms.DockStyle.Top;
            this.checkBoxLogAxis.Location = new System.Drawing.Point(3, 27);
            this.checkBoxLogAxis.Name = "checkBoxLogAxis";
            this.checkBoxLogAxis.Size = new System.Drawing.Size(348, 24);
            this.checkBoxLogAxis.TabIndex = 17;
            this.checkBoxLogAxis.Text = "Log Axis";
            this.checkBoxLogAxis.UseVisualStyleBackColor = true;
            this.checkBoxLogAxis.CheckedChanged += new System.EventHandler(this.checkBoxLogAxis_CheckedChanged);
            // 
            // splitContainer2
            // 
            this.splitContainer2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.splitContainer2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer2.Location = new System.Drawing.Point(0, 0);
            this.splitContainer2.Name = "splitContainer2";
            this.splitContainer2.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // splitContainer2.Panel1
            // 
            this.splitContainer2.Panel1.Controls.Add(this.tableGraphPanel);
            // 
            // splitContainer2.Panel2
            // 
            this.splitContainer2.Panel2.Controls.Add(this.flowLayoutPanel1);
            this.splitContainer2.Size = new System.Drawing.Size(1516, 656);
            this.splitContainer2.SplitterDistance = 500;
            this.splitContainer2.TabIndex = 0;
            // 
            // tableGraphPanel
            // 
            this.tableGraphPanel.AutoSize = true;
            this.tableGraphPanel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.tableGraphPanel.CellBorderStyle = System.Windows.Forms.TableLayoutPanelCellBorderStyle.InsetDouble;
            this.tableGraphPanel.ColumnCount = 1;
            this.tableGraphPanel.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableGraphPanel.Controls.Add(this.zedGraphControl1, 0, 0);
            this.tableGraphPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tableGraphPanel.Location = new System.Drawing.Point(0, 0);
            this.tableGraphPanel.Name = "tableGraphPanel";
            this.tableGraphPanel.RowCount = 1;
            this.tableGraphPanel.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 50F));
            this.tableGraphPanel.Size = new System.Drawing.Size(1514, 498);
            this.tableGraphPanel.TabIndex = 0;
            // 
            // flowLayoutPanel1
            // 
            this.flowLayoutPanel1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.flowLayoutPanel1.Location = new System.Drawing.Point(0, 0);
            this.flowLayoutPanel1.Name = "flowLayoutPanel1";
            this.flowLayoutPanel1.Size = new System.Drawing.Size(1514, 150);
            this.flowLayoutPanel1.TabIndex = 0;
            // 
            // statusStrip1
            // 
            this.statusStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2,
            this.toolStripProgressBar1});
            this.statusStrip1.Location = new System.Drawing.Point(0, 667);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(1882, 22);
            this.statusStrip1.TabIndex = 22;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(0, 15);
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(179, 25);
            this.toolStripStatusLabel2.Text = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Visible = false;
            // 
            // toolStripProgressBar1
            // 
            this.toolStripProgressBar1.Name = "toolStripProgressBar1";
            this.toolStripProgressBar1.Size = new System.Drawing.Size(100, 24);
            this.toolStripProgressBar1.Visible = false;
            // 
            // toolStrip1
            // 
            this.toolStrip1.ImageScalingSize = new System.Drawing.Size(24, 24);
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.nouveauToolStripButton,
            this.ouvrirToolStripButton,
            this.toolStripButtonImport,
            this.enregistrerToolStripButton,
            this.imprimerToolStripButton,
            this.toolStripSeparator,
            this.couperToolStripButton,
            this.copierToolStripButton,
            this.collerToolStripButton,
            this.toolStripSeparator1,
            this.ToolStripButton});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1882, 33);
            this.toolStrip1.TabIndex = 23;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // nouveauToolStripButton
            // 
            this.nouveauToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.nouveauToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("nouveauToolStripButton.Image")));
            this.nouveauToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.nouveauToolStripButton.Name = "nouveauToolStripButton";
            this.nouveauToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.nouveauToolStripButton.Text = "&Nouveau";
            this.nouveauToolStripButton.Click += new System.EventHandler(this.nouveauToolStripButton_Click);
            // 
            // ouvrirToolStripButton
            // 
            this.ouvrirToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ouvrirToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ouvrirToolStripButton.Image")));
            this.ouvrirToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ouvrirToolStripButton.Name = "ouvrirToolStripButton";
            this.ouvrirToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.ouvrirToolStripButton.Text = "&Ouvrir";
            this.ouvrirToolStripButton.Click += new System.EventHandler(this.ouvrirToolStripButton_Click);
            // 
            // toolStripButtonImport
            // 
            this.toolStripButtonImport.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonImport.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButtonImport.Image")));
            this.toolStripButtonImport.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonImport.Name = "toolStripButtonImport";
            this.toolStripButtonImport.Size = new System.Drawing.Size(34, 28);
            this.toolStripButtonImport.Text = "Import";
            this.toolStripButtonImport.Click += new System.EventHandler(this.toolStripButtonImport_Click);
            // 
            // enregistrerToolStripButton
            // 
            this.enregistrerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.enregistrerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("enregistrerToolStripButton.Image")));
            this.enregistrerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.enregistrerToolStripButton.Name = "enregistrerToolStripButton";
            this.enregistrerToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.enregistrerToolStripButton.Text = "&Enregistrer";
            this.enregistrerToolStripButton.Click += new System.EventHandler(this.enregistrerToolStripButton_Click);
            // 
            // imprimerToolStripButton
            // 
            this.imprimerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.imprimerToolStripButton.Enabled = false;
            this.imprimerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("imprimerToolStripButton.Image")));
            this.imprimerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.imprimerToolStripButton.Name = "imprimerToolStripButton";
            this.imprimerToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.imprimerToolStripButton.Text = "&Imprimer";
            // 
            // toolStripSeparator
            // 
            this.toolStripSeparator.Name = "toolStripSeparator";
            this.toolStripSeparator.Size = new System.Drawing.Size(6, 33);
            // 
            // couperToolStripButton
            // 
            this.couperToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.couperToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("couperToolStripButton.Image")));
            this.couperToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.couperToolStripButton.Name = "couperToolStripButton";
            this.couperToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.couperToolStripButton.Text = "C&ouper";
            this.couperToolStripButton.Click += new System.EventHandler(this.couperToolStripButton_Click);
            // 
            // copierToolStripButton
            // 
            this.copierToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.copierToolStripButton.Enabled = false;
            this.copierToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("copierToolStripButton.Image")));
            this.copierToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.copierToolStripButton.Name = "copierToolStripButton";
            this.copierToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.copierToolStripButton.Text = "Co&pier";
            // 
            // collerToolStripButton
            // 
            this.collerToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.collerToolStripButton.Enabled = false;
            this.collerToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("collerToolStripButton.Image")));
            this.collerToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.collerToolStripButton.Name = "collerToolStripButton";
            this.collerToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.collerToolStripButton.Text = "Co&ller";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 33);
            // 
            // ToolStripButton
            // 
            this.ToolStripButton.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.ToolStripButton.Enabled = false;
            this.ToolStripButton.Image = ((System.Drawing.Image)(resources.GetObject("ToolStripButton.Image")));
            this.ToolStripButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.ToolStripButton.Name = "ToolStripButton";
            this.ToolStripButton.Size = new System.Drawing.Size(34, 28);
            this.ToolStripButton.Text = "&?";
            // 
            // circularProgressBar1
            // 
            this.circularProgressBar1.Anchor = System.Windows.Forms.AnchorStyles.None;
            this.circularProgressBar1.AnimationFunction = WinFormAnimation.KnownAnimationFunctions.Liner;
            this.circularProgressBar1.AnimationSpeed = 500;
            this.circularProgressBar1.BackColor = System.Drawing.SystemColors.Window;
            this.circularProgressBar1.Font = new System.Drawing.Font("Microsoft Sans Serif", 72F, System.Drawing.FontStyle.Bold);
            this.circularProgressBar1.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.circularProgressBar1.InnerColor = System.Drawing.Color.White;
            this.circularProgressBar1.InnerMargin = 2;
            this.circularProgressBar1.InnerWidth = -1;
            this.circularProgressBar1.Location = new System.Drawing.Point(594, 0);
            this.circularProgressBar1.MarqueeAnimationSpeed = 2000;
            this.circularProgressBar1.Name = "circularProgressBar1";
            this.circularProgressBar1.OuterColor = System.Drawing.Color.Gray;
            this.circularProgressBar1.OuterMargin = -25;
            this.circularProgressBar1.OuterWidth = 26;
            this.circularProgressBar1.ProgressColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.circularProgressBar1.ProgressWidth = 25;
            this.circularProgressBar1.SecondaryFont = new System.Drawing.Font("Microsoft Sans Serif", 36F);
            this.circularProgressBar1.Size = new System.Drawing.Size(320, 320);
            this.circularProgressBar1.StartAngle = 270;
            this.circularProgressBar1.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.circularProgressBar1.SubscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SubscriptMargin = new System.Windows.Forms.Padding(10, -35, 0, 0);
            this.circularProgressBar1.SubscriptText = ".23";
            this.circularProgressBar1.SuperscriptColor = System.Drawing.Color.FromArgb(((int)(((byte)(166)))), ((int)(((byte)(166)))), ((int)(((byte)(166)))));
            this.circularProgressBar1.SuperscriptMargin = new System.Windows.Forms.Padding(10, 35, 0, 0);
            this.circularProgressBar1.SuperscriptText = "";
            this.circularProgressBar1.TabIndex = 0;
            this.circularProgressBar1.TextMargin = new System.Windows.Forms.Padding(8, 8, 0, 0);
            this.circularProgressBar1.Value = 68;
            this.circularProgressBar1.Visible = false;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            this.openFileDialog1.InitialDirectory = settings4.DataFileInitialDirectory;
            // 
            // rangescalesBindingSource
            // 
            this.rangescalesBindingSource.DataMember = "Rangescales";
            this.rangescalesBindingSource.DataSource = this.statusValuesBindingSource;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Dock = System.Windows.Forms.DockStyle.Left;
            this.label12.Location = new System.Drawing.Point(0, 0);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(105, 20);
            this.label12.TabIndex = 20;
            this.label12.Text = "Elements/sec";
            // 
            // panel2
            // 
            this.panel2.Controls.Add(this.labelElementsPerSec);
            this.panel2.Controls.Add(this.label12);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.panel2.Location = new System.Drawing.Point(3, 180);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(290, 14);
            this.panel2.TabIndex = 0;
            // 
            // labelElementsPerSec
            // 
            this.labelElementsPerSec.AutoSize = true;
            this.labelElementsPerSec.Dock = System.Windows.Forms.DockStyle.Right;
            this.labelElementsPerSec.Location = new System.Drawing.Point(290, 0);
            this.labelElementsPerSec.Name = "labelElementsPerSec";
            this.labelElementsPerSec.Size = new System.Drawing.Size(0, 20);
            this.labelElementsPerSec.TabIndex = 21;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1882, 689);
            this.Controls.Add(this.circularProgressBar1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.toolStrip1);
            this.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.Name = "Form1";
            this.Text = "Cereal Potter";
            this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.zedGraphControl1_KeyPress);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.flowLayoutPanelMainControls.ResumeLayout(false);
            this.flowLayoutPanelMainControls.PerformLayout();
            this.tableLayoutPanelStates.ResumeLayout(false);
            this.tableLayoutPanelStates.PerformLayout();
            this.panelScaleCombo.ResumeLayout(false);
            this.panelScaleCombo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.sensitivityValuesBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.statusValuesBindingSource)).EndInit();
            this.panelGraphCombo.ResumeLayout(false);
            this.panelGraphCombo.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.graphValuesBindingSource)).EndInit();
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.serialValuesBindingSource)).EndInit();
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
            this.tableLayoutPanel2.ResumeLayout(false);
            this.tableLayoutPanel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.numericUpDown1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numBufferLength)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numReadsPerSec)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numPreThreshold)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerLag_ms)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerAvgWindowMs)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.numTriggerDeltamA)).EndInit();
            this.splitContainer2.Panel1.ResumeLayout(false);
            this.splitContainer2.Panel1.PerformLayout();
            this.splitContainer2.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer2)).EndInit();
            this.splitContainer2.ResumeLayout(false);
            this.tableGraphPanel.ResumeLayout(false);
            this.tableGraphPanel.PerformLayout();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rangescalesBindingSource)).EndInit();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkShowDataRead;
        private ZedGraph.ZedGraphControl zedGraphControl1;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanel1;
        private System.Windows.Forms.TableLayoutPanel tableGraphPanel;
        private System.Windows.Forms.CheckBox checkBoxLogAxis;
        public System.Windows.Forms.SplitContainer splitContainer2;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.FlowLayoutPanel flowLayoutPanelMainControls;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox comboBoxAvailableSerial;
        private System.Windows.Forms.Button buttonConnect;
        private System.Windows.Forms.Button buttonStartPlot;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown numReadsPerSec;
        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel2;
        private System.Windows.Forms.NumericUpDown numBufferLength;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown numericUpDown1;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown numPreThreshold;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.NumericUpDown numTriggerLag_ms;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.NumericUpDown numTriggerAvgWindowMs;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.NumericUpDown numTriggerDeltamA;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.ToolStripButton nouveauToolStripButton;
        private System.Windows.Forms.ToolStripButton ouvrirToolStripButton;
        private System.Windows.Forms.ToolStripButton enregistrerToolStripButton;
        private System.Windows.Forms.ToolStripButton imprimerToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator;
        private System.Windows.Forms.ToolStripButton couperToolStripButton;
        private System.Windows.Forms.ToolStripButton copierToolStripButton;
        private System.Windows.Forms.ToolStripButton collerToolStripButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton ToolStripButton;
        private System.Windows.Forms.ToolStripButton toolStripButtonImport;
        public CircularProgressBar.CircularProgressBar circularProgressBar1;
        private System.Windows.Forms.Label label6;
        public System.Windows.Forms.CheckBox checkBoxPower;
        public System.Windows.Forms.ComboBox comboBoxScale;
        public System.Windows.Forms.CheckBox checkBoxDigital;
        private System.Windows.Forms.Panel panelScaleCombo;
        private System.Windows.Forms.Panel panelGraphCombo;
        private System.Windows.Forms.Label label7;
        public System.Windows.Forms.ComboBox comboBoxGraph;
        public System.Windows.Forms.TableLayoutPanel tableLayoutPanelStates;
        public System.Windows.Forms.TextBox textBoxReceive;
        private System.Windows.Forms.Panel panel1;
        public System.Windows.Forms.ComboBox comboBoxSerial;
        private System.Windows.Forms.Label label8;
        public System.Windows.Forms.BindingSource statusValuesBindingSource;
        private System.Windows.Forms.BindingSource sensitivityValuesBindingSource;
        private System.Windows.Forms.BindingSource graphValuesBindingSource;
        private System.Windows.Forms.BindingSource serialValuesBindingSource;
        private System.Windows.Forms.BindingSource rangescalesBindingSource;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Label label12;
        public System.Windows.Forms.Label labelElementsPerSec;
    }
}


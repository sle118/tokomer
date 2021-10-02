
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;
using System.IO.Ports;
using ZedGraph;
using CerealPotter.Properties;
using MyUtilities;
using System.IO;
using static CerealPotter.GraphMover;
using System.Threading;
using System.ComponentModel;
using ProtoBuf;
using Newtonsoft.Json;
using System.Reflection;
using DisableDevice;
using System.Security.Permissions;
using System.Security.Principal;
using System.Diagnostics;

namespace CerealPotter
{
    public partial class Form1 : Form
    {
        SerialPort _serialPort;
        GraphMover graphMover = new GraphMover();
        private Thread busyThread;
        //Dictionary<string, AveragePoints> graphLists = new Dictionary<string, AveragePoints>();
        //Dictionary<string, PointPairList> measureGraphs = new Dictionary<string, PointPairList>();
        //Dictionary<string, TriggerPoints> triggerGraphs = new Dictionary<string, TriggerPoints>();
        FormStorage storage = new FormStorage();
        bool isPlotting = false;
        GraphCutter graphCutter = new GraphCutter();
        float count = 0;
        Dictionary<string, object> lockUpdates = new Dictionary<string, object>();
        NumberFormat numberFormat = new NumberFormat();
        SerialParser parser;
        bool isSerialPortSelectChange = false;
        System.Timers.Timer updateTimer = new System.Timers.Timer(250);
        System.Diagnostics.Stopwatch elapsed = new System.Diagnostics.Stopwatch();
        //ZedGraph.MasterPane MasterPane = new MasterPane();
        public static event EventHandler MouseEnter;
        ActionPoints<AveragePoints> actionPoints;
        private bool StatusUpdateFromDevice = false;
        private delegate void UpdateTextboxDel(TextBox txtBox, string msg);
        private delegate void UpdateDel(Control lbl, string text);
        private delegate void UpdateGraphDel(CustomGraphControl control);

        public Form1()
        {
            InitializeComponent();
            GetSerialPortList();

            toolStripProgressBar1.Visible = false;
            Visible = true;
            FormClosed += Form1_FormClosed;
            _serialPort = new SerialPort();
            _serialPort.DataReceived += _serialPort_DataReceived;
            graphMover.MoveCompleted += MoveCompletedEventHandler;
            CreateGraphControls();
            updateTimer.AutoReset = true;
            updateTimer.Elapsed += OnUpdateEvent;
            parser = new SerialParser(storage.curve_multipliers, storage.curve_names);
            // Set window location
            if (Settings.Default.WindowLocation != null)
            {
                Location = Settings.Default.WindowLocation;
            }

            // Set window size
            if (Settings.Default.WindowSize != null)
            {
                Size = Settings.Default.WindowSize;
            }
            FormClosing += Form1_FormClosing;
            textBoxReceive.Visible = false;
            InitializeBackgroundWorker();
            Surrogates.Register();
            Application.DoEvents();
        }

        private void CreateGraphControls()
        {
            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGraphControl1.GraphPane.XAxis.Title.Text = "s";
            zedGraphControl1.GraphPane.YAxis.MajorGrid.Color = Color.Black;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.YAxis.MinorGrid.IsVisible = true;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = true;

            //zedGraphControl1.GraphPane.XAxis.Scale.Format = "tt";
            //zedGraphControl1.GraphPane.YAxis.Type = AxisType.Log;
            zedGraphControl1.Visible = false;
            tableGraphPanel.RowStyles[0].SizeType = SizeType.Percent;
            tableGraphPanel.RowStyles[0].Height = 0;
            foreach (string name in storage.curve_names)
            {
                tableGraphPanel.RowCount++;
                lockUpdates.Add(name, new object());
                var graphRow = tableGraphPanel.RowStyles.Add(new RowStyle(SizeType.Percent));
                tableGraphPanel.RowStyles[graphRow].Height = 100;
                CustomGraphControl newGraphPanel = new CustomGraphControl
                {
                    Name = name,
                    GraphPane = new GraphPane(zedGraphControl1.GraphPane),
                    Dock = zedGraphControl1.Dock,
                    IsSynchronizeXAxes = false,
                    Visible = true
                };
                newGraphPanel.Click += NewGraphPanel_Click;
                newGraphPanel.ZoomEvent += zedGraphControl1_ZoomEvent;
                newGraphPanel.MouseDownEvent += graphMover.MouseDownEvent;
                newGraphPanel.MouseDownEvent += graphCutter.MouseDownEvent;
                newGraphPanel.MouseUpEvent += graphMover.MouseUpEvent;
                newGraphPanel.MouseMoveEvent += graphMover.MouseMoveEvent;
                newGraphPanel.MouseMoveEvent += graphCutter.MouseMoveEvent;
                newGraphPanel.IsEnableHEdit = false;
                newGraphPanel.IsEnableVEdit = false;
                newGraphPanel.Selection.SelectionChangedEvent += graphCutter.Selection_SelectionChangedEvent;

                newGraphPanel.ContextMenuBuilder += ZedGraphControl1_ContextMenuBuilder;
                newGraphPanel.GraphPane.Title.Text = storage.curve_titles[name.CurveIndex()];
                newGraphPanel.GraphPane.Title.IsVisible = true;
                newGraphPanel.GraphPane.YAxis.Title.Text = name;
                newGraphPanel.IsEnableSelection = true;


                storage.GraphControls.Add(newGraphPanel);
                tableGraphPanel.Controls.Add(newGraphPanel, 0, tableGraphPanel.RowCount - 1);
            }

            // Now reset the hidden zedgraph control. We will use it to generate bitmaps for curve offsetting
            zedGraphControl1.GraphPane.XAxis.Type = AxisType.Linear;
            zedGraphControl1.GraphPane.YAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MajorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.YAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.XAxis.MinorGrid.IsVisible = false;
            zedGraphControl1.GraphPane.Title.IsVisible = false;
            zedGraphControl1.GraphPane.Legend.IsVisible = true;
            zedGraphControl1.Visible = false;

            for (int i = 1; i < tableGraphPanel.RowStyles.Count; i++)
            {
                tableGraphPanel.RowStyles[i].Height = 100 / (tableGraphPanel.RowStyles.Count - 1);
                tableGraphPanel.Invalidate();
            }
            tableGraphPanel.AutoScroll = true;
        }
        public void ShowBusy(bool busy = true)
        {
            if (busy)
            {
                toolStripProgressBar1.Visible = true;
                toolStripProgressBar1.Value = 0;
                toolStripProgressBar1.Minimum = 0;
                toolStripProgressBar1.Maximum = 100;
                circularProgressBar1.Location = new Point((Width - circularProgressBar1.ClientRectangle.Width) / 2 + Left, (Height - circularProgressBar1.ClientRectangle.Height) / 2 + Top - circularProgressBar1.ClientRectangle.Height / 2);
                circularProgressBar1.Visible = true;
                Application.DoEvents();
            }
            else
            {
                circularProgressBar1.Visible = false;
                toolStripProgressBar1.Visible = false;
                Application.DoEvents();
            }
        }

        private LineItem GetNearestLineItem(CustomGraphControl ctrl, MouseEventArgs mouseEventArgs, out int nearestIndex)
        {
            nearestIndex = -1;
            var obj = GetNearestObject(ctrl, mouseEventArgs, out int index);
            if (obj != null && obj.GetType() == typeof(LineItem))
            {
                // 'index' is the index of that data point
                nearestIndex = index;
                return obj as LineItem;
            }
            return null;
        }
        private object GetNearestObject(CustomGraphControl ctrl, MouseEventArgs mouseEventArgs, out int nearestIndex)
        {
            nearestIndex = -1;
            if (ctrl == null || mouseEventArgs == null || mouseEventArgs.Button != MouseButtons.Left) return null;
            object nearestObject;
            ctrl.GraphPane.FindNearestObject(new PointF(mouseEventArgs.X, mouseEventArgs.Y), CreateGraphics(), out nearestObject, out int index);
            if (nearestObject != null)
            {
                // 'index' is the index of that data point
                nearestIndex = index;
                return nearestObject;
            }
            return null;
        }
        private void NewGraphPanel_Click(object sender, EventArgs e)
        {
            CustomGraphControl ctrl = sender as CustomGraphControl;
            MouseEventArgs mouseEventArgs = e as MouseEventArgs;
            Panel_Click(ctrl, mouseEventArgs);
        }
        private void HandleLineSelection(LineItem item, int nearestIdx, CustomGraphControl ctrl)
        {
            if (item == null) return;
            if (graphCutter.HasSelection) return;
            AveragePoints pts = item.Points as AveragePoints;
            if (pts != null)
            {
                pts.ToggleSelection();
                ctrl.Invalidate();
            }
        }
        private void Panel_Click(CustomGraphControl ctrl, MouseEventArgs mouseEventArgs)
        {
            object nearestObject = GetNearestObject(ctrl, mouseEventArgs, out int nearestIdx);
            //selectedObject = nearestObject;
            if (nearestObject != null)
            {
                if (nearestObject.GetType() == typeof(Legend))
                {
                    Legend legend = nearestObject as Legend;
                    legend.Fill.Color = legend.Fill.Color == Color.White ? Color.LightBlue : Color.White;
                    ctrl.Invalidate();
                }
                else if (nearestObject.GetType() == typeof(LineItem))
                {
                    HandleLineSelection(nearestObject as LineItem, nearestIdx, ctrl);
                }

            }
        }

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Copy window location to app settings
            updateTimer.Stop();
        }

        protected void ProcessMenuActions(object sender, System.EventArgs e)
        {
            // do something here.  For example, remove all curves from the graph
            var menu = sender as System.Windows.Forms.ToolStripMenuItem;
            var parentMenu = menu.Owner as System.Windows.Forms.ContextMenuStrip;
            CustomGraphControl graph = parentMenu.SourceControl as CustomGraphControl;
            var measureLabel = graph.GraphPane.CurveList[0].Label.Text;
            if (menu.Name == "change_color")
            {
                var colorDialog1 = new System.Windows.Forms.ColorDialog();
                if (colorDialog1.ShowDialog() == DialogResult.OK)
                {
                    storage.SetCurveColor(colorDialog1.Color);

                }

                return;

            }
            if (storage.measureGraphs.ContainsKey(measureLabel))
            {
                List<GraphObj> textList = graph.GraphPane.GraphObjList.Where(ob => ob.GetType() == typeof(TextObj)).ToList();
                if (menu.Name == "clear_measures")
                {
                    foreach (GraphObj to in textList)
                    {
                        graph.GraphPane.GraphObjList.Remove(to);
                    }
                    graph.GraphPane.CurveList["measure"].Clear();
                }
                else if (menu.Name == "remove_last_measures")
                {
                    if (textList.Count == 0) return;
                    graph.GraphPane.GraphObjList.Remove(textList.Last());
                    var pts = storage.measureGraphs[graph.GraphPane.CurveList[0].Label.Text];
                    pts.RemoveAt(pts.Count - 1);
                }


                UpdateGraph();
            }


        }
        private void ZedGraphControl1_ContextMenuBuilder(ZedGraphControl sender, ContextMenuStrip menuStrip, Point mousePt, CustomGraphControl.ContextMenuObjectState objState)
        {
            ToolStripMenuItem clearMeasuresMenu = new ToolStripMenuItem
            {
                // This is the user-defined Tag so you can find this menu item later if necessary
                Name = "clear_measures",
                Tag = "clear_measures",
                // This is the text that will show up in the menu
                Text = "Clear Measures"
            };
            ToolStripMenuItem removeLastMeasureMenu = new ToolStripMenuItem
            {
                // This is the user-defined Tag so you can find this menu item later if necessary
                Name = "remove_last_measures",
                Tag = "remove_last_measures",
                // This is the text that will show up in the menu
                Text = "Remove Last Measure"
            };
            ToolStripMenuItem ChangeColor = new ToolStripMenuItem
            {
                // This is the user-defined Tag so you can find this menu item later if necessary
                Name = "change_color",
                Tag = "change_color",
                // This is the text that will show up in the menu
                Text = "Change Color"
            };
            // Add a handler that will respond when that menu item is selected
            clearMeasuresMenu.Click += new System.EventHandler(ProcessMenuActions);
            removeLastMeasureMenu.Click += new System.EventHandler(ProcessMenuActions);
            ChangeColor.Click += new System.EventHandler(ProcessMenuActions);
            ChangeColor.Visible = storage.HasSelectedGraph();
            // Add the menu item to the menu
            menuStrip.Items.Add(clearMeasuresMenu);
            menuStrip.Items.Add(removeLastMeasureMenu);
            menuStrip.Items.Add(ChangeColor);


        }
        void zedGraphControl1_ZoomEvent(ZedGraph.ZedGraphControl z, ZedGraph.ZoomState oldState, ZedGraph.ZoomState newState)
        {

            foreach (var control in tableGraphPanel.Controls)
            {
                CustomGraphControl gp = control as CustomGraphControl;
                if (gp == null) continue;
                Scale objScale = gp.GraphPane.XAxis.Scale;

                lock (lockUpdates[gp.GraphPane.YAxis.Title.Text])
                {
                    if (gp.TabIndex != z.TabIndex)
                    {
                        objScale.Min = z.GraphPane.XAxis.Scale.Min;
                        objScale.Max = z.GraphPane.XAxis.Scale.Max;
                        objScale.MaxAuto = z.GraphPane.XAxis.Scale.MaxAuto;
                        objScale.MinAuto = z.GraphPane.XAxis.Scale.MinAuto;
                        objScale.FormatAuto = z.GraphPane.XAxis.Scale.FormatAuto;
                        objScale.MagAuto = z.GraphPane.XAxis.Scale.MagAuto;
                        gp.AutoScaleMode = z.AutoScaleMode;
                        gp.AutoScroll = z.AutoScroll;
                        gp.UpdateAxis(); // Only necessary if one or more scale property is set to Auto.
                        gp.Invalidate();
                    }
                }
            }

            z.Invalidate();
        }

        private void OnUpdateEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            System.Timers.Timer timer = source as System.Timers.Timer;
            CustomGraphControl gp = tableGraphPanel.Controls[1] as CustomGraphControl;
            UpdateGraph();


        }
        private void Form1_FormClosed(object sender, FormClosedEventArgs e)
        {
            isPlotting = false;
            storage.thresholdMet = false;
            updateTimer.Stop();
            elapsed.Stop();


            Settings.Default.WindowLocation = Location;

            // Copy window size to app settings
            if (WindowState == FormWindowState.Normal)
            {
                Settings.Default.WindowSize = Size;
            }
            else
            {
                Settings.Default.WindowSize = RestoreBounds.Size;
            }

            // Save settings
            Settings.Default.Save();
        }

        private void GetSerialPortList()
        {
            string[] serialList = SerialPort.GetPortNames();
            string selectPort = Settings.Default.serial_port;
            foreach (string s in serialList)
            {
                comboBoxAvailableSerial.Items.Add(s);
                if (selectPort == s)
                    comboBoxAvailableSerial.SelectedItem = s;
            }
        }
        private void buttonConnect_Click(object sender, EventArgs e)
        {

            if (comboBoxAvailableSerial.SelectedIndex == -1) return;
            if (_serialPort.IsOpen)
            {
                try
                {
                    _serialPort.Close();
                }
                catch (Exception)
                {

                    
                }
                
                buttonConnect.Text = "Connect";
                return;
            }
            string portSelect = comboBoxAvailableSerial.SelectedItem.ToString();
            //Make application remember what comport user was using last time.
            if (portSelect != Settings.Default.serial_port)
            {
                isSerialPortSelectChange = true;
                Settings.Default.serial_port = portSelect;
            }
            _serialPort.PortName = portSelect;
            _serialPort.BaudRate = 115200;
            try
            {
                _serialPort.Open();
                buttonConnect.Text = "Disconnect";
                _serialPort.WriteLine("status");
            }
            catch (System.IO.IOException)
            {
                var t = _serialPort.GetDevice();
                if(t!="")
                { 
                    try
                    {
                        if (MessageBox.Show(this, $"Failed to open serial port {_serialPort.PortName}. Do you wish to try resetting it?", "Error", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation) == DialogResult.Yes)
                        {
                            AppDomain.CurrentDomain.SetPrincipalPolicy(PrincipalPolicy.WindowsPrincipal);
                            CheckAdministrator();
                            // we're admin! just disable and enable the port
                            DeviceHelper.SetDeviceEnabled(new Guid("{4d36e978-e325-11ce-bfc1-08002be10318}"), t, false);
                            DeviceHelper.SetDeviceEnabled(new Guid("{4d36e978-e325-11ce-bfc1-08002be10318}"), t, true);
                        }

                    }
                    catch (Exception ex)
                    {
                        // we're not admin! span the program as admin to bounce the port 
                        ProcessStartInfo info = new ProcessStartInfo();
                        info.FileName = Application.ExecutablePath;
                        info.UseShellExecute = true;
                        info.Verb = "runas"; // Provides Run as Administrator
                        info.Arguments = t;
                        var newprocess = Process.Start(info);
                        if (newprocess != null)
                        {
                            while (true)
                            {
                                try
                                {
                                    var time = newprocess.StartTime;
                                    break;
                                }
                                catch (Exception) { }
                            }
                            newprocess.WaitForExit();
                            MessageBox.Show(this, $"You can now try to connect again.", "Info", MessageBoxButtons.OK, MessageBoxIcon.Information);
                        }
                        else
                        {
                            MessageBox.Show(this, $"Reset cancelled.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                        }
                        return;
                    }


                    

                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Unable to open serial port {portSelect}. {ex.Message}");

            }
        }
            
    [PrincipalPermission(SecurityAction.Demand, Role = "Administrators")]
    static void CheckAdministrator()
    {
        Console.WriteLine("User is an administrator");
    }

private void UpdateTextBox(TextBox txtBox, string msg)
        {
            if (txtBox.InvokeRequired)
            {
                Invoke(new UpdateTextboxDel(UpdateTextBox), new object[] { txtBox, msg });
                return;
            }
            txtBox.Text = msg;
        }
        class SystemStatus
        {
            public string graph;
            public bool serial;
            public bool power;
            public int scale;
            public bool digital;
            public bool binary;
            public bool overload;
            public string[] commands;
        };

        private void _serialPort_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                while (_serialPort.BytesToRead > 0)
                {
                    string dataRead = _serialPort.ReadLine();
                    if (dataRead[0] == '{')
                    {
                        try
                        {
                            
                            SystemStatus status = JsonConvert.DeserializeObject<SystemStatus>(dataRead);
                            StatusUpdateFromDevice = true;
                            ExtensionMethods.controlList.Clear();
                            foreach (MemberInfo tag in status.GetType().GetMembers().Where(m => m.MemberType == System.Reflection.MemberTypes.Field))
                            {
                                
                                Control ctrl = Controls.FindTag(tag.Name);
                                if (ctrl == null) continue;
                                if (tag.GetUnderlyingType() == typeof(string))
                                {
                                    if (status.GetValue(tag, out string fullvalue))
                                    {
                                        SetControlTag(ctrl,tag.Name + fullvalue);
                                    }
                                    //var fullval = tag.Name+status.GetType().GetMember(tag).GetValue()
                                }
                                else if (tag.GetUnderlyingType() == typeof(bool))
                                {
                                    if (status.GetValue(tag, out bool boolvalue))
                                    {
                                        SetControlTag(ctrl, tag.Name + (boolvalue ? "on" : "off"));
                                    }
                                }
                                else if (tag.GetUnderlyingType() == typeof(int))
                                {
                                    if (status.GetValue(tag, out int intValue))
                                    {
                                        SetControlTag(ctrl, tag.Name + intValue.ToString());
                                    }
                                }

                                Console.WriteLine();
                            }
                            StatusUpdateFromDevice = false;
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show(this, $"Invalid status message {dataRead}. \r\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }

                    }
                    else
                    {
                        // data is received
                        if (chkShowDataRead.Checked)
                        {
                            UpdateTextBox(textBoxReceive, dataRead);
                        }

                        if (isPlotting)
                        {
                            Dictionary<string, double> records = parser.ParsingMatch(dataRead);
                            if (records.Count == 0) return;
                            count++;

                            UpdataSignal(records);

                        }
                    }
                }

            }
            catch (Exception ex)
            {
                string error = ex.Message;
                Console.WriteLine(error);
            }
        }

        private void UpdataSignal(Dictionary<string, double> signals, double forcedTime = -1, string prefix = "")
        {
            double timeStamp = forcedTime >= 0 ? forcedTime : elapsed.ElapsedDecimal();
            if (forcedTime >= 0)
            {
                storage.thresholdMet = true;
                isPlotting = true;
            }
            foreach (string key in signals.Keys)
            {
                storage.Add(key, prefix, timeStamp, signals[key], tableGraphPanel.Graph(key).GraphPane);
            }
        }
        private void UpdateGraph(CustomGraphControl control)
        {

            if (control.InvokeRequired)
            {
                try
                {
                    Invoke(new UpdateGraphDel(UpdateGraph), new object[] { control });
                }
                catch (System.ObjectDisposedException )
                {

                }
                
                return;
            }
            lock (lockUpdates[control.GraphPane.YAxis.Title.Text])
            {
                //                    zedGraphControl1.GraphPane. .YAxis.Type = AxisType.Log;
                for (int i = 1; i < tableGraphPanel.RowStyles.Count; i++)
                {
                    control.GraphPane.YAxis.Type = checkBoxLogAxis.Checked ? AxisType.Log : AxisType.Linear;
                }
                control.UpdateAxis();
                control.Invalidate();
            }
        }
        private void UpdateGraph()
        {
            foreach (var control in tableGraphPanel.Controls)
            {
                CustomGraphControl graphControl = control as CustomGraphControl;
                if (graphControl == null) continue;
                UpdateGraph(graphControl);
            }
        }

        private void SetControlText(Control lbl, string text)
        {
            if (lbl.InvokeRequired)
            {
                Invoke(new UpdateDel(SetControlText), new object[] { lbl, text });
                return;
            }
            lbl.Text = text;
        }

        private void chkShowDataRead_Click(object sender, EventArgs e)
        {
            if (!chkShowDataRead.Checked)
            {
                textBoxReceive.Clear();
            }

        }



        private void checkBoxLogAxis_CheckedChanged(object sender, EventArgs e)
        {
            if (!isPlotting)
            {
                UpdateGraph();
            }
        }
        private void StartRefresh()
        {
            isPlotting = true;
            updateTimer.Start();
            elapsed.Restart();
            storage.Reset();
            Text = "";
            count = 0;
            ResetScale();
            UpdateGraph();
        }

        private void StopRefresh()
        {
            isPlotting = false;
            updateTimer.Stop();
            elapsed.Stop();
            count = 0;
        }
        private void buttonStartPlot_Click_1(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                buttonConnect_Click(buttonConnect, e);
            }

            if (!isPlotting)
            {
                Reset();
                storage.thresholdMet = false;
                buttonStartPlot.Text = "Stop Plotting";
                StartRefresh();

            }
            else
            {
                StopRefresh();
                storage.thresholdMet = false;
                buttonStartPlot.Text = "Start Plotting";

            }
        }
        private void ResetScale()
        {

            foreach (var control in tableGraphPanel.Controls)
            {
                CustomGraphControl gp = control as CustomGraphControl;
                if (gp == null) continue;
                var pane = gp.GraphPane;
                var g = gp.CreateGraphics();
                pane.XAxis.ResetAutoScale(pane, g);
                pane.X2Axis.ResetAutoScale(pane, g);
                foreach (YAxis axis in pane.YAxisList)
                    axis.ResetAutoScale(pane, g);
                foreach (Y2Axis axis in pane.Y2AxisList)
                    axis.ResetAutoScale(pane, g);
                pane.AxisChange();
                gp.Invalidate();
            }
            Application.DoEvents();

        }
        private void chkShowDataRead_CheckedChanged(object sender, EventArgs e)
        {
            if (chkShowDataRead.Checked)
            {
                textBoxReceive.Visible = true;
            }
            else
            {
                textBoxReceive.Visible = false;
            }
        }



        private void buttonAnalyze_click(object sender, EventArgs e)
        {
            bool isActive = isPlotting;
            if (!isActive)
                StartRefresh();
            foreach (var trig in storage.triggerGraphs)
            {
                trig.Value.Update(storage.PointsAt(trig.Key).ToList(), true);
                UpdateGraph();
            }
            if (!isActive)
                StopRefresh();
        }

        private void zedGraphControl1_KeyPress(object sender, KeyPressEventArgs e)
        {



        }

        void SetStatusText(string text)
        {
            toolStripStatusLabel1.Text = text;
        }

        public void Reset()
        {
            storage?.Reset();
            actionPoints?.Cleanup();
            graphCutter.ClearSelection();
            ResetScale();
        }

        private void nouveauToolStripButton_Click(object sender, EventArgs e)
        {
            Reset();
        }





        private void couperToolStripButton_Click(object sender, EventArgs e)
        {
            if (graphCutter.HasSelection)
            {
                storage.CutPoints(graphCutter);
            }
        }



        void MoveCompletedEventHandler(object sender, MoveCompleteEventArgs e)
        {
            storage.OffsetCurves(e);
        }

        #region FileOperations
        private class BackgroundFileOperationsParms
        {
            public string FileName;
            public bool ImportMode;
            public BackgroundFileOperationsParms(string fileName, bool importMode)
            {
                FileName = fileName;
                ImportMode = importMode;
            }
        }
        // Set up the BackgroundWorker object by
        // attaching event handlers.
        private readonly BackgroundWorker backgroundLoadFile = new BackgroundWorker();
        private void InitializeBackgroundWorker()
        {
            backgroundLoadFile.WorkerReportsProgress = true;
            backgroundLoadFile.DoWork +=
                new DoWorkEventHandler(backgroundLoadFile_DoWork);
            backgroundLoadFile.RunWorkerCompleted +=
                new RunWorkerCompletedEventHandler(
            backLoadFile_RunWorkerCompleted);
            backgroundLoadFile.ProgressChanged +=
                new ProgressChangedEventHandler(
            backgroundLoadFile_ProgressChanged);
        }

        // This event handler is where the actual,
        // potentially time-consuming work is done.
        private void backgroundLoadFile_DoWork(object sender, DoWorkEventArgs e)
        {
            // Get the BackgroundWorker that raised this event.
            BackgroundWorker worker = sender as BackgroundWorker;

            // Assign the result of the computation
            // to the Result property of the DoWorkEventArgs
            // object. This is will be available to the
            // RunWorkerCompleted eventhandler.

            e.Result = BackgroundLoadFile(worker, e);
        }

        // This event handler deals with the results of the
        // background operation.
        private void backLoadFile_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            ShowBusy(false);
            updateTimer.Stop();
            elapsed.Stop();
            isPlotting = false;
            ResetScale();

            // First, handle the case where an exception was thrown.
            if (e.Error != null)
            {
                MessageBox.Show(this, e.Error.Message, "Load Failure", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            else if (e.Cancelled)
            {
                // Next, handle the case where the user canceled
                // the operation.
                // Note that due to a race condition in
                // the DoWork event handler, the Cancelled
                // flag may not have been set, even though
                // CancelAsync was called.
                //                resultLabel.Text = "Canceled";

            }
            else
            {
                // Finally, handle the case where the operation
                // succeeded.
                //              resultLabel.Text = e.Result.ToString();
            }

        }

        // This event handler updates the progress bar.
        private void backgroundLoadFile_ProgressChanged(object sender,
            ProgressChangedEventArgs e)
        {
            toolStripProgressBar1.Value = e.ProgressPercentage;
        }
        // This is the method that does the actual work. For this
        // example, it computes a Fibonacci number and
        // reports progress as it does its work.
        bool BackgroundLoadFile(BackgroundWorker worker, DoWorkEventArgs e)
        {
            BackgroundFileOperationsParms operationsParms = e.Argument as BackgroundFileOperationsParms;
            FormStorage local_storage = null;
            Dictionary<string, RollingPointPairList> dict = null;
            object deserialized = null;
            // Abort the operation if the user has canceled.
            // Note that a call to CancelAsync may have set
            // CancellationPending to true just after the
            // last invocation of this method exits, so this
            // code will not have the opportunity to set the
            // DoWorkEventArgs.Cancel flag to true. This means
            // that RunWorkerCompletedEventArgs.Cancelled will
            // not be set to true in your RunWorkerCompleted
            // event handler. This is a race condition.

            if (worker.CancellationPending)
            {
                e.Cancel = true;
                return false;
            }
            else
            {
                try
                {
                    try
                    {
                        using (var stream = File.Open(operationsParms.FileName, FileMode.Open))
                        {
                            local_storage = Serializer.Deserialize<FormStorage>(stream);
                        }
                    }
                    catch (Exception)
                    {


                    }

                    if (local_storage == null)
                    {
                        deserialized = FileSerializer.Deserialize<object>(operationsParms.FileName, worker);
                        if (deserialized == null)
                        {

                        }
                        local_storage = deserialized as FormStorage;
                        dict = deserialized as Dictionary<string, RollingPointPairList>;
                    }

                }

                catch (Exception ex)
                {
                    Reset();
                    throw ex;
                }
                if (local_storage == null && dict == null)
                {
                    throw new Exception("Unknown file format. Cannot load.");
                }

                if (local_storage != null)
                {
                    if (operationsParms.ImportMode)
                    {
                        foreach (var curve in local_storage.graphLists)
                        {
                            curve.Prefix = Path.GetFileNameWithoutExtension(operationsParms.FileName);
                            storage.graphLists.Add(curve);
                            curve.CreateCurve(tableGraphPanel.Graph(curve.Name).GraphPane);
                        }
                        ResetScale();
                    }
                    else
                    {
                        storage = local_storage;
                        foreach (var graph in storage.graphLists)
                        {
                            graph.CreateCurve(tableGraphPanel.Graph(graph.Name).GraphPane);
                            // when loading an existing storage class, initialize it with existing graph controls.
                            if (!storage.GraphControls.Contains(tableGraphPanel.Graph(graph.Name)))
                            {
                                storage.GraphControls.Add(tableGraphPanel.Graph(graph.Name));
                            }
                        }
                    }
                }
                else
                {
                    if (operationsParms.ImportMode)
                    {
                        foreach (var curve in dict)
                        {
                            for (int i = 0; i < curve.Value.Count; i++)
                            {
                                if (i % 5 == 0) worker.ReportProgress(50 + (i / curve.Value.Count / 2));
                                storage.Add(curve.Key, Path.GetFileNameWithoutExtension(operationsParms.FileName), curve.Value[i].X, curve.Value[i].Y, tableGraphPanel.Graph(curve.Key).GraphPane);
                            }
                        }
                    }
                    else
                    {
                        storage = new FormStorage();
                        foreach (var curve in dict)
                        {
                            for (int i = 0; i < curve.Value.Count; i++)
                            {
                                if (i % 5 == 0) worker.ReportProgress(50 + (i / curve.Value.Count / 2));
                                storage.Add(curve.Key, Path.GetFileNameWithoutExtension(operationsParms.FileName), curve.Value[i].X, curve.Value[i].Y, tableGraphPanel.Graph(curve.Key).GraphPane);
                            }
                        }
                    }
                }


            }

            return true;
        }

        private void toolStripButtonImport_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Data Files (*.dat)|*.dat|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                ShowBusy(true);
                backgroundLoadFile.RunWorkerAsync(new BackgroundFileOperationsParms(openFileDialog1.FileName, true));
            }
        }
        private void ouvrirToolStripButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.Filter = "Data Files (*.dat)|*.dat|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 2;
            openFileDialog1.RestoreDirectory = true;
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                Reset();
                //LoadFile(openFileDialog1.FileName);
                isPlotting = true;
                updateTimer.Start();
                elapsed.Start();
                ShowBusy(true);


                backgroundLoadFile.RunWorkerAsync(new BackgroundFileOperationsParms(openFileDialog1.FileName, false));
                Application.DoEvents();
            }
        }
        private void enregistrerToolStripButton_Click(object sender, EventArgs e)
        {
            saveFileDialog1.Filter = "Data Files (*.dat)|*.dat|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = true;
            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                using (var file = File.Create(saveFileDialog1.FileName))
                {
                    Serializer.Serialize(file, storage);
                }
                Text = saveFileDialog1.FileName;
                storage.SetBaseTitle(Path.GetFileNameWithoutExtension(saveFileDialog1.FileName));
            }
        }
        #endregion FileOperations

        

        private void handleCheckBoxes_CheckedChanged(object sender, EventArgs e)
        {
            if (!_serialPort.IsOpen)
            {
                MessageBox.Show(this, $"Connect first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }
            processCommandControl(sender as Control);
        }
        private void processCommandControl(Control ctrl)
        {
            string command = "";
            if (StatusUpdateFromDevice) return; // ignore the first sync to avoid needless calls to backend
            string[] commands = ctrl.Tag.ToString().Split(',');
            if (ctrl.GetType() == typeof(CheckBox))
            {
                command = (ctrl as CheckBox).Checked ?  commands[1]: commands[0];
            }
            else if (ctrl.GetType() == typeof(ComboBox))
            {
                command = commands[(ctrl as ComboBox).SelectedIndex];
            }
            if (string.IsNullOrEmpty(command)) return;
            _serialPort.WriteLine(command);

        }

        private void comboBox1_TextChanged(object sender, EventArgs e)
        {
            processCommandControl(sender as Control);
        }
        public delegate void UpdateControl(Control control, string value);
        public void SetControlTag(Control control, string value)
        {
            if (control.Tag == null) return;
            int position = control.Tag.ToString().Split(',').ToList().IndexOf(value);
            if (position < 0) return;
            if (control.InvokeRequired)
            {
                Invoke(new UpdateControl(SetControlTag), new object[] { control, value });
                return;
            }
            try
            {
                if (control.GetType() == typeof(ComboBox))
                {
                    ((ComboBox)control).SelectedIndex = position;
                    Invalidate();
                }
                else if (control.GetType() == typeof(CheckBox))
                {
                    ((CheckBox)control).Checked = position > 0;
                    Invalidate();
                }
                else
                {
                    return;
                }
            }
            catch (Exception e)
            {

                Console.WriteLine(e);
                return;
            }

            return;
        }
    }

}

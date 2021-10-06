using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO.Ports;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;

namespace CerealPotter
{

    public class SystemStatus
    {
        public class TagUpdateEventArgs
        {
            public Control Control;
            public string Tag;
            public TagUpdateEventArgs(Control ctrl, string tag)
            {
                Control = ctrl;
                Tag = tag;
            }

        }
        // Declare the delegate (if using non-generic pattern).
        public delegate void TagUpdateEventHandler(object sender,  TagUpdateEventArgs e);

        // Declare the event.
        public event TagUpdateEventHandler TagUpdated;
        public string graph;
        public bool serial;
        public bool power;
        public int scale;
        public bool digital;
        public bool binary;
        public bool overload;
        public string[] commands;
        public int[] ranges;
        public int[][] rangeScales;
        public Control.ControlCollection Controls;
        private SerialPort _serialPort;

        public SystemStatus(Control.ControlCollection controls, SerialPort serialPort)
        {
            Controls = controls;
            _serialPort = serialPort;
        }

        public bool ParseSerial(string dataRead)
        {
            if (dataRead[0] != '{')
            {
                return false;
            }
            try
            {

                SystemStatus status = JsonConvert.DeserializeObject<SystemStatus>(dataRead);
                graph = status.graph;
                serial = status.serial;
                power = status.power;
                scale = status.scale;
                digital = status.digital;
                binary = status.binary;
                overload = status.overload;
                commands = status.commands;


                
                ExtensionMethods.controlList.Clear();
                foreach (MemberInfo tag in status.GetType().GetMembers().Where(m => m.MemberType == System.Reflection.MemberTypes.Field))
                {

                    Control ctrl = Controls.FindTag(tag.Name);
                    if (ctrl == null) continue;
                    if (tag.GetUnderlyingType() == typeof(string))
                    {
                        if (status.GetValue(tag, out string fullvalue))
                        {
                            TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrl, tag.Name + fullvalue));
                        }
                        //var fullval = tag.Name+status.GetType().GetMember(tag).GetValue()
                    }
                    else if (tag.GetUnderlyingType() == typeof(bool))
                    {
                        if (status.GetValue(tag, out bool boolvalue))
                        {
                            TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrl, tag.Name + (boolvalue ? "on" : "off")));
                        }
                    }
                    else if (tag.GetUnderlyingType() == typeof(int))
                    {
                        if (status.GetValue(tag, out int intValue))
                        {
                            TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrl, tag.Name + intValue.ToString()));
                        }
                    }
                    Console.WriteLine();
                }
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, $"Invalid status message {dataRead}. \r\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public void ProcessCommandControl(Control ctrl)
        {
            string command = "";
            string[] commands = ctrl.Tag.ToString().Split(',');
            if (ctrl.GetType() == typeof(CheckBox))
            {
                command = (ctrl as CheckBox).Checked ? commands[1] : commands[0];
            }
            else if (ctrl.GetType() == typeof(ComboBox))
            {
                command = commands[(ctrl as ComboBox).SelectedIndex];
            }
            if (string.IsNullOrEmpty(command)) return;
            _serialPort.WriteLine(command);
            // todo: disable control inputs until response is recevied

        }
    };


}

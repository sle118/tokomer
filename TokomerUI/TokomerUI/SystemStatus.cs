using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        public StatusValues values { get; set; } = new StatusValues();
        // Declare the delegate (if using non-generic pattern).
        public delegate void TagUpdateEventHandler(object sender,  TagUpdateEventArgs e);

        // Declare the event.
        public event TagUpdateEventHandler TagUpdated;
       
        public Dictionary<string, Control> ControlsList;
        private SerialPort _serialPort;

        public SystemStatus(Dictionary<string, Control> controlsList, SerialPort serialPort, ISynchronizeInvoke syncInvoke )
        {
            ControlsList = controlsList; 
            _serialPort = serialPort;
            values = new StatusValues(syncInvoke);
        }

        public bool ParseSerial(string dataRead)
        {
            if (dataRead[0] != '{')
            {
                return false;
            }
            try
            {
                values.Update(dataRead);
                
                //ExtensionMethods.controlList.Clear();
                //foreach (MemberInfo tag in values.GetType().GetMembers().Where(m => m.MemberType == MemberTypes.Field))
                //{

                //    foreach (var ctrlEntry in ControlsList)
                //    {
                //        if(ctrlEntry.Key.StartsWith(tag.Name))
                //        {
                //            if (tag.GetUnderlyingType() == typeof(string))
                //            {
                //                if (values.GetValue(tag, out string fullvalue))
                //                {
                //                    TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrlEntry.Value, tag.Name + fullvalue));
                //                }
                //                //var fullval = tag.Name+status.GetType().GetMember(tag).GetValue()
                //            }
                //            else if (tag.GetUnderlyingType() == typeof(bool))
                //            {
                //                if (values.GetValue(tag, out bool boolvalue))
                //                {
                //                    TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrlEntry.Value, tag.Name + (boolvalue ? "on" : "off")));
                //                }
                //            }
                //            else if (tag.GetUnderlyingType() == typeof(int))
                //            {
                //                if (values.GetValue(tag, out int intValue))
                //                {
                //                    TagUpdated?.Invoke(this, new TagUpdateEventArgs(ctrlEntry.Value, tag.Name + intValue.ToString()));
                //                }
                //            }
                //        }
                //    }
                    
                    
                //    Console.WriteLine();
                //}
                return true;
            }
            catch (Exception ex)
            {
                MessageBox.Show(null, $"Invalid status message {dataRead}. \r\n{ex}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            return false;
        }
        public void StartDataTransfer()
        {

        }
        public bool ProcessCommandControl(Control ctrl)
        {

            if (!_serialPort.IsOpen)
            {
                MessageBox.Show(null, $"Connect first.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return false;
            }
            string command = "";
            string[] commands = ctrl.Tag.ToString().Split(',');
            if (ctrl.GetType() == typeof(CheckBox))
            {
                command = (ctrl as CheckBox).Checked ? commands[1] : commands[0];
            }
            else if (ctrl.GetType() == typeof(ComboBox))
            {
                command = (ctrl as ComboBox).SelectedValue.ToString();
            }
            if (string.IsNullOrEmpty(command)) return false;
            _serialPort.WriteLine(command);
            return true;
            // todo: disable control inputs until response is recevied

        }
    };


}

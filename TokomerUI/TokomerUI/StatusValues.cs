using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace CerealPotter
{
   [JsonObject(MemberSerialization.OptIn)]
    public class StatusValues : INotifyPropertyChanged
    {
        public class PropertyChangedExtendedEventArgs : PropertyChangedEventArgs
        {
            public bool IsDeviceUpdate = false;
            public PropertyChangedExtendedEventArgs(string propertyName, bool isDeviceUpdate)
                    : base(propertyName)
            {
                IsDeviceUpdate = isDeviceUpdate;
            }
        }
        [JsonExtensionData]
        private IDictionary<string, JToken> _additionalData = new Dictionary<string, JToken>();

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            // SAMAccountName is not deserialized to any property
            // and so it is added to the extension data dictionary
            
            Console.WriteLine(string.Join(",", _additionalData));

            
        }
        private ISynchronizeInvoke syncronzeInvoke;
        private string graph = "";
        private string serial = "";
        private bool power = false;
        private string scale = "";
        private bool digital = false;
        private bool overload = false;
        private string[] commands;
        private int[] ranges;
        private int[][] rangescales;

        public StatusValues(ISynchronizeInvoke syncInvoke = null)
        {
            syncronzeInvoke = syncInvoke;
        }
        [Bindable(true)]
        public KeyValuePair<string, string>[] GraphValues { get; } = { new KeyValuePair<string, string>("graphvolt", "Volt"), new KeyValuePair<string, string>("graphcurr", "Current") };
        [Bindable(true)]
        public KeyValuePair<string, string>[] SerialValues { get; } = { new KeyValuePair<string, string>("serial0", "Disabled"), new KeyValuePair<string, string>("serial1", "Text Mode"), new KeyValuePair<string, string>("serial2", "Binary Mode") };


        [Bindable(true)]
        public KeyValuePair<string, string>[] SensitivityValues { get; } = {
            new KeyValuePair<string, string>("scale3", "Lowest Precision"),
            new KeyValuePair<string, string>("scale2", "Low Precision"),
            new KeyValuePair<string, string>("scale1", "Medium Precision"),
            new KeyValuePair<string, string>("scale0", "High Precision")
        };

        [Bindable(true)]
        [JsonProperty("graph")]
        public string Graph
        {
            get => graph;
            set
            {
                NotifyPropertyChanged(ref graph, value);
            }
        }



        [Bindable(true)]
        [JsonProperty("serial")]
        public string Serial
        {
            get => serial;
            set
            {
                NotifyPropertyChanged(ref serial, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("power")]

        public bool Power
        {
            get => power;
            set
            {
                NotifyPropertyChanged(ref power, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("scale")]

        public string Scale
        {
            get => scale;
            set
            {
                NotifyPropertyChanged(ref scale, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("digital")]

        public bool Digital
        {
            get => digital;
            set
            {
                NotifyPropertyChanged(ref digital, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("overload")]

        public bool Overload
        {
            get => overload;

            set
            {
                NotifyPropertyChanged(ref overload, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("commands")]

        public string[] Commands
        {
            get => commands;
            set
            {
                NotifyPropertyChanged(ref commands, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("ranges")]

        public int[] Ranges
        {
            get => ranges;
            set
            {
                NotifyPropertyChanged(ref ranges, value);
            }
        }
        [Bindable(true)]
        [JsonProperty("rangescale")]

        public int[][] Rangescales
        {
            get => rangescales;
            set
            {
                NotifyPropertyChanged(ref rangescales, value);
            }
        }

        [JsonConstructor]
        public StatusValues(string graph, string serial, bool power, string scale, bool digital, bool overload, string[] commands, int[] ranges, int[][] rangescales)
        {
            DeviceUpdate = true;
            this.graph = graph;
            this.serial = serial;
            this.power = power;
            this.scale = scale;
            this.digital = digital;
            this.overload = overload;
            this.commands = commands;
            this.ranges = ranges;
            this.rangescales = rangescales;
        }

        public void UpdateFrom(StatusValues  from)
        {
            DeviceUpdate = from.DeviceUpdate;
            Graph = from.Graph;
            Serial = from.Serial;
            Power = from.Power;
            Scale = from.Scale;
            Digital = from.Digital;
            Overload = from.Overload;
            Commands = from.Commands;
            Ranges = from.Ranges;
            Rangescales= from.Rangescales;
            DeviceUpdate = false; // reset the device update indicator
        }

        public bool DeviceUpdate = false;
       
        internal void Update(string dataRead)
        {
            ITraceWriter traceWriter = new MemoryTraceWriter();
            var updatedStruct = JsonConvert.DeserializeObject<StatusValues>(dataRead, new JsonSerializerSettings { TraceWriter = traceWriter });
            UpdateFrom(updatedStruct);
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public delegate void PropertyChangedExtendedEventHandler(object sender, PropertyChangedExtendedEventArgs e);
        public event PropertyChangedExtendedEventHandler LocalPropertyChanged;

        private void NotifyPropertyChanged<T>(ref T fromVal, T toVal, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
        {
            if (fromVal.Equals(toVal) ) return;
            fromVal = toVal;
            InvokeHandler(propertyName);
        }

        private void NotifyPropertyChanged<T>(ref T[] a1, T[] a2, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
        {
            bool same = true;
            if (ReferenceEquals(a1, a2)) return;
            if (a1 == null && a2 != null) same = false;
            if (a1 != null && a2 != null)
            {
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                if (a1.Length != a2.Length) same = false;
                for (int i = 0; i < a1.Length && same; i++)
                {
                    if (!comparer.Equals(a1[i], a2[i])) same = false;
                }
            }
            if (same) return;
            a1 = a2;
            InvokeHandler(propertyName);
        }
        private void NotifyPropertyChanged<T>(ref T[][] a1, T[][] a2, [CallerMemberName] string propertyName = "") where T : IEquatable<T>
        {
            bool same = true;
            if (ReferenceEquals(a1, a2) ) return;
            if (a1 == null && a2 != null) same = false;
            if (a1 != null && a2 != null)
            {
                EqualityComparer<T> comparer = EqualityComparer<T>.Default;
                if (a1.Length != a2.Length) same = false;
                for (int i = 0; i < a1.Length && same; i++)
                {
                    if (a1[i].Length != a2[i].Length) same = false;
                    for (int j = 0; j < a1[i].Length && same; j++)
                    {
                        if (!comparer.Equals(a1[i][j], a2[i][j]))
                        {
                            same = false;
                        }
                    }
                }
            }
            if (same) return;
            a1 = a2;
            InvokeHandler(propertyName);

        }
        private void InvokeHandler(PropertyChangedEventHandler handler, PropertyChangedEventArgs parms)
        {
            if (handler != null)
            {
                if (syncronzeInvoke != null && syncronzeInvoke.InvokeRequired)
                {
                    syncronzeInvoke.Invoke(handler, new object[] { this, parms });
                }
                else
                {
                    handler(this, parms);
                }
            }
        }
        private void InvokeHandler(PropertyChangedExtendedEventHandler  handler, PropertyChangedExtendedEventArgs parms)
        {
            if (handler != null)
            {
                if (syncronzeInvoke != null && syncronzeInvoke.InvokeRequired)
                {
                    syncronzeInvoke.Invoke(handler, new object[] { this, parms });
                }
                else
                {
                    handler(this, parms);
                }
            }
        }

        private void InvokeHandler(string propertyName)
        {
            if (DeviceUpdate )
            {
                InvokeHandler(PropertyChanged, new PropertyChangedEventArgs(propertyName) );

            }
            else
            {
                InvokeHandler(LocalPropertyChanged, new PropertyChangedExtendedEventArgs(propertyName, DeviceUpdate));
                

            }
        }
    };


}

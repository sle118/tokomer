
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CerealPotter
{
    class SerialParserBinary
    {
        struct DataPacket
        {
            public byte lead;
            public UInt16 current;
            public UInt16 voltage;
            public byte[] ToArray()
            {
                var stream = new MemoryStream();
                var writer = new BinaryWriter(stream);

                writer.Write(this.lead);
                writer.Write(this.current);
                writer.Write(this.voltage);

                return stream.ToArray();
            }

            public static DataPacket FromArray(byte[] bytes)
            {
                var reader = new BinaryReader(new MemoryStream(bytes));
                
                var s = default(DataPacket);
                if (bytes.Length < 5)
                {
                    s.lead = 0;
                }
                else
                {
                    s.lead = reader.ReadByte();
                    s.current = reader.ReadUInt16();
                    s.voltage = reader.ReadUInt16();
                }
                return s;
            }
            
            public bool IsValid { get => lead>>4 == 0b1101;  }
            
        }

        public float[] Curve_multipliers { get; set; }
        public string[] Curve_names { get; set; }

        public SerialParserBinary(float[] curve_multipliers, string[] curve_names)
        {
            Curve_multipliers = curve_multipliers;
            Curve_names = curve_names;
        }   
        public Dictionary<string,double> ParsingMatch(byte[] input)
        {
            Dictionary<string, double> matchesTable = new Dictionary<string, double>();
            DataPacket packet = DataPacket.FromArray(input);

            if(packet.IsValid)
            {
                matchesTable.Add("mA", packet.current);
                matchesTable.Add("V", packet.voltage);
            }
            return matchesTable;
        }
    }
}

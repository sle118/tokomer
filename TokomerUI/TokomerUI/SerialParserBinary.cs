
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CerealPotter
{
    class SerialParserBinary
    {
        [StructLayout(LayoutKind.Explicit)]
        struct DataPacket
        {
            [FieldOffset(0)] byte lead;
            [FieldOffset(1)] UInt16 current;


        }

        public float[] Curve_multipliers { get; set; }
        public string[] Curve_names { get; set; }

        public SerialParserBinary(float[] curve_multipliers, string[] curve_names)
        {
            //Curve_multipliers = curve_multipliers;
            //Curve_names = curve_names;
            
            //Collection<string> re = new Collection<string>();
            //foreach (var varName in curve_names)
            //{
            //    re.Add($"(?<{varName}>[^,]+)");
            //}

            
        }   
        public Dictionary<string,double> ParsingMatch(string input)
        {
            Dictionary<string, double> matchesTable = new Dictionary<string, double>();
            //MatchCollection matches = reg.Matches(input);
            //foreach (Match match in matches)
            //{
            //    for (int i = 1; i < match.Groups.Count; i++)
            //    {
            //        double value = double.Parse(match.Groups[i].Value, CultureInfo.InvariantCulture) * Curve_multipliers[i-1];
            //        matchesTable.Add(match.Groups[i].Name   , value);

            //    }
            //}
            return matchesTable;
        }
    }
}

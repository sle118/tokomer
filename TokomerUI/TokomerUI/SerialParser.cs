
using CerealPotter.Properties;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CerealPotter
{
    class SerialParser
    {
        Regex reg;
        string expression;
        public float[] Curve_multipliers { get; set; }
        public string[] Curve_names { get; set; }

        public SerialParser(float[] curve_multipliers, string[] curve_names)
        {
            Curve_multipliers = curve_multipliers;
            Curve_names = curve_names;
            expression = Settings.Default.regex_pattern;
            Collection<string> re = new Collection<string>();
            foreach (var varName in curve_names)
            {
                re.Add($"(?<{varName}>[^,]+)");
            }

            reg = new Regex(string.Join("[,]{1}",re), RegexOptions.IgnoreCase);
        }   
        public Dictionary<string,double> ParsingMatch(string input)
        {
            Dictionary<string, double> matchesTable = new Dictionary<string, double>();
            MatchCollection matches = reg.Matches(input);
            foreach (Match match in matches)
            {
                for (int i = 1; i < match.Groups.Count; i++)
                {
                    double value = double.Parse(match.Groups[i].Value, CultureInfo.InvariantCulture) * Curve_multipliers[i-1];
                    matchesTable.Add(match.Groups[i].Name   , value);

                }
            }
            return matchesTable;
        }
    }
}

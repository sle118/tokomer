
using System;
using System.Collections.Generic;
using System.Linq;

namespace CerealPotter
{
    class NumberFormat
    {
        private static int MaxDecimals = 20;
        List<Interval<decimal>> intervals = new List<Interval<decimal>>();
        public NumberFormat()
        {

            intervals.Add(new Interval<decimal> { Format = "0.#", Min = 1, Max = decimal.MaxValue });
            intervals.Add(new Interval<decimal> { Format = "0", Min = 0, Max = 0});
            intervals.Add(new Interval<decimal> { Format = "0.#", Min = decimal.MinValue , Max = (decimal)-1+ ((decimal)1 / (decimal)(Math.Pow(10, MaxDecimals))) });;
            for ( int i = 1; i < MaxDecimals; i++)
            {
                decimal min = (decimal)1 / (decimal)(Math.Pow(10, i));
                decimal max = (decimal)min * (decimal)10;
                intervals.Add(new Interval<decimal>() { Format = $"0.{new string('#', i)}", Min = min, Max = max });
                intervals.Add(new Interval<decimal>() { Format = $"0.{new string('#', i)}", Min = max * -1, Max = min * -1 });

            }
            intervals.Sort(delegate (Interval<decimal> x, Interval<decimal> y)
            {
                return x.Min.CompareTo(y.Min);
            });
        }
        public string GetFormat(decimal number)
        {
            var format = intervals.Where(i => (number >= i.Min && number < i.Max)).ToList();
            if(format.Count>0)
            {
                return format.First().Format;
            }
            return "0.###";
        }
        public string ToString(decimal number)
        {
            return $"{number.ToString(GetFormat(number))}";
        }
    }

}

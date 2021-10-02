using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CerealPotter
{
    public static class MovingAverageExtensions
    {
        public static IEnumerable<decimal> CumulativeMovingAverage(this IEnumerable<decimal> source)
        {
            ulong count = 0;
            decimal sum = 0;

            foreach (var d in source)
            {
                yield return (sum += d) / ++count;
            }
        }

        public static IEnumerable<decimal> SimpleMovingAverage(this IEnumerable<decimal> source, int length)
        {
            var sample = new Queue<decimal>(length);

            foreach (var d in source)
            {
                if (sample.Count == length)
                {
                    sample.Dequeue();
                }
                sample.Enqueue(d);
                yield return sample.Average();
            }
        }

        public static IEnumerable<decimal> ExponentialMovingAverage(this IEnumerable<decimal> source, int length)
        {
            var alpha = 2 / (decimal)(length + 1);
            var s = source.ToArray();
            decimal result = 0;

            for (var i = 0; i < s.Length; i++)
            {
                result = i == 0
                    ? s[i]
                    : alpha * s[i] + (1 - alpha) * result;
                yield return result;
            }
        }
    }
}

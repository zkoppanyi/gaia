using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Exceptions;
using Gaia.Core;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// Calculate statitics
    /// </summary>
    public sealed class Statistics : Algorithm
    {

        public SortedSet<double> Numbers { get; set; }

        public Statistics(Project project, IMessanger messanger) : base(project, messanger)
        {
            Numbers = new SortedSet<double>();
        }


        /// <summary>
        /// Calculate histogram
        /// </summary>
        /// <param name="bounds">The bounds of a histogram</param>
        /// <returns></returns>
        public SortedList<double, double> CalculateHistogram(SortedSet<double> bounds)
        {
            SortedList<double, double> histgoram = new SortedList<double, double>();
            double[] numberArray = Numbers.ToArray();
            double[] boundsArray = bounds.ToArray();

            int j = 0;
            for (int i = 1; i < boundsArray.Count(); i++)
            {
                double binc = 0;
                for (; j < numberArray.Count(); j++)
                {
                    if ((boundsArray[i] > numberArray[j]) && (boundsArray[i-1] < numberArray[j]))
                    {
                        binc++;
                    }

                    if (boundsArray[i] < numberArray[j]) 
                    {
                        j = j == 0 ? 0 : j--;
                        break;
                    }
                }
                histgoram.Add((bounds.ElementAt(i) + bounds.ElementAt(i - 1)) / 2, binc);
                WriteProgress((double)i / (double)boundsArray.Count() * 100);
            }

            return histgoram;
        }

        public SortedList<double, double> CalculateHistogram()
        {
            double max = Math.Ceiling(Numbers.Max());
            double min = Math.Floor(Numbers.Min());

            if (max != min)
            {
                double step = Math.Floor((max - min) / 10);
                SortedSet<double> bounds = new SortedSet<double>();
                for (double i = min; i <= max; i += step)
                {
                    bounds.Add(i);
                }
                return CalculateHistogram(bounds);
            }
            else
            {
                WriteMessage("Error: Bounderies are same!");
                throw new GaiaException("Bounderies are same!");
            }
        }

        public double CalculateMean()
        {
            double sum = 0;
            int cnt = 0;
            foreach (double d in Numbers)
            {
                sum += d;
                WriteProgress((double)(cnt++) / (double)Numbers.Count() * 100);
            }

            return sum / Numbers.Count();
        }



    }
}

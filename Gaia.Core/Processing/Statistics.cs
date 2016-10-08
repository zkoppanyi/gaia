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
        public SortedList<double, double> Histogram { get; set; }

        public static StatisticsFactory Factory
        {
            get
            {
                return new StatisticsFactory();
            }
        }

        public class StatisticsFactory : AlgorithmFactory
        {
            public String Name { get { return "Statistic calculation (beta)"; } }
            public String Description { get { return "Statistic calculation (beta)"; } }

            public Statistics Create(Project project)
            {
                Statistics algorithm = new Statistics(project, Name, Description);
                return algorithm;
            }
        }

        private Statistics(Project project, String name, String description) : base(project, name, description)
        {
            Numbers = new SortedSet<double>();
            Histogram = new SortedList<double, double>();
        }
        
        /// <summary>
        /// Calculate histogram
        /// </summary>
        /// <returns></returns>
        public AlgorithmResult Run(SortedSet<double> bounds)
        {            
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

                //Histogram.Add((bounds.ElementAt(i) + bounds.ElementAt(i - 1)) / 2, binc); // TODO: FIX
                WriteProgress((double)i / (double)boundsArray.Count() * 100);
            }

            return AlgorithmResult.Sucess;
        }

        protected override AlgorithmResult run()
        {
            double max = Math.Ceiling(Numbers.Max());
            double min = Math.Floor(Numbers.Min());

            if (max != min)
            {
                double step = (max - min) / 100.0;
                SortedSet<double> bounds = new SortedSet<double>();
                for (double i = min; i <= max; i += step)
                {
                    bounds.Add(i);
                }
                return Run(bounds);
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

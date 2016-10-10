using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Accord.Math;
using Accord.Math.Optimization;
using Accord.Math.Convergence;
using Accord.Math.Differentiation;

namespace Gaia.Core.Processing.Optimzers
{
    public class NelderMeadOptimizer
    {
        public int MaximumIterationNumber = 100;

        public double[] Run(Func<double[], double[]> fn, double[] x)
        {
            Func<double[], double> f = unknowns => fn(unknowns).Pow(2).Sum();

            NelderMead nm = new NelderMead(x.Length, f);
            double[] xc = x.Copy();
            bool success = nm.Minimize(xc);

            if (success == true)
            {
                double minValue = nm.Value;
                double[] solution = nm.Solution;
                return solution;
            }
            else
            {
                return null;
            }


            /*Func<double[], double> f = unknowns => fn(unknowns).Pow(2).Sum();
            var df = new FiniteDifferences(x.Length, f, 1, 0.01);
            Func<double[], double[]> g = df.Compute;

            NonlinearObjectiveFunction fo = new NonlinearObjectiveFunction(x.Length, f, g);

            List<NonlinearConstraint> constraints = new List<NonlinearConstraint>();
            constraints.Add(new NonlinearConstraint(x.Length, xv => Math.Sqrt(xv.Subtract(x).Pow(2).Sum()) <= 2));

            //var nm = new Cobyla(x.Length, f);
            var nm = new BoundedBroydenFletcherGoldfarbShanno(x.Length, f, g);
            double[] xc = x.Copy();
            bool success = nm.Minimize(xc);

            if (success == true)
            {
                double minValue = nm.Value;
                double[] solution = nm.Solution;
                return solution;
            }
            else
            {
                return null;
            }*/

        }
    }
}

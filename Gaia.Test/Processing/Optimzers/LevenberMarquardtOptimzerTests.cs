using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gaia.Core.Processing.Optimzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using System.Diagnostics;

namespace Gaia.Core.Processing.Optimzers.Tests
{
    [TestClass()]
    public class LevenberMarquardtOptimzerTests
    {
        double testEpsilon = 1e-4;

        [TestMethod()]
        public void LevenberMarquardtOptimzerTestRegression2D()
        {
            double[] x = new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            double[] y = new double[] { 1.0357, 4.8491, 9.9340, 16.6787, 25.7577, 36.7431, 49.3922, 64.6555, 81.1712, 100.7060 };

            Func<double[], double[]> fn = new Func<double[], double[]>(delegate (double[] value) {
                return x.Pow(value[0]).Subtract(y);
            });
            double[] initialGuess = new double[] { 1 };

            LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
            double[] solution = optimizer.Run(fn, initialGuess);
            double[]  solutionExpected = new double[] { 2.003570638392036 };
            double[] dr = solution.Subtract(solutionExpected);
            Debug.WriteLine("Solution: " + solution);
            Debug.WriteLine("Expected: " + solution);
            Debug.WriteLine("dr: " + dr);
            Debug.WriteLine("dr norm: " + dr.Euclidean());

            Assert.IsTrue(dr.Euclidean() < testEpsilon);
        }

        [TestMethod()]
        public void LevenberMarquardtOptimzerTestRegression3D()
        {
            double[,] x = new double[,] { { 1, 2 },
                                               { 2, 3 },
                                               { 3, 4 },
                                               { 4, 5 },
                                               { 5, 6 },
                                               { 6, 7 } };

            double[] y = new double[] {   0.091360685587087*100,
                                          0.318692922076401*100,
                                          0.735797045873656*100,
                                          1.415498602018363*100,
                                          2.411449547982237*100,
                                          3.798530311177219*100 };

            Func<double[], double[]> fn = new Func<double[], double[]>(delegate (double[] value) {
                return (x.GetColumn(0).Pow(value[0]).Add(x.GetColumn(1).Pow(value[1]))).Subtract(y);
            });
            double[] initialGuess = new double[] { 1, 1 };

            LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
            optimizer.MaximumIterationNumber = 5000;
            optimizer.TolX = 1e-7;
            optimizer.TolY = 1e-7;

            double[] solution = optimizer.Run(fn, initialGuess);
            double[] solutionExpected = new double[] { 2.016903164731437, 2.999558707820020 };
            double[] dr = solution.Subtract(solutionExpected);
            Debug.WriteLine("Solution: " + solution);
            Debug.WriteLine("Expected: " + solutionExpected);
            Debug.WriteLine("dr: " + dr);
            Debug.WriteLine("dr norm: " + dr.Euclidean());

            Assert.IsTrue(dr.Euclidean() < testEpsilon);
        }
    }
}
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gaia.Core.Processing.Optimzers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MathNet.Numerics.LinearAlgebra;
using System.Diagnostics;

namespace Gaia.Core.Processing.Optimzers.Tests
{
    [TestClass()]
    public class LevenberMarquardtOptimzerTests
    {
        double testEpsilon = 1e-4;

        [TestMethod()]
        public void RunTest1()
        {
            Vector<double> x = Vector<double>.Build.DenseOfArray(new double[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 });
            Vector<double> y = Vector<double>.Build.DenseOfArray(new double[] { 1.0357, 4.8491, 9.9340, 16.6787, 25.7577, 36.7431, 49.3922, 64.6555, 81.1712, 100.7060 });

            Func<Vector<double>, Vector<double>> fn = new Func<Vector<double>, Vector<double>>(delegate (Vector<double> value) {
                return x.PointwisePower(value[0]) - y;
            });
            Vector<double> initialGuess = Vector<double>.Build.DenseOfArray(new double[] { 1 });

            LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
            Vector<double> solution = optimizer.Run(fn, initialGuess);
            Vector<double> solutionExpected = Vector<double>.Build.DenseOfArray(new double[] { 2.003570638392036 });
            Vector<double> dr = solution - solutionExpected;
            Debug.WriteLine("Solution: " + solution);
            Debug.WriteLine("Expected: " + solution);
            Debug.WriteLine("dr: " + dr);
            Debug.WriteLine("dr norm: " + dr.L2Norm());

            Assert.IsTrue(dr.L2Norm() < testEpsilon);
        }

        [TestMethod()]
        public void RunTest2()
        {
            Matrix<double> x = Matrix<double>.Build.DenseOfArray(new double[,] {  { 1, 2 },
                                                                                 { 2, 3 },
                                                                                 { 3, 4 },
                                                                                 { 4, 5 },
                                                                                 { 5, 6 },
                                                                                 { 6, 7 } });

            Vector<double> y = Vector<double>.Build.DenseOfArray(new double[] {    0.091360685587087*100,
                                                                                   0.318692922076401*100,
                                                                                   0.735797045873656*100,
                                                                                   1.415498602018363*100,
                                                                                   2.411449547982237*100,
                                                                                   3.798530311177219*100 });

            Func<Vector<double>, Vector<double>> fn = new Func<Vector<double>, Vector<double>>(delegate (Vector<double> value) {
                return (x.Column(0).PointwisePower(value[0]) + x.Column(1).PointwisePower(value[1])) - y;
            });
            Vector<double> initialGuess = Vector<double>.Build.DenseOfArray(new double[] { 1, 1 });

            LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
            Vector<double> solution = optimizer.Run(fn, initialGuess);
            Vector<double> solutionExpected = Vector<double>.Build.DenseOfArray(new double[] { 2.016903164731437, 2.999558707820020 });
            Vector<double> dr = solution - solutionExpected;
            Debug.WriteLine("Solution: " + solution);
            Debug.WriteLine("Expected: " + solutionExpected);
            Debug.WriteLine("dr: " + dr);
            Debug.WriteLine("dr norm: " + dr.L2Norm());

            Assert.IsTrue(dr.L2Norm() < testEpsilon);
        }
    }
}
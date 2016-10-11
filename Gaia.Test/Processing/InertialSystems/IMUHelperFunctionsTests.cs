using Microsoft.VisualStudio.TestTools.UnitTesting;
using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Accord.Math;
using System.Diagnostics;

namespace Gaia.Core.Processing.Tests
{
    [TestClass()]
    public class IMUHelperFunctionsTests
    {
        [TestMethod()]
        public void prh2dcmTest()
        {
            double azimuth = 0.058569976172196;
            double pitch = 0.016057418906273;
            double roll = -0.009318206085104;

            double[,] rot = IMUHelperFunctions.prh2dcm(new double[] { pitch, roll, azimuth });
            double[,] perf = new double[,] { { 0.998156572698104, -0.058683314972773, 0.015483052779751 },
                { 0.058528948713351, 0.998233171397190, 0.010241957079499 }, { -0.016056728872475, -0.009316869974122, 0.999827673847749 } };

            double diff = rot.Subtract(perf).Euclidean();

            Debug.WriteLine("Difference: " + diff);
            Assert.IsTrue(diff < 1e-14);
        }

        [TestMethod()]
        public void dcm2quatTest()
        {
            double azimuth = 0.058569976172196;
            double pitch = 0.016057418906273;
            double roll = -0.009318206085104;

            double[] prh = new double[] { pitch, roll, azimuth };
            double[,] dcm = IMUHelperFunctions.prh2dcm(prh);
            double[] quat = IMUHelperFunctions.dcm2quat(dcm);

            double[] perf = new double[] { 0.999527065409317,
                0.004892020369056, -0.007888676240926, -0.029316930912252 };

            double diff = quat.Subtract(perf).Euclidean();

            Debug.WriteLine("Difference: " + diff);
            Assert.IsTrue(diff < 1e-14);
        }
    }
}
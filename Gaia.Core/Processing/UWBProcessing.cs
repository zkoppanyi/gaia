using Gaia.Core.DataStreams;
using Gaia.Core;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// UWB trajectory calculation
    /// </summary>
    public sealed class UWBProcessing : Algorithm
    {
        public int BufferSize { get; set; }
        public int MaxIterNum { get; set; }

        private Vector<double> localTransformation = Vector<double>.Build.Dense(new double[] { 316210.749, 4254160.421, -25 });

        //public Vector<double> x0 = null;
        Vector<double> x0 = Vector<double>.Build.Dense(new double[] { 60.599796320494171, -48.144656783101361,  -1.277051451053118 });

        private const int bigNum = 100;

        public UWBProcessing(Project project, IMessanger messanger) : base(project, messanger)
        {
            BufferSize = 10;
            MaxIterNum = 10;
        }


        public AlgorithmResult CalculateTrajectory(UWBDataStream uwbDataStream, CoordinateDataStream output)
        {
            uwbDataStream.Open();
            uwbDataStream.Begin();

            output.Open();
            output.Begin();

            List<UWBDataLine> buffer = new List<UWBDataLine>();
            Dictionary<string, GPoint> pointList = new Dictionary<string, GPoint>();

            while (!uwbDataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    uwbDataStream.Close();
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                UWBDataLine dataLine = uwbDataStream.ReadLine() as UWBDataLine;
                buffer.Add(dataLine);

                /*if ((x0 == null) && (buffer.Count == 3))
                {
                    double[] sol = new double[] { 0, 0, 0 };

                    //Vector<double> x0l = Vector<double>.Build.Dense(new double[] { 0, 0, 0 });
                    //Vector<double> x0l = Vector<double>.Build.Dense(new double[] { 60.599796320494171, -48.144656783101361, -1.277051451053118 });

                    Vector<double> x0l;
                    if (x0 != null)
                    {
                        x0l = Vector<double>.Build.DenseOfVector(x0);
                    }
                    else
                    {
                        x0l = Vector<double>.Build.Dense(new double[] { 0, 0, 0 });
                    }


                    Broyden.TryFindRoot(new Func<double[], double[]>(
                            delegate (double[] val)
                            {
                                double[] ret = new double[3] { bigNum, bigNum, bigNum };
                                for (int i = 0; i < buffer.Count; i++)
                                {
                                    UWBDataLine meas = buffer[i];
                                    var targetVect = getStationPoint(meas, pointList);
                                    var valVect = Vector<double>.Build.DenseOfArray(val);
                                    double d = Math.Sqrt(Math.Pow((valVect[0] - targetVect[0]), 2) +
                                                       Math.Pow((valVect[1] - targetVect[1]), 2) +
                                                       Math.Pow((valVect[2] - targetVect[2]), 2));
                                    ret[i] = Math.Abs(d - meas.Distance);
                                }

                                return ret;
                            }),
                            x0l.ToArray(), 0.001, 100, out sol);

                    if (sol != null)
                    {
                        x0 = Vector<double>.Build.DenseOfArray(sol);
                        WriteMessage("NL: " + x0[0].ToString("F3") + " " + x0[1].ToString("F3") + " " + x0[2].ToString("F3"));
                        writer.WriteLine(x0[0].ToString("F3") + " " + x0[1].ToString("F3") + " " + x0[2].ToString("F3"));
                    }
                    else
                    {
                        buffer.RemoveAt(0);
                        buffer.Clear();
                    }

                }*/

                if ((buffer.Count >= BufferSize) && (x0 != null))
                {
                    // Design matrix
                    var A = Matrix<double>.Build.Dense(buffer.Count, 3);
                    var l = Vector<double>.Build.Dense(buffer.Count);
                    Vector<double> x0lse = Vector<double>.Build.DenseOfVector(x0);

                   int iterNum = 0;
                   Vector<double> dx = Vector<double>.Build.Dense(new double[] { bigNum, bigNum, bigNum });
                   Vector<double> v = Vector<double>.Build.Dense(new double[] { bigNum, bigNum, bigNum });
                    while ((dx.L2Norm() > 0.00001) && (iterNum <= MaxIterNum))
                    {
                        for (int i = 0; i < buffer.Count; i++)
                        {
                            UWBDataLine meas = buffer[i];
                            var targetVect = getStationPoint(meas, pointList);

                            // Build design matrix
                            double r0 = (x0 - targetVect).L2Norm();
                            A[i, 0] = (targetVect[0] - x0lse[0]) / r0;
                            A[i, 1] = (targetVect[1] - x0lse[1]) / r0;
                            A[i, 2] = (targetVect[2] - x0lse[2]) / r0;
                            l[i] = meas.Distance - r0;
                        }

                        // Solve the normal equation
                        dx = (A.Transpose() * A).Solve(A.Transpose() * l);
                        v = A * dx - l;
                        x0 = x0 - dx;

                        //Messanger.Write("x0: " + dx.ToString());                               
                        iterNum++;
                    }

                    if (dx.L2Norm() < 1)
                    {
                        x0 = x0lse;
                        WriteMessage("LSE: " + x0[0].ToString("F3") + " " + x0[1].ToString("F3") + " " + x0[2].ToString("F3"));
                        
                    }
                    else
                    {
                        //x0 = null;
                    }

                    buffer.Clear();
                }

                if (x0 != null)
                {
                    CoordinateDataLine outputDataLine = new CoordinateDataLine();
                    outputDataLine.X = x0[0];
                    outputDataLine.Y = x0[1];
                    outputDataLine.Z = x0[2];
                    output.AddDataLine(outputDataLine);
                }

                WriteProgress((double)uwbDataStream.GetPosition() / (double)uwbDataStream.DataNumber * 100.0);
            }


            WriteMessage("Done!");

            output.Close();
            uwbDataStream.Close();

            return AlgorithmResult.Sucess;
        }

        private Vector<double> getStationPoint(UWBDataLine meas, Dictionary<string, GPoint> pointList)
        {
            String targetId = Convert.ToString(meas.TargetPoint);

            // Get the point from the project
            if (!pointList.ContainsKey(targetId))
            {
                GPoint newTarget = this.project.PointManager.GetPoint(targetId);

                // target is not in the point list, skip
                if ((newTarget == null) || (newTarget.PointRole != GPointRole.Actived) || (newTarget.PointType != GPointType.Fixed))
                {
                    return null;
                }

                pointList.Add(targetId, newTarget);
            }

            // get the target point
            Vector<double> targetVect = pointList[targetId].ConvertToVector();
            targetVect = targetVect - localTransformation; // translate to the local coordinate system
            return targetVect;
        }

    }
}

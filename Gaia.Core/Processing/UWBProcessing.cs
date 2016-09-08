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
using Gaia.Core.Processing.Optimzers;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// UWB trajectory calculation
    /// </summary>
    public sealed class UWBProcessing : Algorithm
    {
        public int BufferSize { get; set; }
        public int MaxIterNum { get; set; }
        public double DeepOptimizationDistance { get; set; }
        public double TimeIntervalToClearBuffer { get; set; }
        public double FiltringByResidual { get; set; }

        //private Vector<double> localTransformation = Vector<double>.Build.Dense(new double[] { 316210.749, 4254160.421, -25 });

        //public Vector<double> x0 = null;
        Vector<double> x0 = Vector<double>.Build.Dense(new double[] { 59.0, 88.0, -1.7 });

        private const int bigNum = 100;

        public UWBProcessing(Project project, IMessanger messanger) : base(project, messanger)
        {
            BufferSize = 6;
            MaxIterNum = 200;
            DeepOptimizationDistance = 2;
            TimeIntervalToClearBuffer = 5;
            FiltringByResidual = 1;
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
                WriteProgress((double)uwbDataStream.GetPosition() / (double)uwbDataStream.DataNumber * 100);

                if (getStationPoint(dataLine, pointList) != null)
                {
                    buffer.Add(dataLine);

                    // check that the buffer's content is within a timeperiod
                    if (buffer.Count > 1)
                    {
                        double maxTime = buffer.Max(x => x.TimeStamp);
                        double minTime = buffer.Min(x => x.TimeStamp);
                        if ((maxTime - minTime) > TimeIntervalToClearBuffer)
                        {
                            WriteMessage("The time period of the ranges in the buffer is " + (maxTime - minTime) + " greater than " + TimeIntervalToClearBuffer);
                            WriteMessage("Buffer is cleared at " + dataLine.TimeStamp);
                            WriteMessage(" ");
                            buffer.Clear();
                        }

                    }
                }


                if ((buffer.Count >= BufferSize) && (x0 != null))
                {
                    Matrix<double> stations = Matrix<double>.Build.Dense(buffer.Count, 3);
                    Vector<double> distances = Vector<double>.Build.Dense(buffer.Count);
                    Vector<double> timestamps = Vector<double>.Build.Dense(buffer.Count);

                    int buffer_item_index = 0;
                    foreach (UWBDataLine line in buffer)
                    {
                        Vector<double> targetCoor = getStationPoint(line, pointList);
                        if (targetCoor != null)
                        {
                            stations.SetRow(buffer_item_index, targetCoor);
                            distances[buffer_item_index] = line.Distance;
                            timestamps[buffer_item_index] = line.TimeStamp;
                            buffer_item_index++;
                        }
                    }
                    
                    // Optimization function
                    Func<Vector<double>, Vector<double>> fn = new Func<Vector<double>, Vector<double>>(delegate (Vector<double> pos) {
                        return ((stations.Column(0) - pos[0]).PointwisePower(2) +
                        (stations.Column(1) - pos[1]).PointwisePower(2) +
                        (stations.Column(2) - pos[2]).PointwisePower(2)).PointwisePower(0.5) - distances;

                    });

                    LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
                    optimizer.MaximumIterationNumber = MaxIterNum;
                    Vector<double> x0cand = optimizer.Run(fn, x0);

                    double dsol = (x0cand - x0).L2Norm();
                    if (dsol > DeepOptimizationDistance)
                    {
                        WriteMessage("The L2 norm of the initial guess and the solution is " + dsol + " greater than " + DeepOptimizationDistance);
                        WriteMessage("Optimizer is rerun with " + optimizer.MaximumIterationNumber + " iterations! ");
                        WriteMessage(" ");

                       optimizer.MaximumIterationNumber = 5000;
                        x0cand = optimizer.Run(fn, x0);
                    }

                    double residual = fn(x0cand).Average();

                    if (Math.Abs(residual) < FiltringByResidual)
                    {
                        x0 = x0cand;
                        //WriteMessage(x0.ToString());

                        // Save the data
                        CoordinateDataLine outputDataLine = new CoordinateDataLine();
                        outputDataLine.X = x0[0];
                        outputDataLine.Y = x0[1];
                        outputDataLine.Z = x0[2];
                        outputDataLine.Sigma = residual;
                        outputDataLine.TimeStamp = timestamps.Average();
                        output.AddDataLine(outputDataLine);
                    }
                    else
                    {
                        WriteMessage("Solution's residual is " + residual + " greater than " + FiltringByResidual);
                        WriteMessage("Solution is skipped!");
                        WriteMessage(" ");
                    }

                    buffer.Clear();
                }                 

                WriteProgress((double)uwbDataStream.GetPosition() / (double)uwbDataStream.DataNumber * 100.0);
            }


            WriteMessage("Done!");

            output.Close();
            uwbDataStream.Close();

            this.Project.Save();
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
            //targetVect = targetVect - localTransformation; // translate to the local coordinate system
            return targetVect;
        }

    }
}

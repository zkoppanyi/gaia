﻿using Gaia.Core.DataStreams;
using Gaia.Core;
using Accord.Math;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.Core.Processing.Optimzers;
using Gaia.Exceptions;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// UWB trajectory calculation
    /// </summary>
    public sealed class UWBProcessing : Algorithm
    {

        public UWBDataStream SourceDataStream { get; set; }
        public CoordinateDataStream OutputDataStream { get; set; }

        public int BufferSize { get; set; }
        public int MaxIterNum { get; set; }
        public double DeepOptimizationDistance { get; set; }
        public double TimeIntervalToClearBuffer { get; set; }
        public double FiltringByResidual { get; set; }

        //private Vector<double> localTransformation = Vector<double>.Build.Dense(new double[] { 316210.749, 4254160.421, -25 });
        //public Vector<double> x0 = null;
        double[] x0 = new double[] { 59.0, 88.0, -1.7 };
        private const int bigNum = 100000;

        public static UWBProcessingFactory Factory
        {
            get
            {
                return new UWBProcessingFactory();
            }
        }

        public class UWBProcessingFactory : AlgorithmFactory
        {
            public String Name { get { return "UWB Trajectory Calculation"; } }
            public String Description { get { return "Calculate UWB trajectory from data stream."; } }

            public UWBProcessing Create(Project project, IMessanger messanger, UWBDataStream sourceDataStream, CoordinateDataStream outputDataStream)
            {
                UWBProcessing algorithm = new UWBProcessing(project, messanger, Name, Description);
                algorithm.SourceDataStream = sourceDataStream;
                algorithm.OutputDataStream = outputDataStream;
                return algorithm;
            }
        }

        private UWBProcessing(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {
            BufferSize = 6;
            MaxIterNum = 200;
            DeepOptimizationDistance = 2;
            TimeIntervalToClearBuffer = 5;
            FiltringByResidual = 1;
       }
               

        public override AlgorithmResult Run()
        {
            if (SourceDataStream == null)
            {
                new GaiaAssertException("UWB data stream is null!");
            }

            if (OutputDataStream == null)
            {
                new GaiaAssertException("Output data stream is null!");
            }

            SourceDataStream.Open();
            SourceDataStream.Begin();

            OutputDataStream.Open();
            OutputDataStream.Begin();

            List<UWBDataLine> buffer = new List<UWBDataLine>();
            Dictionary<string, GPoint> pointList = new Dictionary<string, GPoint>();

            while (!SourceDataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    SourceDataStream.Close();
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                UWBDataLine dataLine = SourceDataStream.ReadLine() as UWBDataLine;
                WriteProgress((double)SourceDataStream.GetPosition() / (double)SourceDataStream.DataNumber * 100);

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
                    double[,] stations = Matrix.Create(buffer.Count, 3, 0.0);
                    double[] distances = Vector.Create(buffer.Count, 0.0);
                    double[] timestamps = Vector.Create(buffer.Count, 0.0);

                    int buffer_item_index = 0;
                    foreach (UWBDataLine line in buffer)
                    {
                        double[] targetCoor = getStationPoint(line, pointList);
                        if (targetCoor != null)
                        {
                            stations.SetRow(buffer_item_index, targetCoor);
                            distances[buffer_item_index] = line.Distance;
                            timestamps[buffer_item_index] = line.TimeStamp;
                            buffer_item_index++;
                        }
                    }
                    
                    // Optimization function
                    Func<double[], double[]> fn = new Func<double[], double[]>(delegate (double[] pos) {
                        return (((stations.GetColumn(0).Subtract(pos[0])).Pow(2).Add((stations.GetColumn(1).Subtract(pos[1])).Pow(2)).Add((stations.GetColumn(2).Subtract(pos[2])).Pow(2)))).Sqrt().Subtract(distances);

                    });

                    LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
                    optimizer.MaximumIterationNumber = MaxIterNum;
                    double[] x0cand = optimizer.Run(fn, x0);

                    double dsol = (x0cand.Subtract(x0)).Euclidean();
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
                        OutputDataStream.AddDataLine(outputDataLine);
                    }
                    else
                    {
                        WriteMessage("Solution's residual is " + residual + " greater than " + FiltringByResidual);
                        WriteMessage("Solution is skipped!");
                        WriteMessage(" ");
                    }

                    buffer.Clear();
                }                 

                WriteProgress((double)SourceDataStream.GetPosition() / (double)SourceDataStream.DataNumber * 100.0);
            }


            WriteMessage("Done!");

            OutputDataStream.Close();
            SourceDataStream.Close();

            this.Project.Save();
            return AlgorithmResult.Sucess;
        }

        private double[] getStationPoint(UWBDataLine meas, Dictionary<string, GPoint> pointList)
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
            double[] targetVect = pointList[targetId].ConvertToVector();
            //targetVect = targetVect - localTransformation; // translate to the local coordinate system
            return targetVect;
        }

    }
}

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

        //private Vector<double> localTransformation = Vector<double>.Build.Dense(new double[] { 316210.749, 4254160.421, -25 });

        //public Vector<double> x0 = null;
        Vector<double> x0 = Vector<double>.Build.Dense(new double[] { 59.0, 88.0, -1.7 });

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
                WriteProgress((double)uwbDataStream.GetPosition() / (double)uwbDataStream.DataNumber * 100);

                if (getStationPoint(dataLine, pointList) != null)
                {
                    buffer.Add(dataLine);
                }


                if ((buffer.Count >= BufferSize) && (x0 != null))
                {
                    Matrix<double> stations = Matrix<double>.Build.Dense(buffer.Count, 3);
                    Vector<double> distances = Vector<double>.Build.Dense(buffer.Count);
                    int buffer_item_index = 0;
                    foreach (UWBDataLine line in buffer)
                    {
                        Vector<double> targetCoor = getStationPoint(line, pointList);
                        if (targetCoor != null)
                        {
                            stations.SetRow(buffer_item_index, targetCoor);
                            distances[buffer_item_index] = line.Distance;
                            buffer_item_index++;
                        }
                    }
                    
                    Func<Vector<double>, Vector<double>> fn = new Func<Vector<double>, Vector<double>>(delegate (Vector<double> pos) {
                        return ((stations.Column(0) - pos[0]).PointwisePower(2) +
                        (stations.Column(1) - pos[1]).PointwisePower(2) +
                        (stations.Column(2) - pos[2]).PointwisePower(2)).PointwisePower(0.5) - distances;


                    });

                    LevenberMarquardtOptimzer optimizer = new LevenberMarquardtOptimzer();
                    Vector<double> x0cand = optimizer.Run(fn, x0);

                    if ((x0cand - x0).L2Norm() > 2)
                    {
                        optimizer.MaximumIterationNumber = 5000;
                        x0cand = optimizer.Run(fn, x0);
                    }
                    x0 = x0cand;

                    double residual = fn(x0cand).L2Norm();

                    // Save the data
                    CoordinateDataLine outputDataLine = new CoordinateDataLine();
                    outputDataLine.X = x0[0];
                    outputDataLine.Y = x0[1];
                    outputDataLine.Z = x0[2];
                    outputDataLine.Sigma = residual;
                    output.AddDataLine(outputDataLine);

                    WriteMessage(x0.ToString());
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

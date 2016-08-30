using MathNet.Numerics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Exceptions;
using Gaia.Core.Processing;

namespace Gaia.Core.DataStreams
{
    public enum GPSLogClockErrorModel
    {
        Linear,
        Interpolation
    }

    [Serializable]
    public sealed class GPSLogDataStream : DataStream, IClockErrorModel
    {
        public GPSLogClockErrorModel Model { get; set; }
        private long firstHPC;
        private long lastHPC;

        private GPSLogDataStream(Project project, String fileId) : base(project, fileId)
        {
            this.Model = GPSLogClockErrorModel.Linear;
        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new GPSLogDataStream(project, fileId);
            return stream;
        }

        public override DataLine CreateDataLine()
        {
            return new GPSLogDataLine();
        }
        
        public void CorrectTimestamp(DataStream dataStream)
        {
            double f = 2.628413233862434e+06; //Hz
            this.Model = GPSLogClockErrorModel.Interpolation;

            if (this.Model == GPSLogClockErrorModel.Interpolation)
            {
                this.WriteMessage("Model: Interpolation");
                this.WriteMessage("f: " + f + " Hz");

                if (!this.isTimestampOrdered)
                {
                    throw new GaiaException("GPS Log file timestamps are not increasing!");
                }

                if (!this.isTimestampOrdered)
                {
                    throw new GaiaException("DataStream file timestamps are not increasing!");
                }

                dataStream.Open();

                while (!dataStream.IsEOF())
                {
                    long posStream = dataStream.GetPosition();
                    DataLine lineStream = dataStream.ReadLine();

                    if(lineStream.TimeStamp < this.firstHPC / f)
                    {
                        throw new GaiaException("The timestamps in the datastream is smaller than in the smallest log in GPS Log file");
                    }

                    if (lineStream.TimeStamp > this.lastHPC / f)
                    {
                        throw new GaiaException("The timestamps in the datastream is larger than the largest log in ths GPS Log file");
                    }

                    // Look for the corresponding HPC
                    //this.Begin();
                    while (!this.IsEOF())
                    {
                        long pos = this.GetPosition();
                        GPSLogDataLine line = (GPSLogDataLine)this.ReadLine();
                        if (line.HPCTime /f >= lineStream.TimeStamp)
                        {
                            this.Seek(pos - 1);
                            break;
                        }
                    }

                    //
                    long posLog = this.GetPosition();
                    GPSLogDataLine lineLog = (GPSLogDataLine)this.ReadLine();

                    if (lineLog.HPCTime/f == lineStream.TimeStamp)
                    {
                        lineStream.TimeStamp = lineLog.GPSTime;
                    }
                    else
                    {
                        this.Seek(posLog-1);
                        GPSLogDataLine lineLog2 = (GPSLogDataLine)this.ReadLine();
                        double h1 = lineLog2.HPCTime / f;
                        double t1 = lineLog2.GPSTime;
                        double h2 = lineLog.HPCTime / f;
                        double t2 = (double)lineLog.GPSTime;
                        double ts = (double)lineStream.TimeStamp;
                        lineStream.TimeStamp = (t2 - t1) / (h2 - h1) * (ts - h1) + t1;
                    }

                    this.WriteProgress((double)fileStream.Position/ (double)fileStream.Length*100.0);
                    dataStream.ReplaceDataLine(lineStream, posStream);
                }

                dataStream.Close();
                dataStream.TRS = this.TRS;

            }
            else if (this.Model == GPSLogClockErrorModel.Linear)
            {
                this.WriteMessage("Model: Linear");
                this.WriteMessage("f: " + f + " Hz");

                // Collect GPSLog points into a double[]
                long lineNum = 0;
                double[] xdata = new double[this.DataNumber];
                double[] ydata = new double[this.DataNumber];
                while (lineNum < this.DataNumber)
                {
                    GPSLogDataLine data = (GPSLogDataLine)this.ReadLine();
                    xdata[lineNum] = data.GPSTime;
                    ydata[lineNum] = (double)data.HPCTime / f;
                    this.WriteProgress((double)lineNum / (double)this.DataNumber * 100/2);
                    lineNum++;
                }

                // Estimate parameters
                Tuple<double, double> p = Fit.Line(ydata, xdata);
                double a = p.Item1; // intercept
                double b = p.Item2; // slope
                double r2 = GoodnessOfFit.RSquared(xdata.Select(x => a + b * x), ydata);
                this.WriteMessage("Calculated Model: ");
                this.WriteMessage(b + "x + " + a);
                this.WriteMessage("Godness of fit (R-Squared): " + r2);

                dataStream.Open();
                dataStream.Begin();

                // Calculate the new timestamps and write back the changes
                lineNum = 0;
                while (!dataStream.IsEOF())
                {
                    long pos = dataStream.GetPosition();
                    DataLine line = dataStream.ReadLine();
                    line.TimeStamp = line.TimeStamp * b + a;
                    dataStream.ReplaceDataLine(line, pos);
                    this.WriteProgress(50 + (double)lineNum / (double)dataStream.DataNumber * 100/2);
                    lineNum++;
                }

                dataStream.Close();
                dataStream.TRS = this.TRS;
            }

            this.Begin();

        }

        public override void AddDataLine(DataLine dataLine) 
        {
            base.AddDataLine(dataLine);
            GPSLogDataLine gLine = dataLine as GPSLogDataLine;
            checkHPCTime(gLine);
        }

        public override void ReplaceDataLine(DataLine dataLine, long position)
        {
            base.ReplaceDataLine(dataLine, position);
            GPSLogDataLine gLine = dataLine as GPSLogDataLine;
            checkHPCTime(gLine);
        }

        private void checkHPCTime(GPSLogDataLine gLine)
        {
            if (this.DataNumber == 1)
            {
                this.firstHPC = gLine.HPCTime;
                this.lastHPC = gLine.HPCTime;
            }

            if (gLine.HPCTime < this.firstHPC)
            {
                this.firstHPC = gLine.HPCTime;
            }

            if (gLine.HPCTime > this.lastHPC)
            {
                this.lastHPC = gLine.HPCTime;
            }
        }

        protected override string extension { get { return "glog"; } }

    }
}

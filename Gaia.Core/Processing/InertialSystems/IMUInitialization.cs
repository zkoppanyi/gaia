using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    public class IMUInitialization : Algorithm
    {
        public IMUDataStream SourceDataStream;
        public CoordinateDataStream CoordinateDataStream;

        private double timeMatchingDifference;
        private double initilaizationTime;
        private double detectFirstMovemnetThreshold;

        public static IMUInitializationFactory Factory { get { return new IMUInitializationFactory(); } }

        public class IMUInitializationFactory : AlgorithmFactory
        {
            public String Name { get { return "Evaulate an expression in data streams"; } }
            public String Description { get { return "Evaulate an expression on data lines in stream."; } }

            [System.ComponentModel.DisplayName("Minimum time matching difference [s]")]
            public double TimeMatchingDifference { get; set; }

            [System.ComponentModel.DisplayName("Time for calculating initialization [s]")]
            public double InitilaizationTime { get; set; }

            [System.ComponentModel.DisplayName("Threshold to detect first movement [m]")]
            public double DetectFirstMovemnetThreshold { get; set; }

            public IMUInitializationFactory()
            {
                TimeMatchingDifference = 1;
                InitilaizationTime = 120;
                DetectFirstMovemnetThreshold = 0.01;
            }

            public IMUInitialization Create(Project project, IMessanger messanger, IMUDataStream sourceDataStream, CoordinateDataStream coordinateDataStream)
            {
                IMUInitialization algorithm = new IMUInitialization(project, messanger, this.Name, this.Description, TimeMatchingDifference, InitilaizationTime, DetectFirstMovemnetThreshold);
                algorithm.SourceDataStream = sourceDataStream;
                algorithm.CoordinateDataStream = coordinateDataStream;
                return algorithm;
            }
        }

        private IMUInitialization(Project project, IMessanger messanger, String name, String description,
                                    double timeMatchingDifference, double initilaizationTime, double detectFirstMovemnetThreshold) : base(project, messanger, name, description)
        {
            this.timeMatchingDifference = timeMatchingDifference;
            this.initilaizationTime = initilaizationTime;
            this.detectFirstMovemnetThreshold = detectFirstMovemnetThreshold;
        }
       

        /// <summary>
        /// Calculate initilaization parameters for IMU using coordinate stream
        /// </summary>
        /// <returns>Result object</returns>
        public override AlgorithmResult Run()
        {
            if (SourceDataStream == null)
            {
                new GaiaAssertException("IMU data stream is null!");
            }

            if (CoordinateDataStream == null)
            {
                new GaiaAssertException("Coordinate data stream is null!");
            }

            if (!SourceDataStream.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + CoordinateDataStream.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!CoordinateDataStream.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + CoordinateDataStream.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!(CoordinateDataStream.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
            {
                WriteMessage("The " + CoordinateDataStream.Name + " data stream's coordinate system is not 3D! Choose a geographic coordinate system");
                return AlgorithmResult.Failure;
            }

            if (CoordinateDataStream.CRS.WKT != SourceDataStream.CRS.WKT)
            {
                WriteMessage("The two data stream has to be in the same CRS!");
                return AlgorithmResult.Failure;
            }

            SourceDataStream.Open();
            CoordinateDataStream.Open();

            SourceDataStream.Begin();
            CoordinateDataStream.Begin();

            // Start the first timestamps in both dataset
            IMUDataLine imuLine = SourceDataStream.ReadLine() as IMUDataLine;
            CoordinateDataLine coorLine = CoordinateDataStream.ReadLine() as CoordinateDataLine;
            if (imuLine.TimeStamp > coorLine.TimeStamp)
            {
                while ((coorLine.TimeStamp <= imuLine.TimeStamp) && !CoordinateDataStream.IsEOF())
                {
                    coorLine = CoordinateDataStream.ReadLine() as CoordinateDataLine;
                }
            }
            else if (imuLine.TimeStamp < coorLine.TimeStamp)
            {
                while ((imuLine.TimeStamp <= coorLine.TimeStamp) && !CoordinateDataStream.IsEOF())
                {
                    imuLine = SourceDataStream.ReadLine() as IMUDataLine;
                }
            }

            if (Math.Abs(imuLine.TimeStamp - coorLine.TimeStamp) > this.timeMatchingDifference)
            {
                String msg = "Minimum time difference between starting IMU and coordinate stream is higher than the threshold: " + this.timeMatchingDifference;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            // Calculate initial accelerations and roll and pitch
            double mean_ax = 0, mean_ay = 0, mean_az = 0;
            long data_num = 0;
            double initEnd = imuLine.TimeStamp + initilaizationTime;
            while ((imuLine.TimeStamp <= initEnd) && !CoordinateDataStream.IsEOF())
            {
                imuLine = SourceDataStream.ReadLine() as IMUDataLine;
                mean_ax += imuLine.Ax;
                mean_ay += imuLine.Ay;
                mean_az += imuLine.Az;
                data_num++;
            }

            if ((Math.Abs(imuLine.TimeStamp - initEnd) > this.timeMatchingDifference) || CoordinateDataStream.IsEOF())
            {
                String msg = "Minimum time difference between the last IMU and the end of the initilization periods is higher than the threshold: " + this.timeMatchingDifference;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            mean_ax /= data_num;
            mean_ay /= data_num;
            mean_az /= data_num;

            double roll = Math.Atan2(mean_ay, mean_az) - Math.PI;
            double r = Math.Sqrt(mean_ax * mean_ax + mean_ay * mean_ay + mean_az * mean_az);
            double pitch = Math.Asin(mean_ax / r);

            // Find first movement
            GPoint lastPoint = CoordinateDataStream.ReadDataLineAsGPoint();
            GPoint currPoint = lastPoint;
            GPoint nextPoint = lastPoint;
            bool isFound = false;
            while ((coorLine.TimeStamp <= initEnd) && !CoordinateDataStream.IsEOF())
            {
                currPoint = CoordinateDataStream.ReadDataLineAsGPoint();
                double dr = Utilities.L2Distance2d(lastPoint, currPoint);
                if (dr > this.detectFirstMovemnetThreshold)
                {
                    isFound = true;
                    nextPoint = currPoint;
                    currPoint = lastPoint;
                    break;
                }
                lastPoint = currPoint;
            }

            if (!isFound)
            {
                String msg = "First movement in " + CoordinateDataStream.Name + " stream is not found! Threshold: " + this.detectFirstMovemnetThreshold;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            WriteMessage("First movement detected!");
            WriteMessage("Index: " + CoordinateDataStream.GetPosition());
            WriteMessage("Timestamp: " + currPoint.Timestamp);
            WriteProgress(50);

            SourceDataStream.Close();
            CoordinateDataStream.Close();

            GeographicCoordinateSystem crs = CoordinateDataStream.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;
            double a = crs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
            double b = crs.HorizontalDatum.Ellipsoid.SemiMinorAxis;
            double e2 = 1 - (b * b) / (a * a);

            double lambda = (1 - e2) * (Math.Tan(nextPoint.LatRad) / Math.Tan(currPoint.LatRad)) + e2 * Math.Sqrt(((1 + ((1 - e2) * Math.Pow((Math.Tan(nextPoint.LatRad)), 2)))) / ((1 + ((1 - e2) * Math.Pow((Math.Tan(currPoint.LatRad)), 2)))));
            double azimuth = Math.Atan2(Math.Sin(nextPoint.LonRad - currPoint.LonRad), (lambda - Math.Cos(nextPoint.LonRad - currPoint.LonRad)) * Math.Sin(currPoint.LatRad));

            SourceDataStream.StartTime = currPoint.Timestamp;
            SourceDataStream.InitialHeading = Utilities.ConvertRadToDeg(azimuth);
            SourceDataStream.InitialPitch = Utilities.ConvertRadToDeg(pitch);
            SourceDataStream.InitialRoll = Utilities.ConvertRadToDeg(roll);

            // TODO: leverarm offset
            SourceDataStream.InitialX = currPoint.X;
            SourceDataStream.InitialY = currPoint.Y;
            SourceDataStream.InitialZ = currPoint.Z;

            WriteMessage("");
            WriteMessage("Solution ");
            WriteMessage("--------------");
            WriteMessage("Start time        [s]: " + SourceDataStream.StartTime);
            WriteMessage("Initial heading [deg]: " + SourceDataStream.InitialHeading);
            WriteMessage("Initial pitch   [deg]: " + SourceDataStream.InitialPitch);
            WriteMessage("Initial roll    [deg]: " + SourceDataStream.InitialRoll);
            WriteMessage("Initial X ECEF    [m]: " + SourceDataStream.InitialX);
            WriteMessage("Initial Y ECEF    [m]: " + SourceDataStream.InitialY);
            WriteMessage("Initial Z ECEF    [m]: " + SourceDataStream.InitialZ);
            WriteMessage("Initial Lat     [deg]: " + currPoint.Lat);
            WriteMessage("Initial Lon     [deg]: " + currPoint.Lon);
            WriteMessage("Initial Lat     [dms]: " + currPoint.LatDMS);
            WriteMessage("Initial Lon     [dms]: " + currPoint.LonDMS);
            WriteMessage("Initial H         [m]: " + currPoint.H);
            WriteProgress(100);

            this.Project.Save();
            return AlgorithmResult.Sucess;
        }
    }
}

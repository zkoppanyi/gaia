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
        private IMUDataStream sourceDataStream;
        public IMUDataStream SourceDataStream { get { return sourceDataStream; } }

        private CoordinateDataStream coordinateDataStream;
        public CoordinateDataStream CoordinateDataStream { get { return coordinateDataStream; } }
        
        [System.ComponentModel.DisplayName("Minimum time matching difference [s]")]
        public double TimeMatchingDifference { get; set; }

        [System.ComponentModel.DisplayName("Time for calculating initialization [s]")]
        public double InitilaizationTime { get; set; }

        [System.ComponentModel.DisplayName("Threshold to detect first movement [m]")]
        public double DetectFirstMovemnetThreshold { get; set; }

        public static IMUInitializationFactory Factory { get { return new IMUInitializationFactory(); } }

        public class IMUInitializationFactory : AlgorithmFactory
        {
            public String Name { get { return "IMU initialization"; } }
            public String Description { get { return "Calculate IMU initail value from coordinate stream"; } }

            public IMUInitializationFactory()
            {
                
            }

            public IMUInitialization Create(Project project, IMUDataStream sourceDataStream, CoordinateDataStream coordinateDataStream)
            {
                IMUInitialization algorithm = new IMUInitialization(project, this.Name, this.Description);
                algorithm.sourceDataStream = sourceDataStream;
                algorithm.coordinateDataStream = coordinateDataStream;
                return algorithm;
            }
        }

        private IMUInitialization(Project project, String name, String description) : base(project, name, description)
        {
            TimeMatchingDifference = 1;
            InitilaizationTime = 120;
            DetectFirstMovemnetThreshold = 0.01;
        }


        /// <summary>
        /// Calculate initilaization parameters for IMU using coordinate stream
        /// </summary>
        /// <returns>Result object</returns>
        protected override AlgorithmResult run()
        {
            if (sourceDataStream == null)
            {
                new GaiaAssertException("IMU data stream is null!");
            }

            if (coordinateDataStream == null)
            {
                new GaiaAssertException("Coordinate data stream is null!");
            }

            if (!sourceDataStream.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + coordinateDataStream.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!coordinateDataStream.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + coordinateDataStream.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!(coordinateDataStream.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
            {
                WriteMessage("The " + coordinateDataStream.Name + " data stream's coordinate system is not 3D! Choose a geographic coordinate system");
                return AlgorithmResult.Failure;
            }

            if (coordinateDataStream.CRS.WKT != sourceDataStream.CRS.WKT)
            {
                WriteMessage("The two data stream has to be in the same CRS!");
                return AlgorithmResult.Failure;
            }

            sourceDataStream.Open();
            coordinateDataStream.Open();

            sourceDataStream.Begin();
            coordinateDataStream.Begin();

            // Start the first timestamps in both dataset
            IMUDataLine imuLine = sourceDataStream.ReadLine() as IMUDataLine;
            CoordinateDataLine coorLine = coordinateDataStream.ReadLine() as CoordinateDataLine;
            if (imuLine.TimeStamp > coorLine.TimeStamp)
            {
                while ((coorLine.TimeStamp <= imuLine.TimeStamp) && !coordinateDataStream.IsEOF())
                {
                    coorLine = coordinateDataStream.ReadLine() as CoordinateDataLine;
                }
            }
            else if (imuLine.TimeStamp < coorLine.TimeStamp)
            {
                while ((imuLine.TimeStamp <= coorLine.TimeStamp) && !coordinateDataStream.IsEOF())
                {
                    imuLine = sourceDataStream.ReadLine() as IMUDataLine;
                }
            }

            if (Math.Abs(imuLine.TimeStamp - coorLine.TimeStamp) > this.TimeMatchingDifference)
            {
                String msg = "Minimum time difference between starting IMU and coordinate stream is higher than the threshold: " + this.TimeMatchingDifference;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            // Calculate initial accelerations and roll and pitch
            double mean_ax = 0, mean_ay = 0, mean_az = 0;
            long data_num = 0;
            double initEnd = imuLine.TimeStamp + InitilaizationTime;
            while ((imuLine.TimeStamp <= initEnd) && !coordinateDataStream.IsEOF())
            {
                imuLine = sourceDataStream.ReadLine() as IMUDataLine;
                mean_ax += imuLine.Ax;
                mean_ay += imuLine.Ay;
                mean_az += imuLine.Az;
                data_num++;
            }

            if ((Math.Abs(imuLine.TimeStamp - initEnd) > this.TimeMatchingDifference) || coordinateDataStream.IsEOF())
            {
                String msg = "Minimum time difference between the last IMU and the end of the initilization periods is higher than the threshold: " + this.TimeMatchingDifference;
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
            GPoint lastPoint = coordinateDataStream.ReadDataLineAsGPoint();
            GPoint currPoint = lastPoint;
            GPoint nextPoint = lastPoint;
            bool isFound = false;
            while ((coorLine.TimeStamp <= initEnd) && !coordinateDataStream.IsEOF())
            {
                currPoint = coordinateDataStream.ReadDataLineAsGPoint();
                double dr = Utilities.L2Distance2d(lastPoint, currPoint);
                if (dr > this.DetectFirstMovemnetThreshold)
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
                String msg = "First movement in " + coordinateDataStream.Name + " stream is not found! Threshold: " + this.DetectFirstMovemnetThreshold;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            WriteMessage("First movement detected!");
            WriteMessage("Index: " + coordinateDataStream.GetPosition());
            WriteMessage("Timestamp: " + currPoint.Timestamp);
            WriteProgress(50);

            sourceDataStream.Close();
            coordinateDataStream.Close();

            GeographicCoordinateSystem crs = coordinateDataStream.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;
            double a = crs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
            double b = crs.HorizontalDatum.Ellipsoid.SemiMinorAxis;
            double e2 = 1 - (b * b) / (a * a);

            double lambda = (1 - e2) * (Math.Tan(nextPoint.LatRad) / Math.Tan(currPoint.LatRad)) + e2 * Math.Sqrt(((1 + ((1 - e2) * Math.Pow((Math.Tan(nextPoint.LatRad)), 2)))) / ((1 + ((1 - e2) * Math.Pow((Math.Tan(currPoint.LatRad)), 2)))));
            double azimuth = Math.Atan2(Math.Sin(nextPoint.LonRad - currPoint.LonRad), (lambda - Math.Cos(nextPoint.LonRad - currPoint.LonRad)) * Math.Sin(currPoint.LatRad));

            sourceDataStream.StartTime = currPoint.Timestamp;
            sourceDataStream.InitialHeading = Utilities.ConvertRadToDeg(azimuth);
            sourceDataStream.InitialPitch = Utilities.ConvertRadToDeg(pitch);
            sourceDataStream.InitialRoll = Utilities.ConvertRadToDeg(roll);

            // TODO: leverarm offset
            sourceDataStream.InitialX = currPoint.X;
            sourceDataStream.InitialY = currPoint.Y;
            sourceDataStream.InitialZ = currPoint.Z;

            WriteMessage("");
            WriteMessage("Solution ");
            WriteMessage("--------------");
            WriteMessage("Start time        [s]: " + sourceDataStream.StartTime);
            WriteMessage("Initial heading [deg]: " + sourceDataStream.InitialHeading);
            WriteMessage("Initial pitch   [deg]: " + sourceDataStream.InitialPitch);
            WriteMessage("Initial roll    [deg]: " + sourceDataStream.InitialRoll);
            WriteMessage("Initial X ECEF    [m]: " + sourceDataStream.InitialX);
            WriteMessage("Initial Y ECEF    [m]: " + sourceDataStream.InitialY);
            WriteMessage("Initial Z ECEF    [m]: " + sourceDataStream.InitialZ);
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

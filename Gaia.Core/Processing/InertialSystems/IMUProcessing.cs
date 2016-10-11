using Accord.Math;
using ProjNet.CoordinateSystems;
using System;

using Gaia.Exceptions;
using Gaia.Core.DataStreams;

namespace Gaia.Core.Processing
{

    public class IMUProcessing : Algorithm
    {
        private IMUDataStream SourceDataStream;

        private CoordinateAttitudeDataStream outputDataStream;
        public CoordinateAttitudeDataStream OutputDataStream { get { return outputDataStream; } }

        public double TimeMatchingDifference { get; set; }

        public static IMUProcessingFactory Factory
        {
            get
            {
                return new IMUProcessingFactory();
            }
        }

        public class IMUProcessingFactory : AlgorithmFactory
        {
            public String Name { get { return "Levelling IMU data"; } }
            public String Description { get { return "Lvelling IMU data."; } }

            public IMUProcessingFactory()
            {
                
            }

            public IMUProcessing Create(Project project, IMUDataStream sourceDataStream, CoordinateAttitudeDataStream outputDataStream)
            {
                IMUProcessing algorithm = new IMUProcessing(project, Name, Description);
                algorithm.SourceDataStream = sourceDataStream;
                algorithm.outputDataStream = outputDataStream;
                return algorithm;
            }
        }

        private IMUProcessing(Project project, String name, String description) : base(project,name, description)
        {
            TimeMatchingDifference = 1;
        }

        /// <summary>
        /// Calculate positions, velocities and attitudes. 
        /// </summary>
        /// <returns>Result object</returns>
        protected override AlgorithmResult run()
        {
            if (SourceDataStream == null)
            {
                new GaiaAssertException("IMU data stream is null!");
            }

            if (outputDataStream == null)
            {
                new GaiaAssertException("Output data stream is null!");
            }

            // Parameter checking...
            if (SourceDataStream.IsTimeStampOrdered == false)
            {
                WriteMessage("The " + SourceDataStream.Name + " is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!(SourceDataStream.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
            {
                WriteMessage("The " + SourceDataStream.Name + " data stream's coordinate system is not 3D! Choose a geographic coordinate system!");
                return AlgorithmResult.Failure;
            }

            if ((SourceDataStream.InitialX == 0) && (SourceDataStream.InitialY == 0) && ((SourceDataStream.InitialX == 0)))
            {
                WriteMessage("Initial position is unknown!");
                return AlgorithmResult.Failure;
            }


            // *** Seek for the begining
            WriteMessage("Seek to the begining...");
            WriteMessage("Start time: " + SourceDataStream.StartTime);
            SourceDataStream.Open();
            SourceDataStream.Begin();
            double prevTimeStamp = 0;
            if (SourceDataStream.StartTime != 0)
            {
                IMUDataLine data = null;
                while (!SourceDataStream.IsEOF())
                {
                    data = SourceDataStream.ReadLine() as IMUDataLine;
                    WriteProgress((double)SourceDataStream.GetPosition() / (double)SourceDataStream.DataNumber * 100.0);

                    if (data.TimeStamp >= SourceDataStream.StartTime)
                    {
                        prevTimeStamp = data.TimeStamp;
                        break;
                    }
                }

                if (data == null || SourceDataStream.IsEOF() || (Math.Abs(data.TimeStamp - SourceDataStream.StartTime) > this.TimeMatchingDifference))
                {
                    String msg = "Starting data is not found. The start time may not be correct. Threshold: " + this.TimeMatchingDifference;
                    WriteMessage(msg);
                    return AlgorithmResult.Failure;
                }
            }

            // *** Caclulate 
            WriteMessage("Calculating...");

            // Initial values
            GeographicCoordinateSystem crs = SourceDataStream.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;
            EarthParameters eparam = EarthParameters.CreateWithXYZ(SourceDataStream.InitialX, SourceDataStream.InitialY, SourceDataStream.InitialZ, crs);
            double[] posn = new double[] { eparam.lat, eparam.lon, eparam.h };

            double heading = Utilities.ConvertDegToRad(SourceDataStream.InitialHeading);
            double pitch = Utilities.ConvertDegToRad(SourceDataStream.InitialPitch);
            double roll = Utilities.ConvertDegToRad(SourceDataStream.InitialRoll);
            double[] Vn = new double[] { SourceDataStream.InitialVn, SourceDataStream.InitialVe, SourceDataStream.InitialVd };
            double[] prh = new double[] { pitch, roll, heading};
            double[,] Cbn = IMUHelperFunctions.prh2dcm(prh);
            double[] qbn = IMUHelperFunctions.dcm2quat(Cbn);

            outputDataStream.Open();
            while (!SourceDataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    SourceDataStream.Close();
                    outputDataStream.Close();
                    WriteMessage("Processing canceled!");
                    return AlgorithmResult.Failure;
                }

                if (Double.IsNaN(prh[0]) || Double.IsNaN(prh[1]) || Double.IsNaN(prh[2]))
                {
                    SourceDataStream.Close();
                    outputDataStream.Close();
                    WriteMessage("PRH value(s) is NaN! Stop.");
                    return AlgorithmResult.Sucess;
                }


                IMUDataLine data = SourceDataStream.ReadLine() as IMUDataLine;
                WriteProgress((double)SourceDataStream.GetPosition() / (double)SourceDataStream.DataNumber * 100.0);

                // Get data
                double currentTimeStamp = data.TimeStamp;
                double ax = data.Ax; double ay = data.Ay; double az = data.Az;
                double wx = data.Wx / 180 * Math.PI; double wy = data.Wy / 180 * Math.PI; double wz = data.Wz / 180 * Math.PI;
                double dt = currentTimeStamp - prevTimeStamp;

                double[] vel = new double[] { ax * dt, ay * dt, az * dt };
                double[] angle = new double[] { wx * dt, wy * dt, wz * dt };

                this.update(ref posn, ref Vn, ref qbn, vel, angle, dt, crs);

                prevTimeStamp = currentTimeStamp;
                Cbn = IMUHelperFunctions.quat2dcm(qbn);
                prh = IMUHelperFunctions.dcm2prh(Cbn);
                
                // Write results
                CoordinateAttitudeDataLine resultData = new CoordinateAttitudeDataLine();
                double x, y, z;
                Utilities.ConvertLLHToXYZ(Utilities.ConvertRadToDeg(posn[0]), Utilities.ConvertRadToDeg(posn[1]), posn[2], eparam.a, eparam.f, out x, out y, out z);
                resultData.TimeStamp = data.TimeStamp;
                resultData.X = x;
                resultData.Y = y;
                resultData.Z = z;
                resultData.Sigma = Utilities.Unknown;
                resultData.Roll = Utilities.ConvertRadToDeg(prh[0]);
                resultData.Pitch = Utilities.ConvertRadToDeg(prh[1]);
                resultData.Heading = Utilities.ConvertRadToDeg(prh[2]);
                resultData.AttitudeSigma = Utilities.Unknown;

                outputDataStream.AddDataLine(resultData);
                outputDataStream.CRS = SourceDataStream.CRS;
                outputDataStream.TRS = SourceDataStream.TRS; 
            }

            outputDataStream.Close();
            SourceDataStream.Close();

            this.Project.Save();
            return AlgorithmResult.Sucess;
        }

        /// <summary>
        /// IMU Mechanization in n-frame (NED system)
        /// Implementation with Quaternions
        /// posn, Vn, qbn are updated after the function call.
        /// </summary>
        /// <param name="posn">Position (lat, lon, h) in radians</param>
        /// <param name="Vn">Velocity in n-frame</param>
        /// <param name="qbn">Quaternion between b and n-frame</param>
        /// <param name="vel">Velocity update</param>
        /// <param name="angle">Angle update</param>
        /// <param name="dt">Time difference between the current and the last observations</param>
        /// <param name="crs">Coordinate reference frame</param>
        private void update(ref double[] posn, ref double[] Vn, ref double[] qbn, double[] vel, double[] angle, double dt, GeographicCoordinateSystem crs)
        {
            double lat = posn[0];
            double lon = posn[1];
            double h = posn[2];

            // Calculate earth related values               
            double sL = Math.Sin(lat);
            double cL = Math.Cos(lat);

            // Earth related errors
            EarthParameters eparam = EarthParameters.CreateWithLLH(lat, lon, h, crs);
            double[] wen_n = new double[] {
                            Vn[1] / (eparam.Re + eparam.h), -Vn[0] / (eparam.Rn + eparam.h), -Vn[1] * sL / cL / (eparam.Re + h)};

            double[] wie_n = new double[] { EarthParameters.EARTH_ROTATION_RATE * cL, 0, -EarthParameters.EARTH_ROTATION_RATE * sL };

            // Update velocity
            double[] g_vec = new double[] { 0, 0, eparam.g };
            double[] vel_inc1 = IMUHelperFunctions.quatrot(qbn, vel);
            double[] vel_inc2 = (IMUHelperFunctions.crossProduct3by3(Vn, (wie_n.Multiply(2).Add(wen_n))).Add(g_vec)).Multiply(dt);
            double[] Vn_new = Vn.Add(vel_inc1).Add(vel_inc2);
            double[] Vn_mid = (Vn.Add(Vn_new)).Divide(2);

            // Update attitude in body frame
            double[] qb = IMUHelperFunctions.rvec2quat(angle);
            double[] qbn_new = IMUHelperFunctions.quatmult(qbn, qb);

            // Update attitude in navigation frame
            wen_n = new double[] {
                        Vn_mid[1] / (eparam.Re + h),
                       -Vn_mid[0] / (eparam.Rn + h),
                       -Vn_mid[1] * sL / cL / (eparam.Re + h)};

            double[] qn = IMUHelperFunctions.rvec2quat((wen_n.Add(wie_n)).Multiply(-dt));
            qbn_new = IMUHelperFunctions.quatmult(qn, qbn_new);

            // Update position
            double[,] D = new double[,] {
                    { 1/(eparam.Rn+h), 0, 0 },
                    {0, 1/cL/(eparam.Re+h), 0 },
                    {0, 0,-1 }
                };
            double[] posn_new = posn.Add(D.Dot(Vn).Multiply(dt));

            Vn = Vn_new;
            qbn = qbn_new;
            posn = posn_new;            
        }


        #region Test functions


        /// <summary>
        /// Test function
        /// </summary>
        /// <returns></returns>
        private bool test_rot2quat()
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

            if (diff > 1e-14) return false;

            return true;
        }

        #endregion

    }
}

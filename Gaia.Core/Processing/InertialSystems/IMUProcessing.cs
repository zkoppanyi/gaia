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
            double[,] Cbn = prh2dcm(prh);
            double[] qbn = dcm2quat(Cbn);

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
                Cbn = quat2dcm(qbn);
                prh = dcm2prh(Cbn);
                
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
            double[] vel_inc1 = quatrot(qbn, vel);
            double[] vel_inc2 = (crossProduct3by3(Vn, (wie_n.Multiply(2).Add(wen_n))).Add(g_vec)).Multiply(dt);
            double[] Vn_new = Vn.Add(vel_inc1).Add(vel_inc2);
            double[] Vn_mid = (Vn.Add(Vn_new)).Divide(2);

            // Update attitude in body frame
            double[] qb = rvec2quat(angle);
            double[] qbn_new = quatmult(qbn, qb);

            // Update attitude in navigation frame
            wen_n = new double[] {
                        Vn_mid[1] / (eparam.Re + h),
                       -Vn_mid[0] / (eparam.Rn + h),
                       -Vn_mid[1] * sL / cL / (eparam.Re + h)};

            double[] qn = rvec2quat((wen_n.Add(wie_n)).Multiply(-dt));
            qbn_new = quatmult(qn, qbn_new);

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

        #region IMU converion functions

        /// <summary>
        /// Direction cosine matrix to Euler (pitch, roll, heading)
        /// </summary>
        /// <param name="dcm">Direction cosine matrix</param>
        /// <returns>Vector of Euler angles (pitch, roll, heading)</returns>
        public static double[] dcm2prh(double[,] dcm)
        {
            // pitch is assumed to be[-pi pi]. singular at pi.use ad-hoc methods to remedy this deficiency
            double pitch = Math.Asin(-dcm[2, 0]);  
            double roll = Math.Atan2(dcm[2, 1], dcm[2, 2]);
            double heading = Math.Atan2(dcm[1, 0], dcm[0, 0]);

            return new double[] { pitch, roll, heading };
        }

        /// <summary>
        /// Quaternio to direction cosine matrix
        /// </summary>
        /// <param name="qba">Quaternion</param>
        /// <returns>Direction cosine matrix</returns>
        public static double[,] quat2dcm(double[] qba)
        {
            double a = qba[0];
            double b = qba[1];
            double c = qba[2];
            double d = qba[3];

            double[,] Cba = Matrix.Create<double>(3,3);
            Cba[0, 0] = Math.Pow(a, 2) + Math.Pow(b, 2) - Math.Pow(c, 2) - Math.Pow(d, 2);
            Cba[1, 0] = 2 * (b * c + a * d);
            Cba[2, 0] = 2 * (b * d - a * c);

            Cba[0, 1] = 2 * (b * c - a * d);
            Cba[1, 1] = Math.Pow(a, 2) - Math.Pow(b, 2) + Math.Pow(c, 2) - Math.Pow(d, 2);
            Cba[2, 1] = 2 * (c * d + a * b);

            Cba[0, 2] = 2 * (b * d + a * c);
            Cba[1, 2] = 2 * (c * d - a * b);
            Cba[2, 2] = Math.Pow(a, 2) - Math.Pow(b, 2) - Math.Pow(c, 2) + Math.Pow(d, 2);

            return Cba;
        }

        /// <summary>
        /// Rotation vector to quaternion
        /// </summary>
        /// <param name="rvec">Rotation vector</param>
        /// <returns>Quaternion</returns>
        public static double[] rvec2quat(double[] rvec)
        {
            double rot_ang = Math.Sqrt(Math.Pow(rvec[0],2) + Math.Pow(rvec[1], 2) + Math.Pow(rvec[2], 2));

            if (rot_ang == 0)
            {
                return new double[] { 1, 0, 0, 0 };
            }
            else
            {
                double cR = Math.Cos(rot_ang / 2);
                double sR = Math.Sin(rot_ang / 2);
                double rx = rvec[0] / rot_ang;
                double ry = rvec[1] / rot_ang;
                double rz = rvec[2] / rot_ang;

                return new double[] { cR, rx*sR, ry*sR, rz*sR };
            }
        }

        public static double[] crossProduct3by3(double[] v, double[] u)
        {
            return new double [] {
                u[1] * v[2] + u[2] * v[1],
                u[2] * v[0] + u[0] * v[2],
                u[0] * v[1] + u[1] * v[0] };
        }

        /// <summary>
        /// Convert va velocity in 'a' frame to 'b' frame with qab quaternion
        /// </summary>
        /// <param name="qab">Quaternioin from 'a' frame to 'b' frame</param>
        /// <param name="va">Velocity in 'a' frame</param>
        /// <returns>Velocity in 'b' frame </returns>
        public static double[] quatrot(double[] qab, double[] va)
        {
            double[] va2 = new double[] { 0, va[0], va[1], va[2] };
            double[] vr_a = quatmult(qab, va2);

            double[] q = qab;
            q[1] = -q[1]; q[2] = -q[2]; q[3] = -q[3];

            double[] vb = quatmult(vr_a, q).Get(1,4);
            return vb;

        }

        /// <summary>
        /// Quaternion multiplication
        /// </summary>
        /// <param name="qab">Quaternion from 'a' frame to 'b' frame</param>
        /// <param name="qca">Quaternion from 'c' frame to 'a' frame</param>
        /// <returns>Quaternion from 'c' frame to 'b' frame</returns>
        public static double[] quatmult(double[] qab, double[] qca)
        {
            double qcb1 = qab[0] * qca[0] + -qab[1] * qca[1] + -qab[2] * qca[2] + -qab[3] * qca[3];
            double qcb2 = qab[1] * qca[0] + qab[0] * qca[1] + -qab[3] * qca[2] + qab[2] * qca[3];
            double qcb3 = qab[2] * qca[0] + qab[3] * qca[1] + qab[0] * qca[2] + -qab[1] * qca[3];
            double qcb4 = qab[3] * qca[0] + -qab[2] * qca[1] + qab[1] * qca[2] + qab[0]* qca[3];
            return new double[] { qcb1, qcb2, qcb3, qcb4 };
        }

        /// <summary>
        /// Euler angle (pitch, roll, heading) to direction cosine matrix
        /// </summary>
        /// <param name="pitch">Pitch</param>
        /// <param name="roll">Roll</param>
        /// <param name="heading">Heading</param>
        /// <returns>Direction cosine matrix</returns>
        public static double[,] prh2dcm(double[] prh)
        {
            double pitch = prh[0];
            double roll = prh[1];
            double heading = prh[2];

            double cr = Math.Cos(roll);
            double sr = Math.Sin(roll);   // roll
            double cp = Math.Cos(pitch);
            double sp = Math.Sin(pitch);  // pitch
            double ch = Math.Cos(heading);
            double sh = Math.Sin(heading);  // heading            
                
           
            double r11 = cp * ch;
            double r12 = (sp * sr * ch) - (cr * sh);
            double r13 = (cr * sp * ch) + (sh * sr);

            double r21 = cp * sh;
            double r22 = (sr * sp * sh) + (cr * ch);
            double r23 = (cr * sp * sh) - (sr * ch);

            double r31 = -sp;
            double r32 = sr * cp;
            double r33 = cr * cp;

            return new double[,] { { r11, r12, r13 }, { r21, r22, r23 } , { r31, r32, r33 } };

        }

        /// <summary>
        /// Dircetion cosine matrix to quaternions
        /// </summary>
        /// <param name="dcm">Direction cosine matrix</param>
        /// <returns>Quaternion</returns>
        public static double[] dcm2quat(double[,] dcm)
        {
            double tr = dcm[0, 0] + dcm[1, 1] + dcm[2, 2];
            double Pa = 1 + tr;
            double Pb = 1 + 2 * dcm[0, 0] - tr;
            double Pc = 1 + 2 * dcm[1, 1] - tr;
            double Pd = 1 + 2 * dcm[2, 2] - tr;

            double quat1 = 0, quat2 = 0, quat3 = 0, quat4 = 0;
            if (Pa >= Pb && Pa >= Pc && Pa >= Pd)
            {
                quat1 = 0.5 * Math.Sqrt(Pa);
                quat2 = (dcm[2, 1] - dcm[1, 2]) / 4 / quat1;
                quat3 = (dcm[0, 2] - dcm[2, 0]) / 4 / quat1;
                quat4 = (dcm[1, 0] - dcm[0, 1]) / 4 / quat1;
            }

            else if (Pb >= Pc && Pb >= Pd)
            {
                quat2 = 0.5 * Math.Sqrt(Pb);
                quat3 = (dcm[1, 0] + dcm[0, 1]) / 4 / quat2;
                quat4 = (dcm[0, 2] + dcm[2, 0]) / 4 / quat2;
                quat1 = (dcm[2, 1] - dcm[1, 2]) / 4 / quat2;
            }
            else if (Pc >= Pd)
            {
                quat3 = 0.5 * Math.Sqrt(Pc);
                quat4 = (dcm[2, 1] + dcm[1, 2]) / 4 / quat3;
                quat1 = (dcm[0, 2] - dcm[2, 0]) / 4 / quat3;
                quat2 = (dcm[1, 0] + dcm[0, 1]) / 4 / quat3;
            }
            else
            {
                quat4 = 0.5 * Math.Sqrt(Pd);
                quat1 = (dcm[1, 0] - dcm[0, 1]) / 4 / quat4;
                quat2 = (dcm[0, 2] + dcm[2, 0]) / 4 / quat4;
                quat3 = (dcm[2, 1] + dcm[1, 2]) / 4 / quat4;
            }

            if (quat1 <= 0)
            {
                quat1 *= -1;
                quat2 *= -1;
                quat3 *= -1;
                quat4 *= -1;
            }

            /*double quat1 = 0.5 * (Math.Sqrt(1 + rot.At(0, 0) + rot.At(1, 1) + rot.At(2, 2)));
            double quat2 = (0.25 * (rot.At(1, 2) - rot.At(2, 1))) / (0.5 * Math.Sqrt(1 + rot.At(0, 0) + rot.At(1, 1) + rot.At(2, 2)));
            double quat3 = (0.25 * (rot.At(2, 0) - rot.At(0, 2))) / (0.5 * Math.Sqrt(1 + rot.At(0, 0) + rot.At(1, 1) + rot.At(2, 2)));
            double quat4 = (0.25 * (rot.At(0, 1) - rot.At(1, 0))) / (0.5 * Math.Sqrt(1 + rot.At(0, 0) + rot.At(1, 1) + rot.At(2, 2)));*/

            return new double[] { quat1, quat2, quat3, quat4 };
        }
        #endregion

        #region Test functions

        /// <summary>
        /// Test function
        /// </summary>
        /// <returns></returns>
        private bool test_Rpm2rot()
        {
            double azimuth = 0.058569976172196;
            double pitch = 0.016057418906273;
            double roll = -0.009318206085104;

            double[,] rot = prh2dcm(new double[] { pitch, roll, azimuth} );
            double[,] perf = new double[,] { { 0.998156572698104, -0.058683314972773, 0.015483052779751 }, 
                { 0.058528948713351, 0.998233171397190, 0.010241957079499 }, { -0.016056728872475, -0.009316869974122, 0.999827673847749 } };

            double diff = rot.Subtract(perf).Norm2();

            if (diff > 1e-14) return false;

            return true;
        }

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
            double[,] dcm = prh2dcm(prh);
            double[] quat = dcm2quat(dcm);

            double[] perf = new double[] { 0.999527065409317,
                0.004892020369056, -0.007888676240926, -0.029316930912252 };

            double diff = quat.Subtract(perf).Euclidean();

            if (diff > 1e-14) return false;

            return true;
        }

        #endregion

    }
}

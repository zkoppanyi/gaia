using Gaia.DataStreams;
using Gaia.Excpetions;
using Gaia.GaiaSystem;
using Gaia.Processing;
using MathNet.Numerics.LinearAlgebra;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{

    public class IMUProcessing : Algorithm
    {
        [System.ComponentModel.DisplayName("Minimum time matching difference [s]")]
        public double TimeMatchingDifference { get; set; }

        [System.ComponentModel.DisplayName("Time for calculating initialization [s]")]
        public double InitilaizationTime { get; set; }

        [System.ComponentModel.DisplayName("Threshold to detect first movement [m]")]
        public double DetectFirstMovemnetThreshold { get; set; }

        public IMUProcessing(Project project, IMessanger messanger) : base(project, messanger)
        {
            TimeMatchingDifference = 1;
            InitilaizationTime = 120;
            DetectFirstMovemnetThreshold = 0.01;
        }

        /// <summary>
        /// Calculate initilaization parameters for IMU using coordinate stream
        /// </summary>
        /// <param name="imu">IMU stream</param>
        /// <param name="coor">Coordinate stream</param>
        /// <returns>Result object</returns>
        public AlgorithmResult CalculateInitilazationWithCoordinates(IMUDataStream imu, CoordinateDataStream coor)
        {
            if (!imu.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + coor.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!coor.IsTimeStampOrdered)
            {
                WriteMessage("The timestamps in the " + coor.Name + " dataset is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!(coor.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
            { 
                WriteMessage("The " + coor.Name + " data stream's coordinate system is not 3D! Choose a geographic coordinate system");
                return AlgorithmResult.Failure;
            }

            if (coor.CRS.WKT != imu.CRS.WKT)
            {
                WriteMessage("The two data stream has to be in the same CRS!");
                return AlgorithmResult.Failure;
            }

            imu.Open();
            coor.Open();

            imu.Begin();
            coor.Begin();

            // Start the first timestamps in both dataset
            IMUDataLine imuLine = imu.ReadLine() as IMUDataLine;
            CoordinateDataLine coorLine = coor.ReadLine() as CoordinateDataLine;
            if (imuLine.TimeStamp > coorLine.TimeStamp)
            {
                while ((coorLine.TimeStamp <= imuLine.TimeStamp) && !coor.IsEOF())
                {
                    coorLine = coor.ReadLine() as CoordinateDataLine;
                }
            }
            else if (imuLine.TimeStamp < coorLine.TimeStamp)
            {
                while ((imuLine.TimeStamp <= coorLine.TimeStamp) && !coor.IsEOF())
                {
                    imuLine = imu.ReadLine() as IMUDataLine;
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
            while ((imuLine.TimeStamp <= initEnd) && !coor.IsEOF())
            {
                imuLine = imu.ReadLine() as IMUDataLine;
                mean_ax += imuLine.Ax;
                mean_ay += imuLine.Ay;
                mean_az += imuLine.Az;
                data_num++;
            }

            if ((Math.Abs(imuLine.TimeStamp - initEnd) > this.TimeMatchingDifference) || coor.IsEOF())
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
            GPoint lastPoint = coor.ReadDataLineAsGPoint();
            GPoint currPoint = lastPoint;
            GPoint nextPoint = lastPoint;
            bool isFound = false;
            while ((coorLine.TimeStamp <= initEnd) && !coor.IsEOF())
            {
                currPoint = coor.ReadDataLineAsGPoint();
                double dr = Utilities.L2Distance2d(lastPoint, currPoint);
                if (dr > this.DetectFirstMovemnetThreshold )
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
                String msg = "First movement in " + coor.Name + " stream is not found! Threshold: " + this.DetectFirstMovemnetThreshold;
                WriteMessage(msg);
                return AlgorithmResult.Failure;
            }

            WriteMessage("First movement detected!");
            WriteMessage("Index: " + coor.GetPosition());
            WriteMessage("Timestamp: " + currPoint.Timestamp);
            WriteProgress(50);

            imu.Close();
            coor.Close();

            GeographicCoordinateSystem crs = coor.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;
            double a = crs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
            double b = crs.HorizontalDatum.Ellipsoid.SemiMinorAxis;
            double e2 = 1 - (b * b) / (a * a);

            double lambda = (1 - e2) * (Math.Tan(nextPoint.LatRad) / Math.Tan(currPoint.LatRad)) + e2 * Math.Sqrt(((1 + ((1 - e2) * Math.Pow((Math.Tan(nextPoint.LatRad)), 2)))) / ((1 + ((1 - e2) * Math.Pow((Math.Tan(currPoint.LatRad)), 2)))));
            double azimuth = Math.Atan2(Math.Sin(nextPoint.LonRad - currPoint.LonRad), (lambda - Math.Cos(nextPoint.LonRad - currPoint.LonRad)) * Math.Sin(currPoint.LatRad));

            imu.StartTime = currPoint.Timestamp;
            imu.InitialHeading = Utilities.ConvertRadToDeg(azimuth);
            imu.InitialPitch = Utilities.ConvertRadToDeg(pitch);
            imu.InitialRoll = Utilities.ConvertRadToDeg(roll);

            // TODO: leverarm offset
            imu.InitialX = currPoint.X;
            imu.InitialY = currPoint.Y;
            imu.InitialZ = currPoint.Z;

            WriteMessage("");
            WriteMessage("Solution ");
            WriteMessage("--------------");
            WriteMessage("Start time        [s]: " + imu.StartTime);
            WriteMessage("Initial heading [deg]: " + imu.InitialHeading);
            WriteMessage("Initial pitch   [deg]: " + imu.InitialPitch);
            WriteMessage("Initial roll    [deg]: " + imu.InitialRoll);
            WriteMessage("Initial X ECEF    [m]: " + imu.InitialX);
            WriteMessage("Initial Y ECEF    [m]: " + imu.InitialY);
            WriteMessage("Initial Z ECEF    [m]: " + imu.InitialZ);
            WriteMessage("Initial Lat     [deg]: " + currPoint.Lat);
            WriteMessage("Initial Lon     [deg]: " + currPoint.Lon);
            WriteMessage("Initial Lat     [dms]: " + currPoint.LatDMS);
            WriteMessage("Initial Lon     [dms]: " + currPoint.LonDMS);
            WriteMessage("Initial H         [m]: " + currPoint.H);
            WriteProgress(100);

            this.Project.Save();
            return AlgorithmResult.Sucess;
        }
        

        /// <summary>
        /// Calculate positions, velocities and attitudes. 
        /// </summary>
        /// <param name="imu">IMU data stream</param>
        /// <param name="output">Output data stream</param>
        /// <returns>Result object</returns>
        public AlgorithmResult CalculateTrajectory(IMUDataStream imu, CoordinateAttitudeDataStream output)
        {
            // Parameter checking...
            if (imu.IsTimeStampOrdered == false)
            {
                WriteMessage("The " + imu.Name + " is not ordered by timestamp!");
                return AlgorithmResult.Failure;
            }

            if (!(imu.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
            {
                WriteMessage("The " + imu.Name + " data stream's coordinate system is not 3D! Choose a geographic coordinate system!");
                return AlgorithmResult.Failure;
            }

            if ((imu.InitialX == 0) && (imu.InitialY == 0) && ((imu.InitialX == 0)))
            {
                WriteMessage("Initial position is unknown!");
                return AlgorithmResult.Failure;
            }


            // *** Seek for the begining
            WriteMessage("Seek to the begining...");
            WriteMessage("Start time: " + imu.StartTime);
            imu.Open();
            imu.Begin();
            double prevTimeStamp = 0;
            if (imu.StartTime != 0)
            {
                IMUDataLine data = null;
                while (!imu.IsEOF())
                {
                    data = imu.ReadLine() as IMUDataLine;
                    WriteProgress((double)imu.GetPosition() / (double)imu.DataNumber * 100.0);

                    if (data.TimeStamp >= imu.StartTime)
                    {
                        prevTimeStamp = data.TimeStamp;
                        break;
                    }
                }

                if (data == null || imu.IsEOF() || (Math.Abs(data.TimeStamp - imu.StartTime) > this.TimeMatchingDifference))
                {
                    String msg = "Starting data is not found. The start time may not be correct. Threshold: " + this.TimeMatchingDifference;
                    WriteMessage(msg);
                    return AlgorithmResult.Failure;
                }
            }

            // *** Caclulate 
            WriteMessage("Calculating...");

            // Initial values
            GeographicCoordinateSystem crs = imu.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;
            EarthParameters eparam = EarthParameters.CreateWithXYZ(imu.InitialX, imu.InitialY, imu.InitialZ, crs);
            Vector<double> posn = Vector<double>.Build.DenseOfArray(new double[] { eparam.lat, eparam.lon, eparam.h });

            double heading = Utilities.ConvertDegToRad(imu.InitialHeading);
            double pitch = Utilities.ConvertDegToRad(imu.InitialPitch);
            double roll = Utilities.ConvertDegToRad(imu.InitialRoll);
            Vector<double> Vn = Vector<double>.Build.DenseOfArray(new double[] { imu.InitialVn, imu.InitialVe, imu.InitialVd });
            Vector<double> prh = Vector<double>.Build.DenseOfArray(new double[] { pitch, roll, heading});
            Matrix<double> Cbn = prh2dcm(prh);
            Vector<double> qbn = dcm2quat(Cbn);

            output.Open();
            while (!imu.IsEOF())
            {
                if (IsCanceled())
                {
                    imu.Close();
                    output.Close();
                    WriteMessage("Processing canceled!");
                    return AlgorithmResult.Failure;
                }

                IMUDataLine data = imu.ReadLine() as IMUDataLine;
                WriteProgress((double)imu.GetPosition() / (double)imu.DataNumber * 100.0);

                // Get data
                double currentTimeStamp = data.TimeStamp;
                double ax = data.Ax; double ay = data.Ay; double az = data.Az;
                double wx = data.Wx / 180 * Math.PI; double wy = data.Wy / 180 * Math.PI; double wz = data.Wz / 180 * Math.PI;
                double dt = currentTimeStamp - prevTimeStamp;

                Vector<double> vel = Vector<double>.Build.DenseOfArray(new double[] { ax * dt, ay * dt, az * dt });
                Vector<double> angle = Vector<double>.Build.DenseOfArray(new double[] { wx * dt, wy * dt, wz * dt });

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

                output.AddDataLine(resultData);
                output.CRS = imu.CRS;
                output.TRS = imu.TRS; 
            }

            output.Close();
            imu.Close();

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
        private void update(ref Vector<double> posn, ref Vector<double> Vn, ref Vector<double> qbn, Vector<double> vel, Vector<double> angle, double dt, GeographicCoordinateSystem crs)
        {
            double lat = posn[0];
            double lon = posn[1];
            double h = posn[2];

            // Calculate earth related values               
            double sL = Math.Sin(lat);
            double cL = Math.Cos(lat);

            // Earth related errors
            EarthParameters eparam = EarthParameters.CreateWithLLH(lat, lon, h, crs);
            Vector<double> wen_n = Vector<double>.Build.DenseOfArray(new double[] {
                            Vn.At(1) / (eparam.Re + eparam.h), -Vn.At(0) / (eparam.Rn + eparam.h), -Vn.At(1) * sL / cL / (eparam.Re + h)});

            Vector<double> wie_n = Vector<double>.Build.DenseOfArray(new double[] { EarthParameters.EARTH_ROTATION_RATE * cL, 0, -EarthParameters.EARTH_ROTATION_RATE * sL });

            // Update velocity
            Vector<double> g_vec = Vector<double>.Build.DenseOfArray(new double[] { 0, 0, eparam.g });
            Vector<double> vel_inc1 = quatrot(qbn, vel);
            Vector<double> vel_inc2 = (crossProduct3by3(Vn, (2 * wie_n + wen_n)) + g_vec) * dt;
            Vector<double> Vn_new = Vn + vel_inc1 + vel_inc2;
            Vector<double> Vn_mid = (Vn + Vn_new) / 2;

            // Update attitude in body frame
            Vector<double> qb = rvec2quat(angle);
            Vector<double> qbn_new = quatmult(qbn, qb);

            // Update attitude in navigation frame
            wen_n = Vector<double>.Build.DenseOfArray(new double[] {
                        Vn_mid[1] / (eparam.Re + h),
                       -Vn_mid[0] / (eparam.Rn + h),
                       -Vn_mid[1] * sL / cL / (eparam.Re + h)});

            Vector<double> qn = rvec2quat(-(wen_n + wie_n) * dt);
            qbn_new = quatmult(qn, qbn_new);

            // Update position
            Matrix<double> D = Matrix<double>.Build.DenseOfArray(new double[,] {
                    { 1/(eparam.Rn+h), 0, 0 },
                    {0, 1/cL/(eparam.Re+h), 0 },
                    {0, 0,-1 }
                });
            Vector<double> posn_new = posn + D * Vn * dt;

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
        public static Vector<double> dcm2prh(Matrix<double> dcm)
        {
            // pitch is assumed to be[-pi pi]. singular at pi.use ad-hoc methods to remedy this deficiency
            double pitch = Math.Asin(-dcm[2, 0]);  
            double roll = Math.Atan2(dcm[2, 1], dcm[2, 2]);
            double heading = Math.Atan2(dcm[1, 0], dcm[0, 0]);

            return Vector<double>.Build.DenseOfArray(new double[] { pitch, roll, heading });
        }

        /// <summary>
        /// Quaternio to direction cosine matrix
        /// </summary>
        /// <param name="qba">Quaternion</param>
        /// <returns>Direction cosine matrix</returns>
        public static Matrix<double> quat2dcm(Vector<double> qba)
        {
            double a = qba[0];
            double b = qba[1];
            double c = qba[2];
            double d = qba[3];

            Matrix<double> Cba = Matrix<double>.Build.Dense(3,3);
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
        public static Vector<double> rvec2quat(Vector<double> rvec)
        {
            double rot_ang = Math.Sqrt(Math.Pow(rvec[0],2) + Math.Pow(rvec[1], 2) + Math.Pow(rvec[2], 2));

            if (rot_ang == 0)
            {
                return Vector<double>.Build.DenseOfArray(new double[] { 1, 0, 0, 0 });
            }
            else
            {
                double cR = Math.Cos(rot_ang / 2);
                double sR = Math.Sin(rot_ang / 2);
                double rx = rvec[0] / rot_ang;
                double ry = rvec[1] / rot_ang;
                double rz = rvec[2] / rot_ang;

                return Vector<double>.Build.DenseOfArray(new double[] { cR, rx*sR, ry*sR, rz*sR });
            }
        }

        public static Vector<double> crossProduct3by3(Vector<double> v, Vector<double> u)
        {
            return Vector<double>.Build.DenseOfArray(new double [] {
                u[1] * v[2] + u[2] * v[1],
                u[2] * v[0] + u[0] * v[2],
                u[0] * v[1] + u[1] * v[0] });
        }

        /// <summary>
        /// Convert va velocity in 'a' frame to 'b' frame with qab quaternion
        /// </summary>
        /// <param name="qab">Quaternioin from 'a' frame to 'b' frame</param>
        /// <param name="va">Velocity in 'a' frame</param>
        /// <returns>Velocity in 'b' frame </returns>
        public static Vector<double> quatrot(Vector<double> qab, Vector<double> va)
        {
            Vector<double> va2 = Vector<double>.Build.DenseOfArray(new double[] { 0, va[0], va[1], va[2] });
            Vector<double> vr_a = quatmult(qab, va2);

            Vector<double> q = Vector<double>.Build.DenseOfVector(qab);
            q[1] = -q[1]; q[2] = -q[2]; q[3] = -q[3];

            Vector<double> vb = quatmult(vr_a, q).SubVector(1,3);
            return vb;

        }

        /// <summary>
        /// Quaternion multiplication
        /// </summary>
        /// <param name="qab">Quaternion from 'a' frame to 'b' frame</param>
        /// <param name="qca">Quaternion from 'c' frame to 'a' frame</param>
        /// <returns>Quaternion from 'c' frame to 'b' frame</returns>
        public static Vector<double> quatmult(Vector<double> qab, Vector<double> qca)
        {
            double qcb1 = (qab.At(0)) * qca.At(0) + (-qab.At(1)) * qca.At(1) + (-qab.At(2)) * qca.At(2) + (-qab.At(3)) * qca.At(3);
            double qcb2 = (qab.At(1)) * qca.At(0) + (qab.At(0)) * qca.At(1) + (-qab.At(3)) * qca.At(2) + (qab.At(2)) * qca.At(3);
            double qcb3 = (qab.At(2)) * qca.At(0) + (qab.At(3)) * qca.At(1) + (qab.At(0)) * qca.At(2) + (-qab.At(1)) * qca.At(3);
            double qcb4 = (qab.At(3)) * qca.At(0) + (-qab.At(2)) * qca.At(1) + (qab.At(1)) * qca.At(2) + (qab.At(0)) * qca.At(3);
            return Vector<double>.Build.DenseOfArray(new double[] { qcb1, qcb2, qcb3, qcb4 });
        }

        /// <summary>
        /// Euler angle (pitch, roll, heading) to direction cosine matrix
        /// </summary>
        /// <param name="pitch">Pitch</param>
        /// <param name="roll">Roll</param>
        /// <param name="heading">Heading</param>
        /// <returns>Direction cosine matrix</returns>
        public static Matrix<double> prh2dcm(Vector<double> prh)
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

            return Matrix<double>.Build.DenseOfArray(new double[,] { { r11, r12, r13 }, { r21, r22, r23 } , { r31, r32, r33 } });

        }

        /// <summary>
        /// Dircetion cosine matrix to quaternions
        /// </summary>
        /// <param name="dcm">Direction cosine matrix</param>
        /// <returns>Quaternion</returns>
        public static Vector<double> dcm2quat(Matrix<double> dcm)
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

            return Vector<double>.Build.DenseOfArray(new double[] { quat1, quat2, quat3, quat4 });
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

            Matrix<double> rot = prh2dcm(Vector<double>.Build.DenseOfArray(new double[] { pitch, roll, azimuth}) );
            Matrix<double> perf = Matrix<double>.Build.DenseOfArray(new double[,] { { 0.998156572698104, -0.058683314972773, 0.015483052779751 }, 
                { 0.058528948713351, 0.998233171397190, 0.010241957079499 }, { -0.016056728872475, -0.009316869974122, 0.999827673847749 } });

            double diff = rot.Subtract(perf).L2Norm();

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

            Vector<double> prh = Vector<double>.Build.DenseOfArray(new double[] { pitch, roll, azimuth });
            Matrix<double> dcm = prh2dcm(prh);
            Vector<double> quat = dcm2quat(dcm);

            Vector<double> perf = Vector<double>.Build.DenseOfArray(new double[] { 0.999527065409317,
                0.004892020369056, -0.007888676240926, -0.029316930912252 });

            double diff = quat.Subtract(perf).L2Norm();

            if (diff > 1e-14) return false;

            return true;
        }

        #endregion

    }
}

using Accord.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    public static class IMUHelperFunctions
    {
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

            double[,] Cba = Matrix.Create<double>(3, 3);
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
            double rot_ang = Math.Sqrt(Math.Pow(rvec[0], 2) + Math.Pow(rvec[1], 2) + Math.Pow(rvec[2], 2));

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

                return new double[] { cR, rx * sR, ry * sR, rz * sR };
            }
        }

        public static double[] crossProduct3by3(double[] v, double[] u)
        {
            return new double[] {
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

            double[] vb = quatmult(vr_a, q).Get(1, 4);
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
            double qcb4 = qab[3] * qca[0] + -qab[2] * qca[1] + qab[1] * qca[2] + qab[0] * qca[3];
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

            return new double[,] { { r11, r12, r13 }, { r21, r22, r23 }, { r31, r32, r33 } };

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
    }
}

using Gaia.DataStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Processing
{

    /// <summary>
    /// General utility functions
    /// </summary>
    public static class Utilities
    {

        private static int secPrecision = 8;
        public static double g = 9.80665;
        public static double Unknown = -1;

        public static void ConvertLLHToXYZ(double lat, double lon, double h, double a, double f, out double x, out double y, out double z)
        {
            x = 0; y = 0; z = 0;
            if ((a == 0) || (f == 0)) return;

            double latrad = ConvertDegToRad(lat);
            double lonrad = ConvertDegToRad(lon);

            double e2 = 2*f-f*f;
            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(latrad), 2));
            x = (N + h) * Math.Cos(latrad) * Math.Cos(lonrad);
            y = (N + h) * Math.Cos(latrad) * Math.Sin(lonrad);
            z = ((1 - e2)*N + h)*Math.Sin(latrad);
        }

        public static void ConvertXYZToLLH(double x, double y, double z, double a, double f, out double lat, out double lon, out double h)
        {
            lat = 0; lon = 0; h = 0;
            if ((a == 0) || (f == 0)) return;

            double b = a * (1 - f);
            double p = Math.Sqrt(x * x + y * y);
            double omega = Math.Atan((z * a) / (p * b));
            double e2 = 2 * f - f * f;
            double e2v = (a * a) / (b * b)-1;
            lat = Math.Atan((z + e2v * b * Math.Pow(Math.Sin(omega), 3)) / (p - e2 * a * Math.Pow(Math.Cos(omega), 3)));
            lon = x != 0 ? Math.Atan2(y, x) : 0;
            double N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(lat), 2));
            h = Math.Cos(lat) != 0 ? p / Math.Cos(lat) - N : 0;
        }

        public static void ConvertDegToDMS(double value, out int deg, out int min, out double sec)
        {
            if (value != value) // is nan
            {
                deg = 0;
                min = 0;
                sec = 0;
                return;
            }

            if (value >= 0)
            {
                deg = (int)Math.Floor(value);
                min = (int)Math.Floor((value - (double)deg) * 60);
                sec = (value - (double)deg - (double)min / 60) * 3600;

                if (Math.Round(sec, secPrecision) == 60) { sec = 0; min++; }
                if (min == 60) { min = 0; deg++; }

            }
            else
            {
                deg = (int)Math.Ceiling(value);
                min = (int)Math.Ceiling((value - (double)deg) * 60);
                sec = (value - (double)deg - (double)min / 60) * 3600;

                min = Math.Abs(min);
                sec = Math.Abs(sec);

                if (Math.Round(sec, secPrecision) == 60) { sec = 0; min++; }
                if (min == 60) { min = 0; deg--; }
            }

        }

        public static void ConvertDMSToDeg(int deg, int min, double sec, out double value)
        {
            if (deg >= 0)
            {
                value = (double)deg + (double)min / 60 + sec / 3600;
            }
            else
            {
                value = (double)deg - (double)min / 60 - sec / 3600;
            }
        }

        public static string ConvertDMSToDMSString(int deg, int min, double sec)
        {
            return deg.ToString("F0") + "° " + min.ToString("F0") + "' " + Math.Round(sec,4).ToString("F4") + "\"";
        }

        public static string ConvertDegToDMSString(double val)
        {
            int deg, min;
            double sec;
            ConvertDegToDMS(val, out deg, out min, out sec);
            return ConvertDMSToDMSString(deg, min, sec);
        }

        public static double ConvertDegToRad(double deg)
        {
            return deg / 180.0 * Math.PI;
        }

        public static double ConvertRadToDeg(double rad)
        {
            return rad / Math.PI * 180.0;
        }        

        public static double rad2deg(double val)
        {
            return val * 57.2957795;
        }

        public static double deg2rad(double val)
        {
            return val * 0.01745329252;
        }

        public static double L2Distance2d(CoordinateDataLine line1, CoordinateDataLine line2)
        {
            return Math.Sqrt(Math.Pow(line2.X - line1.X, 1) + Math.Pow(line2.Y - line1.Y, 2));
        }

        public static double L2Distance3d(CoordinateDataLine line1, CoordinateDataLine line2)
        {
            return Math.Sqrt(Math.Pow(line2.X - line1.X, 1) + Math.Pow(line2.Y - line1.Y, 2) + Math.Pow(line2.Z - line1.Z, 2));
        }

        public static double L2Distance2d(GPoint pt1, GPoint pt2)
        {
            return Math.Sqrt(Math.Pow(pt2.X - pt1.X, 2) + Math.Pow(pt2.Y - pt1.Y, 2));
        }

        public static double L2Distance3d(GPoint pt1, GPoint pt2)
        {
            return Math.Sqrt(Math.Pow(pt2.X - pt1.X, 1) + Math.Pow(pt2.Y - pt1.Y, 2) + Math.Pow(pt2.Z - pt1.Z, 2));
        }

        public static object GetValueByDisplayNameAttribute(object obj, String attribute)
        {
            var properties = obj.GetType().GetProperties();
            foreach (var property in properties)
            {
                var displayAttribute = property
                    .GetCustomAttributes(typeof(DisplayNameAttribute), true)
                    .FirstOrDefault() as DisplayNameAttribute;

                if (displayAttribute != null)
                {
                    string displayName = displayAttribute.DisplayName;
                    if (displayName == attribute)
                    {
                        return property.GetValue(obj, null);
                    }
                }
            }

            return null;
        }

    }
}

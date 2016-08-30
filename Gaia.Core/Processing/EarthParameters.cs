using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// Generate Earth related parameters
    /// </summary>
    public class EarthParameters
    {
        /// <summary>
        /// Semi major axis
        /// </summary>
        private double _a;
        public double a { get { return _a; } }

        /// <summary>
        /// Semi minor axis
        /// </summary>
        private double _b;
        public double b { get { return _b; } }

        /// <summary>
        /// Flattening
        /// </summary>
        private double _f;
        public double f { get { return _f; } }

        /// <summary>
        /// Squared eccentricity
        /// </summary>
        private double _e2;
        public double e2 { get { return _e2; } }

        /// <summary>
        /// The parameters are calculated for a point with this latitude
        /// </summary>
        private double _lat;
        public double lat { get { return _lat; } }

        /// <summary>
        /// The parameters are calculated for a point with this longitude
        /// </summary>
        private double _lon;
        public double lon { get { return _lon; } }

        /// <summary>
        /// The parameters are calculated for a point with this height
        /// </summary>
        private double _h;
        public double h { get { return _h; } }

        /// <summary>
        /// Gravitation
        /// </summary>
        private double _g;
        public double g { get { return _g; } }

        private double _Rn;
        public double Rn { get { return _Rn; } }

        private double _Re;
        public double Re { get { return _Re; } }

        private double _N;
        public double N { get { return _N; } }

        private double _M;
        public double M { get { return _M; } }

        public const double EARTH_ROTATION_RATE = 7292115e-11;
        public const double NORMAL_GRAVITY = 9.7803253359;
        public const double GRAVITATIONAL_CONSTANT = 0.00193185265241;
        public const double M_FAKTOR = 0.00344978650684;

        private EarthParameters(double lat, double lon, double h, GeographicCoordinateSystem crs)
        {
            _a = crs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
            _b = crs.HorizontalDatum.Ellipsoid.SemiMinorAxis;
            _f = (_a - _b) / _a;    
            _e2 = 1 - (_b * _b) / (_a * _a);
            _lat = lat;
            _lon = lon;
            _h = h;
            _N = a / Math.Sqrt(1 - e2 * Math.Pow(Math.Sin(_lat), 2));
            _M = a * (1 - e2) / Math.Pow((1 - e2 * Math.Pow(Math.Sin(_lat), 2)), 3.0 / 2.0);

            double sL = Math.Sin(_lat);
            double cL = Math.Cos(_lat);
            _Rn = 6335439.327292829 / (Math.Sqrt(1.0 - e2 * sL * sL) * (1.0 - e2 * sL * sL));
            _Re = a / (Math.Sqrt(1.0 - e2 * sL * sL));
            double g1 = NORMAL_GRAVITY * (1 + GRAVITATIONAL_CONSTANT * sL * sL) / (Math.Sqrt(1.0 - e2 * sL * sL));
            _g = g1 * (1.0 - (2.0 / a) * (1.0 + f + M_FAKTOR - 2.0 * f * sL * sL) * h + 3.0 * h * h / a / a);
        }

        public static EarthParameters CreateWithXYZ(double X, double Y, double Z, GeographicCoordinateSystem crs)
        {
            double a = crs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
            double b = crs.HorizontalDatum.Ellipsoid.SemiMinorAxis;
            double f = (a - b) / a;
            double lat, lon, h;
            Utilities.ConvertXYZToLLH(X, Y, Z, a, f, out lat, out lon, out h);
            return new EarthParameters(X, Y, Z, crs);

        }


        public static EarthParameters CreateWithLLH(double lat, double lon, double h, GeographicCoordinateSystem crs)
        {
            return new EarthParameters(lat, lon, h, crs);

        }
    }
}

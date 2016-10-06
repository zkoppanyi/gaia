using Accord.Math;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;
using Gaia.Core.Processing;
using Gaia.Core.ReferenceFrames;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public enum GPointType
    {
        NA = 0,
        Initial,
        Estimated,
        Fixed,
        Rover
    }

    [Serializable]
    public enum GPointRole
    {
        Actived = 0,
        Deactivated
    }

    [Serializable]
    public class GPoint : GaiaSpatialObject
    {
        [System.ComponentModel.DisplayName("X [m]")]
        public double X { get; set; }

        [System.ComponentModel.DisplayName("Y [m]")]
        public double Y { get; set; }

        [System.ComponentModel.DisplayName("Z [m]")]
        public double Z { get; set; }

        [System.ComponentModel.DisplayName("Timestamp [s]")]
        public double Timestamp { get; set; }

        [System.ComponentModel.DisplayName("Latitude [Deg]")]
        public double Lat {
            get
            {
                if (CRS != null)
                {
                    ICoordinateSystem sys = (ICoordinateSystem)ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(this.CRS.WKT);
                    if (sys is GeographicCoordinateSystem)
                    {
                        GeographicCoordinateSystem sysGCS = sys as GeographicCoordinateSystem;
                        double a = sysGCS.HorizontalDatum.Ellipsoid.SemiMajorAxis;
                        double f = sysGCS.HorizontalDatum.Ellipsoid.InverseFlattening;
                        double lat, lon, h;
                        Utilities.ConvertXYZToLLH(X, Y, Z, a, 1/f, out lat, out lon, out h);
                        return Utilities.ConvertRadToDeg(lat);
                    }
                }
                return 0;
            }
        }

        [System.ComponentModel.DisplayName("Longitude [Deg]")]
        public double Lon
        {
            get
            {
                if (CRS != null)
                {
                    ICoordinateSystem sys = (ICoordinateSystem)ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(this.CRS.WKT);
                    if (sys is GeographicCoordinateSystem)
                    {
                        GeographicCoordinateSystem sysGCS = sys as GeographicCoordinateSystem;
                        double a = sysGCS.HorizontalDatum.Ellipsoid.SemiMajorAxis;
                        double f = sysGCS.HorizontalDatum.Ellipsoid.InverseFlattening;
                        double lat, lon, h;
                        Utilities.ConvertXYZToLLH(X, Y, Z, a, 1/f, out lat, out lon, out h);
                        return Utilities.ConvertRadToDeg(lon);
                    }
                }
                return 0;
            }
        }

        [Browsable(false)]
        public double LonRad
        {
            get
            {
                return this.Lon * 0.01745329251;
            }
        }

        [Browsable(false)]
        public double LatRad
        {
            get
            {
                return this.Lat * 0.01745329251;
            }
        }

        [System.ComponentModel.DisplayName("Height [m]")]
        public double H
        {
            get
            {
                if (CRS != null)
                {
                    ICoordinateSystem sys = (ICoordinateSystem)ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(this.CRS.WKT);
                    if (sys is GeographicCoordinateSystem)
                    {
                        GeographicCoordinateSystem sysGCS = sys as GeographicCoordinateSystem;
                        double a = sysGCS.HorizontalDatum.Ellipsoid.SemiMajorAxis;
                        double f = sysGCS.HorizontalDatum.Ellipsoid.InverseFlattening;
                        double lat, lon, h;
                        Utilities.ConvertXYZToLLH(X, Y, Z, a, 1/f, out lat, out lon, out h);
                        return h;
                    }
                }
                return 0;
            }
        }

        [System.ComponentModel.DisplayName("Pt. Name")]
        public override string Name { get; set; }

        [System.ComponentModel.DisplayName("Latitude [DMS]")]
        public String LatDMS { get {
                if (this.CRS != null)
                {
                    ICoordinateSystem sys = (ICoordinateSystem)ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(this.CRS.WKT);
                    if (sys is GeographicCoordinateSystem)
                    {
                        return Utilities.ConvertDegToDMSString(this.Lat);
                    }
                }
                return "N/A";
            }
        }

        [System.ComponentModel.DisplayName("Longitude [DMS]")]
        public String LonDMS
        {
            get
            {
                if (this.CRS != null)
                {
                    ICoordinateSystem sys = (ICoordinateSystem)ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(this.CRS.WKT);
                    if (sys is GeographicCoordinateSystem)
                    {
                        return Utilities.ConvertDegToDMSString(this.Lon);
                    }
                }
                return "N/A";
            }
        }

        public GPointType PointType { get; set; }
        public GPointRole PointRole { get; set; }

        public GPoint(Project project, String name) : base(project)
        {
            this.Name = name;
            this.PointRole = GPointRole.Actived;
            this.PointType = GPointType.NA;
        }

        public double[] ConvertToVector()
        {
            return new double[] { this.X, this.Y, this.Z };
        }

        public override string ToString()
        {
            return Name;
        }

    }
}

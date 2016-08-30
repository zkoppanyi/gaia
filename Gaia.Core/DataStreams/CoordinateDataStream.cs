using Gaia.Processing;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DataStreams
{
    [Serializable]
    public class CoordinateDataStream : DataStream
    {
        protected CoordinateDataStream(Project project, string fileId) : base(project, fileId)
        {

        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new CoordinateDataStream(project, fileId);
            return stream;
        }

        protected override string extension
        {
            get { return "crd"; }
        }

        public override DataLine CreateDataLine()
        {
            return new CoordinateDataLine();
        }

        public GPoint ReadDataLineAsGPoint()
        {
            CoordinateDataLine line = base.ReadLine() as CoordinateDataLine;

            GPoint pt = new GPoint(this.project, "-1");
            pt.Description = "Convert point at CoordinateDataStream";
            pt.CRS = this.CRS;
            pt.PointRole = GPointRole.Deactivated;
            pt.PointType = GPointType.NA;

            CoordinateTransformer trans = new CoordinateTransformer(this.project, null);
            ICoordinateSystem sys = this.CRS.GetCoordinateSystem();

            if (sys is GeographicCoordinateSystem)
            {
                // TODO: Correct this
                //throw new NotImplementedException();

                pt.X = line.X;
                pt.Y = line.Y;
                pt.Z = line.Z;
                pt.Timestamp = line.TimeStamp;
                return pt;
            }
            else if (sys is GeocentricCoordinateSystem)
            {
                pt.X = line.X;
                pt.Y = line.Y;
                pt.Z = line.Z;
                pt.Timestamp = line.TimeStamp;
                return pt;
            }
            else if (sys is ProjectedCoordinateSystem)
            {
                pt.X = line.X;
                pt.Y = line.Y;
                pt.Z = line.Z;
                pt.Timestamp = line.TimeStamp;
                return pt;
                //throw new NotImplementedException();
            }

            return null;
        }
    }
}

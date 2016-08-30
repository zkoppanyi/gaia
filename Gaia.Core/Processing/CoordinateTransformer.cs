using Gaia.DataStreams;
using Gaia.GaiaSystem;
using Gaia.ReferenceFrames;
using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Processing
{
    /// <summary>
    /// Performing coordinate transformations
    /// </summary>
    public class CoordinateTransformer : Algorithm
    {        
        public CoordinateTransformer(Project project, IMessanger messanger) : base(project, messanger)
        {

        }

        /// <summary>
        /// Transform a GPoint set to another coordinate system.
        /// The algorithm overwrites the actual GPoint coordinates
        /// </summary>
        /// <param name="points"></param>
        /// <param name="crs"></param>
        /// <returns></returns>
        public AlgorithmResult Transform(IEnumerable<GPoint> points, CRS crs)
        {
            if ((points == null) || (points.Count<GPoint>() == 0))
            {
                WriteMessage("No points!");
                WriteProgress(100);
                return AlgorithmResult.InputMissing;
            }                               

            int numPoints = points.Count();
            int numLine = 0;
            foreach (GPoint pt in points)
            {
                if (IsCanceled())
                {
                    WriteMessage("Processing canceled.");
                    return AlgorithmResult.Failure;
                }

                if (pt.CRS == null)
                {
                    WriteMessage("The CRS for " + pt.Name + " point has not been specified!");
                    continue;
                }

                ICoordinateSystem fromCRS = pt.CRS.GetCoordinateSystem();
                ICoordinateSystem toCRS = crs.GetCoordinateSystem();

                WriteMessage("Perform coordinate transformation!");
                WriteMessage("Source CRS: " + fromCRS.Name);
                WriteMessage("Target CRS: " + toCRS.Name);

                transformPoint(fromCRS, toCRS, pt);               
                pt.CRS = crs;
                numLine++;
                WriteProgress((double)numLine/(double)numPoints*100.0);
            }

            return AlgorithmResult.Sucess;
        }

        /// <summary>
        /// Transform a Coordinate DataStream to another coordinate system.
        /// The algorithm overwrites DataStream coordinates
        /// </summary>
        /// <param name="dataStream"></param>
        /// <param name="crs"></param>
        /// <returns></returns>
        public AlgorithmResult Transform(CoordinateDataStream dataStream, CRS crs)
        {
            if (dataStream.CRS == null)
            {
                WriteMessage("The CRS for the DataStream has not been specified!");
                return AlgorithmResult.InputMissing;
            }

            if (!dataStream.IsOpen())
            {
                dataStream.Open();
            }
            dataStream.Begin();

            ICoordinateSystem fromCRS = dataStream.CRS.GetCoordinateSystem();
            ICoordinateSystem toCRS = crs.GetCoordinateSystem();

            WriteMessage("Perform coordinate transformation!");
            WriteMessage("Source CRS: " + fromCRS.Name);
            WriteMessage("Target CRS: " + toCRS.Name);

            long numLine = 0;
            while (!dataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    dataStream.Close();
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                long pos = dataStream.GetPosition();
                GPoint pt = dataStream.ReadDataLineAsGPoint();
                dataStream.Seek(pos);
                CoordinateDataLine line = dataStream.ReadLine() as CoordinateDataLine;
                dataStream.Seek(pos);

                transformPoint(fromCRS, toCRS, pt);
                line.X = pt.X;
                line.Y = pt.Y;
                line.Z = pt.Z;

                dataStream.ReplaceDataLine(line, pos);

                numLine++;
                WriteProgress((double)numLine / (double)dataStream.DataNumber * 100.0);
            }

            dataStream.CRS = crs;
            dataStream.Close();

            return AlgorithmResult.Sucess;
        }

        /// <summary>
        /// Transformation of a single point. 
        /// The function overwrite the pt object.
        /// </summary>
        /// <param name="fromCRS">Source frame</param>
        /// <param name="toCRS">Destination frame</param>
        /// <param name="pt">Point to transform</param>
        private void transformPoint(ICoordinateSystem fromCRS, ICoordinateSystem toCRS, GPoint pt)
        {
            double[] fromPoint = new double[0];

            if (fromCRS is GeographicCoordinateSystem)
            {
                fromPoint = new double[] { pt.Lon, pt.Lat, pt.H };
            }

            if (fromCRS is GeocentricCoordinateSystem)
            {
                throw new NotImplementedException();
                // the inputs are good?
                fromPoint = new double[] { pt.X, pt.Y, pt.Z };
            }

            if (fromCRS is ProjectedCoordinateSystem)
            {
                //throw new NotImplementedException();
                // the inputs are good?
                fromPoint = new double[] { pt.X, pt.Y, pt.Z };
            }

            CoordinateTransformationFactory ctfac = new CoordinateTransformationFactory();
            ICoordinateTransformation trans = ctfac.CreateFromCoordinateSystems(fromCRS, toCRS);

            double[] toPoint = trans.MathTransform.Transform(fromPoint);

            if (toCRS is GeographicCoordinateSystem)
            {
                // test wether it's good...
                GeographicCoordinateSystem toGCS = toCRS as GeographicCoordinateSystem;
                double x, y, z;
                Utilities.ConvertLLHToXYZ(toPoint[1], toPoint[0], toPoint[2], toGCS.HorizontalDatum.Ellipsoid.SemiMajorAxis, 1 / toGCS.HorizontalDatum.Ellipsoid.InverseFlattening, out x, out y, out z);
                pt.X = x; pt.Y = y; pt.Z = z;
            }

            if (toCRS is GeocentricCoordinateSystem)
            {
                pt.X = toPoint[0];
                pt.Y = toPoint[1];
                pt.Z = toPoint[2];
            }

            if (toCRS is ProjectedCoordinateSystem)
            {
                pt.X = toPoint[0];
                pt.Y = toPoint[1];
                pt.Z = toPoint[2];
            }

        }
    }
}

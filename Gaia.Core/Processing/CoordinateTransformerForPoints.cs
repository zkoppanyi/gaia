using ProjNet.CoordinateSystems;
using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core.DataStreams;
using Gaia.Core;
using Gaia.Core.ReferenceFrames;
using Gaia.Exceptions;

namespace Gaia.Core.Processing
{
    /// <summary>
    /// Performing coordinate transformations
    /// </summary>
    public class CoordinateTransformerForPoints : Algorithm
    {
        public IEnumerable<GPoint> Points;
        public CRS CRS;

        public static CoordinateTransformerForPointsFactory Factory
        {
            get
            {
                return new CoordinateTransformerForPointsFactory();
            }
        }

        public class CoordinateTransformerForPointsFactory : AlgorithmFactory
        {
            public String Name { get { return "Coordinate Transformer"; } }
            public String Description { get { return "Transformer the coordinates of point list."; } }

            public CoordinateTransformerForPoints Create(Project project, IEnumerable<GPoint> points, CRS crs)
            {
                CoordinateTransformerForPoints algorithm = new CoordinateTransformerForPoints(project, Name, Description);
                algorithm.Points = points;
                algorithm.CRS = crs;
                return algorithm;
            }
        }

        private CoordinateTransformerForPoints(Project project, String name, String description) : base(project, name, description)
        {
            Points = new List<GPoint>();
        }


        /// <summary>
        /// Transform a GPoint set to another coordinate system.
        /// The algorithm overwrites the actual GPoint coordinates
        /// </summary>
        /// <returns></returns>
        protected override AlgorithmResult run()
        {
            if ((Points == null) || (Points.Count<GPoint>() == 0))
            {
                WriteMessage("No points!");
                WriteProgress(100);
                return AlgorithmResult.InputMissing;
            }                               

            int numPoints = Points.Count();
            int numLine = 0;
            foreach (GPoint pt in Points)
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
                ICoordinateSystem toCRS = CRS.GetCoordinateSystem();

                WriteMessage("Perform coordinate transformation!");
                WriteMessage("Source CRS: " + fromCRS.Name);
                WriteMessage("Target CRS: " + toCRS.Name);

                Utilities.transformPoint(fromCRS, toCRS, pt);               
                pt.CRS = CRS;
                numLine++;
                WriteProgress((double)numLine/(double)numPoints*100.0);
            }

            return AlgorithmResult.Sucess;
        }
      

    }
}

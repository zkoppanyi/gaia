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
    public class CoordinateTransformerForDataStreams : Algorithm
    {
        public CoordinateDataStream SourceDataStream;
        public CRS CRS;

        public static CoordinateTransformerForDataStreamsFactory Factory
        {
            get
            {
                return new CoordinateTransformerForDataStreamsFactory();
            }
        }

        public class CoordinateTransformerForDataStreamsFactory : AlgorithmFactory
        {
            public String Name { get { return "Coordinate Transformer"; } }
            public String Description { get { return "Transformer the coordinates of a coordinate data stream."; } }

            public CoordinateTransformerForDataStreamsFactory() { }

            public CoordinateTransformerForDataStreams Create(Project project, IMessanger messanger, CoordinateDataStream sourceDataStream, CRS crs)
            {                
                CoordinateTransformerForDataStreams algorithm = new CoordinateTransformerForDataStreams(project, messanger, Name, Description);
                algorithm.SourceDataStream = sourceDataStream;
                algorithm.CRS = crs;
                return algorithm;
            }
        }

        private CoordinateTransformerForDataStreams(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {

        }
        
        
        /// <summary>
        /// Transform a Coordinate DataStream to another coordinate system.
        /// The algorithm overwrites DataStream coordinates
        /// </summary>
        /// <returns></returns>
        public override AlgorithmResult Run()
        {
            if (SourceDataStream.CRS == null)
            {
                WriteMessage("The CRS for the DataStream has not been specified!");
                return AlgorithmResult.InputMissing;
            }

            if (!SourceDataStream.IsOpen())
            {
                SourceDataStream.Open();
            }
            SourceDataStream.Begin();

            ICoordinateSystem fromCRS = SourceDataStream.CRS.GetCoordinateSystem();
            ICoordinateSystem toCRS = CRS.GetCoordinateSystem();

            WriteMessage("Perform coordinate transformation!");
            WriteMessage("Source CRS: " + fromCRS.Name);
            WriteMessage("Target CRS: " + toCRS.Name);

            long numLine = 0;
            while (!SourceDataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    SourceDataStream.Close();
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                long pos = SourceDataStream.GetPosition();
                GPoint pt = SourceDataStream.ReadDataLineAsGPoint();
                SourceDataStream.Seek(pos);
                CoordinateDataLine line = SourceDataStream.ReadLine() as CoordinateDataLine;
                SourceDataStream.Seek(pos);

                Utilities.transformPoint(fromCRS, toCRS, pt);
                line.X = pt.X;
                line.Y = pt.Y;
                line.Z = pt.Z;

                SourceDataStream.ReplaceDataLine(line, pos);

                numLine++;
                WriteProgress((double)numLine / (double)SourceDataStream.DataNumber * 100.0);
            }

            SourceDataStream.CRS = CRS;
            SourceDataStream.Close();

            return AlgorithmResult.Sucess;
        }

       
    }
}

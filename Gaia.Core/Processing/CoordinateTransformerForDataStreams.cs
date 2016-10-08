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
        private CoordinateDataStream sourceDataStream;
        public CoordinateDataStream SourceDataStream { get { return sourceDataStream; } }

        private CoordinateDataStream outputDataStream;
        public CoordinateDataStream OutputDataStream { get { return outputDataStream; } }

        private CRS _CRS;
        [System.ComponentModel.DisplayName("Target Coordinate Reference Frame")]
        public CRS TargetCRS { get { return _CRS; } }
        

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

            [System.ComponentModel.DisplayName("Target Coordinate Reference Frame")]
            public CRS TargetCRS { get; set; }

            public CoordinateTransformerForDataStreamsFactory() { }

            public CoordinateTransformerForDataStreams Create(Project project, CoordinateDataStream sourceDataStream, CoordinateDataStream outputDataStream,  CRS crs)
            {                
                CoordinateTransformerForDataStreams algorithm = new CoordinateTransformerForDataStreams(project, Name, Description);
                algorithm.sourceDataStream = sourceDataStream;
                algorithm.outputDataStream = outputDataStream;
                algorithm._CRS = crs;
                return algorithm;
            }
        }

        private CoordinateTransformerForDataStreams(Project project, String name, String description) : base(project, name, description)
        {

        }


        /// <summary>
        /// Transform a Coordinate DataStream to another coordinate system.
        /// The algorithm overwrites DataStream coordinates
        /// </summary>
        /// <returns></returns>
        protected override AlgorithmResult run()
        {
            if ((sourceDataStream == null) || (outputDataStream == null))
            {
                WriteMessage("Source or output data stream is not specified!");
                return AlgorithmResult.InputMissing;
            }


            if (sourceDataStream.CRS == null)
            {
                WriteMessage("The CRS for the DataStream has not been specified!");
                return AlgorithmResult.InputMissing;
            }

            if (!sourceDataStream.IsOpen())
            {
                sourceDataStream.Open();
            }
            sourceDataStream.Begin();

            outputDataStream.Open();
            outputDataStream.CRS = _CRS;
            sourceDataStream.SettingsCopyTo(outputDataStream);
            outputDataStream.Name = sourceDataStream.Name + " Transformed";
            outputDataStream.Description = sourceDataStream.Description + Environment.NewLine + " Original stream was transformed to " + _CRS.Name;

            ICoordinateSystem fromCRS = sourceDataStream.CRS.GetCoordinateSystem();
            ICoordinateSystem toCRS = _CRS.GetCoordinateSystem();

            WriteMessage("Perform coordinate transformation!");
            WriteMessage("Source CRS: " + fromCRS.Name);
            WriteMessage("Target CRS: " + toCRS.Name);

            long numLine = 0;
            while (!sourceDataStream.IsEOF())
            {
                if (IsCanceled())
                {
                    sourceDataStream.Close();
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                long pos = sourceDataStream.GetPosition();
                GPoint pt = sourceDataStream.ReadDataLineAsGPoint();
                sourceDataStream.Seek(pos);
                CoordinateDataLine line = sourceDataStream.ReadLine() as CoordinateDataLine;

                Utilities.transformPoint(fromCRS, toCRS, pt);
                line.X = pt.X;
                line.Y = pt.Y;
                line.Z = pt.Z;

                outputDataStream.AddDataLine(line);

                numLine++;
                WriteProgress((double)numLine / (double)sourceDataStream.DataNumber * 100.0);
            }

            sourceDataStream.Close();
            outputDataStream.Close();

            return AlgorithmResult.Sucess;
        }

       
    }
}

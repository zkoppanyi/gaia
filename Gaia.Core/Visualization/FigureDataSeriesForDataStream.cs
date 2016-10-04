using System;

using Gaia.Core.DataStreams;

namespace Gaia.Core.Visualization
{
    public class FigureDataSeriesForDataStream : FigureDataSeries
    {
        public DataStream DataStream { get; }

        public FigureDataSeriesForDataStream(String name) : this(name, null, "", "")
        {

        }

        public FigureDataSeriesForDataStream(String name, DataStream dataStream, String captionX, String captionY)
        {
            this.Name = name;
            this.DataStream = dataStream;
            this.CaptionX = captionX;
            this.CaptionY = captionY;
        }
    }
}

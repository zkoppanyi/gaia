using System;

using Gaia.Core.DataStreams;

namespace Gaia.Core.Visualization
{
    public class FigureDataSeries
    {
        public String Name { get; }
        public DataStream DataStream { get; }
        public String CaptionX { get; }
        public String CaptionY { get; }

        public FigureDataSeries(String name, DataStream dataStream, String captionX, String captionY)
        {
            this.Name = name;
            this.DataStream = dataStream;
            this.CaptionX = captionX;
            this.CaptionY = captionY;
        }
    }
}

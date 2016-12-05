using System;

using Gaia.Core.DataStreams;
using System.Drawing;
using System.ComponentModel;
using Gaia.Core.Processing;
using static Gaia.Core.Visualization.Figure;
using System.Collections.Generic;

namespace Gaia.Core.Visualization
{
    public class FigureDataSeriesForDataStream : FigureDataSeries
    {
        public DataStream DataStream { get; }
        public SolidBrush MarkerBrush = new SolidBrush(Color.Red);
        public int MarkerSize = 4;

        public FigureDataSeriesForDataStream(String name, DataStream dataStream, String captionX, String captionY)
        {
            this.Name = name;
            this.DataStream = dataStream;
            this.CaptionX = captionX;
            this.CaptionY = captionY;
        }
    }
}

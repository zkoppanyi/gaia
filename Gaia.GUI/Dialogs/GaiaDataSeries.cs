using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core.DataStreams;

namespace Gaia.GUI.Dialogs
{
    public class GaiaDataSeries
    {
        public String Name { get; }
        public DataStream DataStream { get; }
        public String CaptionX { get; }
        public String CaptionY { get; }

        public GaiaDataSeries(String name, DataStream dataStream, String captionX, String captionY)
        {
            this.Name = name;
            this.DataStream = dataStream;
            this.CaptionX = captionX;
            this.CaptionY = captionY;
        }
    }
}

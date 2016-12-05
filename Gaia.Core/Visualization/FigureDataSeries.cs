using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Visualization
{
    public abstract class FigureDataSeries
    {
        static int dataSeriesNo = 0;
        public String Name { get; set; }
        public String CaptionX { get; set; }
        public String CaptionY { get; set; }

        public FigureDataSeries()
        {
            dataSeriesNo++;
            this.Name = "Data Series " + dataSeriesNo;
            this.CaptionX = "";
            this.CaptionY = "";
        }
    }
}

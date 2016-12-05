using Gaia.Core.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.ComponentModel;

namespace Gaia.Core.Visualization
{
    public class FigureDataSearesForPoints : FigureDataSeries
    {
        private List<GPoint> ptList;
        public IReadOnlyCollection<GPoint> PointList { get { return ptList.AsReadOnly(); } }

        public FigureDataSearesForPoints(List<GPoint> pt)
        {
            this.ptList = pt;
        }

    }
}

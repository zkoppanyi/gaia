using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Visualization
{
    public abstract class FigureDataSeriesController
    {
        protected Figure figure;
        public Figure Figure { get { return figure; } }

        protected FigureDataSeries series;
        public FigureDataSeries Series { get { return series; } }

        public abstract void Draw(BackgroundWorker backgroundWorker);
        public abstract void Clear();

        public FigureDataSeriesController(Figure figure, FigureDataSeries series)
        {
            this.figure = figure;
            this.series = series;
        }
    }
}

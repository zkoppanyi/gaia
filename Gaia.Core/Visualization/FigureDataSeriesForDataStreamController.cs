﻿using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Gaia.Core.Visualization.Figure;

namespace Gaia.Core.Visualization
{
    public class FigureDataSeriesForDataStreamController : FigureDataSeriesController
    {
        public bool IsPreviewMode { get; set; }
        private List<Tuple<double, double>> points = new List<Tuple<double, double>>();

        public FigureDataSeriesForDataStreamController(Figure figure, FigureDataSeriesForDataStream series) : base(figure, series)
        {
            IsPreviewMode = false;
        }

        public override void Draw(BackgroundWorker backgroundWorker)
        {
            isLimitsChanged = false;
            points.Clear();
            FigureDataSeriesForDataStream dataStreamSeries = series as FigureDataSeriesForDataStream;
            DataStream dataStream = dataStreamSeries.DataStream;
            int under_sampling = Convert.ToInt32((double)dataStream.DataNumber / 20000.0) + 1;

            dataStream.Open();
            dataStream.Begin();
            int i = 0;
            int lastProgressReport = 0;
            long pos = 0;
            while (!dataStream.IsEOF())
            {
                if (backgroundWorker.CancellationPending)
                {
                    dataStream.Close();
                    isLimitsChanged = false;
                    return;
                }

                DataLine line = dataStream.ReadLine();
                int progress = Convert.ToInt32((double)dataStream.GetPosition() / (double)dataStream.DataNumber * 100);

                object valueXobj = Utilities.GetValueByDisplayNameAttribute(line, series.CaptionX);
                object valueYobj = Utilities.GetValueByDisplayNameAttribute(line, series.CaptionY);
                
                double valueX = Convert.ToDouble(valueXobj);
                double valueY = Convert.ToDouble(valueYobj);

                addPoint(valueX, valueY);

                // Preview mode                    
                if (IsPreviewMode == true)
                {
                    pos = dataStream.GetPosition() + under_sampling;
                    if (pos > dataStream.DataNumber) break;
                    dataStream.Seek(pos);
                    i++;
                }

                if ((progress % 10 == 0) && (lastProgressReport != progress)) // just report for the progress bar
                {
                    if ((isLimitsChanged == true) && (progress % 50 == 0))
                    {
                        isLimitsChanged = false;
                        dataStream.Close();
                        figure.Update();
                        return;
                    }

                    figure.SyncFigureEvent.Reset();
                    flushPoints();
                    backgroundWorker.ReportProgress(progress, null);
                    figure.SyncFigureEvent.WaitOne();
                }

                lastProgressReport = progress;


            }

            if (isLimitsChanged)
            {
                isLimitsChanged = false;
                dataStream.Close();
                figure.Update();
                return;
            }

            figure.SyncFigureEvent.Reset();
            flushPoints();
            backgroundWorker.ReportProgress(100);
            figure.SyncFigureEvent.WaitOne();
            dataStream.Close();
        }

        /// <summary>
        /// Draw a data point on the figure. The points are in world coordinates
        /// </summary>
        /// <param name="x">X in world corrdinate system</param>
        /// <param name="y">Y in world voordinate system</param>
        bool isLimitsChanged = false;
        private void addPoint(double wx, double wy)
        {
            if (wx > figure.XLimMax)
            {
                if (figure.IsFixedLimits == false)
                {
                    figure.XLimMax = wx;
                    isLimitsChanged = true;
                }
            }

            if (wx < figure.XLimMin)
            {
                if (figure.IsFixedLimits == false)
                {
                    figure.XLimMin = wx;
                    isLimitsChanged = true;
                }
            }

            if (wy > figure.YLimMax)
            {
                if (figure.IsFixedLimits == false)
                {
                    figure.YLimMax = wy;
                    isLimitsChanged = true;
                }
            }

            if (wy < figure.YLimMin)
            {
                if (figure.IsFixedLimits == false)
                {
                    figure.YLimMin = wy;
                    isLimitsChanged = true;
                }
            }

            if (figure.IsFixedLimits == true)
            {
                if (!isLimitsChanged)
                {
                    drawPoint(wx, wy);
                }
                isLimitsChanged = false;
            }

            if (figure.IsFixedLimits == false)
            {
                points.Add(new Tuple<double, double>(wx, wy));
                //drawPoint(wx, wy);
            }
        }

        private void flushPoints()
        {
            foreach(Tuple<double, double> p in points)
            {
                drawPoint(p.Item1, p.Item2);
            }
            points.Clear();
        }


        /// <summary>
        /// Draw a data point on the figure. The points are in world coordinates
        /// </summary>
        /// <param name="x">X in world corrdinate system</param>
        /// <param name="y">Y in world voordinate system</param>
        private void drawPoint(double wx, double wy)
        {
            lock (figure.BitmapLocker)
            {
                int ix = 0;
                int iy = 0;
                figure.WorldToImage(wx, wy, ref ix, ref iy);
                FigureDataSeriesForDataStream dataStreamSeries = series as FigureDataSeriesForDataStream;

                if ((ix >= 0) && (ix <= figure.FigureWidth) && (iy >= 0) && (iy < figure.FigureHeight))
                {
                    ix = ix - dataStreamSeries.MarkerSize / 2;
                    iy = iy - dataStreamSeries.MarkerSize / 2;
                    Point dPoint = new Point(ix, iy);
                    Rectangle rect = new Rectangle(dPoint, new Size(dataStreamSeries.MarkerSize, dataStreamSeries.MarkerSize));

                    Graphics g = Graphics.FromImage(figure.FigureBitmap);
                    g.FillEllipse(dataStreamSeries.MarkerBrush, rect);
                    g.Dispose();
                   
                }
            }
        }

        public override void Clear()
        {

        }
    }
}

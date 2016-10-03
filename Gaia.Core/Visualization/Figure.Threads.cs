﻿using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Threading;

namespace Gaia.Core.Visualization
{
    public delegate void FigureUpdatedEventHandler(object source, FigureUpdatedEventArgs e);

    public class FigureUpdatedEventArgs
    {
        private Figure figure;
        private int progress;
        private String msg;

        public FigureUpdatedEventArgs(Figure figure, int progress, String msg = null)
        {
            this.figure = figure;
            this.progress = progress;
            this.msg = msg;
        }

        public Figure Figure
        {
            get { return this.figure; }
        }

        public int Progress
        {
            get { return this.progress; }
        }

        public String Message
        {
            get { return this.msg; }
        }

    }
    public partial class Figure
    {

        private List<FigureDataSeries> dataSeriesList;
        private BackgroundWorker backgroundWorker;
        private bool isPreviewMode;
        private ManualResetEvent syncFigureEvent = new ManualResetEvent(false);

        public FigureUpdatedEventHandler FigureUpdated;
        public FigureUpdatedEventHandler FigureDone;
        public FigureUpdatedEventHandler FigureCancelled;
        public FigureUpdatedEventHandler PreviewLoaded;
        public FigureUpdatedEventHandler FigureError;


        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker.CancellationPending)
            {
                FigureUpdated?.Invoke(this, new FigureUpdatedEventArgs(this, e.ProgressPercentage));
            }
            syncFigureEvent.Set();
        }

        private void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (FigureDataSeries dataSeries in dataSeriesList)
            {
                DataStream dataStream = dataSeries.DataStream;
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
                        e.Cancel = true;
                        return;
                    }

                    DataLine line = dataStream.ReadLine();
                    int progress = Convert.ToInt32((double)dataStream.GetPosition() / (double)dataStream.DataNumber * 100);

                    object valueXobj = Utilities.GetValueByDisplayNameAttribute(line, dataSeries.CaptionX);
                    object valueYobj = Utilities.GetValueByDisplayNameAttribute(line, dataSeries.CaptionY);


                    double valueX = Convert.ToDouble(valueXobj);
                    double valueY = Convert.ToDouble(valueYobj);

                    addPoint(valueX, valueY);

                    // Preview mode                    
                    if (this.isPreviewMode == true)
                    {
                        pos = dataStream.GetPosition() + under_sampling;
                        if (pos > dataStream.DataNumber) break;
                        dataStream.Seek(pos);
                        i++;
                    }

                    if ((progress % 10 == 0) && (lastProgressReport != progress) && (isFigureUpdated == true)) // real report bitmap update
                    {
                        syncFigureEvent.Reset();
                        Redraw();
                        backgroundWorker.ReportProgress(progress);
                        syncFigureEvent.WaitOne();
                        isFigureUpdated = false;
                    }
                    else if ((progress % 5 == 0) && (lastProgressReport != progress)) // just report for the progress bar
                    {
                        syncFigureEvent.Reset();
                        backgroundWorker.ReportProgress(progress, null);
                        syncFigureEvent.WaitOne();
                    }

                    lastProgressReport = progress;


                }

                syncFigureEvent.Reset();
                backgroundWorker.ReportProgress(100);
                syncFigureEvent.WaitOne();
                dataStream.Close();
            }

            e.Cancel = false;
            return;
        }

        public void Cancel()
        {
            if ((this.backgroundWorker != null) && (this.backgroundWorker.IsBusy))
            {
                isUpdateSeriesRequired = false;
                backgroundWorker.CancelAsync();
            }
        }

        private bool isUpdateSeriesRequired = false;
        public void Update()
        {
            if (this.backgroundWorker.IsBusy)
            {
                Cancel();
                isUpdateSeriesRequired = true;
            }
            else
            {
                isPreviewMode = true;
                Clear();
                calculateLimits();
                backgroundWorker.RunWorkerAsync();
                syncFigureEvent.Reset();
            }

        }

        public bool IsBusy()
        {
            return backgroundWorker.IsBusy;
        }

        public void Update(int width, int height)
        {
            this.Cancel();

            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            figureBitmap = new Bitmap(width, height);
            createNewBitmap();
            this.Update();
        }

        public void ZoomByImageCoordinate(int x1, int y1, int x2, int y2)
        {
            //this.Cancel();
            calculateLimits();
            FPoint p1 = ImageToWord(x1, y1);
            FPoint p2 = ImageToWord(x2, y2);
            xLimMin = p1.X < p2.X ? p1.X : p2.X;
            xLimMax = p1.X > p2.X ? p1.X : p2.X;
            yLimMin = p1.Y < p2.Y ? p1.Y : p2.Y;
            yLimMax = p1.Y > p2.Y ? p1.Y : p2.Y;            
            isFixedLimits = true;
            calculateLimits();
            this.Update();
        }

        public void ShowExtent()
        {
            this.Cancel();
            isFixedLimits = false;
            this.Update();
        }

        private void backgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {

            if (e.Error != null)
            {
                FigureError?.Invoke(this, new FigureUpdatedEventArgs(this, 0, e.Error.ToString()));
                return;
            }

            if (!e.Cancelled)
            {
                //toolStripProgressBar.Visible = false;
                if (isPreviewMode == true)
                {
                    isPreviewMode = false;
                    syncFigureEvent.Reset();
                    PreviewLoaded?.Invoke(this, new FigureUpdatedEventArgs(this, 0, "Preview loaded!"));
                    backgroundWorker.RunWorkerAsync();
                }
                else
                {
                    FigureDone?.Invoke(this, new FigureUpdatedEventArgs(this, 100, "All data is loaded!"));
                }
            }
            else
            {
                FigureCancelled?.Invoke(this, new FigureUpdatedEventArgs(this, 100));
                if (isUpdateSeriesRequired)
                {
                    Update();
                }

            }

        }

    }
}

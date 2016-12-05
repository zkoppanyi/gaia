using Gaia.Core.DataStreams;
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

        private List<FigureDataSeriesController> dataSeriesControllerList;
        private BackgroundWorker backgroundWorker;
        private bool isPreviewMode;

        public FigureUpdatedEventHandler FigureUpdated;
        public FigureUpdatedEventHandler FigureDone;
        public FigureUpdatedEventHandler FigureCancelled;
        public FigureUpdatedEventHandler PreviewLoaded;
        public FigureUpdatedEventHandler FigureError;

        private ManualResetEvent syncFigureEvent = new ManualResetEvent(false);
        public ManualResetEvent SyncFigureEvent { get { return syncFigureEvent; } }


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
            foreach (FigureDataSeriesController dataSeriesController in dataSeriesControllerList)
            {
                if (backgroundWorker.CancellationPending)
                {
                    e.Cancel = true;
                    return;
                }

                if (dataSeriesController is FigureDataSeriesForDataStreamController)
                {
                    FigureDataSeriesForDataStreamController dataSeriesForDataStreamCtr = dataSeriesController as FigureDataSeriesForDataStreamController;
                    dataSeriesForDataStreamCtr.IsPreviewMode = isPreviewMode;
                }

                dataSeriesController.Draw(backgroundWorker);
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

        public bool IsBusy()
        {
            return backgroundWorker.IsBusy;
        }

        private bool isUpdateSeriesRequired = false;
        public void Update()
        {
            if (this.backgroundWorker.IsBusy)
            {
                //Cancel();
                isUpdateSeriesRequired = true;
                if (this.backgroundWorker.CancellationPending == false)
                {
                    backgroundWorker.CancelAsync();
                }
            }
            else
            {
                isUpdateSeriesRequired = false;
                isPreviewMode = true;
                calculateLimits();
                createNewBitmap();
                backgroundWorker.RunWorkerAsync();
                syncFigureEvent.Reset();
            }

        }

        public void Update(int width, int height)
        {
            this.Cancel();

            if (width <= 0) width = 1;
            if (height <= 0) height = 1;

            createNewBitmap(width, height);
            this.Update();
        }

        public void ZoomByImageCoordinate(int ix1, int iy1, int ix2, int iy2)
        {
            calculateLimits();
            double wx1 = 0, wy1 = 0, wx2 = 0, wy2 = 0;
            ImageToWord(ix1, iy1, ref wx1, ref wy1);
            ImageToWord(ix2, iy2, ref wx2, ref wy2);
            XLimMin = wx1 < wx2 ? wx1 : wx2;
            XLimMax = wx1 > wx2 ? wx1 : wx2;
            YLimMin = wy1 < wy2 ? wy1 : wy2;
            YLimMax = wy1 > wy2 ? wy1 : wy2;            
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
            }

            if (isUpdateSeriesRequired)
            {
                isUpdateSeriesRequired = false;
                Update();
            }
        }

    }
}

using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace Gaia.GUI.Dialogs
{

    public partial class FigureDlg : Form
    {
        private String captionName;
        public String CaptionName { get { return captionName;  } }

        private bool isPreviewMode;

        private BackgroundWorker backgroundWorker;
        private List<GaiaDataSeries> dataSeriesList;

        private ManualResetEvent syncFigureEvent = new ManualResetEvent(false);
        private FigureObject figure;

        public FigureDlg(String name)
        {
            InitializeComponent();
            toolStripAspectRatio.Items.Add(0.0);
            toolStripAspectRatio.Items.Add(1.0);
            toolStripAspectRatio.Items.Add(0.5);
            toolStripAspectRatio.Items.Add(0.25);
            toolStripAspectRatio.Items.Add(0.1);
            toolStripAspectRatio.Items.Add(0.01);
            toolStripAspectRatio.Items.Add(2.0);
            toolStripAspectRatio.Items.Add(5.0);
            toolStripAspectRatio.Items.Add(10.0);
            toolStripAspectRatio.Items.Add(100.0);

            this.dataSeriesList = new List<GaiaDataSeries>();
            this.captionName = name;
            this.figure = new FigureObject(figureBox.Width, figureBox.Height);
            this.isPreviewMode = true;

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_Completed);
        }

        public void AddDataSeries(GaiaDataSeries dataSerises)
        {
            this.dataSeriesList.Add(dataSerises);
        }

        private class ReportProgressMessage
        {
            public GaiaDataSeries DataSeries { get; }
            public FigureObject Figure { get; }

            public ReportProgressMessage(GaiaDataSeries series, FigureObject figure)
            {
                this.DataSeries = series;
                this.Figure = figure;
                figure.XLabel = series.CaptionX;
                figure.YLabel = series.CaptionY;
            }

        }

        public void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (GaiaDataSeries dataSeries in dataSeriesList)
            {
                DataStream dataStream = dataSeries.DataStream;
                int under_sampling = Convert.ToInt32((double)dataStream.DataNumber / 20000.0) + 1;

                dataStream.Open();
                dataStream.Begin();
                int i = 0;
                double prevNum = Double.NaN;
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

                    try
                    {
                        double valueX = Convert.ToDouble(valueXobj);
                        double valueY = Convert.ToDouble(valueYobj);

                        figure.AddPoint(valueX, valueY);

                        // Preview mode
                        pos = dataStream.GetPosition() + under_sampling;
                        if (this.isPreviewMode == true)
                        {
                            if (pos > dataStream.DataNumber) break;
                            dataStream.Seek(pos);
                            i++;
                        }

                        if ((progress % 10 == 0) && (lastProgressReport != progress))
                        {
                            syncFigureEvent.Reset();
                            figure.Redraw();
                            ReportProgressMessage msgl = new ReportProgressMessage(dataSeries, figure);
                            backgroundWorker.ReportProgress(progress, msgl);
                            syncFigureEvent.WaitOne();
                        }

                        else if ((progress % 2 == 0) && (lastProgressReport != progress))
                        {
                            syncFigureEvent.Reset();
                            backgroundWorker.ReportProgress(progress, null);
                            syncFigureEvent.WaitOne();
                        }
                        
                        prevNum = valueY;
                        lastProgressReport = progress;
                    }
                    catch (Exception ex)
                    {
                        //dlgProgress.Worker.Write("Cannot convert the column's values to double!", "Conversion error");
                        dataStream.Close();
                        return;
                    }
                }

                syncFigureEvent.Reset();
                figure.Redraw();
                ReportProgressMessage msg = new ReportProgressMessage(dataSeries, figure);
                backgroundWorker.ReportProgress(100, msg);
                syncFigureEvent.WaitOne();
                dataStream.Close();
            }

            e.Cancel = false;
            return;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!backgroundWorker.CancellationPending)
            {
                toolStripProgressBar.ProgressBar.BeginInvoke(new Action(() => toolStripProgressBar.Value = e.ProgressPercentage));
                if (e.UserState != null)
                {
                    ReportProgressMessage msg = e.UserState as ReportProgressMessage;
                    //figureBox.Invoke(new Action(() => figureBox.Image = msg.Figure.FigureBitmap));
                    figureBox.Image = msg.Figure.FigureBitmap;
                }
            }
            syncFigureEvent.Set();
        }

        private void FigureDlg_Load(object sender, EventArgs e)
        {
            this.Text = this.captionName;
            UpdateSeries();
        }

        public void CancelFigure()
        {
            //if ((this._backgroundWorker != null) && (this._backgroundWorker.IsBusy))
            if (this.backgroundWorker != null) 
            {
                backgroundWorker.CancelAsync();
            }
        }

        private bool isUpdateSeriesRequired = false;
        public void UpdateSeries()
        {
            if (this.backgroundWorker.IsBusy)
            {
                CancelFigure();
                isUpdateSeriesRequired = true;
            }
            else
            {
                toolStripProgressBar.Visible = true;
                toolStripCancelProgress.Visible = true;
                isPreviewMode = true;
                syncFigureEvent.Reset();
                figure.Clear();
                backgroundWorker.RunWorkerAsync();
            }

        }

        private void backgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled) 
            {
                //toolStripProgressBar.Visible = false;
                if (isPreviewMode == true)
                {
                    double[] lims = e.Result as double[];
                    isPreviewMode = false;
                    toolStripProgressBar.Visible = true;
                    toolStripCancelProgress.Visible = true;
                    syncFigureEvent.Reset();
                    backgroundWorker.RunWorkerAsync();
                    textBoxStatus.AppendText("Preview loaded!" + Environment.NewLine);
                }
                else
                {
                    toolStripProgressBar.Visible = false;
                    toolStripCancelProgress.Visible = false;
                    textBoxStatus.AppendText("All data is loaded!" + Environment.NewLine);
                }
            }
            else
            {
                if (!this.IsDisposed)
                {
                    isPreviewMode = true;
                    toolStripProgressBar.Visible = false;
                    toolStripCancelProgress.Visible = false;
                    textBoxStatus.AppendText("Data loading is cancelled!" + Environment.NewLine);

                    if(isUpdateSeriesRequired)
                    {
                        textBoxStatus.AppendText("Update is required!" + Environment.NewLine);
                        toolStripProgressBar.Visible = true;
                        toolStripCancelProgress.Visible = true;
                        syncFigureEvent.Reset();
                        figure.Clear();
                        backgroundWorker.RunWorkerAsync();
                        isUpdateSeriesRequired = false;
                    }
                }
            }

        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripCancelProgress_Click(object sender, EventArgs e)
        {
            CancelFigure();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
           
        }

        private void toolStripButtonRefresh_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            this.UpdateSeries();
        }

        private void aspectRatioEqualToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void legendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.legendToolStripMenuItem.Checked == true)
            {
                this.legendToolStripMenuItem.Checked = false; 
            }
            else
            {
                this.legendToolStripMenuItem.Checked = true;
            }

            this.UpdateSeries();
        }

        private void StatisticsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            CancelFigure();
            GlobalAccess.RemoveFigure(this);
        }

        bool sizing = false;
        private void FigureDlg_Resize(object sender, EventArgs e)
        {
            // ResizeEnd event is triggered, even of the window just moved, not resized.
            // To workaround, here, I use sizing variable 
            sizing = true;
        }

        private void FigureDlg_ResizeEnd(object sender, EventArgs e)
        {
            if (!sizing) return;

            if (sizing)
            {
                sizing = false;
                //CancelFigure();
                figure.SetNewBitmapSizeAndClear(figureBox.Width, figureBox.Height);
                isPreviewMode = true;
                this.UpdateSeries();
            }
        }

        private void aspectRatioEqualToolStripMenuItem_Click_1(object sender, EventArgs e)
        {
           
        }

        private void toolStripAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if(toolStripAspectRatio.Selected)
            {
                double ratio = (double)toolStripAspectRatio.SelectedItem;
                figure.AspectRatio = ratio;
                this.UpdateSeries();
            }
        }

        private void toolStripAspectRatio_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                double ratio = Convert.ToDouble(toolStripAspectRatio.Text);
                figure.AspectRatio = ratio;
                this.UpdateSeries();
            }
            catch
            {
                textBoxStatus.Text = "Invalid double value!";
            }
        }

        private void toolStripAspectRatio_Click(object sender, EventArgs e)
        {

        }

        private void relativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            relativeToolStripMenuItem.Checked = relativeToolStripMenuItem.Checked == true ? false : true;
            figure.IsRelative = relativeToolStripMenuItem.Checked;
            this.UpdateSeries();

        }
    }
}

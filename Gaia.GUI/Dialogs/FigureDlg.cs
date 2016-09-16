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
        private BackgroundWorker _backgroundWorker;
        private Statistics stat;
        private List<GaiaDataSeries> dataSeriesList;
        bool legendIsOn = true;
        ManualResetEvent syncEvent = new ManualResetEvent(false);
        FigureObject figure;

        public FigureDlg(String name)
        {
            InitializeComponent();
            this.dataSeriesList = new List<GaiaDataSeries>();
            this.captionName = name;
            this.figure = new FigureObject(figureBox.Width, figureBox.Height);
        }

        public void AddDataSeries(GaiaDataSeries dataSerises)
        {
            this.dataSeriesList.Add(dataSerises);
        }

        private class ReportProgressMessage
        {
            public GaiaDataSeries DataSeries { get; }
            public List<double[]> Points { get; }

            public ReportProgressMessage(GaiaDataSeries series, List<double[]> points, double minY, double maxY)
            {
                this.DataSeries = series;
                this.Points = points;
            }

        }

        private int backGroundWorkerMode = 0;
        private double miny = Double.PositiveInfinity, maxy = Double.NegativeInfinity;
        public void backgroundWorker_DoWork(object sender, DoWorkEventArgs e)
        {
            foreach (GaiaDataSeries dataSeries in dataSeriesList)
            {
                DataStream dataStream = dataSeries.DataStream;
                int under_sampling = Convert.ToInt32((double)dataStream.DataNumber / 20000.0) + 1;

                dataStream.Open();
                dataStream.Begin();
                List<double[]> points = new List<double[]>();
                int i = 0;
                double prevNum = Double.NaN;
                int lastProgressReport = 0;
                long pos = 0;
                while (!dataStream.IsEOF())
                {
                    if (_backgroundWorker.CancellationPending)
                    {
                        e.Cancel = true;
                        dataStream.Close();
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
                        if (backGroundWorkerMode == 0)
                        {
                            if (pos > dataStream.DataNumber) break;
                            dataStream.Seek(pos);
                            i++;
                        }

                        if ((progress % 10 == 0) && (lastProgressReport != progress))
                        {
                            syncEvent.Reset();
                            ReportProgressMessage msg = new ReportProgressMessage(dataSeries, points, miny, maxy);
                            _backgroundWorker.ReportProgress(progress, msg);
                            syncEvent.WaitOne();
                            points.Clear();
                        }
                        else if ((progress % 2 == 0) && (lastProgressReport != progress))
                        {
                            syncEvent.Reset();
                            _backgroundWorker.ReportProgress(progress, null);
                            syncEvent.WaitOne();

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

                syncEvent.Reset();
                _backgroundWorker.ReportProgress(100, points.ToArray());
                syncEvent.WaitOne();
                dataStream.Close();
            }

            e.Result = new double[] { miny, maxy };
            e.Cancel = false;
            return;
        }

        private void backgroundWorker_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            if (!_backgroundWorker.CancellationPending)
            {
                toolStripProgressBar.ProgressBar.BeginInvoke(new Action(() => toolStripProgressBar.Value = e.ProgressPercentage));
                if (e.UserState != null)
                {

                    figureBox.Invoke(new Action(() => figureBox.Image = figure.FigureBitmap));
                }
            }
            syncEvent.Set();
        }

        private void Statistics_Load(object sender, EventArgs e)
        {
            this.Text = this.captionName;
            stat = Statistics.Factory.Create(GlobalAccess.Project, null);
           
            UpdateSeries();

        }

        public void UpdateSeries()
        {
            if ((this._backgroundWorker != null)  && (this._backgroundWorker.IsBusy))
            {
                _backgroundWorker.CancelAsync();
            }

            backGroundWorkerMode = 0;

            toolStripProgressBar.Visible = true;
            toolStripCancelProgress.Visible = true;
            _backgroundWorker = new BackgroundWorker();
            _backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            _backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            _backgroundWorker.WorkerReportsProgress = true;
            _backgroundWorker.WorkerSupportsCancellation = true;
            _backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_Completed);
            syncEvent.Reset();
            _backgroundWorker.RunWorkerAsync();
        }

        ElementPosition ipp0 = null;
        private void aspectRatioEqual(Chart chart)
        {
            ChartArea ca = chart.ChartAreas[0];

            // store the original value:
            if (ipp0 == null) ipp0 = ca.InnerPlotPosition;

            // get the current chart area :
            ElementPosition cap = ca.Position;

            // get both area sizes in pixels:
            Size CaSize = new Size((int)(cap.Width * chart.ClientSize.Width / 100f),
                                    (int)(cap.Height * chart.ClientSize.Height / 100f));

            Size IppSize = new Size((int)(ipp0.Width * CaSize.Width / 100f),
                                    (int)(ipp0.Height * CaSize.Height / 100f));

            // we need to use the smaller side:
            int ippNewSide = Math.Min(IppSize.Width, IppSize.Height);

            // calculate the scaling factors
            float px = ipp0.Width / IppSize.Width * ippNewSide;
            float py = ipp0.Height / IppSize.Height * ippNewSide;

            // use one or the other:
            if (IppSize.Width < IppSize.Height)
                ca.InnerPlotPosition = new ElementPosition(ipp0.X, ipp0.Y, ipp0.Width, py);
            else
                ca.InnerPlotPosition = new ElementPosition(ipp0.X, ipp0.Y, px, ipp0.Height);

        }

        private void backgroundWorker_Completed(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                //toolStripProgressBar.Visible = false;
                if (backGroundWorkerMode == 0)
                {
                    double[] lims = e.Result as double[];
                    /*chartSeries.ChartAreas[0].AxisY.Minimum = lims[0];
                    chartSeries.ChartAreas[0].AxisY.Maximum = lims[1];
                    chartSeries.Update();*/

                    backGroundWorkerMode = 1;
                    toolStripProgressBar.Visible = true;
                    toolStripCancelProgress.Visible = true;
                    syncEvent.Reset();
                    _backgroundWorker.RunWorkerAsync();

                    refreshHistogram();
                    textBoxStatus.AppendText("Preview loaded!" + Environment.NewLine);
                }
                else
                {
                    double[] lims = e.Result as double[];
                    /*chartSeries.ChartAreas[0].AxisY.Minimum = lims[0];
                    chartSeries.ChartAreas[0].AxisY.Maximum = lims[1];
                    chartSeries.Update();*/

                    toolStripProgressBar.Visible = false;
                    toolStripCancelProgress.Visible = false;

                    refreshHistogram();
                    textBoxStatus.AppendText("All data is loaded!" + Environment.NewLine);
                }
            }
            else
            {
                if (!this.IsDisposed)
                {
                    toolStripProgressBar.Visible = false;
                    toolStripCancelProgress.Visible = false;
                    refreshHistogram();
                    textBoxStatus.AppendText("Data loading is cancelled! The histogram and the figure are not complete!" + Environment.NewLine);
                }
            }
        }

        private void refreshHistogram()
        {
            if (stat.Numbers.Count() != 0)
            {
                stat.Run();
                SortedList<double, double> histogram = stat.Histogram;
                chartHistogram.Series.Clear();
                chartHistogram.Series.Add("Histogram");
                foreach (double xp in histogram.Keys)
                {
                    double yp = histogram[xp];
                    //DataPoint dp = new DataPoint();
                    //dp.SetValueXY(xp, new double[] { yp });
                    chartHistogram.Series["Histogram"].Points.AddXY(xp, yp);
                }

                chartHistogram.ChartAreas[0].AxisX.LabelStyle.Format = "{0.00}";
                chartHistogram.ChartAreas[0].AxisY.LabelStyle.Format = "{0}";
                chartHistogram.ChartAreas[0].AxisX.LabelStyle.Font = new Font(FontFamily.GenericMonospace, 8);
                chartHistogram.ChartAreas[0].AxisY.LabelStyle.Font = new Font(FontFamily.GenericMonospace, 8);

                chartHistogram.ChartAreas[0].AxisX.Title = dataSeriesList[0].CaptionY;
                chartHistogram.ChartAreas[0].AxisY.Title = "Frequency [-]";
                chartHistogram.ChartAreas[0].AxisX.TitleFont = new Font(FontFamily.GenericMonospace, 8);
                chartHistogram.ChartAreas[0].AxisY.TitleFont = new Font(FontFamily.GenericMonospace, 8);

                chartHistogram.Update();
            }
        }

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        private void toolStripCancelProgress_Click(object sender, EventArgs e)
        {
            _backgroundWorker.CancelAsync();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            /*chartSeries.ChartAreas[0].AxisX.ScaleView.ZoomReset();
            chartSeries.ChartAreas[0].AxisY.ScaleView.ZoomReset();*/
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
            //aspectRatioEqual(chartSeries);
        }

        private void legendToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (this.legendToolStripMenuItem.Checked == true)
            {
                this.legendIsOn = false;
                this.legendToolStripMenuItem.Checked = false; 
            }
            else
            {
                this.legendIsOn = true;
                this.legendToolStripMenuItem.Checked = true;
            }

            this.UpdateSeries();
        }

        private void StatisticsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            _backgroundWorker.CancelAsync();
            GlobalAccess.RemoveFigure(this);
        }
    }
}

using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using Gaia.Core.Visualization;
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

        private bool closeWindowAfterCancellation = false;

        public FigureDlg(String name)
        {
            InitializeComponent();

            this.captionName = name;
            figureControl.FigureDone += new FigureUpdatedEventHandler(FigureDone);
            figureControl.PreviewLoaded += new FigureUpdatedEventHandler(PreviewLoaded);
            figureControl.FigureError += new FigureUpdatedEventHandler(FigureError);
            figureControl.FigureCancelled += new FigureUpdatedEventHandler(FigureCancelled);

        }

        private void FigureCancelled(object source, FigureUpdatedEventArgs e)
        {
           if (closeWindowAfterCancellation)
           {
                this.Close();
           }
        }

        private void FigureDone(object source, FigureUpdatedEventArgs e)
        {
            if (closeWindowAfterCancellation)
            {
                this.Close();
            }
        }

        private void PreviewLoaded(object source, FigureUpdatedEventArgs e)
        {
            if (closeWindowAfterCancellation)
            {
                this.Close();
            }
        }

        private void FigureError(object source, FigureUpdatedEventArgs e)
        {
            if (closeWindowAfterCancellation)
            {
                this.Close();
            }
        }

        public void AddDataSeries(FigureDataSeries dataSerises)
        {
            figureControl.AddDataSeries(dataSerises);
        }


        private void FigureDlg_Load(object sender, EventArgs e)
        {
            this.Text = this.captionName;
            figureControl.Update();
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            GlobalAccess.RemoveFigure(this);

            if (figureControl.IsBusy())
            {
                closeWindowAfterCancellation = true;
                figureControl.CancelFigure();
                e.Cancel = true;
                //textBoxStatus.AppendText("Waiting for the background thread for closing!" + Environment.NewLine);
                return;
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void StatisticsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
        }


    }
}

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

        private FigureObject figure;
        private bool closeWindowAfterCancellation = false;
        private bool isUserCancelled = true;

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

            this.captionName = name;
            this.figure = new FigureObject(figureBox.Width, figureBox.Height);
            figure.FigureDone += new FigureUpdatedEventHandler(FigureDone);
            figure.PreviewLoaded += new FigureUpdatedEventHandler(PreviewLoaded);
            figure.FigureUpdated += new FigureUpdatedEventHandler(FigureUpdated);
            figure.FigureError += new FigureUpdatedEventHandler(FigureError);
            figure.FigureCancelled += new FigureUpdatedEventHandler(FigureCancelled);

        }
        private void FigureCancelled(object source, FigureUpdatedEventArgs e)
        {
            if(closeWindowAfterCancellation)
            {
                this.Close();
                return;
            }

            figureBox.Image = e.Figure.FigureBitmap;
            toolStripProgressBar.ProgressBar.Value = e.Progress;
            textBoxStatus.AppendText("Cancelled" + Environment.NewLine);
            if (e.Message != null) textBoxStatus.AppendText(e.Message + Environment.NewLine);

            if (isUserCancelled)
            {
                toolStripProgressBar.Visible = false;
                toolStripCancelProgress.Visible = false;
            }
        }


        private void FigureDone(object source, FigureUpdatedEventArgs e)
        {
            figureBox.Image = e.Figure.FigureBitmap;
            toolStripProgressBar.ProgressBar.Value = e.Progress;
            if (e.Message != null) textBoxStatus.AppendText(e.Message + Environment.NewLine);
            toolStripProgressBar.Visible = false;
            toolStripCancelProgress.Visible = false;
        }

        private void PreviewLoaded(object source, FigureUpdatedEventArgs e)
        {
            toolStripProgressBar.Visible = true;
            toolStripCancelProgress.Visible = true;
            toolStripProgressBar.ProgressBar.Value = e.Progress;
            figureBox.Image = e.Figure.FigureBitmap;
            if (e.Message != null) textBoxStatus.AppendText(e.Message + Environment.NewLine);
        }

        private void FigureUpdated(object source, FigureUpdatedEventArgs e)
        {
            toolStripProgressBar.ProgressBar.Value = e.Progress;
            figureBox.Image = e.Figure.FigureBitmap;
            if (e.Message != null) textBoxStatus.AppendText(e.Message + Environment.NewLine);
        }

        private void FigureError(object source, FigureUpdatedEventArgs e)
        {
            toolStripProgressBar.ProgressBar.Value = e.Progress;
            figureBox.Image = e.Figure.FigureBitmap;
            if (e.Message != null) textBoxStatus.AppendText("Error: " + e.Message + Environment.NewLine);
        }

        public void AddDataSeries(GaiaDataSeries dataSerises)
        {
            figure.AddDataSeries(dataSerises);
        }


        private void FigureDlg_Load(object sender, EventArgs e)
        {
            this.Text = this.captionName;
            figure.Update();
        }
      

        private void toolStripSplitButton1_ButtonClick(object sender, EventArgs e)
        {

        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            if (figure.IsBusy())
            {
                closeWindowAfterCancellation = true;
                figure.Cancel();
                e.Cancel = true;
                textBoxStatus.AppendText("Waiting for the background thread for closing!" + Environment.NewLine);
                return;
            }
            else
            {
                base.OnFormClosing(e);
            }
        }

        private void toolStripCancelProgress_Click(object sender, EventArgs e)
        {

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
            figure.Update();
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

            figure.Update();

        }

        private void StatisticsDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            figure.Cancel();
            closeWindowAfterCancellation = true;
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
                toolStripProgressBar.Visible = true;
                toolStripCancelProgress.Visible = true;
                isUserCancelled = false;
                figure.Update(figureBox.Width, figureBox.Height);
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
                figure.Update();

            }
        }

        private void toolStripAspectRatio_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                double ratio = Convert.ToDouble(toolStripAspectRatio.Text);
                figure.AspectRatio = ratio;
                figure.Update();
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
            figure.Update();
        }
    }
}

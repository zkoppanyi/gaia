using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gaia.Core.Visualization;

namespace Gaia.Dialogs
{
    /// <summary>
    /// Wrapper class for UI from Figure class from the Core assembly
    /// </summary>
    public partial class FigureControl : UserControl
    {
        private Figure figureObject;

        public FigureUpdatedEventHandler FigureUpdated;
        public FigureUpdatedEventHandler FigureDone;
        public FigureUpdatedEventHandler FigureCancelled;
        public FigureUpdatedEventHandler PreviewLoaded;
        public FigureUpdatedEventHandler FigureError;

        public FigureControl()
        {
            InitializeComponent();
            this.BackgroundImageLayout = ImageLayout.Stretch;

            figureObject = new Figure(figureArea.Width, figureArea.Height);
            figureObject.FigureDone += new FigureUpdatedEventHandler(OnFigureDone);
            figureObject.PreviewLoaded += new FigureUpdatedEventHandler(OnPreviewLoaded);
            figureObject.FigureUpdated += new FigureUpdatedEventHandler(OnFigureUpdated);
            figureObject.FigureError += new FigureUpdatedEventHandler(OnFigureError);
            figureObject.FigureCancelled += new FigureUpdatedEventHandler(OnFigureCancelled);

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
        }

        public void AddDataSeries(FigureDataSeries dataSeries)
        {
            figureObject.AddDataSeries(dataSeries);
        }

        public void UpdateFigure()
        {
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            figureObject.Update();
        }

        public void UpdateFigure(int width, int height)
        {
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            figureObject.Update(width, height);
        }

        public void CancelFigure()
        {
            figureObject.Cancel();
        }

        public bool IsBusy()
        {
            return figureObject.IsBusy();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            if (figureObject != null)
            {
                progressBar.Visible = true;
                toolStripButtonCancelProgress.Visible = true;
                figureObject.Update(figureArea.Width, figureArea.Height);
            }
        }

        protected void OnFigureDone(object source, FigureUpdatedEventArgs e)
        {
            this.figureArea.Image = e.Figure.FigureBitmap;
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureDone?.Invoke(source, e);
        }

        protected void OnPreviewLoaded(object source, FigureUpdatedEventArgs e)
        {
            this.figureArea.Image = e.Figure.FigureBitmap;
            progressBar.Value = e.Progress;
            PreviewLoaded?.Invoke(source, e);
        }

        protected void OnFigureUpdated(object source, FigureUpdatedEventArgs e)
        {
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            this.figureArea.Image = e.Figure.FigureBitmap;
            progressBar.Value = e.Progress;
            FigureUpdated?.Invoke(source, e);

        }

        protected void OnFigureError(object source, FigureUpdatedEventArgs e)
        {
            this.figureArea.Image = e.Figure.FigureBitmap;
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureError?.Invoke(source, e);

        }

        protected void OnFigureCancelled(object source, FigureUpdatedEventArgs e)
        {
            this.figureArea.Image = e.Figure.FigureBitmap;
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureCancelled?.Invoke(source, e);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            figureObject.Update();
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

            figureObject.Update();
        }

        private void toolStripButtonCancelProgress_Click(object sender, EventArgs e)
        {
            figureObject.Cancel();
        }

        private void relativeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            relativeToolStripMenuItem.Checked = relativeToolStripMenuItem.Checked == true ? false : true;
            figureObject.IsRelative = relativeToolStripMenuItem.Checked;
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            figureObject.Update();
        }

        private void toolStripAspectRatio_TextUpdate(object sender, EventArgs e)
        {
            try
            {
                double ratio = Convert.ToDouble(toolStripAspectRatio.Text);
                figureObject.AspectRatio = ratio;
                figureObject.Update();
            }
            catch
            {
                // Invalid aspect ratio
            }
        }

        private void toolStripAspectRatio_Click(object sender, EventArgs e)
        {

        }

        private void toolStripAspectRatio_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (toolStripAspectRatio.Selected)
            {
                double ratio = (double)toolStripAspectRatio.SelectedItem;
                figureObject.AspectRatio = ratio;
                progressBar.Visible = true;
                toolStripButtonCancelProgress.Visible = true;
                figureObject.Update();

            }
        }

        private void figureArea_Click(object sender, EventArgs e)
        {

        }
    }
}

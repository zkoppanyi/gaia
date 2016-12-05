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
using static Gaia.Core.Visualization.Figure;

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

        private bool ZoomModeOn;
        Point mouseFirstPoint;
        Point mouseLastPoint;

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

            ZoomModeOn = false;
        }



        public void AddDataSeries(FigureDataSeries dataSeries)
        {
            if (dataSeries is FigureDataSeriesForDataStream)
            {
                FigureDataSeriesForDataStream dataSeriesForDataStream = dataSeries as FigureDataSeriesForDataStream;
                FigureDataSeriesController controller = new FigureDataSeriesForDataStreamController(figureObject, dataSeriesForDataStream);
                figureObject.AddDataSeriesController(controller);
                figureObject.Update();
            }
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

            if (ZoomModeOn)
            {
                if ((mouseFirstPoint.X != mouseLastPoint.X) || (mouseFirstPoint.Y != mouseLastPoint.Y))
                {
                    Graphics g = figureArea.CreateGraphics();
                    Pen p = new Pen(new SolidBrush(Color.Red));
                    int x = mouseFirstPoint.X < mouseLastPoint.X ? mouseFirstPoint.X : mouseLastPoint.X;
                    int y = mouseFirstPoint.Y < mouseLastPoint.Y ? mouseFirstPoint.Y : mouseLastPoint.Y;
                    int width = Math.Abs(mouseFirstPoint.X - mouseLastPoint.X);
                    int height = Math.Abs(mouseFirstPoint.Y - mouseLastPoint.Y);
                    g.DrawRectangle(p, x, y, width, height);
                }
            }
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
        }

        protected void OnFigureDone(object source, FigureUpdatedEventArgs e)
        {
            figureArea.Image = figureObject.FigureBitmap;
            this.Refresh();
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureDone?.Invoke(source, e);
        }

        protected void OnPreviewLoaded(object source, FigureUpdatedEventArgs e)
        {
            figureArea.Image = figureObject.FigureBitmap;
            this.Refresh();
            progressBar.Value = e.Progress;
            PreviewLoaded?.Invoke(source, e);
        }

        protected void OnFigureUpdated(object source, FigureUpdatedEventArgs e)
        {
            figureArea.Image = (Image)figureObject.FigureBitmap.Clone();
            this.Refresh();
            progressBar.Visible = true;
            toolStripButtonCancelProgress.Visible = true;
            progressBar.Value = e.Progress;
            FigureUpdated?.Invoke(source, e);

        }

        protected void OnFigureError(object source, FigureUpdatedEventArgs e)
        {
            figureArea.Image = figureObject.FigureBitmap;
            this.Refresh();
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureError?.Invoke(source, e);

        }

        protected void OnFigureCancelled(object source, FigureUpdatedEventArgs e)
        {
            figureArea.Image = figureObject.FigureBitmap;
            this.Refresh();
            progressBar.Value = e.Progress;
            progressBar.Visible = false;
            toolStripButtonCancelProgress.Visible = false;
            FigureCancelled?.Invoke(source, e);
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            figureArea.Image = figureObject.FigureBitmap;
            this.Refresh();
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


        private void toolStripButtonZoomIn_Click(object sender, EventArgs e)
        {
            ZoomModeOn = true;
        }

        private void figureArea_MouseDoubleClick(object sender, MouseEventArgs e)
        {
        }

        private void figureArea_MouseMove(object sender, MouseEventArgs e)
        {
            if (ZoomModeOn)
            {
                if (e.Button == MouseButtons.Left)
                {
                    double wx = 0, wy = 0;
                    int ix = 0, iy = 0;
                    figureObject.ImageToWord(e.X, e.Y, ref wx, ref wy);
                    figureObject.WorldToImage(wx, wy, ref ix, ref iy);

                    mouseLastPoint.X = ix;
                    mouseLastPoint.Y = iy;
                    this.Refresh();

                }
            }
        }

        private void figureArea_MouseDown(object sender, MouseEventArgs e)
        {
            if (ZoomModeOn)
            {
                if (e.Button == MouseButtons.Left)
                {
                    double wx = 0, wy = 0;
                    int ix = 0, iy = 0;
                    figureObject.ImageToWord(e.X, e.Y, ref wx, ref wy);
                    figureObject.WorldToImage(wx, wy, ref ix, ref iy);
                    mouseFirstPoint.X = ix;
                    mouseFirstPoint.Y = iy;
                    mouseLastPoint.X = ix;
                    mouseLastPoint.Y = iy;
                    this.Update();
                }
            }
        }

        private void figureArea_MouseUp(object sender, MouseEventArgs e)
        {
            if (ZoomModeOn)
            {
                if (e.Button == MouseButtons.Left)
                {
                    figureObject.ZoomByImageCoordinate(mouseFirstPoint.X, mouseFirstPoint.Y, mouseLastPoint.X, mouseLastPoint.Y);
                    mouseFirstPoint.X = 0;
                    mouseFirstPoint.Y = 0;
                    mouseLastPoint.X = 0;
                    mouseLastPoint.Y = 0;
                    this.Refresh();                    
                }
            }
        }

        private void figureArea_Paint(object sender, PaintEventArgs e)
        {

        }

        private void toolStripButtonZoomExtent_Click(object sender, EventArgs e)
        {
            figureObject.ShowExtent();
        }

        private void figureArea_Resize(object sender, EventArgs e)
        {
           
        }

        private void figureArea_SizeChanged(object sender, EventArgs e)
        {
            resizeTimer.Stop();
            resizeTimer.Start();
        }

        private void figureArea_Click(object sender, EventArgs e)
        {
           
        }

        private void FigureControl_SizeChanged(object sender, EventArgs e)
        {           
            
        }

        private void FigureControl_Click(object sender, EventArgs e)
        {
           
        }

        private void resizeTimer_Tick(object sender, EventArgs e)
        {
            resizeTimer.Stop();
            if (figureObject != null)
            {
                progressBar.Visible = true;
                toolStripButtonCancelProgress.Visible = true;
                figureObject.Update(figureArea.Width, figureArea.Height);
            }
        }
    }
}

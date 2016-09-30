using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using Gaia.Exceptions;
using Gaia.GUI.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaia.GUI
{
    public partial class FigureObject
    {

        private SolidBrush brush = new SolidBrush(Color.LimeGreen);
        private Pen axisPen = new Pen(Color.Black);
        private int markerSize = 4;
        private Color backGroundColor = Color.White;
        private int axisEdgeOuterX = 100;
        private int axisEdgeOuterY = 40;
        private int axisEdgeInnerX = 0;
        private int axisEdgeInnerY = 0;
        private int tickSize = 6;


        SolidBrush tickLabelBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        StringFormat tickLabelFormat = new System.Drawing.StringFormat();
        Font tickLabelFont = new System.Drawing.Font("Courier New", 8);
        String tickLabelXFormat = "F1";
        String tickLabelYFormat = "F1";

        SolidBrush labelBrush = new System.Drawing.SolidBrush(System.Drawing.Color.Black);
        StringFormat labelFormat = new System.Drawing.StringFormat();
        Font labelFont = new System.Drawing.Font("Courier New", 12, FontStyle.Bold);

        public String XLabel;
        public String YLabel;

        private double xLimMin;
        public double XLimMin { get { return xLimMin; } }
        private double xLimMax;
        public double XLimMax { get { return xLimMax; } }

        private double yLimMin;
        public double YLimMin { get { return yLimMin; } }
        private double yLimMax;
        public double YLimMax { get { return yLimMax; } }

        private static Bitmap figureBitmap;
        public Bitmap FigureBitmap
        {
            get
            {
                return new Bitmap(figureBitmap);
            }
        }

        public double AspectRatio { get; set; }
        public bool IsRelative { get; set; }

        List<FPoint> points = new List<FPoint>();

        public FigureObject(int width, int height)
        {
            figureBitmap = new Bitmap(width, height);
            XLabel = "X [-]";
            YLabel = "Y [-]";
            createNewBitmap();
            this.Clear();

            backgroundWorker = new BackgroundWorker();
            backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
            backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
            backgroundWorker.WorkerReportsProgress = true;
            backgroundWorker.WorkerSupportsCancellation = true;
            backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_Completed);

            this.isPreviewMode = true;
            this.dataSeriesList = new List<GaiaDataSeries>();
        }

        public void AddDataSeries(GaiaDataSeries dataSerises)
        {
            this.dataSeriesList.Add(dataSerises);
        }

        public void Clear()
        {
            xLimMin = Double.PositiveInfinity;
            xLimMax = Double.NegativeInfinity;
            yLimMin = Double.PositiveInfinity;
            yLimMax = Double.NegativeInfinity;
            createNewBitmap();
            points.Clear();
        }

        bool hasToRedraw = false;
        public void AddPoint(double x, double y)
        {
            if (x > XLimMax)
            {
                xLimMax = x;
                hasToRedraw = true;
            }

            if (x < XLimMin)
            {
                xLimMin = x;
                hasToRedraw = true;
            }

            if (y > YLimMax)
            {
                yLimMax = y;
                hasToRedraw = true;
            }

            if (y < YLimMin)
            {
                yLimMin = y;
                hasToRedraw = true;
            }

            FPoint picP = WorldToImage(x, y);
            points.Add(new FPoint(x, y));

            drawPoint(picP.X, picP.Y);

            /*if ((picP.X >= 0) && (picP.X < figureBitmap.Width) && (picP.Y >= 0) && (picP.Y < figureBitmap.Height))
            {
                Color c = figureBitmap.GetPixel((int)picP.X, (int)picP.Y);
                if (figureBitmap.GetPixel((int)picP.X, (int)picP.Y).ToArgb() == this.backGroundColor.ToArgb())
                {
                  points.Add(new FPoint(x, y));
                }
            }*/
            //}

        }

        public void Redraw()
        {
            if (hasToRedraw)
            {

                this.axisEdgeOuterY = (int)tickLabelFont.Size * 2 + (int)labelFont.Size + (int)tickLabelFont.Size;

                this.axisEdgeOuterX = Math.Max(YLimMax.ToString(tickLabelYFormat).Length * (int)tickLabelFont.Size,
                               YLimMin.ToString(tickLabelYFormat).Length * (int)tickLabelFont.Size) + (int)labelFont.Size;

                if (AspectRatio != 0)
                {
                    double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
                    double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
                    double ratFigEffectiveX = (FigureBitmap.Width - 2 * this.axisEdgeOuterX - 2 * this.axisEdgeInnerX);
                    double ratFigEffectiveY = (FigureBitmap.Height - 2 * this.axisEdgeOuterY - 2 * this.axisEdgeInnerY);

                    double extraRoomX = ratFigEffectiveX - ratFigEffectiveY / dy * dx * AspectRatio;
                    double extraRoomY = ratFigEffectiveY - ratFigEffectiveX / dx * dy * AspectRatio;

                    if (extraRoomX > extraRoomY)
                    {
                        this.axisEdgeOuterX += (int)(extraRoomX / 2);
                    }
                    else
                    {
                        this.axisEdgeOuterY += (int)(extraRoomY / 2);
                    }

                }

                createNewBitmap();
                foreach (FPoint pt in points)
                {
                    drawPoint(pt.X, pt.Y);
                }
                hasToRedraw = false;
            }
        }

        private void createNewBitmap()
        {
            figureBitmap = new Bitmap(figureBitmap.Width, FigureBitmap.Height);
            Graphics g = Graphics.FromImage(figureBitmap);
            g.Clear(this.backGroundColor);
            drawAxis(g);
            g.Flush();
            g.Dispose();
        }

        private FPoint WorldToImage(double x, double y)
        {
            double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
            double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
            double ratX = (double)(figureBitmap.Width - axisEdgeOuterX*2 - axisEdgeInnerX*2) / dx;
            double ratY = (double)(figureBitmap.Height - axisEdgeOuterY*2 - axisEdgeInnerY*2) / dy;

            int picX = (int)((x - XLimMin) * ratX) + axisEdgeOuterX + axisEdgeInnerX;
            int picY = figureBitmap.Height - (int)((y - YLimMin) * ratY) - axisEdgeOuterY - axisEdgeInnerY;
            return new FPoint(picX, picY);
        }

        private FPoint ImageToWord(int x, int y)
        {
            double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
            double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
            double ratX = (double)(figureBitmap.Width - axisEdgeOuterX * 2 - axisEdgeInnerX * 2) / dx;
            double ratY = (double)(figureBitmap.Height - axisEdgeOuterY * 2 - axisEdgeInnerY * 2) / dy;

            double wrdX = ((double)(x - axisEdgeOuterX - axisEdgeInnerX) / ratX) + XLimMin;
            double wrdY = ((double)(-y + figureBitmap.Height + axisEdgeOuterY + axisEdgeInnerY) / ratY) + YLimMin;
            return new FPoint(wrdX, wrdY);
        }

        private void drawAxis(Graphics g)
        {

            int textLengthX = Math.Max(XLimMax.ToString(tickLabelXFormat).Length * (int)tickLabelFont.Size,
                           XLimMin.ToString(tickLabelXFormat).Length * (int)tickLabelFont.Size);

            double divNX = 5;
            double divNY = 5;

            Point origin = new Point(axisEdgeOuterX, figureBitmap.Height - axisEdgeOuterY);
            Point pX = new Point(figureBitmap.Width - axisEdgeOuterX, figureBitmap.Height - axisEdgeOuterY);
            Point pY = new Point(axisEdgeOuterX, axisEdgeOuterY);

            double rX = Math.Sqrt(Math.Pow(pX.X - origin.X, 2) + Math.Pow(pX.Y - origin.Y, 2));
            double rY = Math.Sqrt(Math.Pow(pY.X - origin.X, 2) + Math.Pow(pY.Y - origin.Y, 2));

            divNX = Math.Ceiling(rX / (double)textLengthX-3);
            divNX = divNX == 0 ? 1 : divNX;
            divNY = Math.Min(divNX, tickLabelFont.Size+3);
            divNY = divNY == 0 ? 1 : divNY;

            // Rounding the spacing
            double ddx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
            double ddy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
            if (Double.IsPositiveInfinity(ddx) || Double.IsNegativeInfinity(ddx)) return;

            double digitsX = getDigits(ddx / divNX);
            double digitsY = getDigits(ddy / divNY);
            double dX = Math.Floor(ddx / divNX / digitsX) * digitsX;
            double dY = Math.Floor(ddy / divNY / digitsY) * digitsY;

            // Drawing 
            g.DrawLine(axisPen, origin, pX);
            g.DrawLine(axisPen, origin, pY);

            for (double i = XLimMin; i <= XLimMax; i += dX)
            {
                FPoint ptTickX = new FPoint(i, YLimMin);
                FPoint tickX = this.WorldToImage(ptTickX.X, ptTickX.Y);
                g.DrawLine(axisPen, new Point((int)tickX.X, (int)tickX.Y - tickSize/2), new Point((int)tickX.X, (int)tickX.Y + tickSize / 2));
                if (IsRelative) ptTickX.X = ptTickX.X - XLimMin;

                String label = ptTickX.X.ToString(tickLabelXFormat);
                int labelWidth = label.Length * (int)tickLabelFont.Size;
                int labelHeight = (int)tickLabelFont.Size;
                g.DrawString(label, tickLabelFont, tickLabelBrush, (int)tickX.X - labelWidth/2, (int)tickX.Y + tickSize , tickLabelFormat);
            }

            g.DrawString(XLabel, labelFont, tickLabelBrush, (int)(origin.X + rX / 2)-XLabel.Length*labelFont.Size/2, origin.Y + tickSize + tickLabelFont.Size*2, labelFormat);

            int tickLabelWidth = 0;
            for (double i = YLimMin; i <= YLimMax; i += dY)
            {
                FPoint ptTickY = new FPoint(XLimMin, i);
                FPoint tickY = this.WorldToImage(ptTickY.X, ptTickY.Y);
                g.DrawLine(axisPen, new Point((int)tickY.X - tickSize / 2, (int)tickY.Y ), new Point((int)tickY.X + tickSize / 2, (int)tickY.Y ));
                if (IsRelative) ptTickY.Y = ptTickY.Y - YLimMin;

                String label = ptTickY.Y.ToString(tickLabelYFormat);
                tickLabelWidth = label.Length * (int)tickLabelFont.Size;
                int labelHeight = (int)tickLabelFont.Size;
                g.DrawString(label, tickLabelFont, tickLabelBrush, (int)tickY.X - tickLabelWidth, (int)tickY.Y - labelHeight, tickLabelFormat);
            }

            g.DrawString(YLabel, labelFont, tickLabelBrush, (int)pY.X - tickLabelWidth, (int)(pY.Y - tickSize - tickLabelFont.Size * 2));

        }

        private double getDigits(double val)
        {
            double digits = 1;
            val = Math.Abs(val);
            if (val > 0.0001) digits = 0.0001;
            if (val > 0.001) digits = 0.001;
            if (val > 0.01) digits = 0.01;
            if (val > 0.1) digits = 0.1;
            if (val > 10.0) digits = 10;
            if (val > 100.0) digits = 100;
            if (val > 1000.0) digits = 1000;
            if (val > 10000.0) digits = 10000;

            return digits;

        }
        private void drawPoint(double x, double y)
        {
            // just for safety, altough: assert
            if (figureBitmap == null)
            {
                createNewBitmap();
            }

            FPoint picP = WorldToImage(x, y);

            if ((picP.X >= 0) && (picP.X <= figureBitmap.Width) && (picP.Y >= 0) && (picP.Y < figureBitmap.Height))
            { 
                Point dPoint = new Point((int)picP.X, (int)picP.Y);

                dPoint.X = dPoint.X - markerSize / 2;
                dPoint.Y = dPoint.Y - markerSize / 2;
                Rectangle rect = new Rectangle(dPoint, new Size(markerSize, markerSize));

                Graphics g = Graphics.FromImage(figureBitmap);
                g.FillEllipse(brush, rect);
                g.Dispose();
            }
        }

        #region FPoint class

        private class FPoint
        {
            public double X { get; set; }
            public double Y { get; set; }

            public FPoint(double X, double Y)
            {
                this.X = X;
                this.Y = Y;
            }

            public override bool Equals(System.Object obj)
            {
                // If parameter is null return false.
                if (obj == null)
                {
                    return false;
                }

                // If parameter cannot be cast to Point return false.
                FPoint p = obj as FPoint;
                if ((System.Object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (X == p.X) && (Y == p.Y);
            }

            public bool Equals(FPoint p)
            {
                // If parameter is null return false:
                if ((object)p == null)
                {
                    return false;
                }

                // Return true if the fields match:
                return (X == p.X) && (Y == p.Y);
            }

            public override int GetHashCode()
            {
                return (int)(X * Y);
            }
        }

        #endregion

    }
   

}

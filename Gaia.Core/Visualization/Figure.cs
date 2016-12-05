using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;


namespace Gaia.Core.Visualization
{
    public partial class Figure
    {

        private Color backGroundColor = Color.White;
        private int axisEdgeOuterX = 100;
        private int axisEdgeOuterY = 40;
        private int axisEdgeInnerX = 0;
        private int axisEdgeInnerY = 0;
        private int tickSize = 6;


        private Pen axisPen = new Pen(Color.Black);
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

        public double XLimMin { get; set; }

        public double XLimMax { get; set; }

        public double YLimMin { get; set; }

        public double YLimMax { get; set; }

        private bool isFixedLimits = false;
        public bool IsFixedLimits { get { return isFixedLimits;  } }

        public readonly object BitmapLocker = new object();

        private static Bitmap figureBitmap;

        private int figureWidth;
        public int FigureWidth { get { return figureWidth;  } }

        private int figureHeight;
        public int FigureHeight { get { return figureHeight; } }

        public Bitmap FigureBitmap
        {
            get
            {
                lock (BitmapLocker)
                {
                    return figureBitmap;
                }
            }
        }

        public double AspectRatio { get; set; }
        public bool IsRelative { get; set; }        

        public Figure(int width, int height)
        {
            lock (BitmapLocker)
            {
                XLabel = "X [-]";
                YLabel = "Y [-]";
                createNewBitmap(width, height);

                this.dataSeriesControllerList = new List<FigureDataSeriesController>();
                this.Clear();

                backgroundWorker = new BackgroundWorker();
                backgroundWorker.DoWork += new DoWorkEventHandler(backgroundWorker_DoWork);
                backgroundWorker.ProgressChanged += new ProgressChangedEventHandler(backgroundWorker_ProgressChanged);
                backgroundWorker.WorkerReportsProgress = true;
                backgroundWorker.WorkerSupportsCancellation = true;
                backgroundWorker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(backgroundWorker_Completed);

                this.isPreviewMode = true;
            } 
        }


        public void AddDataSeriesController(FigureDataSeriesController dataSeriesController)
        {
            this.dataSeriesControllerList.Add(dataSeriesController);
        }

        public void Clear()
        {
            if (!isFixedLimits)
            {
                XLimMin = Double.PositiveInfinity;
                XLimMax = Double.NegativeInfinity;
                YLimMin = Double.PositiveInfinity;
                YLimMax = Double.NegativeInfinity;
            }
            createNewBitmap();

            foreach(FigureDataSeriesController seriesController in this.dataSeriesControllerList)
            {
                seriesController.Clear();
            }
        }


        private void calculateLimits()
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

            if ((this.axisEdgeOuterX < 0) || (this.axisEdgeOuterX > 1000000))
            {
                this.axisEdgeOuterX = 0;
            }

            if ((this.axisEdgeOuterY < 0) || (this.axisEdgeOuterY > 1000000))
            {
                this.axisEdgeOuterY = 0;
            }
        }

        private void createNewBitmap()
        {
            createNewBitmap(figureWidth, figureHeight);
        }

        private void createNewBitmap(int width, int height)
        {
            lock (BitmapLocker)
            {
                figureWidth  = width;
                figureHeight = height;
                figureBitmap = new Bitmap(width, height);
                Graphics g = Graphics.FromImage(figureBitmap);
                g.Clear(this.backGroundColor);
                drawAxis(g);
                g.Flush();
                g.Dispose();
            }
        }

        public void WorldToImage(double x, double y, ref int ix, ref int iy)
        {
                double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
                double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
                double ratX = (double)(figureWidth - axisEdgeOuterX * 2 - axisEdgeInnerX * 2) / dx;
                double ratY = (double)(figureHeight - axisEdgeOuterY * 2 - axisEdgeInnerY * 2) / dy;

                ix = (int)((x - XLimMin) * ratX) + axisEdgeOuterX + axisEdgeInnerX;
                iy = figureHeight - (int)((y - YLimMin) * ratY) - axisEdgeOuterY - axisEdgeInnerY;
        }


        public void ImageToWord(int x, int y, ref double wx, ref double wy)
        {
            double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
            double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
            double ratX = (double)(figureWidth - axisEdgeOuterX * 2 - axisEdgeInnerX * 2) / dx;
            double ratY = (double)(figureHeight - axisEdgeOuterY * 2 - axisEdgeInnerY * 2) / dy;

            wx = ((double)(x - axisEdgeOuterX - axisEdgeInnerX) / ratX) + XLimMin;
            wy = ((double)-(y - figureHeight + axisEdgeOuterY + axisEdgeInnerY) / ratY) + YLimMin;
        }

        private void drawAxis(Graphics g)
        {
            lock (BitmapLocker)
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

                divNX = Math.Ceiling(rX / (double)textLengthX - 3);
                divNX = divNX == 0 ? 1 : divNX;
                divNY = Math.Min(divNX, tickLabelFont.Size + 3);
                divNY = divNY == 0 ? 1 : divNY;

                // Rounding the spacing
                double ddx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 1;
                double ddy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 1;
                if (Double.IsPositiveInfinity(ddx) || Double.IsNegativeInfinity(ddx)) return;

                double digitsX = getDigits(ddx / divNX);
                double digitsY = getDigits(ddy / divNY);
                double dX = Math.Floor(ddx / divNX / digitsX) * digitsX;
                double dY = Math.Floor(ddy / divNY / digitsY) * digitsY;

                if (dX <= 0) dX = XLimMax - XLimMin;
                if (dY <= 0) dY = YLimMax - YLimMin;

                // Drawing 
                g.DrawLine(axisPen, origin, pX);
                g.DrawLine(axisPen, origin, pY);

                for (double i = XLimMin; i <= XLimMax; i += dX)
                {
                    double wxTickX = i;
                    double wyTickX = YLimMin;
                    int ixTickX = 0;
                    int iyTickX = 0;

                    this.WorldToImage(wxTickX, wyTickX, ref ixTickX, ref iyTickX);
                    g.DrawLine(axisPen, new Point((int)ixTickX, (int)iyTickX - tickSize / 2), new Point((int)ixTickX, (int)iyTickX + tickSize / 2));
                    if (IsRelative) wxTickX = wxTickX - XLimMin;

                    String label = wxTickX.ToString(tickLabelXFormat);
                    int labelWidth = label.Length * (int)tickLabelFont.Size;
                    int labelHeight = (int)tickLabelFont.Size;
                    g.DrawString(label, tickLabelFont, tickLabelBrush, (int)ixTickX - labelWidth / 2, (int)iyTickX + tickSize, tickLabelFormat);
                }

                g.DrawString(XLabel, labelFont, tickLabelBrush, (int)(origin.X + rX / 2) - XLabel.Length * labelFont.Size / 2, origin.Y + tickSize + tickLabelFont.Size * 2, labelFormat);

                int tickLabelWidth = 0;
                for (double i = YLimMin; i <= YLimMax; i += dY)
                {
                    double wxTickY = XLimMin;
                    double wyTickY = i;
                    int ixTickY = 0;
                    int iyTickY = 0;

                    this.WorldToImage(wxTickY, wyTickY, ref ixTickY, ref iyTickY);
                    g.DrawLine(axisPen, new Point((int)ixTickY - tickSize / 2, (int)iyTickY), new Point((int)ixTickY + tickSize / 2, (int)iyTickY));
                    if (IsRelative) wyTickY = wyTickY - YLimMin;

                    String label = wyTickY.ToString(tickLabelYFormat);
                    tickLabelWidth = label.Length * (int)tickLabelFont.Size;
                    int labelHeight = (int)tickLabelFont.Size;
                    g.DrawString(label, tickLabelFont, tickLabelBrush, (int)ixTickY - tickLabelWidth, (int)iyTickY - labelHeight, tickLabelFormat);
                }

                g.DrawString(YLabel, labelFont, tickLabelBrush, (int)pY.X - tickLabelWidth, (int)(pY.Y - tickSize - tickLabelFont.Size * 2));
            }

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
        

        #region FPoint class

        public class FPoint
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

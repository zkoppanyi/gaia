using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaia.GUI.Dialogs
{
    public class FigureObject
    {
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

        SolidBrush brush = new SolidBrush(Color.LimeGreen);
        int markerSize = 4;
        Color backGroundColor = Color.White;

        public double XLimMin { get; set; }
        public double XLimMax { get; set; }
        public double YLimMin { get; set; }
        public double YLimMax { get; set; }

        private Bitmap figureBitmap;
        public Bitmap FigureBitmap { get { return new Bitmap(figureBitmap);  } }

        List<FPoint> points = new List<FPoint>();

        public FigureObject(int width, int height)
        {
            figureBitmap = new Bitmap(width, height);
            createNewBitmap();

            this.XLimMin = Double.PositiveInfinity;
            this.XLimMax = Double.NegativeInfinity;
            this.YLimMin = Double.PositiveInfinity;
            this.YLimMax = Double.NegativeInfinity;
        }

        bool hasToRedraw = false;
        public void AddPoint(double x, double y)
        {
            if (x > XLimMax)
            {
                XLimMax = x;
                hasToRedraw = true;
            }

            if (x < XLimMin)
            {
                XLimMin = x;
                hasToRedraw = true;
            }

            if (y > YLimMax)
            {
                YLimMax = y;
                hasToRedraw = true;
            }

            if (y < YLimMin)
            {
                YLimMin = y;
                hasToRedraw = true;
            }

            FPoint picP = WorldToImage(x, y);
            points.Add(new FPoint(x, y));
            /*if ((picP.X >= 0) && (picP.X < figureBitmap.Width) && (picP.Y >= 0) && (picP.Y < figureBitmap.Height))
            {
                Color c = figureBitmap.GetPixel((int)picP.X, (int)picP.Y);
                if (figureBitmap.GetPixel((int)picP.X, (int)picP.Y).ToArgb() == this.backGroundColor.ToArgb())
                {
                  points.Add(new FPoint(x, y));
                }
            }*/
            //}

            if (points.Count % 100 == 0)
            {
                Redraw();
            }
            else
            {
                //drawPoint(x, y);
            }


        }

        private void Redraw()
        {
            createNewBitmap();
            foreach (FPoint pt in points)
            {
                drawPoint(pt.X, pt.Y);
            }
        }

        private void createNewBitmap()
        {
            figureBitmap = new Bitmap(figureBitmap.Width, figureBitmap.Height);
            Graphics g = Graphics.FromImage(figureBitmap);
            g.Clear(this.backGroundColor);
            g.Flush();
            g.Dispose();
        }

        private FPoint WorldToImage(double x, double y)
        {
            double dx = (XLimMax - XLimMin) != 0 ? XLimMax - XLimMin : 10;
            double dy = (YLimMax - YLimMin) != 0 ? YLimMax - YLimMin : 10;
            double ratX = (double)figureBitmap.Width / dx;
            double ratY = (double)figureBitmap.Height / dy;
            int picX = (int)((x - XLimMin) * ratX);
            int picY = (int)((y - YLimMin) * ratY);
            return new FPoint(picX, picY);
        }

        private void drawPoint(double x, double y)
        {
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

    }
}

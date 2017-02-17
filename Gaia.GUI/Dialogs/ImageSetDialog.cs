using Accord.Imaging;
using Accord.Imaging.Filters;
using Gaia.Core.DataStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaia.GUI.Dialogs
{
    public partial class ImageSetDialog : Form
    {
        public ImageDataStream ImageDataStream { get; }
        private Size imageSize;

        public ImageSetDialog(ImageDataStream imageDataStream)
        {
            InitializeComponent();
            ImageDataStream = imageDataStream;
            imageSize = new Size(200, 200);
        }

        private void ImageSetDialog_Load(object sender, EventArgs e)
        {
            RefreshImageList();
        }

        public void RefreshImageList()
        {
            List<ImageDataLine> dataLines = new List<ImageDataLine>();
            ImageDataStream.Open();
            this.imageList.Images.Clear();

            if (imageSize.Width > 255) imageSize.Width = 255;
            if (imageSize.Height > 255) imageSize.Height = 255;
            if (imageSize.Width < 1) imageSize.Width = 1;
            if (imageSize.Height < 1) imageSize.Height = 1;

            this.imageList.ImageSize = imageSize;
            while (!ImageDataStream.IsEOF())
            {
                ImageDataLine dataLine = ImageDataStream.ReadLine() as ImageDataLine;
                if (File.Exists(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName))
                {                    
                    Bitmap img = new Bitmap(System.Drawing.Image.FromFile(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName));

                    List<FastRetinaKeypoint> fastPoints = ImageDataStream.LoadFastKeypoints(dataLine);

                    if (fastPoints != null)
                    {
                        FeaturesMarker features = new FeaturesMarker(fastPoints);
                        img = features.Apply(img);
                    }

                    this.imageList.Images.Add(img);                    
                }
                else
                {
                    Bitmap notFound = new Bitmap(Gaia.Properties.Resources.imagenotfound);
                    this.imageList.Images.Add(notFound);
                }
                dataLines.Add(dataLine);
            }
            ImageDataStream.Close();

            this.listView.Clear();
            this.listView.View = View.LargeIcon;
            this.listView.LargeImageList = this.imageList;

            //or
            //this.listView1.View = View.SmallIcon;
            //this.listView1.SmallImageList = this.imageList1;

            for (int j = 0; j < this.imageList.Images.Count; ++j)
            {
                ListViewItem item = new ListViewItem();
                item.Text = dataLines[j].ImageFileName;
                item.ImageIndex = j;
                this.listView.Items.Add(item);
            }
        }

        private void listView_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void calculateSURFKeypointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
 
        }

        private void calculateFASTKeypointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImageDataStream.Open();
            this.imageList.Images.Clear();
            while (!ImageDataStream.IsEOF())
            {
                ImageDataLine dataLine = ImageDataStream.ReadLine() as ImageDataLine;
                if (File.Exists(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName))
                {
                    Bitmap img = new Bitmap(System.Drawing.Image.FromFile(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName));

                    /*float threshold = 0.0002f;
                    int octaves = 5;
                    int initial = 2;*/

                    int threshold = 10;
                    FastRetinaKeypointDetector freakDetector = new FastRetinaKeypointDetector(threshold);

                    try
                    {
                        List<FastRetinaKeypoint> points = freakDetector.ProcessImage(img);      
                                         
                        //ImageDataStream.SaveKeypoints(dataLine, points);

                    }
                    catch (Exception ex)
                    {
                        // TODO
                    }

                    this.imageList.Images.Add(img);
                }
                else
                {
                    // TODO
                }

            }
            ImageDataStream.Close();
            RefreshImageList();

        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            imageSize.Width -= 20;
            imageSize.Height -= 20;
            RefreshImageList();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            imageSize.Width += 20;
            imageSize.Height += 20;
            RefreshImageList();
        }

        private void calcaulteSURFKeypointsToolStripMenuItem_Click(object sender, EventArgs e)
        {
           
        }
    }
}

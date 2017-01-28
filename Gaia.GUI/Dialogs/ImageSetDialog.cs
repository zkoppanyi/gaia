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

        public ImageSetDialog(ImageDataStream imageDataStream)
        {
            InitializeComponent();
            ImageDataStream = imageDataStream;
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
            this.imageList.ImageSize = new Size(200, 200);
            while (!ImageDataStream.IsEOF())
            {
                ImageDataLine dataLine = ImageDataStream.ReadLine() as ImageDataLine;
                if (File.Exists(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName))
                {

                    Bitmap img = new Bitmap(System.Drawing.Image.FromFile(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName));

                    float threshold = 0.0002f;
                    int octaves = 5;
                    int initial = 2;

                    // Create a new SURF Features Detector using the given parameters
                    FastRetinaKeypointDetector surf =
                        new FastRetinaKeypointDetector();

                    try
                    {
                        List<FastRetinaKeypoint> points = surf.ProcessImage(img);

                        // Create a new AForge's Corner Marker Filter
                        FeaturesMarker features = new FeaturesMarker(points);

                        // Apply the filter and display it on a picturebox
                        img = features.Apply(img);

                        this.imageList.Images.Add(img);

                    } catch(Exception ex)
                    {
                        // TODO
                    }

                    
                }
                else
                {
                    Bitmap notFound = new Bitmap(Gaia.Properties.Resources.imagenotfound);
                    this.imageList.Images.Add(notFound);
                }
                dataLines.Add(dataLine);
            }
            ImageDataStream.Close();

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
    }
}

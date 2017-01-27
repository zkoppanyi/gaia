using Gaia.Core.DataStreams;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
            this.imageList.ImageSize = new Size(150, 150);
            while (!ImageDataStream.IsEOF())
            {
                ImageDataLine dataLine = ImageDataStream.ReadLine() as ImageDataLine;
                this.imageList.Images.Add(Image.FromFile(ImageDataStream.ImageFolder + "\\" + dataLine.ImageFileName));
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
    }
}

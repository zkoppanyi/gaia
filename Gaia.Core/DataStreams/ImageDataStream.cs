using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public sealed class ImageDataStream : DataStream
    {
        public String ImageFolder { get { return this.project.GetDataStreamFolder() + "\\" + fileId + "_IMAGES"; } }

        private ImageDataStream(Project project, string fileId) : base(project, fileId)
        {

        }

        protected override string extension
        {
            get
            {
                return "ims";
            }
        }

        public override DataLine CreateDataLine()
        {
            return new ImageDataLine();
        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new ImageDataStream(project, fileId);
            return stream;
        }

        public override DataLine ReadLine()
        {
            ImageDataLine dataLine = (ImageDataLine)base.ReadLine();

            if (dataLine != null)
            {
                string[] currentImageFiles = Directory.GetFiles(this.ImageFolder, "*.jpg");
                if ((new List<String>(currentImageFiles)).Exists(x => Path.GetFileName(x) == dataLine.ImageFileName))
                {
                    dataLine.SetIsAvailable(true);
                }
                else
                {
                    dataLine.SetIsAvailable(false);
                }
            }

            return dataLine;
        }

        protected internal override void Drop()
        {
            base.Drop();
            this.Close();
            if (Directory.Exists(this.ImageFolder))
            {
                Directory.Delete(this.ImageFolder, true);
            }
        }

        public void RefreshImageList()
        {
            string[] currentImageFiles = Directory.GetFiles(this.ImageFolder, "*.jpg");

            List<ImageDataLine> currentFileNames = new List<ImageDataLine>();
            this.Open();
            while(!this.IsEOF())
            {
                ImageDataLine dataLine = (ImageDataLine)this.ReadLine();
                currentFileNames.Add(dataLine);
            }

            this.Close();

            this.Open();
            foreach(String fileLong in currentImageFiles)
            {
                String file = Path.GetFileName(fileLong);

                if (!currentFileNames.Exists(x => x.ImageFileName == file))
                {
                    ImageDataLine newDataLine = new ImageDataLine();
                    newDataLine.ImageFileName = file;
                    this.AddDataLine(newDataLine);
                }
            }
            this.Close();

            project.Save();

        }
    }
}

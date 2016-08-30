using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.ComponentModel;

using Gaia.Exceptions;
using Gaia.Core;
using Gaia.Core.ReferenceFrames;
using Gaia.Core.Import;


namespace Gaia.Core.DataStreams
{
    public enum DataStreamType
    {
        NoDataStream,
        CoordinateDataStream,
        CoordinateAttitudeDataStream,
        IMUDataStream,
        UWBDataStream,
        GPSLogDataStream
    }

    [Serializable]
    public abstract class DataStream : GaiaSpatialObject
    {
        protected double firstTimeStamp;
        protected double lastTimeStamp;

        protected long dataNumber;
        protected bool isTimestampOrdered;
        protected bool isDropped = false;
        public abstract DataLine CreateDataLine();

        protected String fileId;

        [NonSerialized]
        protected BinaryWriter writer;

        [NonSerialized]
        protected BinaryReader reader;

        [NonSerialized]
        protected FileStream fileStream;

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [Description("Number of data line contained by the data stream.")]
        [DisplayName("Number of the data")]
        public long DataNumber { get { return dataNumber; } }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [Description("The timestamp of the first data.")]
        [DisplayName("First timestamp [s]")]
        public double FirstTimeStamp { get { return firstTimeStamp; } }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [Description("The timestamp of the last data.")]
        [DisplayName("Last timestamp [s]")]
        public double LastTimeStamp { get { return lastTimeStamp; } }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Data")]
        [Description("True, if the timestamps of the data are in increasing order, false otherwise.")]
        [DisplayName("Ordered timestamp ")]
        public bool IsTimeStampOrdered { get { return isTimestampOrdered; } }

        [NonSerialized]
        protected IMessanger messanger;

        protected abstract String extension { get; }

        protected DataStream(Project project, String fileId) : base(project)
        {
            if (!project.DataStreamManager.DataStreamCreateToken)
            {
                throw new GaiaAssertException("DataStream object can be created only through a Project object!");
            }

            this.firstTimeStamp = 0;
            this.lastTimeStamp = 0;
            this.dataNumber = 0;
            this.isTimestampOrdered = true;
            this.fileStream = null;
            this.fileId = fileId;
            this.messanger = null;
        }

        protected void WriteMessage(String message)
        {
            if (messanger != null)
            {
                messanger.Write(message);
            }
        }

        protected void WriteProgress(double percent)
        {
            if (messanger != null)
            {
                messanger.Progress(percent);
            }
        }

        private void checkIsDropped()
        {
            if (isDropped)
            {
                throw new GaiaException("The data stream has been dropped!");
            }
        }

        private String getStreamFile()
        {
            checkIsDropped();
            return project.GetDataStreamFolder() + "\\" + fileId + "." + this.extension;
        }

        public void Open(IMessanger messanger = null)
        {
            checkIsDropped();
            if (fileStream != null)
            {
                this.Close();
            }

            fileStream = File.Open(this.getStreamFile(), FileMode.OpenOrCreate, FileAccess.ReadWrite);
            writer = new BinaryWriter(fileStream);
            reader = new BinaryReader(fileStream);
            this.messanger = messanger;
        }

        public void Close()
        {
            checkIsDropped();
            if (fileStream != null)
            {
                fileStream.Close();
                fileStream = null;
            }

            this.messanger = null;
        }

        public bool IsOpen()
        {
            checkIsDropped();
            return this.fileStream == null ? false : true;
        }

        public virtual void AddDataLine(DataLine dataLine)
        {
            checkIsDropped();
            if (!this.IsOpen())
            {
                throw new GaiaException("The Data Stream has not been opened");
            }
            else
            {
                this.writeDataLine(dataLine);
            }
        }

        public void Begin()
        {
            fileStream.Seek(0, SeekOrigin.Begin);
        }

        public long GetPosition()
        {
            return fileStream.Position / this.CreateDataLine().LineSize();
        }

        public virtual void ReplaceDataLine(DataLine dataLine, long position)
        {
            checkIsDropped();
            if (!this.IsOpen())
            {
                throw new GaiaException("The Data Stream has not been opened");
            }
            else
            {
                this.replaceDataLine(dataLine, position);
            }
        }

        private void replaceDataLine(DataLine dataLine, long position)
        {
            checkIsDropped();
            if (dataLine == null) return;

            // Maintain timestamp order flag
            if (position!=0)
            {
                this.Seek(position-1);
                DataLine line = this.ReadLine();
                if (line.TimeStamp > dataLine.TimeStamp)
                {
                    isTimestampOrdered = false;
                }
            }

            if (position != fileStream.Length)
            {
                this.Seek(position + 1);
                DataLine line = this.ReadLine();
                if (line != null)
                {
                    if (line.TimeStamp < dataLine.TimeStamp)
                    {
                        isTimestampOrdered = false;
                    }
                }
            }

            this.Seek(position);
            dataLine.WriteLine(writer);
        }

        public void Last()
        {
            fileStream.Seek(fileStream.Length, SeekOrigin.Begin);
        }

        protected void checkTimestamps(DataLine dataLine)
        {
            if (dataNumber == 0)
            {
                firstTimeStamp = dataLine.TimeStamp;
            }
            else
            {
                if (dataLine.TimeStamp < lastTimeStamp)
                {
                    isTimestampOrdered = false;
                    this.WriteMessage("TimeStamp is not increasing! Ts: " + dataLine.TimeStamp + " Idx: " + dataLine.Index);
                    //throw new GaiaException("The timestamps are not increasing in the dataset!");
                }
            }

            lastTimeStamp = dataLine.TimeStamp;
        }


        private void writeDataLine(DataLine dataLine)
        {
            checkIsDropped();
            Last();
            checkTimestamps(dataLine);

            dataLine.Index = this.dataNumber;
            this.dataNumber++;

            dataLine.WriteLine(writer);
        }


        /// <summary>
        /// Drop external resources connected to the data stream. 
        /// The object cannot be used afterwards. The caller has to clean all references.
        /// </summary>
        internal void Drop()
        {
            checkIsDropped();
            this.Close();
            if (File.Exists(this.getStreamFile()))
            {
                File.Delete(this.getStreamFile());
            }
        }


        public virtual DataLine ReadLine()
        {
            checkIsDropped();
            if (!this.IsOpen())
            {
                throw new GaiaException("The Data Stream has not been opened");
            }
            else
            {
                return this.readDataLine();
            }
        }

        public void UpdateOrderFlag()
        {
            if (this.DataNumber == 0)
            {
                this.isTimestampOrdered = true;
                return;               
            }

            long n = this.GetPosition();

            this.Begin();
            DataLine prevLine = this.ReadLine();

            while (!this.IsEOF())
            {
                DataLine line = this.ReadLine();
                if (line.TimeStamp < prevLine.TimeStamp)
                {
                    this.isTimestampOrdered = false;
                    return;
                }
                prevLine = line;
            }

            this.isTimestampOrdered = true;
        }


        public void Seek(long numData)
        {
            checkIsDropped();
            fileStream.Seek(numData * this.CreateDataLine().LineSize(), 0);
        }

        /// <summary>
        /// Always set the data to an element that has equal or the smallest greater timestamp.
        /// </summary>
        /// <param name="ts"></param>
        public void SeekByTimestamp(double ts)
        {
            // TODO: logarithmic search...
            checkIsDropped();

            if (ts < this.firstTimeStamp)
            {
                this.Begin();
                return;
            }

            if (ts > this.lastTimeStamp)
            {
                this.Last();
                return;
            }


            this.Begin();
            while (!this.IsEOF())
            {
                long pos = this.GetPosition();
                DataLine line = this.ReadLine();
                if (line.TimeStamp >= ts)
                {
                    this.Seek(pos-1);
                }
            }
        }

        public bool IsEOF()
        {
            return (reader.BaseStream.Position == reader.BaseStream.Length);
        }

        private DataLine readDataLine()
        {
            if (!this.IsEOF())
            {
                DataLine dataLine = this.CreateDataLine();
                dataLine.ReadLine(reader);
                return dataLine;
            }
            else
            {
                return null;
            }
        }

    }
}

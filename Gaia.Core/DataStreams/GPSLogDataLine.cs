using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DataStreams
{
    public class GPSLogDataLine : DataLine
    {
        [System.ComponentModel.DisplayName("GPS Time Stamp [s]")]
        public double GPSTime { get; set; }

        [System.ComponentModel.DisplayName("High Performance Counter")]
        public long HPCTime { get; set; }

        public override int LineSize()
        {
            return 2*sizeof(double) + 2*sizeof(long);
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.GPSTime = reader.ReadDouble();
            this.HPCTime = reader.ReadInt64();
        }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.GPSTime);
            writer.Write(this.HPCTime);
        }
    }
}

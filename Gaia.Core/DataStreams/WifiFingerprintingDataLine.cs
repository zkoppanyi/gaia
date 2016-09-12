using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    public class WifiFingerprintingDataLine : DataLine
    {
        [System.ComponentModel.DisplayName("RSSI")]
        public double RSSI { get; set; }

        [System.ComponentModel.DisplayName("MAC")]
        public char[] MAC { get; set; }

        public override int LineSize()
        {
            return sizeof(long) + (2 * sizeof(double) + (6*sizeof(char)));
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.MAC = reader.ReadChars(6);
            this.RSSI = reader.ReadDouble();

        }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.MAC);
            writer.Write(this.RSSI);
        }
    }
}

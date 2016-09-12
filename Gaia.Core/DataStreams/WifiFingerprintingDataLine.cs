using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public class WifiFingerprintingDataLine : DataLine
    {
        [DisplayName("Signal Strength (RSSI)")]
        public double SignalStrength { get; set; }

        [Browsable(false)]
        public byte[] MAC { get; set; }

        [DisplayName("MAC")]
        [Browsable(true)]
        public string MACString { get { return Utilities.MACAddressToString(MAC); } }

        public override int LineSize()
        {
            return sizeof(long) + (2 * sizeof(double) + (6*sizeof(byte)));
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.MAC = reader.ReadBytes(6);
            this.SignalStrength = reader.ReadDouble();

        }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.MAC.Take(6).ToArray());
            writer.Write(this.SignalStrength);
        }
    }
}

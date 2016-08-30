using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DataStreams
{
    [Serializable]
    public class UWBDataLine : DataLine
    {
        [System.ComponentModel.DisplayName("Target Point #")]
        public long TargetPoint { get; set; }

        /// <summary>
        /// Distance between the two points [m]
        /// </summary>
        [System.ComponentModel.DisplayName("Distance [m]")]
        public double Distance { get; set; }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.TargetPoint);
            writer.Write(this.Distance);
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.TargetPoint = reader.ReadInt64();
            this.Distance = reader.ReadDouble();
        }

        public override int LineSize()
        {
            return (2 * sizeof(double) + (2*sizeof(long)));
        }
    }
}

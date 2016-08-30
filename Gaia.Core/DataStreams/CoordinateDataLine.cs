using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public class CoordinateDataLine : DataLine
    {
        [System.ComponentModel.DisplayName("X [m]")]
        public double X { get; set; }

        [System.ComponentModel.DisplayName("Y [m]")]
        public double Y { get; set; }

        [System.ComponentModel.DisplayName("Z [m]")]
        public double Z { get; set; }

        [System.ComponentModel.DisplayName("Sigma [m]")]
        public double Sigma { get; set; }

        public override int LineSize()
        {
            return (sizeof(long) + (5 * sizeof(double)));
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.X = reader.ReadDouble();
            this.Y = reader.ReadDouble();
            this.Z = reader.ReadDouble();
            this.Sigma = reader.ReadDouble();
        }

 
        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.X);
            writer.Write(this.Y);
            writer.Write(this.Z);
            writer.Write(this.Sigma);
        }
    }
}

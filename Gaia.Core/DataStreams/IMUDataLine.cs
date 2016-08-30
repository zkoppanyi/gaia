using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core.ReferenceFrames;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public class IMUDataLine : DataLine
    {

        [System.ComponentModel.DisplayName("Ax [m/s2]")]
        public double Ax { get; set; }

        [System.ComponentModel.DisplayName("Ay [m/s2]")]
        public double Ay { get; set; }

        [System.ComponentModel.DisplayName("Az [m/s2]")]
        public double Az { get; set; }

        [System.ComponentModel.DisplayName("Wx [deg/s]")]
        public double Wx { get; set; }

        [System.ComponentModel.DisplayName("Wy [deg/s]")]
        public double Wy { get; set; }

        [System.ComponentModel.DisplayName("Wz [deg/s]")]
        public double Wz { get; set; }

        public override string ToString()
        {
            return "TimeStamp: " + TimeStamp  + " Ax: " + Ax + " Ay: " + Ay + " Az: " + Az + " Wx: " + Wx + " Wy: " + Wy + " Wz: " + Wz;
        }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            writer.Write(this.Ax);
            writer.Write(this.Ay);
            writer.Write(this.Az);
            writer.Write(this.Wx);
            writer.Write(this.Wy);
            writer.Write(this.Wz);
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.Ax = reader.ReadDouble();
            this.Ay = reader.ReadDouble();
            this.Az = reader.ReadDouble();
            this.Wx = reader.ReadDouble();
            this.Wy = reader.ReadDouble();
            this.Wz = reader.ReadDouble();
        }

        public override int LineSize()
        {
            return sizeof(long) + 7 * sizeof(double);
        }
    }
}

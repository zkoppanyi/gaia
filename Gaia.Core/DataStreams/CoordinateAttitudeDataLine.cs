using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DataStreams
{
    [Serializable]
    public sealed class CoordinateAttitudeDataLine : CoordinateDataLine
    {
        [System.ComponentModel.DisplayName("Heading [deg]")]
        public double Heading { get; set; }

        [System.ComponentModel.DisplayName("Pitch [m]")]
        public double Pitch { get; set; }

        [System.ComponentModel.DisplayName("Roll [m]")]
        public double Roll { get; set; }

        [System.ComponentModel.DisplayName("Attitude Sigma[m]")]
        public double AttitudeSigma { get; set; }

        public override int LineSize()
        {
            return (base.LineSize() + (4 * sizeof(double)));
        }

        public override void ReadLine(BinaryReader reader)
        {
            base.ReadLine(reader);
            this.Heading = reader.ReadDouble();
            this.Pitch = reader.ReadDouble();
            this.Roll = reader.ReadDouble();
            this.AttitudeSigma = reader.ReadDouble();
        }


        public override void WriteLine(BinaryWriter writer)
        {
            base.WriteLine(writer);
            writer.Write(this.Heading);
            writer.Write(this.Pitch);
            writer.Write(this.Roll);
            writer.Write(this.AttitudeSigma);
        }

    }
}

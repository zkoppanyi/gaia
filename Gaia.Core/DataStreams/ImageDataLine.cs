using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public sealed class ImageDataLine : DataLine
    {
        private const int maxFileNameLength = 200;

        [System.ComponentModel.DisplayName("File name")]
        public String ImageFileName { get; set; }

        [System.ComponentModel.DisplayName("Is Available")]
        public bool IsAvailable { get { return isAvaliable; } }
        private bool isAvaliable = true;

        internal void SetIsAvailable(bool val)
        {
            isAvaliable = val;
        }

        public ImageDataLine()
        {

        }

        public override int LineSize()
        {
            return sizeof(long) + sizeof(double) + maxFileNameLength/2 * sizeof(char);
        }

        public override void ReadLine(BinaryReader reader)
        {
            this.Index = reader.ReadInt64();
            this.TimeStamp = reader.ReadDouble();
            this.ImageFileName = new String(reader.ReadChars(maxFileNameLength));
            this.ImageFileName = this.ImageFileName.Replace("\0", "");
        }

        public override void WriteLine(BinaryWriter writer)
        {
            writer.Write(this.Index);
            writer.Write(this.TimeStamp);
            char[] data = StringToCharArray(this.ImageFileName, maxFileNameLength);
            writer.Write(data);
        }

        private static char[] StringToCharArray(string str, int length)
        {
            return str.PadRight(length, '\0').ToCharArray();
        }
    }
}

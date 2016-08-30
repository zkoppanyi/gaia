using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    public abstract class DataLine
    {
        /// <summary>
        /// Timestamp of the dataline [s]
        /// </summary>
        [System.ComponentModel.DisplayName("Id")]
        public long Index { get; set; }

        /// <summary>
        /// Timestamp of the dataline [s]
        /// </summary>
        [System.ComponentModel.DisplayName("Timestamp [s]")]
        public double TimeStamp { get; set; }

        public abstract void WriteLine(BinaryWriter writer);
        public abstract void ReadLine(BinaryReader reader);
        public abstract int LineSize();
    }
}

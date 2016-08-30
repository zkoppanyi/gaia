using Gaia.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Processing
{
    public interface IClockErrorModel
    {
        string Name { get; set; }
        string Description { get; set; }
        void CorrectTimestamp(DataStream dataStream);
    }
}

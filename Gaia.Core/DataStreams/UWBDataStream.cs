using Gaia.Excpetions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.DataStreams
{
    [Serializable]
    public sealed class UWBDataStream : DataStream
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Station")]
        [Description("Station point ID of the measurements.")]
        [DisplayName("Station Point ID")]
        public long StationPointID { get; set; }

        protected override string extension { get { return "uwb";  } }

        private UWBDataStream(Project project, String fileId) : base(project, fileId)
        {

        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new UWBDataStream(project, fileId);
            return stream;
        }

        public override DataLine CreateDataLine()
        {
            return new UWBDataLine();
        }

        public override void AddDataLine(DataLine dataLine)
        {
            base.AddDataLine(dataLine);
            UWBDataLine uwbDataLine = dataLine as UWBDataLine;

            if (project.AddPoint(uwbDataLine.TargetPoint.ToString()))
            {
                WriteMessage("New point has been created: " + uwbDataLine.TargetPoint);
            }

        }

    }
}

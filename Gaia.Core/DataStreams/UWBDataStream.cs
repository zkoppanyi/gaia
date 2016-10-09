using Gaia.Exceptions;
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

        public override void SettingsCopyTo(DataStream copyDataStream)
        {
            base.SettingsCopyTo(copyDataStream);

            if (copyDataStream is UWBDataStream)
            {
                UWBDataStream uwbCopyDataStream = copyDataStream as UWBDataStream;
                uwbCopyDataStream.StationPointID = this.StationPointID;
            }
        }

        public override void AddDataLine(DataLine dataLine)
        {
            base.AddDataLine(dataLine);
            UWBDataLine uwbDataLine = dataLine as UWBDataLine;

            project.PointManager.AddPoint(uwbDataLine.TargetPoint.ToString());

        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public sealed class CoordinateAttitudeDataStream : CoordinateDataStream
    {
        private CoordinateAttitudeDataStream(Project project, string fileId) : base(project, fileId)
        {

        }

        internal new static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new CoordinateAttitudeDataStream(project, fileId);
            return stream;
        }

        protected override string extension
        {
            get { return "cad"; }
        }

        public override DataLine CreateDataLine()
        {
            return new CoordinateAttitudeDataLine();
        }
    }
}

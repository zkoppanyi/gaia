using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public class WifiFingerptiningDataStream : DataStream
    {
        private WifiFingerptiningDataStream(Project project, string fileId) : base(project, fileId)
        {

        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new WifiFingerptiningDataStream(project, fileId);
            return stream;
        }

        protected override string extension
        {
            get
            {
                return "wif";
            }
        }

        public override DataLine CreateDataLine()
        {
            return new WifiFingerprintingDataLine();
        }
    }
}

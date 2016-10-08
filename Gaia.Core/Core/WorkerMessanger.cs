using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core
{
    public class ReportArgs
    {
        public String Message { get; set; }
        public String Status { get; set; }
        public String MessageGroupStr { get; set; }
        public AlgorithmMessageType  Type { get; set; }
    }
    
    public sealed class AlgorithmWorker : BackgroundWorker
    {

       /// <summary>
        /// Add Algorithm object's event to this worker
        /// </summary>
        /// <param name="algorithm">Algorithm object</param>
        public void SubscirbeAlgorithm(Algorithm algorithm)
        {
            algorithm.SetWorker(this);
            algorithm.MessageReport += Algorithm_MessageReport;
            algorithm.ProgressReport += Algorithm_ProgressReport;
        }

        private void Algorithm_ProgressReport(object sender, AlgorithmProgressEventArgs e)
        {
            this.WriteProgress(e.Progress);
        }

        private void Algorithm_MessageReport(object sender, AlgorithmMessageEventArgs e)
        {
            this.WriteMessage(e.Message, e.Status, e.MessageGroupStr, e.MessageType);
        }

        public void WriteMessage(string message, string status = null, string messageGroupStr = null, AlgorithmMessageType type = AlgorithmMessageType.Message)
        {
            ReportArgs args = new ReportArgs();
            args.Message = message;
            args.Status = status;
            args.MessageGroupStr = messageGroupStr;
            args.Type = type;

            this.ReportProgress(-1, args);
        }

        public void WriteProgress(double percentage)
        {
            this.ReportProgress((int)percentage);
        }


    }
}

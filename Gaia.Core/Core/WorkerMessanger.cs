using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.GaiaSystem
{
    public class ReportArgs
    {
        public String Message { get; set; }
        public String Status { get; set; }
        public String MessageGroupStr { get; set; }
        public ConsoleMessageType  Type { get; set; }
    }
    
    public sealed class WorkerMessanger : BackgroundWorker, IMessanger
    {
        public bool IsCanceled()
        {
            return this.CancellationPending;
        }

        public void Progress(double percentage)
        {
            this.ReportProgress((int)percentage);
        }

        public void Write(string message, string status = null, string messageGroupStr = null, ConsoleMessageType type = ConsoleMessageType.Message)
        {
            ReportArgs args = new ReportArgs();
            args.Message = message;
            args.Status = status;
            args.MessageGroupStr = messageGroupStr;
            args.Type = type;

            this.ReportProgress(-1, args);
        }
        
       

        
    }
}

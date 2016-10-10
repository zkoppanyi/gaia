using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;

namespace Gaia.Core
{
    public enum AlgorithmResult
    {
        Sucess,
        Failure,
        Partial,
        InputMissing
    }

    public enum AlgorithmMessageType
    {
        Message,
        Warning,
        Error
    }

    public delegate void AlgorithmMessageEventHandler(object sender, AlgorithmMessageEventArgs e);
    public class AlgorithmMessageEventArgs
    {
        private String message;
        public String Message { get { return message; } }

        private String status = null;
        public String Status { get { return status; } }

        private String messageGroupStr = null;
        public String MessageGroupStr { get { return messageGroupStr; } }

        private AlgorithmMessageType type = AlgorithmMessageType.Message;
        public AlgorithmMessageType MessageType { get { return type; } }

        public AlgorithmMessageEventArgs(String message, String status = null, String messageGroupStr = null, AlgorithmMessageType messageType = AlgorithmMessageType.Message)
        {
            this.message = message;
            this.status = status;
            this.messageGroupStr = messageGroupStr;
            this.type = messageType;
        }
    }


    public delegate void AlgorithmProgressEventHandler(object sender, AlgorithmProgressEventArgs e);
    public class AlgorithmProgressEventArgs
    {
        private double progress;
        public double Progress { get { return progress; } }

        public AlgorithmProgressEventArgs(double progress)
        {
            this.progress = progress;
        }

    }

    public delegate void AlgorithmCompletedEventHandler(object sender, AlgorithmResult e);
    public class AlgorithmCompletedEventArgs
    {
        private AlgorithmResult result;
        public AlgorithmResult Result { get { return result; } }

        public AlgorithmCompletedEventArgs(AlgorithmResult result)
        {
            this.result = result;
        }

    }

    public abstract class Algorithm : GaiaObject
    {
        protected bool isCanceled = false;

        [field:NonSerialized]
        public event AlgorithmMessageEventHandler MessageReport;

        [field: NonSerialized]
        public event AlgorithmProgressEventHandler ProgressReport;

        [field: NonSerialized]
        public event AlgorithmCompletedEventHandler CompletedReport;

        [NonSerialized]
        private AlgorithmWorker worker;

        protected abstract AlgorithmResult run();

        protected DateTime lastMessage = DateTime.Now;
        protected DateTime lastProgress = DateTime.Now;
        protected StringBuilder stringMessage = new StringBuilder("");
        protected double writerUpdateTime = 0.5;
        protected double progressUpdateTime = 0.1;

        public AlgorithmResult Run()
        {
            AlgorithmResult result = run();
            WriteMessage("", null, null, AlgorithmMessageType.Message, true);
            CompletedReport?.Invoke(this, result);
            ProgressReport?.Invoke(this, new AlgorithmProgressEventArgs(100));
            return result;
        }

        public interface AlgorithmFactory
        {
            String Name { get; }
            String Description { get; }
        }

        public Algorithm(Project project, String name, String description) : base(project)
        {
            this.Name = name;
            this.Description = description;
            worker = null;
        }

        public void SetWorker(AlgorithmWorker worker)
        {
            this.worker = worker;
        }

        protected void WriteMessage(String message, String status = null, String messageGroupStr = null, AlgorithmMessageType messageType = AlgorithmMessageType.Message, bool forceShow = false)
        {
            stringMessage.Append(message + Environment.NewLine);
            double dt = (DateTime.Now.Ticks - lastMessage.Ticks) / TimeSpan.TicksPerSecond;
            if ((dt > writerUpdateTime) || (forceShow == true))
            {
                MessageReport?.Invoke(this, new AlgorithmMessageEventArgs(stringMessage.ToString(), status, messageGroupStr, messageType));
                lastMessage = DateTime.Now;
                stringMessage.Clear();
            }
        }

        protected void WriteProgress(double percent)
        {
            double dt = (DateTime.Now.Ticks - lastProgress.Ticks) / TimeSpan.TicksPerSecond;
            if (dt > progressUpdateTime)
            {
                ProgressReport?.Invoke(this, new AlgorithmProgressEventArgs(percent));
                lastProgress = DateTime.Now;
            }
            
        }

        protected void Completed(AlgorithmResult result)
        {
            CompletedReport?.Invoke(this, result);
        }

        public void Cancel()
        {
            isCanceled = true;

            if (worker.CancellationPending == false)
            {
                worker.CancelAsync();
            }
        }

        public bool IsCanceled()
        {
            if (worker != null)
            {
                return worker.CancellationPending;
            }
            else
            {
                return isCanceled;
            }
        }
    }
}

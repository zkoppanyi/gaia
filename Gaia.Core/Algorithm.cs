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

    public abstract class Algorithm : GaiaObject
    {
        protected bool isCanceled = false;
        protected IMessanger messanger { get; set; }

        public Algorithm(Project project) : this(project, null)
        {

        }

        public Algorithm(Project project, IMessanger messanger) : base(project)
        {
            this.messanger = messanger;
        }

        protected void WriteMessage(String message)
        {
            if (messanger != null)
            {
                messanger.Write(message);
            }
        }

        protected void WriteMessage(String message, String status = null, String messageGroupStr = null, ConsoleMessageType type = ConsoleMessageType.Message)
        {
            if (messanger == null)
            {
                return;
            }

            messanger.Write(message, status, messageGroupStr, type);
        }

        protected void WriteProgress(double percent)
        {
            if (messanger != null)
            {
                messanger.Progress(percent);
            }
        }

        public bool IsCanceled()
        {
            if (messanger == null)
            {
                return false;
            }

            return messanger.IsCanceled();
        }
    }
}

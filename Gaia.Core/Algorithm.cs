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

        public abstract AlgorithmResult Run();

        public interface AlgorithmFactory
        {
            String Name { get; }
            String Description { get; }

        }

        public Algorithm(Project project, String name, String description) : this(project, null, name, description)
        {

        }

        public Algorithm(Project project, IMessanger messanger, String name, String description) : base(project)
        {
            this.messanger = messanger;
            this.Name = name;
            this.Description = description;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core
{
    public enum ConsoleMessageType
    {
        Message,
        Warning,
        Error
    }

    public interface IMessanger
    {
        void Write(String message, String status = null, String messageGroupStr = null, ConsoleMessageType type = ConsoleMessageType.Message);
        void Progress(double percentage);
        bool IsCanceled();
    }
}

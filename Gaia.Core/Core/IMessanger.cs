using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core
{
    [Obsolete]
    public enum ConsoleMessageType
    {
        Message,
        Warning,
        Error
    }

    [Obsolete]
    public interface IMessanger
    {
        void Write(String message, String status = null, String messageGroupStr = null, ConsoleMessageType type = ConsoleMessageType.Message);
        void Progress(double percentage);
        bool IsCanceled();
    }
}

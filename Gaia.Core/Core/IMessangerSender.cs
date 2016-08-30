using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.GaiaSystem
{
    public interface IMessangerSender
    {
        IMessanger Messanger { get; set; }
    }
}

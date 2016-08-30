using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Excpetions
{
    public class GaiaAssertException : GaiaException
    {
        public GaiaAssertException() : base()
        {

        }

        public GaiaAssertException(String message) : base(message)
        {

        }
    }
}

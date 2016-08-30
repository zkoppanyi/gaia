using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Excpetions
{
    public class GaiaException : Exception
    {
        public GaiaException() : base()
        {

        }

        public GaiaException(String message) : base(message)
        {

        }
    }
}

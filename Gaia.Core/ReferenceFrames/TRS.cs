using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.ReferenceFrames
{
    /// <summary>
    /// Time reference systems
    /// </summary>

    [Serializable]
    public class TRS
    {
        private String name;
        private String description;

        public TRS(String name, String description)
        {
            this.name = name;
            this.description = description;
        }

        public string Name { get { return name;  } }
        public string Description {
            get
            {
                return name + " - " + description;
            }
        }

        public override string ToString()
        {
            return this.Name;
        }
    }


}

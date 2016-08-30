using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;

namespace Gaia.Core.Import
{
    [Serializable]
    public abstract class UWBImporter : Importer
    {
        public UWBImporter(Project project, IMessanger messanger = null) : base(project, messanger)
        {

        }

    }
}

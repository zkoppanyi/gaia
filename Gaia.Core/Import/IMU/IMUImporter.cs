using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;

namespace Gaia.Core.Import
{
    [Serializable]
    public abstract class IMUImporter : Importer
    {

        protected IMUImporter(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {

        }

    }
}

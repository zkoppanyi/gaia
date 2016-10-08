﻿using System;
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
        protected UWBImporter(Project project, String name, String description) : base(project, name, description)
        {

        }

    }
}

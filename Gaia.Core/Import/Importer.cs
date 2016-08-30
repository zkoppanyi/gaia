﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

using Gaia.Core;
using Gaia.Core.DataStreams;

namespace Gaia.Core.Import
{
    [Serializable]
    public abstract class Importer : Algorithm
    {
        public abstract AlgorithmResult Import(String filePath, DataStream stream);
        public abstract DataStreamType GetDataStreamType();

        public Importer(Project project, IMessanger messanger = null) : base(project, messanger)
        {

        }        

        public virtual String SupportedFileFormats()
        {
            return "All files (*.*)|*.*";
        }

        public void SetMessanger(IMessanger messanger)
        {
            this.messanger = messanger;
        }

        public void SetProject(Project project)
        {
            this.project = project;
        }

    }
}

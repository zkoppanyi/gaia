using System;
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

        protected Importer(Project project, String name, String description) : base(project, name, description)
        {

        }

        public interface ImporterFactory : AlgorithmFactory
        {
            Importer Create(string filePath, DataStream dataStream, Project project);
            DataStreamType GetDataStreamType();
        }

        public virtual String SupportedFileFormats()
        {
            return "All files (*.*)|*.*";
        }

        public void SetProject(Project project)
        {
            this.project = project;
        }

    }
}

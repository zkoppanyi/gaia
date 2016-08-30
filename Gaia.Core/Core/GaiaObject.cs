using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core
{
    [Serializable]
    public class GaiaObject
    {
        protected Project project;

        [Browsable(false)]
        public virtual  Project Project { get { return project; } }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("General")]
        [Description("Name of the object.")]
        public virtual String Name { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("General")]
        [Description("Description of the object")]
        public virtual String Description { get; set; }

        public GaiaObject(Project project)
        {
            this.project = project;
        }
    }
}

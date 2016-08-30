using Gaia.ReferenceFrames;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.GaiaSystem
{
    [Serializable]
    public class GaiaSpatialObject : GaiaObject
    {
        public GaiaSpatialObject(Project project) : base(project)
        {

        }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Reference Frame")]
        [Description("Coordinate reference frame of the object.")]
        public virtual CRS CRS { get; set; }

        [Browsable(true)]
        [ReadOnly(true)]
        [Category("Reference Frame")]
        [Description("Time reference frame of the object.")]
        public virtual TRS TRS { get; set; }
    }
}

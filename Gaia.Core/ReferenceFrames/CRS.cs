using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.ReferenceFrames
{
    [Serializable]
    public class CRS
    {
        private String name;
        public String Name { get { return name; } }

        private String wkt;
        public String WKT { get { return wkt; } }

        public CRS(String name, String wkt)
        {
            this.name = name;
            this.wkt = wkt;
        }

        public ICoordinateSystem GetCoordinateSystem()
        {
            return ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(WKT) as ICoordinateSystem;
        }

        public override string ToString()
        {
            return this.name;
        }
    }
}

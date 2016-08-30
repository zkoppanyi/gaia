using Gaia.Core.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core
{
    public sealed partial class Project
    {

        [Serializable]
        public class PointManagerClass
        {
            Project project;

            public PointManagerClass(Project project)
            {
                this.project = project;
            }

            public bool DoesPointIdExist(String ptId)
            {
                GPoint ptf = project.points.Find(x => x.Name == ptId);
                if (ptf == null) return false;
                return true;
            }

            public bool AddPoint(GPoint pt)
            {
                if (!DoesPointIdExist(pt.Name))
                {
                    project.points.Add(pt);
                    return true;
                }
                else
                {
                    return false;
                }
            }

            public bool AddPoint(String ptId)
            {
                if (!DoesPointIdExist(ptId))
                {
                    GPoint pt = new GPoint(project, ptId);
                    pt.X = 0; pt.Y = 0; pt.Z = 0;
                    pt.PointType = GPointType.NA;
                    pt.CRS = null;
                    pt.TRS = project.TimeFrames[0];
                    project.points.Add(pt);
                    return true;
                }
                else
                {
                    return false;

                }
            }

            public GPoint GetPoint(string ptId)
            {
                return project.points.Find(x => x.Name == ptId);
            }

            public void RemovePoint(String ptId)
            {
                GPoint pt = GetPoint(ptId);
                if (pt != null)
                    project.points.Remove(pt);
            }

        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.ReferenceFrames;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using Gaia.Import;
using Gaia.DataStreams;
using Gaia.GaiaSystem;
using Gaia.Processing;

namespace Gaia
{

    [Serializable]
    public sealed partial class Project
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public String Location { get; set; }

        private List<GPoint> points;
        public List<IClockErrorModel> ClockErrorModels;

        private DataStreamManagerClass dataStreamManager = null;
        public DataStreamManagerClass DataStreamManager { get { return dataStreamManager; } }

        public IEnumerable<GPoint> PointsList
        {
            get
            {
                foreach (GPoint point in points) yield return point;
            }
        }

        private List<DataStream> dataStreams;
        public IEnumerable<DataStream> DataStreams
        {
            get
            {
                foreach (var stream in dataStreams) yield return stream;
            }
        }

        public List<TRS> TimeFrames;
        private static String DATASTREAM_FOLDER = "DataStreams";

        private Project(String location)
        {
            this.points = new List<GPoint>();
            this.dataStreams = new List<DataStream>();
            this.TimeFrames = new List<TRS>();
            this.ClockErrorModels = new List<IClockErrorModel>();
            this.Location = location;
            CreateFolderStructure();
            dataStreamManager = new Project.DataStreamManagerClass(this);
        }

        public void Save()
        {
            this.Save(this.Location);
        }

        public void Clean()
        {
            for(int i= 0; i<this.dataStreams.Count(); i++)
            {
                if (this.dataStreams[i] == null)
                {
                    this.dataStreams.RemoveAt(i);
                }
            }

            if (ClockErrorModels == null)
            {
                ClockErrorModels = new List<IClockErrorModel>();
            }
        }

        public static Project CreateDefaultProject(String location)
        {
            Project newProject = new Project(location);            
            return newProject;
        }

        public static Project Load(String projectPath)
        {
            using (Stream stream = new FileStream(projectPath, FileMode.Open))
            {
                if (stream == null) return null;
                BinaryFormatter formatter = new BinaryFormatter();
                Project project = (Project)formatter.Deserialize(stream);
                String location = Path.GetDirectoryName(projectPath);
                project.Location = location;
                return project;
            }
        }

        private void Save(String path)
        {
            // Serialize object
            String projectLocationPath = path + "\\" + this.Name + ".gpj";
            Stream stream = new FileStream(projectLocationPath, FileMode.Create, FileAccess.Write, FileShare.None);
            IFormatter formatter = new BinaryFormatter();
            formatter.Serialize(stream, this);
            stream.Close();
        }

        private void CreateFolderStructure()
        {
            Directory.CreateDirectory(this.Location + "\\" + Project.DATASTREAM_FOLDER);
        }

        public String GetProjectFile()
        {
           return this.Location + "\\" + this.Name;
        }

        public String GetDataStreamFolder()
        {
            return this.Location + "\\" + DATASTREAM_FOLDER;
        }

        public void SetDefault()
        {
            this.TimeFrames.Add(new TRS("N/A", "Not known"));
            this.TimeFrames.Add(new TRS("Local", "Local unkwon timing"));
            this.TimeFrames.Add(new TRS("UTC", "Universal Coordinated Time"));
            this.TimeFrames.Add(new TRS("GPST", "GPS Time"));

        }

        public TRS GetTimeFrameByName(String trsName)
        {
            return this.TimeFrames.Find(x => x.Name == trsName);
        }

        public bool DoesPointIdExist(String ptId)
        {
            GPoint ptf = this.points.Find(x => x.Name == ptId);
            if (ptf == null) return false;
            return true;
        }

        public bool AddPoint(GPoint pt)
        {
            if (!DoesPointIdExist(pt.Name))
            {
                points.Add( pt);
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
                GPoint pt = new GPoint(this, ptId);
                pt.X = 0; pt.Y = 0; pt.Z = 0;
                pt.PointType = GPointType.NA;
                pt.CRS = null;
                pt.TRS = this.TimeFrames[0];
                points.Add(pt);
                return true;
            }
            else
            {
                return false;

            }
        }

        public GPoint GetPoint(string ptId)
        {
            return points.Find(x => x.Name == ptId);
        }       

        public void RemovePoint(String ptId)
        {
            GPoint pt = GetPoint(ptId);
            if (pt != null)
                this.points.Remove(pt);
        }


    }
}

using Gaia.DataStreams;
using Gaia.Import;
using Gaia.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia
{

    public sealed partial class Project
    {

        [Serializable]
        public class DataStreamManagerClass
        {
            Project project;

            public DataStreamManagerClass(Project project)
            {
                this.project = project;
            }

            private bool dataStreamCreateToken;
            internal bool DataStreamCreateToken { get { return dataStreamCreateToken; } }

            public DataStream ImportDataStream(String resourceLocation, Importer importer)
            {
                DataStream stream = this.CreateDataStream(importer.GetDataStreamType());
                if (stream == null) return null;

                importer.SetProject(project);
                if (importer.Import(resourceLocation, stream) == AlgorithmResult.Failure)
                {
                    this.RemoveDataStream(stream);
                    return null;
                }

                return stream;
            }

            public DataStream CreateDataStream(DataStreamType dataStreamType)
            {
                dataStreamCreateToken = true;

                DataStream stream = null;

                if (dataStreamType == DataStreamType.CoordinateDataStream)
                {
                    stream = CoordinateDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
                }
                else if (dataStreamType == DataStreamType.CoordinateAttitudeDataStream)
                {
                    stream = CoordinateAttitudeDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
                }
                else if (dataStreamType == DataStreamType.GPSLogDataStream)
                {
                    stream = GPSLogDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
                }
                else if (dataStreamType == DataStreamType.IMUDataStream)
                {
                    stream = IMUDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
                }
                else if (dataStreamType == DataStreamType.UWBDataStream)
                {
                    stream = UWBDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
                }

                if (stream != null)
                {
                    if (project.dataStreams == null) project.dataStreams = new List<DataStream>(); // if would be any problem
                    project.dataStreams.Add(stream);
                }

                dataStreamCreateToken = false;

                project.Save();
                return stream;
            }

            public void RemoveDataStream(DataStream stream)
            {
                if (stream != null)
                {
                    project.dataStreams.Remove(stream);
                    stream.Drop();
                    if (stream is IClockErrorModel)
                    {
                        project.ClockErrorModels.Remove(stream as IClockErrorModel);
                    }
                    project.Save();
                }
            }
        }
    }
}

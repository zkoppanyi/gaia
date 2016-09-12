using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using Gaia.Core.Import;
using Gaia.Core.Processing;

namespace Gaia.Core
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

            public DataStream ImportDataStream(String resourceLocation, Importer.ImporterFactory importerFactory, IMessanger messanger = null)
            {
                DataStream stream = this.CreateDataStream(importerFactory.GetDataStreamType());
                if ((stream == null) && (importerFactory.Name != "Point Importer"))
                {
                    throw new GaiaAssertException("Importer is not found!");
                }

                Importer importer = importerFactory.Create(resourceLocation, stream, project, messanger);
                if (importer.Run() == AlgorithmResult.Failure)
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
                else if (dataStreamType == DataStreamType.WifiFingerprinting)
                {
                    stream = WifiFingerptiningDataStream.Create(project, DateTime.Now.ToString("yyyyMMddhhmmss"));
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

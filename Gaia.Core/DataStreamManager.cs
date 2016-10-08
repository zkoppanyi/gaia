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

            [field: NonSerialized]
            public event AlgorithmMessageEventHandler ImportMessage;

            [field: NonSerialized]
            public event AlgorithmProgressEventHandler ImportProgress;

            [field: NonSerialized]
            public event AlgorithmCompletedEventHandler ImportCompleted;

            public DataStreamManagerClass(Project project)
            {
                this.project = project;
            }

            private bool dataStreamCreateToken;
            internal bool DataStreamCreateToken { get { return dataStreamCreateToken; } }

            DataStream importDataStream = null;
            public DataStream ImportDataStream(String resourceLocation, Importer.ImporterFactory importerFactory, AlgorithmWorker worker=null)
            {
                importDataStream = this.CreateDataStream(importerFactory.GetDataStreamType());
                if ((importDataStream  == null) && (importerFactory.Name != "Point Importer"))
                {
                    throw new GaiaAssertException("Importer is not found!");
                }

                Importer importer = importerFactory.Create(resourceLocation, importDataStream, project);
                if (worker != null) worker.SubscirbeAlgorithm(importer);
                importer.MessageReport += Importer_MessageReport;
                importer.ProgressReport += Importer_ProgressReport;
                importer.CompletedReport += Importer_CompletedReport;
                importer.Run();
                this.clearEvents();
                return importDataStream;
            }

            private void clearEvents()
            {
                foreach (Delegate d in this.ImportMessage.GetInvocationList())
                {
                    ImportMessage -= (AlgorithmMessageEventHandler)d;
                }

                foreach (Delegate d in this.ImportProgress.GetInvocationList())
                {
                    ImportProgress -= (AlgorithmProgressEventHandler)d;
                }

                foreach (Delegate d in this.ImportCompleted.GetInvocationList())
                {
                    ImportCompleted -= (AlgorithmCompletedEventHandler)d;
                }
            }

            private void Importer_CompletedReport(object sender, AlgorithmResult e)
            {
                if (e != AlgorithmResult.Sucess)
                {
                    if (importDataStream != null)
                    {
                        this.RemoveDataStream(importDataStream);
                    }
                }

                ImportCompleted?.Invoke(sender, e);
            }

            private void Importer_ProgressReport(object sender, AlgorithmProgressEventArgs e)
            {
                ImportProgress?.Invoke(sender, e);
            }

            private void Importer_MessageReport(object sender, AlgorithmMessageEventArgs e)
            {
                ImportMessage?.Invoke(sender, e);
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

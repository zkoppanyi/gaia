using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using Gaia.Core;

namespace Gaia.Core.Import
{
    public sealed class IMUEpsonLogImporter : IMUTextImporter
    {
        public static IMUEpsonLogImporterFactory Factory
        {
            get
            {
                return new IMUEpsonLogImporterFactory();
            }
        }

        public class IMUEpsonLogImporterFactory : ImporterFactory
        {
            public String Name { get { return "Epson IMU Import CSV"; } }
            public String Description { get { return "Import Epson CSV Log file!" + Environment.NewLine + "Format: check the sample file!"; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.IMUDataStream;
            }


            public Importer Create(string filePath, DataStream dataStream, Project project, IMessanger messanger = null)
            {
                IMUEpsonLogImporter importer = new IMUEpsonLogImporter(project, messanger, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private IMUEpsonLogImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream) : base(project, messanger, name, description, filePath, dataStream)
        {
            Separator = ',';
            ColumnTimeStamp = 1;
            ColumnAx = 7;
            ColumnAy = 8;
            ColumnAz = 9;
            ColumnWx = 4;
            ColumnWy = 5;
            ColumnWz = 6;
            ColumnRatioAx = Utilities.g / 1000;
            ColumnRatioAy = -Utilities.g / 1000;
            ColumnRatioAz = -Utilities.g / 1000;
            ColumnRatioWx = 1;
            ColumnRatioWy = -1;
            ColumnRatioWz = -1;
            HeaderRowNo = 7;
        }
     
        public override string SupportedFileFormats()
        {
            return "CSV files (*.csv)|*.csv|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        }


        public override AlgorithmResult Run()
        {
            base.dataStream.TRS = project.GetTimeFrameByName("GPST");
            return base.Run();
        }

    }
}

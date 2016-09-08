using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Gaia.Core.DataStreams;
using Gaia.Core;
using Gaia.Exceptions;

namespace Gaia.Core.Import
{
    [Serializable]
    public class IMUMicroStrainLogImporter : IMUTextImporter
    {
        public static IMUMicroStrainLogImporterFactory Factory
        {
            get
            {
                return new IMUMicroStrainLogImporterFactory();
            }
        }

        public class IMUMicroStrainLogImporterFactory : ImporterFactory
        {
            public String Name { get { return "Import Microstrain CSV Log file!" + Environment.NewLine + "Format: check the sample file!"; } }
            public String Description { get { return "MicroStrain Import CSV"; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.IMUDataStream;
            }


            public Importer Create(string filePath, DataStream dataStream, Project project, IMessanger messanger = null)
            {
                IMUMicroStrainLogImporter importer = new IMUMicroStrainLogImporter(project, messanger, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private IMUMicroStrainLogImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream) : base(project, messanger, name, description, filePath, dataStream)

        {
            Separator = ',';
            ColumnTimeStamp = 2;
            ColumnAx = 7;
            ColumnAy = 8;
            ColumnAz = 9;
            ColumnWx = 10;
            ColumnWy = 11;
            ColumnWz = 12;
            ColumnRatioAx = 1;
            ColumnRatioAy = 1;
            ColumnRatioAz = 1;
            ColumnRatioWx = 1;
            ColumnRatioWy = 1;
            ColumnRatioWz = 1;
            HeaderRowNo = 16;
            this.Name = name;
            this.Description = description;
            this.dataStream = dataStream;
            this.filePath = filePath;
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

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

        public class IMUEpsonLogImporterFactory : IMUTextImporterFactory
        {
            public new String Name { get { return "Epson IMU Import CSV"; } }
            public new String Description { get { return "Import Epson CSV Log file!" + Environment.NewLine + "Format: check the sample file!"; } }

            public IMUEpsonLogImporterFactory()
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
                ParseAllYouCan = false;
            }

            public new Importer Create(string filePath, DataStream dataStream, Project project, IMessanger messanger = null)
            {

                IMUEpsonLogImporter importer = new IMUEpsonLogImporter(project, messanger, Name, Description, filePath, dataStream,
                        ColumnTimeStamp, ColumnAx, ColumnAy, ColumnAz, ColumnWx, ColumnWy, ColumnWz,
                        ColumnRatioAx, ColumnRatioAy, ColumnRatioAz, ColumnRatioWx, ColumnRatioWy, ColumnRatioWz, HeaderRowNo, ParseAllYouCan);

                return importer;
            }
        }

        private IMUEpsonLogImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream,
            int columnTimeStamp, int columnAx, int columnAy, int columnAz, int columnWx, int columnWy, int columnWz,
            double columnRatioAx, double columnRatioAy, double columnRatioAz,
            double columnRatioWx, double columnRatioWy, double columnRatioWz, int headerRowNo, bool parseAllYouCan) : base(project, messanger, name, description, filePath, dataStream,
                        columnTimeStamp, columnAx, columnAy, columnAz, columnWx, columnWy, columnWz,
                        columnRatioAx, columnRatioAy, columnRatioAz, columnRatioWx, columnRatioWy, columnRatioWz, headerRowNo, parseAllYouCan)

        {
            
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

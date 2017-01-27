using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;

using Gaia.Core.DataStreams;
using Gaia.Core;
using Gaia.Exceptions;
using Gaia.Core.Processing;

namespace Gaia.Core.Import
{
    [Serializable]
    public class IMUMicroStrainLogImporter : IMUTextImporter
    {
        public new static IMUMicroStrainLogImporterFactory Factory
        {
            get
            {
                return new IMUMicroStrainLogImporterFactory();
            }
        }

        public class IMUMicroStrainLogImporterFactory : IMUTextImporterFactory
        {
            public new String Name { get { return "Import Microstrain CSV Log file!" + Environment.NewLine + "Format: check the sample file!"; } }
            public new String Description { get { return "MicroStrain Import CSV"; } }

            public IMUMicroStrainLogImporterFactory()
            {
                Separator = ',';
                ColumnTimeStamp = 2;
                ColumnAx = 7;
                ColumnAy = 8;
                ColumnAz = 9;
                ColumnWx = 10;
                ColumnWy = 11;
                ColumnWz = 12;
                ColumnRatioAx = Utilities.g;
                ColumnRatioAy = Utilities.g;
                ColumnRatioAz = Utilities.g;
                ColumnRatioWx = 180.0/ Math.PI;
                ColumnRatioWy = 180.0/ Math.PI;
                ColumnRatioWz = 180.0/ Math.PI;
                HeaderRowNo = 16;
                ParseAllYouCan = true;
            }

            public new Importer Create(string filePath, DataStream dataStream, Project project)
            {

                IMUMicroStrainLogImporter importer = new IMUMicroStrainLogImporter(project, Name, Description, filePath, dataStream,
                        ColumnTimeStamp, ColumnAx, ColumnAy, ColumnAz, ColumnWx, ColumnWy, ColumnWz,
                        ColumnRatioAx, ColumnRatioAy, ColumnRatioAz, ColumnRatioWx, ColumnRatioWy, ColumnRatioWz, HeaderRowNo, ParseAllYouCan);

                return importer;
            }
        }

        private IMUMicroStrainLogImporter(Project project, String name, String description, String filePath, DataStream dataStream,
            int columnTimeStamp, int columnAx, int columnAy, int columnAz, int columnWx, int columnWy, int columnWz,
            double columnRatioAx, double columnRatioAy, double columnRatioAz,
            double columnRatioWx, double columnRatioWy, double columnRatioWz, int headerRowNo, bool parseAllYouCan) : base(project, name, description, filePath, dataStream,
                        columnTimeStamp, columnAx, columnAy, columnAz, columnWx, columnWy, columnWz,
                        columnRatioAx, columnRatioAy, columnRatioAz, columnRatioWx, columnRatioWy, columnRatioWz, headerRowNo, parseAllYouCan)
        {

        }

        public override string SupportedFileFormats()
        {
            return "CSV files (*.csv)|*.csv|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        }


        protected override AlgorithmResult run()
        {
            base.dataStream.TRS = project.GetTimeFrameByName("GPST");
            return base.run();
        }

    }
}

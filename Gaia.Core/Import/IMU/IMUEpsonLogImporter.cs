using Gaia.DataStreams;
using Gaia.Excpetions;
using Gaia.GaiaSystem;
using Gaia.Processing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Import
{
    public sealed class IMUEpsonLogImporter : IMUTextImporter
    {

        public IMUEpsonLogImporter(Project project, IMessanger messanger = null) : base(project, messanger)
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


        override public string Description
        {
            get
            {
                return "Import Epson CSV Log file!" + Environment.NewLine + "Format: check the sample file!";
            }
        }

        override public string Name
        {
            get
            {
                return "Epson IMU Import CSV";
            }
        }

        public override string SupportedFileFormats()
        {
            return "CSV files (*.csv)|*.csv|TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        }


        public override AlgorithmResult Import(string filePath, DataStream stream)
        {
            stream.TRS = project.GetTimeFrameByName("GPST");
            return base.Import(filePath, stream);
        }

    }
}

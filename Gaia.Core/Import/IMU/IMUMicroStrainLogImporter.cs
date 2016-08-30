using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.DataStreams;
using System.IO;
using Gaia.GaiaSystem;
using Gaia.Excpetions;

namespace Gaia.Import
{
    [Serializable]
    public class IMUMicroStrainLogImporter : IMUTextImporter
    {

        public IMUMicroStrainLogImporter(Project project, IMessanger messanger = null) : base(project, messanger)
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
        }

        override public string Description
        {
            get
            {
                return "Import Microstrain CSV Log file!" + Environment.NewLine + "Format: check the sample file!";
            }
        }

        override public string Name
        {
            get
            {
                return "MicroStrain Import CSV";
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

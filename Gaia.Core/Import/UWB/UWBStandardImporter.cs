using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.Core.DataStreams;
using System.IO;
using Gaia.Exceptions;
using Gaia.Core;
using System.ComponentModel;

namespace Gaia.Core.Import
{
    [Serializable]
    public class UWBStandardImporter : UWBImporter
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column timestamp. Unit: [s].")]
        [DisplayName("Column Timestamp")]
        public int ColumnTimeStampNum { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column target ID.")]
        [DisplayName("Column Target ID")]
        public int ColumnTargetNum { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column X coordinate. Unit: [m].")]
        [DisplayName("Column Distance")]
        public int ColumnDistanceNum { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Others")]
        [Description("The column separator.")]
        [DisplayName("Separator")]
        public char Separator { get; set; }

        protected String filePath;
        protected DataStream dataStream;

        public static UWBStandardImporterFactory Factory
        {
            get
            {
                return new UWBStandardImporterFactory();
            }
        }

        public class UWBStandardImporterFactory : ImporterFactory
        {
            public String Name { get { return "Standard UWB Importer"; } }
            public String Description { get { return "Standard UWB Importer logged by SPIN's data logging software!";  } }


            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.UWBDataStream;
            }

            Importer ImporterFactory.Create(string filePath, DataStream dataStream, Project project, IMessanger messanger)
            {
                UWBStandardImporter importer = new UWBStandardImporter(project, messanger, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        public override string SupportedFileFormats()
        {
            return "TXT files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
        }

        protected UWBStandardImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream) : base(project, messanger, name, description)
        {
            ColumnTimeStampNum = 27;
            ColumnTargetNum = 7;
            ColumnDistanceNum = 13;
            Separator = ' ';
            this.Name = name;
            this.Description = description;
            this.dataStream = dataStream;
            this.filePath = filePath;
        }


        public override AlgorithmResult Run()
        {
            if (project == null)
            {
                new GaiaAssertException("Project has not been set for the importer!");
            }

            try
            {                
                dataStream.Open();

                var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                messanger.Write("Import stream is opened: " + filePath);
                messanger.Write("Importing...");

                using (StreamReader reader = new StreamReader(sourceStream, Encoding.UTF8))
                {
                    int lineNum = 0;
                    while (!reader.EndOfStream)
                    {
                        if (IsCanceled())
                        {
                            dataStream.Close();
                            reader.Close();
                            WriteMessage("Importing canceled!", null, null, ConsoleMessageType.Warning);
                            return AlgorithmResult.Partial;
                        }

                        String line = reader.ReadLine();
                        UWBDataLine uwbLine = new UWBDataLine();
                        string[] sline = line.Split(this.Separator);

                        int errorSign = Convert.ToInt16(sline[8]);
                        if (errorSign == 0)
                        {
                            uwbLine.TimeStamp = Convert.ToDouble(sline[ColumnTimeStampNum]);
                            uwbLine.Distance = Convert.ToDouble(sline[ColumnDistanceNum]) / 1000;
                            uwbLine.TargetPoint = Convert.ToInt32(sline[ColumnTargetNum]);
                            dataStream.AddDataLine(uwbLine);    
                        }

                        lineNum++;

                        if (messanger != null)
                        {
                            messanger.Progress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));
                        }
                    }
                }
                dataStream.Close();

                if (dataStream.DataNumber == 0)
                {
                    WriteMessage("No data has been parsed!");
                    return AlgorithmResult.Failure;
                }

                WriteMessage("Parsed " + dataStream.DataNumber + " data!");
                WriteMessage("Importing is done!");
                return AlgorithmResult.Sucess;

            }
            catch (Exception ex)
            {
                WriteMessage("Importer error: " + ex.Message, null, null, ConsoleMessageType.Error);
                return AlgorithmResult.Failure;
            }

        }

    }
}

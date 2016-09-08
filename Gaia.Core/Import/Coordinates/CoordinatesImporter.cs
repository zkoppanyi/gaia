using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using Gaia.Exceptions;
using System.ComponentModel;

using Gaia.Core.DataStreams;
using Gaia.Core;

namespace Gaia.Core.Import
{
    public sealed class CoordinatesImporter : Importer
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column timestamp. Unit: [s].")]
        [DisplayName("Column Timestamp")]
        public int ColumnTimeStamp { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column X coordinate. Unit: [m].")]
        [DisplayName("Column X coordinate")]
        public int ColumnX { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column Y coordinate. Unit: [m].")]
        [DisplayName("Column Y coordinate")]
        public int ColumnY { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column Z coordinate. Unit: [m].")]
        [DisplayName("Column Z coordinate")]
        public int ColumnZ { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column sigma. Unit: [m].")]
        [DisplayName("Column Sigma (optional)")]
        public int ColumnSigma { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Others")]
        [Description("No. of lines of the header.")]
        [DisplayName("Header Row Number")]
        public int HeaderRowNo { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Others")]
        [Description("The column separator.")]
        [DisplayName("Separator")]
        public char Separator { get; set; }

        private String filePath;
        private DataStream dataStream;

        public static CoordinatesImporterFactory Factory
        {
            get
            {
                return new CoordinatesImporterFactory();
            }
        }

        public class CoordinatesImporterFactory : ImporterFactory
        {
            public String Name { get { return "Coordinate importer"; } }
            public String Description { get { return "Coordinate importer. " + Environment.NewLine + "Format: {TS},{X},{Y},{Z}[,{Sigma}]"; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.CoordinateDataStream;
            }

            public Importer Create(string filePath, DataStream dataStream, Project project, IMessanger messanger=null)
            {
                CoordinatesImporter importer = new CoordinatesImporter(project, messanger, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private CoordinatesImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream) : base(project, messanger, name, description)
        {
            this.HeaderRowNo = 0;
            this.ColumnTimeStamp = 0;
            this.ColumnX = 1;
            this.ColumnY = 2;
            this.ColumnZ = 3;
            this.ColumnSigma = 4;
            this.Separator = ',';
            this.filePath = filePath;
            this.dataStream = dataStream;
        }
      

        public override AlgorithmResult Run()
        {
            if (project == null)
            {
                new GaiaAssertException("Project has not been set for the importer!");
            }

            try
            {                
                dataStream.Open(this.messanger);

                var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                messanger.Write("Import stream is opened: " + filePath);
                messanger.Write("Importing...");

                int numLine = 0;
                using (StreamReader reader = new StreamReader(sourceStream, Encoding.UTF8))
                {
                    // Header
                    for (int i = 0; i < HeaderRowNo; i++)
                    {
                        String line = reader.ReadLine();
                        numLine++;
                    }

                    while (!reader.EndOfStream)
                    {
                        String line = reader.ReadLine();
                        CoordinateDataLine coorLine = new CoordinateDataLine();
                        string[] sline = line.Split(this.Separator);

                        coorLine.TimeStamp = Convert.ToDouble(sline[ColumnTimeStamp]);
                        coorLine.Index = numLine;
                        coorLine.X = Convert.ToDouble(sline[ColumnX]);
                        coorLine.Y = Convert.ToDouble(sline[ColumnY]);
                        coorLine.Z = Convert.ToDouble(sline[ColumnZ]);
                        if (sline.Count() > 4)
                        {
                            coorLine.Sigma = Convert.ToDouble(sline[ColumnSigma]);
                        }
                        else
                        {
                            coorLine.Sigma = -1;
                        }

                        dataStream.AddDataLine(coorLine);

                        numLine++;

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

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
    public sealed class RTKLibPosImporter : Importer
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Others")]
        [Description("No. of lines of the header.")]
        [DisplayName("Header Row Number")]
        public int HeaderRowNo { get; set; }

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
        [Description("The column separator.")]
        [DisplayName("Separator")]
        public char Separator { get; set; }

        private String filePath;
        private DataStream dataStream;

        public static RTKLibPosImporterFactory Factory
        {
            get
            { 
                return new RTKLibPosImporterFactory();
            }
        }

        public class RTKLibPosImporterFactory : ImporterFactory
        {
            public String Name { get { return "RTK Lib Position File Importer"; } }
            public String Description { get { return "RTK Lib POS File"; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.CoordinateDataStream;
            }

            public Importer Create(string filePath, DataStream dataStream, Project project, IMessanger messanger=null)
            {
                RTKLibPosImporter importer = new RTKLibPosImporter(project, messanger, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private RTKLibPosImporter(Project project, IMessanger messanger, String name, String description, String filePath, DataStream dataStream) : base(project, messanger, name, description)
        {
            HeaderRowNo = 26;
            ColumnTimeStamp = 1;
            ColumnX = 2;
            ColumnY = 3;
            ColumnZ = 4;
            ColumnSigma = 14; // ratio
            Separator = ' ';
            this.filePath = filePath;
            this.dataStream = dataStream;
        }
    

        public override string SupportedFileFormats()
        {
            return "POS files (*.pos)|*.pos|All files (*.*)|*.*";
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
                        if (IsCanceled())
                        {
                            dataStream.Close();
                            reader.Close();
                            WriteMessage("Importing canceled!", null, null, ConsoleMessageType.Warning);
                            return AlgorithmResult.Partial;
                        }

                        String line = reader.ReadLine();
                        CoordinateDataLine coorLine = new CoordinateDataLine();
                        string[] sline = line.Split(this.Separator);
                        sline = (new List<string>(sline)).FindAll(x => x != "").ToArray();

                        String ts = sline[ColumnTimeStamp];
                        string[] tssplit = ts.Split(':');
                        coorLine.TimeStamp = Convert.ToInt32(tssplit[0]) * 3600 + Convert.ToInt32(tssplit[1]) * 60 + Convert.ToDouble(tssplit[2]);
                        coorLine.Index = numLine;
                        coorLine.X = Convert.ToDouble(sline[ColumnX]);
                        coorLine.Y = Convert.ToDouble(sline[ColumnY]);
                        coorLine.Z = Convert.ToDouble(sline[ColumnZ]);
                        coorLine.Sigma = Convert.ToDouble(sline[ColumnSigma]);
                        dataStream.AddDataLine(coorLine);

                        numLine++;

                        if (messanger != null)
                        {
                            messanger.Progress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));
                        }
                    }
                }
                dataStream.Close();

                if(dataStream.DataNumber == 0)
                {
                    WriteMessage("No data has been parsed!");
                    return AlgorithmResult.Failure;
                }

                WriteMessage("Parsed " + dataStream.DataNumber + " data!");
                WriteMessage("Importing is done!");
                return AlgorithmResult.Sucess;
            }
            catch(Exception ex)
            {
                WriteMessage("Importer error: " + ex.Message, null, null, ConsoleMessageType.Error);
                return AlgorithmResult.Failure;
            }
        }
    }
}

using Gaia.DataStreams;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Gaia.GaiaSystem;
using System.IO;
using Gaia.Excpetions;
using System.ComponentModel;

namespace Gaia.Import
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

        public override string Name
        {
            get
            {
                return "Coordinate importer";
            }
        }

        public override string Description
        {
            get
            {
                return "Coordinate importer. " + Environment.NewLine + "Format: {TS},{X},{Y},{Z}[,{Sigma}]";
            }
        }

        public CoordinatesImporter(Project project, IMessanger messanger = null) : base(project, messanger)
        {
            HeaderRowNo = 0;
            ColumnTimeStamp = 0;
            ColumnX = 1;
            ColumnY = 2;
            ColumnZ = 3;
            ColumnSigma = 4;
            Separator = ',';
        }

        public override DataStreamType GetDataStreamType()
        {
            return DataStreamType.CoordinateDataStream;
        }



        public override AlgorithmResult Import(string filePath, DataStream dataStream)
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

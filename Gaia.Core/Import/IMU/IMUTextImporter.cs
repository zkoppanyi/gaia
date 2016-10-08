using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Exceptions;
using Gaia.Core.DataStreams;
using Gaia.Core;

namespace Gaia.Core.Import
{
    public class IMUTextImporter : IMUImporter
    {
        private int columnTimeStamp;
        private int columnAx;
        private int columnAy;
        private int columnAz;
        private int columnWx;
        private int columnWy;
        private int columnWz;
        private double columnRatioAx;
        private double columnRatioAy;
        private double columnRatioAz;
        private double columnRatioWx;
        private double columnRatioWy;
        private double columnRatioWz;
        private int headerRowNo;
        private char separator;
        private bool parseAllYouCan;

        protected String filePath;
        protected DataStream dataStream;

        public static IMUTextImporterFactory Factory
        {
            get
            {
                return new IMUTextImporterFactory();
            }
        }

        public class IMUTextImporterFactory : ImporterFactory
        {
            public String Name { get { return "IMU Text File Reader"; } }
            public String Description { get { return "IMU Text File Reader!" + Environment.NewLine + "Format: set up the format"; } }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column timestamp. Unit: [s].")]
            [DisplayName("Column Time stamp")]
            public int ColumnTimeStamp { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Acceleration X.")]
            [DisplayName("Column Acceleration X")]
            public int ColumnAx { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Acceleration Y.")]
            [DisplayName("Column Acceleration Y")]
            public int ColumnAy { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Acceleration Z.")]
            [DisplayName("Column Acceleration Z")]
            public int ColumnAz { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Gyro X.")]
            [DisplayName("Column Gyro X")]
            public int ColumnWx { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Gyro Y.")]
            [DisplayName("Column Gyro Y")]
            public int ColumnWy { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Columns")]
            [Description("The number of the column Gyro Z.")]
            [DisplayName("Column Gyro Z")]
            public int ColumnWz { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Acceleration X will be multiplied with this value.")]
            [DisplayName("Ratio Acceleration X")]
            public double ColumnRatioAx { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Acceleration Y will be multiplied with this value.")]
            [DisplayName("Ratio Acceleration Y")]
            public double ColumnRatioAy { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Acceleration Z will be multiplied with this value.")]
            [DisplayName("Ratio Acceleration Z")]
            public double ColumnRatioAz { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Gyro X will be multiplied with this value.")]
            [DisplayName("Ratio Gyro X")]
            public double ColumnRatioWx { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Gyro Y will be multiplied with this value.")]
            [DisplayName("Ratio Gyro Y")]
            public double ColumnRatioWy { get; set; }

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Modify")]
            [Description("This Gyro Z will be multiplied with this value.")]
            [DisplayName("Ratio Gyro Z")]
            public double ColumnRatioWz { get; set; }

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

            [Browsable(true)]
            [ReadOnly(false)]
            [Category("Others")]
            [Description("False, if stop when one line cannot be processed")]
            [DisplayName("Parse all you can")]
            public bool ParseAllYouCan { get; set; }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.IMUDataStream;
            }

            public IMUTextImporterFactory()
            {
                Separator = ',';
                ColumnTimeStamp = 0;
                ColumnAx = 1;
                ColumnAy = 2;
                ColumnAz = 3;
                ColumnWx = 4;
                ColumnWy = 5;
                ColumnWz = 6;
                ColumnRatioAx = 1;
                ColumnRatioAy = 1;
                ColumnRatioAz = 1;
                ColumnRatioWx = 1;
                ColumnRatioWy = 1;
                ColumnRatioWz = 1;
                ParseAllYouCan = false;
                HeaderRowNo = 0;
            }

            public Importer Create(string filePath, DataStream dataStream, Project project)
            {
              
                IMUTextImporter importer = new IMUTextImporter(project, Name, Description, filePath, dataStream, 
                    ColumnTimeStamp, ColumnAx, ColumnAy, ColumnAz, ColumnWx, ColumnWy, ColumnWz,
                    ColumnRatioAx, ColumnRatioAy, ColumnRatioAz, ColumnRatioWx, ColumnRatioWy, ColumnRatioWz, HeaderRowNo, ParseAllYouCan);

                return importer;
            }
        }

        protected IMUTextImporter(Project project, String name, String description, String filePath, DataStream dataStream,
            int columnTimeStamp, int columnAx, int columnAy, int columnAz, int columnWx, int columnWy, int columnWz,
            double columnRatioAx, double columnRatioAy, double columnRatioAz,
            double columnRatioWx, double columnRatioWy, double columnRatioWz, int headerRowNo, bool parseAllYouCan) : base(project, name, description)
        {
            this.separator = ',';
            this.columnTimeStamp = columnTimeStamp;
            this.columnAx = columnAx;
            this.columnAy = columnAy;
            this.columnAz = columnAz;
            this.columnWx = columnWx;
            this.columnWy = columnWy;
            this.columnWz = columnWz;
            this.columnRatioAx = columnRatioAx;
            this.columnRatioAy = columnRatioAy;
            this.columnRatioAz = columnRatioAz;
            this.columnRatioWx = columnRatioWx;
            this.columnRatioWy = columnRatioWy;
            this.columnRatioWz = columnRatioWz;
            this.headerRowNo = headerRowNo;
            this.parseAllYouCan = parseAllYouCan;

            this.Name = name;
            this.Description = description;
            this.dataStream = dataStream;
            this.filePath = filePath;

        }

        override public string SupportedFileFormats()
        {
            return "TXT files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
        }


        protected override AlgorithmResult run()
        {
            if (project == null)
            {
                new GaiaAssertException("Project has not been set for the importer!");
            }

            try
            {
                long numLine = 0;
                StreamReader reader = new StreamReader(filePath);

                // Header
                for (int i = 0; i < headerRowNo; i++)
                {
                    String line = reader.ReadLine();
                    numLine++;
                }

                WriteMessage("Importing...");
                dataStream.Open();

                StringBuilder message = new StringBuilder(""); 
                while (!reader.EndOfStream)
                {
                    if (IsCanceled())
                    {
                        dataStream.Close();
                        reader.Close();
                        WriteMessage("Importing canceled!", null, null, AlgorithmMessageType.Warning);
                        return AlgorithmResult.Partial;
                    }

                    String line = reader.ReadLine();
                    numLine++;
                    var line_parts = line.Split(this.separator);

                    IMUDataLine dataLine = null;

                    try
                    {
                        dataLine = new IMUDataLine();
                        dataLine.TimeStamp = Convert.ToDouble(line_parts[columnTimeStamp]);
                        dataLine.Ax = Convert.ToDouble(line_parts[columnAx]) * this.columnRatioAx;
                        dataLine.Ay = Convert.ToDouble(line_parts[columnAy]) * this.columnRatioAy;
                        dataLine.Az = Convert.ToDouble(line_parts[columnAz]) * this.columnRatioAz;
                        dataLine.Wx = Convert.ToDouble(line_parts[columnWx]) * this.columnRatioWx;
                        dataLine.Wy = Convert.ToDouble(line_parts[columnWy]) * this.columnRatioWy;
                        dataLine.Wz = Convert.ToDouble(line_parts[columnWz]) * this.columnRatioWz;
                        dataStream.AddDataLine(dataLine);
                    }
                    catch
                    {
                        WriteMessage("Cannot parse at line " + numLine + "." + Environment.NewLine);
                        if (!parseAllYouCan) break;
                    }

                    WriteProgress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));
                }

                dataStream.Close();

                if (dataStream.DataNumber == 0)
                {
                    WriteMessage("No data has been parsed!", null, null, AlgorithmMessageType.Error);
                    return AlgorithmResult.Failure;
                }

                WriteMessage("Parsed " + dataStream.DataNumber + " data!");
                WriteMessage("Importing is done!");
                return AlgorithmResult.Sucess;

            }
            catch (Exception ex)
            {
                WriteMessage("Importer error: " + ex.Message, null, null, AlgorithmMessageType.Error);
                return AlgorithmResult.Failure;
            }
        }
    }
}

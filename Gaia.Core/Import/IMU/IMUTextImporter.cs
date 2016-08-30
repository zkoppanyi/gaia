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
        protected IMUDataStream dataStream;

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

        public IMUTextImporter(Project project, IMessanger messanger = null) : base(project, messanger)
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
            HeaderRowNo = 0;
        }

        public override DataStreamType GetDataStreamType()
        {
            return DataStreamType.IMUDataStream;
        }


        override public string Description
        {
            get
            {
                return "IMU Text File Reader!" + Environment.NewLine + "Format: set up the format";
            }
        }

        override public string Name
        {
            get
            {
                return "IMU Text File Reader";
            }
        }

        override public string SupportedFileFormats()
        {
            return "TXT files (*.txt)|*.txt|CSV files (*.csv)|*.csv|All files (*.*)|*.*";
        }


        public override AlgorithmResult Import(string filePath, DataStream dataStream)
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
                for (int i = 0; i < HeaderRowNo; i++)
                {
                    String line = reader.ReadLine();
                    numLine++;
                }

                dataStream.Open();

                while (!reader.EndOfStream)
                {
                    if (IsCanceled())
                    {
                        dataStream.Close();
                        reader.Close();
                        WriteMessage("Importing canceled!", null, null, ConsoleMessageType.Warning);
                        return AlgorithmResult.Partial;
                    }

                    if (messanger != null)
                    {
                       WriteProgress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));
                    }

                    String line = reader.ReadLine();
                    numLine++;
                    var line_parts = line.Split(this.Separator);

                    IMUDataLine dataLine = null;

                    try
                    {
                        dataLine = new IMUDataLine();
                        dataLine.TimeStamp = Convert.ToDouble(line_parts[ColumnTimeStamp]);
                        dataLine.Ax = Convert.ToDouble(line_parts[ColumnAx]) * this.ColumnRatioAx;
                        dataLine.Ay = Convert.ToDouble(line_parts[ColumnAy]) * this.ColumnRatioAy;
                        dataLine.Az = Convert.ToDouble(line_parts[ColumnAz]) * this.ColumnRatioAz;
                        dataLine.Wx = Convert.ToDouble(line_parts[ColumnWx]) * this.ColumnRatioWx;
                        dataLine.Wy = Convert.ToDouble(line_parts[ColumnWy]) * this.ColumnRatioWy;
                        dataLine.Wz = Convert.ToDouble(line_parts[ColumnWz]) * this.ColumnRatioWz;
                        dataStream.AddDataLine(dataLine);
                    }
                    catch
                    {
                        WriteMessage("Cannot parse at line " + numLine + ".", null, null, ConsoleMessageType.Error);
                        break;
                    }
                }

                dataStream.Close();

                if (dataStream.DataNumber == 0)
                {
                    WriteMessage("No data has been parsed!", null, null, ConsoleMessageType.Error);
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

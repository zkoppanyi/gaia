using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.ComponentModel;

using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using Gaia.Core.ReferenceFrames;
using Gaia.Core;

namespace Gaia.Core.Import
{
    public sealed class PointsImporter : Importer
    {

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Columns")]
        [Description("The number of the column ID")]
        [DisplayName("Column ID Number")]
        public int ColumnID { get; set; }

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
        [Category("Others")]
        [Description("The column separator.")]
        [DisplayName("Separator")]
        public char Separator { get; set; }

        [Browsable(false)]
        public CRS SetCRS { get; set; }

        [Browsable(false)]
        public TRS SetTRS { get; set; }

        private String filePath;
        private DataStream dataStream;

        public static PointsImporterFactory Factory
        {
            get
            {
                return new PointsImporterFactory();
            }
        }

        public class PointsImporterFactory : ImporterFactory
        {
            public String Name { get { return "Point Importer"; } }
            public String Description { get { return "Import points to point list from text file."; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.NoDataStream;
            }

            public Importer Create(string filePath, DataStream dataStream, Project project)
            {
                PointsImporter importer = new PointsImporter(project, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private PointsImporter(Project project, String name, String description, String filePath, DataStream dataStream) : base(project, name, description)
        {
            this.ColumnID = 0;
            this.ColumnX = 1;
            this.ColumnY = 2;
            this.ColumnZ = 3;
            this.Separator = ',';
            this.Name = name;
            this.Description = description;
            this.filePath = filePath;
            this.dataStream = dataStream;
        }

        public override string SupportedFileFormats()
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
                var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);
                WriteMessage("Import stream is opened: " + filePath);
                WriteMessage("Importing...");

                using (StreamReader reader = new StreamReader(sourceStream, Encoding.UTF8))
                {
                    int lineNum = 0;
                    while (!reader.EndOfStream)
                    {
                        if (IsCanceled())
                        {
                            reader.Close();
                            WriteMessage("Importing canceled!", null, null, AlgorithmMessageType.Warning);
                            return AlgorithmResult.Partial;
                        }

                        String line = reader.ReadLine();
                        string[] sline = line.Split(this.Separator);

                        try
                        {
                            String ID = sline[ColumnID];
                            double X = Convert.ToDouble(sline[ColumnX]);
                            double Y = Convert.ToDouble(sline[ColumnY]);
                            double Z = Convert.ToDouble(sline[ColumnZ]);

                            GPoint point = new GPoint(project, ID);
                            point.X = X;
                            point.Y = Y;
                            point.Z = Z;
                            point.CRS = this.SetCRS;
                            point.TRS = this.SetTRS;

                            if (!project.PointManager.AddPoint(point))
                            {
                                WriteMessage("The following point is already exist: " + ID + " Cannot be added to the point list!");
                            }

                            lineNum++;
                        }
                        catch
                        {
                            WriteMessage("Cannot parse " + lineNum + "th line.");
                        }

                    }

                    WriteProgress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));
                }

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

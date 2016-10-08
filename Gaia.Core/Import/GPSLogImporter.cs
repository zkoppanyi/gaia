﻿using System;
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
    public sealed class GPSLogImporter : Importer
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Others")]
        [Description("TODO.")]
        [DisplayName("HPC Step Error")]
        public long HPCStepError { get; set; }

        private String filePath;
        private DataStream dataStream;

        public static GPSLogImporterFactory Factory
        {
            get
            {
                return new GPSLogImporterFactory();
            }
        }

        public class GPSLogImporterFactory : ImporterFactory
        {
            public String Name { get { return "GPSLog Importer"; } }
            public String Description { get { return "Import GPSLog txt file to load time matches between GPS and internal High Performance Counter" + Environment.NewLine + "Format: {HPC (long int16)}{NMEA message string}"; } }

            public DataStreamType GetDataStreamType()
            {
                return DataStreamType.GPSLogDataStream;
            }


            public Importer Create(string filePath, DataStream dataStream, Project project)
            {
                GPSLogImporter importer = new GPSLogImporter(project, Name, Description, filePath, dataStream);
                return importer;
            }
        }

        private GPSLogImporter(Project project, String name, String description, String filePath, DataStream dataStream) : base(project, name, description)
        {
            HPCStepError = (long)1e11;
            this.Name = name;
            this.Description = description;
            this.filePath = filePath;
            this.dataStream = dataStream;

        }

        public override string SupportedFileFormats()
        {
            return "TXT files (*.txt)|*.txt|All files (*.*)|*.*";
        }

        protected override AlgorithmResult run()
        {
            if (project == null)
            {
                new GaiaAssertException("Project has not been set for the importer!");
            }

            try
            {
                dataStream.Open();

                var sourceStream = new FileStream(filePath, FileMode.Open, FileAccess.Read);

                WriteMessage("External GPSLog stream is opened: " + filePath);
                WriteMessage("Importing...");

                long prevHpc = 0;
                using (BinaryReader reader = new BinaryReader(sourceStream, Encoding.ASCII))
                {
                    int lineNum = 0;
                    while (reader.BaseStream.Position != reader.BaseStream.Length)
                    {
                        if (IsCanceled())
                        {
                            dataStream.Close();
                            reader.Close();
                            WriteMessage("Importing canceled!", null, null, AlgorithmMessageType.Warning);
                            return AlgorithmResult.Partial;
                        }

                        //String line = reader.ReadLine();
                        GPSLogDataLine gpslogLine = new GPSLogDataLine();
                        long hpc = 0;
                        hpc = reader.ReadInt64();

                        StringBuilder nmea = new StringBuilder();
                        char newChar = ' ';
                        while ((reader.BaseStream.Position != reader.BaseStream.Length) && (newChar != '\n') && (newChar != '\r'))
                        {
                            try
                            {
                                newChar = reader.ReadChar();
                                nmea.Append(newChar);
                            }
                            catch
                            {
                                WriteMessage("Cannot read HPC in line" + lineNum);
                                break;
                            }
                        }

                        if ((prevHpc != 0) && (Math.Abs(prevHpc - hpc) > HPCStepError))
                        {
                            WriteMessage("HPC is likely to be wrong: " + hpc + " Skip!");
                            prevHpc = hpc;
                            reader.ReadChar();
                            continue;
                        }
                        prevHpc = hpc;

                        try
                        {
                            String strNmea = nmea.ToString();
                            string[] strNmeaSplit = strNmea.Split(',');
                            string strNmeaTime = strNmeaSplit[1];
                            double tNmea = Convert.ToDouble(strNmeaTime.Substring(0, 2)) * 3600 + Convert.ToDouble(strNmeaTime.Substring(2, 2)) * 60 + Convert.ToDouble(strNmeaTime.Substring(4, 2));
                            gpslogLine.TimeStamp = tNmea;
                            gpslogLine.GPSTime = tNmea;
                            gpslogLine.HPCTime = hpc;

                            dataStream.AddDataLine(gpslogLine);
                        }
                        catch
                        {
                            WriteMessage("Cannot parse NMEA message in line" + lineNum + " NMEA: " + nmea.ToString());
                        }

                        lineNum++;

                        WriteProgress((int)((double)reader.BaseStream.Position / ((double)reader.BaseStream.Length) * 100));

                        if (reader.BaseStream.Position == reader.BaseStream.Length) break;
                        reader.ReadChar();
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
                WriteMessage("Importer error: " + ex.Message, null, null, AlgorithmMessageType.Error);
                return AlgorithmResult.Failure;
            }
        }
    }
}

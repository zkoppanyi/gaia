using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing
{
    public class TimeStampOrderCheckProcessing : Algorithm
    {
        public List<DataStream> SourceStreams { get; set; }

        public static TimeStampOrderCheckProcessingFactory Factory
        {
            get
            {
                return new TimeStampOrderCheckProcessingFactory();
            }
        }

        public class TimeStampOrderCheckProcessingFactory : AlgorithmFactory
        {
            public String Name { get { return "Timestamp order check"; } }
            public String Description { get { return "Check whether the timestamps in the data stream is ordered, and change the order flag."; } }

            public TimeStampOrderCheckProcessing Create(Project project, List<DataStream> streams)
            {
                TimeStampOrderCheckProcessing algorithm = new TimeStampOrderCheckProcessing(project, Name, Description);
                algorithm.SourceStreams = streams;
                return algorithm;
            }
        }
        
        private TimeStampOrderCheckProcessing(Project project, String name, String description) : base(project, name, description)
        {
            SourceStreams = new List<DataStream>();
        }


        protected override AlgorithmResult run()
        {
            if (SourceStreams == null)
            {
                new GaiaAssertException("Source streams reference is null!");
            }

            int streamCnt = 0;
            foreach(DataStream stream in SourceStreams)
            {
                if (IsCanceled())
                {
                    WriteMessage("Processing canceled");
                    return AlgorithmResult.Failure;
                }

                stream.UpdateOrderFlag();
                streamCnt++;
                WriteProgress((double)streamCnt / SourceStreams.Count);
            }

            return AlgorithmResult.Sucess;
        }
    }
}

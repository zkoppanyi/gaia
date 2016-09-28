using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.Core.Processing.InertialSystems
{
    public class IMULevelling : Algorithm
    {
        public IMUDataStream sourceDataStream;

        private double _initilaizationTime;
        [System.ComponentModel.DisplayName("Time for calculating initialization [s]")]
        public double InitilaizationTime { get { return _initilaizationTime; } }

        public static IMULevellingFactory Factory { get { return new IMULevellingFactory(); } }

        public class IMULevellingFactory : AlgorithmFactory
        {
            [System.ComponentModel.DisplayName("Time for calculating initialization [s]")]
            public double InitilaizationTime { get; set; }

            public String Name { get { return "Evaulate an expression in data streams"; } }
            public String Description { get { return "Evaulate an expression on data lines in stream."; } }

            public IMULevellingFactory()
            {
                InitilaizationTime = 60;
            }

            public IMULevelling Create(Project project, IMessanger messanger, IMUDataStream sourceDataStream)
            {
                IMULevelling algorithm = new IMULevelling(project, messanger, this.Name, this.Description);
                algorithm.sourceDataStream = sourceDataStream;
                algorithm._initilaizationTime = InitilaizationTime;

                return algorithm;
            }
        }

        private IMULevelling(Project project, IMessanger messanger, String name, String description) : base(project, messanger, name, description)
        {
        }


        /// <summary>
        /// Calculate levelling (roll, pitch) parameters for IMU using acceleration data
        /// </summary>
        /// <returns>Result object</returns>
        public override AlgorithmResult Run()
        {
            if (sourceDataStream == null)
            {
                new GaiaAssertException("IMU data stream is null!");                
            }

            if (sourceDataStream.DataNumber == 0)
            {
                WriteMessage("IMU datastream is empty!");
                return AlgorithmResult.Failure;
            }

            sourceDataStream.Open();
            sourceDataStream.Begin();
            IMUDataLine imuLine = sourceDataStream.ReadLine() as IMUDataLine;
            

            // Calculate initial accelerations and roll and pitch
            double mean_ax = 0, mean_ay = 0, mean_az = 0;
            long data_num = 0;
            double initEnd = imuLine.TimeStamp + _initilaizationTime;
            while (imuLine.TimeStamp <= initEnd)
            {
                imuLine = sourceDataStream.ReadLine() as IMUDataLine;
                mean_ax += imuLine.Ax;
                mean_ay += imuLine.Ay;
                mean_az += imuLine.Az;
                data_num++;
            }

            mean_ax /= data_num;
            mean_ay /= data_num;
            mean_az /= data_num;

            double roll = Math.Atan2(mean_ay, mean_az) - Math.PI;
            double r = Math.Sqrt(mean_ax * mean_ax + mean_ay * mean_ay + mean_az * mean_az);
            double pitch = Math.Asin(mean_ax / r);

            sourceDataStream.InitialPitch = Utilities.ConvertRadToDeg(pitch);
            sourceDataStream.InitialRoll = Utilities.ConvertRadToDeg(roll);

            WriteMessage("");
            WriteMessage("Levelling result ");
            WriteMessage("--------------");
            WriteMessage("Initial pitch   [deg]: " + sourceDataStream.InitialPitch);
            WriteMessage("Initial roll    [deg]: " + sourceDataStream.InitialRoll);
            WriteMessage("Used sample no.   [-]: " + data_num);
            WriteProgress(100);

            this.Project.Save();
            return AlgorithmResult.Sucess;
        }
    }
    
}

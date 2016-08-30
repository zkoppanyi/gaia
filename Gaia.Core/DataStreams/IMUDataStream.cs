using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections;
using System.IO;
using System.ComponentModel;

using Gaia.Core.Import;
using Gaia.Core.ReferenceFrames;
using Gaia.Core.Processing;

namespace Gaia.Core.DataStreams
{
    [Serializable]
    public sealed class IMUDataStream : DataStream
    {
        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial roll value in deg/s.")]
        [DisplayName("Initial Roll [deg/s]")]
        public double InitialRoll { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial pitch in deg/s.")]
        [DisplayName("Initial Pitch [deg/s]")]
        public double InitialPitch { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial heading in deg/s.")]
        [DisplayName("Initial Heading [deg/s]")]
        public double InitialHeading { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial X coordinate in ECEF.")]
        [DisplayName("Initial X [m]")]
        public double InitialX { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial Y coordinate in ECEF.")]
        [DisplayName("Initial Y [m]")]
        public double InitialY { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial Z coordinate in ECEF.")]
        [DisplayName("Initial Z [m]")]
        public double InitialZ { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial velocity along X direction in NED frame and m/s.")]
        [DisplayName("Initial Velocity North [m/s]")]
        public double InitialVn { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial velocity along Y direction in NED frame and m/s.")]
        [DisplayName("Initial Velocity East [m/s]")]
        public double InitialVe { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Initial velocity along Z direction in NED frame and m/s.")]
        [DisplayName("Initial Velocity Down [m/s]")]
        public double InitialVd { get; set; }

        [Browsable(true)]
        [ReadOnly(false)]
        [Category("Initial parameters")]
        [Description("Time of the initial parameters. Start time of the data processing. ")]
        [DisplayName("Start time [s]")]
        public double StartTime { get; set; }

        private IMUDataStream(Project project, String fileId) : base(project, fileId)
        {

        }

        internal static DataStream Create(Project project, string fileId)
        {
            DataStream stream = new IMUDataStream(project, fileId);
            return stream;
        }

        public override DataLine CreateDataLine()
        {
            return new IMUDataLine();
        }

        protected override string extension { get { return "imu"; } }



    }
}

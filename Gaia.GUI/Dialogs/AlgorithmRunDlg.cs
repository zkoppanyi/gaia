using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.GUI.Dialogs;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Gaia.Core.Algorithm;

namespace Gaia.GUI.Dialogs
{
    public partial class AlgorithmRunDlg : Form
    {
        private Algorithm algorithm;

        /// <summary>
        /// If the processing fails, remove this datastreams.
        /// </summary>
        public List<DataStream> CleanUpDataStream { get; set; }

        public AlgorithmRunDlg(Algorithm algorithmFactory)
        {   
            InitializeComponent();
            this.algorithm = algorithmFactory;
            propertyGrid.SelectedObject = this.algorithm;
            CleanUpDataStream = new List<DataStream>();
        }

        private void AlgorithmRunDlg_Load(object sender, EventArgs e)
        {

        }

        private void btnRun_Click(object sender, EventArgs e)
        {
            ProgressBarDlg dlgProgress = new ProgressBarDlg(algorithm);       
            this.Close();
            dlgProgress.ShowDialog();
        }
    }
}

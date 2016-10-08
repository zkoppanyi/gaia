using System;
using System.ComponentModel;
using System.Windows.Forms;

using Gaia.Core;
using static Gaia.Core.Import.Importer;

namespace Gaia.GUI.Dialogs
{
    public partial class ProgressBarDlg : Form
    {
        private AlgorithmWorker worker;
        public AlgorithmWorker Worker { get { return worker; } }
        private Algorithm algorithm;

        public ProgressBarDlg()
        {
            InitializeComponent();
            worker = new AlgorithmWorker();
        }

        public ProgressBarDlg(Algorithm algorithm) : this()
        {            
            this.algorithm = algorithm;
            algorithm.SetWorker(worker);
        }

        private void ProgressBarDlg_Load(object sender, EventArgs e)
        {
            worker.WorkerReportsProgress = true;
            worker.WorkerSupportsCancellation = true;
            worker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            if (algorithm != null)
            {
                this.worker.SubscirbeAlgorithm(algorithm);
                this.worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    AlgorithmResult result = algorithm.Run();
                });
            }
            worker.RunWorkerAsync();
        }        


        private void bw_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if ((e.Cancelled == true))
            {
                txtMessage.AppendText("Canceled!");
            }

            else if (!(e.Error == null))
            {
                txtMessage.AppendText("Error: " + e.Error.Message);
            }

            else
            {
                txtMessage.AppendText("Done!");
            }

            btnCancel.Text = "OK";
            btnCancel.Image = global::Gaia.Properties.Resources.ok_button;
        }
        private void bw_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            this.progressBar.Text = (e.ProgressPercentage.ToString() + "%");

            if (e.ProgressPercentage > 0)
            {
                this.progressBar.Value = e.ProgressPercentage <= 100 ? e.ProgressPercentage : 100;
            }

            if (e.UserState is ReportArgs)
            {
                ReportArgs msg = e.UserState as ReportArgs;
                /*Invoke(new MethodInvoker(() =>
                {*/
                    txtMessage.AppendText(msg.Message + Environment.NewLine);
                //}));
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
           
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }

        private void ProgressBarDlg_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (worker.WorkerSupportsCancellation == true)
            {
                worker.CancelAsync();
            }

            this.Close();
        }
    }
}

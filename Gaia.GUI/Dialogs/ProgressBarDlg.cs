using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core;

namespace Gaia.GUI.Dialogs
{
    public partial class ProgressBarDlg : Form
    {
        public WorkerMessanger Worker;

        public ProgressBarDlg()
        {
            InitializeComponent();
            Worker = new WorkerMessanger();
        }

        private void ProgressBarDlg_Load(object sender, EventArgs e)
        {
            Worker.WorkerReportsProgress = true;
            Worker.WorkerSupportsCancellation = true;
            Worker.ProgressChanged += new ProgressChangedEventHandler(bw_ProgressChanged);
            Worker.RunWorkerCompleted += new RunWorkerCompletedEventHandler(bw_RunWorkerCompleted);

            Worker.RunWorkerAsync();
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
            if (Worker.WorkerSupportsCancellation == true)
            {
                Worker.CancelAsync();
            }

            this.Close();
        }

        private void txtMessage_TextChanged(object sender, EventArgs e)
        {

        }
    }
}

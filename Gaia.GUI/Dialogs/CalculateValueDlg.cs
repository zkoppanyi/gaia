using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core.Processing;
using Gaia.Core.DataStreams;
using Gaia.Core;

namespace Gaia.GUI.Dialogs
{
    public partial class CalculateValueDlg : Form
    {
        DataStream dataStream = null;

        public CalculateValueDlg(DataStream dataStream)
        {
            InitializeComponent();
            this.dataStream = dataStream;            
        }

        private void splitContainer2_Panel2_Paint(object sender, PaintEventArgs e)
        {

        }

        private void CalculateValueDlg_Load(object sender, EventArgs e)
        {
            foreach (PropertyInfo prop in dataStream.CreateDataLine().GetType().GetProperties())
            {
                if (prop.CanRead)
                {
                    if ((prop.PropertyType == typeof(double)) || ((prop.PropertyType == typeof(long))) || ((prop.PropertyType == typeof(int))))
                    {
                        listBoxProperties.Items.Add(prop);
                    }
                }
            }
            listBoxProperties.DisplayMember = "Name";
        }

        private void listBoxProperties_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void listBoxProperties_DoubleClick(object sender, EventArgs e)
        {
            PropertyInfo value = listBoxProperties.SelectedItem as PropertyInfo;
            if (value != null)
            {
                txtExpression.Text += " [" + value.Name + "] ";
            }
        }

        private void btnTest_Click(object sender, EventArgs e)
        {
            String expr = txtExpression.Text;
            txtTestResult.Text = "";
            EvaluateProcessing proc = EvaluateProcessing.Factory.Create(GlobalAccess.Project, this.dataStream, expr);
            proc.ProcessingLineNum = 10;
            proc.Run();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        public void Write(string message, string status = null, string messageGroupStr = null)
        {
            txtTestResult.Text += message + Environment.NewLine;
        }

        public void Progress(double percentage)
        {

        }

        public bool IsCanceled()
        {
            return false;
        }

        private void btnApply_Click(object sender, EventArgs e)
        {
            EvaluateProcessing algo = EvaluateProcessing.Factory.Create(GlobalAccess.Project, this.dataStream, txtExpression.Text);
            ProgressBarDlg dlgProgress = new ProgressBarDlg(algo);
            dlgProgress.ShowDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            AddToExpression(" := ");
        }

        private void button2_Click(object sender, EventArgs e)
        {
            AddToExpression(" + ");
        }

        private void button3_Click(object sender, EventArgs e)
        {
            AddToExpression(" - ");
        }

        private void button5_Click(object sender, EventArgs e)
        {
            AddToExpression(" ( ");
        }

        private void AddToExpression(String str)
        {
            txtExpression.Text = txtExpression.Text + str;
        }

        private void button7_Click(object sender, EventArgs e)
        {
            AddToExpression(" ) ");
        }

        private void button8_Click(object sender, EventArgs e)
        {
            AddToExpression(" = ");
        }

        private void button4_Click(object sender, EventArgs e)
        {
            AddToExpression(" * ");
        }

        private void button6_Click(object sender, EventArgs e)
        {
            AddToExpression(" / ");
        }

        private void button9_Click(object sender, EventArgs e)
        {
            txtExpression.Text = " ";
        }
    }
}

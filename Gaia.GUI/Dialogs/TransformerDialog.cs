using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using Gaia.Core.ReferenceFrames;
using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaia.GUI.Dialogs
{
    public partial class TransformerDialog : Form
    {
        IEnumerable<GPoint> points;
        CoordinateDataStream dataStream;

        public TransformerDialog(IEnumerable<GPoint> points)
        {
            InitializeComponent();
            this.points = points;
        }

        public TransformerDialog(CoordinateDataStream dataStream)
        {
            InitializeComponent();
            this.dataStream = dataStream;
        }

        private void CoordinateTransformDialog_Load(object sender, EventArgs e)
        {
            List<IInfo> list = SRIDDatabase.Instance.SRIDList.ToList<IInfo>();
            cmbCRS.DataSource = list;
            cmbCRS.DisplayMember = "Name";
            cmbCRS.SelectedItem = dataStream.CRS;

            cmbTRS.DataSource = GlobalAccess.Project.TimeFrames;
            cmbTRS.DisplayMember = "Name";
            cmbTRS.SelectedItem = dataStream.TRS;
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void cmbCRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            IInfo crs = (IInfo)cmbCRS.SelectedItem;
            if (crs != null)
            {
                txtCRS.Text = "WKT: " + crs.WKT;
            }
        }

        private void cmbTRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            TRS trs = (TRS)cmbTRS.SelectedItem;
            if (trs != null)
            {
                lblTRSDescription.Text = "Description: " + trs.Description;
            }
        }

        private void btnImport_Click(object sender, EventArgs e)
        {
            // CRS Transform
            IInfo crs = (IInfo)cmbCRS.SelectedItem;
            if (crs != null)
            {
                CRS toCRS = new CRS(crs.Name, crs.WKT);
                if (((dataStream != null) && (dataStream.CRS.WKT != crs.WKT)) || (points != null))
                {

                    try
                    {
                        if (points != null)
                        {
                            CoordinateTransformerForPoints transformer = CoordinateTransformerForPoints.Factory.Create(GlobalAccess.Project, points, toCRS);
                            ProgressBarDlg dlgProgress = new ProgressBarDlg(transformer);
                            dlgProgress.ShowDialog();
                        }
                        else if (dataStream != null)
                        {

                            CoordinateDataStream output = GlobalAccess.Project.DataStreamManager.CreateDataStream(DataStreamType.CoordinateDataStream) as CoordinateDataStream;
                            CoordinateTransformerForDataStreams transformer = CoordinateTransformerForDataStreams.Factory.Create(GlobalAccess.Project, dataStream, output, toCRS);
                            ProgressBarDlg dlgProgress = new ProgressBarDlg(transformer);
                            dlgProgress.ShowDialog();
                        }
                        

                        GlobalAccess.WriteConsole("Points are transformed!");
                        this.Close();

                    }
                    catch
                    {
                        MessageBox.Show("Problem is occured during the transformation!", "Transformation error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
                
            }

            TRS trs = (TRS)cmbTRS.SelectedItem;
            if ((dataStream!=null) && (trs != dataStream.TRS))
            {
                new NotImplementedException();
            }
        }


        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}

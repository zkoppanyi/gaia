using ProjNet.CoordinateSystems.Transformations;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core.DataStreams;
using Gaia.Core;
using Gaia.Core.ReferenceFrames;

namespace Gaia.GUI.Dialogs
{
    public partial class PointDlg : Form
    {
        public PointDlg()
        {
            InitializeComponent();
            UpdateGridView();
        }

        private void PointDlg_Load(object sender, EventArgs e)
        {
            DataGridViewComboBoxColumn columnType = (DataGridViewComboBoxColumn)pointGridView.Columns[0];
            columnType.ValueType = typeof(GPointType);
            columnType.DataSource = Enum.GetValues(typeof(GPointType));

            DataGridViewComboBoxColumn columnRole = (DataGridViewComboBoxColumn)pointGridView.Columns[1];
            columnRole.ValueType = typeof(GPointRole);
            columnRole.DataSource = Enum.GetValues(typeof(GPointRole));
        }

        public void UpdateGridView()
        {
            pointGridView.DataSource = null;
            pointGridView.DataSource = new List<GPoint>(GlobalAccess.Project.PointsList);
            pointGridView.Update();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            UpdateGridView();
        }

        private void openPointPropertyDlg()
        {
            if (pointGridView.SelectedRows.Count == 1)
            {
                PropertiesDlg dlg = new PropertiesDlg((GaiaObject)pointGridView.SelectedRows[0].DataBoundItem);
                dlg.ShowDialog();
                UpdateGridView();
            }
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openPointPropertyDlg();
        }

        private void dataStreamGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            openPointPropertyDlg();
        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ImportDlg dlg = new ImportDlg();
            dlg.ShowDialog();
        }

        private void toolStripDeleteStream_Click(object sender, EventArgs e)
        {
            foreach (DataGridViewRow row in pointGridView.SelectedRows)
            {
                GPoint pt = (GPoint)row.DataBoundItem;
                GlobalAccess.Project.PointManager.RemovePoint(pt.Name);
            }

            UpdateGridView();
        }

        private void toolStripButton4_Click_1(object sender, EventArgs e)
        {
           

        }

        private void toolStripButton5_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog exportDataWindowDialog = new SaveFileDialog();

            exportDataWindowDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            exportDataWindowDialog.FilterIndex = 2;
            exportDataWindowDialog.RestoreDirectory = true;

            if (exportDataWindowDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = exportDataWindowDialog.OpenFile()) != null)
                {
                    TextWriter sw = new StreamWriter(myStream);

                    int colscount = pointGridView.Columns.Count;
                    for (int j = 0; j < colscount; j++)
                    {
                        DataGridViewColumn col = pointGridView.Columns[j];
                        sw.Write(col.HeaderText);
                        if (j != colscount - 1) sw.Write(",");
                    }
                    sw.Write(Environment.NewLine);

                    int rowcount = pointGridView.Rows.Count;
                    for (int i = 0; i < rowcount; i++)
                    {
                        for (int j = 0; j < colscount; j++)
                        {
                            sw.Write(pointGridView.Rows[i].Cells[j].Value.ToString());
                            if (j != colscount - 1) sw.Write(",");
                        }
                        sw.Write(Environment.NewLine);
                    }
                    sw.Close();
                    myStream.Close();
                }
                GlobalAccess.WriteConsole("Text file was created from Data window.", "Text file saved!");
            }
        }

        private void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pointGridView.SelectedRows.Count == 0) return;

            IList<GPoint> selectedPoints = new List<GPoint>();
            foreach (DataGridViewRow row in pointGridView.SelectedRows)
            {
                GPoint pt = (GPoint)row.DataBoundItem;
                selectedPoints.Add(pt);
            }

            TransformerDialog dlg = new TransformerDialog(selectedPoints);
            dlg.ShowDialog();
        }

        private void addNewPointToolStripMenuItem_Click(object sender, EventArgs e)
        {
            GPoint pt = new GPoint(GlobalAccess.Project, "Unknown New Point");
            int pointId = 1;
            while(!GlobalAccess.Project.PointManager.AddPoint(pt))
            {
                pt.Name = "Unknown New Point " + pointId;
                pointId++;
            }
            UpdateGridView();
        }

        private void pointGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void pointGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            /*if(pointGridView.Columns[e.ColumnIndex].HeaderText.Contains("DMS"))
            {
                GPoint pt = pointGridView.SelectedRows[0].DataBoundItem as GPoint;
                var myValue = pointGridView[e.ColumnIndex, e.RowIndex].Value.ToString();

            }*/

        }
    }
}

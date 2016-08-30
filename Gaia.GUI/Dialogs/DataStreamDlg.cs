using Gaia.Core.Processing;
using Gaia.DataStreams;
using Gaia.Excpetions;
using Gaia.GaiaSystem;
using Gaia.Processing;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Numerics.RootFinding;
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
    public partial class DataStreamDlg : Form
    {
        public DataStreamDlg()
        {
            InitializeComponent();
            UpdateGridView();
        }

        public override void Refresh()
        {
            base.Refresh();
            this.UpdateGridView();
        }

        private void DataStreamDlg_Load(object sender, EventArgs e)
        {
        }

        private void dataStreamGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void toolStripShowData_Click(object sender, EventArgs e)
        {
            if (dataStreamGridView.SelectedRows.Count > 0)
            {
                DataStream stream = (DataStream)dataStreamGridView.SelectedRows[0].DataBoundItem;
                DataViewDlg dlg = new DataViewDlg(stream);
                dlg.MdiParent = this.MdiParent;
                dlg.Visible = true;
            }
        }

        public void UpdateGridView()
        {
            int selectedIndex = -1;
            if (dataStreamGridView.SelectedRows.Count > 0)
            {
                selectedIndex = dataStreamGridView.SelectedRows[0].Index;
            }

            dataStreamGridView.DataSource = null;
            dataStreamGridView.DataSource = new List<DataStream>(GlobalAccess.Project.DataStreams);
            dataStreamGridView.Update();

            if ((dataStreamGridView.Rows.Count > 0) && (selectedIndex >= 0) && (selectedIndex < dataStreamGridView.Rows.Count))
            {
                foreach (DataGridViewRow row in dataStreamGridView.SelectedRows)
                {
                    row.Selected = false;
                }
                dataStreamGridView.Rows[selectedIndex].Selected = true;
                dataStreamGridView.CurrentCell = dataStreamGridView.SelectedCells[0];
            }
        }

        private void toolStripDeleteStream_Click(object sender, EventArgs e)
        {
            if (dataStreamGridView.SelectedRows.Count > 0)
            {
                DataStream stream = (DataStream)dataStreamGridView.SelectedRows[0].DataBoundItem;
                GlobalAccess.Project.DataStreamManager.RemoveDataStream(stream);
                UpdateGridView();
            }
        }

        private void dataStreamGridView_ColumnHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {

        }

        private void openDataStreamPropertyDlg()
        {
            if (dataStreamGridView.SelectedRows.Count == 1)
            {
                PropertiesDlg dlg = new PropertiesDlg((GaiaObject)dataStreamGridView.SelectedRows[0].DataBoundItem);
                dlg.ShowDialog();
                UpdateGridView();
            }
        }
        private void dataStreamGridView_RowHeaderMouseDoubleClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            openDataStreamPropertyDlg();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            openDataStreamPropertyDlg();
        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            ImportDlg dlg = new ImportDlg();
            dlg.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void correctInternalTimestampsToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void toolStripButton4_Click(object sender, EventArgs e)
        {

        }

        private void correctTimingToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void calculateTrajectoryToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataStream dataStream = (DataStream)(dataStreamGridView.SelectedRows[0].DataBoundItem);

            if (dataStream is UWBDataStream)
            {
                ProgressBarDlg dlgProgress = new ProgressBarDlg();
                UWBProcessing proc = new UWBProcessing(GlobalAccess.Project, dlgProgress.Worker);
                dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    proc.CalculateTrajectory(dataStream as UWBDataStream);
                });
                dlgProgress.ShowDialog();
            }

            if (dataStream is IMUDataStream)
            {
                ProgressBarDlg dlgProgress = new ProgressBarDlg();
                IMUProcessing proc = new IMUProcessing(GlobalAccess.Project, dlgProgress.Worker);
                dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                {
                    CoordinateAttitudeDataStream output = GlobalAccess.Project.DataStreamManager.CreateDataStream(DataStreamType.CoordinateAttitudeDataStream) as CoordinateAttitudeDataStream;
                    output.Name = dataStream.Name + " Trajectory";
                    output.Description = "Trajectory calculated from " + dataStream.Name + " IMU data stream.";
                    AlgorithmResult result = proc.CalculateTrajectory(dataStream as IMUDataStream, output);
                    if ((result == AlgorithmResult.Failure) ||
                        (result == AlgorithmResult.InputMissing))
                    {
                        GlobalAccess.Project.DataStreamManager.RemoveDataStream(output);
                    }
                });
                dlgProgress.ShowDialog();
            }
        }

        private void transformCRSToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void fileToolStripMenuItem_Click(object sender, EventArgs e)
        {

        }

        private void transformToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DataStream dataStream = (DataStream)(dataStreamGridView.SelectedRows[0].DataBoundItem);
            if (dataStream is CoordinateDataStream)
            {
                TransformerDialog dlg = new TransformerDialog(dataStream as CoordinateDataStream);
                dlg.ShowDialog();
            }
        }

        private void addNewDataStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            new NotImplementedException();
        }

        private void iMUInitializationToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (dataStreamGridView.SelectedRows.Count >= 2)
            {
                IMUDataStream imuStream = null;
                CoordinateDataStream coorStream = null;
                foreach (DataGridViewRow row in dataStreamGridView.SelectedRows)
                {
                    if (row.DataBoundItem is IMUDataStream)
                    {
                        imuStream = row.DataBoundItem as IMUDataStream;
                    }

                    if (row.DataBoundItem is CoordinateDataStream)
                    {
                        coorStream = row.DataBoundItem as CoordinateDataStream;
                    }

                    if ((coorStream != null) && (imuStream != null))
                    {
                        // Open Progressbar dialog
                        ProgressBarDlg dlgProgress = new ProgressBarDlg();
                        IMUProcessing proc = new IMUProcessing(GlobalAccess.Project, dlgProgress.Worker);
                        dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
                        {
                            proc.CalculateInitilazationWithCoordinates(imuStream, coorStream);
                        });
                        dlgProgress.ShowDialog();
                    }
                }
            }
        }
    }
}

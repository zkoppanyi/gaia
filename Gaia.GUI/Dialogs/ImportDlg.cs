using ProjNet.CoordinateSystems;
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

using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Core.Import;
using Gaia.Core.Processing;
using Gaia.Core.ReferenceFrames;

namespace Gaia.GUI.Dialogs
{
    public partial class ImportDlg : Form
    {
        private List<Importer> importers = new List<Importer>();

        public ImportDlg()
        {
            InitializeComponent();
            this.Refresh();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        public override void Refresh()
        {
            base.Refresh();

            if ((txtFileLocation.Text != "") && (cmbImport.SelectedItem != null))
            {
                btnImport.Enabled = true;
            }
            else
            {
                btnImport.Enabled = false;
            }

            Importer importer = (Importer)cmbImport.SelectedItem;
            tabControl.TabPages.Remove(tabPageReferenceFrame);

            if (importer != null)
            {
                txtDesc.Text = importer.Description;

                if (importer is PointsImporter)
                {
                    tabControl.TabPages.Add(tabPageReferenceFrame);

                    List<IInfo> list = SRIDDatabase.Instance.SRIDList.ToList<IInfo>();
                    cmbCRS.DataSource = list;
                    cmbCRS.DisplayMember = "Name";

                    cmbTRS.DataSource = GlobalAccess.Project.TimeFrames;
                    cmbTRS.DisplayMember = "Name";
                }

                propertyGridImporter.SelectedObject = importer;
            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            Importer importer = (Importer)cmbImport.SelectedItem;

            if (importer is PointsImporter)
            {
                PointsImporter ptImporter = importer as PointsImporter;
                IInfo crs = (IInfo)cmbCRS.SelectedItem;
                TRS trs = (TRS)cmbTRS.SelectedItem;
                ptImporter.SetCRS = new CRS(crs.Name, crs.WKT);
                ptImporter.SetTRS = trs;
            }

            String path = txtFileLocation.Text;

            if (!File.Exists(path))
            {
                MessageBox.Show("Invalid source file: " + path, "Cannot be imported", MessageBoxButtons.OK, MessageBoxIcon.Error);
                //GlobalAccess.WriteConsole("Invalid source file: " + path, "Cannot be imported", ConsoleTextType.Error);
                return;
            }

            // Open Progressbar dialog
            ProgressBarDlg dlgProgress = new ProgressBarDlg();
            importer.SetMessanger(dlgProgress.Worker);
            bool error = false;
            DataStream stream = null;
            dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
            {
                try
                {
                    stream = GlobalAccess.Project.DataStreamManager.ImportDataStream(path, importer);

                    if (stream != null)
                    {
                        if (!(importer is PointsImporter))
                        {
                            stream.Name = Path.GetFileNameWithoutExtension(path);
                            stream.Description = "Imported from " + path;
                        }

                        if (importer is GPSLogImporter)
                        {
                            GlobalAccess.Project.ClockErrorModels.Add(stream as IClockErrorModel);
                        }
                    }

                }
                catch (Exception ex)
                {
                    error = true;
                    dlgProgress.Worker.Write("Cannot import the file. The problem: " + ex, "Cannot import", null, Core.ConsoleMessageType.Error);
                }
            });
            dlgProgress.ShowDialog();

            GlobalAccess.RefreshMainForm();
            GlobalAccess.WriteConsole("File is imported: " + path, "File is imported!");

            // Show the DataStream Properties dialog
            if ((!(importer is PointsImporter)) && (error == false) && (stream != null))
            {
                PropertiesDlg dlg = new PropertiesDlg(stream);
                dlg.Location = this.Location;
                dlg.ShowDialog();
            }

            this.Close();

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void ImportUWBDlg_Load(object sender, EventArgs e)
        {
            foreach (Importer imp in GlobalAccess.Importers)
            {
                importers.Add(imp);
            }

            cmbImport.DataSource = importers;
            //cmbImport.DataSource = GlobalAccess.Project.Importers.Where<Importer>((obj) => obj is UWBImporter).ToList<UWBImporter>();
            cmbImport.DisplayMember = "Name";
            if (cmbImport.Items.Count > 0)
            {
                cmbImport.SelectedIndex = 0;
            }
        }

        private void cmbImport_SelectedIndexChanged(object sender, EventArgs e)
        {            
            this.Refresh();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "All files (*.*)|*.*";
            if (cmbImport.SelectedItem != null)
            {
                Importer importer = (Importer)cmbImport.SelectedItem;
                openFileDialog.Filter = importer.SupportedFileFormats();
            }

            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                txtFileLocation.Text = openFileDialog.FileName;
            }
        }

        private void groupBox2_Enter(object sender, EventArgs e)
        {

        }

        private void tabPage1_Click(object sender, EventArgs e)
        {

        }

        private void txtFileLocation_TextChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}

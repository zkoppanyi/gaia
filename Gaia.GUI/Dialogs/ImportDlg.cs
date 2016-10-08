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
        private List<Importer.ImporterFactory> importerFactories = new List<Importer.ImporterFactory>();

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

            Importer.ImporterFactory importerFactory = (Importer.ImporterFactory)cmbImport.SelectedItem;
            tabControl.TabPages.Remove(tabPageReferenceFrame);

            if (importerFactory != null)
            {
                txtDesc.Text = importerFactory.Description;

                if (importerFactory is PointsImporter.PointsImporterFactory)
                {
                    tabControl.TabPages.Add(tabPageReferenceFrame);

                    List<IInfo> list = SRIDDatabase.Instance.SRIDList.ToList<IInfo>();
                    cmbCRS.DataSource = list;
                    cmbCRS.DisplayMember = "Name";

                    cmbTRS.DataSource = GlobalAccess.Project.TimeFrames;
                    cmbTRS.DisplayMember = "Name";
                }

                propertyGridImporter.SelectedObject = importerFactory;
            }
        }


        private void btnImport_Click(object sender, EventArgs e)
        {
            Importer.ImporterFactory importerFactory = (Importer.ImporterFactory)cmbImport.SelectedItem;

            if (importerFactory is PointsImporter.PointsImporterFactory)
            {
                PointsImporter.PointsImporterFactory ptImporterFactory = importerFactory as PointsImporter.PointsImporterFactory;
                /*IInfo crs = (IInfo)cmbCRS.SelectedItem;
                TRS trs = (TRS)cmbTRS.SelectedItem;
                ptImporter.SetCRS = new CRS(crs.Name, crs.WKT);
                ptImporter.SetTRS = trs;*/
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
            bool error = false;
            DataStream stream = null;
            dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
            {
                try
                {
                    GlobalAccess.Project.DataStreamManager.ImportMessage += delegate (object sender2, AlgorithmMessageEventArgs e2)
                    {
                        dlgProgress.Worker.WriteMessage(e2.Message, e2.Status, e2.MessageGroupStr, e2.MessageType);
                    };

                    GlobalAccess.Project.DataStreamManager.ImportProgress += delegate (object sender2, AlgorithmProgressEventArgs e2)
                    {
                        dlgProgress.Worker.WriteProgress(e2.Progress);
                    };

                    GlobalAccess.Project.DataStreamManager.ImportCompleted += delegate (object sender2, AlgorithmResult e2)
                    {
                        if(e2 != AlgorithmResult.Sucess)
                        {
                            error = true;
                        }
                    };

                    stream = GlobalAccess.Project.DataStreamManager.ImportDataStream(path, importerFactory, dlgProgress.Worker);

                    if (stream != null)
                    {
                        if (!(importerFactory is PointsImporter.PointsImporterFactory))
                        {
                            stream.Name = Path.GetFileNameWithoutExtension(path);
                            stream.Description = "Imported from " + path;
                        }
                    }

                }
                catch (Exception ex)
                {
                    error = true;
                    dlgProgress.Worker.WriteMessage("Cannot import the file. The problem: " + ex, "Cannot import", null, Core.AlgorithmMessageType.Error);
                }
            });

            dlgProgress.Worker.RunWorkerCompleted += delegate (object sender2, RunWorkerCompletedEventArgs e2) {

                GlobalAccess.RefreshMainForm();

                if ((e2.Cancelled == false) && (e2.Error == null) && (error == false))
                {
                    
                    GlobalAccess.WriteConsole("File is imported: " + path, "File is imported!");

                    // Show the DataStream Properties dialog
                    if ((!(importerFactory is PointsImporter.PointsImporterFactory)) && (error == false) && (stream != null))
                    {
                        PropertiesDlg dlg = new PropertiesDlg(stream);
                        dlg.Location = this.Location;
                        dlg.ShowDialog();
                    }
                }
                else
                {
                    GlobalAccess.WriteConsole("File cannot be imported!");
                }
            };
            dlgProgress.ShowDialog();

            this.Close();

        }

        private void tabPage2_Click(object sender, EventArgs e)
        {

        }

        private void ImportUWBDlg_Load(object sender, EventArgs e)
        {
            foreach (Importer.ImporterFactory factory in GlobalAccess.ImporterFactories)
            {
                importerFactories.Add(factory);
            }

            cmbImport.DataSource = importerFactories;
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
                Importer.ImporterFactory importer = (Importer.ImporterFactory)cmbImport.SelectedItem;
                //openFileDialog.Filter = importer.SupportedFileFormats(); // TODO
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

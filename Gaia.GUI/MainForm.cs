using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using MathNet.Numerics;

using Gaia.Core.ReferenceFrames;
using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Core.Processing;
using Gaia.GUI.Dialogs;
using Gaia.GUI.DataAcquisition;

namespace Gaia.GUI
{
    public partial class MainForm : Form
    {
        private Dialogs.Console console;
        private DataStreamDlg dataStreamDlg;
        private PointDlg pointDlg;
        private List<FigureDlg> figureDlgs;
        public IEnumerable<FigureDlg> FigureDlgs
        {
            get
            {
                foreach (var dlgs in figureDlgs) yield return dlgs;
            }
        }


        public MainForm()
        {
            InitializeComponent();
            figureDlgs = new List<FigureDlg>();
        }

        public FigureDlg CreateFigureDialog()
        {
            String name = "Figure " + (figureDlgs.Count + 1);
            FigureDlg fig = new FigureDlg(name);
            fig.MdiParent = this;
            this.figureDlgs.Add(fig);
            return fig;
        }

        public void RemoveFigureDialog(FigureDlg dlg)
        {
            this.figureDlgs.Remove(dlg);
        }

        public override void Refresh()
        {
            base.Refresh();
            if (GlobalAccess.Project == null)
            {
                saveProjectToolStripMenuItem.Enabled = false;
                importDataStreamToolStripMenuItem.Enabled = false;
                toolStripDataStream.Enabled = false;
                toolStripPoints.Enabled = false;
                dataAcquisitionToolStripMenuItem.Enabled = false;
            }
            else
            {
                saveProjectToolStripMenuItem.Enabled = true;
                importDataStreamToolStripMenuItem.Enabled = true;
                toolStripDataStream.Enabled = true;
                toolStripPoints.Enabled = true;
                dataAcquisitionToolStripMenuItem.Enabled = true;
            }

            if (Properties.Settings.Default.PreviousProject != "")
            {
                ToolStripMenuItem item = new ToolStripMenuItem();
                item.Name = "toolStripMenuItemPreviousProject1";
                item.Tag = Properties.Settings.Default.PreviousProject;
                item.Text = Properties.Settings.Default.PreviousProject;
                item.Click += new EventHandler(PreviousProjectMenuItemClickHandler);
                recentProjectsToolStripMenuItem.DropDownItems.Clear();
                recentProjectsToolStripMenuItem.DropDownItems.Add(item);
            }

            if (this.dataStreamDlg != null)
            {
                dataStreamDlg.Refresh();
            }

            if (this.pointDlg != null)
            {
                pointDlg.Refresh();
            }

        }


        private void createNewProject()
        {
            NewProjectDlg dlg = new NewProjectDlg();
            dlg.ShowDialog();
            this.Refresh();
        }

        private void openProject(String projectPath)
        {
            GlobalAccess.Project = Project.Load(projectPath);
            GlobalAccess.Project.Clean();

            Properties.Settings.Default.PreviousProject = projectPath;
            Properties.Settings.Default.Save();

            this.Refresh();
            this.WriteConsole("Project has been loaded: " + GlobalAccess.Project.Location, "Project has been loaded!");
        }

        private void saveProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (GlobalAccess.Project != null)
            {
                GlobalAccess.Project.Save();
                this.WriteConsole("Project has been saved: " + GlobalAccess.Project.Location, "Project has been saved!");
            }
                 
        }

        private void openProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();

            openFileDialog.Filter = "Gaia Project files (*.gpj)|*.gpj|All files (*.*)|*.*";
            openFileDialog.FilterIndex = 1;
            openFileDialog.RestoreDirectory = true;

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                try
                {
                    openProject(openFileDialog.FileName);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Error: Could not read file from disk. Original error: " + ex.Message);
                }
            }

            this.Refresh();         
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            console = new Dialogs.Console();
            console.Visible = true;
            console.MdiParent = this;
            this.Refresh();
        }

        private void PreviousProjectMenuItemClickHandler(object sender, EventArgs e)
        {
            ToolStripMenuItem clickedItem = (ToolStripMenuItem)sender;
            openProject(clickedItem.Tag as String);
        }

        
        public void WriteConsole(String str, String status = null, ConsoleMessageType type=ConsoleMessageType.Message)
        {
            console.WriteConsole(str, type);
            if (status != null)
            {
                statusMain.Text = status;
            }
        }

        private void newProjectToolStripMenuItem_Click(object sender, EventArgs e)
        {
            createNewProject();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            console.Visible = true;
            pointDlg.BringToFront();
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            if ((dataStreamDlg == null) || (dataStreamDlg.IsDisposed))
            {
                dataStreamDlg = new DataStreamDlg();
                dataStreamDlg.MdiParent = this;
                dataStreamDlg.Visible = true;
            }

            dataStreamDlg.BringToFront();
        }

        private void importDataStreamToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ImportDlg dlg = new ImportDlg();
            dlg.ShowDialog();
        }

        private void toolStripButton1_Click_1(object sender, EventArgs e)
        {
            if ((pointDlg == null) || (pointDlg.IsDisposed))
            {
                pointDlg = new PointDlg();
                pointDlg.MdiParent = this;
                pointDlg.Visible = true;
            }
            pointDlg.BringToFront();
        }

        private void toolStripButtonFigure_Click(object sender, EventArgs e)
        {
            FigureDlg dlg = this.CreateFigureDialog();
            dlg.Show();
        }

        private void menuStrip1_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {

        }

        private void wiFiFingerptingMeasurementToolStripMenuItem_Click(object sender, EventArgs e)
        {
                
        }

        private void startWiFiDataAcquisition(object sender, EventArgs e)
        {
            ToolStripMenuItem item = sender as ToolStripMenuItem;

            ProgressBarDlg dlgProgress = new ProgressBarDlg();
            dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1)
            {
                WifiFingerptiningDataStream output = GlobalAccess.Project.DataStreamManager.CreateDataStream(DataStreamType.WifiFingerprinting) as WifiFingerptiningDataStream;
                Fingerprinting proc = Fingerprinting.Factory.Create(GlobalAccess.Project, dlgProgress.Worker, output);
                if (item.Name == "continousToolStripMenuItem") proc.IsContinous = true;
                output.Name = "WiFi Fingerprinting Data";
                output.Description = "WiFi Fingerprinting Data. Acquired at " + DateTime.Now.ToString("h:mm:ss tt");
                AlgorithmResult result = proc.Run();
                if ((result == AlgorithmResult.Failure) ||
                    (result == AlgorithmResult.InputMissing))
                {
                    GlobalAccess.Project.DataStreamManager.RemoveDataStream(output);
                }
            });
            dlgProgress.ShowDialog();
        }
    }
}

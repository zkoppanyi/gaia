using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core;
using Gaia.Core.ReferenceFrames;

namespace Gaia.GUI.Dialogs
{
    public partial class NewProjectDlg : Form
    {
        public NewProjectDlg()
        {
            InitializeComponent();
        }

        private void checkCreate()
        {
            if ((this.Name != "") && (txtLocation.Text != ""))
            {
                btnCreate.Enabled = true;
            }
            else
            {
                btnCreate.Enabled = false;
            }
        }

        private void btnCreate_Click(object sender, EventArgs e)
        {
            String path = txtLocation.Text;
            if (!Directory.Exists(path))
            {
                String msg = "The specified location does not exist:  " + path;
                MessageBox.Show(this, msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                GlobalAccess.WriteConsole(msg, "Project could not be created!");
                return;
            }

            String projectLocationFolder = path;

            if (chkCreateFolder.Checked)
            {
                projectLocationFolder = path + "\\" + txtName.Text;
                if (Directory.Exists(projectLocationFolder))
                {
                    String msg = "Project folder cannot be created, because it already exists: " + projectLocationFolder;
                    MessageBox.Show(this, msg, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
                    GlobalAccess.WriteConsole(msg, "Project could not be created!");

                    return;
                }

                Directory.CreateDirectory(projectLocationFolder);
            }

            // Create default project
            Project project = Project.CreateDefaultProject(projectLocationFolder);

            project.Name = txtName.Text;
            project.Description= txtDescription.Text;
            project.SetDefault();

            project.Save();


            // Set up system variables
            GlobalAccess.Project = project;

            this.DialogResult = DialogResult.OK;
            this.Close();

            GlobalAccess.WriteConsole("Project has been created sucessfully!" + Environment.NewLine + "File location: " + GlobalAccess.Project.Location, "Project has been created!");

        }

        private void btnLocation_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog fbd = new FolderBrowserDialog();

            DialogResult result = fbd.ShowDialog();

            if (!string.IsNullOrWhiteSpace(fbd.SelectedPath))
            {
                string[] files = Directory.GetFiles(fbd.SelectedPath);
                if (files.Length != 0)
                {
                    DialogResult rst = MessageBox.Show(this, "The folder is not empty! Are you sure you want to create the project in this folder?", this.Text, MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                    if (rst == DialogResult.No) return;
                }
                txtLocation.Text = fbd.SelectedPath;
            }
        }

        private void NewProjectDlg_Load(object sender, EventArgs e)
        {
            checkCreate();
        }

        private void txtName_TextChanged(object sender, EventArgs e)
        {
            checkCreate();
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {
            checkCreate();
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }
    }
}

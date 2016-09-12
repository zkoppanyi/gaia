namespace Gaia.GUI
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.statusStrip = new System.Windows.Forms.StatusStrip();
            this.statusMain = new System.Windows.Forms.ToolStripStatusLabel();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.newProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.openProjectToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.recentProjectsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.importDataStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataAcquisitionToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.wiFiFingerptingMeasurementToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripNewProject = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripDataStream = new System.Windows.Forms.ToolStripButton();
            this.toolStripPoints = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonFigure = new System.Windows.Forms.ToolStripButton();
            this.limitedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.continousToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.statusStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // statusStrip
            // 
            this.statusStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.statusMain});
            this.statusStrip.Location = new System.Drawing.Point(0, 562);
            this.statusStrip.Name = "statusStrip";
            this.statusStrip.Size = new System.Drawing.Size(897, 22);
            this.statusStrip.TabIndex = 1;
            this.statusStrip.Text = "statusStrip1";
            // 
            // statusMain
            // 
            this.statusMain.Name = "statusMain";
            this.statusMain.Size = new System.Drawing.Size(16, 17);
            this.statusMain.Text = "...";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.dataAcquisitionToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(897, 24);
            this.menuStrip1.TabIndex = 3;
            this.menuStrip1.Text = "menuStrip1";
            this.menuStrip1.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menuStrip1_ItemClicked);
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.newProjectToolStripMenuItem,
            this.saveProjectToolStripMenuItem,
            this.openProjectToolStripMenuItem,
            this.recentProjectsToolStripMenuItem,
            this.toolStripSeparator1,
            this.importDataStreamToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(37, 20);
            this.fileToolStripMenuItem.Text = "File";
            // 
            // newProjectToolStripMenuItem
            // 
            this.newProjectToolStripMenuItem.Image = global::Gaia.Properties.Resources.create_button;
            this.newProjectToolStripMenuItem.Name = "newProjectToolStripMenuItem";
            this.newProjectToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.newProjectToolStripMenuItem.Text = "New Project...";
            this.newProjectToolStripMenuItem.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // saveProjectToolStripMenuItem
            // 
            this.saveProjectToolStripMenuItem.Name = "saveProjectToolStripMenuItem";
            this.saveProjectToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.saveProjectToolStripMenuItem.Text = "Save Project...";
            this.saveProjectToolStripMenuItem.Click += new System.EventHandler(this.saveProjectToolStripMenuItem_Click);
            // 
            // openProjectToolStripMenuItem
            // 
            this.openProjectToolStripMenuItem.Name = "openProjectToolStripMenuItem";
            this.openProjectToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.openProjectToolStripMenuItem.Text = "Open Project...";
            this.openProjectToolStripMenuItem.Click += new System.EventHandler(this.openProjectToolStripMenuItem_Click);
            // 
            // recentProjectsToolStripMenuItem
            // 
            this.recentProjectsToolStripMenuItem.Name = "recentProjectsToolStripMenuItem";
            this.recentProjectsToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.recentProjectsToolStripMenuItem.Text = "Recent Projects";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(174, 6);
            // 
            // importDataStreamToolStripMenuItem
            // 
            this.importDataStreamToolStripMenuItem.Image = global::Gaia.Properties.Resources.import_database_button;
            this.importDataStreamToolStripMenuItem.Name = "importDataStreamToolStripMenuItem";
            this.importDataStreamToolStripMenuItem.Size = new System.Drawing.Size(177, 22);
            this.importDataStreamToolStripMenuItem.Text = "Import Data Stream";
            this.importDataStreamToolStripMenuItem.Click += new System.EventHandler(this.importDataStreamToolStripMenuItem_Click);
            // 
            // dataAcquisitionToolStripMenuItem
            // 
            this.dataAcquisitionToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.wiFiFingerptingMeasurementToolStripMenuItem});
            this.dataAcquisitionToolStripMenuItem.Name = "dataAcquisitionToolStripMenuItem";
            this.dataAcquisitionToolStripMenuItem.Size = new System.Drawing.Size(106, 20);
            this.dataAcquisitionToolStripMenuItem.Text = "Data Acquisition";
            // 
            // wiFiFingerptingMeasurementToolStripMenuItem
            // 
            this.wiFiFingerptingMeasurementToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.limitedToolStripMenuItem,
            this.continousToolStripMenuItem});
            this.wiFiFingerptingMeasurementToolStripMenuItem.Name = "wiFiFingerptingMeasurementToolStripMenuItem";
            this.wiFiFingerptingMeasurementToolStripMenuItem.Size = new System.Drawing.Size(175, 22);
            this.wiFiFingerptingMeasurementToolStripMenuItem.Text = "WiFi Fingerprinting";
            this.wiFiFingerptingMeasurementToolStripMenuItem.Click += new System.EventHandler(this.wiFiFingerptingMeasurementToolStripMenuItem_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripNewProject,
            this.toolStripSeparator2,
            this.toolStripDataStream,
            this.toolStripPoints,
            this.toolStripSeparator3,
            this.toolStripButton3,
            this.toolStripButtonFigure});
            this.toolStrip1.Location = new System.Drawing.Point(0, 24);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(897, 25);
            this.toolStrip1.TabIndex = 5;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripNewProject
            // 
            this.toolStripNewProject.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripNewProject.Image = global::Gaia.Properties.Resources.create_button;
            this.toolStripNewProject.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripNewProject.Name = "toolStripNewProject";
            this.toolStripNewProject.Size = new System.Drawing.Size(23, 22);
            this.toolStripNewProject.Text = "toolStripButton2";
            this.toolStripNewProject.Click += new System.EventHandler(this.newProjectToolStripMenuItem_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripDataStream
            // 
            this.toolStripDataStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDataStream.Image = global::Gaia.Properties.Resources.database_button;
            this.toolStripDataStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDataStream.Name = "toolStripDataStream";
            this.toolStripDataStream.Size = new System.Drawing.Size(23, 22);
            this.toolStripDataStream.Text = "Data Streams...";
            this.toolStripDataStream.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripPoints
            // 
            this.toolStripPoints.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripPoints.Image = global::Gaia.Properties.Resources.settings_button;
            this.toolStripPoints.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripPoints.Name = "toolStripPoints";
            this.toolStripPoints.Size = new System.Drawing.Size(23, 22);
            this.toolStripPoints.Text = "Points...";
            this.toolStripPoints.ToolTipText = "Points...";
            this.toolStripPoints.Click += new System.EventHandler(this.toolStripButton1_Click_1);
            // 
            // toolStripSeparator3
            // 
            this.toolStripSeparator3.Name = "toolStripSeparator3";
            this.toolStripSeparator3.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Gaia.Properties.Resources.info_button2;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "Console...";
            this.toolStripButton3.ToolTipText = "Console...";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButtonFigure
            // 
            this.toolStripButtonFigure.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonFigure.Image = global::Gaia.Properties.Resources.analysis;
            this.toolStripButtonFigure.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonFigure.Name = "toolStripButtonFigure";
            this.toolStripButtonFigure.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonFigure.Text = "toolStripButton1";
            this.toolStripButtonFigure.ToolTipText = "Create New Figure";
            this.toolStripButtonFigure.Click += new System.EventHandler(this.toolStripButtonFigure_Click);
            // 
            // limitedToolStripMenuItem
            // 
            this.limitedToolStripMenuItem.Name = "limitedToolStripMenuItem";
            this.limitedToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.limitedToolStripMenuItem.Text = "Fixed Samples";
            this.limitedToolStripMenuItem.Click += new System.EventHandler(this.startWiFiDataAcquisition);
            // 
            // continousToolStripMenuItem
            // 
            this.continousToolStripMenuItem.Name = "continousToolStripMenuItem";
            this.continousToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.continousToolStripMenuItem.Text = "Continuous";
            this.continousToolStripMenuItem.Click += new System.EventHandler(this.startWiFiDataAcquisition);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(897, 584);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip);
            this.Controls.Add(this.menuStrip1);
            this.IsMdiContainer = true;
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "MainForm";
            this.Text = "Gaia";
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.statusStrip.ResumeLayout(false);
            this.statusStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.StatusStrip statusStrip;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem newProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem openProjectToolStripMenuItem;
        private System.Windows.Forms.ToolStripStatusLabel statusMain;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem importDataStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripDataStream;
        private System.Windows.Forms.ToolStripButton toolStripNewProject;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripPoints;
        private System.Windows.Forms.ToolStripMenuItem recentProjectsToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
        private System.Windows.Forms.ToolStripButton toolStripButtonFigure;
        private System.Windows.Forms.ToolStripMenuItem dataAcquisitionToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem wiFiFingerptingMeasurementToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem limitedToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem continousToolStripMenuItem;
    }
}


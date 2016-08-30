namespace Gaia.GUI.Dialogs
{
    partial class DataStreamDlg
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
            this.components = new System.ComponentModel.Container();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataStreamDlg));
            this.menuStripDataStream = new System.Windows.Forms.MenuStrip();
            this.fileToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewDataStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.removeDataStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.importDataStreamToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.iMUInitializationToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.calculateTrajectoryToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripShowData = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteStream = new System.Windows.Forms.ToolStripButton();
            this.dataStreamGridView = new System.Windows.Forms.DataGridView();
            this.nameDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.descriptionDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.iDataStreamBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.dataGridViewImageColumn1 = new System.Windows.Forms.DataGridViewImageColumn();
            this.dataGridViewImageColumn2 = new System.Windows.Forms.DataGridViewImageColumn();
            this.menuStripDataStream.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStreamGridView)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDataStreamBindingSource)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStripDataStream
            // 
            this.menuStripDataStream.AllowMerge = false;
            this.menuStripDataStream.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.fileToolStripMenuItem,
            this.calculateToolStripMenuItem});
            this.menuStripDataStream.Location = new System.Drawing.Point(0, 0);
            this.menuStripDataStream.Name = "menuStripDataStream";
            this.menuStripDataStream.Size = new System.Drawing.Size(459, 24);
            this.menuStripDataStream.TabIndex = 0;
            this.menuStripDataStream.Text = "menuStrip";
            // 
            // fileToolStripMenuItem
            // 
            this.fileToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewDataStreamToolStripMenuItem,
            this.removeDataStreamToolStripMenuItem,
            this.importDataStreamToolStripMenuItem,
            this.toolStripSeparator2,
            this.transformToolStripMenuItem});
            this.fileToolStripMenuItem.Name = "fileToolStripMenuItem";
            this.fileToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.fileToolStripMenuItem.Text = "Data Stream";
            this.fileToolStripMenuItem.Click += new System.EventHandler(this.fileToolStripMenuItem_Click);
            // 
            // addNewDataStreamToolStripMenuItem
            // 
            this.addNewDataStreamToolStripMenuItem.Name = "addNewDataStreamToolStripMenuItem";
            this.addNewDataStreamToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.addNewDataStreamToolStripMenuItem.Text = "Add New Data Stream...";
            this.addNewDataStreamToolStripMenuItem.Click += new System.EventHandler(this.addNewDataStreamToolStripMenuItem_Click);
            // 
            // removeDataStreamToolStripMenuItem
            // 
            this.removeDataStreamToolStripMenuItem.Name = "removeDataStreamToolStripMenuItem";
            this.removeDataStreamToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.removeDataStreamToolStripMenuItem.Text = "Remove Data Stream...";
            this.removeDataStreamToolStripMenuItem.Click += new System.EventHandler(this.toolStripDeleteStream_Click);
            // 
            // importDataStreamToolStripMenuItem
            // 
            this.importDataStreamToolStripMenuItem.Name = "importDataStreamToolStripMenuItem";
            this.importDataStreamToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.importDataStreamToolStripMenuItem.Text = "Import Data Stream...";
            this.importDataStreamToolStripMenuItem.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(196, 6);
            // 
            // transformToolStripMenuItem
            // 
            this.transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            this.transformToolStripMenuItem.Size = new System.Drawing.Size(199, 22);
            this.transformToolStripMenuItem.Text = "Transform...";
            this.transformToolStripMenuItem.Click += new System.EventHandler(this.transformToolStripMenuItem_Click);
            // 
            // calculateToolStripMenuItem
            // 
            this.calculateToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.iMUInitializationToolStripMenuItem,
            this.calculateTrajectoryToolStripMenuItem});
            this.calculateToolStripMenuItem.Name = "calculateToolStripMenuItem";
            this.calculateToolStripMenuItem.Size = new System.Drawing.Size(68, 20);
            this.calculateToolStripMenuItem.Text = "Calculate";
            // 
            // iMUInitializationToolStripMenuItem
            // 
            this.iMUInitializationToolStripMenuItem.Name = "iMUInitializationToolStripMenuItem";
            this.iMUInitializationToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.iMUInitializationToolStripMenuItem.Text = "IMU Initialization";
            this.iMUInitializationToolStripMenuItem.Click += new System.EventHandler(this.iMUInitializationToolStripMenuItem_Click);
            // 
            // calculateTrajectoryToolStripMenuItem
            // 
            this.calculateTrajectoryToolStripMenuItem.Name = "calculateTrajectoryToolStripMenuItem";
            this.calculateTrajectoryToolStripMenuItem.Size = new System.Drawing.Size(179, 22);
            this.calculateTrajectoryToolStripMenuItem.Text = "Calculate Trajectory";
            this.calculateTrajectoryToolStripMenuItem.Click += new System.EventHandler(this.calculateTrajectoryToolStripMenuItem_Click);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton1,
            this.toolStripShowData,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripDeleteStream});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(459, 25);
            this.toolStrip.TabIndex = 1;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton3
            // 
            this.toolStripButton3.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton3.Image = global::Gaia.Properties.Resources.refresh_button;
            this.toolStripButton3.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton3.Name = "toolStripButton3";
            this.toolStripButton3.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton3.Text = "toolStripButton3";
            this.toolStripButton3.Click += new System.EventHandler(this.toolStripButton3_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Gaia.Properties.Resources.info_button2;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripShowData
            // 
            this.toolStripShowData.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripShowData.Image = global::Gaia.Properties.Resources.info_button;
            this.toolStripShowData.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripShowData.Name = "toolStripShowData";
            this.toolStripShowData.Size = new System.Drawing.Size(23, 22);
            this.toolStripShowData.Text = "toolStripButton1";
            this.toolStripShowData.Click += new System.EventHandler(this.toolStripShowData_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 25);
            // 
            // toolStripButton2
            // 
            this.toolStripButton2.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton2.Image = global::Gaia.Properties.Resources.import_database_button;
            this.toolStripButton2.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton2.Name = "toolStripButton2";
            this.toolStripButton2.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton2.Text = "toolStripButton2";
            this.toolStripButton2.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // toolStripDeleteStream
            // 
            this.toolStripDeleteStream.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripDeleteStream.Image = global::Gaia.Properties.Resources.delete_database_button;
            this.toolStripDeleteStream.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripDeleteStream.Name = "toolStripDeleteStream";
            this.toolStripDeleteStream.Size = new System.Drawing.Size(23, 22);
            this.toolStripDeleteStream.Text = "toolStripButton1";
            this.toolStripDeleteStream.Click += new System.EventHandler(this.toolStripDeleteStream_Click);
            // 
            // dataStreamGridView
            // 
            this.dataStreamGridView.AllowUserToAddRows = false;
            this.dataStreamGridView.AllowUserToDeleteRows = false;
            this.dataStreamGridView.AutoGenerateColumns = false;
            this.dataStreamGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataStreamGridView.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.dataStreamGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.dataStreamGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.dataStreamGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.dataStreamGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.dataStreamGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataStreamGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.nameDataGridViewTextBoxColumn,
            this.descriptionDataGridViewTextBoxColumn,
            this.cRSDataGridViewTextBoxColumn,
            this.tRSDataGridViewTextBoxColumn});
            this.dataStreamGridView.DataSource = this.iDataStreamBindingSource;
            dataGridViewCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle2.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle2.Font = new System.Drawing.Font("Courier New", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle2.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle2.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle2.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.dataStreamGridView.DefaultCellStyle = dataGridViewCellStyle2;
            this.dataStreamGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataStreamGridView.Location = new System.Drawing.Point(0, 49);
            this.dataStreamGridView.Name = "dataStreamGridView";
            this.dataStreamGridView.ReadOnly = true;
            this.dataStreamGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.dataStreamGridView.Size = new System.Drawing.Size(459, 275);
            this.dataStreamGridView.TabIndex = 2;
            this.dataStreamGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataStreamGridView_CellContentClick);
            this.dataStreamGridView.ColumnHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataStreamGridView_ColumnHeaderMouseDoubleClick);
            this.dataStreamGridView.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataStreamGridView_RowHeaderMouseDoubleClick);
            // 
            // nameDataGridViewTextBoxColumn
            // 
            this.nameDataGridViewTextBoxColumn.DataPropertyName = "Name";
            this.nameDataGridViewTextBoxColumn.HeaderText = "Name";
            this.nameDataGridViewTextBoxColumn.Name = "nameDataGridViewTextBoxColumn";
            this.nameDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // descriptionDataGridViewTextBoxColumn
            // 
            this.descriptionDataGridViewTextBoxColumn.DataPropertyName = "Description";
            this.descriptionDataGridViewTextBoxColumn.HeaderText = "Description";
            this.descriptionDataGridViewTextBoxColumn.Name = "descriptionDataGridViewTextBoxColumn";
            this.descriptionDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // cRSDataGridViewTextBoxColumn
            // 
            this.cRSDataGridViewTextBoxColumn.DataPropertyName = "CRS";
            this.cRSDataGridViewTextBoxColumn.HeaderText = "Coordinate Ref. Sys.";
            this.cRSDataGridViewTextBoxColumn.Name = "cRSDataGridViewTextBoxColumn";
            this.cRSDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // tRSDataGridViewTextBoxColumn
            // 
            this.tRSDataGridViewTextBoxColumn.DataPropertyName = "TRS";
            this.tRSDataGridViewTextBoxColumn.HeaderText = "Time Ref. Sys.";
            this.tRSDataGridViewTextBoxColumn.Name = "tRSDataGridViewTextBoxColumn";
            this.tRSDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // iDataStreamBindingSource
            // 
            this.iDataStreamBindingSource.DataSource = typeof(Gaia.Core.DataStreams.DataStream);
            // 
            // dataGridViewImageColumn1
            // 
            this.dataGridViewImageColumn1.DataPropertyName = "Name";
            this.dataGridViewImageColumn1.HeaderText = "";
            this.dataGridViewImageColumn1.Image = global::Gaia.Properties.Resources.delete_database_button;
            this.dataGridViewImageColumn1.ImageLayout = System.Windows.Forms.DataGridViewImageCellLayout.Stretch;
            this.dataGridViewImageColumn1.Name = "dataGridViewImageColumn1";
            this.dataGridViewImageColumn1.ReadOnly = true;
            this.dataGridViewImageColumn1.Width = 25;
            // 
            // dataGridViewImageColumn2
            // 
            this.dataGridViewImageColumn2.HeaderText = "";
            this.dataGridViewImageColumn2.Image = global::Gaia.Properties.Resources.info_button;
            this.dataGridViewImageColumn2.Name = "dataGridViewImageColumn2";
            this.dataGridViewImageColumn2.ReadOnly = true;
            this.dataGridViewImageColumn2.Width = 25;
            // 
            // DataStreamDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(459, 324);
            this.Controls.Add(this.dataStreamGridView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStripDataStream);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStripDataStream;
            this.Name = "DataStreamDlg";
            this.Text = "Data Streams";
            this.Load += new System.EventHandler(this.DataStreamDlg_Load);
            this.menuStripDataStream.ResumeLayout(false);
            this.menuStripDataStream.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataStreamGridView)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.iDataStreamBindingSource)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStripDataStream;
        private System.Windows.Forms.ToolStripMenuItem fileToolStripMenuItem;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.DataGridView dataStreamGridView;
        private System.Windows.Forms.BindingSource iDataStreamBindingSource;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn1;
        private System.Windows.Forms.DataGridViewImageColumn dataGridViewImageColumn2;
        private System.Windows.Forms.DataGridViewTextBoxColumn nameDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn descriptionDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRSDataGridViewTextBoxColumn;
        private System.Windows.Forms.ToolStripButton toolStripShowData;
        private System.Windows.Forms.ToolStripButton toolStripDeleteStream;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripMenuItem calculateToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem calculateTrajectoryToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewDataStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem removeDataStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem importDataStreamToolStripMenuItem;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripMenuItem iMUInitializationToolStripMenuItem;
    }
}
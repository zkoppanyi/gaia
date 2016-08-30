namespace Gaia.GUI.Dialogs
{
    partial class PointDlg
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
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle6 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle2 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle3 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle4 = new System.Windows.Forms.DataGridViewCellStyle();
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle5 = new System.Windows.Forms.DataGridViewCellStyle();
            this.pointGridView = new System.Windows.Forms.DataGridView();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton3 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButton5 = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.toolStripButton2 = new System.Windows.Forms.ToolStripButton();
            this.toolStripDeleteStream = new System.Windows.Forms.ToolStripButton();
            this.dataGridViewComboBoxColumn1 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn2 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn3 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn4 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.dataGridViewComboBoxColumn5 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.pointsToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.addNewPointToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.transformToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.dataGridViewComboBoxColumn6 = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.gPointBindingSource = new System.Windows.Forms.BindingSource(this.components);
            this.gPointBindingSource1 = new System.Windows.Forms.BindingSource(this.components);
            this.pointTypeDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PointRole = new System.Windows.Forms.DataGridViewComboBoxColumn();
            this.PointName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.xDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.yDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.zDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.cRSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.tRSDataGridViewTextBoxColumn = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LatDMS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.LonDMS = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.H = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.pointGridView)).BeginInit();
            this.toolStrip.SuspendLayout();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gPointBindingSource)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPointBindingSource1)).BeginInit();
            this.SuspendLayout();
            // 
            // pointGridView
            // 
            this.pointGridView.AutoGenerateColumns = false;
            this.pointGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.pointGridView.BackgroundColor = System.Drawing.SystemColors.ActiveBorder;
            this.pointGridView.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.pointGridView.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.pointGridView.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.None;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle1.BackColor = System.Drawing.SystemColors.Control;
            dataGridViewCellStyle1.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle1.ForeColor = System.Drawing.SystemColors.WindowText;
            dataGridViewCellStyle1.Format = "N4";
            dataGridViewCellStyle1.NullValue = "-";
            dataGridViewCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.True;
            this.pointGridView.ColumnHeadersDefaultCellStyle = dataGridViewCellStyle1;
            this.pointGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.pointGridView.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.pointTypeDataGridViewTextBoxColumn,
            this.PointRole,
            this.PointName,
            this.xDataGridViewTextBoxColumn,
            this.yDataGridViewTextBoxColumn,
            this.zDataGridViewTextBoxColumn,
            this.cRSDataGridViewTextBoxColumn,
            this.tRSDataGridViewTextBoxColumn,
            this.LatDMS,
            this.LonDMS,
            this.H});
            this.pointGridView.DataSource = this.gPointBindingSource;
            dataGridViewCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft;
            dataGridViewCellStyle6.BackColor = System.Drawing.SystemColors.Window;
            dataGridViewCellStyle6.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            dataGridViewCellStyle6.ForeColor = System.Drawing.SystemColors.ControlText;
            dataGridViewCellStyle6.SelectionBackColor = System.Drawing.SystemColors.Highlight;
            dataGridViewCellStyle6.SelectionForeColor = System.Drawing.SystemColors.HighlightText;
            dataGridViewCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.False;
            this.pointGridView.DefaultCellStyle = dataGridViewCellStyle6;
            this.pointGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pointGridView.Location = new System.Drawing.Point(0, 49);
            this.pointGridView.Name = "pointGridView";
            this.pointGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.pointGridView.Size = new System.Drawing.Size(1076, 259);
            this.pointGridView.TabIndex = 4;
            this.pointGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.pointGridView_CellContentClick);
            this.pointGridView.CellDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.pointGridView_CellDoubleClick);
            this.pointGridView.RowHeaderMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataStreamGridView_ColumnHeaderMouseDoubleClick);
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton3,
            this.toolStripButton1,
            this.toolStripButton5,
            this.toolStripSeparator1,
            this.toolStripButton2,
            this.toolStripDeleteStream});
            this.toolStrip.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip.Location = new System.Drawing.Point(0, 24);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(1076, 25);
            this.toolStrip.TabIndex = 3;
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
            // toolStripButton5
            // 
            this.toolStripButton5.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton5.Image = global::Gaia.Properties.Resources.export_button;
            this.toolStripButton5.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton5.Name = "toolStripButton5";
            this.toolStripButton5.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton5.Text = "toolStripButton5";
            this.toolStripButton5.Click += new System.EventHandler(this.toolStripButton5_Click);
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
            // dataGridViewComboBoxColumn1
            // 
            this.dataGridViewComboBoxColumn1.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn1.HeaderText = "Role";
            this.dataGridViewComboBoxColumn1.Name = "dataGridViewComboBoxColumn1";
            this.dataGridViewComboBoxColumn1.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn1.Width = 259;
            // 
            // dataGridViewComboBoxColumn2
            // 
            this.dataGridViewComboBoxColumn2.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn2.HeaderText = "Role";
            this.dataGridViewComboBoxColumn2.Name = "dataGridViewComboBoxColumn2";
            this.dataGridViewComboBoxColumn2.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn2.Width = 518;
            // 
            // dataGridViewComboBoxColumn3
            // 
            this.dataGridViewComboBoxColumn3.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn3.HeaderText = "Role";
            this.dataGridViewComboBoxColumn3.Name = "dataGridViewComboBoxColumn3";
            this.dataGridViewComboBoxColumn3.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn3.Width = 259;
            // 
            // dataGridViewComboBoxColumn4
            // 
            this.dataGridViewComboBoxColumn4.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn4.HeaderText = "Role";
            this.dataGridViewComboBoxColumn4.Name = "dataGridViewComboBoxColumn4";
            this.dataGridViewComboBoxColumn4.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn4.Width = 259;
            // 
            // dataGridViewComboBoxColumn5
            // 
            this.dataGridViewComboBoxColumn5.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn5.HeaderText = "Role";
            this.dataGridViewComboBoxColumn5.Name = "dataGridViewComboBoxColumn5";
            this.dataGridViewComboBoxColumn5.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn5.Width = 207;
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.pointsToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1076, 24);
            this.menuStrip1.TabIndex = 5;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // pointsToolStripMenuItem
            // 
            this.pointsToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.addNewPointToolStripMenuItem,
            this.transformToolStripMenuItem});
            this.pointsToolStripMenuItem.Name = "pointsToolStripMenuItem";
            this.pointsToolStripMenuItem.Size = new System.Drawing.Size(52, 20);
            this.pointsToolStripMenuItem.Text = "Points";
            // 
            // addNewPointToolStripMenuItem
            // 
            this.addNewPointToolStripMenuItem.Name = "addNewPointToolStripMenuItem";
            this.addNewPointToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.addNewPointToolStripMenuItem.Text = "Add New Point";
            this.addNewPointToolStripMenuItem.Click += new System.EventHandler(this.addNewPointToolStripMenuItem_Click);
            // 
            // transformToolStripMenuItem
            // 
            this.transformToolStripMenuItem.Name = "transformToolStripMenuItem";
            this.transformToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.transformToolStripMenuItem.Text = "Transform...";
            this.transformToolStripMenuItem.Click += new System.EventHandler(this.transformToolStripMenuItem_Click);
            // 
            // dataGridViewComboBoxColumn6
            // 
            this.dataGridViewComboBoxColumn6.DataPropertyName = "PointRole";
            this.dataGridViewComboBoxColumn6.HeaderText = "Role";
            this.dataGridViewComboBoxColumn6.Name = "dataGridViewComboBoxColumn6";
            this.dataGridViewComboBoxColumn6.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.dataGridViewComboBoxColumn6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            this.dataGridViewComboBoxColumn6.Width = 207;
            // 
            // gPointBindingSource
            // 
            this.gPointBindingSource.DataSource = typeof(Gaia.DataStreams.GPoint);
            // 
            // gPointBindingSource1
            // 
            this.gPointBindingSource1.DataSource = typeof(Gaia.DataStreams.GPoint);
            // 
            // pointTypeDataGridViewTextBoxColumn
            // 
            this.pointTypeDataGridViewTextBoxColumn.DataPropertyName = "PointType";
            this.pointTypeDataGridViewTextBoxColumn.HeaderText = "Type";
            this.pointTypeDataGridViewTextBoxColumn.Name = "pointTypeDataGridViewTextBoxColumn";
            this.pointTypeDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.pointTypeDataGridViewTextBoxColumn.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // PointRole
            // 
            this.PointRole.DataPropertyName = "PointRole";
            this.PointRole.HeaderText = "Role";
            this.PointRole.Name = "PointRole";
            this.PointRole.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            this.PointRole.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.Automatic;
            // 
            // PointName
            // 
            this.PointName.DataPropertyName = "Name";
            this.PointName.HeaderText = "Name";
            this.PointName.Name = "PointName";
            // 
            // xDataGridViewTextBoxColumn
            // 
            this.xDataGridViewTextBoxColumn.DataPropertyName = "X";
            dataGridViewCellStyle2.Format = "0.0000";
            this.xDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle2;
            this.xDataGridViewTextBoxColumn.HeaderText = "X";
            this.xDataGridViewTextBoxColumn.Name = "xDataGridViewTextBoxColumn";
            // 
            // yDataGridViewTextBoxColumn
            // 
            this.yDataGridViewTextBoxColumn.DataPropertyName = "Y";
            dataGridViewCellStyle3.Format = "0.0000";
            this.yDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle3;
            this.yDataGridViewTextBoxColumn.HeaderText = "Y";
            this.yDataGridViewTextBoxColumn.Name = "yDataGridViewTextBoxColumn";
            // 
            // zDataGridViewTextBoxColumn
            // 
            this.zDataGridViewTextBoxColumn.DataPropertyName = "Z";
            dataGridViewCellStyle4.Format = "0.0000";
            this.zDataGridViewTextBoxColumn.DefaultCellStyle = dataGridViewCellStyle4;
            this.zDataGridViewTextBoxColumn.HeaderText = "Z";
            this.zDataGridViewTextBoxColumn.Name = "zDataGridViewTextBoxColumn";
            // 
            // cRSDataGridViewTextBoxColumn
            // 
            this.cRSDataGridViewTextBoxColumn.DataPropertyName = "CRS";
            this.cRSDataGridViewTextBoxColumn.HeaderText = "CRS";
            this.cRSDataGridViewTextBoxColumn.Name = "cRSDataGridViewTextBoxColumn";
            this.cRSDataGridViewTextBoxColumn.ReadOnly = true;
            this.cRSDataGridViewTextBoxColumn.Resizable = System.Windows.Forms.DataGridViewTriState.True;
            // 
            // tRSDataGridViewTextBoxColumn
            // 
            this.tRSDataGridViewTextBoxColumn.DataPropertyName = "TRS";
            this.tRSDataGridViewTextBoxColumn.HeaderText = "TRS";
            this.tRSDataGridViewTextBoxColumn.Name = "tRSDataGridViewTextBoxColumn";
            this.tRSDataGridViewTextBoxColumn.ReadOnly = true;
            // 
            // LatDMS
            // 
            this.LatDMS.DataPropertyName = "LatDMS";
            this.LatDMS.HeaderText = "LatDMS";
            this.LatDMS.Name = "LatDMS";
            this.LatDMS.ReadOnly = true;
            // 
            // LonDMS
            // 
            this.LonDMS.DataPropertyName = "LonDMS";
            this.LonDMS.HeaderText = "LonDMS";
            this.LonDMS.Name = "LonDMS";
            this.LonDMS.ReadOnly = true;
            // 
            // H
            // 
            this.H.DataPropertyName = "H";
            dataGridViewCellStyle5.Format = "0.0000";
            this.H.DefaultCellStyle = dataGridViewCellStyle5;
            this.H.HeaderText = "H";
            this.H.Name = "H";
            this.H.ReadOnly = true;
            // 
            // PointDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1076, 308);
            this.Controls.Add(this.pointGridView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "PointDlg";
            this.Text = "Points";
            this.Load += new System.EventHandler(this.PointDlg_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pointGridView)).EndInit();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.gPointBindingSource)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.gPointBindingSource1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView pointGridView;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripButton toolStripButton3;
        private System.Windows.Forms.ToolStripButton toolStripButton2;
        private System.Windows.Forms.ToolStripButton toolStripDeleteStream;
        private System.Windows.Forms.BindingSource gPointBindingSource;
        private System.Windows.Forms.BindingSource gPointBindingSource1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn1;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn2;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn3;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn4;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn5;
        private System.Windows.Forms.ToolStripButton toolStripButton5;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem pointsToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem transformToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem addNewPointToolStripMenuItem;
        private System.Windows.Forms.DataGridViewComboBoxColumn pointTypeDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewComboBoxColumn PointRole;
        private System.Windows.Forms.DataGridViewTextBoxColumn PointName;
        private System.Windows.Forms.DataGridViewTextBoxColumn xDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn yDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn zDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn cRSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn tRSDataGridViewTextBoxColumn;
        private System.Windows.Forms.DataGridViewTextBoxColumn LatDMS;
        private System.Windows.Forms.DataGridViewTextBoxColumn LonDMS;
        private System.Windows.Forms.DataGridViewTextBoxColumn H;
        private System.Windows.Forms.DataGridViewComboBoxColumn dataGridViewComboBoxColumn6;
    }
}
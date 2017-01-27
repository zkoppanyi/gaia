namespace Gaia.GUI.Dialogs
{
    partial class DataViewDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(DataViewDlg));
            this.vScroll = new System.Windows.Forms.VScrollBar();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.lblTime = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonStatistics = new System.Windows.Forms.ToolStripButton();
            this.toolStripButtonCalculate = new System.Windows.Forms.ToolStripButton();
            this.dataGridView = new System.Windows.Forms.DataGridView();
            this.statusStrip1.SuspendLayout();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // vScroll
            // 
            this.vScroll.Dock = System.Windows.Forms.DockStyle.Right;
            this.vScroll.Location = new System.Drawing.Point(543, 0);
            this.vScroll.Name = "vScroll";
            this.vScroll.Size = new System.Drawing.Size(17, 464);
            this.vScroll.TabIndex = 1;
            this.vScroll.Scroll += new System.Windows.Forms.ScrollEventHandler(this.vScroll_Scroll);
            this.vScroll.ValueChanged += new System.EventHandler(this.vScroll_ValueChanged);
            this.vScroll.KeyUp += new System.Windows.Forms.KeyEventHandler(this.vScroll_KeyUp);
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.lblTime});
            this.statusStrip1.Location = new System.Drawing.Point(0, 442);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(543, 22);
            this.statusStrip1.TabIndex = 3;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // lblTime
            // 
            this.lblTime.Name = "lblTime";
            this.lblTime.Size = new System.Drawing.Size(16, 17);
            this.lblTime.Text = "...";
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripButton1,
            this.toolStripButtonStatistics,
            this.toolStripButtonCalculate});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(543, 25);
            this.toolStrip.TabIndex = 4;
            this.toolStrip.Text = "toolStrip1";
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = global::Gaia.Properties.Resources.export_button;
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // toolStripButtonStatistics
            // 
            this.toolStripButtonStatistics.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonStatistics.Image = global::Gaia.Properties.Resources.analysis;
            this.toolStripButtonStatistics.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonStatistics.Name = "toolStripButtonStatistics";
            this.toolStripButtonStatistics.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonStatistics.Text = "toolStripButton2";
            this.toolStripButtonStatistics.Click += new System.EventHandler(this.toolStripButtonStatistics_Click);
            // 
            // toolStripButtonCalculate
            // 
            this.toolStripButtonCalculate.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCalculate.Image = global::Gaia.Properties.Resources.save_pencil_button;
            this.toolStripButtonCalculate.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCalculate.Name = "toolStripButtonCalculate";
            this.toolStripButtonCalculate.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCalculate.Text = "toolStripButton2";
            this.toolStripButtonCalculate.Click += new System.EventHandler(this.toolStripButton2_Click);
            // 
            // dataGridView
            // 
            this.dataGridView.AllowUserToAddRows = false;
            this.dataGridView.AllowUserToDeleteRows = false;
            this.dataGridView.AllowUserToOrderColumns = true;
            this.dataGridView.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill;
            this.dataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dataGridView.Location = new System.Drawing.Point(0, 25);
            this.dataGridView.Name = "dataGridView";
            this.dataGridView.ScrollBars = System.Windows.Forms.ScrollBars.Horizontal;
            this.dataGridView.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullColumnSelect;
            this.dataGridView.Size = new System.Drawing.Size(543, 417);
            this.dataGridView.TabIndex = 5;
            this.dataGridView.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView_CellContentClick);
            // 
            // DataViewDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(560, 464);
            this.Controls.Add(this.dataGridView);
            this.Controls.Add(this.toolStrip);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.vScroll);
            this.Font = new System.Drawing.Font("Courier New", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "DataViewDlg";
            this.Text = "Data";
            this.Load += new System.EventHandler(this.DataViewDlg_Load);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.VScrollBar vScroll;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel lblTime;
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.DataGridView dataGridView;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.ToolStripButton toolStripButtonStatistics;
        private System.Windows.Forms.ToolStripButton toolStripButtonCalculate;
    }
}
namespace Gaia.GUI.Dialogs
{
    partial class FigureDlg
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea2 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend2 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series2 = new System.Windows.Forms.DataVisualization.Charting.Series();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FigureDlg));
            this.tableLayoutPanel1 = new System.Windows.Forms.TableLayoutPanel();
            this.textBoxStatus = new System.Windows.Forms.TextBox();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSeries = new System.Windows.Forms.TabPage();
            this.tabPageHistogram = new System.Windows.Forms.TabPage();
            this.chartHistogram = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripProgressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripCancelProgress = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aspectRatioEqualToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButton1 = new System.Windows.Forms.ToolStripButton();
            this.figureBox = new System.Windows.Forms.PictureBox();
            this.tableLayoutPanel1.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPageSeries.SuspendLayout();
            this.tabPageHistogram.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.chartHistogram)).BeginInit();
            this.statusStrip1.SuspendLayout();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.figureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // tableLayoutPanel1
            // 
            this.tableLayoutPanel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tableLayoutPanel1.ColumnCount = 2;
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 95.3271F));
            this.tableLayoutPanel1.ColumnStyles.Add(new System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 4.672897F));
            this.tableLayoutPanel1.Controls.Add(this.textBoxStatus, 0, 1);
            this.tableLayoutPanel1.Controls.Add(this.tabControl1, 0, 0);
            this.tableLayoutPanel1.Location = new System.Drawing.Point(0, 28);
            this.tableLayoutPanel1.Name = "tableLayoutPanel1";
            this.tableLayoutPanel1.RowCount = 2;
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 90.13158F));
            this.tableLayoutPanel1.RowStyles.Add(new System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Percent, 9.868421F));
            this.tableLayoutPanel1.Size = new System.Drawing.Size(687, 456);
            this.tableLayoutPanel1.TabIndex = 0;
            // 
            // textBoxStatus
            // 
            this.textBoxStatus.Dock = System.Windows.Forms.DockStyle.Fill;
            this.textBoxStatus.Font = new System.Drawing.Font("Courier New", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.textBoxStatus.Location = new System.Drawing.Point(3, 413);
            this.textBoxStatus.Multiline = true;
            this.textBoxStatus.Name = "textBoxStatus";
            this.textBoxStatus.ReadOnly = true;
            this.textBoxStatus.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.textBoxStatus.Size = new System.Drawing.Size(648, 40);
            this.textBoxStatus.TabIndex = 2;
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSeries);
            this.tabControl1.Controls.Add(this.tabPageHistogram);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(648, 404);
            this.tabControl1.TabIndex = 3;
            // 
            // tabPageSeries
            // 
            this.tabPageSeries.Controls.Add(this.figureBox);
            this.tabPageSeries.Location = new System.Drawing.Point(4, 22);
            this.tabPageSeries.Name = "tabPageSeries";
            this.tabPageSeries.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSeries.Size = new System.Drawing.Size(640, 378);
            this.tabPageSeries.TabIndex = 0;
            this.tabPageSeries.Text = "Series";
            this.tabPageSeries.UseVisualStyleBackColor = true;
            // 
            // tabPageHistogram
            // 
            this.tabPageHistogram.Controls.Add(this.chartHistogram);
            this.tabPageHistogram.Location = new System.Drawing.Point(4, 22);
            this.tabPageHistogram.Name = "tabPageHistogram";
            this.tabPageHistogram.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageHistogram.Size = new System.Drawing.Size(640, 378);
            this.tabPageHistogram.TabIndex = 1;
            this.tabPageHistogram.Text = "Histogram";
            this.tabPageHistogram.UseVisualStyleBackColor = true;
            // 
            // chartHistogram
            // 
            chartArea2.Name = "ChartArea1";
            this.chartHistogram.ChartAreas.Add(chartArea2);
            this.chartHistogram.Dock = System.Windows.Forms.DockStyle.Fill;
            legend2.Name = "Legend1";
            this.chartHistogram.Legends.Add(legend2);
            this.chartHistogram.Location = new System.Drawing.Point(3, 3);
            this.chartHistogram.Name = "chartHistogram";
            series2.ChartArea = "ChartArea1";
            series2.Legend = "Legend1";
            series2.Name = "Series1";
            this.chartHistogram.Series.Add(series2);
            this.chartHistogram.Size = new System.Drawing.Size(634, 372);
            this.chartHistogram.TabIndex = 2;
            this.chartHistogram.Text = "chart1";
            // 
            // statusStrip1
            // 
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripProgressBar,
            this.toolStripCancelProgress});
            this.statusStrip1.Location = new System.Drawing.Point(0, 487);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(687, 22);
            this.statusStrip1.TabIndex = 1;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripProgressBar
            // 
            this.toolStripProgressBar.Name = "toolStripProgressBar";
            this.toolStripProgressBar.Size = new System.Drawing.Size(100, 16);
            // 
            // toolStripCancelProgress
            // 
            this.toolStripCancelProgress.Image = global::Gaia.Properties.Resources.cancel_button;
            this.toolStripCancelProgress.Name = "toolStripCancelProgress";
            this.toolStripCancelProgress.Size = new System.Drawing.Size(59, 17);
            this.toolStripCancelProgress.Text = "Cancel";
            this.toolStripCancelProgress.Click += new System.EventHandler(this.toolStripCancelProgress_Click);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripSplitButton1,
            this.toolStripButton1});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(687, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.aspectRatioEqualToolStripMenuItem,
            this.legendToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::Gaia.Properties.Resources.refresh_button;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.refreshToolStripMenuItem.Text = "Refresh...";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // aspectRatioEqualToolStripMenuItem
            // 
            this.aspectRatioEqualToolStripMenuItem.Name = "aspectRatioEqualToolStripMenuItem";
            this.aspectRatioEqualToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.aspectRatioEqualToolStripMenuItem.Text = "Aspect Ratio Equal...";
            this.aspectRatioEqualToolStripMenuItem.Click += new System.EventHandler(this.aspectRatioEqualToolStripMenuItem_Click);
            // 
            // legendToolStripMenuItem
            // 
            this.legendToolStripMenuItem.Checked = true;
            this.legendToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.legendToolStripMenuItem.Name = "legendToolStripMenuItem";
            this.legendToolStripMenuItem.Size = new System.Drawing.Size(181, 22);
            this.legendToolStripMenuItem.Text = "Legend";
            this.legendToolStripMenuItem.Click += new System.EventHandler(this.legendToolStripMenuItem_Click);
            // 
            // toolStripButton1
            // 
            this.toolStripButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButton1.Image = ((System.Drawing.Image)(resources.GetObject("toolStripButton1.Image")));
            this.toolStripButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButton1.Name = "toolStripButton1";
            this.toolStripButton1.Size = new System.Drawing.Size(23, 22);
            this.toolStripButton1.Text = "toolStripButton1";
            this.toolStripButton1.Click += new System.EventHandler(this.toolStripButton1_Click);
            // 
            // figureBox
            // 
            this.figureBox.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figureBox.Location = new System.Drawing.Point(3, 3);
            this.figureBox.Name = "figureBox";
            this.figureBox.Size = new System.Drawing.Size(634, 372);
            this.figureBox.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.figureBox.TabIndex = 0;
            this.figureBox.TabStop = false;
            // 
            // FigureDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(687, 509);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.tableLayoutPanel1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FigureDlg";
            this.Text = "Figure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsDlg_FormClosing);
            this.Load += new System.EventHandler(this.Statistics_Load);
            this.tableLayoutPanel1.ResumeLayout(false);
            this.tableLayoutPanel1.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPageSeries.ResumeLayout(false);
            this.tabPageHistogram.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.chartHistogram)).EndInit();
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.figureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TableLayoutPanel tableLayoutPanel1;
        private System.Windows.Forms.TextBox textBoxStatus;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripProgressBar toolStripProgressBar;
        private System.Windows.Forms.ToolStripStatusLabel toolStripCancelProgress;
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton toolStripButton1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSeries;
        private System.Windows.Forms.TabPage tabPageHistogram;
        private System.Windows.Forms.DataVisualization.Charting.Chart chartHistogram;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aspectRatioEqualToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legendToolStripMenuItem;
        private System.Windows.Forms.PictureBox figureBox;
    }
}
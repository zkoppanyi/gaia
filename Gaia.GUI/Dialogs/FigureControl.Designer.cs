namespace Gaia.Dialogs
{
    partial class FigureControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.toolStrip = new System.Windows.Forms.ToolStrip();
            this.progressBar = new System.Windows.Forms.ToolStripProgressBar();
            this.toolStripSplitButton1 = new System.Windows.Forms.ToolStripSplitButton();
            this.refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.legendToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.relativeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripButtonCancelProgress = new System.Windows.Forms.ToolStripButton();
            this.toolStripAspectRatio = new System.Windows.Forms.ToolStripComboBox();
            this.figureArea = new System.Windows.Forms.PictureBox();
            this.toolStrip.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.figureArea)).BeginInit();
            this.SuspendLayout();
            // 
            // toolStrip
            // 
            this.toolStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.progressBar,
            this.toolStripSplitButton1,
            this.toolStripButtonCancelProgress,
            this.toolStripAspectRatio});
            this.toolStrip.Location = new System.Drawing.Point(0, 0);
            this.toolStrip.Name = "toolStrip";
            this.toolStrip.Size = new System.Drawing.Size(641, 25);
            this.toolStrip.TabIndex = 0;
            this.toolStrip.Text = "toolStrip1";
            // 
            // progressBar
            // 
            this.progressBar.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.progressBar.Name = "progressBar";
            this.progressBar.Size = new System.Drawing.Size(100, 22);
            // 
            // toolStripSplitButton1
            // 
            this.toolStripSplitButton1.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripSplitButton1.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.refreshToolStripMenuItem,
            this.legendToolStripMenuItem,
            this.relativeToolStripMenuItem});
            this.toolStripSplitButton1.Image = global::Gaia.Properties.Resources.refresh_button;
            this.toolStripSplitButton1.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripSplitButton1.Name = "toolStripSplitButton1";
            this.toolStripSplitButton1.Size = new System.Drawing.Size(32, 22);
            this.toolStripSplitButton1.Text = "toolStripSplitButton1";
            // 
            // refreshToolStripMenuItem
            // 
            this.refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            this.refreshToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.refreshToolStripMenuItem.Text = "Refresh...";
            this.refreshToolStripMenuItem.Click += new System.EventHandler(this.refreshToolStripMenuItem_Click);
            // 
            // legendToolStripMenuItem
            // 
            this.legendToolStripMenuItem.Checked = true;
            this.legendToolStripMenuItem.CheckState = System.Windows.Forms.CheckState.Checked;
            this.legendToolStripMenuItem.Name = "legendToolStripMenuItem";
            this.legendToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.legendToolStripMenuItem.Text = "Legend";
            this.legendToolStripMenuItem.Click += new System.EventHandler(this.legendToolStripMenuItem_Click);
            // 
            // relativeToolStripMenuItem
            // 
            this.relativeToolStripMenuItem.Name = "relativeToolStripMenuItem";
            this.relativeToolStripMenuItem.Size = new System.Drawing.Size(122, 22);
            this.relativeToolStripMenuItem.Text = "Relative";
            this.relativeToolStripMenuItem.Click += new System.EventHandler(this.relativeToolStripMenuItem_Click);
            // 
            // toolStripButtonCancelProgress
            // 
            this.toolStripButtonCancelProgress.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.toolStripButtonCancelProgress.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Image;
            this.toolStripButtonCancelProgress.Image = global::Gaia.Properties.Resources.cancel_button;
            this.toolStripButtonCancelProgress.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.toolStripButtonCancelProgress.Name = "toolStripButtonCancelProgress";
            this.toolStripButtonCancelProgress.Size = new System.Drawing.Size(23, 22);
            this.toolStripButtonCancelProgress.Text = "toolStripButton1";
            this.toolStripButtonCancelProgress.Click += new System.EventHandler(this.toolStripButtonCancelProgress_Click);
            // 
            // toolStripAspectRatio
            // 
            this.toolStripAspectRatio.Name = "toolStripAspectRatio";
            this.toolStripAspectRatio.Size = new System.Drawing.Size(121, 25);
            this.toolStripAspectRatio.SelectedIndexChanged += new System.EventHandler(this.toolStripAspectRatio_SelectedIndexChanged);
            this.toolStripAspectRatio.TextUpdate += new System.EventHandler(this.toolStripAspectRatio_TextUpdate);
            this.toolStripAspectRatio.Click += new System.EventHandler(this.toolStripAspectRatio_Click);
            // 
            // figureArea
            // 
            this.figureArea.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figureArea.Location = new System.Drawing.Point(0, 25);
            this.figureArea.Name = "figureArea";
            this.figureArea.Size = new System.Drawing.Size(641, 432);
            this.figureArea.TabIndex = 1;
            this.figureArea.TabStop = false;
            this.figureArea.Click += new System.EventHandler(this.figureArea_Click);
            // 
            // FigureControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.Controls.Add(this.figureArea);
            this.Controls.Add(this.toolStrip);
            this.Name = "FigureControl";
            this.Size = new System.Drawing.Size(641, 457);
            this.toolStrip.ResumeLayout(false);
            this.toolStrip.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.figureArea)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.ToolStrip toolStrip;
        private System.Windows.Forms.ToolStripProgressBar progressBar;
        private System.Windows.Forms.PictureBox figureArea;
        private System.Windows.Forms.ToolStripSplitButton toolStripSplitButton1;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem legendToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem relativeToolStripMenuItem;
        private System.Windows.Forms.ToolStripButton toolStripButtonCancelProgress;
        private System.Windows.Forms.ToolStripComboBox toolStripAspectRatio;
    }
}

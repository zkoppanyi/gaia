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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FigureDlg));
            this.figureControl = new Gaia.Dialogs.FigureControl();
            this.SuspendLayout();
            // 
            // figureControl
            // 
            this.figureControl.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.figureControl.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.figureControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.figureControl.Location = new System.Drawing.Point(0, 0);
            this.figureControl.Name = "figureControl";
            this.figureControl.Size = new System.Drawing.Size(617, 554);
            this.figureControl.TabIndex = 0;
            // 
            // FigureDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(617, 554);
            this.Controls.Add(this.figureControl);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FigureDlg";
            this.Text = "Figure";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.StatisticsDlg_FormClosing);
            this.Load += new System.EventHandler(this.FigureDlg_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private Gaia.Dialogs.FigureControl figureControl;
    }
}
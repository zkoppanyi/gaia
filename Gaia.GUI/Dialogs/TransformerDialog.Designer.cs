namespace Gaia.GUI.Dialogs
{
    partial class TransformerDialog
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
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtCRS = new System.Windows.Forms.TextBox();
            this.lblTRSDescription = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbTRS = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCRS = new System.Windows.Forms.ComboBox();
            this.tabControl = new System.Windows.Forms.TabControl();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.chkClockErrorModel = new System.Windows.Forms.CheckBox();
            this.label3 = new System.Windows.Forms.Label();
            this.cmbClockErrorModel = new System.Windows.Forms.ComboBox();
            this.btnImport = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.tabPage1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabControl.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(435, 275);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Transform coordinates";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.txtCRS);
            this.groupBox1.Controls.Add(this.lblTRSDescription);
            this.groupBox1.Controls.Add(this.label10);
            this.groupBox1.Controls.Add(this.cmbTRS);
            this.groupBox1.Controls.Add(this.label9);
            this.groupBox1.Controls.Add(this.cmbCRS);
            this.groupBox1.Location = new System.Drawing.Point(3, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(415, 263);
            this.groupBox1.TabIndex = 7;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Reference Frames";
            // 
            // txtCRS
            // 
            this.txtCRS.Location = new System.Drawing.Point(5, 76);
            this.txtCRS.Multiline = true;
            this.txtCRS.Name = "txtCRS";
            this.txtCRS.ReadOnly = true;
            this.txtCRS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCRS.Size = new System.Drawing.Size(392, 89);
            this.txtCRS.TabIndex = 12;
            // 
            // lblTRSDescription
            // 
            this.lblTRSDescription.AutoSize = true;
            this.lblTRSDescription.Location = new System.Drawing.Point(6, 227);
            this.lblTRSDescription.Name = "lblTRSDescription";
            this.lblTRSDescription.Size = new System.Drawing.Size(16, 13);
            this.lblTRSDescription.TabIndex = 11;
            this.lblTRSDescription.Text = "...";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 181);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 13);
            this.label10.TabIndex = 10;
            this.label10.Text = "Time Reference Frame";
            // 
            // cmbTRS
            // 
            this.cmbTRS.FormattingEnabled = true;
            this.cmbTRS.Location = new System.Drawing.Point(6, 203);
            this.cmbTRS.Name = "cmbTRS";
            this.cmbTRS.Size = new System.Drawing.Size(391, 21);
            this.cmbTRS.TabIndex = 9;
            this.cmbTRS.SelectedIndexChanged += new System.EventHandler(this.cmbTRS_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 27);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 13);
            this.label9.TabIndex = 8;
            this.label9.Text = "Coordinate Reference Frame";
            // 
            // cmbCRS
            // 
            this.cmbCRS.FormattingEnabled = true;
            this.cmbCRS.Location = new System.Drawing.Point(6, 49);
            this.cmbCRS.Name = "cmbCRS";
            this.cmbCRS.Size = new System.Drawing.Size(391, 21);
            this.cmbCRS.TabIndex = 7;
            this.cmbCRS.SelectedIndexChanged += new System.EventHandler(this.cmbCRS_SelectedIndexChanged);
            // 
            // tabControl
            // 
            this.tabControl.Controls.Add(this.tabPage1);
            this.tabControl.Controls.Add(this.tabPage2);
            this.tabControl.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabControl.Location = new System.Drawing.Point(0, 0);
            this.tabControl.Name = "tabControl";
            this.tabControl.SelectedIndex = 0;
            this.tabControl.Size = new System.Drawing.Size(443, 301);
            this.tabControl.TabIndex = 3;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(435, 275);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Clock Error Model";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.chkClockErrorModel);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.cmbClockErrorModel);
            this.groupBox2.Location = new System.Drawing.Point(3, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(426, 119);
            this.groupBox2.TabIndex = 8;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Reference Frames";
            // 
            // chkClockErrorModel
            // 
            this.chkClockErrorModel.AutoSize = true;
            this.chkClockErrorModel.Location = new System.Drawing.Point(9, 26);
            this.chkClockErrorModel.Name = "chkClockErrorModel";
            this.chkClockErrorModel.Size = new System.Drawing.Size(132, 17);
            this.chkClockErrorModel.TabIndex = 9;
            this.chkClockErrorModel.Text = "Use Clock Error Model";
            this.chkClockErrorModel.UseVisualStyleBackColor = true;
            this.chkClockErrorModel.CheckedChanged += new System.EventHandler(this.checkBox1_CheckedChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 52);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(136, 13);
            this.label3.TabIndex = 8;
            this.label3.Text = "Selected Clock Error Model";
            // 
            // cmbClockErrorModel
            // 
            this.cmbClockErrorModel.Enabled = false;
            this.cmbClockErrorModel.FormattingEnabled = true;
            this.cmbClockErrorModel.Location = new System.Drawing.Point(9, 77);
            this.cmbClockErrorModel.Name = "cmbClockErrorModel";
            this.cmbClockErrorModel.Size = new System.Drawing.Size(403, 21);
            this.cmbClockErrorModel.TabIndex = 7;
            // 
            // btnImport
            // 
            this.btnImport.Image = global::Gaia.Properties.Resources.import_database_button;
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(337, 319);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(102, 43);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Transform";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Gaia.Properties.Resources.cancel_button;
            this.btnCancel.Location = new System.Drawing.Point(239, 319);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 43);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // TransformerDialog
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(443, 374);
            this.Controls.Add(this.tabControl);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnCancel);
            this.Name = "TransformerDialog";
            this.Text = "Transform...";
            this.Load += new System.EventHandler(this.CoordinateTransformDialog_Load);
            this.tabPage1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabControl.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TabControl tabControl;
        private System.Windows.Forms.TextBox txtCRS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbCRS;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Label lblTRSDescription;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.ComboBox cmbTRS;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox cmbClockErrorModel;
        private System.Windows.Forms.CheckBox chkClockErrorModel;
    }
}
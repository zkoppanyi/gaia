namespace Gaia.GUI.Dialogs
{
    partial class FigureSeriesSelectorDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FigureSeriesSelectorDlg));
            this.tabControlFieldSelector = new System.Windows.Forms.TabControl();
            this.tabPageFieldSelector = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.label6 = new System.Windows.Forms.Label();
            this.comboBoxFigures = new System.Windows.Forms.ComboBox();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.comboBoxYSeriesField = new System.Windows.Forms.ComboBox();
            this.comboBoxYSeriesDataStream = new System.Windows.Forms.ComboBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.comboBoxXSeriesField = new System.Windows.Forms.ComboBox();
            this.label2 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.comboBoxXSeriesDataStream = new System.Windows.Forms.ComboBox();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnAdd = new System.Windows.Forms.Button();
            this.tabControlFieldSelector.SuspendLayout();
            this.tabPageFieldSelector.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControlFieldSelector
            // 
            this.tabControlFieldSelector.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.tabControlFieldSelector.Controls.Add(this.tabPageFieldSelector);
            this.tabControlFieldSelector.Controls.Add(this.tabPage2);
            this.tabControlFieldSelector.Location = new System.Drawing.Point(0, 0);
            this.tabControlFieldSelector.Name = "tabControlFieldSelector";
            this.tabControlFieldSelector.SelectedIndex = 0;
            this.tabControlFieldSelector.Size = new System.Drawing.Size(404, 288);
            this.tabControlFieldSelector.TabIndex = 0;
            // 
            // tabPageFieldSelector
            // 
            this.tabPageFieldSelector.Controls.Add(this.groupBox3);
            this.tabPageFieldSelector.Controls.Add(this.groupBox2);
            this.tabPageFieldSelector.Controls.Add(this.groupBox1);
            this.tabPageFieldSelector.Location = new System.Drawing.Point(4, 22);
            this.tabPageFieldSelector.Name = "tabPageFieldSelector";
            this.tabPageFieldSelector.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageFieldSelector.Size = new System.Drawing.Size(396, 262);
            this.tabPageFieldSelector.TabIndex = 0;
            this.tabPageFieldSelector.Text = "Field Selector";
            this.tabPageFieldSelector.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox3.Controls.Add(this.label6);
            this.groupBox3.Controls.Add(this.comboBoxFigures);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(382, 59);
            this.groupBox3.TabIndex = 2;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Figure";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(6, 22);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(36, 13);
            this.label6.TabIndex = 1;
            this.label6.Text = "Figure";
            // 
            // comboBoxFigures
            // 
            this.comboBoxFigures.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxFigures.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxFigures.FormattingEnabled = true;
            this.comboBoxFigures.Location = new System.Drawing.Point(84, 19);
            this.comboBoxFigures.Name = "comboBoxFigures";
            this.comboBoxFigures.Size = new System.Drawing.Size(282, 21);
            this.comboBoxFigures.TabIndex = 0;
            // 
            // groupBox2
            // 
            this.groupBox2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox2.Controls.Add(this.comboBoxYSeriesField);
            this.groupBox2.Controls.Add(this.comboBoxYSeriesDataStream);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Location = new System.Drawing.Point(6, 161);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(382, 90);
            this.groupBox2.TabIndex = 1;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Y Series";
            // 
            // comboBoxYSeriesField
            // 
            this.comboBoxYSeriesField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxYSeriesField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYSeriesField.Enabled = false;
            this.comboBoxYSeriesField.FormattingEnabled = true;
            this.comboBoxYSeriesField.Location = new System.Drawing.Point(84, 48);
            this.comboBoxYSeriesField.Name = "comboBoxYSeriesField";
            this.comboBoxYSeriesField.Size = new System.Drawing.Size(282, 21);
            this.comboBoxYSeriesField.TabIndex = 7;
            this.comboBoxYSeriesField.SelectedIndexChanged += new System.EventHandler(this.comboBoxYSeriesField_SelectedIndexChanged);
            // 
            // comboBoxYSeriesDataStream
            // 
            this.comboBoxYSeriesDataStream.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxYSeriesDataStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxYSeriesDataStream.FormattingEnabled = true;
            this.comboBoxYSeriesDataStream.Location = new System.Drawing.Point(84, 19);
            this.comboBoxYSeriesDataStream.Name = "comboBoxYSeriesDataStream";
            this.comboBoxYSeriesDataStream.Size = new System.Drawing.Size(282, 21);
            this.comboBoxYSeriesDataStream.TabIndex = 4;
            this.comboBoxYSeriesDataStream.SelectedIndexChanged += new System.EventHandler(this.comboBoxYSeriesDataStream_SelectedIndexChanged);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 51);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(29, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Field";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(6, 22);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(66, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Data Stream";
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.comboBoxXSeriesField);
            this.groupBox1.Controls.Add(this.label2);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Controls.Add(this.comboBoxXSeriesDataStream);
            this.groupBox1.Location = new System.Drawing.Point(8, 71);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(382, 84);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "X Series";
            // 
            // comboBoxXSeriesField
            // 
            this.comboBoxXSeriesField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxXSeriesField.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXSeriesField.Enabled = false;
            this.comboBoxXSeriesField.FormattingEnabled = true;
            this.comboBoxXSeriesField.Location = new System.Drawing.Point(84, 48);
            this.comboBoxXSeriesField.Name = "comboBoxXSeriesField";
            this.comboBoxXSeriesField.Size = new System.Drawing.Size(282, 21);
            this.comboBoxXSeriesField.TabIndex = 3;
            this.comboBoxXSeriesField.SelectedIndexChanged += new System.EventHandler(this.comboBoxXSeriesField_SelectedIndexChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 51);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(29, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Field";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 22);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(66, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Data Stream";
            // 
            // comboBoxXSeriesDataStream
            // 
            this.comboBoxXSeriesDataStream.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.comboBoxXSeriesDataStream.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.comboBoxXSeriesDataStream.FormattingEnabled = true;
            this.comboBoxXSeriesDataStream.Location = new System.Drawing.Point(84, 19);
            this.comboBoxXSeriesDataStream.Name = "comboBoxXSeriesDataStream";
            this.comboBoxXSeriesDataStream.Size = new System.Drawing.Size(282, 21);
            this.comboBoxXSeriesDataStream.TabIndex = 0;
            this.comboBoxXSeriesDataStream.SelectedIndexChanged += new System.EventHandler(this.comboBoxXSeriesDataStream_SelectedIndexChanged);
            // 
            // tabPage2
            // 
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(396, 262);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Query";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Image = global::Gaia.Properties.Resources.cancel_button;
            this.btnCancel.Location = new System.Drawing.Point(209, 290);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 43);
            this.btnCancel.TabIndex = 8;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnAdd
            // 
            this.btnAdd.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAdd.Enabled = false;
            this.btnAdd.Image = global::Gaia.Properties.Resources.import_button;
            this.btnAdd.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnAdd.Location = new System.Drawing.Point(307, 290);
            this.btnAdd.Name = "btnAdd";
            this.btnAdd.Size = new System.Drawing.Size(93, 43);
            this.btnAdd.TabIndex = 7;
            this.btnAdd.Text = "Add";
            this.btnAdd.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnAdd.UseVisualStyleBackColor = true;
            this.btnAdd.Click += new System.EventHandler(this.btnAdd_Click);
            // 
            // FigureSeriesSelectorDlg
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(404, 340);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnAdd);
            this.Controls.Add(this.tabControlFieldSelector);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "FigureSeriesSelectorDlg";
            this.Text = "Add Series to Figure...";
            this.Load += new System.EventHandler(this.FigureSeriresSelectorDlg_Load);
            this.tabControlFieldSelector.ResumeLayout(false);
            this.tabPageFieldSelector.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TabControl tabControlFieldSelector;
        private System.Windows.Forms.TabPage tabPageFieldSelector;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.ComboBox comboBoxYSeriesField;
        private System.Windows.Forms.ComboBox comboBoxYSeriesDataStream;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.ComboBox comboBoxXSeriesField;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox comboBoxXSeriesDataStream;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnAdd;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.ComboBox comboBoxFigures;
    }
}
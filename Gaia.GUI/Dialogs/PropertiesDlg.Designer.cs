namespace Gaia.GUI.Dialogs
{
    partial class PropertiesDlg
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(PropertiesDlg));
            this.tabProperties = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.txtDescreption = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.groupBox3 = new System.Windows.Forms.GroupBox();
            this.txtCRS = new System.Windows.Forms.TextBox();
            this.lblTRSDescription = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.cmbTRS = new System.Windows.Forms.ComboBox();
            this.label9 = new System.Windows.Forms.Label();
            this.cmbCRS = new System.Windows.Forms.ComboBox();
            this.tabPageXYZCoordinates = new System.Windows.Forms.TabPage();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.label17 = new System.Windows.Forms.Label();
            this.label18 = new System.Windows.Forms.Label();
            this.txtXYZ_Z = new System.Windows.Forms.TextBox();
            this.label14 = new System.Windows.Forms.Label();
            this.label15 = new System.Windows.Forms.Label();
            this.txtXYZ_Y = new System.Windows.Forms.TextBox();
            this.label16 = new System.Windows.Forms.Label();
            this.label22 = new System.Windows.Forms.Label();
            this.txtXYZ_X = new System.Windows.Forms.TextBox();
            this.tabPageLLHCoordinates = new System.Windows.Forms.TabPage();
            this.groupBoxGeographic = new System.Windows.Forms.GroupBox();
            this.label19 = new System.Windows.Forms.Label();
            this.txtLLHHeight = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtLonSec = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.txtLonMin = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtLonDegree = new System.Windows.Forms.TextBox();
            this.label11 = new System.Windows.Forms.Label();
            this.txtLatSec = new System.Windows.Forms.TextBox();
            this.label12 = new System.Windows.Forms.Label();
            this.txtLatMin = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.txtLatDegree = new System.Windows.Forms.TextBox();
            this.tabPageProperties = new System.Windows.Forms.TabPage();
            this.propertyGrid = new System.Windows.Forms.PropertyGrid();
            this.button1 = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.tabProperties.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabPage2.SuspendLayout();
            this.groupBox3.SuspendLayout();
            this.tabPageXYZCoordinates.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.tabPageLLHCoordinates.SuspendLayout();
            this.groupBoxGeographic.SuspendLayout();
            this.tabPageProperties.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabProperties
            // 
            this.tabProperties.Controls.Add(this.tabPage1);
            this.tabProperties.Controls.Add(this.tabPage2);
            this.tabProperties.Controls.Add(this.tabPageXYZCoordinates);
            this.tabProperties.Controls.Add(this.tabPageLLHCoordinates);
            this.tabProperties.Controls.Add(this.tabPageProperties);
            this.tabProperties.Dock = System.Windows.Forms.DockStyle.Top;
            this.tabProperties.Location = new System.Drawing.Point(0, 0);
            this.tabProperties.Name = "tabProperties";
            this.tabProperties.SelectedIndex = 0;
            this.tabProperties.Size = new System.Drawing.Size(427, 281);
            this.tabProperties.TabIndex = 3;
            this.tabProperties.TabStop = false;
            this.tabProperties.SelectedIndexChanged += new System.EventHandler(this.tabControl_SelectedIndexChanged);
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(419, 255);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Metadata";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.txtDescreption);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Controls.Add(this.txtName);
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Location = new System.Drawing.Point(3, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(407, 161);
            this.groupBox2.TabIndex = 4;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Metadata";
            // 
            // txtDescreption
            // 
            this.txtDescreption.Location = new System.Drawing.Point(9, 89);
            this.txtDescreption.Multiline = true;
            this.txtDescreption.Name = "txtDescreption";
            this.txtDescreption.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtDescreption.Size = new System.Drawing.Size(392, 60);
            this.txtDescreption.TabIndex = 3;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(6, 73);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(60, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Description";
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(9, 41);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(392, 20);
            this.txtName.TabIndex = 1;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(6, 25);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(35, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Name";
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.groupBox3);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(419, 255);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Reference Frames";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // groupBox3
            // 
            this.groupBox3.Controls.Add(this.txtCRS);
            this.groupBox3.Controls.Add(this.lblTRSDescription);
            this.groupBox3.Controls.Add(this.label10);
            this.groupBox3.Controls.Add(this.cmbTRS);
            this.groupBox3.Controls.Add(this.label9);
            this.groupBox3.Controls.Add(this.cmbCRS);
            this.groupBox3.Location = new System.Drawing.Point(8, 6);
            this.groupBox3.Name = "groupBox3";
            this.groupBox3.Size = new System.Drawing.Size(403, 243);
            this.groupBox3.TabIndex = 0;
            this.groupBox3.TabStop = false;
            this.groupBox3.Text = "Reference Frames";
            // 
            // txtCRS
            // 
            this.txtCRS.Location = new System.Drawing.Point(5, 72);
            this.txtCRS.Multiline = true;
            this.txtCRS.Name = "txtCRS";
            this.txtCRS.ReadOnly = true;
            this.txtCRS.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtCRS.Size = new System.Drawing.Size(392, 89);
            this.txtCRS.TabIndex = 6;
            // 
            // lblTRSDescription
            // 
            this.lblTRSDescription.AutoSize = true;
            this.lblTRSDescription.Location = new System.Drawing.Point(6, 223);
            this.lblTRSDescription.Name = "lblTRSDescription";
            this.lblTRSDescription.Size = new System.Drawing.Size(16, 13);
            this.lblTRSDescription.TabIndex = 5;
            this.lblTRSDescription.Text = "...";
            // 
            // label10
            // 
            this.label10.AutoSize = true;
            this.label10.Location = new System.Drawing.Point(6, 177);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(115, 13);
            this.label10.TabIndex = 3;
            this.label10.Text = "Time Reference Frame";
            // 
            // cmbTRS
            // 
            this.cmbTRS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbTRS.FormattingEnabled = true;
            this.cmbTRS.Location = new System.Drawing.Point(6, 199);
            this.cmbTRS.Name = "cmbTRS";
            this.cmbTRS.Size = new System.Drawing.Size(391, 21);
            this.cmbTRS.TabIndex = 2;
            this.cmbTRS.SelectedIndexChanged += new System.EventHandler(this.cmbTRS_SelectedIndexChanged);
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(6, 23);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(143, 13);
            this.label9.TabIndex = 1;
            this.label9.Text = "Coordinate Reference Frame";
            // 
            // cmbCRS
            // 
            this.cmbCRS.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbCRS.FormattingEnabled = true;
            this.cmbCRS.Location = new System.Drawing.Point(6, 45);
            this.cmbCRS.Name = "cmbCRS";
            this.cmbCRS.Size = new System.Drawing.Size(391, 21);
            this.cmbCRS.TabIndex = 0;
            this.cmbCRS.SelectedIndexChanged += new System.EventHandler(this.cmbCRS_TextChanged);
            this.cmbCRS.TextChanged += new System.EventHandler(this.cmbCRS_SelectedIndexChanged);
            // 
            // tabPageXYZCoordinates
            // 
            this.tabPageXYZCoordinates.Controls.Add(this.groupBox1);
            this.tabPageXYZCoordinates.Location = new System.Drawing.Point(4, 22);
            this.tabPageXYZCoordinates.Name = "tabPageXYZCoordinates";
            this.tabPageXYZCoordinates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageXYZCoordinates.Size = new System.Drawing.Size(419, 255);
            this.tabPageXYZCoordinates.TabIndex = 2;
            this.tabPageXYZCoordinates.Text = "XYZ Coordinates";
            this.tabPageXYZCoordinates.UseVisualStyleBackColor = true;
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.label17);
            this.groupBox1.Controls.Add(this.label18);
            this.groupBox1.Controls.Add(this.txtXYZ_Z);
            this.groupBox1.Controls.Add(this.label14);
            this.groupBox1.Controls.Add(this.label15);
            this.groupBox1.Controls.Add(this.txtXYZ_Y);
            this.groupBox1.Controls.Add(this.label16);
            this.groupBox1.Controls.Add(this.label22);
            this.groupBox1.Controls.Add(this.txtXYZ_X);
            this.groupBox1.Location = new System.Drawing.Point(8, 6);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(403, 125);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Certasian";
            // 
            // label17
            // 
            this.label17.AutoSize = true;
            this.label17.Location = new System.Drawing.Point(30, 80);
            this.label17.Name = "label17";
            this.label17.Size = new System.Drawing.Size(17, 13);
            this.label17.TabIndex = 21;
            this.label17.Text = "Z:";
            // 
            // label18
            // 
            this.label18.AutoSize = true;
            this.label18.Location = new System.Drawing.Point(367, 84);
            this.label18.Name = "label18";
            this.label18.Size = new System.Drawing.Size(15, 13);
            this.label18.TabIndex = 20;
            this.label18.Text = "m";
            // 
            // txtXYZ_Z
            // 
            this.txtXYZ_Z.Location = new System.Drawing.Point(53, 77);
            this.txtXYZ_Z.Name = "txtXYZ_Z";
            this.txtXYZ_Z.Size = new System.Drawing.Size(308, 20);
            this.txtXYZ_Z.TabIndex = 19;
            // 
            // label14
            // 
            this.label14.AutoSize = true;
            this.label14.Location = new System.Drawing.Point(30, 54);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(17, 13);
            this.label14.TabIndex = 18;
            this.label14.Text = "Y:";
            // 
            // label15
            // 
            this.label15.AutoSize = true;
            this.label15.Location = new System.Drawing.Point(367, 58);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(15, 13);
            this.label15.TabIndex = 17;
            this.label15.Text = "m";
            // 
            // txtXYZ_Y
            // 
            this.txtXYZ_Y.Location = new System.Drawing.Point(53, 51);
            this.txtXYZ_Y.Name = "txtXYZ_Y";
            this.txtXYZ_Y.Size = new System.Drawing.Size(308, 20);
            this.txtXYZ_Y.TabIndex = 16;
            // 
            // label16
            // 
            this.label16.AutoSize = true;
            this.label16.Location = new System.Drawing.Point(30, 28);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(17, 13);
            this.label16.TabIndex = 15;
            this.label16.Text = "X:";
            // 
            // label22
            // 
            this.label22.AutoSize = true;
            this.label22.Location = new System.Drawing.Point(367, 32);
            this.label22.Name = "label22";
            this.label22.Size = new System.Drawing.Size(15, 13);
            this.label22.TabIndex = 2;
            this.label22.Text = "m";
            // 
            // txtXYZ_X
            // 
            this.txtXYZ_X.Location = new System.Drawing.Point(53, 25);
            this.txtXYZ_X.Name = "txtXYZ_X";
            this.txtXYZ_X.Size = new System.Drawing.Size(308, 20);
            this.txtXYZ_X.TabIndex = 1;
            // 
            // tabPageLLHCoordinates
            // 
            this.tabPageLLHCoordinates.Controls.Add(this.groupBoxGeographic);
            this.tabPageLLHCoordinates.Location = new System.Drawing.Point(4, 22);
            this.tabPageLLHCoordinates.Name = "tabPageLLHCoordinates";
            this.tabPageLLHCoordinates.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageLLHCoordinates.Size = new System.Drawing.Size(419, 255);
            this.tabPageLLHCoordinates.TabIndex = 3;
            this.tabPageLLHCoordinates.Text = "LLH Coordinates";
            this.tabPageLLHCoordinates.UseVisualStyleBackColor = true;
            // 
            // groupBoxGeographic
            // 
            this.groupBoxGeographic.Controls.Add(this.label19);
            this.groupBoxGeographic.Controls.Add(this.txtLLHHeight);
            this.groupBoxGeographic.Controls.Add(this.label3);
            this.groupBoxGeographic.Controls.Add(this.label8);
            this.groupBoxGeographic.Controls.Add(this.label7);
            this.groupBoxGeographic.Controls.Add(this.label4);
            this.groupBoxGeographic.Controls.Add(this.txtLonSec);
            this.groupBoxGeographic.Controls.Add(this.label5);
            this.groupBoxGeographic.Controls.Add(this.txtLonMin);
            this.groupBoxGeographic.Controls.Add(this.label6);
            this.groupBoxGeographic.Controls.Add(this.txtLonDegree);
            this.groupBoxGeographic.Controls.Add(this.label11);
            this.groupBoxGeographic.Controls.Add(this.txtLatSec);
            this.groupBoxGeographic.Controls.Add(this.label12);
            this.groupBoxGeographic.Controls.Add(this.txtLatMin);
            this.groupBoxGeographic.Controls.Add(this.label13);
            this.groupBoxGeographic.Controls.Add(this.txtLatDegree);
            this.groupBoxGeographic.Location = new System.Drawing.Point(8, 17);
            this.groupBoxGeographic.Name = "groupBoxGeographic";
            this.groupBoxGeographic.Size = new System.Drawing.Size(405, 114);
            this.groupBoxGeographic.TabIndex = 2;
            this.groupBoxGeographic.TabStop = false;
            this.groupBoxGeographic.Text = "Geographic DMS";
            // 
            // label19
            // 
            this.label19.AutoSize = true;
            this.label19.Location = new System.Drawing.Point(315, 85);
            this.label19.Name = "label19";
            this.label19.Size = new System.Drawing.Size(15, 13);
            this.label19.TabIndex = 21;
            this.label19.Text = "m";
            // 
            // txtLLHHeight
            // 
            this.txtLLHHeight.Location = new System.Drawing.Point(63, 78);
            this.txtLLHHeight.Name = "txtLLHHeight";
            this.txtLLHHeight.Size = new System.Drawing.Size(246, 20);
            this.txtLLHHeight.TabIndex = 18;
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(6, 78);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(41, 13);
            this.label3.TabIndex = 17;
            this.label3.Text = "Height:";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(6, 54);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(57, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Longitude:";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(6, 28);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(48, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Latitude:";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(315, 51);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(11, 13);
            this.label4.TabIndex = 12;
            this.label4.Text = "\'\'";
            // 
            // txtLonSec
            // 
            this.txtLonSec.Location = new System.Drawing.Point(169, 51);
            this.txtLonSec.Name = "txtLonSec";
            this.txtLonSec.Size = new System.Drawing.Size(140, 20);
            this.txtLonSec.TabIndex = 11;
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(154, 51);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(9, 13);
            this.label5.TabIndex = 10;
            this.label5.Text = "\'";
            // 
            // txtLonMin
            // 
            this.txtLonMin.Location = new System.Drawing.Point(117, 51);
            this.txtLonMin.Name = "txtLonMin";
            this.txtLonMin.Size = new System.Drawing.Size(31, 20);
            this.txtLonMin.TabIndex = 9;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(100, 51);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(11, 13);
            this.label6.TabIndex = 8;
            this.label6.Text = "°";
            // 
            // txtLonDegree
            // 
            this.txtLonDegree.Location = new System.Drawing.Point(63, 51);
            this.txtLonDegree.Name = "txtLonDegree";
            this.txtLonDegree.Size = new System.Drawing.Size(31, 20);
            this.txtLonDegree.TabIndex = 7;
            // 
            // label11
            // 
            this.label11.AutoSize = true;
            this.label11.Location = new System.Drawing.Point(315, 25);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(11, 13);
            this.label11.TabIndex = 6;
            this.label11.Text = "\'\'";
            // 
            // txtLatSec
            // 
            this.txtLatSec.Location = new System.Drawing.Point(169, 25);
            this.txtLatSec.Name = "txtLatSec";
            this.txtLatSec.Size = new System.Drawing.Size(140, 20);
            this.txtLatSec.TabIndex = 5;
            // 
            // label12
            // 
            this.label12.AutoSize = true;
            this.label12.Location = new System.Drawing.Point(154, 25);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(9, 13);
            this.label12.TabIndex = 4;
            this.label12.Text = "\'";
            // 
            // txtLatMin
            // 
            this.txtLatMin.Location = new System.Drawing.Point(117, 25);
            this.txtLatMin.Name = "txtLatMin";
            this.txtLatMin.Size = new System.Drawing.Size(31, 20);
            this.txtLatMin.TabIndex = 3;
            // 
            // label13
            // 
            this.label13.AutoSize = true;
            this.label13.Location = new System.Drawing.Point(100, 25);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(11, 13);
            this.label13.TabIndex = 2;
            this.label13.Text = "°";
            // 
            // txtLatDegree
            // 
            this.txtLatDegree.Location = new System.Drawing.Point(63, 25);
            this.txtLatDegree.Name = "txtLatDegree";
            this.txtLatDegree.Size = new System.Drawing.Size(31, 20);
            this.txtLatDegree.TabIndex = 1;
            // 
            // tabPageProperties
            // 
            this.tabPageProperties.Controls.Add(this.propertyGrid);
            this.tabPageProperties.Location = new System.Drawing.Point(4, 22);
            this.tabPageProperties.Name = "tabPageProperties";
            this.tabPageProperties.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageProperties.Size = new System.Drawing.Size(419, 255);
            this.tabPageProperties.TabIndex = 4;
            this.tabPageProperties.Text = "Properties";
            this.tabPageProperties.UseVisualStyleBackColor = true;
            // 
            // propertyGrid
            // 
            this.propertyGrid.Dock = System.Windows.Forms.DockStyle.Fill;
            this.propertyGrid.Location = new System.Drawing.Point(3, 3);
            this.propertyGrid.Name = "propertyGrid";
            this.propertyGrid.Size = new System.Drawing.Size(413, 249);
            this.propertyGrid.TabIndex = 0;
            // 
            // button1
            // 
            this.button1.Image = global::Gaia.Properties.Resources.save_pencil_button;
            this.button1.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.button1.Location = new System.Drawing.Point(231, 287);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(93, 43);
            this.button1.TabIndex = 7;
            this.button1.Text = "Save";
            this.button1.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.btnCreate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Image = global::Gaia.Properties.Resources.cancel_button;
            this.btnCancel.Location = new System.Drawing.Point(133, 287);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(92, 43);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click_1);
            // 
            // btnImport
            // 
            this.btnImport.Image = global::Gaia.Properties.Resources.import_database_button;
            this.btnImport.ImageAlign = System.Drawing.ContentAlignment.MiddleLeft;
            this.btnImport.Location = new System.Drawing.Point(330, 287);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(93, 43);
            this.btnImport.TabIndex = 5;
            this.btnImport.Text = "Save && Exit";
            this.btnImport.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageBeforeText;
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.button1_Click);
            // 
            // PropertiesDlg
            // 
            this.ClientSize = new System.Drawing.Size(427, 340);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.tabProperties);
            this.Controls.Add(this.btnImport);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "PropertiesDlg";
            this.Load += new System.EventHandler(this.DataStreamPropertiesDlg_Load);
            this.tabProperties.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabPage2.ResumeLayout(false);
            this.groupBox3.ResumeLayout(false);
            this.groupBox3.PerformLayout();
            this.tabPageXYZCoordinates.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.tabPageLLHCoordinates.ResumeLayout(false);
            this.groupBoxGeographic.ResumeLayout(false);
            this.groupBoxGeographic.PerformLayout();
            this.tabPageProperties.ResumeLayout(false);
            this.ResumeLayout(false);

        }


        #endregion

        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.ComboBox cmbCRS;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.ComboBox cmbTRS;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.GroupBox groupBox3;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabControl tabProperties;
        private System.Windows.Forms.TextBox txtDescreption;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label lblTRSDescription;
        private System.Windows.Forms.TextBox txtCRS;
        private System.Windows.Forms.TabPage tabPageXYZCoordinates;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label17;
        private System.Windows.Forms.Label label18;
        private System.Windows.Forms.TextBox txtXYZ_Z;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.TextBox txtXYZ_Y;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.Label label22;
        private System.Windows.Forms.TextBox txtXYZ_X;
        private System.Windows.Forms.TabPage tabPageLLHCoordinates;
        private System.Windows.Forms.GroupBox groupBoxGeographic;
        private System.Windows.Forms.TextBox txtLLHHeight;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtLonSec;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.TextBox txtLonMin;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtLonDegree;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.TextBox txtLatSec;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.TextBox txtLatMin;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.TextBox txtLatDegree;
        private System.Windows.Forms.Label label19;
        private System.Windows.Forms.TabPage tabPageProperties;
        private System.Windows.Forms.PropertyGrid propertyGrid;
    }
}
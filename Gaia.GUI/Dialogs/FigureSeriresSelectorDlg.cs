using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core.DataStreams;

namespace Gaia.GUI.Dialogs
{
    public partial class FigureSeriesSelectorDlg : Form
    {      

        public DataStream SelectedStreamX
        {
            get
            {
                return this.comboBoxXSeriesDataStream.SelectedItem as DataStream;
            }
            set
            {
                this.comboBoxXSeriesDataStream.SelectedItem = value;
            }
        }

        public String SelectedFieldX
        {
            get
            {
                return this.comboBoxXSeriesField.SelectedItem as String;
            }
            set
            {
                this.comboBoxXSeriesField.SelectedItem = value;
            }
        }

        public DataStream SelectedStreamY
        {
            get
            {
                return this.comboBoxYSeriesDataStream.SelectedItem as DataStream;
            }
            set
            {
                this.comboBoxYSeriesDataStream.SelectedItem = value;
            }
        }

        public String SelectedFieldY
        {
            get
            {
                return this.comboBoxYSeriesField.SelectedItem as String;
            }
            set
            {
                this.comboBoxYSeriesField.SelectedItem = value;
            }
        }

        public FigureDlg SelectedFigure
        {
            get
            {
                return this.comboBoxFigures.SelectedItem as FigureDlg;
            }
            set
            {
                this.comboBoxFigures.SelectedItem = value;
            }
        }

        public FigureSeriesSelectorDlg()
        {
            InitializeComponent();

            foreach (DataStream stream in GlobalAccess.Project.DataStreams)
            {
                this.comboBoxXSeriesDataStream.Items.Add(stream);
                this.comboBoxXSeriesDataStream.DisplayMember = "Name";

                this.comboBoxYSeriesDataStream.Items.Add(stream);
                this.comboBoxYSeriesDataStream.DisplayMember = "Name";

            }

            foreach (FigureDlg fig in GlobalAccess.GetFigures())
            {
                this.comboBoxFigures.Items.Add(fig);
                this.comboBoxFigures.DisplayMember = "CaptionName";
            }
        }

        private void FigureSeriresSelectorDlg_Load(object sender, EventArgs e)
        {
           
        }

        public override void Refresh()
        {
            base.Refresh();
            if ((comboBoxXSeriesDataStream.SelectedItem != null) && (comboBoxYSeriesDataStream.SelectedItem != null) &&
                (comboBoxXSeriesField.SelectedItem != null) && (comboBoxYSeriesField.SelectedItem != null))
            {
                btnAdd.Enabled = true;
            }
            else
            {
                btnAdd.Enabled = false;
            }
        }

        private void comboBoxXSeriesDataStream_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataStream selectedStream = comboBoxXSeriesDataStream.SelectedItem as DataStream;
            comboBoxXSeriesField.Enabled = true;
            comboBoxXSeriesField.Items.Clear();
            foreach (PropertyInfo prop in selectedStream.CreateDataLine().GetType().GetProperties())
            {
                DisplayNameAttribute attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
                if (attribute != null )
                {
                    comboBoxXSeriesField.Items.Add(attribute.DisplayName);
                }
            }

            this.Refresh();
        }

        private void comboBoxYSeriesDataStream_SelectedIndexChanged(object sender, EventArgs e)
        {
            DataStream selectedStream = comboBoxYSeriesDataStream.SelectedItem as DataStream;
            comboBoxYSeriesField.Enabled = true;
            comboBoxYSeriesField.Items.Clear();
            foreach (PropertyInfo prop in selectedStream.CreateDataLine().GetType().GetProperties())
            {
                DisplayNameAttribute attribute = prop.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;
                if (attribute != null)
                {
                    comboBoxYSeriesField.Items.Add(attribute.DisplayName);
                }
            }
            this.Refresh();
        }

        private void btnImport_Click(object sender, EventArgs e)
        {

        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
        }

        private void comboBoxXSeriesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }

        private void comboBoxYSeriesField_SelectedIndexChanged(object sender, EventArgs e)
        {
            this.Refresh();
        }
    }
}

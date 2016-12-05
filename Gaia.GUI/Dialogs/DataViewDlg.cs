using Gaia.Core.DataStreams;
using Gaia.Exceptions;
using Gaia.Core.Processing;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Gaia.Core.Visualization;

namespace Gaia.GUI.Dialogs
{
    public partial class DataViewDlg : Form
    {
        private DataStream dataStream;
        private const int numPopulateRecords = 50;

        public DataViewDlg(DataStream dataStream)
        {
            InitializeComponent();
            this.dataStream = dataStream;
            vScroll.Minimum = 0;
            vScroll.Maximum = (int)dataStream.DataNumber;
            populateGrid();
        }

        private void populateGrid()
        {
            populateGrid(0);
        }

        private void populateGrid(int startNum)
        {
            // FIX: Make it seamless
            if (dataStream is UWBDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                List<UWBDataLine> dataLines = new List<UWBDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    UWBDataLine dataLine = (UWBDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            else if (dataStream is IMUDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                List<IMUDataLine> dataLines = new List<IMUDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    IMUDataLine dataLine = (IMUDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            else if (dataStream is GPSLogDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                List<GPSLogDataLine> dataLines = new List<GPSLogDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    GPSLogDataLine dataLine = (GPSLogDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;

            }
            else if (dataStream is CoordinateAttitudeDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                BindingList<CoordinateAttitudeDataLine> dataLines = new BindingList<CoordinateAttitudeDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    CoordinateAttitudeDataLine dataLine = (CoordinateAttitudeDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            else if (dataStream is CoordinateDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                BindingList<CoordinateDataLine> dataLines = new BindingList<CoordinateDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    CoordinateDataLine dataLine = (CoordinateDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            else if (dataStream is WifiFingerptiningDataStream)
            {
                dataStream.Open();
                dataStream.Seek(startNum);

                BindingList<WifiFingerprintingDataLine> dataLines = new BindingList<WifiFingerprintingDataLine>();
                for (int i = 0; i < numPopulateRecords; i++)
                {
                    WifiFingerprintingDataLine dataLine = (WifiFingerprintingDataLine)dataStream.ReadLine();
                    dataLines.Add(dataLine);
                }
                dataStream.Close();

                // set up data source
                dataGridView.SelectionMode = DataGridViewSelectionMode.RowHeaderSelect;
                dataGridView.DataSource = dataLines;
                foreach (DataGridViewColumn col in dataGridView.Columns)
                {
                    col.SortMode = DataGridViewColumnSortMode.NotSortable;
                }
                dataGridView.SelectionMode = DataGridViewSelectionMode.ColumnHeaderSelect;
            }
            else
            {
                throw new GaiaException("The DataStream is not suppported in DataViewDlg");
            }
        }


        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void vScroll_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void vScroll_ValueChanged(object sender, EventArgs e)
        {
            populateGrid(vScroll.Value);
        }

        private void dataGridView_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void DataViewDlg_Load(object sender, EventArgs e)
        {
            this.Text = "Data: " + dataStream.Name;
        }

        private void toolStripButton1_Click(object sender, EventArgs e)
        {
            Stream myStream;
            SaveFileDialog exportDataWindowDialog = new SaveFileDialog();

            exportDataWindowDialog.Filter = "txt files (*.txt)|*.txt|All files (*.*)|*.*";
            exportDataWindowDialog.FilterIndex = 2;
            exportDataWindowDialog.RestoreDirectory = true;

            if (exportDataWindowDialog.ShowDialog() == DialogResult.OK)
            {
                if ((myStream = exportDataWindowDialog.OpenFile()) != null)
                {
                    TextWriter sw = new StreamWriter(myStream);

                    ProgressBarDlg dlgProgress = new ProgressBarDlg();
                    dlgProgress.Worker.DoWork += new DoWorkEventHandler(delegate (object sender1, DoWorkEventArgs e1) {
                        try
                        {
                            dlgProgress.Worker.WriteMessage("Exporting...");
                            dlgProgress.Worker.WriteMessage("File: " + exportDataWindowDialog.FileName);

                            // Write header
                            int colscount = dataGridView.Columns.Count;
                            for (int j = 0; j < colscount; j++)
                            {
                                DataGridViewColumn col = dataGridView.Columns[j];
                                sw.Write(col.HeaderText);
                                if (j != colscount - 1) sw.Write(",");
                            }
                            sw.Write(Environment.NewLine);

                            // Write stream
                            dataStream.Open();
                            dataStream.Begin();

                            while(!dataStream.IsEOF())
                            {
                                DataLine line = dataStream.ReadLine();
                                //foreach(DataGridViewColumn col in dataGridView.Columns)
                                for(int i =0; i < dataGridView.Columns.Count; i++)
                                {
                                    DataGridViewColumn col = dataGridView.Columns[i];
                                    String caption = col.HeaderText;

                                    object value = Utilities.GetValueByDisplayNameAttribute(line, caption);
                                    if (value != null)
                                    {
                                        sw.Write(value);
                                    }

                                    if (i != dataGridView.Columns.Count - 1) sw.Write(",");
                                }
                                dlgProgress.Worker.WriteProgress((double)dataStream.GetPosition() / (double)dataStream.DataNumber * 100);
                                sw.Write(Environment.NewLine);
                            }

                            sw.Close();
                            dataStream.Close();
                        }
                        catch (Exception ex)
                        {
                            dlgProgress.Worker.WriteMessage("Cannot export datastream. The problem: " + ex, "Cannot export", null, Core.AlgorithmMessageType.Error);
                        }

                    });
                    dlgProgress.ShowDialog();
                  
                }
                GlobalAccess.WriteConsole("Text file was created from Data window.", "Text file saved!");
            }
        }

        private void toolStripButtonStatistics_Click(object sender, EventArgs e)
        {
            // Choose fields
            FigureSeriesSelectorDlg selector = new FigureSeriesSelectorDlg();
            selector.SelectedStreamX = dataStream;
            selector.SelectedStreamY = dataStream;
            if (dataGridView.SelectedColumns.Count > 0)
            {
                DataGridViewColumn col = dataGridView.SelectedColumns[0];
                String captionY = col.HeaderText;
                List<PropertyInfo> infos = dataStream.CreateDataLine().GetType().GetProperties().ToList();
                PropertyInfo timestampInfo = dataStream.CreateDataLine().GetType().GetProperties().ToList().Find(x => x.Name == "TimeStamp");
                if (timestampInfo != null)
                {
                    DisplayNameAttribute displayAttributeName = timestampInfo.GetCustomAttributes(typeof(DisplayNameAttribute), true).FirstOrDefault() as DisplayNameAttribute;

                    if (displayAttributeName != null)
                    {
                        selector.SelectedFieldX = displayAttributeName.DisplayName;
                    }
                }
                selector.SelectedFieldY = captionY;
            }

            if (dataGridView.SelectedColumns.Count > 1)
            {
                DataGridViewColumn col = dataGridView.SelectedColumns[1];
                String captionX = col.HeaderText;

                selector.SelectedFieldX = captionX;
            }

            selector.ShowDialog();

            // If everything's fine, create a new figure
            if (selector.DialogResult == DialogResult.OK)
            {
                bool update = true;

                FigureDlg selectedFigure = selector.SelectedFigure;
                if (selectedFigure == null)
                {
                    MainForm form = this.MdiParent as MainForm;
                    selectedFigure = form.CreateFigureDialog();
                    update = false;
                }

                FigureDataSeriesForDataStream dataSeries = new FigureDataSeriesForDataStream(selector.SelectedStreamX.Name, selector.SelectedStreamX, selector.SelectedFieldX, selector.SelectedFieldY);
                dataSeries.SeriesColor = new SolidBrush(selector.SeriesColor);
                selectedFigure.AddDataSeries(dataSeries);
                selectedFigure.MdiParent = this.MdiParent;
                selectedFigure.Show();
            }

        }

        private void toolStripButton2_Click(object sender, EventArgs e)
        {
            CalculateValueDlg dlg = new CalculateValueDlg(this.dataStream);
            dlg.ShowDialog();
        }
    }
}

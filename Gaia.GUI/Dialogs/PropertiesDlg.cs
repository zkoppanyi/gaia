using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia.Core;
using Gaia.Core.DataStreams;
using Gaia.Core.Import;
using Gaia.Core.Processing;
using Gaia.Core.ReferenceFrames;

namespace Gaia.GUI.Dialogs
{
    public partial class PropertiesDlg : Form
    {

        private GaiaObject obj = null;

        public PropertiesDlg(GaiaObject obj)
        {
            InitializeComponent();
            this.obj = obj;
        }

        private void txtLocation_TextChanged(object sender, EventArgs e)
        {

        }


        private void btnCreate_Click(object sender, EventArgs e)
        {
            this.saveObjectChanges();
        }

        private bool saveObjectChanges()
        {
            obj.Name = txtName.Text;
            obj.Description = txtDescreption.Text;

            if (obj is GaiaSpatialObject)
            {
                GaiaSpatialObject spatialObj = obj as GaiaSpatialObject;
                IInfo crs = (IInfo)cmbCRS.SelectedItem;
                TRS trs = (TRS)cmbTRS.SelectedItem;
                spatialObj.CRS = new CRS(crs.Name, crs.WKT);
                spatialObj.TRS = trs;
            }

            if(obj is GPoint)
            {
                if (!updateXYZ())
                {
                    return false;
                }

                GPoint pt = obj as GPoint;
                try
                {
                    pt.X = Convert.ToDouble(txtXYZ_X.Text);
                    pt.Y = Convert.ToDouble(txtXYZ_Y.Text);
                    pt.Z = Convert.ToDouble(txtXYZ_Z.Text);
                }
                catch
                {
                    MessageBox.Show("Format error in XYZ coordinates!", "Format error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return false;
                }
            }

            obj.Project.Save();
            return true;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            if (this.saveObjectChanges())
            {
                this.Close();
            }
        }

        private void DataStreamPropertiesDlg_Load(object sender, EventArgs e)
        {
            this.Text = "Objec name: " + obj.Name;
            txtName.Text = obj.Name;
            txtDescreption.Text = obj.Description;

            if (obj is GaiaSpatialObject)
            {
                GaiaSpatialObject spatialObj = obj as GaiaSpatialObject;
                List<IInfo> list = SRIDDatabase.Instance.SRIDList.ToList<IInfo>();
                cmbCRS.DataSource = list;
                cmbCRS.DisplayMember = "Name";

                cmbTRS.DataSource = GlobalAccess.Project.TimeFrames;
                cmbTRS.DisplayMember = "Name";

                if (spatialObj.TRS != null)
                {
                    cmbTRS.SelectedItem = spatialObj.TRS;
                }

                if (spatialObj.CRS != null)
                {
                    IEnumerable<IInfo> sridList = SRIDDatabase.Instance.FindByName(spatialObj.CRS.Name);
                    if (sridList.Count() > 0)
                    {
                        cmbCRS.SelectedItem = sridList.First<IInfo>();
                    }
                    else
                    {
                        MessageBox.Show("SRID is not found! Name: " + spatialObj.CRS.Name + " Update the srid.csv file!", "SRID does not found!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

                propertyGrid.SelectedObject = obj;
            }

            if (!(obj is GPoint))
            {
                tabProperties.TabPages.Remove(tabPageXYZCoordinates);
                tabProperties.TabPages.Remove(tabPageLLHCoordinates);
            }

            if (obj is GPoint)
            {
                GPoint pt = obj as GPoint;
                txtXYZ_X.Text = Convert.ToString(pt.X);
                txtXYZ_Y.Text = Convert.ToString(pt.Y);
                txtXYZ_Z.Text = Convert.ToString(pt.Z);

                updateLLH();
            }
        }



        private void btnCancel_Click_1(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void cmbCRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            IInfo crs = (IInfo)cmbCRS.SelectedItem;
            if (crs != null)
            {
                txtCRS.Text = "WKT: " + crs.WKT;
            }
        }

        private void cmbTRS_SelectedIndexChanged(object sender, EventArgs e)
        {
            TRS trs = (TRS)cmbTRS.SelectedItem;
            lblTRSDescription.Text = "Description: " + trs.Description;

        }

        private void cmbCRS_TextChanged(object sender, EventArgs e)
        {
        }

        private int lastTabCount = -2;
        private void tabControl_SelectedIndexChanged(object sender, EventArgs e)
        {
            int tabControlIndexLLHCoordinates = tabProperties.TabPages.IndexOfKey("tabPageLLHCoordinates");
            int tabControlIndexXYZCoordinates = tabProperties.TabPages.IndexOfKey("tabPageXYZCoordinates");

            if (lastTabCount == tabControlIndexLLHCoordinates)
            {
                if (!updateXYZ())
                {
                    
                }
            }

            if (lastTabCount == tabControlIndexXYZCoordinates)
            {
                if (!updateLLH())
                {

                }
            }

            lastTabCount = tabProperties.SelectedIndex;
        }

        private bool updateLLH()
        {
            // If the object is a GPoint 
            if (obj is GPoint)
            {
                GPoint pt = obj as GPoint;
                pt.X = Convert.ToDouble(txtXYZ_X.Text);
                pt.Y = Convert.ToDouble(txtXYZ_Y.Text);
                pt.Z = Convert.ToDouble(txtXYZ_Z.Text);

                if ((pt.CRS != null) && (pt.CRS.GetCoordinateSystem() is GeographicCoordinateSystem))
                {
                    int deg, min;
                    double sec;

                    Utilities.ConvertDegToDMS(pt.Lat, out deg, out min, out sec);
                    txtLatDegree.Text = Convert.ToString(deg);
                    txtLatMin.Text = Convert.ToString(min);
                    txtLatSec.Text = Math.Round(sec, 4).ToString("F4");

                    Utilities.ConvertDegToDMS(pt.Lon, out deg, out min, out sec);
                    txtLonDegree.Text = Convert.ToString(deg);
                    txtLonMin.Text = Convert.ToString(min);
                    txtLonSec.Text = Math.Round(sec, 4).ToString("F4");

                    txtLLHHeight.Text = pt.H.ToString("F4");
                    return true;
                }
                else
                {
                    tabProperties.TabPages.Remove(tabPageLLHCoordinates);
                    return false;
                }
            }
            else
            {
                return false;
            }

        }

        private bool updateXYZ()
        {
            int tabControlIndexLLHCoordinates = tabProperties.TabPages.IndexOfKey("tabPageLLHCoordinates");

            if (tabControlIndexLLHCoordinates != -1)
            {
                try
                {
                    double lat;
                    int deg = Convert.ToInt16(txtLatDegree.Text);
                    int min = Convert.ToInt16(txtLatMin.Text);
                    double sec = Convert.ToDouble(txtLatSec.Text);
                    Utilities.ConvertDMSToDeg(deg, min, sec, out lat);

                    double lon;
                    deg = Convert.ToInt16(txtLonDegree.Text);
                    min = Convert.ToInt16(txtLonMin.Text);
                    sec = Convert.ToDouble(txtLonSec.Text);
                    Utilities.ConvertDMSToDeg(deg, min, sec, out lon);

                    double h = Convert.ToDouble(txtLLHHeight.Text);

                    double x, y, z;
                    GPoint pt = obj as GPoint;
                    GeographicCoordinateSystem gcs = pt.CRS.GetCoordinateSystem() as GeographicCoordinateSystem;

                    double a = gcs.HorizontalDatum.Ellipsoid.SemiMajorAxis;
                    double f = 1 / gcs.HorizontalDatum.Ellipsoid.InverseFlattening;
                    Utilities.ConvertLLHToXYZ(lat, lon, h, a, f, out x, out y, out z);

                    txtXYZ_X.Text = Convert.ToString(x);
                    txtXYZ_Y.Text = Convert.ToString(y);
                    txtXYZ_Z.Text = Convert.ToString(z);
                    return true;
                }
                catch
                {
                    MessageBox.Show("Format error in XYZ coordinates!", "Format error!", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    tabProperties.SelectedIndex = tabControlIndexLLHCoordinates;
                    return false;
                }
            }
            else
            {
                return true;
            }

        }
    }
}

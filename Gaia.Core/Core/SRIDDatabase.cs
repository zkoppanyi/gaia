using ProjNet.CoordinateSystems;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Exceptions;

namespace Gaia.Core
{
    public class SRIDDatabase
    {
        private static Dictionary<int, IInfo> database;
        private static SRIDDatabase instance;
        public static SRIDDatabase Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new SRIDDatabase();
                }
                return instance;
            }
        }

        public struct WKTstring
        {
            public int WKID;
            public string WKT;
        }


       private SRIDDatabase()
       {

       }

        public IEnumerable<IInfo> SRIDList
        {
            get
            {
                foreach (IInfo info in database.Values) yield return info;
            }
        }

        public IEnumerable<IInfo> FindByName(String str)
        {

            foreach (IInfo info in database.Values)
            {
                if (info.Name.Contains(str))
                    yield return info;
            }
        }

        public void Init(String filename)
        {
            database = new Dictionary<int, IInfo>();

            try
            {
                using (System.IO.StreamReader sr = System.IO.File.OpenText(filename))
                {
                    while (!sr.EndOfStream)
                    {
                        string line = sr.ReadLine();
                        int split = line.IndexOf(';');
                        if (split > -1)
                        {
                            WKTstring wkt = new WKTstring();
                            wkt.WKID = int.Parse(line.Substring(0, split));
                            wkt.WKT = line.Substring(split + 1);
                            IInfo info = ProjNet.Converters.WellKnownText.CoordinateSystemWktReader.Parse(wkt.WKT);

                            try
                            {
                                database.Add(wkt.WKID, info);
                            }
                            catch
                            {

                            }
                        }
                    }
                    sr.Close();
                }
            }
            catch (FileNotFoundException ex)
            {
                throw new GaiaAssertException("Can't load the SRID file!");
            }
            catch (Exception ex)
            {
                throw new GaiaAssertException(ex.ToString());
            }
        }

        /// <summary>Gets a coordinate system from the SRID.csv file</summary>
        /// <param name="id">EPSG ID</param>
        /// <returns>Coordinate system, or null if SRID was not found.</returns>
        public ICoordinateSystem GetCSbyID(int id)
        {
            return database[id] as ICoordinateSystem;
        }
    }
}

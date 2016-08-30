using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;
using Gaia.Core.Import;
using Gaia.GUI.Dialogs;

namespace Gaia.GUI
{
    static class GlobalAccess
    {
        public static IMessanger Console;
        public static List<Importer> Importers;

        private static MainForm mainFormInst;
        public static void Init(MainForm mainForm, IMessanger console)
        {
            mainFormInst = mainForm;
            Console = console;

            Importers = new List<Importer>();
            Importers.Add(new UWBStandardImporter(GlobalAccess.Project));
            Importers.Add(new IMUMicroStrainLogImporter(GlobalAccess.Project));
            Importers.Add(new IMUEpsonLogImporter(GlobalAccess.Project));
            Importers.Add(new PointsImporter(GlobalAccess.Project));
            Importers.Add(new GPSLogImporter(GlobalAccess.Project));
            Importers.Add(new CoordinatesImporter(GlobalAccess.Project));
            Importers.Add(new RTKLibPosImporter(GlobalAccess.Project));

            SRIDDatabase.Instance.Init("srid.csv");
        }

        public static Project Project { get; set; }
        
        public static void RefreshMainForm()
        {
            mainFormInst.Refresh(); 
        }

        public static IEnumerable<FigureDlg> GetFigures()
        {
            return mainFormInst.FigureDlgs;
        }

        public static void RemoveFigure(FigureDlg dlg)
        {
            mainFormInst.RemoveFigureDialog(dlg);
        }

        public static void WriteConsole(String text, String status = null, ConsoleMessageType type = ConsoleMessageType.Message)
        {
            Console.Write(text, status, null, type);
        }


    }
}

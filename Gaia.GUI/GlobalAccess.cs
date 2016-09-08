using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Gaia.Core;
using Gaia.Core.Import;
using Gaia.GUI.Dialogs;
using System.Reflection;

namespace Gaia.GUI
{
    static class GlobalAccess
    {
        public static IMessanger Console;
        public static List<Importer.ImporterFactory> ImporterFactories;

        private static MainForm mainFormInst;
        public static void Init(MainForm mainForm, IMessanger console)
        {
            mainFormInst = mainForm;
            Console = console;

            ImporterFactories = new List<Importer.ImporterFactory>();
            ImporterFactories.Add(UWBStandardImporter.Factory);
            ImporterFactories.Add(IMUMicroStrainLogImporter.Factory);
            ImporterFactories.Add(IMUEpsonLogImporter.Factory);
            ImporterFactories.Add(PointsImporter.Factory);
            ImporterFactories.Add(GPSLogImporter.Factory);
            ImporterFactories.Add(CoordinatesImporter.Factory);
            ImporterFactories.Add(RTKLibPosImporter.Factory);

            SRIDDatabase.Instance.Init(Assembly.GetExecutingAssembly().GetManifestResourceStream("Gaia.srid.txt"));
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

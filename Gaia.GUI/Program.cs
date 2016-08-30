using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia;
using Gaia.GaiaSystem;

namespace Gaia.GUI
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            MainForm mainForm = new MainForm();
            ConsoleMessanger console = new ConsoleMessanger(mainForm);
            GlobalAccess.Init(mainForm, console);
            Application.Run(mainForm);
        }
    }
}

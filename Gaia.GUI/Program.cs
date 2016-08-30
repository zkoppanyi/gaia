using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

using Gaia;
using Gaia.Core;
using Gaia.Exceptions;

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

            try
            {
                GlobalAccess.Init(mainForm, console);
            }
            catch (GaiaAssertException ex)
            {
                String msg = "Error during starting the application: " + ex.Message;
                MessageBox.Show(mainForm, msg, "Error during starting the application", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            Application.Run(mainForm);


        }
    }
}

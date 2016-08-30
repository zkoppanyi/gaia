using Gaia.GaiaSystem;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gaia.GUI.Dialogs
{   
    public partial class Console : Form
    {
        public Console()
        {
            InitializeComponent();
        }

        public void WriteConsole(String text, ConsoleMessageType type)
        {
            textConsole.Text += text + Environment.NewLine;
        }

        private void Console_Load(object sender, EventArgs e)
        {

        }

        private void textConsole_TextChanged(object sender, EventArgs e)
        {

        }

        private void Console_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
    }
}

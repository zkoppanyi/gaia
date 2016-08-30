﻿using Gaia.GaiaSystem;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gaia.GUI
{

    [Serializable]
    public sealed class ConsoleMessanger : IMessanger
    {
        MainForm mainForm;
        public ConsoleMessanger(MainForm mainForm)
        {
            this.mainForm = mainForm;
        }

        public bool IsCanceled()
        {
            return false;
        }

        public void Progress(double percentage)
        {
            mainForm.WriteConsole(percentage + " %", percentage + " %");
        }


        public void Write(string message, string status = null, string messageGroupStr = null, ConsoleMessageType type = ConsoleMessageType.Message)
        {
            mainForm.WriteConsole(message, status);
        }
    }
}

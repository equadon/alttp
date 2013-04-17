using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console
{
    public class OutputEventArgs : EventArgs
    {
        public string Output { get; private set; }
        public ConsoleOutputType Type { get; private set; }

        public OutputEventArgs(string output, ConsoleOutputType type)
        {
            Output = output;
            Type = type;
        }
    }
}

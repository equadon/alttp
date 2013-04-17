using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console
{
    public enum ConsoleOutputType
    {
        Command,
        Output
    }

    public class ConsoleOutput
    {
        public string Output { get; private set; }
        public ConsoleOutputType Type { get; private set; }

        public ConsoleOutput(string output, ConsoleOutputType type)
        {
            Output = output;
            Type = type;
        }

        public override string ToString()
        {
            return Output;
        }
    }
}

using System;

namespace Alttp.Console.Events
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

        public override string ToString()
        {
            if (Type == ConsoleOutputType.Command)
                return ">>> " + Output;
            return Output;
        }
    }
}

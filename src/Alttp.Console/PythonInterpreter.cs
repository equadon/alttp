using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;

namespace Alttp.Console
{
    public delegate void CommandEventHandler(object sender, OutputEventArgs e);

    public class PythonInterpreter
    {
        public event CommandEventHandler CommandReceived;
        public event CommandEventHandler CommandProcessed;

        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

        public List<string> CommandHistory { get; private set; }

        public PythonInterpreter()
        {
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();
        }

        /// <summary>
        /// Process user input.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Command output</returns>
        public string Process(string input)
        {
            input = CleanInput(input);

            OnCommandReceived(new OutputEventArgs(input, ConsoleOutputType.Command));

            string output;

            try
            {
                ScriptSource source = _engine.CreateScriptSourceFromString(input, SourceCodeKind.Expression);
                output = source.Execute(_scope).ToString();

                CommandHistory.Add(input);

                OnCommandProcessed(new OutputEventArgs(output, ConsoleOutputType.Output));
            }
            catch (Exception e)
            {
                output = "Error executing code: " + e;
            }

            return output;
        }

        /// <summary>
        /// Strip white spaces and the command prompt >>> chars.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Cleaned user input</returns>
        private string CleanInput(string input)
        {
            string cleaned = input.Trim(' ');

            if (cleaned.StartsWith(">"))
                cleaned = cleaned.TrimStart('>');

            return cleaned.Trim(' ');
        }

        private void OnCommandReceived(OutputEventArgs e)
        {
            if (CommandReceived != null)
                CommandReceived(this, e);
        }

        private void OnCommandProcessed(OutputEventArgs e)
        {
            if (CommandProcessed != null)
                CommandProcessed(this, e);
        }
    }
}

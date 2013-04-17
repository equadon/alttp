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
        public event CommandEventHandler CommandInput;
        public event CommandEventHandler CommandOutput;

        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

        public List<string> CommandHistory { get; private set; }

        public PythonInterpreter()
        {
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();

            CommandHistory = new List<string>();
        }

        /// <summary>
        /// Process user input.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Command output</returns>
        public string Process(string input)
        {
            input = Clean(input);

            OnCommandInput(new OutputEventArgs(input, ConsoleOutputType.Command));

            string output;

            if (input == String.Empty)
                return String.Empty;

            try
            {
                ScriptSource source = _engine.CreateScriptSourceFromString(input, SourceCodeKind.Expression);
                output = source.Execute(_scope).ToString();

                CommandHistory.Add(input);
            }
            catch (Exception e)
            {
                output = "Error: " + e;
            }

            OnCommandOutput(new OutputEventArgs(Clean(output), ConsoleOutputType.Output));

            return output;
        }

        /// <summary>
        /// Strip white spaces and the command prompt >>> chars.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Cleaned user input</returns>
        private string Clean(string input)
        {
            string cleaned = input.Replace("\r", "").Trim(' ');

            if (cleaned.StartsWith(">"))
                cleaned = cleaned.TrimStart('>');

            return cleaned.Trim(' ');
        }

        private void OnCommandInput(OutputEventArgs e)
        {
            if (CommandInput != null)
                CommandInput(this, e);
        }

        private void OnCommandOutput(OutputEventArgs e)
        {
            if (CommandOutput != null)
                CommandOutput(this, e);
        }
    }
}

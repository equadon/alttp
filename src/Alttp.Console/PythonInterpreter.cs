using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Console.Commands;
using IronPython;
using IronPython.Hosting;
using Microsoft.Scripting;
using Microsoft.Scripting.Hosting;
using Ninject.Extensions.Logging;

namespace Alttp.Console
{
    public delegate void CommandEventHandler(object sender, OutputEventArgs e);

    public class PythonInterpreter
    {
        public event CommandEventHandler CommandInput;
        public event CommandEventHandler CommandOutput;

        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

        protected ILogger Log { get; set; }

        public Dictionary<string, IConsoleCommand> Commands { get; private set; }

        public List<string> CommandHistory { get; private set; }

        public PythonInterpreter(ILogger logger)
        {
            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();
            Log = logger;

            Commands = new Dictionary<string, IConsoleCommand>();
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

        public void RegisterCommand(IConsoleCommand command)
        {
            Commands.Add(command.Name, command);

            _scope.SetVariable(command.Name, new Func<string>(command.Execute));

            Log.Info("Registered command: " + command.Name);
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

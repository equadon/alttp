using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
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

        private int _autoCompleteIndex = -1;
        private object _prevAutoCompleteObject = null;
        private List<string> _autoCompleteMembers;

        protected ILogger Log { get; set; }

        public Dictionary<string, IConsoleCommand> Commands { get; private set; }
        public Dictionary<string, string> Variables { get; private set; }

        public List<string> CommandHistory { get; private set; }

        public PythonInterpreter(ILogger logger)
        {
            Log = logger;

            Commands = new Dictionary<string, IConsoleCommand>();
            Variables = new Dictionary<string, string>();

            CommandHistory = new List<string>();

            _engine = Python.CreateEngine();
            _scope = _engine.CreateScope();

            var root = Directory.GetParent(Assembly.GetExecutingAssembly().Location).FullName;

            _engine.Runtime.LoadAssembly(Assembly.LoadFile(Path.Combine(root, "Alttp.Core.dll")));
            _engine.Runtime.LoadAssembly(Assembly.LoadFile(Path.Combine(root, "Alttp.Game.exe")));
        }

        /// <summary>
        /// Process user input.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Command output</returns>
        public string Process(string input)
        {
            input = Clean(input);

            CommandHistory.Add(input);

            OnCommandInput(new OutputEventArgs(input, ConsoleOutputType.Command));

            string output;

            if (input == String.Empty)
                return String.Empty;

            try
            {
                ScriptSource source = _engine.CreateScriptSourceFromString(input, SourceCodeKind.AutoDetect);
                var results = source.Execute(_scope);
                output = (results == null) ? "null" : results.ToString();
            }
            catch (Exception e)
            {
                output = "Error: " + e;
            }

            if (output != "null")
                OnCommandOutput(new OutputEventArgs(Clean(output), ConsoleOutputType.Output));

            return output;
        }

        public void AddImport(string import)
        {
            _engine.Execute(import);

            Log.Debug("Added import: " + import);
        }

        public void RegisterCommand(IConsoleCommand command)
        {
            Commands.Add(command.Name, command);

            _scope.SetVariable(command.Name, new Func<string>(command.Execute));

            Log.Debug("Registered command: " + command.Name);
        }

        public void SetVariable(string name, string description, object obj)
        {
            _scope.SetVariable(name, obj);

            if (!Variables.ContainsKey(name))
                Variables.Add(name, description);

            Log.Debug("Set variable: " + name);
        }

        /// <summary>
        /// Auto complete text and send the results back.
        /// </summary>
        /// <param name="text">Currently entered text</param>
        /// <param name="moveBackward">Move in opposite direction</param>
        /// <returns>Auto completed text</returns>
        public string AutoComplete(string text, bool moveBackward)
        {
            var cmdSplit = text.Split(' ', ';');
            text = String.Join(".", cmdSplit.Last().Split('.'));

            string res = text.Contains(".")
                ? AutoCompleteMember(text, moveBackward)
                : AutoCompleteGlobal(text, moveBackward);

            Log.Debug("Requested auto completion: \"{0}\" => \"{1}\"", text, res);

            return res;
        }

        private string AutoCompleteGlobal(string text, bool moveBackward)
        {
            string res = String.Empty;
            _autoCompleteMembers = _scope.GetVariableNames().OrderBy(x => x).ToList();

            if (text != String.Empty)
            {
                // Text already present, check for variables/functions starting with `text`.
                res = _autoCompleteMembers.FirstOrDefault(x => x.StartsWith(text));

                if (res != null && res != text)
                    return (Commands.ContainsKey(res)) ? res + "()" : res;
            }

            int lastParenthesis = text.LastIndexOf('(');
            string textWithoutParenthesis = (lastParenthesis == -1) ? text : text.Substring(0, lastParenthesis);

            if (text != String.Empty && !_autoCompleteMembers.Contains(textWithoutParenthesis))
                return text;

            // If text is a function without parenthesis just return text with parenthesis
            if (Commands.ContainsKey(text))
                return text + "()";

            if (_autoCompleteIndex == -1 && moveBackward)
                _autoCompleteIndex = _autoCompleteMembers.Count;

            NextAutoCompleteIndex(moveBackward, _autoCompleteMembers.Count);

            // Cycle through the global variables
            string name = _autoCompleteMembers[_autoCompleteIndex];

            while (name.StartsWith("_"))
            {
                NextAutoCompleteIndex(moveBackward, _autoCompleteMembers.Count);

                name = _autoCompleteMembers[_autoCompleteIndex];
            }

            if (Variables.ContainsKey(name))
                return name;
            if (Commands.ContainsKey(name))
                return name + "()";
            return text;
        }

        private string AutoCompleteMember(string text, bool moveBackward)
        {
            // Split text by dots
            var cmdSplit = text.Split(' ', ';');
            var dotSplit = cmdSplit.Last().Split('.');

            dynamic variable = AutoCompleteGetMemberVariable(dotSplit.Take(dotSplit.Length - 1));

            if (_prevAutoCompleteObject != variable)
            {
                _prevAutoCompleteObject = variable;
                _autoCompleteIndex = -1;
                _autoCompleteMembers = null;
            }

            if (variable == null)
                return text;

            if (_autoCompleteMembers == null)
            {
                _autoCompleteMembers = new List<string>();

                var type = variable.GetType();
                foreach (var m in type.GetMembers(BindingFlags.Public | BindingFlags.Instance))
                {
                    string mName = m.Name;
                    if (!(mName.StartsWith("get_") || mName.StartsWith("set_") || mName.StartsWith("add_") || mName.StartsWith("remove_") || mName.StartsWith(".")))
                    {
                        _autoCompleteMembers.Add((m.MemberType == MemberTypes.Method) ? mName + "()" : mName);
                    }
                }

                _autoCompleteMembers = _autoCompleteMembers.Distinct().OrderBy(x => x).ToList();
            }

            // Find the member starting with the last part of split[]
            string sEnd = dotSplit[dotSplit.Length - 1];
            string found = _autoCompleteMembers.FirstOrDefault(x => x.ToLower().StartsWith(sEnd.ToLower()));

            if (_autoCompleteIndex == -1 && moveBackward)
                _autoCompleteIndex = _autoCompleteMembers.Count;

            if (found == null)
            {
                if (sEnd == String.Empty)
                    NextAutoCompleteIndex(moveBackward, _autoCompleteMembers.Count);
                else
                    return text;
            }
            else
            {
                if (found != sEnd)
                    _autoCompleteIndex = _autoCompleteMembers.IndexOf(found);
                else
                    NextAutoCompleteIndex(moveBackward, _autoCompleteMembers.Count);
            }

            return String.Join(".", dotSplit.Take(dotSplit.Length - 1)) + "." + _autoCompleteMembers[_autoCompleteIndex];
        }

        private dynamic AutoCompleteGetMemberVariable(IEnumerable<string> split)
        {
            dynamic variable = null;

            var ops = _engine.Operations;

            foreach (var s in split)
            {
                if (variable == null)
                {
                    variable = _scope.GetVariable(s);
                }
                else
                {
                    variable = ops.GetMember(variable, s);
                }
            }

            return variable;
        }

        private void NextAutoCompleteIndex(bool moveBackward, int length)
        {
            if (moveBackward)
            {
                _autoCompleteIndex--;
                if (_autoCompleteIndex < 0)
                    _autoCompleteIndex = length - 1;
            }
            else
            {
                _autoCompleteIndex++;
                _autoCompleteIndex %= length;
            }
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

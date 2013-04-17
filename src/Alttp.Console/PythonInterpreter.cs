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
    public class PythonInterpreter
    {
        private readonly ScriptEngine _engine;
        private readonly ScriptScope _scope;

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

            string output;

            try
            {
                ScriptSource source = _engine.CreateScriptSourceFromString(input, SourceCodeKind.Expression);
                output = source.Execute(_scope).ToString();
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
    }
}

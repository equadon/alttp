using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Boo.Lang.Interpreter;

namespace Alttp.Console
{
    public class CommandProcessor
    {
        private readonly InteractiveInterpreter _boo;

        public CommandProcessor()
        {
            _boo = new InteractiveInterpreter();
            _boo.Ducky = true;
            _boo.RememberLastValue = true;
        }

        /// <summary>
        /// Process user input.
        /// </summary>
        /// <param name="input">User input</param>
        /// <returns>Command output</returns>
        public string Process(string input)
        {
            return "";
        }
    }
}

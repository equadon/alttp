using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console
{
    public class CommandProcessor
    {
        private Dictionary<string, IConsoleCommand> _commands;

        public CommandProcessor()
        {
            _commands = new Dictionary<string, IConsoleCommand>();
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

        public void AddCommand(IConsoleCommand command)
        {
            _commands.Add(command.Name, command);
        }
    }
}

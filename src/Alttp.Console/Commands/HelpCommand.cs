using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Scripting.Hosting;
using Microsoft.Xna.Framework;

namespace Alttp.Console.Commands
{
    /// <summary>
    /// Help command displays the help menu which lists the available commands.
    /// </summary>
    public class HelpCommand : ConsoleCommand
    {
        private readonly Dictionary<string, IConsoleCommand> _commands;
        private readonly Dictionary<string, string> _variables;

        public HelpCommand(Dictionary<string, IConsoleCommand> commands, Dictionary<string, string> variables)
            : base("help", "List available commands and variables")
        {
            _commands = commands;
            _variables = variables;
        }

        public override string Execute()
        {
            // Commands
            string output = "Commands:\n";

            foreach (KeyValuePair<string, IConsoleCommand> item in _commands.OrderBy(cmd => cmd.Key))
                output += String.Format("   {0}(): {1}\n", item.Value.Name, item.Value.Description);

            // Variables
            output += "\nVariables:\n";

            foreach (KeyValuePair<string, string> item in _variables.OrderBy(cmd => cmd.Key))
                output += String.Format("   {0}: {1}\n", item.Key, item.Value);

            return output;
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Alttp.Console.Commands
{
    /// <summary>
    /// Help command displays the help menu which lists the available commands.
    /// </summary>
    public class HelpCommand : ConsoleCommand
    {
        private readonly Dictionary<string, IConsoleCommand> _commands;

        public HelpCommand(Dictionary<string, IConsoleCommand> commands)
            : base("help", "List available commands")
        {
            _commands = commands;
        }

        public override string Execute()
        {
            string output = "";

            foreach (KeyValuePair<string, IConsoleCommand> item in _commands.OrderBy(cmd => cmd.Key))
            {
                output += String.Format("{0}: {1}\n", item.Value.Name, item.Value.Description);
            }

            return output;
        }
    }
}

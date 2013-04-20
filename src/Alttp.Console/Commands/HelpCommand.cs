using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using Alttp.Core;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
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

        public override string Execute(object obj)
        {
            string output = String.Empty;

            var gameObject = obj as IGameObject;
            if (gameObject != null)
            {
                var members = SortMembers(gameObject.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance));

                return OutputMembers(gameObject.Name, members);
            }

            var player = obj as Player;
            if (player != null)
            {
                var members = SortMembers(player.GetType().GetMembers(BindingFlags.Public | BindingFlags.Instance));

                return OutputMembers("Player", members);
            }

            return output;
        }

        private string OutputMembers(string name, Dictionary<string, List<MemberInfo>> members)
        {
            string output = String.Empty;
            var width = System.Console.WindowWidth;

            int index = 4;

            output += "Name: " + name + "\n";

            // Output Fields, Properties and Method names
            foreach (var key in members.Keys.OrderBy(x => x))
            {
                output += "\n" + key + ":\n   ";

                foreach (var m in members[key])
                {
                    string newName = m.Name + ((key == "Methods") ? "()    " : "    ");

                    if (index + newName.Length > width)
                    {
                        output += "\n   ";
                        index = 4;
                    }

                    output += newName;
                    index += newName.Length;
                }

                output += "\n";
            }

            return output;
        }

        private Dictionary<string, List<MemberInfo>> SortMembers(MemberInfo[] members)
        {
            var sorted = new Dictionary<string, List<MemberInfo>>();

            // List available members of this game object
            foreach (var memberInfo in members)
            {
                // Skip getter/setting and event add/rmove
                var mType = memberInfo.MemberType.ToString();
                var mName = memberInfo.Name;

                if (mName.StartsWith("get_") || mName.StartsWith("set_") || mName.StartsWith("add_") || mName.StartsWith("remove_") || mName.StartsWith("."))
                    continue;

                if (mType == "Method")
                    mType = "Methods";
                else if (mType == "Field")
                    mType = "Fields";
                else if (mType == "Property")
                    mType = "Properties";

                if (!sorted.ContainsKey(mType))
                    sorted[mType] = new List<MemberInfo>();
                sorted[mType].Add(memberInfo);
            }

            for (int i = 0; i < sorted.Keys.Count; i++)
            {
                var key = sorted.Keys.ToArray()[i];
                sorted[key] = sorted[key].Distinct().OrderBy(x => x.Name).ToList();
            }

            return sorted;
        }
    }
}

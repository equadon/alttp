using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console.Commands
{
    public class ConsoleCommand : IConsoleCommand
    {
        #region Properties

        public string Name { get; private set; }

        public string Description { get; private set; }

        #endregion

        public ConsoleCommand(string name, string description)
        {
            Name = name;
            Description = description;
        }

        public virtual string Execute()
        {
            throw new NotImplementedException();
        }

        public virtual string Execute(string[] arguments)
        {
            throw new NotImplementedException();
        }
    }
}

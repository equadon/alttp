using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Console
{
    public interface IConsoleCommand
    {
        string Name { get; }
        string Description { get; }

        string Execute();
        string Execute(object obj);
        string Execute(string[] arguments);
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Alttp.Console.Commands
{
    public class ExitCommand : ConsoleCommand
    {
        private readonly Game _game;

        public ExitCommand(Game game)
            : base("exit", "Exit the game")
        {
            _game = game;
        }

        public override string Execute()
        {
            _game.Exit();

            return "";
        }
    }
}

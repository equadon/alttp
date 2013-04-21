using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;

namespace Alttp.Core.UI.ContextMenus
{
    public class WorldContextMenu : ContextMenu
    {
        public WorldContextMenu()
            : base("World Menu")
        {
        }

        public void Update(Vector2 screenPos, Game game)
        {
            Position = screenPos;

            // Add commands
            AddCommand("Exit", game.Exit);

            UpdateHeight();
        }
    }
}

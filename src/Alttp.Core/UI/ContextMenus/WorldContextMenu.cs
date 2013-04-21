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
        public WorldContextMenu(Action hide, Vector2 screenPos, Game game)
            : base("World Menu", hide)
        {
            Position = screenPos;
            
            // Add commands
            AddCommand("Exit", game.Exit);

            UpdateHeight();
        }
    }
}

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
        public WorldContextMenu(Vector2 screenPos)
        {
            Position = screenPos;
            Bounds.Size = new UniVector(200, 200);
            
            // Add items
            Items.Add("World");
            Items.Add("---");
            Items.Add("Exit");
        }
    }
}

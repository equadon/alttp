using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;

namespace Alttp.Core.UI.ContextMenus
{
    public class GameObjectContextMenu : ContextMenu
    {
        public GameObjectContextMenu(Action hide, Vector2 screenPos, IGameObject gameObject)
            : base("Game Object Menu", hide)
        {
            Position = screenPos;
            
            // Add commands
            AddCommand("Hide", gameObject.Hide);

            UpdateHeight();
        }
    }
}

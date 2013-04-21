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
        public GameObjectContextMenu()
            : base("Game Object Menu")
        {
        }

        public void Update(Vector2 pos, IGameObject gameObject)
        {
            Name = gameObject.GetType().Name + " Object Menu";

            Position = pos;

            // Clear previous commands
            Items.Clear();
            Commands.Clear();

            // Add commands
            AddCommand((gameObject.IsVisible) ? "Hide" : "Show", gameObject.ToggleVisibility);

            UpdateHeight();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            string oldName = (Commands.ContainsKey("Hide")) ? "Hide" : "Show",
                   newName = (Commands.ContainsKey("Hide")) ? "Show" : "Hide";
            ChangeCommand(oldName, newName, Commands[oldName]);
        }
    }
}

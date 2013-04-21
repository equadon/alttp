using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;

namespace Alttp.Core.UI.ContextMenus
{
    public class GameObjectContextMenu : ContextMenu
    {
        protected IGameObject Object { get; set; }
        protected Link Link { get; set; }

        public GameObjectContextMenu()
            : base("Game Object Menu")
        {
        }

        public GameObjectContextMenu(string name)
            : base(name)
        {
        }

        public void Update(Vector2 pos, IGameObject gameObject, Link link)
        {
            Clear();

            Object = gameObject;
            Link = link;

            Name = Object.GetType().Name + " Object Menu";

            Position = pos;

            // Add commands
            AddCommands();

            UpdateSize();
        }

        protected override void AddCommands()
        {
            AddCommand((Object.IsVisible) ? "Hide" : "Show", Object.ToggleVisibility);
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
        }
    }
}

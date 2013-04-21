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
    public class CharacterContextMenu : GameObjectContextMenu
    {
        private Character _character;

        public CharacterContextMenu()
            : base("Character Object Menu")
        {
        }

        public void Update(Vector2 pos, IGameObject gameObject, Link link)
        {
            Clear();

            Object = gameObject;
            _character = Object as Character;
            Link = link;

            Name = Object.GetType().Name + " Character Menu";

            Position = pos;

            // Add commands
            AddCommands();

            UpdateSize();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
        }

        protected override void AddCommands()
        {
            base.AddCommands();

            if (_character != null)
                AddCommand("Unequip All", _character.UnequipAll);
        }
    }
}

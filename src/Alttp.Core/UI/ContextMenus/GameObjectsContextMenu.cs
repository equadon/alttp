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
    public class GameObjectsContextMenu : ContextMenu
    {
        private IGameObject[] _gameObjects;
        private Link _link;

        public GameObjectsContextMenu()
            : base("Game Objects Menu")
        {
        }

        public void Update(Vector2 pos, IGameObject[] gameObjects, Link link)
        {
            Clear();

            _gameObjects = gameObjects;
            _link = link;

            Name = "Game Objects (" + _gameObjects.Length + ") Menu";

            Position = pos;

            // Add commands
            AddCommand("Toggle Show/Hide", ToggleVisibility);

//            var equipment = _gameObjects as IEquipment;
//            if (equipment != null)
//                AddCommand((equipment.IsEquipped) ? "Unequip" : "Equip", ToggleEquip);

            UpdateSize();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
            ToggleCommandName("Equip", "Unequip");
        }

        private void ToggleVisibility()
        {
            foreach (var obj in _gameObjects)
                obj.ToggleVisibility();
        }

        private void ToggleEquip()
        {
        }
    }
}

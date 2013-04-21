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
            AddCommand("Show All", ShowAll);
            AddCommand("Hide All", HideAll);

            // If all game objects are IEquipment add Equip/Unequip All commands
            if (_gameObjects.All(o => o is IEquipment))
            {
                AddCommand("Equip All", EquipAll);
                AddCommand("Unequip All", UnequipAll);
            }

            UpdateSize();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
            ToggleCommandName("Equip", "Unequip");
        }

        private void ShowAll()
        {
            foreach (var o in _gameObjects)
                o.Show();
        }

        private void HideAll()
        {
            foreach (var o in _gameObjects)
                o.Hide();
        }

        private void EquipAll()
        {
            foreach (var o in _gameObjects)
            {
                var equipment = o as IEquipment;
                if (equipment != null)
                    _link.Equip(equipment);
            }
        }

        private void UnequipAll()
        {
            foreach (var o in _gameObjects)
            {
                var equipment = o as IEquipment;
                if (equipment != null)
                    _link.Unequip(equipment);
            }
        }
    }
}

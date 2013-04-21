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
        private IGameObject _gameObject;
        private Link _link;

        public GameObjectContextMenu()
            : base("Game Object Menu")
        {
        }

        public void Update(Vector2 pos, IGameObject gameObject, Link link)
        {
            Clear();

            _gameObject = gameObject;
            _link = link;

            Name = _gameObject.GetType().Name + " Object Menu";

            Position = pos;

            // Add commands
            AddCommand((_gameObject.IsVisible) ? "Hide" : "Show", gameObject.ToggleVisibility);

            var equipment = _gameObject as IEquipment;
            if (equipment != null)
                AddCommand((equipment.IsEquipped) ? "Unequip" : "Equip", ToggleEquip);

            UpdateSize();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
            ToggleCommandName("Equip", "Unequip");
        }

        private void ToggleEquip()
        {
            var equipment = _gameObject as IEquipment;
            if (equipment != null)
            {
                if (_link.IsEquipped(equipment))
                    _link.Unequip(equipment);
                else
                    _link.Equip(equipment);
            }
        }
    }
}

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
    public class EquipmentContextMenu : GameObjectContextMenu
    {
        private IEquipment _equipment;

        public EquipmentContextMenu()
            : base("Equipment Menu")
        {
        }

        public new void Update(Vector2 pos, IGameObject gameObject, Link link)
        {
            Clear();

            Object = gameObject;
            _equipment = Object as IEquipment;
            Link = link;

            Name = Object.GetType().Name + " Equipment Menu";

            Position = pos;

            // Add commands
            AddCommands();

            UpdateSize();
        }

        public override void Execute(int row)
        {
            base.Execute(row);

            ToggleCommandName("Show", "Hide");
            ToggleCommandName("Equip", "Unequip");
        }

        protected override void AddCommands()
        {
            base.AddCommands();

            if (_equipment != null)
                AddCommand((_equipment.IsEquipped) ? "Unequip" : "Equip", ToggleEquip);
        }

        private void ToggleEquip()
        {
            if (_equipment != null)
            {
                if (Link.IsEquipped(_equipment))
                    Link.Unequip(_equipment);
                else
                    Link.Equip(_equipment);
            }
        }
    }
}

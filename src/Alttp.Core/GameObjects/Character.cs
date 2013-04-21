using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.Shields;
using Microsoft.Xna.Framework;

namespace Alttp.Core.GameObjects
{
    public class Character : GameObject
    {
        public Dictionary<string, IEquipment> Equipment { get; private set; }

        public Shield Shield
        {
            get { return Equipment["shield"] as Shield; }
        }

        public bool IsShieldEquipped
        {
            get { return (Equipment.ContainsKey("shield")) && Equipment["shield"] != null; }
        }

        public Character(AnimationsDict animations, string currentAnimation = "")
            : this(Vector2.Zero, animations, currentAnimation)
        {
        }

        public Character(Vector2 position, AnimationsDict animations, string currentAnimation = "")
            : base(position, animations, currentAnimation)
        {
            Equipment = new Dictionary<string, IEquipment>();
        }

        public virtual void Attack()
        {
        }

        /// <summary>
        /// Equip equipment.
        /// </summary>
        /// <param name="equipment">Equipment to equip.</param>
        public void Equip(IEquipment equipment)
        {
            var shield = equipment as IShield;

            if (shield != null)
            {
                // If another shield is already equipped set its position to Position
                if (IsShieldEquipped)
                    Equipment["shield"].UnequippedBy(this);

                Equipment["shield"] = shield;
            }

            equipment.EquippedBy(this);
        }

        /// <summary>
        /// Unequip equipment.
        /// </summary>
        public void Unequip(IEquipment equipment)
        {
            var shield = equipment as IShield;

            if (shield != null)
            {
                Equipment["shield"] = null;
                Equipment.Remove("shield");
            }

            equipment.UnequippedBy(this);
        }

        public void UnequipAll()
        {
            var keys = Equipment.Keys.ToArray();
            for (int i = 0; i < keys.Length; i++)
            {
                string key = keys[i];
                var equipment = Equipment[key];
                equipment.UnequippedBy(this);
                Equipment.Remove(key);
            }
        }

        /// <summary>
        /// Checks if the specified equipment is equipped
        /// </summary>
        /// <param name="equipment">Equipment object</param>
        /// <returns>True if equipment is equipped</returns>
        public bool IsEquipped(IEquipment equipment)
        {
            return Equipment.ContainsValue(equipment);
        }
    }
}

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
        public IShield Shield { get; private set; }

        public bool IsShieldEquipped
        {
            get { return Shield != null; }
        }

        public Character(AnimationsDict animations, string currentAnimation = "")
            : this(Vector2.Zero, animations, currentAnimation)
        {
        }

        public Character(Vector2 position, AnimationsDict animations, string currentAnimation = "")
            : base(position, animations, currentAnimation)
        {
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
                    Shield.UnequippedBy(this);

                Shield = shield;
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
                Shield = null;

            equipment.UnequippedBy(this);
        }

        public void UnequipAll()
        {
            Shield.UnequippedBy(this);
            Shield = null;
        }

        /// <summary>
        /// Checks if the specified equipment is equipped
        /// </summary>
        /// <param name="equipment">Equipment object</param>
        /// <returns>True if equipment is equipped</returns>
        public bool IsEquipped(IEquipment equipment)
        {
            return (Shield == equipment);
        }
    }
}

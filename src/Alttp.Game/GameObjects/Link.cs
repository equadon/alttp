using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Animation;
using Alttp.Core.Graphics;
using Alttp.Shields;
using Microsoft.Xna.Framework;
using Ninject.Extensions.Logging;

namespace Alttp.GameObjects
{
    public class Link : GameObject
    {
        public IShield Shield { get; private set; }

        public bool ShieldEquipped
        {
            get { return Shield != null; }
        }

        public Link(Vector2 position, AnimationsDict animations, Sprite shadowSprite)
            : base(position, animations, "/Idle/Down")
        {
            MaxSpeed = 1.3f;
            Animation.Fps = 60;

            Shadow = new Shadow(this, shadowSprite, new Vector2(2, 15));
        }

        public override void Draw(Nuclex.Ninject.Xna.ISpriteBatch batch)
        {
            if (Shadow != null)
                Shadow.Draw(batch);

            if (ShieldEquipped)
            {
                var obj = (Shield as GameObject);
                if (obj.Frame == null)
                    Frame.Draw(batch, Frame, Position);
                else
                    Frame.Draw(batch, Frame, Position, obj.Frame);
            }
            else
            {
                Frame.Draw(batch, Frame, Position);
            }
        }

        public override void Move(Vector2 direction)
        {
            // Can't move if we're attacking
            if (IsAttacking)
                return;

            base.Move(direction);

            ChangeAnimation("/Run/" + DirectionText, AnimationPlayAction.Loop, GameObjectState.Moving);
        }

        public override void Attack()
        {
            base.Attack();

            ChangeAnimation("/Swing/Sword/" + DirectionText, AnimationPlayAction.PlayOnce, GameObjectState.Attacking);

            Animation.Finished += IdleAnimationOnFinished;
        }

        /// <summary>
        /// Equip equipment.
        /// </summary>
        /// <param name="equipment">Equipment to equip.</param>
        public void Equip(IEquipment equipment)
        {
            var shield = equipment as IShield;

            if (shield != null)
                Shield = shield;
        }

        /// <summary>
        /// Unequip shield.
        /// </summary>
        public void UnequipShield()
        {
            Shield = null;
        }

        public override void Idle()
        {
            base.Idle();

            ChangeAnimation("/Idle/" + DirectionText, AnimationPlayAction.Loop, GameObjectState.Idle);
        }

        /// <summary>
        /// Set animation to idle once it's finished.
        /// </summary>
        private void IdleAnimationOnFinished(object sender, EventArgs eventArgs)
        {
            ChangeAnimation("/Idle/" + DirectionText, AnimationPlayAction.Loop, GameObjectState.Idle);

            Animation.Finished -= IdleAnimationOnFinished;
        }
    }
}

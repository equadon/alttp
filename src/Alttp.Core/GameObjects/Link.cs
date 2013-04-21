using System;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.Graphics;
using Alttp.Core.Shields;
using Microsoft.Xna.Framework;

namespace Alttp.Core.GameObjects
{
    public class Link : Character
    {
        public Link(Vector2 position, AnimationsDict animations, Sprite shadowSprite)
            : base(position, animations, "/Idle/Down")
        {
            MaxSpeed = 1.3f;
            Animation.Fps = 60;

            Shadow = new Shadow(this, shadowSprite, new Vector2(2, 15));
        }

        public override void Draw(Nuclex.Ninject.Xna.ISpriteBatch batch)
        {
            if (IsHidden)
                return;

            if (Shadow != null)
                Shadow.Draw(batch);

            if (IsShieldEquipped)
            {
                if (Shield.Frame == null)
                    Frame.Draw(batch, Frame, Position);
                else
                    Frame.Draw(batch, Frame, Position, Shield.Frame);
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

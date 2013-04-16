using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Animation;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;
using Ninject.Extensions.Logging;

namespace Alttp.GameObjects
{
    public class Link : GameObject
    {
        public Link(ILogger logger, Vector2 position, AnimationsDict animations)
            : base(logger, position, animations, "/Idle/Down")
        {
            MaxSpeed = 1.3f;
            Animation.Fps = 60;

            Log.Debug("Loaded \"Link\" with {0} animation(s).", animations.Keys.Count);
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

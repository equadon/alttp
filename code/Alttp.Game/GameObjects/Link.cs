﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;

namespace Alttp.GameObjects
{
    public class Link : GameObject
    {
        public Link(Vector2 position, AnimationsDict animations)
            : base(position, animations, "/Idle/Down")
        {
            MaxSpeed = 1.3f;
            Animation.Fps = 60;

            Pause();
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            Play("/Run/" + DirectionText, AnimationPlayAction.Loop);
        }

        /// <summary>
        /// Attack with the currently selected IWeapon.
        /// </summary>
        public override void Attack()
        {
            base.Attack();

            Play("/Swing/Sword/" + DirectionText, AnimationPlayAction.PlayOnce, "/Idle/" + DirectionText);
        }

        public override void Stop()
        {
            base.Stop();

            Play("/Idle/" + DirectionText, AnimationPlayAction.Loop);
        }
    }
}

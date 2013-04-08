using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Animation;
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
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);
        }

        public override void Attack()
        {
            base.Attack();
        }

        public override void Stop()
        {
            base.Stop();
        }
    }
}

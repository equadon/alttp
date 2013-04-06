﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Graphics;
using Alttp.Engine;
using Microsoft.Xna.Framework;

namespace Alttp.GameObjects
{
    public class Link : GameObject
    {
        public Link(Vector2 position, AnimationsDict animations)
            : base(position,  animations, "/Idle/Down")
        {
            Fps = 58f;
            Pause();
        }

        public override void Move(Vector2 direction)
        {
            base.Move(direction);

            Play("/Run/" + DirectionText);
        }

        public override void Stop()
        {
            base.Stop();

            Play("/Idle/" + DirectionText);
        }
    }
}

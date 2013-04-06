using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Engine;
using Microsoft.Xna.Framework;

namespace Alttp.GameObjects
{
    public class Link : GameObject
    {
        public Link(Vector2 position)
            : base(position,  "/Idle/Down")
        {
            Fps = 60f;
            Pause();
        }

        public override void Move(Vector2 direction)
        {
            Play("/Run/" + DirectionText);

            base.Move(direction);
        }

        public override void Stop()
        {
            base.Stop();

            Play("/Idle/" + DirectionText);
        }
    }
}

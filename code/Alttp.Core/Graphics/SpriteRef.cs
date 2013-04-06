using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.Graphics
{
    public class SpriteRef
    {
        public string Name { get; private set; }

        public int X { get; private set; }
        public int Y { get; private set; }
        public int Z { get; private set; }

        public float Rotation { get; private set; }

        public bool FlipH { get; private set; }
        public bool FlipV { get; private set; }

        public SpriteEffects SpriteEffects
        {
            get
            {
                var effects = SpriteEffects.None;
                if (FlipH)
                    effects |= SpriteEffects.FlipHorizontally;
                if (FlipV)
                    effects |= SpriteEffects.FlipVertically;
                return effects;
            }
        }

        public SpriteRef(string name, int x, int y, int z, float angle, bool flipH, bool flipV)
        {
            Name = name;
            X = x;
            Y = y;
            Z = z;
            Rotation = angle;
            FlipH = flipH;
            FlipV = flipV;
        }
    }
}

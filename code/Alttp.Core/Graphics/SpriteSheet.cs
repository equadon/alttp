using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.Graphics
{
    public class SpriteSheet
    {
        public Dictionary<string, Sprite[]> Sprites { get; private set; }

        public Texture2D Texture { get; private set; }

        public SpriteSheet(Texture2D texture, Dictionary<string, Sprite[]> sprites)
        {
            Sprites = sprites;
            Texture = texture;
        }
    }
}

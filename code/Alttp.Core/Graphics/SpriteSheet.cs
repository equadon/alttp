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

        /// <summary>
        /// Find and return a sprite in this SpriteSheet.
        /// </summary>
        /// <param name="fullName">Full name and path of the sprite</param>
        /// <returns>A sprite object or null if none was found</returns>
        public Sprite FindSprite(string fullName)
        {
            string[] parts = fullName.Split('/');

            string path = String.Join("/", parts.Take(parts.Length - 1));
            string name = parts[parts.Length - 1];

            foreach (var sprite in Sprites[path])
                if (sprite.Name == name)
                    return sprite;

            return null;
        }
    }
}

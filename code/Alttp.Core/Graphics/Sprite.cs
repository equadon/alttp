using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.Graphics
{
    public class Sprite
    {
        public Texture2D Texture { get; private set; }

        public string Name { get; private set; }
        public string Path { get; private set; }

        public Rectangle Source { get; private set; }

        public Vector2 Origin
        {
            get { return new Vector2(Source.Width / 2f, Source.Height / 2f); }
        }

        public string FullName
        {
            get { return String.Join("/", Path, Name); }
        }

        public Sprite(Texture2D texture, string name, string path, int x, int y, int width, int height)
        {
            Texture = texture;
            Name = name;
            Path = path;

            Source = new Rectangle(x, y, width, height);
        }
    }
}

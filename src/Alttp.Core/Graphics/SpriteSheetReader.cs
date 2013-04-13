using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.Graphics
{
    public class SpriteSheetReader : ContentTypeReader<SpriteSheet>
    {
        #region Overrides of ContentTypeReader<SpriteSheet>

        protected override SpriteSheet Read(ContentReader input, SpriteSheet existingInstance)
        {
            if (input == null) throw new ArgumentNullException("SpriteSheetReader input");

            var texture = (Texture2D) input.ReadExternalReference<Texture>();

            var keys = input.ReadObject<string[]>();
            var sprites = new Dictionary<string, Sprite[]>();

            foreach (var key in keys)
            {
                int spriteCount = input.ReadInt32();

                if (!sprites.ContainsKey(key))
                    sprites[key] = new Sprite[spriteCount];

                for (int i = 0; i < spriteCount; i++)
                {
                    var name = input.ReadString();
                    var path = input.ReadString();

                    var x = input.ReadInt32();
                    var y = input.ReadInt32();

                    var w = input.ReadInt32();
                    var h = input.ReadInt32();

                    sprites[key][i] = new Sprite(texture, name, path, x, y, w, h);
                }
            }

            var spriteSheet = existingInstance ?? new SpriteSheet(texture, sprites);

            return spriteSheet;
        }

        #endregion
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using Alttp.DarkFunction.Data;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Processors;
using Microsoft.Xna.Framework.Content.Pipeline.Serialization.Compiler;

namespace Alttp.DarkFunction
{
    [ContentTypeWriter]
    public class DarkFunctionSpritesWriter : ContentTypeWriter<SpriteSheetData>
    {
        protected override void Write(ContentWriter output, SpriteSheetData data)
        {
            output.WriteExternalReference(data.ImageRef);

            output.WriteObject(data.Sprites.Keys.ToArray());

            foreach (var key in data.Sprites.Keys)
            {
                var sprites = data.Sprites[key];
                output.Write(sprites.Count);

                foreach (SpriteData sprite in sprites)
                {
                    output.Write(sprite.Name);
                    output.Write(sprite.Path);

                    output.Write(sprite.X);
                    output.Write(sprite.Y);

                    output.Write(sprite.Width);
                    output.Write(sprite.Height);
                }
            }
        }

        public override string GetRuntimeReader(TargetPlatform targetPlatform)
        {
            return "Alttp.Core.Graphics.SpriteSheetReader, Alttp.Core";
        }

        public override string GetRuntimeType(TargetPlatform targetPlatform)
        {
            return "Alttp.Core.Graphics.SpriteSheet, Alttp.Core";
        }
    }
}

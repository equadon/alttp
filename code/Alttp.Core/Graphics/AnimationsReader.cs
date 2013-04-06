using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.Graphics
{
    public class AnimationsReader : ContentTypeReader<AnimationsDict>
    {
        #region Overrides of ContentTypeReader<SpriteSheet>

        protected override AnimationsDict Read(ContentReader input, AnimationsDict data)
        {
            if (input == null) throw new ArgumentNullException("AnimationsReader input");

            var spriteSheet = input.ReadExternalReference<SpriteSheet>();

            var anims = new AnimationsDict();

            int animationCount = input.ReadInt32();
            for (int i = 0; i < animationCount; i++)
            {
                string animName = input.ReadString();
                int animLoops = input.ReadInt32();

                // read frames
                int frameCount = input.ReadInt32();

                if (!anims.ContainsKey(animName))
                    anims[animName] = new Frame[frameCount];

                for (int j = 0; j < frameCount; j++)
                {
                    int frameIndex = input.ReadInt32();
                    int frameDelay = input.ReadInt32();

                    // sprite refs
                    int spriteCount = input.ReadInt32();

                    var spriteRefs = new SpriteRef[spriteCount];

                    for (int k = 0; k < spriteCount; k++)
                    {
                        string sprName = input.ReadString();
                        int sprX = input.ReadInt32();
                        int sprY = input.ReadInt32();
                        int sprZ = input.ReadInt32();
                        var sprAngle = (float) input.ReadDecimal();
                        bool sprFlipH = input.ReadBoolean();
                        bool sprFlipV = input.ReadBoolean();

                        var sprite = spriteSheet.FindSprite(sprName);

                        spriteRefs[k] = new SpriteRef(sprite, sprName, sprX, sprY, sprZ, sprAngle, sprFlipH, sprFlipV);
                    }

                    anims[animName][j] = new Frame(frameIndex, spriteRefs);
                }
            }

            return anims;
        }

        #endregion
    }
}

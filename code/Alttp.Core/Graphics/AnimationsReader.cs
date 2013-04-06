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

            var anims = new Dictionary<string, List<Frame>>();

            int animationCount = input.ReadInt32();
            for (int i = 0; i < animationCount; i++)
            {
                string animName = input.ReadString();
                int animLoops = input.ReadInt32();

                // read frames
                int frameCount = input.ReadInt32();

                if (!anims.ContainsKey(animName))
                    anims[animName] = new List<Frame>();

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

                    // sort sprite refs by z value
                    var sortedSpriteRefs = (from spriteRef in spriteRefs.ToList()
                                           orderby spriteRef.Z
                                           select spriteRef).OrderByDescending(x => x.Z).ToArray();

                    anims[animName].Add(new Frame(frameIndex, sortedSpriteRefs));

                    // Add one extra frame for each delay
                    for (int l = 0; l < frameDelay - 1; l++)
                        anims[animName].Add(new Frame(frameIndex, sortedSpriteRefs));
                }
            }

            var a = new AnimationsDict();
            foreach (var key in anims.Keys)
                a[key] = anims[key].ToArray();

            return a;
        }

        #endregion
    }
}

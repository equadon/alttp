using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;

namespace Alttp.Core.Graphics
{
    public class Frame
    {
        public int Index { get; private set; }

        public SpriteRef[] SpriteRefs { get; private set; }

        public Frame(int index, SpriteRef[] spriteRefs)
        {
            Index = index;
            SpriteRefs = spriteRefs;
        }

        /// <summary>
        /// Draw sprites in this frame.
        /// </summary>
        /// <param name="batch">SpriteBatch to draw with</param>
        /// <param name="position">World position of frame</param>
        public void Draw(ISpriteBatch batch, Vector2 position)
        {
            foreach (var spriteRef in SpriteRefs)
            {
                Vector2 spritePos = position;
                spritePos.X += spriteRef.X;
                spritePos.Y += spriteRef.Y;

                batch.Draw(spriteRef.Sprite.Texture, spritePos, spriteRef.Sprite.Source, Color.White, spriteRef.Rotation, spriteRef.Sprite.Origin, 1, spriteRef.SpriteEffects, 0);
            }
        }
    }
}

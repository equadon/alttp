using System.Collections.Generic;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Core.Animation
{
    public class Frame
    {
        public RectangleF Bounds { get; private set; }

        public int Index { get; private set; }

        public SpriteRef[] SpriteRefs { get; private set; }

        public Vector2 Origin
        {
            get { return new Vector2(Bounds.Width / 2f, Bounds.Height / 2f); }
        }

        public Frame(int index, SpriteRef[] spriteRefs, RectangleF bounds)
        {
            Index = index;
            Bounds = bounds;

            // TEMP TODO: Remove shadow sprite since we draw that separately. Eventually we need
            // to delete it from the .anim file.
            var refs = new List<SpriteRef>();
            foreach (var spriteRef in spriteRefs)
                if (spriteRef.Name != "/Shadow")
                    refs.Add(spriteRef);

            SpriteRefs = refs.ToArray();
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

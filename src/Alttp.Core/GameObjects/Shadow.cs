﻿using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;

namespace Alttp.Core.GameObjects
{
    public class Shadow
    {
        public Sprite Sprite { get; private set; }

        public IGameObject Parent { get; private set; }

        public Vector2 Offset { get; private set; }

        public Vector2 Position
        {
            get { return new Vector2(Parent.Position.X + Offset.X - Sprite.Origin.X, Parent.Position.Y + Offset.Y - Sprite.Origin.Y); }
        }

        public Shadow(IGameObject parent, Sprite sprite, Vector2 offset)
        {
            Parent = parent;
            Sprite = sprite;
            Offset = offset;
        }

        public void Draw(ISpriteBatch batch)
        {
            batch.Draw(Sprite.Texture, Position, Sprite.Source, Color.White);
        }
    }
}

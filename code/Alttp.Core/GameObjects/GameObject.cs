using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Engine
{
    public class GameObject
    {
        public static readonly List<GameObject> GameObjects = new List<GameObject>();

        private int _frameIndex;
        private double _frameDelay;

        #region Properties

        public Vector2 Position { get; protected set; }
        public Vector2 Direction { get; protected set; }

        public bool Paused { get; private set; }

        public string AnimationName { get; private set; }

        /// <summary>GameObject's animation FPS.</summary>
        public float Fps { get; protected set; }

        /// <summary>Returns the current direction as text (Up, Down, Left, Right)</summary>
        public string DirectionText
        {
            get
            {
                if (Direction.X < 0)
                    return "Left";
                if (Direction.X > 0)
                    return "Right";
                if (Direction.Y < 0)
                    return "Up";
                return "Down";
            }
        }

        private double FrameDelay
        {
            get { return 1 / Fps; }
        }

        public int FrameIndex
        {
            get { return _frameIndex; }
            set
            {
//                if (value < 0)
//                    value = Animation.Frames.Count - 1;
//                else
//                    value %= Animation.Frames.Count;
                _frameIndex = value;
            }
        }

        public Rectangle Bounds
        {
            get { return Rectangle.Empty; }
        }

        #endregion

        public GameObject(Vector2 position, string currentAnimation)
        {
            AnimationName = currentAnimation;

            Position = position;
            Direction = new Vector2(0, 1);

            GameObjects.Add(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                _frameDelay += gameTime.ElapsedGameTime.TotalSeconds;
                if (_frameDelay > FrameDelay)
                {
                    FrameIndex++;
                    _frameDelay = 0;
                }
            }
        }

        public virtual void Draw(ISpriteBatch batch)
        {
        }

        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();
            Direction = direction;

            Resume();
        }

        protected void Play(string animation)
        {
        }

        protected void Resume()
        {
            Paused = false;
        }

        protected void Pause()
        {
            Paused = true;
        }

        public virtual void Stop()
        {
            Pause();
            FrameIndex = 0;
        }

        /// <summary>
        /// Find objects inside the specified area.
        /// </summary>
        /// <param name="bounds">Area we're looking for object</param>
        /// <param name="camera">Camera object we use to find world coordinates</param>
        /// <returns>The first GameObject found</returns>
        public static GameObject Find(Rectangle bounds, Camera camera)
        {
            foreach (var obj in GameObjects)
            {
                var screenPos = camera.WorldToScreen(obj.Position);

                if (bounds.Contains((int)screenPos.X, (int)screenPos.Y))
                    return obj;
            }

            return null;
        }

//        public static void DrawFrame(ISpriteBatch batch, SpriteAnimationFrameDefinition frame, Vector2 position)
//        {
//            foreach (var sprRef in frame.SpriteReferences)
//            {
//                var origin = new Vector2(sprRef.Sprite.SourceRectangle.Width / 2, sprRef.Sprite.SourceRectangle.Height / 2);
//
//                SpriteEffects spriteEffects;
//                if (sprRef.FlipH && sprRef.FlipV)
//                    spriteEffects = SpriteEffects.FlipHorizontally | SpriteEffects.FlipVertically;
//                else if (!sprRef.FlipH && !sprRef.FlipV)
//                    spriteEffects = SpriteEffects.None;
//                else
//                    spriteEffects = (sprRef.FlipH) ? SpriteEffects.FlipHorizontally : SpriteEffects.FlipVertically;
//
//                var pos = new Vector2(
//                    position.X + sprRef.Position.X,
//                    position.Y + sprRef.Position.Y);
//
//                batch.Draw(sprRef.Sprite.SourceTexture, pos, sprRef.Sprite.SourceRectangle, Color.White, 0, origin, 1, spriteEffects, 0);
//            }
//        }
    }
}

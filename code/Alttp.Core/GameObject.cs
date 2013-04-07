using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Graphics;
using Alttp.Core.TileEngine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Core
{
    public class GameObject
    {
        public static readonly List<GameObject> GameObjects = new List<GameObject>();

        private readonly AnimationsDict _animations;

        private int _frameIndex;

        private double _moveStartTime;

        private Vector2 _position;

        #region Properties

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

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

        public Frame[] Frames
        {
            get { return _animations[AnimationName]; }
        }

        public Frame Frame
        {
            get { return Frames[FrameIndex]; }
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
                if (value < 0)
                    value = Frames.Length - 1;
                else
                    value %= Frames.Length;
                _frameIndex = value;
            }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)(Position.X + Frame.Bounds.Left), (int)(Position.Y + Frame.Bounds.Top), (int)Frame.Bounds.Width, (int)Frame.Bounds.Height); }
        }

        public RectangleF BoundsF
        {
            get { return new RectangleF(Position.X + Frame.Bounds.Left, Position.Y + Frame.Bounds.Top, Frame.Bounds.Width, Frame.Bounds.Height); }
        }

        #endregion

        public GameObject(Vector2 position, AnimationsDict animations, string currentAnimation)
        {
            _animations = animations;

            AnimationName = currentAnimation;

            Position = position;
            Direction = new Vector2(0, 1);

            GameObjects.Add(this);
        }

        public virtual void Update(GameTime gameTime)
        {
            if (!Paused)
            {
                int newFrame = (int) ((gameTime.TotalGameTime.TotalSeconds - _moveStartTime) / FrameDelay) % Frames.Length;

                FrameIndex = newFrame;
            }
        }

        public virtual void Draw(ISpriteBatch batch)
        {
            Frame.Draw(batch, Position);
        }

        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();
            Direction = direction;

            Resume();
        }

        public void BeginMove(double startTime)
        {
            _moveStartTime = startTime;
        }

        protected void Play(string animation)
        {
            if (animation != AnimationName)
            {
                Resume();
                AnimationName = animation;
            }
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
        /// Find object inside the specified area.
        /// </summary>
        /// <param name="region">Area we're looking for object</param>
        /// <param name="camera">Camera object we use to find world coordinates</param>
        /// <returns>The first GameObject found</returns>
        public static GameObject Find(Rectangle region, Camera camera)
        {
            foreach (var obj in GameObjects)
            {
                var bounds = camera.WorldToScreen(obj.Bounds);

                if (region.Intersects(bounds))
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Find all objects inside the specified area.
        /// </summary>
        /// <param name="region">Area we're looking for object</param>
        /// <param name="camera">Camera object we use to find world coordinates</param>
        /// <returns>The first GameObject found</returns>
        public static GameObject[] FindAll(Rectangle region, Camera camera)
        {
            var objects = new List<GameObject>();

            foreach (var obj in GameObjects)
            {
                var bounds = camera.WorldToScreen(obj.Bounds);

                if (region.Intersects(bounds))
                    objects.Add(obj);
            }

            return objects.ToArray();
        }
    }
}

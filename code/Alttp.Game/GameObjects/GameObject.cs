using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.GameObjects
{
    public enum AnimationPlayAction
    {
        Loop,
        PlayOnce,
        LoopReverse
    }

    public class GameObject
    {
        public static readonly List<GameObject> GameObjects = new List<GameObject>();

        private readonly AnimationsDict _animations;

        private int _frameIndex;

        private double _moveStartTime;

        private Vector2 _position;

        #region Properties

        public float Speed { get; protected set; }

        public AnimationPlayAction AnimationPlayAction { get; protected set; }

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
            protected set
            {
                if (value < 0)
                {
                    value = (AnimationPlayAction == AnimationPlayAction.Loop) ? Frames.Length - 1 : 0;
                }
                else
                {
                    if (value >= Frames.Length - 1 &&
                        AnimationPlayAction == AnimationPlayAction.PlayOnce)
                    {
                        FrameIndex = 0;
                    }
                    else
                    {
                        value %= Frames.Length;
                    }
                }
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
                _moveStartTime += gameTime.ElapsedGameTime.TotalSeconds;

                if (!(AnimationPlayAction == AnimationPlayAction.PlayOnce && FrameIndex == Frames.Length - 1) &&
                    _moveStartTime >= FrameDelay)
                {
                    FrameIndex++;
                    _moveStartTime = 0;
                }

                _position.X += Speed * Direction.X * (float) (gameTime.ElapsedGameTime.TotalSeconds * Fps);
                _position.Y += Speed * Direction.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds * Fps) * 0.75f;
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

        protected void Play(string animation, AnimationPlayAction action)
        {
            if (animation != AnimationName)
            {
                AnimationName = animation;
                AnimationPlayAction = action;
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
        /// <param name="cameraPosition">Position of the camera</param>
        /// <param name="zoom">Current camera zoom</param>
        /// <returns>The first GameObject found</returns>
        public static GameObject Find(Rectangle region, Vector2 cameraPosition, float zoom)
        {
            foreach (var obj in GameObjects)
            {
                var bounds = Utils.WorldToScreen(obj.Bounds, cameraPosition, zoom);

                if (region.Intersects(bounds))
                    return obj;
            }

            return null;
        }

        /// <summary>
        /// Find all objects inside the specified area.
        /// </summary>
        /// <param name="region">Area we're looking for object</param>
        /// <param name="cameraPosition">Position of the camera</param>
        /// <param name="zoom">Current camera zoom</param>
        /// <returns>The first GameObject found</returns>
        public static GameObject[] FindAll(Rectangle region, Vector2 cameraPosition, float zoom)
        {
            var objects = new List<GameObject>();

            foreach (var obj in GameObjects)
            {
                var bounds = Utils.WorldToScreen(obj.Bounds, cameraPosition, zoom);

                if (region.Intersects(bounds))
                    objects.Add(obj);
            }

            return objects.ToArray();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using Alttp.Core.Animation;
using Alttp.Core.Graphics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.GameObjects
{
    public class GameObject
    {
        public static readonly List<GameObject> GameObjects = new List<GameObject>();

        private readonly AnimationsDict _animations;

        private Vector2 _position;

        #region Properties

        public float MaxSpeed { get; protected set; }
        public float Speed { get; protected set; }

        public AnimationPlayAction AnimationPlayAction { get; protected set; }

        public Vector2 Position
        {
            get { return _position; }
            set { _position = value; }
        }

        public Vector2 Direction { get; protected set; }

        public string AnimationName { get; private set; }

        public Animation Animation
        {
            get { return _animations[AnimationName]; }
        }

        public Frame Frame
        {
            get { return Animation.Frame; }
        }

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
            if (Speed > 0)
            {
                _position.X += Speed * Direction.X * (float)(gameTime.ElapsedGameTime.TotalSeconds * Animation.Fps);

                // Move slightly slower vertically
                _position.Y += Speed * 0.75f * Direction.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds * Animation.Fps);
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

            Speed = MaxSpeed;
        }

        /// <summary>
        /// Attack with the currently selected IWeapon.
        /// </summary>
        public virtual void Attack()
        {
        }

//        protected void Play(string animation, AnimationPlayAction action, string nextAnimation = null)
//        {
//            if (animation != AnimationName)
//            {
//                AnimationName = animation;
//                AnimationPlayAction = action;
//
//                if (action == AnimationPlayAction.ReverseLoop ||
//                    action == AnimationPlayAction.ReversePlayOnce ||
//                    action == AnimationPlayAction.ReverseLoopBackForth ||
//                    action == AnimationPlayAction.ReversePlayOnceBackForth)
//                    FrameIndex = Frames.Length - 1;
//
//                if (nextAnimation != null &&
//                    (action == AnimationPlayAction.PlayOnce ||
//                     action == AnimationPlayAction.ReversePlayOnce ||
//                     action == AnimationPlayAction.PlayOnceBackForth ||
//                     action == AnimationPlayAction.ReversePlayOnceBackForth))
//                    NextAnimationName = nextAnimation;
//            }
//        }

//        protected void Resume()
//        {
//            Paused = false;
//        }

//        protected void Pause()
//        {
//            Paused = true;
//        }
//
        public virtual void Stop()
        {
            Speed = 0;
        }

        /// <summary>
        /// Advance to the next frame.
        /// </summary>
//        public void AdvanceFrame()
//        {
//            switch (AnimationPlayAction)
//            {
//                case AnimationPlayAction.Loop:
//                    FrameIndex++;
//                    FrameIndex %= Frames.Length;
//                    break;
//
//                case AnimationPlayAction.ReverseLoop:
//                    FrameIndex--;
//                    if (FrameIndex < 0)
//                        FrameIndex = Frames.Length - 1;
//                    break;
//
//                case AnimationPlayAction.PlayOnce:
//                    if (FrameIndex < Frames.Length - 1)
//                    {
//                        FrameIndex++;
//                    }
//                    else
//                    {
//                        if (NextAnimationName != null)
//                        {
//                            FrameIndex = 0;
//                            AnimationName = NextAnimationName;
//                            NextAnimationName = null;
//                        }
//                    }
//                    break;
//
//                case AnimationPlayAction.ReversePlayOnce:
//                    if (FrameIndex > 0)
//                        FrameIndex--;
//                    break;
//
//                case AnimationPlayAction.LoopBackForth:
//                case AnimationPlayAction.PlayOnceBackForth:
//                    // We played the animation once, stop here unless we want to loop
//                    if (!(AnimationPlayAction == AnimationPlayAction.PlayOnceBackForth &&
//                        !_advanceAnimationForward &&
//                        FrameIndex == 0))
//                    {
//                        if (_advanceAnimationForward)
//                        {
//                            FrameIndex++;
//                            if (FrameIndex >= Frames.Length - 1)
//                                _advanceAnimationForward = false;
//                        }
//                        else
//                        {
//                            FrameIndex--;
//                            if (FrameIndex <= 0)
//                                _advanceAnimationForward = AnimationPlayAction == AnimationPlayAction.LoopBackForth;
//                        }
//                    }
//                    break;
//
//                case AnimationPlayAction.ReverseLoopBackForth:
//                case AnimationPlayAction.ReversePlayOnceBackForth:
//                    // We played the animation once, stop here unless we want to loop
//                    if (!(AnimationPlayAction == AnimationPlayAction.ReversePlayOnceBackForth &&
//                        !_advanceAnimationForward &&
//                        FrameIndex == Frames.Length - 1))
//                    {
//                        if (_advanceAnimationForward)
//                        {
//                            FrameIndex--;
//                            if (FrameIndex <= 0)
//                                _advanceAnimationForward = false;
//                        }
//                        else
//                        {
//                            FrameIndex++;
//                            if (FrameIndex >= Frames.Length - 1)
//                                _advanceAnimationForward = AnimationPlayAction == AnimationPlayAction.ReverseLoopBackForth;
//                        }
//                    }
//                    break;
//            }
//        }

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

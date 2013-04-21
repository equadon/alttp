using System.Collections.Generic;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Core.GameObjects
{
    public enum GameObjectState
    {
        Idle,
        Moving,
        Attacking
    }

    public class GameObject : IGameObject
    {
        public static readonly List<GameObject> GameObjects = new List<GameObject>();

        private static int _nextAvailableIndex = 0;

        private Vector2 _position;

        #region Properties

        public AnimationsDict Animations { get; private set; }

        public int Index { get; private set; }

        public string Name { get { return GetType().Name + Index; } }

        public GameObjectState State { get; private set; }

        public bool IsVisible { get; private set; }

        public float MaxSpeed { get; protected set; }
        public float Speed { get; protected set; }

        public Vector2 Direction { get; protected set; }

        public virtual string AnimationName { get; private set; }

        public Shadow Shadow { get; protected set; }

        public virtual Vector2 Position
        {
            get { return _position; }
            set { Move(value.X, value.Y); }
        }

        public bool IsIdle { get { return State == GameObjectState.Idle; } }
        public bool IsMoving { get { return State == GameObjectState.Moving; } }
        public bool IsAttacking { get { return State == GameObjectState.Attacking; } }

        public Animation.Animation Animation
        {
            get { return Animations[AnimationName]; }
        }

        public virtual Frame Frame
        {
            get { return Animation.Frame; }
        }

        public bool IsHidden
        {
            get { return !IsVisible; }
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
            get
            {
                return (Frame == null)
                    ? Rectangle.Empty
                    : new Rectangle((int)(Position.X + Frame.Bounds.Left), (int)(Position.Y + Frame.Bounds.Top), (int)Frame.Bounds.Width, (int)Frame.Bounds.Height);
            }
        }

        public RectangleF BoundsF
        {
            get
            {
                return (Frame == null)
                    ? RectangleF.Empty
                    : new RectangleF(Position.X + Frame.Bounds.Left, Position.Y + Frame.Bounds.Top, Frame.Bounds.Width, Frame.Bounds.Height);
            }
        }

        #endregion

        public GameObject(AnimationsDict animations, string currentAnimation = "")
            : this(Vector2.Zero, animations, currentAnimation)
        {
        }

        public GameObject(Vector2 position, AnimationsDict animations, string currentAnimation = "")
        {
            Animations = animations;

            AnimationName = currentAnimation;

            Position = position;
            Direction = new Vector2(0, 1);

            IsVisible = true;

            GameObjects.Add(this);

            // Retrieve a unique index
            Index = GetUniqueIndex(GameObjects);
        }

        public virtual void Update(GameTime gameTime)
        {
            Animation.Update(gameTime);

            if (IsMoving)
            {
                _position.X += Speed * Direction.X * (float)(gameTime.ElapsedGameTime.TotalSeconds * Animation.Fps);

                // Move slightly slower vertically
                _position.Y += Speed * 0.75f * Direction.Y * (float)(gameTime.ElapsedGameTime.TotalSeconds * Animation.Fps);
            }
        }

        public virtual void Draw(ISpriteBatch batch)
        {
            if (IsHidden)
                return;

            if (Shadow != null)
                Shadow.Draw(batch);

            if (Frame != null)
                Frame.Draw(batch, Frame, Position);
        }

        public void Move(float x, float y)
        {
            _position.X = x;
            _position.Y = y;
        }

        public virtual void Move(Vector2 direction)
        {
            direction.Normalize();
            Direction = direction;

            Speed = MaxSpeed;
        }

        /// <summary>
        /// Allow this object to be rendered
        /// </summary>
        public void Show()
        {
            IsVisible = true;
        }

        /// <summary>
        /// Hide object.
        /// </summary>
        public void Hide()
        {
            IsVisible = false;
        }

        /// <summary>
        /// Toggle visibility between hidden and visible
        /// </summary>
        public void ToggleVisibility()
        {
            if (IsVisible)
                Hide();
            else
                Show();
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
        /// <summary>
        /// Force game object into an idle state.
        /// </summary>
        public virtual void Idle()
        {
            Speed = 0;
        }

        /// <summary>
        /// Change animation. Do not change if object is trying to move to a new direction.
        /// </summary>
        protected void ChangeAnimation(string newAnimation, AnimationPlayAction action, GameObjectState newState)
        {
            if (State == GameObjectState.Moving && newState == GameObjectState.Moving)
                return;

            State = newState;

            // Stop current animation if it's playing
            if (!Animation.IsStopped)
                Animation.Stop();

            AnimationName = newAnimation;
            Animation.Action = action;

            // Play new animation
            Animation.Play();
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
        public static IGameObject[] FindAll(Rectangle region, Vector2 cameraPosition, float zoom)
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

        /// <summary>
        /// Retrieve the next unique index and update the variable holding the next index.
        /// </summary>
        /// <param name="objects">List of objects</param>
        /// <returns>A unique index</returns>
        public static int GetUniqueIndex(List<GameObject> objects)
        {
            int nextIndex = _nextAvailableIndex;

            // Update the next available index
            _nextAvailableIndex++;

            return nextIndex;
        }
    }
}

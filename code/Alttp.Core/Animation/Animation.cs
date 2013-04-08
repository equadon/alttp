using System;
using Microsoft.Xna.Framework;

namespace Alttp.Core.Animation
{
    public enum AnimationState
    {
        Playing,
        Finished,
        Paused,
        Stopped
    }

    public enum AnimationPlayAction
    {
        Loop,
        PlayOnce,

        ReverseLoop,
        ReversePlayOnce,

        LoopBackForth,
        PlayOnceBackForth,

        ReverseLoopBackForth,
        ReversePlayOnceBackForth,
    }

    public class Animation
    {
        // Events
        public event AnimationEventHandler AnimationStateChanged;
        public event AnimationEventHandler FrameIndexChanged;

        #region Fields

        private double _frameDuration;

        private AnimationState _state;
        private int _frameIndex;

        #endregion

        #region Properties

        public string Name { get; private set; }
        public float Fps { get; set; }
        public AnimationPlayAction Action { get; set; }

        public Frame[] Frames { get; private set; }

        public Frame Frame
        {
            get { return Frames[FrameIndex]; }
        }

        /// <summary>Current animation frame index. The FrameIndexChanged event will be triggered when the value changes.</summary>
        public int FrameIndex
        {
            get { return _frameIndex; }
            set
            {
                if (_frameIndex != value)
                    OnFrameIndexChanged(new FrameIndexEventArgs(_frameIndex, value));
                _frameIndex = value;
            }
        }

        /// <summary>Current animation state. The AnimationStateChanged event will be triggered when the value changes.</summary>
        public AnimationState State
        {
            get { return _state; }
            set
            {
                if (_state != value)
                    OnAnimationStateChange(new AnimationStateEventArgs(_state, value));
                _state = value;
            }
        }

        // State helpers
        public bool IsPlaying { get { return _state == AnimationState.Playing; } }
        public bool IsFinished { get { return _state == AnimationState.Finished; } }
        public bool IsPaused { get { return _state == AnimationState.Paused; } }
        public bool IsStopped { get { return _state == AnimationState.Stopped; } }

        /// <summary>Amount of time the active frame will be shown.</summary>
        public double FrameTime
        {
            get { return 1 / Fps; }
        }

        #endregion

        public Animation(string name, Frame[] frames)
        {
            Name = name;
            Frames = frames;

            Fps = 60;

            Action = AnimationPlayAction.Loop;
        }

        public void Update(GameTime gameTime)
        {
            if (IsPlaying)
            {
                _frameDuration += gameTime.ElapsedGameTime.TotalSeconds;
            }
        }

        /// <summary>
        /// Play the animation.
        /// 
        /// TODO: What should happen when Play() is called while another animation is playing?
        /// </summary>
        public void Play()
        {
            State = AnimationState.Playing;
        }

        /// <summary>
        /// Pause the animation.
        /// </summary>
        public void Pause()
        {
            State = AnimationState.Paused;
        }

        /// <summary>
        /// Stop the animation and set the frame index to frameIndex
        /// </summary>
        /// <param name="frameIndex">Frame index to jump to</param>
        public void Stop(int frameIndex = 0)
        {
            State = AnimationState.Stopped;
            FrameIndex = frameIndex;
        }

        /// <summary>
        /// Reset the animation by setting the FrameIndex to its first
        /// frame depending on which Action we're using.
        /// </summary>
        public void Reset()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Invoke the AnimationStateChanged event.
        /// </summary>
        /// <param name="e"></param>
        private void OnAnimationStateChange(AnimationStateEventArgs e)
        {
            if (AnimationStateChanged != null)
                AnimationStateChanged(this, e);
        }

        /// <summary>
        /// Invoke the FrameIndexChanged event.
        /// </summary>
        /// <param name="e"></param>
        public void OnFrameIndexChanged(FrameIndexEventArgs e)
        {
            if (FrameIndexChanged != null)
                FrameIndexChanged(this, e);
        }
    }
}

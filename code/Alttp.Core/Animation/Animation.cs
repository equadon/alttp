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
        public event AnimationEventHandler Finished;
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

        /// <summary>Current animation state.</summary>
        public AnimationState State
        {
            get { return _state; }
            set
            {
                if (_state != value && value == AnimationState.Finished)
                    OnFinished(new AnimationStateEventArgs(_state, value));
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

                if (_frameDuration >= FrameTime)
                {
                    AdvanceFrame();
                }
            }
        }

        /// <summary>
        /// Play the animation.
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
        /// Advance the current frame to the next.
        /// </summary>
        private void AdvanceFrame()
        {
            switch (Action)
            {
                case AnimationPlayAction.Loop:
                    FrameIndex++;
                    FrameIndex %= Frames.Length;
                    break;
            
                case AnimationPlayAction.ReverseLoop:
                    FrameIndex--;
                    if (FrameIndex < 0)
                        FrameIndex = Frames.Length - 1;
                    break;
            
                case AnimationPlayAction.PlayOnce:
                    if (!IsFinished)
                    {
                        if (FrameIndex < Frames.Length - 1)
                        {
                            FrameIndex++;
                        }
                        else
                        {
                            FrameIndex = 0;
                            State = AnimationState.Finished;
                        }
                    }
                    break;
            
                case AnimationPlayAction.ReversePlayOnce:
                    if (FrameIndex > 0)
                        FrameIndex--;
                    break;
            
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
            }
        }

        /// <summary>
        /// Invoke the AnimationStateChanged event.
        /// </summary>
        /// <param name="e"></param>
        private void OnFinished(AnimationStateEventArgs e)
        {
            if (Finished != null)
                Finished(this, e);
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Core.Graphics
{
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
        public string Name { get; private set; }
        public float Fps { get; set; }
        public AnimationPlayAction Action { get; private set; }

        public Frame[] Frames { get; private set; }

        public int FrameIndex { get; private set; }

        public Frame Frame
        {
            get { return Frames[FrameIndex]; }
        }

        /// <summary>Amount of time the active frame will be shown.</summary>
        public double FrameTime
        {
            get { return 1 / Fps; }
        }

        public Animation(string name, Frame[] frames)
        {
            Name = name;
            Frames = frames;

            Fps = 60;

            Action = AnimationPlayAction.Loop;
        }

        public void Play()
        {
        }

        public void Pause()
        {
        }

        public void Stop()
        {
        }

        public void Reset()
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Core.Animation
{
    public delegate void AnimationEventHandler(object sender, EventArgs e);

    public class AnimationStateEventArgs : EventArgs
    {
        public AnimationState PreviousState { get; private set; }
        public AnimationState State { get; private set; }

        public AnimationStateEventArgs(AnimationState previousState, AnimationState state)
        {
            PreviousState = previousState;
            State = state;
        }
    }

    public class FrameIndexEventArgs : EventArgs
    {
        public int PreviousIndex { get; private set; }
        public int Index { get; private set; }

        public FrameIndexEventArgs(int previousIndex, int index)
        {
            PreviousIndex = previousIndex;
            Index = index;
        }
    }
}

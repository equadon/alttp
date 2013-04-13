using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.DarkFunction.Data
{
    public class AnimationData
    {
        public string Name { get; set; }
        public int Loops { get; set; }

        public List<FrameData> Frames { get; set; }

        public AnimationData()
        {
            Frames = new List<FrameData>();
        }
    }
}

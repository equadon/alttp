using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.DarkFunction.Data
{
    public class FrameData
    {
        public int Index { get; set; }
        public int Delay { get; set; }

        public List<SpriteRefData> Sprites { get; set; }

        public FrameData()
        {
            Sprites = new List<SpriteRefData>();
        }
    }
}

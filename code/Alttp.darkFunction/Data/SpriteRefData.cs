using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.DarkFunction.Data
{
    public class SpriteRefData
    {
        public string Name { get; set; }

        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public float Angle { get; set; }

        public bool FlipH { get; set; }
        public bool FlipV { get; set; }
    }
}

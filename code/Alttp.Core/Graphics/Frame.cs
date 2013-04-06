using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.Core.Graphics
{
    public class Frame
    {
        public int Index { get; private set; }

        public SpriteRef[] SpriteRefs { get; private set; }

        public Frame(int index, SpriteRef[] spriteRefs)
        {
            Index = index;
            SpriteRefs = spriteRefs;
        }
    }
}

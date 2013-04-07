using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.TileEngine;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;

namespace Alttp
{
    public class LightWorld : World
    {
        public LightWorld(IContentManager content, string mapResource)
            : base(content, mapResource)
        {
        }

        public LightWorld(Map map)
            : base(map)
        {
        }
    }
}

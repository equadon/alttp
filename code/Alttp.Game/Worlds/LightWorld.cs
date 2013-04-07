using Alttp.Core.TileEngine;
using FuncWorks.XNA.XTiled;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
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

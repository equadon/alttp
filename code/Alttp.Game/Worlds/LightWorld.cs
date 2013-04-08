using Alttp.Core.Animation;
using FuncWorks.XNA.XTiled;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class LightWorld : World
    {
        public LightWorld(IContentManager content, string mapResource, string worldObjectsResource)
            : base(content, mapResource, worldObjectsResource)
        {
        }

        public LightWorld(Map map, AnimationsDict worldObjectAnimations)
            : base(map, worldObjectAnimations)
        {
        }
    }
}

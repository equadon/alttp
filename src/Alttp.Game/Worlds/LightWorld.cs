using Alttp.Core.Animation;
using Alttp.Core.World;
using FuncWorks.XNA.XTiled;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class LightWorld : World
    {
        public LightWorld(IContentManager content, string mapResource, string worldObjectsResource, ILogger logger)
            : base(content, mapResource, worldObjectsResource, logger)
        {
        }

        public LightWorld(Map map, AnimationsDict worldObjectAnimations, ILogger logger)
            : base(map, worldObjectAnimations, logger)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Microsoft.Xna.Framework;
using Ninject.Extensions.Logging;

namespace Alttp.GameObjects
{
    public class Bush : GameObject//, ILootable, ILiftable
    {
        public Bush(ILogger logger, Vector2 position, AnimationsDict animations, string currentAnimation)
            : base(logger, position, animations, currentAnimation)
        {
        }
    }
}

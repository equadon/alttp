﻿using Alttp.Core.Animation;
using Microsoft.Xna.Framework;

namespace Alttp.Core.GameObjects
{
    public class Bush : GameObject//, ILootable, ILiftable
    {
        public Bush(Vector2 position, AnimationsDict animations, string currentAnimation)
            : base(position, animations, currentAnimation)
        {
        }
    }
}

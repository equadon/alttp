﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects.Interfaces;

namespace Alttp.Core.Shields
{
    public class BlueShield : Shield
    {
        public BlueShield(AnimationsDict animations)
            : base(ShieldType.Fighters, animations, "/Shield/Fighters/Idle/Down")
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects.Interfaces;

namespace Alttp.Core.Shields
{
    public class FireShield : Shield
    {
        public FireShield(AnimationsDict animations)
            : base(ShieldType.Fire, animations, "/Shield/Fire/Idle/Down")
        {
        }
    }
}

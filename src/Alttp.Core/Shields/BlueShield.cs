using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.GameObjects.Interfaces;

namespace Alttp.Core.Shields
{
    public class BlueShield : Shield
    {
        public BlueShield(IGameObject parent)
            : base(parent, ShieldType.Blue)
        {
        }
    }
}

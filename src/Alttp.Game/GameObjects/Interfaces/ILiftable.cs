using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Alttp.GameObjects
{
    public interface ILiftable
    {
        float Weight { get; }

        bool Lift();
        void Throw();
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Alttp.GameObjects;
using Microsoft.Xna.Framework;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Shields
{
    public enum ShieldTypes { Blue }

    public class Shield : GameObject, IShield
    {
        public IGameObject Parent { get; private set; }
        public Vector2 Offset { get; private set; }

        public ShieldTypes Type { get; private set; }

        public override string AnimationName
        {
            get { return "/Shield/" + Type.ToString() + Parent.AnimationName; }
        }

        public override Frame Frame
        {
            get { return (Animations.ContainsKey(AnimationName)) ? Animation.Frames[Parent.Animation.FrameIndex] : null; }
        }

        public override Vector2 Position
        {
            get { return Parent.Position; }
            set { }
        }

        public Shield(Vector2 position, AnimationsDict animations, IGameObject parent, Vector2 offset, ShieldTypes type)
            : base(position, animations)
        {
            Parent = parent;
            Offset = offset;
            Type = type;
        }
    }
}

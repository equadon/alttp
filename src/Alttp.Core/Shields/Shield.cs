using Alttp.Core.Animation;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;

namespace Alttp.Core.Shields
{
    public enum ShieldTypes { Blue }

    public class Shield : GameObject, IShield
    {
        public IGameObject Parent { get; private set; }

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

        public Shield(IGameObject parent, ShieldTypes type)
            : base(parent.Position, parent.Animations)
        {
            Parent = parent;
            Type = type;
        }
    }
}

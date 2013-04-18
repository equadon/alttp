using Alttp.Core.Animation;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;

namespace Alttp.Core.Shields
{
    public enum ShieldType { Blue }

    public class Shield : GameObject, IShield
    {
        public IGameObject Parent { get; set; }

        public ShieldType Type { get; private set; }

        public override string AnimationName
        {
            get
            {
                return (Parent == null)
                           ? base.AnimationName
                           : "/Shield/" + Type.ToString() + Parent.AnimationName;
            }
        }

        public override Frame Frame
        {
            get { return (Animations.ContainsKey(AnimationName)) ? Animation.Frames[Parent.Animation.FrameIndex] : null; }
        }

        public override Vector2 Position
        {
            get
            {
                return (Parent == null)
                           ? base.Position
                           : Parent.Position;
            }
            set
            {
                if (Parent == null)
                    base.Position = value;
            }
        }

        public Shield(ShieldType type, AnimationsDict animations, string animationName)
            : base(animations, animationName)
        {
            Type = type;
        }
    }
}

using Alttp.Core.Animation;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Microsoft.Xna.Framework;

namespace Alttp.Core.Shields
{
    public enum ShieldType { Fighters, Fire, Mirror }

    public class Shield : GameObject, IShield
    {
        public IGameObject Parent { get; set; }

        public ShieldType Type { get; private set; }

        public bool IsEquipped
        {
            get { return Parent != null; }
        }

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
            get
            {
                if (Parent == null)
                    return Animation.Frames[0];
                return (Animations.ContainsKey(AnimationName)) ? Animation.Frames[Parent.Animation.FrameIndex] : null;
            }
        }

        public override Vector2 Position
        {
            get
            {
                return (Parent == null)
                           ? base.Position
                           : Parent.Position;
            }
            set { base.Position = value; }
        }

        public Shield(ShieldType type, AnimationsDict animations, string animationName)
            : base(animations, animationName)
        {
            Type = type;
        }

        /// <summary>
        /// This item was equipped by its parent
        /// </summary>
        /// <param name="parent">Parent object</param>
        public void EquippedBy(IGameObject parent)
        {
            Parent = parent;
        }

        /// <summary>
        /// This item was unequipped by its parent
        /// </summary>
        /// <param name="parent">Parent object</param>
        public void UnequippedBy(IGameObject parent)
        {
            if (Parent == parent)
            {
                Parent = null;
                Position = parent.Position;
            }
        }
    }
}

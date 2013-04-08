using Alttp.Core.Animation;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.GameObjects
{
    public interface IGameObject
    {
        GameObjectState State { get; }
        float MaxSpeed { get; }
        float Speed { get; }
        Vector2 Direction { get; }
        string AnimationName { get; }
        Vector2 Position { get; set; }
        Animation Animation { get; }
        Frame Frame { get; }

        /// <summary>Returns the current direction as text (Up, Down, Left, Right)</summary>
        string DirectionText { get; }

        Rectangle Bounds { get; }
        RectangleF BoundsF { get; }
        void Update(GameTime gameTime);
        void Draw(ISpriteBatch batch);
        void Move(Vector2 direction);
    }
}
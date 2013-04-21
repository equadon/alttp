using Alttp.Core.Animation;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Core.GameObjects.Interfaces
{
    public interface IGameObject
    {
        int Index { get; }
        string Name { get; }
        GameObjectState State { get; }
        bool IsVisible { get; }
        bool IsHidden { get; }
        float MaxSpeed { get; }
        float Speed { get; }
        Vector2 Direction { get; }
        Vector2 Position { get; set; }
        AnimationsDict Animations { get; }
        string AnimationName { get; }
        Animation.Animation Animation { get; }
        Frame Frame { get; }

        /// <summary>Returns the current direction as text (Up, Down, Left, Right)</summary>
        string DirectionText { get; }

        Rectangle Bounds { get; }
        RectangleF BoundsF { get; }
        void Update(GameTime gameTime);
        void Draw(ISpriteBatch batch);
        void Move(Vector2 direction);

        void Show();
        void Hide();
        void ToggleVisibility();
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Input;
using Alttp.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.Ninject.Xna;

namespace Alttp
{
    /// <summary>
    /// Store and handle player information.
    /// </summary>
    public class Player
    {
        private readonly InputManager _input;

        public GameObject Object { get; set; }

        public Player(InputManager input)
        {
            _input = input;
        }

        public void Draw(ISpriteBatch batch)
        {
            if (Object == null)
                return;

            Object.Draw(batch);
        }

        public void Update(GameTime gameTime)
        {
            if (Object == null)
                return;

            // Handle keyboard input
            HandleKeyboardInput();
        }

        private void HandleKeyboardInput()
        {
            // Move object
            Vector2 direction = Vector2.Zero;

            if (_input.IsKeyDown(Keys.W))
                direction.Y--;
            if (_input.IsKeyDown(Keys.A))
                direction.X--;
            if (_input.IsKeyDown(Keys.S))
                direction.Y++;
            if (_input.IsKeyDown(Keys.D))
                direction.X++;

            if (direction != Vector2.Zero)
                Object.Move(direction);

            if (_input.IsKeyReleased(Keys.W))
                Object.Stop();
            if (_input.IsKeyReleased(Keys.A))
                Object.Stop();
            if (_input.IsKeyReleased(Keys.S))
                Object.Stop();
            if (_input.IsKeyReleased(Keys.D))
                Object.Stop();

            // Attack
            if (_input.IsKeyPressed(Keys.Space))
                Object.Attack();
        }
    }
}

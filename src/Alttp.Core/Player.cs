using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects;
using Alttp.Core.Graphics;
using Alttp.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Core
{
    /// <summary>
    /// Store and handle player information.
    /// </summary>
    public class Player
    {
        private readonly InputManager _input;

        public Link Link { get; private set; }

        protected ILogger Log { get; set; }

        public Player(InputManager input, IContentManager content)
        {
            _input = input;

            LoadLink(content);
        }

        private void LoadLink(IContentManager content)
        {
            // Load link
            var linkAnimations = content.Load<AnimationsDict>("GameObjects/Link/LinkAnimations");
            var linkSprites = content.Load<SpriteSheet>("GameObjects/Link/LinkSprites");

            Link = new Link(new Vector2(2230, 2820), linkAnimations, linkSprites.FindSprite("/Shadow"));
        }

        public void Draw(ISpriteBatch batch)
        {
            Link.Draw(batch);
        }

        public void Update(GameTime gameTime, bool processInput = true)
        {
            Link.Update(gameTime);

            // Handle keyboard input
            if (processInput)
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
                Link.Move(direction);

            if (_input.IsKeyReleased(Keys.W))
                Link.Idle();
            if (_input.IsKeyReleased(Keys.A))
                Link.Idle();
            if (_input.IsKeyReleased(Keys.S))
                Link.Idle();
            if (_input.IsKeyReleased(Keys.D))
                Link.Idle();

            // Attack
            if (_input.IsKeyPressed(Keys.Space))
                Link.Attack();
        }
    }
}

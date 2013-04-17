using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Input;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Console
{
    public enum ConsoleState
    {
        Opened,
        Opening,
        Closed,
        Closing
    }

    public class AlttpConsole : DrawableGameComponent
    {
        private readonly InputManager _input;
        private readonly ISpriteBatch _batch;

        protected ILogger Log { get; set; }

        public ConsoleWindow Window { get; private set; }

        public AlttpConsole(Game game, ILogger logger, InputManager input, ISpriteBatch batch)
            : base(game)
        {
            Log = logger;
            _input = input;
            _batch = batch;

            Window = new ConsoleWindow(game.GraphicsDevice.Viewport.Width / 2, game.GraphicsDevice.Viewport.Height / 2);

            Log.Debug("Initialized console");
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);

            if (Window.IsOpening)
            {
            }

            if (Window.IsClosing)
            {
            }
        }

        private void HandleInput(GameTime gameTime)
        {
            if (_input.IsKeyPressed(Keys.OemTilde))
            {
                Window.Toggle();
            }
        }

        #endregion

        public override void Draw(GameTime gameTime)
        {
            if (Window.IsClosed)
                return;

            // Render console window
        }
    }
}

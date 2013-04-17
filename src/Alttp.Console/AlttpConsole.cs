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
using Nuclex.UserInterface;

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
        private readonly GuiManager _gui;
        private readonly Game _game;

        protected ILogger Log { get; set; }

        public ConsoleWindow Window { get; private set; }

        public AlttpConsole(Game game, ILogger logger, InputManager input, ISpriteBatch batch, GuiManager gui)
            : base(game)
        {
            Log = logger;
            _game = game;
            _input = input;
            _batch = batch;
            _gui = gui;

            Log.Debug("Initialized console");
        }

        public override void Initialize()
        {
            base.Initialize();

            // Setup UI
            Window = new ConsoleWindow((int)(_game.GraphicsDevice.Viewport.Width * 0.75f), (int)(_game.GraphicsDevice.Viewport.Height * 0.67f));

            _gui.Screen.Desktop.Children.Add(Window);
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput(gameTime);

            Window.Update(gameTime);
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

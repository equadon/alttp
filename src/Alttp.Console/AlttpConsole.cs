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
using Boo.Lang.Compiler;
using Boo.Lang.Interpreter;

namespace Alttp.Console
{
    public enum ConsoleState
    {
        Opened,
        Opening,
        Closed,
        Closing
    }

    public class AlttpConsole : GameComponent
    {
        private readonly InputManager _input;
        private readonly ISpriteBatch _batch;
        private readonly GuiManager _gui;
        private readonly Game _game;

        private readonly CommandProcessor _processor;

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

            _processor = new CommandProcessor();

            Log.Debug("Initialized console");
        }

        public override void Initialize()
        {
            base.Initialize();

            // Setup UI
            Window = new ConsoleWindow(_processor, _gui.Screen, (int)(_game.GraphicsDevice.Viewport.Width * 0.75f), (int)(_game.GraphicsDevice.Viewport.Height * 0.67f));

            _gui.Screen.Desktop.Children.Add(Window);
        }

        #region Update

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            HandleInput();

            Window.Update(gameTime);
        }

        private void HandleInput()
        {
            if (_input.IsKeyPressed(Keys.OemTilde))
                Window.Toggle();
            
            if (_input.IsKeyPressed(Keys.Escape) && Window.IsOpen)
                Window.Toggle();
        }

        #endregion
    }
}

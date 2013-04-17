using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Console.Commands;
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

    public class AlttpConsole : GameComponent
    {
        private readonly InputManager _input;
        private readonly ISpriteBatch _batch;
        private readonly GuiManager _gui;
        private readonly Game _game;

        private PythonInterpreter _python;

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

            // Python interpreter
            _python = new PythonInterpreter(Log);

            // Subscribe to command events
            _python.CommandInput += PythonOnCommandInput;
            _python.CommandOutput += PythonOnCommandOutput;

            // Setup UI
            Window = new ConsoleWindow(_python, _gui.Screen, (int)(_game.GraphicsDevice.Viewport.Width * 0.75f), (int)(_game.GraphicsDevice.Viewport.Height * 0.67f));

            _gui.Screen.Desktop.Children.Add(Window);

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            // Help
            _python.RegisterCommand(new HelpCommand(_python.Commands));

            // Exit
            _python.RegisterCommand(new ExitCommand(_game));
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
            if (_input.IsKeyPressed(Keys.OemTilde) ||
                (_input.IsKeyPressed(Keys.Escape) && (Window.IsOpen || Window.IsOpening)))
            {
                Window.Toggle();
            }
        }

        #endregion

        #region Command Events

        private void PythonOnCommandInput(object sender, OutputEventArgs e)
        {
            var output = new ConsoleOutput(e.Output, e.Type);
            Window.Outputs.Add(output);
            Window.OutputList.Items.Add(e.ToString());
        }

        private void PythonOnCommandOutput(object sender, OutputEventArgs e)
        {
            // Add one item per line
            var lines = e.ToString().Split('\n');

            foreach (var line in lines)
                Window.OutputList.Items.Add(line);
        }

        #endregion
    }
}

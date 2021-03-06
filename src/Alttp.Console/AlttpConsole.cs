﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Console.Commands;
using Alttp.Core;
using Alttp.Core.Events;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.Input;
using Alttp.Core.Shields;
using Alttp.Core.World;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

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

        private readonly PythonInterpreter _python;

        private int _cmdListIndex;

        protected ILogger Log { get; set; }

        public ConsoleWindow Window { get; private set; }

        public AlttpConsole(Game game, ILogger logger, InputManager input, ISpriteBatch batch, GuiManager gui, Player player, PythonInterpreter python)
            : base(game)
        {
            Log = logger;
            _game = game;
            _input = input;
            _batch = batch;
            _gui = gui;
            _python = python;

            // Subscribe to command events
            _python.CommandInput += PythonOnCommandInput;
            _python.CommandOutput += PythonOnCommandOutput;

            AddImports();
            SetVariables(player);
            RegisterCommands();

            Log.Debug("Initialized console");
        }

        public override void Initialize()
        {
            base.Initialize();

            // Setup UI
            Window = new ConsoleWindow(_python, _gui.Screen, (int)(_game.GraphicsDevice.Viewport.Width * 0.75f), (int)(_game.GraphicsDevice.Viewport.Height * 0.67f));

            _gui.Screen.FocusChanged += ScreenOnFocusChanged;

            _gui.Screen.Desktop.Children.Add(Window);

            // Handle auto complete
            Window.CommandInput.HandleAutoComplete += CommandInputOnHandleAutoComplete;

            // Process new command
            Window.CommandInput.ProcessCommand += CommandInputOnProcessCommand;
        }

        /// <summary>
        /// Add python imports.
        /// </summary>
        private void AddImports()
        {
        }

        /// <summary>
        /// Set python global variables
        /// </summary>
        /// <param name="player">Player object</param>
        private void SetVariables(Player player)
        {
            // Player
            _python.SetVariable("player", "Player object", player);
        }

        /// <summary>
        /// Sets the camera variable in the console to the currently active camera.
        /// </summary>
        /// <param name="camera">Current active camera</param>
        public void SetActiveCamera(Camera camera)
        {
            _python.SetVariable("camera", "Currently active camera", camera);
        }

        /// <summary>
        /// Sets the selected game objects from the debug manager.
        /// 
        /// If the text box is empty add the objects variable name.
        /// </summary>
        /// <param name="selectedGameObjects">Array of selected game objects</param>
        public void SetSelectedObjects(IGameObject[] selectedGameObjects)
        {
            _python.SetVariable("objects", "Selected objects", selectedGameObjects);
        }

        /// <summary>
        /// Register global python functions
        /// </summary>
        private void RegisterCommands()
        {
            // Help
            var help = new HelpCommand(_python.Commands, _python.Variables);
            _python.RegisterCommand(help);
            _python.RegisterCommand(help, new Func<object, string>(help.Execute), "h");

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

            // Previous command in list
            if (_input.IsKeyPressed(Keys.Up))
            {
                _gui.Screen.FocusedControl = Window.CommandInput;
                Window.CommandInput.Text = PreviousCommand();
                Window.CommandInput.UpdateCaret();
            }

            // Next command in list
            if (_input.IsKeyPressed(Keys.Down))
            {
                _gui.Screen.FocusedControl = Window.CommandInput;
                Window.CommandInput.Text = NextCommand();
                Window.CommandInput.UpdateCaret();
            }

            // Make sure we don't lose focus of text box when caret no longer can move left
            if (_input.IsKeyPressed(Keys.Left))
                _gui.Screen.FocusedControl = Window.CommandInput;
        }

        #endregion

        #region Next/previous command

        private string PreviousCommand()
        {
            if (_python.CommandHistory.Count == 0)
                return Window.CommandInput.Text;

            string s = (_cmdListIndex >= _python.CommandHistory.Count) ? "" : _python.CommandHistory[_cmdListIndex];
            while (_cmdListIndex > 0)
            {
                _cmdListIndex--;
                if (_python.CommandHistory[_cmdListIndex] != s)
                    break;
            }

            _cmdListIndex = (int)MathHelper.Clamp(_cmdListIndex, 0, _python.CommandHistory.Count - 1);

            return _python.CommandHistory[_cmdListIndex];
        }

        private string NextCommand()
        {
            if (_python.CommandHistory.Count == 0)
                return Window.CommandInput.Text;

            _cmdListIndex = (int) MathHelper.Clamp(_cmdListIndex + 1, 0, _python.CommandHistory.Count - 1);

            return _python.CommandHistory[_cmdListIndex];
        }

        #endregion

        #region Events

        /// <summary>
        /// Force focus to text box if user clicks the list.
        /// </summary>
        /// <param name="sender">Screen object</param>
        /// <param name="e">Event args</param>
        private void ScreenOnFocusChanged(object sender, ControlEventArgs e)
        {
            if (e.Control == Window.OutputList)
                _gui.Screen.FocusedControl = Window.CommandInput;
        }

        private void CommandInputOnHandleAutoComplete(object sender, GenericEventArgs<bool> e)
        {
            var control = sender as CommandInputControl;

            if (control != null)
                control.Text = _python.AutoComplete(control.CleanText, e.Value);
        }

        private void CommandInputOnProcessCommand(object sender, OutputEventArgs e)
        {
            var control = sender as CommandInputControl;

            if (control != null)
            {
                _python.Process(e.Output);
                control.Clear();
            }
        }

        private void PythonOnCommandInput(object sender, OutputEventArgs e)
        {
            var output = new ConsoleOutput(e.Output, e.Type);
            Window.Outputs.Add(output);
            Window.OutputList.Items.Add(e.ToString());
            _cmdListIndex = _python.CommandHistory.Count;
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

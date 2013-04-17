using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class ConsoleWindow : WindowControl
    {
        public static readonly int OffsetX = 20;
        public static readonly int OffsetY = -26;

        private readonly PythonInterpreter _python;
        private readonly Screen _screen;
        private ConsoleState _state;

        private DateTime _stateChangeTime;

        #region Properties

        /// <summary>List of all command lines</summary>
        public List<ConsoleOutput> Outputs { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        // Controls
        public CommandInputControl CommandInput { get; private set; }

        public float OpenedY { get { return OffsetY; } }
        public float ClosedY { get { return OffsetY - Height; } }

        #region State Properties

        public bool IsOpen
        {
            get { return _state == ConsoleState.Opened; }
        }

        public bool IsOpening
        {
            get { return _state == ConsoleState.Opening; }
        }

        public bool IsClosed
        {
            get { return _state == ConsoleState.Closed; }
        }

        public bool IsClosing
        {
            get { return _state == ConsoleState.Closing; }
        }

        #endregion

        #endregion

        public ConsoleWindow(Screen screen, int width, int height)
        {
            _state = ConsoleState.Closed;
            _screen = screen;
            _python = new PythonInterpreter();

            EnableDragging = false;

            Width = width;
            Height = height;

            Bounds = new UniRectangle(20, ClosedY, Width, Height);

            Outputs = new List<ConsoleOutput>();

            SetupControls();

            // Subscribe to command events
            _python.CommandReceived += PythonOnCommand;
            _python.CommandProcessed += PythonOnCommand;
        }

        private void SetupControls()
        {
            // Command input prompt
            const int cmdPromptHeight = 26;
            CommandInput = new CommandInputControl(_python)
                {
                    Bounds = new UniRectangle(1, Bounds.Size.Y - cmdPromptHeight - 1, Bounds.Size.X - 2, cmdPromptHeight),
                    Enabled = false
                };
            CommandInput.Clear();

            Children.Add(CommandInput);
        }

        public void Update(GameTime gameTime)
        {
            if (IsOpening)
            {
                Bounds.Top = MathHelper.SmoothStep(Bounds.Top.Offset, OpenedY, ((float)((DateTime.Now - _stateChangeTime).TotalSeconds / 1)));
                if (Bounds.Top == OpenedY)
                {
                    _state = ConsoleState.Opened;
                    CommandInput.Enabled = true;
                    CommandInput.CaretPosition = CommandInput.Text.Length;
                    _screen.FocusedControl = CommandInput;
                }
            }

            if (IsClosing)
            {
                Bounds.Top = MathHelper.SmoothStep(Bounds.Top.Offset, ClosedY, ((float)((DateTime.Now - _stateChangeTime).TotalSeconds / 1)));
                if (Bounds.Top == ClosedY)
                    _state = ConsoleState.Closed;
            }
        }

        #region Toggle Methods

        public void Toggle()
        {
            if (IsOpen || IsOpening)
                Close();
            else if (IsClosed || IsClosing)
                Open();
        }

        private void Open()
        {
            _state = ConsoleState.Opening;
            _stateChangeTime = DateTime.Now;
        }

        private void Close()
        {
            _state = ConsoleState.Closing;
            _stateChangeTime = DateTime.Now;
            CommandInput.Enabled = false;
            _screen.FocusedControl = this;
        }

        #endregion

        #region Command Events

        private void PythonOnCommand(object sender, OutputEventArgs e)
        {
            Outputs.Add(new ConsoleOutput(e.Output, e.Type));
        }

        #endregion
    }
}

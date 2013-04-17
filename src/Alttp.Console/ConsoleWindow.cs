using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class ConsoleWindow : WindowControl
    {
        public static readonly int OffsetX = 20;
        public static readonly int OffsetY = -26;

        private ConsoleState _state;

        private DateTime _stateChangeTime;

        #region Properties

        public int Width { get; private set; }
        public int Height { get; private set; }

        // Controls
        public InputControl CommandInput { get; private set; }

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

        public ConsoleWindow(int width, int height)
        {
            _state = ConsoleState.Closed;

            EnableDragging = false;

            Width = width;
            Height = height;

            Bounds = new UniRectangle(20, ClosedY, Width, Height);

            SetupControls();
        }

        private void SetupControls()
        {
            // Command input prompt
            const int cmdPromptHeight = 26;
            CommandInput = new InputControl()
                {
                    Bounds = new UniRectangle(1, Bounds.Size.Y - cmdPromptHeight - 1, Bounds.Size.X - 2, cmdPromptHeight),
                    Enabled = true,
                    Text = "> "
                };

            Children.Add(CommandInput);
        }

        public void Update(GameTime gameTime)
        {
            if (IsOpening)
            {
                Bounds.Top = MathHelper.SmoothStep(Bounds.Top.Offset, OpenedY, ((float)((DateTime.Now - _stateChangeTime).TotalSeconds / 1)));
                if (Bounds.Top == OpenedY)
                    _state = ConsoleState.Opened;
            }

            if (IsClosing)
            {
                Bounds.Top = MathHelper.SmoothStep(Bounds.Top.Offset, ClosedY, ((float)((DateTime.Now - _stateChangeTime).TotalSeconds / 1)));
                if (Bounds.Top == ClosedY)
                    _state = ConsoleState.Closed;
            }
        }

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
        }
    }
}

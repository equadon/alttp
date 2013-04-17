using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class ConsoleWindow : WindowControl
    {
        private ConsoleState _state;

        public int Width { get; private set; }
        public int Height { get; private set; }

        public bool IsOpen { get { return _state == ConsoleState.Opened; } }
        public bool IsOpening { get { return _state == ConsoleState.Opening; } }
        public bool IsClosed { get { return _state == ConsoleState.Closed; } }
        public bool IsClosing { get { return _state == ConsoleState.Closing; } }

        public ConsoleWindow(int width, int height)
        {
            _state = ConsoleState.Closed;

            Width = width;
            Height = height;
        }

        public void Toggle()
        {
            if (IsOpening || IsClosing)
                return;

            if (IsOpen)
                Close();
            else if (IsClosed)
                Open();
        }

        private void Open()
        {
            _state = ConsoleState.Opening;
        }

        private void Close()
        {
            _state = ConsoleState.Closing;
        }
    }
}

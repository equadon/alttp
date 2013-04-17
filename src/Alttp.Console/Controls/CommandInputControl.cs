using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class CommandInputControl : InputControl
    {
        private readonly PythonInterpreter _processor;

        public CommandInputControl(PythonInterpreter processor)
        {
            _processor = processor;
        }

        protected override bool OnKeyPressed(Keys key)
        {
            return (key == Keys.Enter || base.OnKeyPressed(key));
        }

        protected override void OnKeyReleased(Keys key)
        {
            if (key == Keys.Enter)
                ProcessCommand(Text);
        }

        public void ProcessCommand(string input)
        {
            if (input != null || input != "")
            {
                _processor.Process(input);
                Clear();
            }
        }

        public void Clear()
        {
            Text = ">>> ";
        }
    }
}

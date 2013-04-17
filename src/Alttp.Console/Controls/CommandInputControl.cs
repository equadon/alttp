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
        private CommandProcessor _processor;

        public CommandInputControl(CommandProcessor processor)
        {
            _processor = processor;
        }

        protected override bool OnKeyPressed(Keys key)
        {
            if (key == Keys.Enter)
                ProcessCommand(Text);

            return base.OnKeyPressed(key);
        }

        protected override void OnKeyReleased(Keys key)
        {
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

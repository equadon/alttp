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
        public CommandInputControl()
        {
            
        }

        protected override bool OnKeyPressed(Keys key)
        {
            if (key == Keys.Enter)
                Clear();

            return base.OnKeyPressed(key);
        }

        protected override void OnKeyReleased(Keys key)
        {
        }

        public void Clear()
        {
            Text = "> ";
        }
    }
}

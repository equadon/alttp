using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Events;
using Alttp.Core.Input;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class CommandInputControl : InputControl
    {
        public event CommandEventHandler ProcessCommand;
        public event EventHandler<GenericEventArgs<bool>>  HandleAutoComplete;

        public static readonly string Prompt = ">>> ";

        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = Prompt + CleanPrompt(value);
                UpdateCaret();
            }
        }

        public string CleanText
        {
            get { return CleanPrompt(Text); }
        }

        protected override bool OnKeyPressed(Keys key)
        {
            switch (key)
            {
                case Keys.Enter:
                    return true;
                case Keys.Tab:
                    return false;
                case Keys.Left:
                    return (CaretPosition > Prompt.Length) && base.OnKeyPressed(key);
                case Keys.Back:
                    if (Text == Prompt)
                        return false;
                    goto default;
                default:
                    return base.OnKeyPressed(key);
            }
        }

        protected override void OnKeyReleased(Keys key)
        {
            switch (key)
            {
                case Keys.Enter:
                    OnProcessCommand(Text);
                    break;
            }
        }

        protected override void OnCharacterEntered(char c)
        {
            // Do not enter these chars
            if (c == (char)Keys.Escape ||
                c == '`' ||
                c == Convert.ToChar(167)) // § - console
                return;

            // Handle tab completion
            if (c == (char)Keys.Tab)
            {
                // If shift is pressed auto complete in the opposite direction
                OnHandleAutoComplete();
                return;
            }

            base.OnCharacterEntered(c);
        }

        private void OnHandleAutoComplete()
        {
            bool moveBackwards = Keyboard.GetState().IsKeyDown(Keys.LeftShift) ||
                                 Keyboard.GetState().IsKeyDown(Keys.RightShift);

            if (HandleAutoComplete != null)
                HandleAutoComplete(this, new GenericEventArgs<bool>(moveBackwards));
        }

        public void OnProcessCommand(string input)
        {
            if (ProcessCommand != null && (input != null || input != ""))
                ProcessCommand(this, new OutputEventArgs(input, ConsoleOutputType.Command));
        }

        private string CleanPrompt(string str)
        {
            return (str.StartsWith(Prompt)) ? str.Substring(Prompt.Length) : str;
        }

        public void Clear()
        {
            Text = Prompt;
        }

        public void UpdateCaret()
        {
            CaretPosition = Text.Length;
        }
    }
}

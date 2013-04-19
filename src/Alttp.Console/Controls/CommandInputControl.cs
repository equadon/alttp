using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Input;
using Microsoft.Xna.Framework.Input;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Console
{
    public class CommandInputControl : InputControl
    {
        public static readonly string Prompt = ">>> ";

        private readonly PythonInterpreter _processor;

        public new string Text
        {
            get { return base.Text; }
            set
            {
                base.Text = Prompt + Clean(value);
                UpdateCaret();
            }
        }

        public CommandInputControl(PythonInterpreter processor)
        {
            _processor = processor;
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
                    ProcessCommand(Text);
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
                Text = _processor.AutoComplete(Clean(Text), Keyboard.GetState().IsKeyDown(Keys.LeftShift) || Keyboard.GetState().IsKeyDown(Keys.RightShift));
                return;
            }

            base.OnCharacterEntered(c);
        }

        public void ProcessCommand(string input)
        {
            if (input != null || input != "")
            {
                _processor.Process(input);
                Clear();
            }
        }

        private string Clean(string str)
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

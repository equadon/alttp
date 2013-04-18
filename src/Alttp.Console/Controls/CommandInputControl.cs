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
        public static readonly string Prompt = ">>> ";

        private readonly PythonInterpreter _processor;

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

        protected override void OnCharacterEntered(char character)
        {
            base.OnCharacterEntered(character);

            if (character == (char)Keys.Escape ||
                character == '`')
            {
                Text = Text.Substring(0, Text.Length - 1);
            }
        }

        public void ProcessCommand(string input)
        {
            if (input != null || input != "")
            {
                _processor.Process(input);
                Clear();
            }
        }

        public void SetText(string text)
        {
            Text = Prompt + text;
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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;

namespace Alttp.Engine.Input
{
    public enum MouseButton
    {
        Left,
        Middle,
        Right
    }

    public class InputManager : Nuclex.Input.InputManager
    {
        #region Properties

        public KeyboardState LastKeyboardState { get; private set; }
        public KeyboardState KeyboardState { get; private set; }

        public MouseState LastMouseState { get; private set; }
        public MouseState MouseState { get; private set; }

        public int LastMouseWheelValue { get; private set; }
        public int MouseWheelValue { get; private set; }

        public int MouseWheelValueDiff
        {
            get { return MouseWheelValue - LastMouseWheelValue; }
        }
        
        public Vector2 MousePos
        {
            get { return new Vector2(GetMouse().GetState().X, GetMouse().GetState().Y); }
        }

        #endregion

        #region Keyboard Helper Methods

        public bool IsKeyPressed(Keys key)
        {
            return LastKeyboardState.IsKeyUp(key) &&
                   KeyboardState.IsKeyDown(key);
        }

        public bool IsKeyReleased(Keys key)
        {
            return LastKeyboardState.IsKeyDown(key) &&
                   KeyboardState.IsKeyUp(key);
        }

        public bool IsKeyUp(Keys key)
        {
            return KeyboardState.IsKeyUp(key);
        }

        public bool IsKeyDown(Keys key)
        {
            return KeyboardState.IsKeyDown(key);
        }

        #endregion

        #region Mouse Helper Methods

        public bool IsMouseButtonPressed(MouseButton button)
        {
            return GetButtonState(LastMouseState, button) == ButtonState.Released &&
                   GetButtonState(MouseState, button) == ButtonState.Pressed;
        }

        public bool IsMouseButtonReleased(MouseButton button)
        {
            return GetButtonState(LastMouseState, button) == ButtonState.Pressed &&
                   GetButtonState(MouseState, button) == ButtonState.Released;
        }

        public bool IsMouseButtonDown(MouseButton button)
        {
            return GetButtonState(MouseState, button) == ButtonState.Pressed;
        }

        public bool IsMouseButtonUp(MouseButton button)
        {
            return GetButtonState(MouseState, button) == ButtonState.Released;
        }

        public bool MouseWheelValueChanged()
        {
            return MouseWheelValue != LastMouseWheelValue;
        }

        private ButtonState GetButtonState(MouseState state, MouseButton button)
        {
            switch (button)
            {
                case MouseButton.Left:
                    return state.LeftButton;
                case MouseButton.Middle:
                    return state.MiddleButton;
                case MouseButton.Right:
                    return state.RightButton;
                default:
                    throw new Exception("Unknown mouse button: " + button);
            }
        }

        #endregion

        public void UpdateStates()
        {
            LastKeyboardState = KeyboardState;
            KeyboardState = GetKeyboard().GetState();

            LastMouseState = MouseState;
            MouseState = GetMouse().GetState();

            LastMouseWheelValue = MouseWheelValue;
            MouseWheelValue = MouseState.ScrollWheelValue;
        }
    }
}

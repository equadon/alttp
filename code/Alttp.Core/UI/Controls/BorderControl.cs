﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls;

namespace Alttp.UI.Controls
{
    /// <summary>
    /// BorderControl draws a border around the specified region.
    /// </summary>
    public class BorderControl : ImageControl
    {
        public int BorderSize { get; private set; }
        public Color BorderColor { get; private set; }

        public BorderControl(Texture2D texture, int borderSize, Color borderColor)
            : base(texture)
        {
            BorderSize = borderSize;
            BorderColor = borderColor;
        }
    }
}

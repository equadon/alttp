using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Alttp.Core.UI.Controls
{
    /// <summary>
    /// BorderControl draws a border around the specified region.
    /// </summary>
    public class BorderControl : ImageControl
    {
        public int BorderSize { get; set; }
        public Color BorderColor { get; set; }

        public BorderControl(Texture2D texture, int borderSize, Color borderColor)
            : base(texture)
        {
            BorderSize = borderSize;
            BorderColor = borderColor;
        }
    }
}

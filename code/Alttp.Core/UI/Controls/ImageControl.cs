using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface.Controls;

namespace Alttp.Core.UI.Controls
{
    public class ImageControl : Control
    {
        public Texture2D Texture { get; private set; }

        public ImageControl(Texture2D texture)
        {
            Texture = texture;
        }
    }
}

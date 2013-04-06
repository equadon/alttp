using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Alttp.UI.Controls
{
    public class FlatImageControlRenderer : IFlatControlRenderer<ImageControl>
    {
        public void Render(ImageControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            graphics.DrawElement("image", controlBounds, control.Texture, Color.White);
        }
    }
}

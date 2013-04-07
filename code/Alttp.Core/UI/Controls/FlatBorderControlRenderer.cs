using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;

namespace Alttp.Core.UI.Controls
{
    public class FlatBorderControlRenderer : IFlatControlRenderer<BorderControl>
    {
        public void Render(BorderControl control, IFlatGuiGraphics graphics)
        {
            RectangleF controlBounds = control.GetAbsoluteBounds();

            var minimap = new Rectangle((int) controlBounds.X, (int) controlBounds.Y, (int) controlBounds.Width, (int) controlBounds.Height);

            // Top
            graphics.DrawElement("image", new RectangleF(minimap.X, minimap.Y, minimap.Width, control.BorderSize), control.Texture, control.BorderColor);
            
            // Bottom
            graphics.DrawElement("image", new RectangleF(minimap.X, minimap.Y + minimap.Height, minimap.Width, control.BorderSize), control.Texture, control.BorderColor);

            // Left
            graphics.DrawElement("image", new RectangleF(minimap.X, minimap.Y, control.BorderSize, minimap.Height), control.Texture, control.BorderColor);

            // Right
            graphics.DrawElement("image", new RectangleF(minimap.X + minimap.Width, minimap.Y, control.BorderSize, minimap.Height + control.BorderSize), control.Texture, control.BorderColor);
        }
    }
}

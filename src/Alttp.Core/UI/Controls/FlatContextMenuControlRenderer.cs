using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using Nuclex.UserInterface.Visuals.Flat.Renderers;

namespace Alttp.Core.UI.Controls
{
    public class FlatContextMenuControlRenderer : IFlatControlRenderer<ContextMenuControl>
    {
        public void Render(ContextMenuControl control, IFlatGuiGraphics graphics)
        {
            RectangleF bounds = control.GetAbsoluteBounds();

            graphics.DrawElement("context", bounds);
            graphics.DrawString("context", bounds, control.Name);
        }
    }
}
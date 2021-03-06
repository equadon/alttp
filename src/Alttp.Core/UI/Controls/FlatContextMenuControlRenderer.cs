﻿using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;
using Nuclex.UserInterface.Visuals.Flat;
using Nuclex.UserInterface.Visuals.Flat.Renderers;

namespace Alttp.Core.UI.Controls
{
    public class FlatContextMenuControlRenderer : IFlatControlRenderer<ContextMenuControl>, IListRowLocator
    {
        private IFlatGuiGraphics _graphics;

        public void Render(ContextMenuControl control, IFlatGuiGraphics graphics)
        {
            _graphics = graphics;

            RectangleF bounds = control.GetAbsoluteBounds();

            graphics.DrawElement("context", bounds);

            string text = control.Name;
            graphics.DrawString("context", bounds, text);

            var itemBounds = new RectangleF(bounds.X + 3, bounds.Y + 30, bounds.Width - 12, ContextMenuControl.ItemHeight);

            foreach (var item in control.Items)
            {
                if (control.SelectedItems.Contains(control.Items.IndexOf(item)))
                    graphics.DrawElement("context.item.highlighted", itemBounds);
                graphics.DrawString("context.item", itemBounds, item);
                itemBounds.Y += itemBounds.Height;
            }

            control.ListRowLocator = this;
        }

        #region Implementation of IListRowLocator

        public int GetRow(RectangleF bounds, float thumbPosition, int itemCount, float y)
        {
            // Calculate the item that should be under the requested Y coordinate
            float res = (y - 30) / GetRowHeight(bounds);
            if (res < 0)
                return -1;
            return (int) res;
        }

        public float GetRowHeight(RectangleF bounds)
        {
            return ContextMenuControl.ItemHeight;
        }

        #endregion
    }
}
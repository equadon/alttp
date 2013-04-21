using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI.Controls
{
    public class ContextMenuControl : ListControl
    {
        public ContextMenuControl()
        {
            Children.RemoveAt(0);
        }

        /// <summary>
        /// Select item beneath the mouse, if any
        /// </summary>
        /// <param name="x">Mouse X position</param>
        /// <param name="y">Mouse Y position</param>
        protected override void OnMouseMoved(float x, float y)
        {
            if (ListRowLocator != null)
            {
                int row = ListRowLocator.GetRow(GetAbsoluteBounds(), 0, Items.Count, y);
                if (row >= 0 && row < Items.Count)
                {
                    SelectedItems.Clear();
                    SelectedItems.Add(row);
                }
                else
                {
                    SelectedItems.Clear();
                }
            }
        }

        /// <summary>
        /// Mouse left control, clear all selected items.
        /// </summary>
        protected override void OnMouseLeft()
        {
            SelectedItems.Clear();
        }
    }
}

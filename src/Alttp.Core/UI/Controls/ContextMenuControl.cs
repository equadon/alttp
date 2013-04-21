using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Nuclex.Input;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI.Controls
{
    public class ContextMenuControl : ListControl
    {
        public static readonly int ItemHeight = 22;

        private float _mouseY;

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
            _mouseY = y;

            if (ListRowLocator != null)
            {
                int row = ListRowLocator.GetRow(GetAbsoluteBounds(), 0, Items.Count, _mouseY);
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

        protected override void OnMousePressed(Nuclex.Input.MouseButtons button)
        {
            if (button != MouseButtons.Left)
                return;

            if (ListRowLocator != null)
            {
                int row = ListRowLocator.GetRow(GetAbsoluteBounds(), 0, Items.Count, _mouseY);
                if (row >= 0 && row < Items.Count)
                {
                    OnRowClicked(row);
                }
            }
        }
    }
}

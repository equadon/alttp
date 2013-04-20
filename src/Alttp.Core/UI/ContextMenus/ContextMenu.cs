using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.UI.Controls;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI.ContextMenus
{
    public class ContextMenu : ContextMenuControl
    {
        public Vector2 Position
        {
            get { return new Vector2(Bounds.Top.Offset, Bounds.Left.Offset); }
            set { Bounds.Location = new UniVector(value.X, value.Y); }
        }

        public ContextMenu()
        {
            SelectionMode = ListSelectionMode.Single;
        }
    }
}

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
    }
}

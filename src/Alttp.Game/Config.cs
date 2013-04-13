using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;

namespace Alttp
{
    static class Config
    {
        public static readonly string WindowTitle = "Alttp Remake - Alpha Build";

        public static readonly bool FullScreen = false;

        public static readonly int ScreenWidth = 1440;
        public static readonly int ScreenHeight = 900;

        public static readonly bool VsyncEnabled = true;

        public static readonly bool DisplayCursor = true;
        public static readonly bool AllowWindowResizing = false;

        // Colors
        public static readonly Color GridColor = new Color(50, 50, 50, 150);

        // Minimap
        public static readonly int MinimapBorderSize = 2;
        public static readonly Color MinimapViewportColor = Color.LightGray;
    }
}

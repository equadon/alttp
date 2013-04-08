using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;

namespace Alttp.Worlds
{
    public class Region
    {
        public string Name { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Point[] PolygonPoint { get; private set; }

        public Region(string name, Rectangle bounds, Point[] polygonPoints)
        {
            if (polygonPoints == null && (bounds.Width == 0 || bounds.Height == 0))
                throw new Exception("Width/height cannot be zero if no polygon points are present.");

            Name = name;
            Bounds = bounds;
            PolygonPoint = polygonPoints;
        }

        /// <summary>
        /// Creates a Region object from a Tiled map object.
        /// </summary>
        /// <param name="obj">Map object to load the data from</param>
        public static Region FromMapObject(MapObject obj)
        {
            if (obj.Polygon != null)
                return new Region(obj.Name, obj.Bounds, obj.Polygon.Points);

            return new Region(obj.Name, obj.Bounds, null);
        }
    }
}

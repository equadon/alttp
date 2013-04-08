using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class Region
    {
        public string Name { get; private set; }
        public Rectangle Bounds { get; private set; }
        public Polygon Polygon { get; private set; }

        public Region(string name, Rectangle bounds, Polygon polygon)
        {
            if (polygon == null && (bounds.Width == 0 || bounds.Height == 0))
                throw new Exception("Width/height cannot be zero if no polygon points are present.");

            Name = name;
            Bounds = bounds;
            Polygon = polygon;
        }

        /// <summary>
        /// Returns true if the position is inside the region borders.
        /// </summary>
        /// <param name="position">World position</param>
        /// <returns></returns>
        public bool Contains(Vector2 position)
        {
            return (Polygon == null) ? Bounds.Contains((int)position.X, (int)position.Y)
                                     : Polygon.Contains(position);
        }

        public void DrawBorders(ISpriteBatch batch, Texture2D texture, Camera camera)
        {
            if (Polygon == null)
                return;

            for (int i = 0; i < Polygon.Lines.Length; i++)
                DrawLine(batch, texture, Polygon.Lines[i], camera);
        }

        private void DrawLine(ISpriteBatch batch, Texture2D texture, Line line, Camera camera)
        {
            Vector2 position = camera.WorldToScreen(line.Start); //Map.Translate(line.Start, region);
            batch.Draw(texture, position, new Rectangle?(), Color.White, line.Angle, Vector2.Zero, new Vector2(line.Length * camera.InvZoom, 2 * camera.InvZoom), SpriteEffects.None, 0);
        }
    }
}

using System;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;

namespace Alttp.Core.World
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

            // Move polygon points at x=0 or y=0 to -1 so 0,0 is Contained within the region
            UpdatePolygon();
        }

        private void UpdatePolygon()
        {
            for (int i = 0; i < Polygon.Points.Length; i++)
            {
                var point = Polygon.Points[i];

                if (point.X == 0)
                    Polygon.Points[i] = new Point(-1, point.Y);
                if (point.Y == 0)
                    Polygon.Points[i] = new Point(point.X, -1);
            }

            for (int i = 0; i < Polygon.Lines.Length; i++)
            {
                var line = Polygon.Lines[i];

                if (line.Start.X == 0)
                    line.Start = new Vector2(-1, line.Start.Y);
                if (line.Start.Y == 0)
                    line.Start = new Vector2(line.Start.X, -1);

                if (line.End.X == 0)
                    line.End = new Vector2(-1, line.End.Y);
                if (line.End.Y == 0)
                    line.End = new Vector2(line.End.X, -1);

                Polygon.Lines[i] = line;
            }
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

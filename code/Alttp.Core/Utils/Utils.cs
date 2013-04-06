using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;

namespace Alttp.Engine.Utils
{
    public static class Utils
    {
        public static Rectangle CastRectangleF(RectangleF rect)
        {
            return new Rectangle((int)rect.X, (int)rect.Y, (int)rect.Width, (int)rect.Height);
        }

        public static void DrawBorder(ISpriteBatch spriteBatch, Texture2D blankTexture, Rectangle content, int borderWidth, Color color, bool borderOutsideContent = true)
        {
            int left = (borderOutsideContent) ? content.Left - borderWidth : content.Left;
            int right = (borderOutsideContent) ? content.Right : content.Right - borderWidth;
            int top = (borderOutsideContent) ? content.Top - borderWidth : content.Top;
            int bottom = (borderOutsideContent) ? content.Bottom : content.Bottom - borderWidth;

            int width = (borderOutsideContent) ? content.Width + borderWidth * 2 : content.Width;
            int height = (borderOutsideContent) ? content.Height + borderWidth * 2 : content.Height;

            // Top border
            spriteBatch.Draw(blankTexture, new Rectangle(left, top, width, borderWidth), color);

            // Bottom border
            spriteBatch.Draw(blankTexture, new Rectangle(left, bottom, width, borderWidth), color);

            // Left border
            spriteBatch.Draw(blankTexture, new Rectangle(left, top, borderWidth, height), color);

            // Right border
            spriteBatch.Draw(blankTexture, new Rectangle(right, top, borderWidth, height), color);
        }
    }
}

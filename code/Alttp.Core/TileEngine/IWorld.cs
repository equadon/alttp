using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;

namespace Alttp.Engine
{
    public interface IWorld
    {
        int TileWidth { get; }
        int TileHeight { get; }
        int Width { get; }
        int Height { get; }
        int WidthInPixels { get; }
        int HeightInPixels { get; }
        void Draw(GameTime gameTime, ISpriteBatch spriteBatch, Camera camera);
    }
}
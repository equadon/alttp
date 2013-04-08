using Alttp.Core.Animation;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public interface IWorld
    {
        bool RenderCollisionTiles { get; set; }

        int TileWidth { get; }
        int TileHeight { get; }
        int Width { get; }
        int Height { get; }
        int WidthInPixels { get; }
        int HeightInPixels { get; }

        AnimationsDict WorldObjectAnimations { get; }

        Region[] Regions { get; }

        Tile[] SourceTiles { get; }
        Tileset[] Tilesets { get; }

        TileLayer BackgroundTiles { get; }
        TileLayer ForegroundTiles { get; }
        TileLayer CollisionTiles { get; }

        Region GetRegion(Vector2 position);

        void Draw(GameTime gameTime, ISpriteBatch spriteBatch, Camera camera);
    }
}
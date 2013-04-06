using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.Ninject.Xna;

namespace Alttp.Engine
{
    public class World : IWorld
    {
        private readonly Map _map;

        public int TileWidth { get { return _map.TileWidth; } }
        public int TileHeight { get { return _map.TileWidth; } }

        public int Width { get { return _map.Width; } }
        public int Height { get { return _map.Height; } }

        public int WidthInPixels { get { return Width * TileWidth; } }
        public int HeightInPixels { get { return Height * TileHeight; } }

        public World(IContentManager content, string mapResource)
            : this(content.Load<Map>(mapResource))
        {
        }

        public World(Map map)
        {
            _map = map;
        }

        public void Draw(GameTime gameTime, ISpriteBatch spriteBatch, Camera camera)
        {
            DrawLayer(gameTime, spriteBatch, 0, camera, 0);
        }

        public void DrawLayer(GameTime gameTime, ISpriteBatch spriteBatch, int layerId, Camera camera, Single layerDepth)
        {
            Rectangle region = camera.GetTilesRegion();

            for (int y = region.Top; y <= region.Bottom; y++)
            {
                for (int x = region.Left; x <= region.Right; x++)
                {
                    // check that we aren't going outside the map, and that there is a tile at this location
                    if (x >= 0 && x < _map.TileLayers[layerId].Tiles.Length && y >= 0 && y < _map.TileLayers[layerId].Tiles[x].Length
                        && _map.TileLayers[layerId].Tiles[x][y] != null)
                    {
                        spriteBatch.Draw(
                            // the texture (image) of the tile sheet is mapped by
                            // Tile.SourceID -> TileLayers.TilesetID -> Map.Tileset.Texture
                            _map.Tilesets[_map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].TilesetID].Texture,

                            // destination for the tile
                            new Rectangle(x * _map.TileWidth, y * _map.TileHeight, _map.TileWidth, _map.TileHeight),

                            // source of the tile in the tilesheet
                            _map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].Source,

                            // layers can have an opacity value, this property is Color.White at the opacity of the layer
                            _map.TileLayers[layerId].OpacityColor,

                            // tile rotation value
                            _map.TileLayers[layerId].Tiles[x][y].Rotation,

                            // origin of the tile, this is always the center of the tile
                            _map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].Origin,

                            // tile horizontal or vertical flipping value
                            _map.TileLayers[layerId].Tiles[x][y].Effects,

                            // depth for SpriteSortMode
                            layerDepth);
                    }
                }
            }
        }

//        public void DrawBufferedLayer(GameTime gameTime, SpriteBatch spriteBatch, SpriteBatch tmpSpriteBatch, GraphicsDevice graphics, Int32 layerId, Rectangle region, Single layerDepth, RenderTarget2D tilesTexture)
//        {
//            Int32 txMin = region.Left / _map.TileWidth;
//            Int32 txMax = (Int32)Math.Ceiling(region.Right / (float)_map.TileWidth);
//
//            Int32 tyMin = region.Top / _map.TileHeight;
//            Int32 tyMax = 1 + (Int32)Math.Ceiling(region.Bottom / (float)_map.TileHeight);
//
//            // Render tiles onto a texture to prevent black vertical lines
//            graphics.SetRenderTarget(tilesTexture);
//            graphics.Clear(Color.Black);
//
//            tmpSpriteBatch.Begin();
//
//            for (int y = tyMin; y <= tyMax; y++)
//            {
//                for (int x = txMin; x <= txMax; x++)
//                {
//                    // check that we aren't going outside the map, and that there is a tile at this location
//                    if (x >= 0 && x < _map.TileLayers[layerId].Tiles.Length && y >= 0 && y < _map.TileLayers[layerId].Tiles[x].Length
//                        && _map.TileLayers[layerId].Tiles[x][y] != null)
//                    {
//                        tmpSpriteBatch.Draw(
//                            // the texture (image) of the tile sheet is mapped by
//                            // Tile.SourceID -> TileLayers.TilesetID -> Map.Tileset.Texture
//                            _map.Tilesets[_map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].TilesetID].Texture,
//
//                            // destination for the tile
//                            new Rectangle(x * _map.TileWidth, y * _map.TileHeight, _map.TileWidth, _map.TileHeight),
//
//                            // source of the tile in the tilesheet
//                            _map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].Source,
//
//                            // layers can have an opacity value, this property is Color.White at the opacity of the layer
//                            _map.TileLayers[layerId].OpacityColor,
//
//                            // tile rotation value
//                            _map.TileLayers[layerId].Tiles[x][y].Rotation,
//
//                            // origin of the tile, this is always the center of the tile
//                            _map.SourceTiles[_map.TileLayers[layerId].Tiles[x][y].SourceID].Origin,
//
//                            // tile horizontal or vertical flipping value
//                            _map.TileLayers[layerId].Tiles[x][y].Effects,
//
//                            // depth for SpriteSortMode
//                            layerDepth);
//                    }
//                }
//            }
//
//            tmpSpriteBatch.End();
//            graphics.SetRenderTarget(null);
//
//            // Draw the generated texture to screen
//            spriteBatch.Draw(tilesTexture, Vector2.Zero, null, Color.White);
//        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class World : IWorld
    {
        public bool RenderCollisionTiles { get; set; }

        public Region[] Regions { get; private set; }

        public int TileWidth { get; private set; }
        public int TileHeight { get; private set; }

        public int Width { get; private set; }
        public int Height { get; private set; }

        public int WidthInPixels { get { return Width * TileWidth; } }
        public int HeightInPixels { get { return Height * TileHeight; } }

        public Tile[] SourceTiles { get; private set; }
        public Tileset[] Tilesets { get; private set; }

        // Layers
        public TileLayer BackgroundTiles { get; private set; }
        public TileLayer ForegroundTiles { get; private set; }
        public TileLayer CollisionTiles { get; private set; }

        public World(IContentManager content, string mapResource)
            : this(content.Load<Map>(mapResource))
        {
        }

        public World(Map map)
        {
            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            Width = map.Width;
            Height = map.Height;

            // Layers
            LoadTileLayers(map);

            // Source tiles & tilesets
            SourceTiles = map.SourceTiles;
            Tilesets = map.Tilesets;

            // Load objects
            ObjectLayer regions = null;
            ObjectLayer bushes = null;

            // Object layers
            foreach (var layer in map.ObjectLayers)
            {
                if (layer.Name == "Regions")
                    regions = layer;
                else if (layer.Name == "Bushes")
                    bushes = layer;
            }

            Regions = LoadRegions(regions);
        }

        private void LoadTileLayers(Map map)
        {
            foreach (var layer in map.TileLayers)
            {
                if (layer.Name == "Background")
                    BackgroundTiles = layer;
                else if (layer.Name == "Foreground")
                    ForegroundTiles = layer;
                else if (layer.Name == "Collision")
                    CollisionTiles = layer;
            }

            if (BackgroundTiles == null || ForegroundTiles == null || CollisionTiles == null)
                throw new Exception("Failed to load map layers");
        }

        private Region[] LoadRegions(ObjectLayer layer)
        {
            if (layer == null)
                return new Region[0];

            var regions = new List<Region>();

            foreach (var obj in layer.MapObjects)
                regions.Add(new Region(obj.Name, obj.Bounds, obj.Polygon));

            return regions.ToArray();
        }

        /// <summary>
        /// Returns the region the provided position is in
        /// </summary>
        /// <param name="position">Position in world</param>
        /// <returns></returns>
        public Region GetRegion(Vector2 position)
        {
            return Regions.FirstOrDefault(region => region.Contains(position));
        }

        public void Draw(GameTime gameTime, ISpriteBatch spriteBatch, Camera camera)
        {
            DrawLayer(gameTime, spriteBatch, BackgroundTiles, camera, 0);

            if (RenderCollisionTiles)
                DrawLayer(gameTime, spriteBatch, CollisionTiles, camera, 0);
        }

        public void DrawLayer(GameTime gameTime, ISpriteBatch spriteBatch, TileLayer layer, Camera camera, Single layerDepth)
        {
            Rectangle region = camera.GetTilesRegion();

            for (int y = region.Top; y <= region.Bottom; y++)
            {
                for (int x = region.Left; x <= region.Right; x++)
                {
                    // check that we aren't going outside the map, and that there is a tile at this location
                    if (x >= 0 && x < layer.Tiles.Length && y >= 0 && y < layer.Tiles[x].Length
                        && layer.Tiles[x][y] != null)
                    {
                        spriteBatch.Draw(
                            // the texture (image) of the tile sheet is mapped by
                            // Tile.SourceID -> TileLayers.TilesetID -> Map.Tileset.Texture
                            Tilesets[SourceTiles[layer.Tiles[x][y].SourceID].TilesetID].Texture,

                            // destination for the tile
                            new Rectangle(x * TileWidth, y * TileHeight, TileWidth, TileHeight),

                            // source of the tile in the tilesheet
                            SourceTiles[layer.Tiles[x][y].SourceID].Source,

                            // layers can have an opacity value, this property is Color.White at the opacity of the layer
                            layer.OpacityColor,

                            // tile rotation value
                            layer.Tiles[x][y].Rotation,

                            // origin of the tile, this is always the center of the tile
                            SourceTiles[layer.Tiles[x][y].SourceID].Origin,

                            // tile horizontal or vertical flipping value
                            layer.Tiles[x][y].Effects,

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

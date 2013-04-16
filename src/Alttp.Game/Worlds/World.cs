using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Alttp.Core.Animation;
using Alttp.GameObjects;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class World : IWorld
    {
        protected ILogger Log { get; set; }

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

        // Objects
        public AnimationsDict WorldObjectAnimations { get; private set; }

        public List<GameObject>[,] Objects { get; private set; }

        public World(IContentManager content, string mapResource, string worldObjectsResource, ILogger logger)
            : this(content.Load<Map>(mapResource), content.Load<AnimationsDict>(worldObjectsResource), logger)
        {
        }

        public World(Map map, AnimationsDict worldObjectAnimations, ILogger logger)
        {
            Log = logger;

            TileWidth = map.TileWidth;
            TileHeight = map.TileHeight;

            Width = map.Width;
            Height = map.Height;

            Log.Info("Loading \"" + GetType().Name + "\"...");

            Objects = new List<GameObject>[Width, Height];
            WorldObjectAnimations = worldObjectAnimations;

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

            LoadRegions(regions);

            LoadBushes(bushes);

            Log.Info("Successfully loaded \"" + GetType().Name + "\".");
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

            Log.Debug(" + 3 tile layers.");
        }

        private void LoadRegions(ObjectLayer layer)
        {
            if (layer == null)
            {
                Regions = new Region[0];
                return;
            }

            var regions = new List<Region>();

            foreach (var obj in layer.MapObjects)
                regions.Add(new Region(obj.Name, obj.Bounds, obj.Polygon));

            Regions = regions.ToArray();

            Log.Debug(" + {0} region(s).", Regions.Length);
        }

        /// <summary>
        /// Load all the bush objects into the world.
        /// </summary>
        /// <param name="bushes"></param>
        private void LoadBushes(ObjectLayer bushes)
        {
            int count = 0;

            foreach (var mapObject in bushes.MapObjects)
            {
                int x = mapObject.Bounds.X,
                    y = mapObject.Bounds.Y;
                int tileX = x / TileWidth,
                    tileY = y / TileHeight;

                var position = new Vector2(x - mapObject.Bounds.Width / 2f, y - mapObject.Bounds.Height / 2f);

                var bush = new Bush(Log, position, WorldObjectAnimations, "/Bush/Green/Idle");

                if (Objects[tileX, tileY] == null)
                    Objects[tileX, tileY] = new List<GameObject>();
                Objects[tileX, tileY].Add(bush);

                count++;
            }

            Log.Debug(" + {0} bushes.", count);
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
            DrawLayer(gameTime, spriteBatch, BackgroundTiles, camera);

            DrawObjects(spriteBatch, camera);

            if (RenderCollisionTiles)
                DrawLayer(gameTime, spriteBatch, CollisionTiles, camera);
        }

        public void DrawLayer(GameTime gameTime, ISpriteBatch spriteBatch, TileLayer layer, Camera camera)
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
                            0);
                    }
                }
            }
        }

        /// <summary>
        /// Draw objects inside camera viewport.
        /// </summary>
        /// <param name="batch"></param>
        /// <param name="camera"></param>
        private void DrawObjects(ISpriteBatch batch, Camera camera)
        {
            Rectangle region = camera.GetTilesRegion();

            for (int y = region.Top; y <= region.Bottom; y++)
            {
                for (int x = region.Left; x <= region.Right; x++)
                {
                    var objects = Objects[x, y];
                    if (objects != null)
                        foreach (var obj in objects)
                            obj.Draw(batch);
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

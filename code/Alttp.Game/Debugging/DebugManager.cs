using System;
using Alttp.Core;
using Alttp.Core.UI;
using Alttp.Core.UI.Controls;
using Alttp.Debugging.Overlays;
using Alttp.GameObjects;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Nuclex.Input;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using InputManager = Alttp.Core.Input.InputManager;

namespace Alttp.Debugging
{
    public class DebugManager : DrawableGameComponent
    {
        private readonly GuiManager _gui;
        private readonly IContentManager _content;
        private readonly ISpriteBatch _batch;
        private readonly InputManager _input;
        private readonly WorldManager _world;

        public int Frame { get; private set; }
        public GameTime ElapsedTime { get; private set; }

        public GameObject[] SelectedGameObjects { get; private set; }

        public Rectangle SelectionBounds { get; private set; }

        public Texture2D BlankTexture { get; private set; }
        public Texture2D MinimapTexture { get; private set; }

        /// <summary>Render tile grid.</summary>
        public bool RenderGrid { get; private set; }

        /// <summary>Render debug overlay.</summary>
        public bool RenderOverlay
        {
            get { return _gui.Visible; }
            set { _gui.Visible = value; }
        }

        // Overlays
        public CameraOverlay CameraOverlay { get; private set; }
        public GameInfoOverlay GameInfoOverlay { get; private set; }
        public GameObjectOverlay GameObjectOverlay { get; private set; }
        public MinimapOverlay MinimapOverlay { get; private set; }

        public DebugManager(Game game, GuiManager gui, IContentManager content, ISpriteBatch batch, InputManager input, WorldManager world)
            : base(game)
        {
            _gui = gui;
            _content = content;
            _batch = batch;
            _input = input;
            _world = world;

            RenderOverlay = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            _gui.Visualizer = FlatGuiVisualizer.FromFile(Game.Services, @"Content/Skins/Alttp/Alttp.skin.xml");
            
            // Add image control renderer to renderer repository
            var flatGuiVisualizer = _gui.Visualizer as FlatGuiVisualizer;

            if (flatGuiVisualizer == null)
                throw new NullReferenceException("flatGuiVisualizer cannot be null");

            flatGuiVisualizer.RendererRepository.AddAssembly(typeof(FlatImageControlRenderer).Assembly);

            _gui.Screen = new Screen(Game.GraphicsDevice.Viewport.Width, Game.GraphicsDevice.Viewport.Height);
            _gui.DrawOrder = 1100;

            SelectionBounds = Rectangle.Empty;

            SetupOverlays();
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            BlankTexture = _content.Load<Texture2D>("Textures/Blank");
            MinimapTexture = _content.Load<Texture2D>("Textures/Minimap");
        }

        public override void Update(GameTime gameTime)
        {
            Frame++;
            ElapsedTime = gameTime;

            base.Update(gameTime);

            _gui.Update(gameTime);
            
            // Update overlays
            foreach (var child in _gui.Screen.Desktop.Children)
            {
                var overlay = child as Overlay;

                if (overlay != null)
                    overlay.Update(gameTime);
            }

            // Enable/disable grid lines with the G key
            if (_input.IsKeyPressed(Keys.G))
                RenderGrid = !RenderGrid;

            // Enable/disable grid lines with the O key
            if (_input.IsKeyPressed(Keys.O))
                RenderOverlay = !RenderOverlay;

            Vector2 mousePos = _input.MousePos;
            RectangleF minimapBounds = MinimapOverlay.Minimap.GetAbsoluteBounds();
            
            // Selection region controls
            if (_input.IsMouseButtonPressed(MouseButtons.Left) && !minimapBounds.Contains(mousePos))
            {
                SelectionBounds = new Rectangle((int) mousePos.X, (int) mousePos.Y, 0, 0);
            }
            else if (_input.IsMouseButtonReleased(MouseButtons.Left) && !minimapBounds.Contains(mousePos))
            {
                SelectedGameObjects = GameObject.FindAll(Utils.RenderableRectangle(SelectionBounds), _world.ActiveCamera.Position, _world.ActiveCamera.InvZoom);
                
                SelectionBounds = Rectangle.Empty;

                // Set camera mode if there are game objects selected
                if (SelectedGameObjects.Length > 0)
                {
                    _world.ActiveCamera.Follow(SelectedGameObjects[0]);

                    if (!_gui.Screen.Desktop.Children.Contains(GameObjectOverlay))
                        _gui.Screen.Desktop.Children.Add(GameObjectOverlay);
                }
                else
                {
                    _world.ActiveCamera.Free();

                    if (_gui.Screen.Desktop.Children.Contains(GameObjectOverlay))
                        _gui.Screen.Desktop.Children.Remove(GameObjectOverlay);
                }
            }

            if (_input.IsMouseButtonDown(MouseButtons.Left) && SelectionBounds != Rectangle.Empty)
            {
                var bounds = SelectionBounds;

                int width = (int)mousePos.X - bounds.X,
                    height = (int)mousePos.Y - bounds.Y;

                SelectionBounds = new Rectangle(bounds.X, bounds.Y, width, height);
            }

            // Move minimap viewport with left mouse button.
            // Do not check this if the overlay is hidden.
            if (_gui.Visible &&
                _world.ActiveCamera.CameraMode != CameraMode.Follow &&
                _input.MouseState.LeftButton == ButtonState.Pressed &&
                minimapBounds.Contains(mousePos))
            {
                Vector2 minimapPos = mousePos;
                minimapPos.X -= minimapBounds.Left;
                minimapPos.Y -= minimapBounds.Top;

                var minimapBoundsInt = new Rectangle((int)minimapBounds.X, (int)minimapBounds.Y, (int)minimapBounds.Width, (int)minimapBounds.Height);

                Vector2 newViewportPos = _world.ActiveCamera.MinimapToWorldCoordinates(minimapPos, minimapBoundsInt);
                newViewportPos.X -= _world.ActiveCamera.Viewport.Width / (float)2;
                newViewportPos.Y -= _world.ActiveCamera.Viewport.Height / (float)2;

                _world.ActiveCamera.Position = newViewportPos;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _batch.Begin();

            if (RenderGrid)
                DrawGridLines();

            // Draw selection rectangle
            if (SelectionBounds != Rectangle.Empty)
            {
                var selectionBounds = Utils.RenderableRectangle(SelectionBounds);

                _batch.Draw(BlankTexture, selectionBounds, new Color(100, 0, 0, 75));
                Utils.DrawBorder(_batch, BlankTexture, selectionBounds, 1, new Color(150, 0, 0, 175));
            }

            // Render border around selected game objects
            if (SelectedGameObjects != null && SelectedGameObjects.Length > 0)
            {
                foreach (var o in SelectedGameObjects)
                {
                    Vector2 objScreenPos = _world.ActiveCamera.WorldToScreen(new Vector2(o.BoundsF.X, o.BoundsF.Y));
                    var objScreenSize = new Vector2(o.BoundsF.Width * _world.ActiveCamera.InvZoom, o.BoundsF.Height * _world.ActiveCamera.InvZoom);

                    Utils.DrawBorder(_batch, BlankTexture, new Rectangle((int)objScreenPos.X, (int)objScreenPos.Y, (int)objScreenSize.X, (int)objScreenSize.Y), 2, Color.DarkRed);
                }
            }

            _batch.End();
        }

        private void DrawGridLines()
        {
            RectangleF viewport = _world.ActiveCamera.ViewportF;

            float tileSizeX = _world.World.TileWidth * _world.ActiveCamera.InvZoom,
                  tileSizeY = _world.World.TileHeight * _world.ActiveCamera.InvZoom;

            float minX = viewport.Left - _world.World.TileWidth;
            float maxX = viewport.Right + _world.World.TileWidth;

            float minY = viewport.Top - _world.World.TileHeight;
            float maxY = viewport.Bottom + _world.World.TileHeight;

            Vector2 minScreen = _world.ActiveCamera.WorldToScreen(new Vector2(minX, minY));
            Vector2 maxScreen = _world.ActiveCamera.WorldToScreen(new Vector2(maxX, maxY));

            float offsetX = (_world.ActiveCamera.Position.X % _world.World.TileWidth) * _world.ActiveCamera.InvZoom;
            float offsetY = (_world.ActiveCamera.Position.Y % _world.World.TileHeight) * _world.ActiveCamera.InvZoom;

            // Vertical grid lines
            float x = minScreen.X;
            while (x <= maxScreen.X)
            {
                _batch.Draw(BlankTexture, new Vector2(x - offsetX, 0), null, Config.GridColor, 0, Vector2.Zero, new Vector2(1, Config.ScreenHeight), SpriteEffects.None, 0);

                x += tileSizeX;
            }

            // Horizontal grid lines
            float y = minScreen.Y;
            while (y <= maxScreen.Y)
            {
                _batch.Draw(BlankTexture, new Vector2(0, y - offsetY), null, Config.GridColor, 0, Vector2.Zero, new Vector2(Config.ScreenWidth, 1), SpriteEffects.None, 0);

                y += tileSizeY;
            }
        }

        private void SetupOverlays()
        {
            // Camera overlay
            CameraOverlay = new CameraOverlay(_world, "Camera", 230, _input)
            {
                X = new UniScalar(0, 10),
                Y = new UniScalar(0, 10)
            };

            const int gameInfoWidth = 245;
            GameInfoOverlay = new GameInfoOverlay(this, "Game Info", gameInfoWidth, _world.ActiveCamera)
            {
                X = new UniScalar(1, -gameInfoWidth - 10),
                Y = new UniScalar(0, 10)
            };

            const int gameObjectWidth = 250;
            GameObjectOverlay = new GameObjectOverlay(this, "Object", gameObjectWidth, _world.ActiveCamera)
            {
                X = new UniScalar(0, 10)
            };

            const int minimapWidth = 276,
                      minimapHeight = 302;
            MinimapOverlay = new MinimapOverlay(BlankTexture, "Minimap", minimapWidth, minimapHeight, MinimapTexture, _world)
            {
                X = new UniScalar(1, -minimapWidth - 10),
                Y = new UniScalar(1, -minimapHeight - 10)
            };

            _gui.Screen.Desktop.Children.Add(CameraOverlay);
            _gui.Screen.Desktop.Children.Add(GameInfoOverlay);
            _gui.Screen.Desktop.Children.Add(MinimapOverlay);

            // Update minimap viewport bounds
            RectangleF minimapBounds = MinimapOverlay.Minimap.GetAbsoluteBounds();

            MinimapOverlay.Viewport = _world.ActiveCamera.GetMiniMapViewport(Utils.CastRectangleF(minimapBounds));
        }
    }
}

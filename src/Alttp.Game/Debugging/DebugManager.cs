using System;
using System.Collections.Generic;
using System.Linq;
using Alttp.Console;
using Alttp.Core;
using Alttp.Core.Events;
using Alttp.Core.GameObjects;
using Alttp.Core.GameObjects.Interfaces;
using Alttp.Core.Shields;
using Alttp.Core.UI;
using Alttp.Core.UI.ContextMenus;
using Alttp.Core.UI.Controls;
using Alttp.Core.World;
using Alttp.Debugging.Overlays;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject.Extensions.Logging;
using Nuclex.Input;
using Nuclex.Ninject.Xna;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Visuals.Flat;
using InputManager = Alttp.Core.Input.InputManager;

namespace Alttp.Debugging
{
    public class DebugManager : DrawableGameComponent
    {
        protected readonly ILogger Log;

        private readonly GuiManager _gui;
        private readonly IContentManager _content;
        private readonly ISpriteBatch _batch;
        private readonly InputManager _input;
        private readonly WorldManager _world;
        private readonly AlttpConsole _console;

        public int Frame { get; private set; }
        public GameTime ElapsedTime { get; private set; }

        public IGameObject[] SelectedGameObjects { get; private set; }

        public Rectangle SelectionBounds { get; private set; }

        public Texture2D BlankTexture { get; private set; }
        public Texture2D MinimapTexture { get; private set; }

        /// <summary>Render tile grid.</summary>
        public bool RenderGrid { get; private set; }

        /// <summary>Render region borders.</summary>
        public bool RenderRegionBorders { get; private set; }

        /// <summary>Render debug overlay.</summary>
        public bool RenderOverlay
        {
            get { return _gui.Visible; }
            set { _gui.Visible = value; }
        }

        // Overlays
        public CameraOverlay CameraOverlay { get; private set; }
        public GameInfoOverlay GameInfoOverlay { get; private set; }
        public MinimapOverlay MinimapOverlay { get; private set; }

        // Context menus
        public WorldContextMenu WorldContextMenu { get; private set; }
        public GameObjectContextMenu GameObjectContextMenu { get; private set; }
        public GameObjectsContextMenu GameObjectsContextMenu { get; private set; }
        public EquipmentContextMenu EquipmentContextMenu { get; private set; }

        /// <summary>Currently showing context menu</summary>
        public ContextMenu ActiveContextMenu { get; private set; }

        public DebugManager(ILogger logger, Game game, GuiManager gui, IContentManager content, ISpriteBatch batch, InputManager input, WorldManager world, AlttpConsole console)
            : base(game)
        {
            Log = logger;
            _gui = gui;
            _content = content;
            _batch = batch;
            _input = input;
            _world = world;
            _console = console;

            RenderOverlay = true;
        }

        public override void Initialize()
        {
            base.Initialize();

            SelectionBounds = Rectangle.Empty;
            SelectedGameObjects = new GameObject[0];

            SetupOverlays();

            // Setup context menus
            WorldContextMenu = new WorldContextMenu();
            GameObjectContextMenu = new GameObjectContextMenu();
            GameObjectsContextMenu = new GameObjectsContextMenu();
            EquipmentContextMenu = new EquipmentContextMenu();
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
            
            // Update overlays
            foreach (var child in _gui.Screen.Desktop.Children)
            {
                var overlay = child as Overlay;

                if (overlay != null)
                    overlay.Update(gameTime);
            }

            Vector2 mousePos = _input.MousePos;
            Vector2 mouseWorldPos = _world.ActiveCamera.ScreenToWorld(mousePos);
            RectangleF minimapBounds = MinimapOverlay.Minimap.GetAbsoluteBounds();

            // Move minimap viewport with left mouse button.
            // Do not check this if the overlay is hidden.
            if (_gui.Visible &&
                SelectionBounds == Rectangle.Empty &&
                _world.ActiveCamera.CameraMode != CameraMode.Follow &&
                _input.MouseState.LeftButton == ButtonState.Pressed &&
                minimapBounds.Contains(mousePos))
            {
                Vector2 minimapPos = mousePos;
                minimapPos.X -= minimapBounds.Left;
                minimapPos.Y -= minimapBounds.Top;

                var minimapBoundsInt = new Rectangle((int)minimapBounds.X, (int)minimapBounds.Y, (int)minimapBounds.Width, (int)minimapBounds.Height);

                Vector2 newViewportPos = _world.ActiveCamera.MinimapToWorldCoordinates(minimapPos, minimapBoundsInt);
                newViewportPos.X -= _world.ActiveCamera.Viewport.Width / 2f;
                newViewportPos.Y -= _world.ActiveCamera.Viewport.Height / 2f;

                _world.ActiveCamera.Position = newViewportPos;
            }

            // Clear context menu
            if (ActiveContextMenu != null &&
                (_input.IsMouseButtonPressed(MouseButtons.Left) || _input.IsMouseButtonPressed(MouseButtons.Right)) &&
                !ActiveContextMenu.GetAbsoluteBounds().Contains(mousePos))
            {
                HideContextMenu();
            }

            // Selection region controls
            if (ActiveContextMenu == null &&
                _input.IsMouseButtonPressed(MouseButtons.Left) && !minimapBounds.Contains(mousePos))
            {
                SelectionBounds = new Rectangle((int)mousePos.X, (int)mousePos.Y, 0, 0);

            }
            else if (_input.IsMouseButtonReleased(MouseButtons.Left) &&
                SelectionBounds != Rectangle.Empty)
            {
                SelectedGameObjects = GetSelectedObjects();

                SelectionBounds = Rectangle.Empty;
            }

            // Update selection bounds if mouse is pressed
            if (_input.IsMouseButtonDown(MouseButtons.Left) && SelectionBounds != Rectangle.Empty)
            {
                var bounds = SelectionBounds;

                int width = (int)mousePos.X - bounds.X,
                    height = (int)mousePos.Y - bounds.Y;

                SelectionBounds = new Rectangle(bounds.X, bounds.Y, width, height);
            }

            // Show context menu with right mouse click
            if (_input.IsMouseButtonReleased(MouseButtons.Right))
            {
                // If objects are selected and user right clicks while not hovering
                // any of the selected objects, deselect them all.
                if (SelectedGameObjects.Length > 0)
                {
                    bool containsMouse = SelectedGameObjects.Any(o => o.BoundsF.Contains(mouseWorldPos));
                    if (!containsMouse)
                        SelectedGameObjects = new IGameObject[0];
                }

                SelectedGameObjects = GetSelectedObjects(mouseWorldPos);

                DisplayContextMenu(mousePos, mouseWorldPos);
            }

            // Do not process any of the commands if the console is open
            if (_console.Window.IsClosed)
            {
                // Enable/disable drawing of the collision tiles layer
                if (_input.IsKeyPressed(Keys.C))
                    _world.ActiveCamera.World.RenderCollisionTiles = !_world.ActiveCamera.World.RenderCollisionTiles;

                // Enable/disable grid lines with the G key
                if (_input.IsKeyPressed(Keys.G))
                    RenderGrid = !RenderGrid;

                // Enable/disable grid lines with the O key
                if (_input.IsKeyPressed(Keys.O))
                    RenderOverlay = !RenderOverlay;

                // Enable/disable region borders with the R key
                if (_input.IsKeyPressed(Keys.R))
                    RenderRegionBorders = !RenderRegionBorders;
            }
        }

        #region Draw Methods

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _batch.Begin();

            if (RenderGrid)
                DrawGridLines();

            // Draw selection rectangle
            if (SelectionBounds != Rectangle.Empty &&
                ((SelectionBounds.Width > 5 || SelectionBounds.Width < -5) && (SelectionBounds.Height > 5 || SelectionBounds.Height < -5)))
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
                    var objScreenSize = new Vector2(o.BoundsF.Width * _world.ActiveCamera.InvZoom,
                                                    o.BoundsF.Height * _world.ActiveCamera.InvZoom);

                    Utils.DrawBorder(_batch, BlankTexture,
                                     new Rectangle((int) objScreenPos.X, (int) objScreenPos.Y, (int) objScreenSize.X,
                                                   (int) objScreenSize.Y), 2, Color.DarkRed);
                }
            }

            // Render region borders
            if (RenderRegionBorders)
            {
                foreach (var region in _world.ActiveCamera.World.Regions)
                    region.DrawBorders(_batch, BlankTexture, _world.ActiveCamera);
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
                _batch.Draw(BlankTexture, new Vector2(x - offsetX, 0), null, Config.GridColor, 0, Vector2.Zero,
                            new Vector2(1, Config.ScreenHeight), SpriteEffects.None, 0);

                x += tileSizeX;
            }

            // Horizontal grid lines
            float y = minScreen.Y;
            while (y <= maxScreen.Y)
            {
                _batch.Draw(BlankTexture, new Vector2(0, y - offsetY), null, Config.GridColor, 0, Vector2.Zero,
                            new Vector2(Config.ScreenWidth, 1), SpriteEffects.None, 0);

                y += tileSizeY;
            }
        }

        #endregion

        private void SetupOverlays()
        {
            // Camera overlay
            CameraOverlay = new CameraOverlay(_world, "Camera", 230, _input)
            {
                X = new UniScalar(0, 10)
            };
            CameraOverlay.Y = new UniScalar(1, -CameraOverlay.Height - 10);

            const int gameInfoWidth = 245;
            GameInfoOverlay = new GameInfoOverlay(this, "Game Info", gameInfoWidth, _world.ActiveCamera)
            {
                X = new UniScalar(1, -gameInfoWidth - 10),
                Y = new UniScalar(0, 10)
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

            MinimapOverlay.UpdateCameraBorderBounds(0, _world.ActiveCamera.GetMiniMapViewport(Utils.CastRectangleF(minimapBounds)));
        }

        /// <summary>
        /// Display context menu related to the object mousePos is hovering
        /// </summary>
        /// <param name="screenPos">Screen position</param>
        /// <param name="worldPos">World position</param>
        private void DisplayContextMenu(Vector2 screenPos, Vector2 worldPos)
        {
            if (ActiveContextMenu != null)
                return;

            if (SelectedGameObjects.Length == 0)
            {
                // No objects found, display context menu for World
                WorldContextMenu.Update(screenPos, Game);
                ActiveContextMenu = WorldContextMenu;
            }
            else if (SelectedGameObjects.Length == 1)
            {
                var equipment = SelectedGameObjects[0] as IEquipment;
                if (equipment != null)
                {
                    EquipmentContextMenu.Update(screenPos, SelectedGameObjects[0], _world.Player.Link);
                    ActiveContextMenu = EquipmentContextMenu;
                }

                // No derived game objects context menu found, use default
                if (ActiveContextMenu == null)
                {
                    GameObjectContextMenu.Update(screenPos, SelectedGameObjects[0], _world.Player.Link);
                    ActiveContextMenu = GameObjectContextMenu;
                }
            }
            else
            {
                GameObjectsContextMenu.Update(screenPos, SelectedGameObjects, _world.Player.Link);
                ActiveContextMenu = GameObjectsContextMenu;
            }

            if (ActiveContextMenu != null)
            {
                _gui.Screen.Desktop.Children.Add(ActiveContextMenu);

                // Subscribe to command executed event
                ActiveContextMenu.CommandExecuted += ContextCommandExecuted;
            }
        }

        /// <summary>
        /// Hide active context menu
        /// </summary>
        private void HideContextMenu()
        {
            if (ActiveContextMenu != null)
            {
                // Unsubscribe to command executed event
                ActiveContextMenu.CommandExecuted -= ContextCommandExecuted;

                // Remove from screen
                _gui.Screen.Desktop.Children.Remove(ActiveContextMenu);
                ActiveContextMenu = null;
            }
        }

        private IGameObject[] GetSelectedObjects(Vector2? mousePos = null)
        {
            IGameObject[] objs;

            if (mousePos == null)
            {
                objs = GameObject.FindAll(Utils.RenderableRectangle(SelectionBounds), _world.ActiveCamera.Position, _world.ActiveCamera.InvZoom);
            }
            else
            {
                IGameObject obj = GameObject.GameObjects.FirstOrDefault(o => o.BoundsF.Contains((Vector2)mousePos));
                if (obj == null)
                    objs = new IGameObject[0];
                else
                    if (SelectedGameObjects.Contains(obj))
                        objs = SelectedGameObjects;
                    else
                        objs = new [] { obj };
            }

            // Update selected objects variable in console
            _console.SetSelectedObjects(objs);

            return objs;
        }

        private void ContextCommandExecuted(object sender, EventArgs e)
        {
            HideContextMenu();
        }
    }
}

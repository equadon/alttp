using System;
using Alttp.Core;
using Alttp.GameObjects;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;

namespace Alttp.Worlds
{
    public enum CameraMode { Free, Follow }

    public delegate void ViewportChangedEventHandler(object sender, EventArgs e);

    public class Camera
    {
        public static readonly float DefaultCameraSpeed = 4.0f;

        public event ViewportChangedEventHandler ViewportChanged;

        private readonly Game _game;

        private RectangleF _viewportF;

        #region Properties

        public string Name { get; private set; }

        public Rectangle Bounds { get; private set; }

        public CameraMode CameraMode { get; private set; }

        public IWorld World { get; set; }

        public float Speed { get; set; }

        /// <summary>GameObject the camera will target if Mode is set to Follow</summary>
        public GameObject Target { get; private set; }

        /// <summary>Returns the region this camera is currently in</summary>
        public Region Region
        {
            get { return World.GetRegion(CenterPosition); }
        }

        public float Zoom
        {
            get { return _viewportF.Width / ScreenWidth; }
        }

        public float InvZoom
        {
            get { return 1 / Zoom; }
        }

        public float HorizontalZoomAmount
        {
            get { return 1 / (Bounds.Width / _viewportF.Width) * 5f; }
        }

        public float VerticalZoomAmount
        {
            get { return 1 / (Bounds.Width / _viewportF.Width) * 5f * (_viewportF.Height / _viewportF.Width); }
        }

        public Rectangle Viewport
        {
            get { return new Rectangle((int)_viewportF.X, (int)_viewportF.Top, (int)_viewportF.Width, (int)_viewportF.Height); }
        }

        public RectangleF ViewportF
        {
            get { return _viewportF; }
        }

        public Vector2 Position
        {
            get { return new Vector2(_viewportF.Left, _viewportF.Top); }
            set
            {
                _viewportF.X = MathHelper.Clamp(value.X, 0, MapWidth - _viewportF.Width - TileOrigin.X);
                _viewportF.Y = MathHelper.Clamp(value.Y, 0, MapHeight - _viewportF.Height - TileOrigin.Y);

                OnViewportChanged(EventArgs.Empty);
            }
        }

        public Vector2 CenterPosition
        {
            get { return new Vector2(Position.X + Width / 2f, Position.Y + Height / 2f); }
        }

        public int ScreenWidth { get { return _game.GraphicsDevice.Viewport.Width; } }
        public int ScreenHeight { get { return _game.GraphicsDevice.Viewport.Height; } }

        public float Width
        {
            get { return _viewportF.Width; }
            set
            {
                _viewportF.Width = MathHelper.Clamp(value, 0, MapWidth - _viewportF.X);
                _viewportF.Height = value * (ScreenHeight / (float)ScreenWidth);

                OnViewportChanged(EventArgs.Empty);
            }
        }

        public float Height
        {
            get { return _viewportF.Height; }
        }

        public int MapWidth
        {
            get { return (World == null) ? 0 : World.WidthInPixels; }
        }

        public int MapHeight
        {
            get { return (World == null) ? 0 : World.HeightInPixels; }
        }

        public float TileWidth
        {
            get { return World.TileWidth * InvZoom; }
        }

        public float TileHeight
        {
            get { return World.TileHeight * InvZoom; }
        }

        public Vector2 TileOrigin
        {
            get { return new Vector2(TileWidth / 2f, TileHeight / 2f); }
        }

        public Matrix TranslationMatrix
        {
            get { return Matrix.CreateTranslation(new Vector3(-Position, 0)); }
        }

        public Matrix Matrix
        {
            get { return TranslationMatrix * Matrix.CreateScale(InvZoom); }
        }

        #endregion

        public Camera(string name, Game game, IWorld world)
        {
            Name = name;

            _game = game;
            World = world;

            CameraMode = CameraMode.Free;

            Bounds = new Rectangle(0, 0, ScreenWidth, ScreenHeight);

            Speed = DefaultCameraSpeed;

            _viewportF = RectangleF.Empty;
        }

        public void Move(Vector2 direction)
        {
            Move(direction.X * Speed, direction.Y * Speed);
        }

        public void Move(float x, float y)
        {
            Position = new Vector2(_viewportF.X + x * Zoom, _viewportF.Y + y * Zoom);
        }

        public void ZoomIn(float factor = 1.0f)
        {
            factor = Math.Abs(factor);
            _viewportF.Inflate(-HorizontalZoomAmount * factor, -VerticalZoomAmount * factor);

            AdjustViewport();
        }

        public void ZoomOut(float factor = 1.0f)
        {
            factor = Math.Abs(factor);
            _viewportF.Inflate(HorizontalZoomAmount * factor, VerticalZoomAmount * factor);

            AdjustViewport();
        }

        private void AdjustViewport()
        {
            Position = new Vector2(_viewportF.X, _viewportF.Y);

            Width = _viewportF.Width;
        }

        public void ResetZoom()
        {
            Width = ScreenWidth;
        }

        public void DefaultZoom()
        {
            Width = ScreenWidth / 3.125f;
        }

        public void MaxZoomOut()
        {
            Position = new Vector2(0, 0);

            Width = MapWidth;
        }

        public Vector2 MinimapToWorldCoordinates(Vector2 minimapPos, Rectangle minimap)
        {
            float scaleX = 1 / (float)minimap.Width;
            float scaleY = 1 / (float)minimap.Height;

            return new Vector2(
                minimapPos.X * scaleX * MapWidth,
                minimapPos.Y * scaleY * MapHeight);
        }

        public Rectangle GetMiniMapViewport(Rectangle minimap)
        {
            const float scaleX = 1 / (float)4096;
            const float scaleY = 1 / (float)4096;

            int viewX = minimap.Left + (int)(minimap.Width * scaleX * Viewport.Left),
                viewY = minimap.Top + (int)(minimap.Height * scaleY * Viewport.Top),
                viewW = (int)(minimap.Width * scaleX * Viewport.Width),
                viewH = (int)(minimap.Height * scaleY * Viewport.Height);

            return new Rectangle(viewX, viewY, viewW, viewH);
        }

        /// <summary>
        /// Helper method to convert world to screen coordinates.
        /// </summary>
        /// <param name="worldPosition">World position</param>
        /// <returns>Screen position</returns>
        public Vector2 WorldToScreen(Vector2 worldPosition)
        {
            return Utils.WorldToScreen(worldPosition, Position, InvZoom);
        }

        /// <summary>
        /// Helper method to convert a rectangle with world to screen coordinates.
        /// </summary>
        /// <param name="bounds">World position bounds</param>
        /// <returns>Screen position</returns>
        public Rectangle WorldToScreen(Rectangle bounds)
        {
            return Utils.WorldToScreen(bounds, Position, InvZoom);
        }

        /// <summary>
        /// Helper method to convert screen to world coordinates.
        /// </summary>
        /// <param name="screenPosition">Screen position</param>
        /// <returns>World position</returns>
        public Vector2 ScreenToWorld(Vector2 screenPosition)
        {
            return Utils.ScreenToWorld(screenPosition, Position, InvZoom);
        }

        private void OnViewportChanged(EventArgs e)
        {
            if (ViewportChanged != null)
                ViewportChanged(this, e);
        }

        public Rectangle GetTilesRegion()
        {
            int minX = Viewport.Left / World.TileWidth;
            int maxX = (int)MathHelper.Clamp((int)Math.Ceiling(Viewport.Right / (float)World.TileWidth), 0, World.Width - 1);

            int minY = Viewport.Top / World.TileHeight;
            int maxY = (int)MathHelper.Clamp(1 + (int)Math.Ceiling(Viewport.Bottom / (float)World.TileHeight), 0, World.Height - 1);

            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }

        /// <summary>
        /// Change camera mode to follow the game object.
        /// </summary>
        /// <param name="gameObject">GameObject the camera will follow</param>
        public void Follow(GameObject gameObject)
        {
            Target = gameObject;

            CameraMode = CameraMode.Follow;
        }

        /// <summary>
        /// Allow camera to move freely.
        /// </summary>
        public void Free()
        {
            CameraMode = CameraMode.Free;
        }

        public void Update(GameTime gameTime)
        {
            if (CameraMode == CameraMode.Follow && Target != null)
            {
                Position = new Vector2(
                    Target.Position.X - Width / 2f,
                    Target.Position.Y - Height / 2f);
            }
        }
    }
}
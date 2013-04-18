using System.Collections.Generic;
using Alttp.Console;
using Alttp.Core;
using Alttp.Core.Animation;
using Alttp.Core.GameObjects;
using Alttp.Core.Graphics;
using Alttp.Core.Input;
using Alttp.Core.World;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using Ninject.Extensions.Logging;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class WorldManager : DrawableGameComponent
    {
        private readonly InputManager _input;
        private readonly IContentManager _content;
        private readonly ISpriteBatch _batch;
        private readonly AlttpConsole _console;

        // Middle mouse camera movement
        private bool _middleMouseDown;
        private Vector2 _middleMouseStartPosition;

        private int _activeCameraIndex;

        #region Properties

        protected ILogger Log { get; set; }

        public Dictionary<int, Camera> Cameras { get; private set; }

        public AnimationsDict WorldObjectAnimations { get; private set; }

        // Worlds
        public IWorld World { get; private set; }

        public Player Player { get; private set; }

        public int ActiveCameraIndex
        {
            get { return _activeCameraIndex; }
            private set
            {
                _activeCameraIndex = value;
                _console.SetActiveCamera(ActiveCamera);
            }
        }

        public Camera ActiveCamera
        {
            get { return Cameras[ActiveCameraIndex]; }
        }

        #endregion

        public WorldManager(ILogger logger, Game game, IContentManager content, IWorld world, ISpriteBatch batch, InputManager input, Player player, AlttpConsole console)
            : base(game)
        {
            Log = logger;

            _content = content;
            _batch = batch;
            _input = input;
            _console = console;

            Cameras = new Dictionary<int, Camera>();

            World = world;

            Player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Cameras
            var camera = new Camera("Camera 1", Game, World, new Vector2(2015, 2673));
            camera.DefaultZoom();

            Cameras.Add(0, camera);
            
            SelectCamera(0);
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, ActiveCamera.Matrix);

            World.Draw(gameTime, _batch, ActiveCamera);

            Player.Draw(_batch);

            _batch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            ActiveCamera.Update(gameTime);

            // Handle user input in Player class
            Player.Update(gameTime, _console.Window.IsClosed);

            if (_console.Window.IsClosed)
                HandleKeyboardInput(gameTime);

            HandleMouseInput(gameTime);
        }

        private void HandleKeyboardInput(GameTime gameTime)
        {
            if (_input.IsKeyPressed(Keys.D1))
                SelectCamera(0, true);
            if (_input.IsKeyPressed(Keys.D2))
                SelectCamera(1, true);
            if (_input.IsKeyPressed(Keys.D3))
                SelectCamera(2, true);
            if (_input.IsKeyPressed(Keys.D4))
                SelectCamera(3, true);
            if (_input.IsKeyPressed(Keys.D5))
                SelectCamera(4, true);
            if (_input.IsKeyPressed(Keys.D6))
                SelectCamera(5, true);
            if (_input.IsKeyPressed(Keys.D7))
                SelectCamera(6, true);
            if (_input.IsKeyPressed(Keys.D8))
                SelectCamera(7, true);
            if (_input.IsKeyPressed(Keys.D9))
                SelectCamera(8, true);

            // None of the below should be checked if the camera is in follow mode
            if (ActiveCamera.CameraMode == CameraMode.Follow)
                return;

            // Camera movement
            Vector2 camDirection = Vector2.Zero;

            if (_input.IsKeyDown(Keys.Up))
                camDirection.Y -= 1;
            if (_input.IsKeyDown(Keys.Down))
                camDirection.Y += 1;

            if (_input.IsKeyDown(Keys.Left))
                camDirection.X -= 1;
            if (_input.IsKeyDown(Keys.Right))
                camDirection.X += 1;

            if (camDirection != Vector2.Zero)
                ActiveCamera.Move(camDirection);

            // Camera zoom
            if (_input.IsKeyDown(Keys.PageUp))
                ActiveCamera.ZoomIn();
            if (_input.IsKeyDown(Keys.PageDown))
                ActiveCamera.ZoomOut();

            if (_input.IsKeyPressed(Keys.Home))
                ActiveCamera.DefaultZoom();
            if (_input.IsKeyPressed(Keys.End))
                ActiveCamera.MaxZoomOut();
            if (_input.IsKeyPressed(Keys.D0))
                ActiveCamera.ResetZoom();
        }

        private void HandleMouseInput(GameTime gameTime)
        {
            Vector2 mousePos = _input.MousePos;

            // Mouse wheel
            if ((_console.Window.IsClosed || _console.Window.IsClosing) && _input.MouseWheelValueChanged())
            {
                if (_input.MouseWheelValueDiff > 0)
                    ActiveCamera.ZoomIn(8);
                else
                    ActiveCamera.ZoomOut(8);
            }

            // None of the below should be checked if the camera is in follow mode
            if (ActiveCamera.CameraMode == CameraMode.Follow)
                return;

            // Move around with middle mouse button
            if (_input.MouseState.MiddleButton == ButtonState.Pressed)
            {
                if (!_middleMouseDown)
                    _middleMouseStartPosition = mousePos;

                // Move camera based on mouse position relative to the starting position
                float amountX = _middleMouseStartPosition.X - mousePos.X,
                      amountY = _middleMouseStartPosition.Y - mousePos.Y;

                ActiveCamera.Move(amountX, amountY);

                _middleMouseStartPosition = mousePos;

                _middleMouseDown = true;
            }
            else if (_middleMouseDown && _input.MouseState.MiddleButton == ButtonState.Released)
            {
                _middleMouseDown = false;
            }
        }

        /// <summary>
        /// Set the active camera to the camera with the specified index.
        /// </summary>
        /// <param name="cameraIndex">Index of the camera</param>
        /// <param name="createCamera">Creates a new camera if true</param>
        private void SelectCamera(int cameraIndex, bool createCamera = false)
        {
            if (createCamera && !Cameras.ContainsKey(cameraIndex))
            {
                var camera = new Camera("Camera " + (cameraIndex + 1), Game, World, Vector2.Zero);
                camera.DefaultZoom();

                Cameras.Add(cameraIndex, camera);

                Log.Debug("\"{0}\" was created", camera.Name);
            }

            if (Cameras.ContainsKey(cameraIndex))
                ActiveCameraIndex = cameraIndex;
        }
    }
}

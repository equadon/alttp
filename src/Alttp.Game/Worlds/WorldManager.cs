using System.Collections.Generic;
using Alttp.Core.Animation;
using Alttp.Core.Graphics;
using Alttp.Core.Input;
using Alttp.GameObjects;
using FuncWorks.XNA.XTiled;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using Nuclex.Ninject.Xna;

namespace Alttp.Worlds
{
    public class WorldManager : DrawableGameComponent
    {
        private readonly InputManager _input;
        private readonly IContentManager _content;
        private readonly ISpriteBatch _batch;

        // Middle mouse camera movement
        private bool _middleMouseDown;
        private Vector2 _middleMouseStartPosition;

        private int _activeCamIndex;

        #region Properties

        public List<Camera> Cameras { get; private set; }

        public AnimationsDict WorldObjectAnimations { get; private set; }

        // Worlds
        public IWorld World { get; private set; }

        public Player Player { get; private set; }

        public Camera ActiveCamera
        {
            get { return Cameras[_activeCamIndex]; }
        }

        #endregion

        public WorldManager(Game game, IContentManager content, IWorld world, ISpriteBatch batch, InputManager input, Player player)
            : base(game)
        {
            _content = content;
            _batch = batch;
            _input = input;

            Cameras = new List<Camera>();

            World = world;

            Player = player;
        }

        public override void Initialize()
        {
            base.Initialize();

            // Cameras
            var camera = new Camera("Main", Game, World)
                {
                    World = World,
                    Position = new Vector2(2015, 2673)
                };
            camera.DefaultZoom();
            Cameras.Add(camera);

            _activeCamIndex = 0;

            camera = new Camera("Secondary", Game, World)
                {
                    World = World,
                    Position = new Vector2(1000, 1000)
                };
            camera.ResetZoom();
            Cameras.Add(camera);
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            Player.Object = new Link(new Vector2(2200, 2850), _content.Load<AnimationsDict>("GameObjects/Link/LinkAnimations"));
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
            Player.Update(gameTime);

            HandleKeyboardInput(gameTime);
            HandleMouseInput(gameTime);
        }

        private void HandleKeyboardInput(GameTime gameTime)
        {
            // Switch active camera
            //    1 = main camera
            //    2 = secondary camera
            if (_input.IsKeyPressed(Keys.D1))
                SelectCamera(0);
            if (_input.IsKeyPressed(Keys.D2))
                SelectCamera(1);

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
            if (_input.MouseWheelValueChanged())
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
        /// <param name="cameraIndex"></param>
        private void SelectCamera(int cameraIndex)
        {
            if (cameraIndex >= 0 && cameraIndex < Cameras.Count)
                _activeCamIndex = cameraIndex;
        }
    }
}

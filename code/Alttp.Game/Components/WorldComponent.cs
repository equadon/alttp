using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.Graphics;
using Alttp.Engine;
using Alttp.Engine.Input;
using Alttp.GameObjects;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Ninject;
using Nuclex.Ninject.Xna;
using TiledMap = FuncWorks.XNA.XTiled.Map;

namespace Alttp
{
    public class WorldComponent : DrawableGameComponent
    {
        private readonly InputManager _input;
        private readonly IContentManager _content;
        private readonly ISpriteBatch _batch;
        private readonly Camera _mainCamera;
        private readonly Camera _secondaryCamera;

        // Middle mouse camera movement
        private bool _middleMouseDown;
        private Vector2 _middleMouseStartPosition;

        #region Properties

        // Worlds
        public LightWorld World { get; private set; }

        public Camera ActiveCamera { get; set; }

        public AnimationsDict Anims { get; private set; }

        #endregion

        public WorldComponent(Game game, IContentManager content, ISpriteBatch batch, InputManager input, [Named("Main")]Camera mainCamera, [Named("Secondary")]Camera secondaryCamera)
            : base(game)
        {
            _content = content;
            _batch = batch;
            _input = input;
            _mainCamera = mainCamera;
            _secondaryCamera = secondaryCamera;
        }

        public override void Initialize()
        {
            base.Initialize();

            _mainCamera.World = World;
            _mainCamera.DefaultZoom();
            _mainCamera.Position = new Vector2(2015, 2673);

            _secondaryCamera.World = World;
            _secondaryCamera.ResetZoom();
            _secondaryCamera.Position = new Vector2(1000, 1000);

            ActiveCamera = _mainCamera;
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            World = new LightWorld(_content.Load<TiledMap>(@"Maps/LightWorld"));
            Anims = _content.Load<AnimationsDict>("GameObjects/Link/LinkAnimations");
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);

            _batch.Begin(SpriteSortMode.Deferred, BlendState.AlphaBlend, SamplerState.PointClamp, null, null, null, ActiveCamera.Matrix);

            World.Draw(gameTime, _batch, ActiveCamera);

            Anims["/Idle/Down"][0].Draw(_batch, new Vector2(2200, 2850));

            _batch.End();
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            switch (ActiveCamera.CameraMode)
            {
                case CameraMode.Free:
                    HandleKeyboardInput();
                    HandleMouseInput();
                    break;
            }
        }

        private void HandleKeyboardInput()
        {
            // Move link
//            Vector2 direction = Vector2.Zero;
//            if (_input.IsKeyDown(Keys.W))
//                direction.Y--;
//            if (_input.IsKeyDown(Keys.A))
//                direction.X--;
//            if (_input.IsKeyDown(Keys.S))
//                direction.Y++;
//            if (_input.IsKeyDown(Keys.D))
//                direction.X++;
//
//            if (direction != Vector2.Zero)
//                Link.Move(direction);
//
//            if (_input.IsKeyReleased(Keys.W))
//                Link.Stop();
//            if (_input.IsKeyReleased(Keys.A))
//                Link.Stop();
//            if (_input.IsKeyReleased(Keys.S))
//                Link.Stop();
//            if (_input.IsKeyReleased(Keys.D))
//                Link.Stop();
//
//            if (_input.IsKeyPressed(Keys.K))
//                Link.FrameIndex++;
//
//            if (_input.IsKeyPressed(Keys.J))
//                Link.FrameIndex--;

            // Switch active camera
            //    1 = main camera
            //    2 = secondary camera
            if (_input.IsKeyPressed(Keys.D1))
                ActiveCamera = _mainCamera;

            if (_input.IsKeyPressed(Keys.D2))
                ActiveCamera = _secondaryCamera;

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

        private void HandleMouseInput()
        {
            Vector2 mousePos = _input.MousePos;

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

            // Mouse wheel
            if (_input.MouseWheelValueChanged())
            {
                if (_input.MouseWheelValueDiff > 0)
                    ActiveCamera.ZoomIn(8);
                else
                    ActiveCamera.ZoomOut(8);
            }
        }
    }
}

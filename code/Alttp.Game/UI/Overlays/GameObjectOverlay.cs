using System;
using Alttp.Core.TileEngine;
using Alttp.Core.UI;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace Alttp.UI.Overlays
{
    public class GameObjectOverlay : Overlay
    {
        private readonly DebugComponent _debug;
        private readonly Camera _camera;

        // String formats
        private const string LblPositionFormat = "Position: {0}";
        private const string LblAnimationFormat = "Animation: {0}";
        private const string LblAnimationFrameFormat = "Frame: {0}";
        private const string LblFpsFormat = "FPS: {0}";

        // Controls
        private LabelControl _lblPosition;
        private LabelControl _lblAnimation;
        private LabelControl _lblAnimationFrame;
        private LabelControl _lblFps;

        public string PositionText { get { return String.Format(LblPositionFormat, _debug.SelectedGameObjects[0].Position); } }
        public string AnimationText { get { return String.Format(LblAnimationFormat, _debug.SelectedGameObjects[0].AnimationName); } }
        public string AnimationFrameText { get { return String.Format(LblAnimationFrameFormat, _debug.SelectedGameObjects[0].FrameIndex); } }
        public string FpsText { get { return String.Format(LblFpsFormat, _debug.SelectedGameObjects[0].Fps); } }

        public GameObjectOverlay(DebugComponent debug, string title, int width, Camera camera)
            : base(title, width)
        {
            _debug = debug;
            _camera = camera;

            SetupControls();

            CalculateHeight();

            Y = new UniScalar(1, -Height - 10);
        }

        private void SetupControls()
        {
            const int height = 24;

            _lblPosition = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0))
            };
            Children.Add(_lblPosition);

            _lblAnimation = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0))
            };
            Children.Add(_lblAnimation);

            _lblAnimationFrame = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0))
            };
            Children.Add(_lblAnimationFrame);

            _lblFps = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0))
            };
            Children.Add(_lblFps);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (_debug.SelectedGameObjects == null || _debug.SelectedGameObjects.Length == 0)
            {
                Title = "Object: None";
                _lblPosition.Text = "";
                _lblAnimation.Text = "";
                _lblAnimationFrame.Text = "";
                _lblFps.Text = "";
            }
            else
            {
                Title = "Object: " + _debug.SelectedGameObjects[0].GetType().Name;
                _lblPosition.Text = PositionText;
                _lblAnimation.Text = AnimationText;
                _lblAnimationFrame.Text = AnimationFrameText;
                _lblFps.Text = FpsText;
            }
        }
    }
}

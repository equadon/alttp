﻿using System;
using Alttp.Core.UI;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace Alttp.Debugging.Overlays
{
    public class GameObjectOverlay : Overlay
    {
        private readonly DebugManager _debug;
        private readonly Camera _camera;

        // String formats
        private const string LblPositionFormat = "Position: ({0:F1}, {1:F1})";
        private const string LblAnimationFormat = "Animation: {0}";
        private const string LblAnimationFrameFormat = "Frame: {0}";
        private const string LblFpsFormat = "FPS: {0}";
        private const string LblRegionFormat = "Region: {0}";

        // Controls
        private LabelControl _lblPosition;
        private LabelControl _lblAnimation;
        private LabelControl _lblAnimationFrame;
        private LabelControl _lblFps;
        private LabelControl _lblRegion;

        public string PositionText { get { return String.Format(LblPositionFormat, _debug.SelectedGameObjects[0].Position.X, _debug.SelectedGameObjects[0].Position.Y); } }
        public string AnimationText { get { return String.Format(LblAnimationFormat, _debug.SelectedGameObjects[0].AnimationName); } }
        public string AnimationFrameText { get { return String.Format(LblAnimationFrameFormat, _debug.SelectedGameObjects[0].Animation.FrameIndex); } }
        public string FpsText { get { return String.Format(LblFpsFormat, _debug.SelectedGameObjects[0].Animation.Fps); } }
        public string RegionText { get { return String.Format(LblRegionFormat, _camera.World.GetRegion(_debug.SelectedGameObjects[0].Position).Name); } }

        public GameObjectOverlay(DebugManager debug, string title, int width, Camera camera)
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

            _lblRegion = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0))
            };
            Children.Add(_lblRegion);
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
                _lblRegion.Text = "";
            }
            else
            {
                Title = "Object: " + _debug.SelectedGameObjects[0].GetType().Name;
                _lblPosition.Text = PositionText;
                _lblAnimation.Text = AnimationText;
                _lblAnimationFrame.Text = AnimationFrameText;
                _lblFps.Text = FpsText;
                _lblRegion.Text = RegionText;
            }
        }
    }
}
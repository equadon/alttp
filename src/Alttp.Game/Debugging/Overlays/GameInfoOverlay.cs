using System;
using Alttp.Core.UI;
using Alttp.Core.World;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;

namespace Alttp.Debugging.Overlays
{
    public class GameInfoOverlay : Overlay
    {
        private readonly DebugManager _debug;
        private readonly Camera _camera;

        // String formats
        private const string LblFpsFormat = "FPS: {0}";
        private const string LblResolutionFormat = "Resolution: {0}x{1}";
        private const string LblVsyncFormat = "Vsync: {0}";
        private const string LblTilesFormat = "Visible Tiles: {0}";
        private const string LblFrameFormat = "Frame: {0}";
        private const string LblTimeFormat = "Elapsed Time: {0:hh\\:mm\\:ss\\.ff}";

        // Controls
        private LabelControl _lblFps;
        private LabelControl _lblResolution;
        private LabelControl _lblVsync;
        private LabelControl _lblTiles;
        private LabelControl _lblFrame;
        private LabelControl _lblTime;

        private int _frameCount;
        private double _framesTime;

        public int MeasuredFps { get; private set; }

        public int VisibleTiles
        {
            get
            {
                Rectangle region = _camera.GetTilesRegion();

                return region.Width * region.Height;
            }
        }

        public string FpsText { get { return String.Format(LblFpsFormat, MeasuredFps); } }
        public string ResolutionText { get { return String.Format(LblResolutionFormat, _debug.Game.GraphicsDevice.Viewport.Width, _debug.Game.GraphicsDevice.Viewport.Height); } }
        public string VsyncText { get { return String.Format(LblVsyncFormat, (Config.VsyncEnabled) ? "Enabled" : "Disabled"); } }
        public string TilesText { get { return String.Format(LblTilesFormat, VisibleTiles); } }
        public string FrameText { get { return String.Format(LblFrameFormat, _debug.Frame); } }
        public string TimeText { get { return String.Format(LblTimeFormat, _debug.ElapsedTime.TotalGameTime); } }

        public GameInfoOverlay(DebugManager debug, string title, int width, Camera camera)
            : base(title, width)
        {
            _debug = debug;
            _camera = camera;

            SetupControls();

            CalculateHeight();
        }

        private void SetupControls()
        {
            const int height = 24;

            _lblResolution = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = ResolutionText
            };
            Children.Add(_lblResolution);

            _lblVsync = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = VsyncText
            };
            Children.Add(_lblVsync);

            _lblFps = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = FpsText
            };
            Children.Add(_lblFps);

            _lblFrame = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = FrameText
            };
            Children.Add(_lblFrame);

            _lblTiles = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = TilesText
            };
            Children.Add(_lblTiles);

            _lblTime = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * height), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = TilesText
            };
            Children.Add(_lblTime);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            _frameCount++;
            _framesTime += gameTime.ElapsedGameTime.TotalSeconds;

            if (_framesTime >= 1.0f)
            {
                MeasuredFps = (int)(_frameCount / _framesTime);
                _frameCount = 0;
                _framesTime = 0.0;

                _lblFps.Text = FpsText;
            }

            _lblResolution.Text = ResolutionText;
            _lblVsync.Text = VsyncText;
            _lblTiles.Text = TilesText;
            _lblFrame.Text = FrameText;
            _lblTime.Text = TimeText;
        }
    }
}

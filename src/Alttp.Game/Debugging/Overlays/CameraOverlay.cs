using System;
using Alttp.Core.Input;
using Alttp.Core.UI;
using Alttp.Core.World;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Debugging.Overlays
{
    public class CameraOverlay : Overlay
    {
        private readonly WorldManager _world;
        private readonly InputManager _input;

        // String formats
        private const string LblTitleFormat = "Camera ({0})";
        private const string LblPositionFormat = "Position: ({0:F0}, {1:F0})";
        private const string LblMousePosFormat = "Mouse: ({0:F0}, {1:F0})";
        private const string LblSizeFormat = "Size: {0}x{1}";
        private const string LblZoomFormat = "Zoom: {0:F2}";
        private const string LblTileSizeFormat = "Tile Width: {0:F1} px";
        private const string LblRegionFormat = "Region: {0}";

        // Controls
        private LabelControl _lblPosition;
        private LabelControl _lblMousePos;
        private LabelControl _lblSize;
        private LabelControl _lblZoom;
        private LabelControl _lblTileSize;
        private LabelControl _lblRegion;

        private OptionControl _optLockCamera;

        public string TitleText { get { return String.Format(LblTitleFormat, _world.ActiveCamera.Name); } }
        public string PositionText { get { return String.Format(LblPositionFormat, _world.ActiveCamera.Position.X, _world.ActiveCamera.Position.Y); } }

        public string MousePosText
        {
            get
            {
                Vector2 mouseWorldPos = _world.ActiveCamera.ScreenToWorld(_input.MousePos);
                return String.Format(LblMousePosFormat, mouseWorldPos.X, mouseWorldPos.Y);
            }
        }
        public string SizeText { get { return String.Format(LblSizeFormat, _world.ActiveCamera.Viewport.Width, _world.ActiveCamera.Viewport.Height); } }

        public string ZoomText { get { return String.Format(LblZoomFormat, _world.ActiveCamera.InvZoom); } }
        public string TileSizeText { get { return String.Format(LblTileSizeFormat, _world.ActiveCamera.TileWidth); } }
        public string RegionText { get { return String.Format(LblRegionFormat, _world.ActiveCamera.Region.Name); } }

        public CameraOverlay(WorldManager world, string title, int width, InputManager input)
            : base(title, width)
        {
            _world = world;
            _input = input;

            SetupControls();

            // Register event for when the lock camera check box is changed
            _optLockCamera.Changed += OptLockCameraOnChanged;

            CalculateHeight();
        }

        private void SetupControls()
        {
            _lblPosition = new LabelControl()
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                    Text = PositionText
                };
            Children.Add(_lblPosition);

            _lblMousePos = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = MousePosText
            };
            Children.Add(_lblMousePos);

            _lblSize = new LabelControl()
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                    Text = SizeText
                };
            Children.Add(_lblSize);

            _lblZoom = new LabelControl()
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                    Text = ZoomText
                };
            Children.Add(_lblZoom);

            _lblTileSize = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = ZoomText
            };
            Children.Add(_lblTileSize);

            _lblRegion = new LabelControl()
            {
                Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), new UniScalar(1, -10), new UniScalar(0, 0)),
                Text = ZoomText
            };
            Children.Add(_lblRegion);

            _optLockCamera = new OptionControl()
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 40 + Children.Count * 25), 15, 19),
                    Enabled = true,
                    Selected = false,
                    Text = "Lock Camera"
                };
            Children.Add(_optLockCamera);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Title = TitleText;

            _lblPosition.Text = PositionText;
            _lblMousePos.Text = MousePosText;
            _lblSize.Text = SizeText;
            _lblZoom.Text = ZoomText;
            _lblTileSize.Text = TileSizeText;
            _lblRegion.Text = RegionText;
            _optLockCamera.Selected = _world.ActiveCamera.CameraMode == CameraMode.Follow;
        }

        private void OptLockCameraOnChanged(object sender, EventArgs eventArgs)
        {
            if (_optLockCamera.Selected)
                _world.ActiveCamera.Follow(_world.Player.Link);
            else
                _world.ActiveCamera.Free();
        }
    }
}

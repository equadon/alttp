using System.Collections.Generic;
using Alttp.Core;
using Alttp.Core.UI;
using Alttp.Core.UI.Controls;
using Alttp.Worlds;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Nuclex.UserInterface;

namespace Alttp.Debugging.Overlays
{
    public class MinimapOverlay : Overlay
    {
        public static readonly Color ActiveBorderColor = new Color(211, 211, 211);
        public static readonly Color InactiveBorderColor = new Color(160, 160, 160);

        private readonly WorldManager _world;

        public Texture2D MinimapTexture { get; private set; }
        public Texture2D BlankTexture { get; private set; }

        // Controls
        public ImageControl Minimap { get; private set; }

        public BorderControl ActiveCamBorder { get; private set; }
        public Dictionary<int, BorderControl> CameraBorders { get; private set; }

        public MinimapOverlay(Texture2D blankTexture, string title, int width, int height, Texture2D minimapTexture, WorldManager world)
            : base(title, width, height)
        {
            _world = world;

            MinimapTexture = minimapTexture;
            BlankTexture = blankTexture;

            CameraBorders = new Dictionary<int, BorderControl>();

            SetupControls();
        }

        private void SetupControls()
        {
            Children.Add(new BorderControl(BlankTexture, 2, Color.LightGray)
                {
                    Bounds = new UniRectangle(10, 36, MinimapTexture.Width, MinimapTexture.Height)
                });

            Children.Add(Minimap = new ImageControl(MinimapTexture)
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 36), MinimapTexture.Width, MinimapTexture.Height)
                });
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            foreach (int camIndex in _world.Cameras.Keys)
                UpdateCameraBorderBounds(camIndex, _world.Cameras[camIndex].GetMiniMapViewport(Utils.CastRectangleF(Minimap.GetAbsoluteBounds())));
        }

        public void UpdateCameraBorderBounds(int index, Rectangle rect)
        {
            if (!CameraBorders.ContainsKey(index))
            {
                var border = new BorderControl(BlankTexture, 2, Color.LightGray)
                {
                    Bounds = new UniRectangle(10, 36, MinimapTexture.Width, MinimapTexture.Height)
                };
                CameraBorders.Add(index, border);
                Children.Insert(0, border);
            }

            var minimap = Minimap.GetAbsoluteBounds();

            float x = Minimap.Bounds.Left.Offset + rect.X - minimap.X,
                  y = Minimap.Bounds.Top.Offset + rect.Y - minimap.Y;

            CameraBorders[index].Bounds = new UniRectangle(x, y, rect.Width, rect.Height);

            // Set border color
            CameraBorders[index].BorderColor = (index == _world.ActiveCameraIndex)
                                                   ? ActiveBorderColor
                                                   : InactiveBorderColor;
        }
    }
}

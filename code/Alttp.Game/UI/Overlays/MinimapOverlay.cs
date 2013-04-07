using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Alttp.Core.UI;
using Alttp.Core.UI.Controls;
using Alttp.Core.Utils;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp
{
    public class MinimapOverlay : Overlay
    {
        private readonly WorldComponent _world;

        public Texture2D MinimapTexture { get; private set; }
        public Texture2D BlankTexture { get; private set; }

        // Controls
        public ImageControl Minimap { get; private set; }

        public BorderControl CameraBorder { get; private set; }

        /// <summary>Sets the minimap viewport border control</summary>
        public Rectangle Viewport
        {
            set
            {
                var minimap = Minimap.GetAbsoluteBounds();

                float x = Minimap.Bounds.Left.Offset + value.X - minimap.X,
                      y = Minimap.Bounds.Top.Offset + value.Y - minimap.Y;

                CameraBorder.Bounds = new UniRectangle(x, y, value.Width, value.Height);
            }
        }

        public MinimapOverlay(Texture2D blankTexture, string title, int width, int height, Texture2D minimapTexture, WorldComponent world)
            : base(title, width, height)
        {
            _world = world;

            MinimapTexture = minimapTexture;
            BlankTexture = blankTexture;

            SetupControls();
        }

        private void SetupControls()
        {
            Minimap = new ImageControl(MinimapTexture)
                {
                    Bounds = new UniRectangle(new UniScalar(0, 10), new UniScalar(0, 36), MinimapTexture.Width, MinimapTexture.Height)
                };

            CameraBorder = new BorderControl(BlankTexture, 2, Color.LightGray)
                {
                    Bounds = new UniRectangle(10, 36, MinimapTexture.Width, MinimapTexture.Height)
                };

            Children.Add(CameraBorder);
            Children.Add(Minimap);
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            Viewport = _world.ActiveCamera.GetMiniMapViewport(Utils.CastRectangleF(Minimap.GetAbsoluteBounds()));
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Ninject;
using Nuclex.Ninject.Xna;

namespace Alttp
{
    public class AlttpGame : NinjectGame
    {
        private GraphicsDeviceManager _graphics;

        public AlttpGame(IKernel kernel)
            : base(kernel)
        {
            Content.RootDirectory = "Content";

            IsMouseVisible = Config.DisplayCursor;

            Window.Title = Config.WindowTitle;
            Window.AllowUserResizing = Config.AllowWindowResizing;

            // Graphics device manager
            _graphics = new GraphicsDeviceManager(this)
                {
                    IsFullScreen = Config.FullScreen,
                    PreferredBackBufferWidth = Config.ScreenWidth,
                    PreferredBackBufferHeight = Config.ScreenHeight,
                    SynchronizeWithVerticalRetrace = Config.VsyncEnabled
                };
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            base.Draw(gameTime);
        }
    }
}

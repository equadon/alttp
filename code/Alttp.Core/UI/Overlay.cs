using System;
using Microsoft.Xna.Framework;
using Nuclex.UserInterface;
using Nuclex.UserInterface.Controls.Desktop;

namespace Alttp.Core.UI
{
    public class Overlay : WindowControl
    {
        private int _width;

        private UniScalar _x;
        private UniScalar _y;

        #region Properties

        public int Height { get; private set; }

        public int Width
        {
            get { return _width; }
            set
            {
                _width = value;
                Bounds = new UniRectangle(X, Y, value, Height);
            }
        }

        public UniScalar X
        {
            get { return _x; }
            set
            {
                _x = value;
                Bounds = new UniRectangle(value, Y, Width, Height);
            }
        }

        public UniScalar Y
        {
            get { return _y; }
            set
            {
                _y = value;
                Bounds = new UniRectangle(X, value, Width, Height);
            }
        }

        #endregion

        public Overlay(string title, int width, int height = -1)
        {
            Title = title;
            EnableDragging = false;

            X = new UniScalar(0, 0);
            Y = new UniScalar(0, 0);

            Width = width;
            Height = height;
        }

        protected void CalculateHeight()
        {
            if (Height != -1)
                return;

            int height = 0;

            foreach (var child in Children)
                height = Math.Max(height, (int) child.Bounds.Bottom.Offset);

            Height = 20 + height;
        }

        public virtual void Update(GameTime gameTime)
        {
        }
    }
}

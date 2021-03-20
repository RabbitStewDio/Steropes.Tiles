using System;
using Microsoft.Xna.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.TexturePack.Operations;
using Point = Microsoft.Xna.Framework.Point;

namespace Steropes.Tiles.Monogame
{
    public static class XnaExtensions
    {
        public static Rectangle Clip(this Rectangle rect1, Rectangle rect2)
        {
            Rectangle rectangle = new Rectangle(Math.Max(rect1.X, rect2.X), Math.Max(rect1.Y, rect2.Y), 0, 0);
            rectangle.Width = Math.Min(rect1.Right, rect2.Right) - rectangle.X;
            rectangle.Height = Math.Min(rect1.Bottom, rect2.Bottom) - rectangle.Y;
            if (rectangle.Width < 0)
            {
                rectangle.X += rectangle.Width;
                rectangle.Width = 0;
            }

            if (rectangle.Height < 0)
            {
                rectangle.Y += rectangle.Height;
                rectangle.Height = 0;
            }

            return rectangle;
        }

        public static Vector2 ToVector2(this Point point)
        {
            return new Vector2((float)point.X, (float)point.Y);
        }

        public static Rectangle Union(this Rectangle r1, Rectangle r2)
        {
            int x = Math.Min(r1.Left, r2.Left);
            int y = Math.Min(r1.Top, r2.Top);
            return new Rectangle(x, y, Math.Max(r1.Right, r2.Right) - x, Math.Max(r1.Bottom, r2.Bottom) - y);
        }

        public static Rectangle ToXna(this IntRect rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static Rectangle ToXna(this TextureCoordinateRect rect)
        {
            return new Rectangle(rect.X, rect.Y, rect.Width, rect.Height);
        }

        public static IntRect ToTileRect(this Rectangle rect)
        {
            return new IntRect(rect.X, rect.Y, rect.Width, rect.Height);
        }
    }
}

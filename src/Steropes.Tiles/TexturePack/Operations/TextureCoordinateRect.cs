using JetBrains.Annotations;
using System;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.TexturePack.Operations
{
    public struct TextureCoordinateRect : IEquatable<TextureCoordinateRect>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public TextureCoordinateRect(TextureCoordinatePoint point, IntDimension size)
        {
            this.X = point.X;
            this.Y = point.Y;
            this.Width = size.Width;
            this.Height = size.Height;
        }

        public TextureCoordinateRect(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(TextureCoordinateRect other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TextureCoordinateRect && Equals((TextureCoordinateRect) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X;
                hashCode = (hashCode * 31) ^ Y;
                hashCode = (hashCode * 31) ^ Width;
                hashCode = (hashCode * 31) ^ Height;
                return hashCode;
            }
        }

        public TextureCoordinatePoint Origin => new TextureCoordinatePoint(X, Y);

        public IntDimension Size
        {
            get { return new IntDimension(Width, Height); }
        }

        public static bool operator ==(TextureCoordinateRect left, TextureCoordinateRect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TextureCoordinateRect left, TextureCoordinateRect right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return
                $"TextureRect({nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height})";
        }

        [Pure]
        public bool Contains(int x, int y)
        {
            return x >= X && y >= Y && y < (Y + Height) && x < (X + Width);
        }


        public TextureCoordinateRect Clip(TextureCoordinateRect rect2)
        {
            var rect1 = this;
            var x = Math.Max(rect1.X, rect2.X);
            var y = Math.Max(rect1.Y, rect2.Y);
            var width = Math.Min(rect1.X + rect1.Width, rect2.X + rect2.Width) - x;
            var height = Math.Min(rect1.Y + rect1.Height, rect2.Y + rect2.Height) - y;
            if (width < 0)
            {
                x += width;
                width = 0;
            }

            if (height < 0)
            {
                y += height;
                height = 0;
            }

            return new TextureCoordinateRect(x, y, width, height);
        }

        public TextureCoordinateRect Translate(TextureCoordinatePoint origin)
        {
            return new TextureCoordinateRect(origin + Origin, Size);
        }
    }
}
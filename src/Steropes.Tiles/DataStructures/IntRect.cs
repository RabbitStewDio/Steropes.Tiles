using System;

namespace Steropes.Tiles.DataStructures
{
    public readonly struct IntRect : IEquatable<IntRect>
    {
        public readonly int X;
        public readonly int Y;
        public readonly int Width;
        public readonly int Height;

        public IntRect(int x, int y, int width, int height)
        {
            this.X = x;
            this.Y = y;
            this.Width = width;
            this.Height = height;
        }

        public bool Equals(IntRect other)
        {
            return X == other.X && Y == other.Y && Width == other.Width && Height == other.Height;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntRect && Equals((IntRect)obj);
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

        public IntPoint Origin => new IntPoint(X, Y);

        public IntDimension Size
        {
            get { return new IntDimension(Width, Height); }
        }

        public static bool operator ==(IntRect left, IntRect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntRect left, IntRect right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"Rect({nameof(X)}: {X}, {nameof(Y)}: {Y}, {nameof(Width)}: {Width}, {nameof(Height)}: {Height})";
        }

        public bool Contains(int x, int y)
        {
            return x >= X && y >= Y && y < (Y + Height) && x < (X + Width);
        }

        public IntRect Clip(IntRect rect2)
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

            return new IntRect(x, y, width, height);
        }
    }
}

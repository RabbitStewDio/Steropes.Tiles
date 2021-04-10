using System;

namespace Steropes.Tiles.DataStructures
{
    public readonly struct ContinuousViewportCoordinates : IEquatable<ContinuousViewportCoordinates>
    {
        public const int Scale = 4;

        public double X { get; }
        public double Y { get; }

        public ContinuousViewportCoordinates(double x, double y)
        {
            X = x;
            Y = y;
        }

        public static implicit operator ContinuousViewportCoordinates(ViewportCoordinates c)
        {
            return new ContinuousViewportCoordinates(c.X, c.Y);
        }

        public static double FromTileCoordinates(double x)
        {
            return x * Scale;
        }

        public bool Equals(ContinuousViewportCoordinates other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ContinuousViewportCoordinates && Equals((ContinuousViewportCoordinates)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 397) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(ContinuousViewportCoordinates left, ContinuousViewportCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ContinuousViewportCoordinates left, ContinuousViewportCoordinates right)
        {
            return !left.Equals(right);
        }

        public static ContinuousViewportCoordinates operator +(ContinuousViewportCoordinates left,
                                                               ContinuousViewportCoordinates right)
        {
            return new ContinuousViewportCoordinates(left.X + right.X, left.Y + right.Y);
        }

        public static ContinuousViewportCoordinates operator -(ContinuousViewportCoordinates left,
                                                               ContinuousViewportCoordinates right)
        {
            return new ContinuousViewportCoordinates(left.X - right.X, left.Y - right.Y);
        }

        /// <summary>
        ///   Translates the given pixel position (with (0,0) in the upper left corner and y going downwards)
        ///   to screen coordinates.
        /// </summary>
        /// <param name="tileSize"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static ContinuousViewportCoordinates FromPixels(IntDimension tileSize, double x, double y)
        {
            var sx = x * Scale / tileSize.Width;
            var sy = y * Scale / tileSize.Height;
            return new ContinuousViewportCoordinates(sx, sy);
        }

        public DoublePoint ToPixels(IntDimension tileSize)
        {
            return new DoublePoint(X * tileSize.Width / Scale,
                                   Y * tileSize.Height / Scale);
        }

        public ViewportCoordinates ToViewCoordinate()
        {
            var sx = (int)Math.Round(X);
            var sy = (int)Math.Round(Y);
            return new ViewportCoordinates(sx, sy);
        }

        public override string ToString()
        {
            return $"({nameof(X)}: {X}, {nameof(Y)}: {Y})";
        }
    }
}

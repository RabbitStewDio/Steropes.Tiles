using System;

namespace Steropes.Tiles.DataStructures
{
    /// <summary>
    ///  A viewport coordinate pair. Viewport coordinates form a uniform (width ==
    ///  height) grid that defines where tiles are positioned on the screen. A
    ///  renderer takes these coordinates and transforms them into screen/pixel
    ///  coordinates that are used for the actual rendering.
    /// 
    ///  In the viewport coordinate system a tile consumes 4 units. This allows
    ///  the system to express half-tile positions necessary for Isometric and Hex
    ///  systems and also provides options for quarter positions for tiles that
    ///  are assembled from 4 corner sprites.
    /// </summary>
    public readonly struct ViewportCoordinates : IEquatable<ViewportCoordinates>
    {
        public const int Scale = 4;

        public int X { get; }
        public int Y { get; }

        public ViewportCoordinates(int x, int y)
        {
            X = x;
            Y = y;
        }

        public static int FromTileCoordinates(int x)
        {
            return x * Scale;
        }

        public bool Equals(ViewportCoordinates other)
        {
            return X == other.X && Y == other.Y;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is ViewportCoordinates && Equals((ViewportCoordinates)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X * 397) ^ Y;
            }
        }

        public static bool operator ==(ViewportCoordinates left, ViewportCoordinates right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ViewportCoordinates left, ViewportCoordinates right)
        {
            return !left.Equals(right);
        }

        public static ViewportCoordinates operator +(ViewportCoordinates left, ViewportCoordinates right)
        {
            return new ViewportCoordinates(left.X + right.X, left.Y + right.Y);
        }

        public static ViewportCoordinates operator -(ViewportCoordinates left, ViewportCoordinates right)
        {
            return new ViewportCoordinates(left.X - right.X, left.Y - right.Y);
        }

        public override string ToString()
        {
            return $"({nameof(X)}: {X}, {nameof(Y)}: {Y})";
        }
    }
}

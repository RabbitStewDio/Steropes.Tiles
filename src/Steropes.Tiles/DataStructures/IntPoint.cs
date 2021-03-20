using System;

namespace Steropes.Tiles.DataStructures
{
    public struct IntPoint : IEquatable<IntPoint>
    {
        public int X;
        public int Y;

        public IntPoint(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        public bool Equals(IntPoint other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntPoint && Equals((IntPoint) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 31) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(IntPoint left, IntPoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntPoint left, IntPoint right)
        {
            return !left.Equals(right);
        }

        public static IntPoint operator +(IntPoint left, IntPoint right)
        {
            return new IntPoint(left.X + right.X, left.Y + right.Y);
        }

        public static IntPoint operator -(IntPoint left, IntPoint right)
        {
            return new IntPoint(left.X - right.X, left.Y - right.Y);
        }
    }
}
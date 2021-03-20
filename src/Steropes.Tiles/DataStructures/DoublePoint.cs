using System;

namespace Steropes.Tiles.DataStructures
{
    public struct DoublePoint : IEquatable<DoublePoint>
    {
        public double X;
        public double Y;

        public DoublePoint(double x, double y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"{nameof(X)}: {X}, {nameof(Y)}: {Y}";
        }

        public bool Equals(DoublePoint other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DoublePoint && Equals((DoublePoint)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (X.GetHashCode() * 31) ^ Y.GetHashCode();
            }
        }

        public static bool operator ==(DoublePoint left, DoublePoint right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DoublePoint left, DoublePoint right)
        {
            return !left.Equals(right);
        }

        public static DoublePoint operator +(DoublePoint left, DoublePoint right)
        {
            return new DoublePoint(left.X + right.X, left.Y + right.Y);
        }

        public static DoublePoint operator -(DoublePoint left, DoublePoint right)
        {
            return new DoublePoint(left.X - right.X, left.Y - right.Y);
        }

        public static implicit operator DoublePoint(IntPoint p)
        {
            return new DoublePoint(p.X, p.Y);
        }
    }
}

using System;

namespace Steropes.Tiles.DataStructures
{
    public struct IntDimension : IEquatable<IntDimension>
    {
        public readonly int Width;
        public readonly int Height;

        public IntDimension(int width, int height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(IntDimension other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is IntDimension && Equals((IntDimension) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 31) ^ Height.GetHashCode();
            }
        }

        public static bool operator ==(IntDimension left, IntDimension right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntDimension left, IntDimension right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"({nameof(Width)}: {Width}, {nameof(Height)}: {Height})";
        }
    }
}
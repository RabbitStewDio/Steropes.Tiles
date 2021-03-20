using System;

namespace Steropes.Tiles.DataStructures
{
    public struct DoubleDimension : IEquatable<DoubleDimension>
    {
        public readonly double Width;
        public readonly double Height;

        public DoubleDimension(double width, double height)
        {
            Width = width;
            Height = height;
        }

        public bool Equals(DoubleDimension other)
        {
            return Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is DoubleDimension && Equals((DoubleDimension) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Width.GetHashCode() * 31) ^ Height.GetHashCode();
            }
        }

        public static bool operator ==(DoubleDimension left, DoubleDimension right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(DoubleDimension left, DoubleDimension right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"({nameof(Width)}: {Width}, {nameof(Height)}: {Height})";
        }
    }
}
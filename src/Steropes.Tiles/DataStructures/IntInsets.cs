using System;

namespace Steropes.Tiles.DataStructures
{
    public struct IntInsets : IEquatable<IntInsets>
    {
        public int Top { get; }
        public int Left { get; }
        public int Bottom { get; }
        public int Right { get; }

        public IntInsets(int top, int left, int bottom, int right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public bool Equals(IntInsets other)
        {
            return Top.Equals(other.Top) &&
                   Left.Equals(other.Left) &&
                   Bottom.Equals(other.Bottom) &&
                   Right.Equals(other.Right);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is IntInsets && Equals((IntInsets)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Top.GetHashCode();
                hashCode = (hashCode * 31) ^ Left.GetHashCode();
                hashCode = (hashCode * 31) ^ Bottom.GetHashCode();
                hashCode = (hashCode * 31) ^ Right.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(IntInsets left, IntInsets right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(IntInsets left, IntInsets right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(Top)}: {Top}, {nameof(Left)}: {Left}, {nameof(Bottom)}: {Bottom}, {nameof(Right)}: {Right}";
        }
    }
}

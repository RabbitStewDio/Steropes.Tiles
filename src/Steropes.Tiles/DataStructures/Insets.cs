using System;

namespace Steropes.Tiles.DataStructures
{
    public readonly struct Insets : IEquatable<Insets>
    {
        public double Top { get; }
        public double Left { get; }
        public double Bottom { get; }
        public double Right { get; }

        public Insets(double top, double left, double bottom, double right)
        {
            Top = top;
            Left = left;
            Bottom = bottom;
            Right = right;
        }

        public bool Equals(Insets other)
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

            return obj is Insets && Equals((Insets)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Top.GetHashCode();
                hashCode = (hashCode * 397) ^ Left.GetHashCode();
                hashCode = (hashCode * 397) ^ Bottom.GetHashCode();
                hashCode = (hashCode * 397) ^ Right.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Insets left, Insets right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Insets left, Insets right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(Top)}: {Top}, {nameof(Left)}: {Left}, {nameof(Bottom)}: {Bottom}, {nameof(Right)}: {Right}";
        }
    }
}

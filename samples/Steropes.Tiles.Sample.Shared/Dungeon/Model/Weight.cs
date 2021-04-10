using System;

namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    public struct Weight : IEquatable<Weight>
    {
        public readonly int Value;

        Weight(int value)
        {
            if (value < 0)
            {
                throw new ArgumentException();
            }

            this.Value = value;
        }

        public bool Equals(Weight other)
        {
            return Value == other.Value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Weight && Equals((Weight)obj);
        }

        public override int GetHashCode()
        {
            return Value;
        }

        public static bool operator ==(Weight left, Weight right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Weight left, Weight right)
        {
            return !left.Equals(right);
        }

        public static Weight operator +(Weight left, Weight right)
        {
            return new Weight(left.Value + right.Value);
        }

        public static Weight Kilo(float kg)
        {
            return new Weight((int)Math.Ceiling(kg * 1000));
        }

        public static Weight Gram(int g)
        {
            return new Weight(g);
        }

        public override string ToString()
        {
            return $"Weight({Value})";
        }
    }
}

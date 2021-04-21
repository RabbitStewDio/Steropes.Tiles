using System;
using System.Collections.Generic;

namespace Steropes.Tiles.DataStructures
{
    public readonly struct TupleKey<TFirst, TSecond> : IEquatable<TupleKey<TFirst, TSecond>>
    {
        public TFirst Item1 { get; }
        public TSecond Item2 { get; }

        public TupleKey(TFirst item1, TSecond item2)
        {
            Item1 = item1;
            Item2 = item2;
        }

        public bool Equals(TupleKey<TFirst, TSecond> other)
        {
            return EqualityComparer<TFirst>.Default.Equals(Item1, other.Item1)
                   && EqualityComparer<TSecond>.Default.Equals(Item2, other.Item2);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TupleKey<TFirst, TSecond> key && Equals(key);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (EqualityComparer<TFirst>.Default.GetHashCode(Item1) * 397)
                       ^ EqualityComparer<TSecond>.Default.GetHashCode(Item2);
            }
        }

        public static bool operator ==(TupleKey<TFirst, TSecond> left, TupleKey<TFirst, TSecond> right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TupleKey<TFirst, TSecond> left, TupleKey<TFirst, TSecond> right)
        {
            return !left.Equals(right);
        }
    }
}

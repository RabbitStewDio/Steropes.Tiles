﻿using System;

namespace Steropes.Tiles.DataStructures
{
    public readonly struct Rect : IEquatable<Rect>
    {
        public double X { get; }
        public double Y { get; }

        public Rect(double x, double y, double width, double height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public DoublePoint Offset
        {
            get { return new DoublePoint(X, Y); }
        }

        public double Width { get; }
        public double Height { get; }

        public DoubleDimension Size
        {
            get { return new DoubleDimension(Width, Height); }
        }

        public DoublePoint Center
        {
            get
            {
                return new DoublePoint(X + Width / 2, Y + Height / 2);
            }
        }

        public bool Equals(Rect other)
        {
            return X.Equals(other.X) && Y.Equals(other.Y) && Width.Equals(other.Width) && Height.Equals(other.Height);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Rect && Equals((Rect)obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = X.GetHashCode();
                hashCode = (hashCode * 31) ^ Y.GetHashCode();
                hashCode = (hashCode * 31) ^ Width.GetHashCode();
                hashCode = (hashCode * 31) ^ Height.GetHashCode();
                return hashCode;
            }
        }

        public static bool operator ==(Rect left, Rect right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Rect left, Rect right)
        {
            return !left.Equals(right);
        }

        public override string ToString()
        {
            return $"{nameof(Offset)}: {Offset}, {nameof(Size)}: {Size}";
        }
    }
}

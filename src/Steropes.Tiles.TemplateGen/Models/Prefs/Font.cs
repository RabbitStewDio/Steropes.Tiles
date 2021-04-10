using SkiaSharp;
using System;

namespace Steropes.Tiles.TemplateGen.Models
{
    public readonly struct Font : IEquatable<Font>
    {
        public readonly bool Bold;
        public readonly bool Italic;
        public readonly string? Typeface;
        public readonly float Size;

        public Font(string typeface, float size, bool bold = false, bool italic = false)
        {
            Bold = bold;
            Italic = italic;
            Typeface = typeface;
            Size = size;
        }

        public bool Equals(Font other)
        {
            return string.Equals(Typeface, other.Typeface) && Size.Equals(other.Size);
        }

        public override bool Equals(object? obj)
        {
            return obj is Font other && Equals(other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return ((Typeface?.GetHashCode() ?? 0) * 397) ^ Size.GetHashCode();
            }
        }

        public static bool operator ==(Font left, Font right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Font left, Font right)
        {
            return !left.Equals(right);
        }

        public SKTypeface ToSkia()
        {
            return SKTypeface.FromFamilyName(Typeface, Bold ? SKFontStyleWeight.Bold : SKFontStyleWeight.Normal, SKFontStyleWidth.Normal, Italic ? SKFontStyleSlant.Italic : SKFontStyleSlant.Upright);
        }
    }
}

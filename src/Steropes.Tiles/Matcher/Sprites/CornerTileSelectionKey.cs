using System;
using Steropes.Tiles.Matcher.Registry;

namespace Steropes.Tiles.Matcher.Sprites
{
  public sealed class CornerTileSelectionKey : IEquatable<CornerTileSelectionKey>
  {
    CornerTileSelectionKey(Direction pos, bool m0, bool m1, bool m2)
    {
      this.Pos = pos;
      this.M0 = m0;
      this.M1 = m1;
      this.M2 = m2;
    }

    public bool Equals(CornerTileSelectionKey other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return M0 == other.M0 && M1 == other.M1 && M2 == other.M2 && Pos == other.Pos;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((CornerTileSelectionKey)obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = M0.GetHashCode();
        hashCode = (hashCode * 397) ^ M1.GetHashCode();
        hashCode = (hashCode * 397) ^ M2.GetHashCode();
        hashCode = (hashCode * 397) ^ (int)Pos;
        return hashCode;
      }
    }

    public static bool operator ==(CornerTileSelectionKey left, CornerTileSelectionKey right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(CornerTileSelectionKey left, CornerTileSelectionKey right)
    {
      return !Equals(left, right);
    }

    public bool M0 { get; }

    public bool M1 { get; }

    public bool M2 { get; }

    public Direction Pos { get; }

    public int LinearIndex
    {
      get
      {
        int index = 0;
        index += M0 ? 1 : 0;
        index += M1 ? 2 : 0;
        index += M2 ? 4 : 0;
        return index * (int)Pos;
      }
    }

    public static CornerTileSelectionKey ValueOf(Direction pos, bool m0, bool m1, bool m2)
    {
      int index = 0;
      index += m0 ? 1 : 0;
      index += m1 ? 2 : 0;
      index += m2 ? 4 : 0;
      index *= (int)pos;
      return values[index];
    }

    public static CornerTileSelectionKey[] Values
    {
      get
      {
        var x = new CornerTileSelectionKey[values.Length];
        values.CopyTo(x, 0);
        return x;
      }
    }

    static readonly CornerTileSelectionKey[] values;

    static CornerTileSelectionKey()
    {
      values = new CornerTileSelectionKey[8 * 4];
      foreach (Direction value in Enum.GetValues(typeof(Direction)))
      {
        for (var idx = 0; idx < 8; idx += 1)
        {
          var m0 = (idx & 1) != 0;
          var m1 = (idx & 2) != 0;
          var m2 = (idx & 4) != 0;

          var key = new CornerTileSelectionKey(value, m0, m1, m2);
          values[key.LinearIndex] = key;
        }
      }
    }
  }
}
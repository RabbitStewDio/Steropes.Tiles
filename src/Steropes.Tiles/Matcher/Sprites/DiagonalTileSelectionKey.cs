using System;

namespace Steropes.Tiles.Matcher.Sprites
{
  public sealed class DiagonalTileSelectionKey : IEquatable<DiagonalTileSelectionKey>
  {
    static readonly DiagonalTileSelectionKey[] values;

    static DiagonalTileSelectionKey()
    {
      values = new DiagonalTileSelectionKey[16];
      for (var idx = 0; idx < 16; idx += 1)
      {
        var nw = (idx & 1) != 0;
        var ne = (idx & 2) != 0;
        var se = (idx & 4) != 0;
        var sw = (idx & 8) != 0;
        values[idx] = new DiagonalTileSelectionKey(nw, ne, se, sw);
      }
    }

    DiagonalTileSelectionKey(bool northWest, bool northEast, bool southEast, bool southWest)
    {
      NorthWest = northWest;
      NorthEast = northEast;
      SouthEast = southEast;
      SouthWest = southWest;
    }

    public bool NorthWest { get; }

    public bool NorthEast { get; }

    public bool SouthEast { get; }

    public bool SouthWest { get; }

    public int LinearIndex
    {
      get
      {
        var index = 0;
        index += NorthWest ? 1 : 0;
        index += NorthEast ? 2 : 0;
        index += SouthEast ? 4 : 0;
        index += SouthWest ? 8 : 0;
        return index;
      }
    }

    public static DiagonalTileSelectionKey[] Values
    {
      get
      {
        var x = new DiagonalTileSelectionKey[values.Length];
        values.CopyTo(x, 0);
        return x;
      }
    }

    public bool Equals(DiagonalTileSelectionKey other)
    {
      if (ReferenceEquals(null, other))
      {
        return false;
      }

      if (ReferenceEquals(this, other))
      {
        return true;
      }

      return NorthWest == other.NorthWest &&
             NorthEast == other.NorthEast &&
             SouthEast == other.SouthEast &&
             SouthWest == other.SouthWest;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }

      if (ReferenceEquals(this, obj))
      {
        return true;
      }

      if (obj.GetType() != GetType())
      {
        return false;
      }

      return Equals((DiagonalTileSelectionKey) obj);
    }

    public override int GetHashCode()
    {
      unchecked
      {
        var hashCode = NorthWest.GetHashCode();
        hashCode = (hashCode * 397) ^ NorthEast.GetHashCode();
        hashCode = (hashCode * 397) ^ SouthEast.GetHashCode();
        hashCode = (hashCode * 397) ^ SouthWest.GetHashCode();
        return hashCode;
      }
    }

    public static bool operator ==(DiagonalTileSelectionKey left, DiagonalTileSelectionKey right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(DiagonalTileSelectionKey left, DiagonalTileSelectionKey right)
    {
      return !Equals(left, right);
    }

    public static DiagonalTileSelectionKey ValueOf(bool north, bool east, bool south, bool west)
    {
      var index = 0;
      index += north ? 1 : 0;
      index += east ? 2 : 0;
      index += south ? 4 : 0;
      index += west ? 8 : 0;
      return values[index];
    }
  }
}
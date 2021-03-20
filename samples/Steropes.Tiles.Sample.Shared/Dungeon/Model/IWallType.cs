using System;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
  /// <summary>
  ///   Walls are just that - walls. We only use this as selector for the tiles. If a dungeon
  ///   could have windows, then these definitions would go in here as well (see through,
  ///   pass arrow or magic (straw walls, fire walls)).
  /// </summary>
  public interface IWallType: IEquatable<IWallType>
  {
    WallTypeId Id { get; }

    string Name { get; }

    bool ObstructWalking { get; }
    bool ObstructSight { get; }
  }

  public struct WallTypeId : IEquatable<WallTypeId>
  {
    public bool Equals(WallTypeId other)
    {
      return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }
      return obj is WallTypeId && Equals((WallTypeId) obj);
    }

    public override int GetHashCode()
    {
      return Id;
    }

    public static bool operator ==(WallTypeId left, WallTypeId right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(WallTypeId left, WallTypeId right)
    {
      return !left.Equals(right);
    }

    public int Id { get; }

    public WallTypeId(int id)
    {
      Id = id;
    }

    public static implicit operator WallTypeId(int id)
    {
      return new WallTypeId(id);
    }
  }
}
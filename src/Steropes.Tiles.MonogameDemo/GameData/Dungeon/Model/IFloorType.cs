using System;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model
{
  public interface IFloorType
  {
    FloorTypeId Id { get; }

    /// <summary>
    ///   Naming things makes debugging so much more fun.
    /// </summary>
    string Name { get; }

    /// <summary>
    ///   How quickly can we move across this floor. The system runs in ticks (1/60th of a second)
    ///   and this tells us how many ticks it needs to cross a single tile.
    /// </summary>
    int MovementCost { get; }

    /// <summary>
    ///   Fire anyone? Gives the cost to the health (in deductions per tick) when standing on
    ///   this floor. I am using integer arithmetic and one HP is represented as 1000 units.
    /// </summary>
    int HealthCost { get; }
  }

  public struct FloorTypeId : IEquatable<FloorTypeId>
  {
    public bool Equals(FloorTypeId other)
    {
      return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }
      return obj is FloorTypeId && Equals((FloorTypeId) obj);
    }

    public override int GetHashCode()
    {
      return Id;
    }

    public static bool operator ==(FloorTypeId left, FloorTypeId right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(FloorTypeId left, FloorTypeId right)
    {
      return !left.Equals(right);
    }

    public int Id { get; }

    public FloorTypeId(int id)
    {
      Id = id;
    }

    public static implicit operator FloorTypeId(int id)
    {
      return new FloorTypeId(id);
    }
  }
}
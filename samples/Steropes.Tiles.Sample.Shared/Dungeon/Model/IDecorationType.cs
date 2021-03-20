using System;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
  /// <summary>
  ///   Decorations are immobile items. Players and NPCs can interact with them, but they cannot be moved around.
  ///   Doors and Windows are such usable decorations.
  /// </summary>
  public interface IDecorationType: IEquatable<IDecorationType>
  {
    DecorationTypeId Id { get; }

    string Name { get; }

    /// <summary>
    ///   The amount of light this decoration radiates in percent. We keep thing linear, so this just specifies
    ///   the distance in tiles until the light runs out. Anything over 100% simply illuminates to the maximum
    ///   amount. Given in >1 for full light, 0 for no light.
    /// </summary>
    float LightSource { get; }

    /// <summary>
    ///   Defines whether this decoration blocks light. Again given in percent (0..1).  Holes in the wall provide
    ///   100% visibility, while solid walls block all light. This setting exists mainly for Windows and Doorways.
    /// </summary>
    float BlockVisibility { get; }
  }


  public struct DecorationTypeId : IEquatable<DecorationTypeId>
  {
    public bool Equals(DecorationTypeId other)
    {
      return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj))
      {
        return false;
      }
      return obj is DecorationTypeId && Equals((DecorationTypeId) obj);
    }

    public override int GetHashCode()
    {
      return Id;
    }

    public static bool operator ==(DecorationTypeId left, DecorationTypeId right)
    {
      return left.Equals(right);
    }

    public static bool operator !=(DecorationTypeId left, DecorationTypeId right)
    {
      return !left.Equals(right);
    }

    public int Id { get; }

    public DecorationTypeId(int id)
    {
      Id = id;
    }

    public static implicit operator DecorationTypeId(int id)
    {
      return new DecorationTypeId(id);
    }
  }
}
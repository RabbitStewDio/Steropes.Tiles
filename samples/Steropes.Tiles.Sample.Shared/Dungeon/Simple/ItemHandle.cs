using System;
using Steropes.Tiles.Demo.Core.GameData.Dungeon.Model;

namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Simple
{
  public class ItemHandle: IItem, IEquatable<ItemHandle>
  {
    public ItemHandle(int id, IItemType metadata)
    {
      Id = id;
      ItemType = metadata;
    }

    public int Id { get; }
    public IItemType ItemType { get; }

    public bool Equals(ItemHandle other)
    {
      if (ReferenceEquals(null, other)) return false;
      if (ReferenceEquals(this, other)) return true;
      return Id == other.Id;
    }

    public override bool Equals(object obj)
    {
      if (ReferenceEquals(null, obj)) return false;
      if (ReferenceEquals(this, obj)) return true;
      if (obj.GetType() != this.GetType()) return false;
      return Equals((ItemHandle) obj);
    }

    public override int GetHashCode()
    {
      return Id;
    }

    public static bool operator ==(ItemHandle left, ItemHandle right)
    {
      return Equals(left, right);
    }

    public static bool operator !=(ItemHandle left, ItemHandle right)
    {
      return !Equals(left, right);
    }
  }
}
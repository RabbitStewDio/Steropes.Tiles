using System;
using Steropes.Tiles.MonogameDemo.GameData.Dungeon.Model;

namespace Steropes.Tiles.MonogameDemo.GameData.Dungeon.Simple
{
  public class ItemType : IItemType
  {
    public ItemClass ItemClass { get; }
    public ItemTypeId Id { get; }
    public string Name { get; }
    public int MaxStackSize { get; }
    /// <summary>
    /// Weight is given in gramm increments. This is good enough for even representing a stack of paper accurately.
    /// </summary>
    public Weight Weight { get; }
    public ContainerType Container { get; }

    public ItemType(ItemTypeId id,
                    string name,
                    int maxStackSize,
                    Weight weight,
                    ContainerType container,
                    ItemClass itemClass)
    {
      if (maxStackSize < 1)
      {
        throw new ArgumentException("An item cannot have a zero or negative stack size.");
      }
      if (container != ContainerType.None)
      {
        if (maxStackSize != 1)
        {
          throw new ArgumentException("Container cannot be stacked.");
        }
      }

      Id = id;
      Name = name ?? throw new ArgumentNullException(nameof(name));
      MaxStackSize = maxStackSize;
      Weight = weight;
      Container = container;
      this.ItemClass = itemClass;
    }
  }
}
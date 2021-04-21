using System;

namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///   Represents a single item type. This defines all properties that are true for all items of the same type.
    ///   For instance, the weight of a sword of a given type is always the same, regardless of the current instance
    ///   of the sword. So that weight would be defined once for all swords. The duability of the sword however is
    ///   unique to each instance, and thus would be a instance-level property on the item itself.
    /// </summary>
    public interface IItemType
    {
        ItemTypeId Id { get; }

        string Name { get; }

        /// <summary>
        ///   Returns whether the items can be stacked. Should never return numbers of less than 1.
        /// </summary>
        int MaxStackSize { get; }

        /// <summary>
        ///   Items should have a weight. This controls how much an character can carry.
        /// </summary>
        Weight Weight { get; }

        /// <summary>
        ///   Gives an indication on what container type this item is.
        /// </summary>
        ContainerType Container { get; }

        /// <summary>
        ///   Gives a hint on which traits the item type posses. This normally directly translates into
        ///   item traits.
        /// </summary>
        ItemClass ItemClass { get; }
    }

    public struct ItemTypeId : IEquatable<ItemTypeId>
    {
        public bool Equals(ItemTypeId other)
        {
            return Id == other.Id;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj))
            {
                return false;
            }

            return obj is ItemTypeId && Equals((ItemTypeId)obj);
        }

        public override int GetHashCode()
        {
            return Id;
        }

        public static bool operator ==(ItemTypeId left, ItemTypeId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ItemTypeId left, ItemTypeId right)
        {
            return !left.Equals(right);
        }

        public int Id { get; }

        public ItemTypeId(int id)
        {
            Id = id;
        }

        public static implicit operator ItemTypeId(int id)
        {
            return new ItemTypeId(id);
        }
    }
}

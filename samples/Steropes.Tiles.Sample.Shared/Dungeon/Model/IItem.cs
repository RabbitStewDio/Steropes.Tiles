namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///   Simple data handle to represent an item.
    /// </summary>
    public interface IItem
    {
        /// <summary>
        ///   A system wide unique id for this item.
        /// </summary>
        int Id { get; }

        /// <summary>
        ///   Item metadata that applies to all items.
        /// </summary>
        IItemType ItemType { get; }
    }
}

namespace Steropes.Tiles.Sample.Shared.Dungeon.Model
{
    /// <summary>
    ///   Defines the stacking and size class of the various containers. A container item of a
    ///   given type can only contain items of a lesser type. This avoids infinite recursive
    ///   stacking (or stacking of chests into NPCs who then get stashed into a bag. This is
    ///   not east-london after all).
    /// </summary>
    public enum ContainerType
    {
        /// <summary>
        ///   Cannot contain any other items, not a container.
        /// </summary>
        None = 0,

        /// <summary>
        ///   A bag can contain non-container items.
        /// </summary>
        Bag = 1,

        /// <summary>
        ///   Chests can contain bags and non-container items.
        /// </summary>
        Chest = 2,

        /// <summary>
        ///   Characters can carry chests, if they are strong enough.
        /// </summary>
        Character = 3,

        /// <summary>
        ///   This item has no restrictions on its storage capabilities. This
        ///   should not be assigned to normal items, but is used to access
        ///   the virtual container collection that makes up a map-cell.
        /// </summary>
        Unlimited = 4
    }
}

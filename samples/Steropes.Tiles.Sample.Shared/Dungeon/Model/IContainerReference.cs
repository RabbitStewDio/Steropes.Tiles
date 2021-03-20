namespace Steropes.Tiles.Demo.Core.GameData.Dungeon.Model
{
    /// <summary>
    ///   A container reference. There are three entities that can be container:
    ///   - A map coordinate
    ///   - A character (PC or NPC or monster)
    ///   - An item (chest, bags etc)
    /// </summary>
    /// A container reference only needs to compare for equality.
    public interface IContainerReference
    {
        ContainerReferenceType ReferenceType { get; }
        ContainerType ContainerType { get; }
    }

    public enum ContainerReferenceType
    {
        Map,
        Item,
        Character
    }
}

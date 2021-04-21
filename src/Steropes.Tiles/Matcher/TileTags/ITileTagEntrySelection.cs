namespace Steropes.Tiles.Matcher.TileTags
{
    /// <summary>
    ///  Selection entries for cellmap and corner matchers. Create instances of this
    ///  interface via the CellEntrySelectionFactory.
    /// </summary>
    public interface ITileTagEntrySelection
    {
        /// <summary>
        ///  Position within the factory.
        /// </summary>
        ushort Index { get; }

        /// <summary>
        ///  Text representation of the tag used for a tile lookup.
        ///  This property exists for documentation and ease of debugging.
        /// </summary>
        string Tag { get; }

        /// <summary>
        ///  Number of possible entries in the set of tile tag entries.
        /// </summary>
        int Cardinality { get; }
    }

    public interface ITileTagEntrySelection<TSelector> : ITileTagEntrySelection
    {
        TSelector Selector { get; }
        ITileTagEntrySelectionFactory<TSelector> Owner { get; }
    }
}

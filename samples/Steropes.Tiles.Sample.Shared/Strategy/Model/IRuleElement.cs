using System.Collections.Generic;

namespace Steropes.Tiles.Sample.Shared.Strategy.Model
{
    public interface IRuleElement
    {
        /// <summary>
        ///   The name of the terrain type, used for documentation and in the UI.
        /// </summary>
        string Name { get; }

        /// <summary>
        ///   The name of the graphical tag. This maps the terrain type to the tile set.
        ///   This is slightly different from the name property, as multiple terrains could
        ///   map to the same graphical representation.
        /// </summary>
        string GraphicTag { get; }

        /// <summary>
        ///   An alternative graphic tag if the first one is not found in the tile set.
        ///   Entries are tried in order of apperance until the tags match.
        ///   This allows you to evolve the tile set over time, simply move the old tags
        ///   into this list when you change the primary tag.
        /// </summary>
        IReadOnlyList<string> AlternativeGraphicTags { get; }

        /// <summary>
        ///   The identifier is used when loading and saving the map. It must be unique
        ///   across all terrain tiles.
        ///   <p />
        ///   We use freeciv's
        ///   idea of saving maps as text blocks. This makes it incredible easy to
        ///   debug and test stuff (as maps can be expressed as strings in unittests).
        /// </summary>
        char AsciiId { get; }

        /// <summary>
        ///   A unique, internal identifier.
        /// </summary>
        string Id { get; }
    }
}

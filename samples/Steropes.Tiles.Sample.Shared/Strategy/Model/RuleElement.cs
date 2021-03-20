using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Model
{
    public abstract class RuleElement
    {
        protected RuleElement(string id,
                              char asciiId,
                              string name,
                              string graphicTag,
                              params string[] alternativeGraphicTags)
        {
            Id = id ?? throw new ArgumentNullException(nameof(id));
            AsciiId = asciiId;
            Name = name ?? throw new ArgumentNullException(nameof(name));
            GraphicTag = graphicTag;
            AlternativeGraphicTags = ((IReadOnlyList<string>)alternativeGraphicTags?.Clone()) ?? throw new ArgumentNullException(nameof(alternativeGraphicTags));
        }

        /// <summary>
        ///   The name of the terrain type, used for documentation and in the UI.
        /// </summary>
        public string Name { get; }

        /// <summary>
        ///   The name of the graphical tag. This maps the terrain type to the tile set.
        ///   This is slightly different from the name property, as multiple terrains could
        ///   map to the same graphical representation.
        /// </summary>
        public string GraphicTag { get; }

        /// <summary>
        ///   An alternative graphic tag if the first one is not found in the tile set.
        ///   Entries are tried in order of apperance until the tags match.
        ///   This allows you to evolve the tile set over time, simply move the old tags
        ///   into this list when you change the primary tag.
        /// </summary>
        public IReadOnlyList<string> AlternativeGraphicTags { get; }

        /// <summary>
        ///   The identifier is used when loading and saving the map. It must be unique
        ///   across all terrain tiles.
        ///   <p />
        ///   We use freeciv's
        ///   idea of saving maps as text blocks. This makes it incredible easy to
        ///   debug and test stuff (as maps can be expressed as strings in unittests).
        /// </summary>
        public char AsciiId { get; }

        /// <summary>
        ///   A unique, internal identifier.
        /// </summary>
        public string Id { get; }
    }
}

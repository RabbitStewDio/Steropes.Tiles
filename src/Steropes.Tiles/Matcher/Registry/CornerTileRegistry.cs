using System;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
    public class CornerTileRegistry<TRenderTile> : ITileRegistryEx<CornerTileSelectionKey, TRenderTile>
    {
        readonly ITileRegistry<TRenderTile> baseRegistry;
        readonly string[] suffixMapping;

        [Obsolete]
        public CornerTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                  ITileTagEntrySelectionFactory<bool> booleanFormats,
                                  ITileTagEntrySelectionFactory<Direction> directions,
                                  string suffixFormat,
                                  string format) : this(baseRegistry, booleanFormats, directions,
                                                        ProduceCombinedFormatString(suffixFormat, format))
        { }

        static string ProduceCombinedFormatString(string tagFormat = null,
                                                  string format = null)
        {
            var formatString = tagFormat ?? "{0}{1}{2}{3}";
            var secondLevel = format ?? "{0}_{1}";
            return string.Format(secondLevel, "{{0}}", formatString);
        }

        public CornerTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                  ITileTagEntrySelectionFactory<bool> booleanFormats = null,
                                  ITileTagEntrySelectionFactory<Direction> directions = null,
                                  string format = null)
        {
            this.baseRegistry = baseRegistry;
            this.suffixMapping = GenerateSuffixes(booleanFormats ?? TileTagEntries.CreateFlagTagEntries(),
                                                  directions ?? TileTagEntries.CreateDirectionTagEntries(),
                                                  format ?? "{{0}}_{0}{1}{2}{3}");
        }

        string[] GenerateSuffixes(ITileTagEntrySelectionFactory<bool> booleanFormats,
                                  ITileTagEntrySelectionFactory<Direction> directions,
                                  string suffixFormat)
        {
            var keys = CornerTileSelectionKey.Values;
            var retval = new string[keys.Length];
            for (var i = 0; i < keys.Length; i++)
            {
                var key = keys[i];
                var m0 = booleanFormats.Lookup(key.M0).Tag;
                var m1 = booleanFormats.Lookup(key.M1).Tag;
                var m2 = booleanFormats.Lookup(key.M2).Tag;
                var p = directions.Lookup(key.Pos).Tag;
                retval[i] = string.Format(suffixFormat, p, m0, m1, m2);
            }

            return retval;
        }

        public bool TryFind(string tag, CornerTileSelectionKey selector, out TRenderTile tile)
        {
            var format = suffixMapping[selector.LinearIndex];
            return baseRegistry.TryFind(string.Format(format, tag), out tile);
        }
    }
}

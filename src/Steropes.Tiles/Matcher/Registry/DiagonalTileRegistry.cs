using System;
using System.Linq;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
    public class DiagonalTileRegistry<TRenderTile> : ITileRegistryEx<DiagonalTileSelectionKey, TRenderTile>
    {
        readonly ITileRegistry<TRenderTile> baseRegistry;
        readonly string[] suffixMapping;

        [Obsolete]
        public DiagonalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                    ITileTagEntrySelectionFactory<bool> tags,
                                    string tagFormat,
                                    string format) : this(baseRegistry, tags, ProduceCombinedFormatString(tagFormat, format))
        { }

        static string ProduceCombinedFormatString(string tagFormat = null,
                                                  string format = null)
        {
            var formatString = tagFormat ?? "nw{0}ne{1}se{2}sw{3}";
            var secondLevel = format ?? "{0}_{1}";
            return string.Format(secondLevel, "{{0}}", formatString);
        }

        public DiagonalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                    ITileTagEntrySelectionFactory<bool> tags = null,
                                    string tagFormat = null)
        {
            this.baseRegistry = baseRegistry ?? throw new ArgumentNullException(nameof(baseRegistry));
            var tagProvider = tags ?? TileTagEntries.CreateFlagTagEntries();
            var formatString = tagFormat ?? "{{0}}_nw{0}ne{1}se{2}sw{3}";
            this.suffixMapping = DiagonalTileSelectionKey.Values.Select(e => Format(e, tagProvider, formatString)).ToArray();
        }

        string Format(DiagonalTileSelectionKey k,
                      ITileTagEntrySelectionFactory<bool> formatProvider,
                      string tagFormat)
        {
            var n = formatProvider.Lookup(k.NorthWest).Tag;
            var e = formatProvider.Lookup(k.NorthEast).Tag;
            var s = formatProvider.Lookup(k.SouthEast).Tag;
            var w = formatProvider.Lookup(k.SouthWest).Tag;
            return string.Format(tagFormat, n, e, s, w);
        }

        public bool TryFind(string tag, DiagonalTileSelectionKey selector, out TRenderTile tile)
        {
            var format = suffixMapping[selector.LinearIndex];
            return baseRegistry.TryFind(string.Format(format, tag), out tile);
        }
    }
}

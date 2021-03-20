using System;
using System.Linq;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
    public class CardinalTileRegistry<TRenderTile> : ITileRegistryEx<CardinalTileSelectorKey, TRenderTile>
    {
        readonly ITileRegistry<TRenderTile> baseRegistry;
        readonly string[] suffixMapping;

        [Obsolete]
        public CardinalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                    ITileTagEntrySelectionFactory<bool> tags,
                                    string tagFormat,
                                    string format) : this(baseRegistry, tags, ProduceCombinedFormatString(tagFormat, format))
        { }

        static string ProduceCombinedFormatString(string tagFormat = null,
                                                  string format = null)
        {
            var formatString = tagFormat ?? "n{0}e{1}s{2}w{3}";
            var secondLevel = format ?? "{0}_{1}";
            return string.Format(secondLevel, "{{0}}", formatString);
        }

        public CardinalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                    ITileTagEntrySelectionFactory<bool> tags = null,
                                    string tagFormat = null)
        {
            this.baseRegistry = baseRegistry ?? throw new ArgumentNullException(nameof(baseRegistry));
            this.suffixMapping = CardinalTileSelectorKey.Values.Select(e => Format(e, tags, tagFormat)).ToArray();
        }

        public bool TryFind(string tag, CardinalTileSelectorKey selector, out TRenderTile tile)
        {
            var format = suffixMapping[selector.LinearIndex];
            return baseRegistry.TryFind(string.Format(format, tag), out tile);
        }

        static string Format(CardinalTileSelectorKey k,
                             ITileTagEntrySelectionFactory<bool> formatProvider = null,
                             string format = null)
        {
            formatProvider = formatProvider ?? TileTagEntries.CreateFlagTagEntries();
            format = format ?? "{{0}}_n{0}e{1}s{2}w{3}";
            var n = formatProvider.Lookup(k.North).Tag;
            var e = formatProvider.Lookup(k.East).Tag;
            var s = formatProvider.Lookup(k.South).Tag;
            var w = formatProvider.Lookup(k.West).Tag;
            return string.Format(format, n, e, s, w);
        }
    }

    public static class CardinalTileRegistry
    {
        [Obsolete]
        public static string Format(CardinalTileSelectorKey k,
                                    ITileTagEntrySelectionFactory<bool> formatProvider = null,
                                    string format = null)
        {
            formatProvider = formatProvider ?? TileTagEntries.CreateFlagTagEntries();
            format = format ?? "n{0}e{1}s{2}w{3}";
            var n = formatProvider.Lookup(k.North).Tag;
            var e = formatProvider.Lookup(k.East).Tag;
            var s = formatProvider.Lookup(k.South).Tag;
            var w = formatProvider.Lookup(k.West).Tag;
            return string.Format(format, n, e, s, w);
        }
    }
}

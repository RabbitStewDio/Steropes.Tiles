using System;
using System.Linq;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
  public class CardinalTileRegistry<TRenderTile> : ITileRegistryEx<CardinalTileSelectorKey, TRenderTile>
  {
    readonly ITileRegistry<TRenderTile> baseRegistry;
    readonly string format;
    readonly string[] suffixMapping;

    public CardinalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                ITileTagEntrySelectionFactory<bool> tags = null,
                                string tagFormat = null, 
                                string format = null)
    {
      this.baseRegistry = baseRegistry ?? throw new ArgumentNullException(nameof(baseRegistry));
      this.format = format ?? "{0}_{1}";
      this.suffixMapping = CardinalTileSelectorKey.Values.Select(e => CardinalTileRegistry.Format(e, tags, tagFormat)).ToArray();
    }

    public TRenderTile Find(string tag, CardinalTileSelectorKey selector)
    {
      return baseRegistry.Find(string.Format(format, tag, suffixMapping[selector.LinearIndex]));
    }

    public bool TryFind(string tag, CardinalTileSelectorKey selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(string.Format(format, tag, suffixMapping[selector.LinearIndex]), out tile);
    }
  }

  public static class CardinalTileRegistry
  {
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
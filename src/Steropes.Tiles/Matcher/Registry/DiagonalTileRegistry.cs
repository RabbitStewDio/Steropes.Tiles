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
    readonly string format;

    public DiagonalTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                ITileTagEntrySelectionFactory<bool> tags = null,
                                string tagFormat = null,
                                string format = null)
    {
      this.baseRegistry = baseRegistry ?? throw new ArgumentNullException(nameof(baseRegistry));
      var tagProvider = tags ?? TileTagEntries.CreateFlagTagEntries();
      var formatString = tagFormat ?? "nw{0}ne{1}se{2}sw{3}";
      this.format = format ?? "{0}_{1}";
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

    public TRenderTile Find(string tag, DiagonalTileSelectionKey selector)
    {
      return baseRegistry.Find(string.Format(format, tag, suffixMapping[selector.LinearIndex]));
    }

    public bool TryFind(string tag, DiagonalTileSelectionKey selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(string.Format(format, tag, suffixMapping[selector.LinearIndex]), out tile);
    }
  }
}
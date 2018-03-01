using System;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Registry
{
  public class CardinalIndexTileRegistry<TRenderTile> : ITileRegistryEx<CardinalIndex, TRenderTile>
  {
    readonly ITileRegistry<TRenderTile> baseRegistry;
    readonly ITileTagEntrySelectionFactory<CardinalIndex> suffixMapping;
    readonly string format;

    public CardinalIndexTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                     ITileTagEntrySelectionFactory<CardinalIndex> suffixMapping,
                                     string format = null)
    {
      this.format = format ?? "{0}_{1}";
      this.baseRegistry = baseRegistry ?? throw new ArgumentNullException();
      this.suffixMapping = suffixMapping ?? throw new ArgumentNullException();
      if (suffixMapping.Count != 4)
      {
        throw new ArgumentException();
      }
    }

    public static CardinalIndexTileRegistry<TRenderTile> CreateLong(ITileRegistry<TRenderTile> baseRegistry)
    {
      return new CardinalIndexTileRegistry<TRenderTile>(baseRegistry, TileTagEntries.CreateCardinalIndexTagEntries());
    }

    public static CardinalIndexTileRegistry<TRenderTile> CreateShort(ITileRegistry<TRenderTile> baseRegistry)
    {
      return new CardinalIndexTileRegistry<TRenderTile>(baseRegistry,
                                                        TileTagEntries.CreateShortCardinalIndexTagEntries());
    }

    public TRenderTile Find(string tag, CardinalIndex selector)
    {
      return baseRegistry.Find(string.Format(format, tag, suffixMapping.Lookup(selector).Tag));
    }

    public bool TryFind(string tag, CardinalIndex selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(string.Format(format, tag, suffixMapping.Lookup(selector).Tag), out tile);
    }
  }
}
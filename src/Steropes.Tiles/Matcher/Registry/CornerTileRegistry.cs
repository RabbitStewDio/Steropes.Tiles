using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
  public class CornerTileRegistry<TRenderTile> : ITileRegistryEx<CornerTileSelectionKey, TRenderTile>
  {
    readonly ITileRegistry<TRenderTile> baseRegistry;
    readonly string format;
    readonly string[] suffixMapping;

    public CornerTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                              ITileTagEntrySelectionFactory<bool> booleanFormats = null,
                              ITileTagEntrySelectionFactory<Direction> directions = null,
                              string suffixFormat = null,
                              string format = null)
    {
      this.baseRegistry = baseRegistry;
      this.format = format ?? "{0}_{1}";
      this.suffixMapping = GenerateSuffixes(booleanFormats ?? TileTagEntries.CreateFlagTagEntries(),
                                            directions ?? TileTagEntries.CreateDirectionTagEntries(),
                                            suffixFormat ?? "{0}{1}{2}{3}");
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
        var p = directions.Lookup(key.Pos);
        retval[i] = string.Format(suffixFormat, p, m0, m1, m2);
      }

      return retval;
    }

    public TRenderTile Find(string tag, CornerTileSelectionKey selector)
    {
      return baseRegistry.Find(string.Format(format, tag, suffixMapping[selector.LinearIndex]));
    }

    public bool TryFind(string tag, CornerTileSelectionKey selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(string.Format(format, tag, suffixMapping[selector.LinearIndex]), out tile);
    }
  }
}
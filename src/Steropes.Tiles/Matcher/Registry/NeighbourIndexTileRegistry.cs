using System;
using System.Collections.Generic;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Registry
{
  /// <summary>
  ///   A tile registry that qualifies a base graphic tag with a directional indicator.
  ///   <para />
  ///   <list type="table">
  ///     <listheader>
  ///       <term>Direction</term>
  ///       <description>Tag</description>
  ///     </listheader>
  ///     <item>
  ///       <term>NeighbourMatchPosition.Isolated</term>
  ///       <description>{prefix}_isolated</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.North</term>
  ///       <description>{prefix}_north</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.NorthEast</term>
  ///       <description>{prefix}_northeast</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.East</term>
  ///       <description>{prefix}_east</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.SouthEast</term>
  ///       <description>{prefix}_southeast</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.South</term>
  ///       <description>{prefix}_south</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.SouthWest</term>
  ///       <description>{prefix}_southwest</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.West</term>
  ///       <description>{prefix}_west</description>
  ///     </item>
  ///     <item>
  ///       <term>NeighbourMatchPosition.NorthWest</term>
  ///       <description>{prefix}_northwest</description>
  ///     </item>
  ///   </list>
  /// </summary>
  /// <typeparam name="TRenderTile">The target type.</typeparam>
  public class NeighbourIndexTileRegistry<TRenderTile> : ITileRegistryEx<NeighbourMatchPosition, TRenderTile>
  {
    readonly ITileRegistry<TRenderTile> baseRegistry;
    readonly string format;
    readonly ITileTagEntrySelectionFactory<NeighbourMatchPosition> suffixMapping;

    public NeighbourIndexTileRegistry(ITileRegistry<TRenderTile> baseRegistry,
                                      ITileTagEntrySelectionFactory<NeighbourMatchPosition> suffixMapping,
                                      string format = null)
    {
      this.format = format ?? "{0}_{1}";
      this.baseRegistry = baseRegistry ?? throw new ArgumentNullException();
      this.suffixMapping = suffixMapping ?? throw new ArgumentNullException();
      if (suffixMapping.Count != 9)
      {
        throw new ArgumentException();
      }
    }

    public TRenderTile Find(string tag, NeighbourMatchPosition selector)
    {
      return baseRegistry.Find(string.Format(format, tag, suffixMapping.Lookup(selector).Tag));
    }

    public bool TryFind(string tag, NeighbourMatchPosition selector, out TRenderTile tile)
    {
      return baseRegistry.TryFind(string.Format(format, tag, suffixMapping.Lookup(selector).Tag), out tile);
    }

    public static NeighbourIndexTileRegistry<TRenderTile> CreateLong(ITileRegistry<TRenderTile> baseRegistry)
    {
      return new NeighbourIndexTileRegistry<TRenderTile>(baseRegistry, LongSelector());
    }

    public static NeighbourIndexTileRegistry<TRenderTile> CreateShort(ITileRegistry<TRenderTile> baseRegistry)
    {
      return new NeighbourIndexTileRegistry<TRenderTile>(baseRegistry, ShortSelector());
    }

    public static ITileTagEntrySelectionFactory<NeighbourMatchPosition> LongSelector()
    {
      return new TileTagEntrySelectionFactory<NeighbourMatchPosition>(
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.Isolated, "isolated"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.North, "north"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.NorthEast, "northeast"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.East, "east"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.SouthEast, "southeast"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.South, "south"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.SouthWest, "southwest"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.West, "west"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.NorthWest, "northwest")
      );
    }

    public static ITileTagEntrySelectionFactory<NeighbourMatchPosition> ShortSelector()
    {
      return new TileTagEntrySelectionFactory<NeighbourMatchPosition>(
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.Isolated, "isolated"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.North, "n"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.NorthEast, "ne"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.East, "e"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.SouthEast, "se"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.South, "s"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.SouthWest, "sw"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.West, "w"),
        new KeyValuePair<NeighbourMatchPosition, string>(NeighbourMatchPosition.NorthWest, "nw")
      );
    }
  }
}
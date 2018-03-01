using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  /// <summary>
  /// <para>
  ///   Matches a tile with possible neighbouring tiles along the cardinal direction. 
  ///   This produces 16 different match combinations to choose the right tile. 
  ///  </para>
  /// <para>
  ///   The selector uses two different matchers. The self-matcher is a strict match 
  ///   selecting the tile type to be rendered. The neighbour-matchers are used to
  ///   decide whether this strictly matched tile connects with any of the neigbouring
  ///   tiles.
  /// </para>
  /// <para>
  ///   Example: When matching rivers, the a river tile should only be rendered when the map
  ///   tile in question is a river. However, rivers flow into the ocean, so the river
  ///   represented by this tile should treat oceans as connectable tiles, in the same
  ///   way a neighbouring river tile would be connected.
  /// </para>
  /// </summary>
  /// <typeparam name="TRenderTile">The target tile type as provided by the tile registry.</typeparam>
  /// <typeparam name="TContext"></typeparam>
  public class CardinalTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
  {
    readonly GridMatcher selfMatcher;
    readonly TRenderTile[] tags;
    protected MapCoordinate[] Coordinates;

    public CardinalTileSelector(GridMatcher matcher,
                                GridMatcher selfMatcher,
                                IMapNavigator<GridDirection> gridNavigator,
                                ITileRegistryEx<CardinalTileSelectorKey, TRenderTile> registry,
                                string prefix,
                                Func<int, int, TContext> contextProvider = null) : 
      base(matcher, gridNavigator, contextProvider)
    {
      this.selfMatcher = selfMatcher ?? throw new ArgumentNullException(nameof(selfMatcher));
      tags = FillInTags(prefix, registry);
    }

    public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      if (!selfMatcher(x, y))
      {
        return false;
      }

      Coordinates = GridNavigator.NavigateCardinalNeighbours(new MapCoordinate(x, y), Coordinates);
      int idx = 0;
      idx += MatchAsFlag(Coordinates[CardinalIndex.North.AsInt()]) << 0;
      idx += MatchAsFlag(Coordinates[CardinalIndex.East.AsInt()]) << 1;
      idx += MatchAsFlag(Coordinates[CardinalIndex.South.AsInt()]) << 2;
      idx += MatchAsFlag(Coordinates[CardinalIndex.West.AsInt()]) << 3;

      resultCollector(SpritePosition.Whole, tags[idx], ContextProvider(x, y));
      return true;
    }

    protected TRenderTile[] FillInTags(string prefix,
                                       ITileRegistryEx<CardinalTileSelectorKey, TRenderTile> registry)
    {
      var preparedTags = new TRenderTile[16];
      for (var idx = 0; idx < 16; idx += 1)
      {
        var n = (idx & 1) != 0;
        var e = (idx & 2) != 0;
        var s = (idx & 4) != 0;
        var w = (idx & 8) != 0;
        preparedTags[idx] = registry.Find(prefix, CardinalTileSelectorKey.ValueOf(n, e, s, w));
      }
      return preparedTags;
    }
  }
}
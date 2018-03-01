using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  public class DiagonalTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
  {
    readonly GridMatcher selfMatcher;
    readonly TRenderTile[] tags;
    MapCoordinate[] neighbours;

    public DiagonalTileSelector(GridMatcher matcher,
                                GridMatcher selfMatcher,
                                IMapNavigator<GridDirection> gridNavigator,
                                ITileRegistryEx<DiagonalTileSelectionKey, TRenderTile> registry,
                                string prefix,
                                Func<int, int, TContext> contextProvider = null)
      : base(matcher, gridNavigator, contextProvider)
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

      neighbours = GridNavigator.NavigateNeighbours(new MapCoordinate(x, y), neighbours);
      var idx = MatchAsFlag(neighbours[NeighbourIndex.NorthEast.AsInt()]) << 0;
      idx += MatchAsFlag(neighbours[NeighbourIndex.SouthEast.AsInt()]) << 1;
      idx += MatchAsFlag(neighbours[NeighbourIndex.SouthWest.AsInt()]) << 2;
      idx += MatchAsFlag(neighbours[NeighbourIndex.NorthWest.AsInt()]) << 4;

      resultCollector(SpritePosition.Whole, tags[idx], ContextProvider(x, y));
      return true;
    }

    TRenderTile[] FillInTags(string prefix, ITileRegistryEx<DiagonalTileSelectionKey, TRenderTile> registry)
    {
      var tiles = new TRenderTile[16];
      for (var idx = 0; idx < 16; idx += 1)
      {
        var n = (idx & 1) != 0;
        var e = (idx & 2) != 0;
        var s = (idx & 4) != 0;
        var w = (idx & 8) != 0;
        var key = DiagonalTileSelectionKey.ValueOf(n, e, s, w);
        tiles[idx] = registry.Find(prefix, key);
      }
      return tiles;
    }
  }
}
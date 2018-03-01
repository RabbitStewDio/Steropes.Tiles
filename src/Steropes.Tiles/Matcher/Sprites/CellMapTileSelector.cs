using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  /// <summary>
  ///   Cell map tile rendering uses a pre-rendered blended tile that spans
  ///   four adjacent cells . Less technical, it is a tile that is rendered
  ///   so that its centre-point is rendered over the point where the 4 neighbouring
  ///   cells meet. To cover a full tile, this process must be repeated for all
  ///   four corners.
  /// </summary>
  public class CellMapTileSelector<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
  {
    readonly ICellMatcher matchers;
    readonly IMapNavigator<GridDirection> gridNavigator;
    readonly ITileRegistryEx<CellMapTileSelectorKey, TRenderTile> registry;
    readonly string prefix;
    readonly Func<int, int, TContext> contextProvider;
    readonly TRenderTile[] tiles;

    public CellMapTileSelector(ICellMatcher matchers,
                               IMapNavigator<GridDirection> gridNavigator,
                               ITileRegistryEx<CellMapTileSelectorKey, TRenderTile> registry,
                               string prefix,
                               Func<int, int, TContext> contextProvider = null)
    {
      this.matchers = matchers ?? throw new ArgumentNullException();
      this.gridNavigator = gridNavigator;
      this.registry = registry;
      this.prefix = prefix;
      this.contextProvider = contextProvider ?? DefaultContextProvider;
      this.tiles = Prepare();
    }

    TRenderTile[] Prepare()
    {
      var owner = matchers.Owner;
      var card = owner.Count;
      var elements = (int) Math.Pow(card, 4);
      var result = new TRenderTile[elements];
      for (int a = 0; a < card; a += 1)
      {
        for (int b = 0; b < card; b += 1)
        {
          for (int c = 0; c < card; c += 1)
          {
            for (int d = 0; d < card; d += 1)
            {
              var key = new CellMapTileSelectorKey(owner[a], owner[b], owner[c], owner[d]);
              result[key.LinearIndex] = registry.Find(prefix, key);
            }
          }
        }
      }

      return result;
    }

    static TContext DefaultContextProvider(int x, int y)
    {
      return default(TContext);
    }

    void NavigateForUpwardRenderDirection(int x,
                                          int y,
                                          out MapCoordinate coordA,
                                          out MapCoordinate coordB,
                                          out MapCoordinate coordC,
                                          out MapCoordinate coordD)
    {
      //
      //       /\
      //      /A \
      //     /\ 2/\
      //    / D\/ B \
      //    \1 /\ 3 /
      //     \/ C\/
      //      \  /
      //       \/
      //
      // tile_cell_A_B_C_D
      // self is at C

      coordC = new MapCoordinate(x, y);
      gridNavigator.NavigateTo(GridDirection.North, coordC, out coordB);
      gridNavigator.NavigateTo(GridDirection.NorthWest, coordC, out coordA);
      gridNavigator.NavigateTo(GridDirection.West, coordC, out coordD);
    }

    void NavigateForDownwardRenderDirection(int x,
                                            int y,
                                            out MapCoordinate coordA,
                                            out MapCoordinate coordB,
                                            out MapCoordinate coordC,
                                            out MapCoordinate coordD)
    {
      //
      //       /\
      //      /A \
      //     /\ 2/\
      //    / D\/ B \
      //    \1 /\ 3 /
      //     \/ C\/
      //      \  /
      //       \/
      //
      // self is A
      coordA = new MapCoordinate(x, y);
      gridNavigator.NavigateTo(GridDirection.South, coordA, out coordD);
      gridNavigator.NavigateTo(GridDirection.East, coordA, out coordB);
      gridNavigator.NavigateTo(GridDirection.SouthEast, coordA, out coordC);
    }


    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      NavigateForDownwardRenderDirection(x, y, out MapCoordinate coordA,
                                         out MapCoordinate coordB,
                                         out MapCoordinate coordC,
                                         out MapCoordinate coordD);

      if (matchers.Match(coordA.X, coordA.Y, out ITileTagEntrySelection matchA) &&
          matchers.Match(coordB.X, coordB.Y, out ITileTagEntrySelection matchB) &&
          matchers.Match(coordC.X, coordC.Y, out ITileTagEntrySelection matchC) &&
          matchers.Match(coordD.X, coordD.Y, out ITileTagEntrySelection matchD))
      {
        var tile = tiles[CellMapTileSelectorKey.LinearIndexOf(matchA, matchB, matchC, matchD)];
        resultCollector(SpritePosition.CellMap, tile, contextProvider(x, y));
        return true;
      }

      return false;
    }
  }
}
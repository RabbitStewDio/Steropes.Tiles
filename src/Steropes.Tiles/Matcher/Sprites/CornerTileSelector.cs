using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  public class CornerTileSelector<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
  {
    readonly SelectorDefinition[] selectors;
    readonly GridMatcher selfMatcher;
    readonly Func<int, int, TContext> contextProvider;
    MapCoordinate[] neighbours;

    public CornerTileSelector(GridMatcher matcher,
                              GridMatcher selfMatcher,
                              IMapNavigator<GridDirection> gridNavigator,
                              ITileRegistryEx<CornerTileSelectionKey, TRenderTile> registry,
                              string prefix,
                              Func<int, int, TContext> contextProvider = null)
    {
      this.selfMatcher = selfMatcher ?? throw new ArgumentNullException(nameof(selfMatcher));
      Matcher = matcher;
      GridNavigator = gridNavigator;
      this.contextProvider = contextProvider ?? DefaultContextProvider;
      selectors = PrepareSelectors(prefix, registry);
    }

    static TContext DefaultContextProvider(int x, int y)
    {
      return default(TContext);
    }

    public GridMatcher Matcher { get; }

    public IMapNavigator<GridDirection> GridNavigator { get; }

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> tile)
    {
      if (!selfMatcher(x, y))
      {
        return false;
      }

      neighbours = GridNavigator.NavigateNeighbours(new MapCoordinate(x, y), neighbours);
      for (var index = 0; index < selectors.Length; index++)
      {
        var d = selectors[index];
        var idx = 0;
        idx += MatchAsFlag(neighbours[d.SelectorCoordinates[0]]) << 0;
        idx += MatchAsFlag(neighbours[d.SelectorCoordinates[1]]) << 1;
        idx += MatchAsFlag(neighbours[d.SelectorCoordinates[2]]) << 2;

        var tileName = d.AsTileName(idx);
        tile(d.Position.MapToPosition(), tileName, contextProvider(x,y));
      }
      return true;
    }

    SelectorDefinition[] PrepareSelectors(string prefix,
                                          ITileRegistryEx<CornerTileSelectionKey, TRenderTile> registry)
    {
      return new[]
      {
        new SelectorDefinition(prefix,
                               Direction.Up,
                               registry,
                               NeighbourIndex.West.AsInt(),
                               NeighbourIndex.NorthWest.AsInt(),
                               NeighbourIndex.North.AsInt()),
        new SelectorDefinition(prefix,
                               Direction.Right,
                               registry,
                               NeighbourIndex.North.AsInt(),
                               NeighbourIndex.NorthEast.AsInt(),
                               NeighbourIndex.East.AsInt()),
        new SelectorDefinition(prefix,
                               Direction.Down,
                               registry,
                               NeighbourIndex.East.AsInt(),
                               NeighbourIndex.SouthEast.AsInt(),
                               NeighbourIndex.South.AsInt()),
        new SelectorDefinition(prefix,
                               Direction.Left,
                               registry,
                               NeighbourIndex.South.AsInt(),
                               NeighbourIndex.SouthWest.AsInt(),
                               NeighbourIndex.West.AsInt())
      };
    }

    protected int MatchAsFlag(MapCoordinate c)
    {
      return Matcher(c.X, c.Y) ? 1 : 0;
    }

    struct SelectorDefinition
    {
      readonly TRenderTile[] tiles;
      public Direction Position { get; }
      public int[] SelectorCoordinates { get; }

      public SelectorDefinition(string tag,
                                Direction position,
                                ITileRegistryEx<CornerTileSelectionKey, TRenderTile> registry,
                                params int[] selectorCoordinates)
      {
        Position = position;
        SelectorCoordinates = selectorCoordinates;
        tiles = FillInTags(tag, position, registry);
      }

      public TRenderTile AsTileName(int idx)
      {
        return tiles[idx];
      }

      static TRenderTile[] FillInTags(string tag,
                                      Direction prefix,
                                      ITileRegistryEx<CornerTileSelectionKey, TRenderTile> registry)
      {
        var tags = new TRenderTile[8];
        for (var idx = 0; idx < 8; idx += 1)
        {
          var m0 = (idx & 1) == 1;
          var m1 = (idx & 2) == 2;
          var m2 = (idx & 4) == 4;
          tags[idx] = registry.Find(tag, CornerTileSelectionKey.ValueOf(prefix, m0, m1, m2));
        }
        return tags;
      }
    }
  }
}
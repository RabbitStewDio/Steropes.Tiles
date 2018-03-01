using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.TileTags;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  public class CornerCellSelector<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
  {
    readonly ITileRegistryEx<CellMapTileSelectorKey, TRenderTile> registry;
    readonly string prefix;
    readonly SelectorDefinition[] selectors;
    readonly GridMatcher selfMatcher;
    readonly Func<int, int, TContext> contextProvider;
    MapCoordinate[] neighbours;
    readonly ITileTagEntrySelectionFactory<Direction> directionTileTagSelection;
    readonly TRenderTile[] tiles;

    public CornerCellSelector(ICellMatcher matchers,
                              GridMatcher selfMatcher,
                              IMapNavigator<GridDirection> gridNavigator,
                              ITileRegistryEx<CellMapTileSelectorKey, TRenderTile> registry,
                              string prefix,
                              Func<int, int, TContext> contextProvider = null)
    {
      this.registry = registry;
      this.prefix = prefix;
      this.selfMatcher = selfMatcher ?? throw new ArgumentNullException(nameof(selfMatcher));

      Matchers = matchers ?? throw new ArgumentNullException();
      GridNavigator = gridNavigator;
      this.contextProvider = contextProvider ?? DefaultContextProvider;

      directionTileTagSelection = TileTagEntries.CreateDirectionTagEntries();
      selectors = PrepareSelectors();
      tiles = Prepare();
    }

    static TContext DefaultContextProvider(int x, int y)
    {
      return default(TContext);
    }


    TRenderTile[] Prepare()
    {
      var owner = Matchers.Owner;
      var card = Matchers.Cardinality;
      var elements = (int)Math.Pow(card, 3) * directionTileTagSelection.Count;
      var result = new TRenderTile[elements];
      for (int dir = 0; dir < directionTileTagSelection.Count; dir += 1)
      {
        for (int b = 0; b < card; b += 1)
        {
          for (int c = 0; c < card; c += 1)
          {
            for (int d = 0; d < card; d += 1)
            {
              var key = new CellMapTileSelectorKey(directionTileTagSelection[dir], owner[b], owner[c], owner[d]);
              result[key.LinearIndex] = registry.Find(prefix, key);
            }
          }
        }
      }

      return result;
    }


    public ICellMatcher Matchers { get; }
    public IMapNavigator<GridDirection> GridNavigator { get; }

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      if (!selfMatcher(x, y))
      {
        return false;
      }

      neighbours = GridNavigator.NavigateNeighbours(new MapCoordinate(x, y), neighbours);
      bool result = false;
      for (var index = 0; index < selectors.Length; index++)
      {
        var d = selectors[index];
        if (MatchAsFlag(neighbours[d.SelectorCoordinates[0]], out ITileTagEntrySelection m0) &&
            MatchAsFlag(neighbours[d.SelectorCoordinates[1]], out ITileTagEntrySelection m1) &&
            MatchAsFlag(neighbours[d.SelectorCoordinates[2]], out ITileTagEntrySelection m2))
        {
          var tile = tiles[CellMapTileSelectorKey.LinearIndexOf(d.PositionEntry, m0, m1, m2)];
          resultCollector(d.SpritePosition, tile, contextProvider(x, y));
          result = true;
        }
      }
      return result;
    }

    bool MatchAsFlag(MapCoordinate c, out ITileTagEntrySelection s)
    {
      return Matchers.Match(c.X, c.Y, out s);
    }

    SelectorDefinition[] PrepareSelectors()
    {
      return new[]
      {
        new SelectorDefinition(directionTileTagSelection.Lookup(Direction.Up), SpritePosition.Up,
                               NeighbourIndex.West.AsInt(),
                               NeighbourIndex.NorthWest.AsInt(),
                               NeighbourIndex.North.AsInt()),
        new SelectorDefinition(directionTileTagSelection.Lookup(Direction.Right), SpritePosition.Right,
                               NeighbourIndex.North.AsInt(),
                               NeighbourIndex.NorthEast.AsInt(),
                               NeighbourIndex.East.AsInt()),
        new SelectorDefinition(directionTileTagSelection.Lookup(Direction.Down), SpritePosition.Down,
                               NeighbourIndex.East.AsInt(),
                               NeighbourIndex.SouthEast.AsInt(),
                               NeighbourIndex.South.AsInt()),
        new SelectorDefinition(directionTileTagSelection.Lookup(Direction.Left), SpritePosition.Left,
                               NeighbourIndex.South.AsInt(),
                               NeighbourIndex.SouthWest.AsInt(),
                               NeighbourIndex.West.AsInt())
      };
    }

    struct SelectorDefinition
    {
      public ITileTagEntrySelection<Direction> PositionEntry { get; }
      public int[] SelectorCoordinates { get; }
      public SpritePosition SpritePosition { get; }

      public SelectorDefinition(ITileTagEntrySelection<Direction> positionEntry, SpritePosition pos, 
                                params int[] selectorCoordinates)
      {
        SpritePosition = pos;
        PositionEntry = positionEntry;
        SelectorCoordinates = selectorCoordinates;
      }
    }
  }
}
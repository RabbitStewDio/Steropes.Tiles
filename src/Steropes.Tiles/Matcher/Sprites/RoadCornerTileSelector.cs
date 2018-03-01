using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  /// <summary>
  ///  Supplemental matcher that completes diagonal roads. Roads going across diagonal corners
  /// (ie from 0,0 to 1,1 on a grid map) would not look right, as they miss some chunks where
  /// both corners meed (there is only one pixel space here). This selector adds additional 
  /// chunks to supplement the roads.
  /// </summary>
  /// <para>
  ///  This matcher should always be active for all tiles, as it only reacts to neighbours not
  ///  to the contents of the tile for which it is called.
  /// </para>
  public class RoadCornerTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
  {
    readonly TRenderTile[] tiles;
    MapCoordinate[] coordinates;

    public RoadCornerTileSelector(GridMatcher matcher,
                                  IMapNavigator<GridDirection> gridNavigator,
                                  string prefix,
                                  ITileRegistryEx<Direction, TRenderTile> registry,
                                  Func<int, int, TContext> contextProvider = null)
      : base(matcher, gridNavigator, contextProvider)
    {
      tiles = FillInTags(prefix, registry);
    }

    protected TRenderTile[] FillInTags(string prefix, ITileRegistryEx<Direction, TRenderTile> registry)
    {
      var preparedTags = new TRenderTile[4];
      preparedTags[(int) Direction.Up] = registry.Find(prefix, Direction.Up);
      preparedTags[(int) Direction.Right] = registry.Find(prefix, Direction.Right);
      preparedTags[(int) Direction.Down] = registry.Find(prefix, Direction.Down);
      preparedTags[(int) Direction.Left] = registry.Find(prefix, Direction.Left);
      return preparedTags;
    }

    public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      coordinates = GridNavigator.NavigateCardinalNeighbours(new MapCoordinate(x, y), coordinates);

      var n = Matches(coordinates[(int) Direction.Up]);
      var e = Matches(coordinates[(int) Direction.Right]);
      var s = Matches(coordinates[(int) Direction.Down]);
      var w = Matches(coordinates[(int) Direction.Left]);
      bool result = false;
      if (n)
      {
        if (e)
        {
          resultCollector(SpritePosition.Whole, tiles[1], ContextProvider(x, y));
          result = true;
        }
        if (w)
        {
          resultCollector(SpritePosition.Whole, tiles[0], ContextProvider(x, y));
          result = true;
        }
      }
      if (s)
      {
        if (e)
        {
          resultCollector(SpritePosition.Whole, tiles[2], ContextProvider(x, y));
          result = true;
        }
        if (w)
        {
          resultCollector(SpritePosition.Whole, tiles[3], ContextProvider(x, y));
          result = true;
        }
      }
      return result;
    }
  }
}
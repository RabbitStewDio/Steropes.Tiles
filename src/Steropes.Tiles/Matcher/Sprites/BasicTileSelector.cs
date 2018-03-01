using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
  public class BasicTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
  {
    readonly TRenderTile tile;

    public BasicTileSelector(GridMatcher matcher,
                             IMapNavigator<GridDirection> gridNavigator,
                             ITileRegistry<TRenderTile> registry,
                             string tag,
                             Func<int, int, TContext> contextProvider = null) : base(matcher, gridNavigator, contextProvider)
    {
      tile = registry.Find(tag);
    }

    public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      if (Matcher(x, y))
      {
        resultCollector(SpritePosition.Whole, tile, ContextProvider(x,y));
        return true;
      }
      return false;
    }
  }
}
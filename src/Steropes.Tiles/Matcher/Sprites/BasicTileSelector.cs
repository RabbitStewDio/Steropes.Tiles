using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    public class BasicTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
    {
        readonly TRenderTile tile;
        readonly bool tileExists;

        public BasicTileSelector(GridMatcher matcher,
                                 IMapNavigator<GridDirection> gridNavigator,
                                 ITileRegistry<TRenderTile> registry,
                                 string tag,
                                 Func<int, int, TContext> contextProvider = null) : base(matcher, gridNavigator,
                                                                                         contextProvider)
        {
            tileExists = registry.TryFind(tag, out tile);
        }

        public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            if (tileExists && Matcher(x, y))
            {
                resultCollector(SpritePosition.Whole, tile, ContextProvider(x, y));
                return true;
            }

            return false;
        }
    }
}

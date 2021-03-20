using System;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    public abstract class BaseTileSelector<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
    {
        public BaseTileSelector(GridMatcher matcher,
                                IMapNavigator<GridDirection> gridNavigator,
                                Func<int, int, TContext> contextProvider = null)
        {
            ContextProvider = contextProvider ?? DefaultContextProvider;
            GridNavigator = gridNavigator ?? throw new ArgumentNullException(nameof(gridNavigator));
            Matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
        }

        protected Func<int, int, TContext> ContextProvider { get; }

        protected IMapNavigator<GridDirection> GridNavigator { get; }

        protected GridMatcher Matcher { get; }

        public abstract bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector);

        protected int MatchAsFlag(MapCoordinate c)
        {
            return Matcher(c.X, c.Y) ? 1 : 0;
        }

        protected bool Matches(MapCoordinate c)
        {
            return Matcher(c.X, c.Y);
        }

        static TContext DefaultContextProvider(int x, int y)
        {
            return default(TContext);
        }
    }
}

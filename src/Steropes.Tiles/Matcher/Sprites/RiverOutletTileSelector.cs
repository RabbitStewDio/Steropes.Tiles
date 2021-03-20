using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    /// <summary>
    ///  This tile selector selects all ocean tiles that have a river outlet. For
    ///  this, self-match checks for oceans, and neighbour match checks for land
    ///  tiles that  have the river-flag set. This matcher works in addition and
    ///  after the normal  ocean tile matcher.
    /// </summary>
    /// <para>
    ///  River matching is a combination of a (separate) CardinalTileSelector
    ///  matching any tile with a river-flag set and this shore-matcher. We assume
    ///  that the map is smart enough  to not flag ocean tiles as having a river.
    ///  The self-matcher should strictly check  whether a tile is flagged as
    ///  having a river. The neighbour-matcher should check whether  any of the
    ///  matched tiles is either a river or a water tile (lake, ocean).
    /// </para>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class RiverOutletTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
    {
        readonly GridMatcher selfMatcher;
        readonly TRenderTile northTile;
        readonly TRenderTile eastTile;
        readonly TRenderTile southTile;
        readonly TRenderTile westTile;
        readonly bool northTileExists;
        readonly bool eastTileExists;
        readonly bool southTileExists;
        readonly bool westTileExists;
        MapCoordinate[] coordinates;

        public RiverOutletTileSelector(GridMatcher matcher,
                                       GridMatcher selfMatcher,
                                       IMapNavigator<GridDirection> gridNavigator,
                                       ITileRegistryEx<CardinalIndex, TRenderTile> outletRegistry,
                                       string prefix,
                                       Func<int, int, TContext> contextProvider = null)
            : base(matcher, gridNavigator, contextProvider)
        {
            this.selfMatcher = selfMatcher ?? throw new ArgumentNullException(nameof(selfMatcher));

            this.northTileExists = outletRegistry.TryFind(prefix, CardinalIndex.North, out northTile);
            this.eastTileExists = outletRegistry.TryFind(prefix, CardinalIndex.East, out eastTile);
            this.southTileExists = outletRegistry.TryFind(prefix, CardinalIndex.South, out southTile);
            this.westTileExists = outletRegistry.TryFind(prefix, CardinalIndex.West, out westTile);
        }

        public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            // The current tile is not a land tile. If it is an ocean tile, we can test 
            // whether one of the cardinal neighbours is a river that flows into the
            // ocean. 
            if (!selfMatcher(x, y))
            {
                return false;
            }

            coordinates = GridNavigator.NavigateCardinalNeighbours(new MapCoordinate(x, y), coordinates);
            if (Matches(coordinates[CardinalIndex.North.AsInt()]))
            {
                if (northTileExists)
                {
                    resultCollector(SpritePosition.Whole, northTile, ContextProvider(x, y));
                }
                return true;
            }

            if (Matches(coordinates[CardinalIndex.East.AsInt()]))
            {
                if (eastTileExists)
                {
                    resultCollector(SpritePosition.Whole, eastTile, ContextProvider(x, y));
                }
                return true;
            }

            if (Matches(coordinates[CardinalIndex.South.AsInt()]))
            {
                if (southTileExists)
                {
                    resultCollector(SpritePosition.Whole, southTile, ContextProvider(x, y));
                }
                return true;
            }

            if (Matches(coordinates[CardinalIndex.West.AsInt()]))
            {
                if (westTileExists)
                {
                    resultCollector(SpritePosition.Whole, westTile, ContextProvider(x, y));
                }
                return true;
            }

            return false;
        }
    }
}
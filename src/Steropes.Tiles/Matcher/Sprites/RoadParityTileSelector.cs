using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    /// <summary>
    ///   The road-parity selector is a combined tile selector that generates at max two layers of tiles.
    ///   The selector combines a tile for the cardinal matched neighbours with a tile for diagonally matched
    ///   neighbours. If there are no neighbours a special "isolated" tile is used.
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class RoadParityTileSelector<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
    {
        readonly CardinalTileSelector<TRenderTile, TContext> cardinalTileSelector;
        readonly DiagonalTileSelector<TRenderTile, TContext> diagonalTileSelector;
        readonly BasicTileSelector<TRenderTile, TContext> isolatedTileMatcher;

        public RoadParityTileSelector(GridMatcher matcher,
                                      GridMatcher selfMatcher,
                                      IMapNavigator<GridDirection> gridNavigator,
                                      ITileRegistry<TRenderTile> isolatedRegistry,
                                      ITileRegistryEx<CardinalTileSelectorKey, TRenderTile> cardinalRegistry,
                                      ITileRegistryEx<DiagonalTileSelectionKey, TRenderTile> diagonalRegistry,
                                      string tag)
        {
            isolatedTileMatcher = new BasicTileSelector<TRenderTile, TContext>(selfMatcher, gridNavigator, isolatedRegistry, tag);
            diagonalTileSelector =
                new DiagonalTileSelector<TRenderTile, TContext>(matcher, selfMatcher, gridNavigator, diagonalRegistry, tag);
            cardinalTileSelector =
                new CardinalTileSelector<TRenderTile, TContext>(matcher, selfMatcher, gridNavigator, cardinalRegistry, tag);
        }

        public Func<int, int, TContext> ContextProvider { get; set; }

        public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            // we need to run both matchers regardless of the result. Both selected tiles will be overlaid on to each other.
            var cardinalMatch = cardinalTileSelector.Match(x, y, resultCollector);
            var diagonalMatch = diagonalTileSelector.Match(x, y, resultCollector);
            if (cardinalMatch || diagonalMatch)
            {
                return true;
            }

            return isolatedTileMatcher.Match(x, y, resultCollector);
        }
    }
}

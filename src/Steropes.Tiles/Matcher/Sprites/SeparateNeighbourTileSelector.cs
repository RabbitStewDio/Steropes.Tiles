using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    /// <summary>
    ///  A tile selector that produces one tile per neighbour matched. This is useful when you
    ///  need to blend with multiple neighbour tiles but dont want to provide blended tiles
    ///  for each combination of neighbours.
    /// <para/>
    ///  The selector will try to lookup the various matched tiles using the given prefix and
    ///  a directional suffix (_n
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class SeparateNeighbourTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
    {
        readonly TRenderTile[] tiles;
        readonly bool[] tileExists;
        readonly NeighbourMatchPosition[] positions;
        MapCoordinate[] neighbourPositions;

        public SeparateNeighbourTileSelector(GridMatcher matcher,
                                             IMapNavigator<GridDirection> gridNavigator,
                                             ITileRegistryEx<NeighbourMatchPosition, TRenderTile> registry,
                                             string prefix,
                                             Func<int, int, TContext> contextProvider = null) : base(
            matcher, gridNavigator, contextProvider)
        {
            positions = (NeighbourMatchPosition[])Enum.GetValues(typeof(NeighbourMatchPosition));
            Prepopulate(prefix, registry, out tiles, out tileExists);
        }

        void Prepopulate(string tag,
                         ITileRegistryEx<NeighbourMatchPosition, TRenderTile> reg,
                         out TRenderTile[] matches,
                         out bool[] matchesExist)
        {
            matches = new TRenderTile[positions.Length];
            matchesExist = new bool[positions.Length];

            for (var i = 0; i < positions.Length; i++)
            {
                var matchPos = positions[i];
                matchesExist[i] = reg.TryFind(tag, matchPos, out matches[i]);
            }
        }

        public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            if (!Matcher(x, y))
            {
                return false;
            }

            var context = ContextProvider(x, y);
            neighbourPositions = GridNavigator.NavigateNeighbours(new MapCoordinate(x, y), neighbourPositions);
            bool matchedOne = false;
            for (var i = 1; i < positions.Length; i++)
            {
                var mc = neighbourPositions[i - 1];
                if (Matcher(mc.X, mc.Y) && tileExists[i])
                {
                    resultCollector(SpritePosition.Whole, tiles[i], context);
                    matchedOne = true;
                }
            }

            if (!matchedOne)
            {
                // isolated tile ..
                if (tileExists[0])
                {
                    resultCollector(SpritePosition.Whole, tiles[0], context);
                }
            }

            return true;
        }
    }
}

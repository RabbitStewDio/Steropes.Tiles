using System;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Matcher.Sprites
{
    /// <summary>
    ///  Supplemental matcher that completes diagonal roads. Roads going across
    ///  diagonal corners (ie from 0,0 to 1,1 on a grid map) would not look right,
    ///  as they miss some chunks where both corners meed (there is only one pixel
    ///  space here). This selector adds additional  chunks to supplement the
    ///  roads.
    /// </summary>
    /// <para>
    ///  This matcher should always be active for all tiles, as it only reacts to
    ///  neighbours not to the contents of the tile for which it is called.
    /// </para>
    public class RoadCornerTileSelector<TRenderTile, TContext> : BaseTileSelector<TRenderTile, TContext>
    {
        readonly TRenderTile[] tiles;
        readonly bool[] tileExists;
        MapCoordinate[] coordinates;

        public RoadCornerTileSelector(GridMatcher matcher,
                                      IMapNavigator<GridDirection> gridNavigator,
                                      string prefix,
                                      ITileRegistryEx<Direction, TRenderTile> registry,
                                      Func<int, int, TContext> contextProvider = null)
            : base(matcher, gridNavigator, contextProvider)
        {
            FillInTags(prefix, registry, out tiles, out tileExists);
        }

        protected void FillInTags(string prefix,
                                  ITileRegistryEx<Direction, TRenderTile> registry,
                                  out TRenderTile[] preparedTags,
                                  out bool[] preparedTagExists)
        {
            preparedTags = new TRenderTile[4];
            preparedTagExists = new bool[4];
            preparedTagExists[(int)Direction.Up] =
                registry.TryFind(prefix, Direction.Up, out preparedTags[(int)Direction.Up]);
            preparedTagExists[(int)Direction.Right] =
                registry.TryFind(prefix, Direction.Right, out preparedTags[(int)Direction.Right]);
            preparedTagExists[(int)Direction.Down] =
                registry.TryFind(prefix, Direction.Down, out preparedTags[(int)Direction.Down]);
            preparedTagExists[(int)Direction.Left] =
                registry.TryFind(prefix, Direction.Left, out preparedTags[(int)Direction.Left]);
        }

        public override bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            coordinates = GridNavigator.NavigateCardinalNeighbours(new MapCoordinate(x, y), coordinates);

            var n = Matches(coordinates[(int)Direction.Up]);
            var e = Matches(coordinates[(int)Direction.Right]);
            var s = Matches(coordinates[(int)Direction.Down]);
            var w = Matches(coordinates[(int)Direction.Left]);
            bool result = false;
            if (n)
            {
                if (e)
                {
                    if (tileExists[1])
                    {
                        resultCollector(SpritePosition.Whole, tiles[1], ContextProvider(x, y));
                    }

                    result = true;
                }

                if (w)
                {
                    if (tileExists[0])
                    {
                        resultCollector(SpritePosition.Whole, tiles[0], ContextProvider(x, y));
                    }

                    result = true;
                }
            }

            if (s)
            {
                if (e)
                {
                    if (tileExists[2])
                    {
                        resultCollector(SpritePosition.Whole, tiles[2], ContextProvider(x, y));
                    }

                    result = true;
                }

                if (w)
                {
                    if (tileExists[3])
                    {
                        resultCollector(SpritePosition.Whole, tiles[3], ContextProvider(x, y));
                    }

                    result = true;
                }
            }

            return result;
        }
    }
}

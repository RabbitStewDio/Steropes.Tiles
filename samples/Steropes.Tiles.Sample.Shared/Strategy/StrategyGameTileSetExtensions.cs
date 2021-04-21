using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Sample.Shared.Strategy.Model;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public static class StrategyGameTileSetExtensions
    {
        public static TerrainGraphic FindFirstTerrain(this IStrategyGameTileSet tileSet, ITerrain t)
        {
            foreach (var tg in t.AllGraphicTags())
            {
                if (tileSet.TryGetTerrain(tg, out TerrainGraphic g))
                {
                    return g;
                }
            }

            return null;
        }

        public static bool FindFirstTile<TRenderTile>(this ITileRegistry<TRenderTile> tileSet,
                                                      IRuleElement t,
                                                      out TRenderTile result)
        {
            foreach (var tg in t.AllGraphicTags())
            {
                if (tileSet.TryFind(tg, out TRenderTile g))
                {
                    result = g;
                    return true;
                }
            }

            result = default(TRenderTile);
            return false;
        }
    }
}

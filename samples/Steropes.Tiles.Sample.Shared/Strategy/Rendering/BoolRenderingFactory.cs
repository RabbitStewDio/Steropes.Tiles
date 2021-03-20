using Steropes.Tiles.Demo.Core.GameData.Util;
using Steropes.Tiles.Matcher;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Rendering
{
    public class BoolRenderingFactory : TaggedRendingFactoryBase
    {
        public BoolRenderingFactory(ITileRenderModeContext context) : base(context)
        { }

        public GridMatcher CreateMatcher(int layerIndex, RenderLayerDefinition rd)
        {
            var otherGraphicTags = ComputeMatchingTerrains(layerIndex, rd.MatchWith);

            var terrainTypes = Context.GameData.Rules.TerrainTypes;
            var terrainsByKey = new bool[terrainTypes.Count];
            foreach (var pair in otherGraphicTags)
            {
                foreach (var terrain in pair.Value)
                {
                    var b = terrainTypes.IndexOf(terrain);
                    terrainsByKey[b] = true;
                }
            }

            var map = Context.GameData.Terrain;

            bool MatchOthers(int x, int y)
            {
                return terrainsByKey[map[x, y].TerrainIdx];
            }

            return MatchOthers;
        }
    }
}

using System.Collections.Generic;
using System.Linq;
using Steropes.Tiles.Demo.Core.GameData.Strategy.Model;
using Steropes.Tiles.Demo.Core.GameData.Util;
using Steropes.Tiles.Matcher.Sprites;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy.Rendering
{
    public class TagSetRenderingFactory : TaggedRendingFactoryBase
    {
        public TagSetRenderingFactory(ITileRenderModeContext context) : base(context)
        { }

        public ICellMatcher CreateMatcher(int layerIndex, RenderLayerDefinition rd)
        {
            // In cell matching with multiple targets, the matcher must include a self-match.
            var matchSpecs = new List<string>();
            if (rd.MatchAs != null)
            {
                matchSpecs.Add(rd.MatchAs);
            }

            matchSpecs.AddRange(rd.MatchWith);

            // tagname -> List of matching ITerrains
            var otherGraphicTags = ComputeMatchingTerrains(layerIndex, matchSpecs);
            var factory = TileTagEntrySelectionFactory.FromTags(otherGraphicTags.Keys.ToArray());
            var rules = Context.GameData.Rules;
            var terrainsByKey = new ITileTagEntrySelection[rules.TerrainTypes.Count];
            foreach (var pair in otherGraphicTags)
            {
                var key = factory.Lookup(pair.Key);
                foreach (var terrain in pair.Value)
                {
                    var b = rules.TerrainTypes.IndexOf(terrain);
                    terrainsByKey[b] = key;
                }
            }

            var map = Context.GameData.Terrain;
            return new LookupMatcher(map, factory, terrainsByKey);
        }

        class LookupMatcher : ICellMatcher
        {
            readonly IMap2D<TerrainData> map;
            readonly ITileTagEntrySelection[] selections;

            public LookupMatcher(IMap2D<TerrainData> map,
                                 ITileTagEntrySelectionFactory factory,
                                 ITileTagEntrySelection[] selections)
            {
                this.Owner = factory;
                this.map = map;
                this.selections = selections;
            }

            public bool Match(int x, int y, out ITileTagEntrySelection result)
            {
                var idx = map[x, y].TerrainIdx;
                result = selections[idx];
                return result != null;
            }

            public int Cardinality => Owner.Count;
            public ITileTagEntrySelectionFactory Owner { get; }
        }
    }
}

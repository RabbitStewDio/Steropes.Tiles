using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public class RenderLayerDefinition
    {
        public string Tag { get; }
        public TerrainMatchType MatchType { get; }
        public string MatchAs { get; }
        public IReadOnlyList<string> MatchWith { get; }

        public RenderLayerDefinition(TerrainMatchType matchType,
                                     string tag,
                                     string matchAs,
                                     IReadOnlyList<string> matchWith)
        {
            Tag = tag;
            MatchType = matchType;
            MatchAs = matchAs;
            MatchWith = matchWith ?? throw new ArgumentNullException(nameof(matchWith));
        }
    }
}

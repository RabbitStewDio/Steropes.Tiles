using Steropes.Tiles.TemplateGen.Models.Rendering.Generators;
using System.Collections.Generic;

namespace Steropes.Tiles.TemplateGen.Models.Rendering
{
    public static class MatchTypeStrategyFactory
    {
        static readonly Dictionary<MatcherType, IMatchTypeStrategy> GeneratorRules;

        static MatchTypeStrategyFactory()
        {
            GeneratorRules = new Dictionary<MatcherType, IMatchTypeStrategy>
            {
                {MatcherType.CardinalIndex, new CardinalIndexMatchTypeStrategy()},
                {MatcherType.CardinalFlags, new CardinalFlagMatchTypeStrategy()},
                {MatcherType.CellMap, new CellMapMatchTypeStrategy()},
                {MatcherType.Corner, new CornerMatchTypeStrategy()},
                {MatcherType.DiagonalFlags, new DiagonalFlagMatchTypeStrategy()},
                {MatcherType.NeighbourIndex, new NeighbourIndexMatchTypeStrategy()}
            };
        }

        public static IMatchTypeStrategy StrategyFor(MatcherType t)
        {
            if (GeneratorRules.TryGetValue(t, out var r))
            {
                return r;
            }

            return new BasicMatchTypeStrategy();
        }
    }
}

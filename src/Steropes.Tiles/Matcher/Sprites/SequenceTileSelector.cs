using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Matcher.Sprites
{
    /// <summary>
    ///  A basic tile selector that selects the best tile from a (possibly) incomplete
    ///  list of choices. This can be used to select different tiles based on some 
    ///  comparation factor, like a city's population size.
    /// </summary>
    public class SequenceTileSelector<TRenderTile, TContext, TFactor> : ITileMatcher<TRenderTile, TContext>
    {
        public delegate bool QueryFunction(int x, int y, out TFactor result);

        readonly QueryFunction matcher;
        readonly Func<int, int, TContext> contextProvider;
        readonly List<TFactor> choiceKeys;
        readonly List<TRenderTile> choiceValues;
        readonly IComparer<TFactor> comparer;

        public SequenceTileSelector(QueryFunction matcher,
                                    SortedList<TFactor, TRenderTile> choices,
                                    Func<int, int, TContext> contextProvider = null)
        {
            this.matcher = matcher;
            this.comparer = choices.Comparer;
            this.choiceKeys = new List<TFactor>(choices.Keys);
            this.choiceValues = new List<TRenderTile>(choices.Values);
            this.contextProvider = contextProvider ?? DefaultContext;
        }

        static TContext DefaultContext(int x, int y)
        {
            return default(TContext);
        }

        public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
        {
            if (matcher(x, y, out TFactor factor))
            {
                var b = choiceKeys.BinarySearch(factor, comparer);
                if (b < 0)
                {
                    b = ~b - 1;
                    if (b < 0)
                    {
                        return false;
                    }
                }

                var tile = choiceValues[b];
                resultCollector(SpritePosition.Whole, tile, contextProvider(x, y));
                return true;
            }

            return false;
        }
    }
}

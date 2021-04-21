using System;
using System.Collections.Generic;

namespace Steropes.Tiles.Matcher
{
    /// <summary>
    ///   A tile matcher that matches a tile based on some unique identifying key characteristic.
    ///   This selection process can find the correct matcher in linear time.
    /// </summary>
    /// <para>
    ///   Use this if your map entries are distinct and where there is a clear mapping between
    ///   map content and the render matchers that can apply. If multiple matchers need to apply,
    ///   combine this class with the AggregateTileMatcher.
    /// </para>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class DistinctTileMatcher<TKey, TTile, TContext> : ITileMatcher<TTile, TContext>
    {
        readonly Func<int, int, TKey> keyFn;
        readonly Dictionary<TKey, ITileMatcher<TTile, TContext>> matchers;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="keyFn">Key extractor function. Must never return null if TKey is an class type.</param>
        public DistinctTileMatcher(Func<int, int, TKey> keyFn)
        {
            this.keyFn = keyFn;
            matchers = new Dictionary<TKey, ITileMatcher<TTile, TContext>>();
        }

        public bool Match(int x, int y, TileResultCollector<TTile, TContext> onMatchFound)
        {
            var key = keyFn(x, y);
            if (matchers.TryGetValue(key, out ITileMatcher<TTile, TContext> m))
            {
                if (m.Match(x, y, onMatchFound))
                {
                    return true;
                }
            }

            return false;
        }

        public void Add(TKey key, ITileMatcher<TTile, TContext> matcher)
        {
            matchers.Add(key, matcher);
        }
    }
}

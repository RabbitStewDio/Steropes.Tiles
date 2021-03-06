﻿using System;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher
{
    public class BitVector32Matcher<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>
    {
        public delegate bool ResolveDelegate(int bitPosition, out TRenderTile result);

        readonly Func<int, int, uint> queryFn;
        readonly Func<int, int, int, TContext> contextFn;
        readonly int lowerLimit;
        readonly int limit;
        readonly TRenderTile[] tiles;
        readonly bool[] tileExists;

        public BitVector32Matcher(Func<int, int, uint> queryFn,
                                  ResolveDelegate resultFn,
                                  Func<int, int, int, TContext> contextFn = null) : this(0, 31, queryFn, resultFn,
                                                                                         contextFn)
        { }

        public BitVector32Matcher(int lowerLimit,
                                  int upperLimit,
                                  Func<int, int, uint> queryFn,
                                  ResolveDelegate resultFn,
                                  Func<int, int, int, TContext> contextFn = null)
        {
            if (limit < 0 || limit >= 32)
            {
                throw new ArgumentOutOfRangeException();
            }

            if (resultFn == null)
            {
                throw new ArgumentNullException(nameof(resultFn));
            }

            this.queryFn = queryFn ?? throw new ArgumentNullException(nameof(queryFn));
            this.contextFn = contextFn ?? DefaultContext;
            this.lowerLimit = lowerLimit;
            this.limit = upperLimit;

            tiles = new TRenderTile[limit + 1];
            tileExists = new bool[limit + 1];
            for (int i = lowerLimit; i <= limit; i += 1)
            {
                tileExists[i] = resultFn(i, out tiles[i]);
            }
        }

        TContext DefaultContext(int x, int y, int idx)
        {
            return default(TContext);
        }

        public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound)
        {
            var q = queryFn(x, y) >> lowerLimit;
            if (q == 0)
            {
                return false;
            }

            var result = false;
            for (var i = lowerLimit; i <= limit; i += 1)
            {
                if ((q & 1) == 1)
                {
                    var context = contextFn(x, y, i);
                    if (tileExists[i])
                    {
                        onMatchFound(SpritePosition.Whole, tiles[i], context);
                        result = true;
                    }
                }

                q >>= 1;
            }

            return result;
        }
    }
}

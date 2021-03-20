using System;

namespace Steropes.Tiles.Matcher
{
    public class MapLookupTable<T>
    {
        readonly int tableSize;
        readonly T[] results;
        readonly Func<int, int, int> lookupResultIndex;
        readonly T defaultValue;

        public MapLookupTable(int tableSize,
                              Func<int, int, int> lookupResultIndex,
                              Func<int, T> computeResultIndex,
                              T defaultValue = default(T))
        {
            this.tableSize = tableSize;
            this.lookupResultIndex = lookupResultIndex;
            this.defaultValue = defaultValue;
            this.results = new T[tableSize];
            for (int i = 0; i < tableSize; i += 1)
            {
                results[i] = computeResultIndex(i);
            }
        }

        public T Match(int x, int y)
        {
            var idx = lookupResultIndex(x, y);
            if (idx < 0 || idx >= tableSize)
            {
                return defaultValue;
            }

            return results[idx];
        }
    }

    public class LookupTable<T>
    {
        readonly int tableSize;
        readonly T[] results;
        readonly T defaultValue;

        public LookupTable(int tableSize,
                           Func<int, T> computeResultIndex,
                           T defaultValue = default(T))
        {
            this.tableSize = tableSize;
            this.defaultValue = defaultValue;
            this.results = new T[tableSize];
            for (int i = 0; i < tableSize; i += 1)
            {
                results[i] = computeResultIndex(i);
            }
        }

        public T Lookup(int idx)
        {
            if (idx < 0 || idx >= tableSize)
            {
                return defaultValue;
            }

            return results[idx];
        }
    }

    public static class LookupTable
    {
        public static bool UnwrapTuple<TTextureTile, TContext>(Tuple<TTextureTile, TContext> data,
                                                               out TTextureTile t,
                                                               out TContext context)
        {
            if (data != null)
            {
                t = data.Item1;
                context = data.Item2;
                return true;
            }

            t = default(TTextureTile);
            context = default(TContext);
            return false;
        }
    }
}

using System;
using System.Collections.Generic;

namespace Steropes.Tiles.DataStructures
{
    /// <summary>
    ///  A helper data structure that collects items and at the end executes an
    ///  action over the sorted set of entries. Use this if you need to draw
    ///  sprites/tiles in a particular order that is not guaranteed by the
    ///  system. The most common use for this structure sorts item/character
    ///  tiles based on the y/z axis within a set of tiles.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class BatchBuffer<T>
    {
        readonly PositionedSpriteComparer posComp;
        int[] indices;
        T[] values;

        public BatchBuffer(IComparer<T> comp)
        {
            posComp = new PositionedSpriteComparer(comp);
            indices = new int[0];
            values = new T[0];
        }

        public int Count { get; set; }

        void EnsureSize()
        {
            if (Count == indices.Length)
            {
                Array.Resize(ref values, Count + 1000);
                Array.Resize(ref indices, Count + 1000);
            }
        }

        public void Add(in T value)
        {
            EnsureSize();
            indices[Count] = Count;
            values[Count] = value;
            Count += 1;
        }

        public void Clear()
        {
            Count = 0;
        }

        public void Consume(Action<T> a)
        {
            posComp.Sprites = values;
            Array.Sort(indices, 0, Count, posComp);
            // Sort(indices, 0, Count, posComp);

            for (var i = 0; i < Count; i++)
            {
                var idx = indices[i];
                var t = values[idx];
                a(t);
            }

            Clear();
        }

        class PositionedSpriteComparer : IComparer<int>
        {
            readonly IComparer<T> comp;
            public T[] Sprites;

            public PositionedSpriteComparer(IComparer<T> comp)
            {
                this.comp = comp ?? throw new ArgumentNullException(nameof(comp));
            }

            public int Compare(int x, int y)
            {
                var x1 = Sprites[x];
                var x2 = Sprites[y];
                return comp.Compare(x1, x2);
            }
        }
        /*

        static void Sort<TData>(TData[] array, int startIndex, int endIndex, IComparer<TData> comparer)
        {
            if (startIndex < 0) throw new IndexOutOfRangeException();
            if (endIndex < 0) throw new IndexOutOfRangeException();
            if (endIndex < startIndex) throw new ArgumentException();
            SortInternal(array, startIndex, endIndex, comparer);
        }


        static void SortInternal<TData>(TData[] array, int startIndex, int endIndex, IComparer<TData> comparer)
        {
            if (startIndex >= endIndex) return;
            var partitionBoundary = Partition(array, startIndex, endIndex, comparer);
            SortInternal(array, startIndex, partitionBoundary - 1, comparer);
            SortInternal(array, partitionBoundary + 1, endIndex, comparer);
        }

        static int Partition<TData>(TData[] array, int startIndex, int endIndex, IComparer<TData> comparer)
        {
            var middle = startIndex;
            for (int u = startIndex; u < endIndex; u += 1)
            {
                if (comparer.Compare(array[u], array[endIndex]) <= 0)
                {
                    Swap(array, u, middle);
                    middle += 1;
                }
            }

            return middle;
        }

        static void Swap<TData>(TData[] data, int idxA, int idxB)
        {
            TData tmp = data[idxA];
            data[idxA] = data[idxB];
            data[idxB] = tmp;
        }
        */
    }
}

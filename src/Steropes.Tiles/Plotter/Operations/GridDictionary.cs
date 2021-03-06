﻿using System;
using System.Collections.Generic;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Plotter.Operations
{
    public interface IGridDictionary<TValue>
    {
        bool TryGetValue(in MapCoordinate key, out TValue value);
        void Store(in MapCoordinate key, TValue value);

        void Remove(in MapCoordinate key);
        void RemoveWhere(int expectedSize, Func<TValue, bool> predicate);

        void UpdateBounds(in MapCoordinate centerPoint, in IntInsets drawArea);
    }

    public class GridDictionary<TValue> : IGridDictionary<TValue>
    {
        readonly Dictionary<MapCoordinate, TValue> renderAtRecords;
        readonly List<MapCoordinate> removeKeyBuffer;

        public GridDictionary()
        {
            removeKeyBuffer = new List<MapCoordinate>();
            renderAtRecords = new Dictionary<MapCoordinate, TValue>();
        }

        public bool TryGetValue(in MapCoordinate key, out TValue value)
        {
            return renderAtRecords.TryGetValue(key, out value);
        }

        public int Count
        {
            get { return renderAtRecords.Count; }
        }

        public void Store(in MapCoordinate key, TValue value)
        {
            renderAtRecords[key] = value;
        }

        public void Remove(in MapCoordinate key)
        {
            renderAtRecords.Remove(key);
        }

        public void RemoveWhere(int expectedSize, Func<TValue, bool> predicate)
        {
            if (Count - expectedSize < 100)
            {
                // not enough garbage to worry about cleaning up
                return;
            }

            removeKeyBuffer.Clear();
            foreach (var record in renderAtRecords)
            {
                var v = record.Value;
                if (predicate(v))
                {
                    removeKeyBuffer.Add(record.Key);
                }
            }

            foreach (var coordinate in removeKeyBuffer)
            {
                renderAtRecords.Remove(coordinate);
            }

            removeKeyBuffer.Clear();
        }

        public void UpdateBounds(in MapCoordinate centerPoint, in IntInsets drawArea)
        { }
    }
}

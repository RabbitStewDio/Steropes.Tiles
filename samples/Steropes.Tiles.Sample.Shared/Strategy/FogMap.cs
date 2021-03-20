using System;
using System.Collections;

namespace Steropes.Tiles.Demo.Core.GameData.Strategy
{
    public interface IFogMap
    {
        int Width { get; }
        int Height { get; }
        FogState this[int x, int y] { get; }

        void MarkExplored(int x, int y);
        void MarkUnexplored(int x, int y);
        void MarkRangeExplored(int x, int y, int radius);
        void MarkRangeUnexplored(int x, int y, int radius);
        void UpdateExplored(int x, int y, bool val);

        void MarkVisible(int x, int y);
        void MarkInvisible(int x, int y);
        void MarkRangeVisible(int x, int y, int radius);
        void MarkRangeInvisible(int x, int y, int radius);
        void UpdateVisible(int x, int y, short val);

        event EventHandler<MapDataChangedEventArgs> MapDataChanged;
        void FireMapDataChanged(int x, int y, int range = 1);
    }

    /// <summary>
    ///  This class manages the tile visibility for a single player. A map tile tracks two
    ///  different states: Explored and Visible.  
    /// 
    ///  Explored tracks whether the player has ever seen this tile. This exposes the terrain
    ///  type of a tile, but does not tell us anything about the units on that tile. Once the
    ///  unit that has explored the tile moves elsewhere, the tile remains explored.
    /// 
    ///  Visible tracks whether at least one unit actively looks at the tile. This means the
    ///  unit or city is close by and can monitor all activities on the tile. Once the unit
    ///  leaves the visible range for that tile, the map will be shrouded in the fog of war.
    ///  Visibility is tracked as a reference counter, each unit updates the visible flag of 
    ///  the fog-map when it moves. 
    /// </summary>
    public class FogMap : IFogMap
    {
        public int Width { get; }
        public int Height { get; }
        public event EventHandler<MapDataChangedEventArgs> MapDataChanged;

        readonly BitArray explored;
        readonly short[] visible;

        public FogMap(int width, int height)
        {
            explored = new BitArray(width * height);
            visible = new short[width * height];
            Width = width;
            Height = height;
        }

        public FogState this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                {
                    return FogState.Unknown;
                }

                var idx = x + y * Width;
                if (visible[idx] > 0)
                {
                    return FogState.Explored | FogState.Visible;
                }

                return explored[idx] ? FogState.Explored : FogState.Unknown;
            }
        }

        public void FireMapDataChanged(int x, int y, int range = 1)
        {
            MapDataChanged?.Invoke(this, new MapDataChangedEventArgs(x, y, range));
        }

        public void MarkExplored(int x, int y)
        {
            UpdateExplored(x, y, true);
            FireMapDataChanged(x, y);
        }

        public void UpdateExplored(int x, int y, bool val)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                throw new ArgumentException();
            }

            var idx = x + y * Width;
            explored[idx] = val;
        }

        public void MarkRangeExplored(int x, int y, int radius)
        {
            var upperX = Math.Min(x + radius, Width - 1);
            var upperY = Math.Min(y + radius, Height - 1);

            for (int ty = Math.Max(0, y - radius); ty <= upperY; ty += 1)
            {
                for (int tx = Math.Max(0, x - radius); tx <= upperX; tx += 1)
                {
                    UpdateExplored(tx, ty, true);
                }
            }

            FireMapDataChanged(x, y, radius);
        }

        public void MarkRangeUnexplored(int x, int y, int radius)
        {
            var upperX = Math.Min(x + radius, Width - 1);
            var upperY = Math.Min(y + radius, Height - 1);

            for (int ty = Math.Max(0, y - radius); ty <= upperY; ty += 1)
            {
                for (int tx = Math.Max(0, x - radius); tx <= upperX; tx += 1)
                {
                    UpdateExplored(tx, ty, false);
                }
            }

            FireMapDataChanged(x, y, radius);
        }

        public void MarkUnexplored(int x, int y)
        {
            UpdateExplored(x, y, false);
            FireMapDataChanged(x, y);
        }

        public void MarkVisible(int x, int y)
        {
            UpdateVisible(x, y, 1);
            FireMapDataChanged(x, y);
        }

        public void UpdateVisible(int x, int y, short val)
        {
            if (x < 0 || y < 0 || x >= Width || y >= Height)
            {
                throw new ArgumentException();
            }

            var idx = x + y * Width;
            visible[idx] += val;
        }

        public void MarkInvisible(int x, int y)
        {
            UpdateVisible(x, y, -1);
            FireMapDataChanged(x, y);
        }

        /// <summary>
        ///  Marks any tile visible that can be reached with ${radius} moves.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public void MarkRangeVisible(int x, int y, int radius)
        {
            var upperX = Math.Min(x + radius, Width - 1);
            var upperY = Math.Min(y + radius, Height - 1);

            for (int ty = Math.Max(0, y - radius); ty <= upperY; ty += 1)
            {
                for (int tx = Math.Max(0, x - radius); tx <= upperX; tx += 1)
                {
                    UpdateVisible(tx, ty, 1);
                }
            }

            FireMapDataChanged(x, y, radius);
        }

        /// <summary>
        ///  Marks any tile visible that can be reached with ${radius} moves.
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="radius"></param>
        public void MarkRangeInvisible(int x, int y, int radius)
        {
            var upperX = Math.Min(x + radius, Width - 1);
            var upperY = Math.Min(y + radius, Height - 1);

            for (int ty = Math.Max(0, y - radius); ty <= upperY; ty += 1)
            {
                for (int tx = Math.Max(0, x - radius); tx <= upperX; tx += 1)
                {
                    UpdateVisible(tx, ty, -1);
                }
            }

            FireMapDataChanged(x, y, radius);
        }
    }
}

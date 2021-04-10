using Steropes.Tiles.Navigation;
using System;

namespace Steropes.Tiles.Sample.Shared.Strategy
{
    public class MapDataChangedEventArgs : EventArgs
    {
        public MapCoordinate Coordinate { get; }
        public int Range { get; }

        public MapDataChangedEventArgs(int x, int y, int range = 1)
        {
            this.Coordinate = new MapCoordinate(x, y);
            Range = range;
        }
    }
}

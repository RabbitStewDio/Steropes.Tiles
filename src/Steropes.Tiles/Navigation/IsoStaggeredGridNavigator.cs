using System;

namespace Steropes.Tiles.Navigation
{
    public class IsoStaggeredGridNavigator : IMapNavigator<GridDirection>
    {
        public bool NavigateTo(GridDirection direction, in MapCoordinate source, out MapCoordinate result, int steps = 1)
        {
            if (steps == 0)
            {
                result = source;
                return true;
            }

            result = source;
            for (var i = 0; i < steps; i += 1)
            {
                switch (direction)
                {
                    case GridDirection.None:
                        result = source;
                        break;
                    case GridDirection.North:
                        result = new MapCoordinate(source.X, source.Y - 2);
                        break;
                    case GridDirection.NorthEast:
                        result = new MapCoordinate(source.X + (source.Y & 1), source.Y - 1);
                        break;
                    case GridDirection.East:
                        result = new MapCoordinate(source.X + 1, source.Y);
                        break;
                    case GridDirection.SouthEast:
                        result = new MapCoordinate(source.X + (source.Y & 1), source.Y + 1);
                        break;
                    case GridDirection.South:
                        result = new MapCoordinate(source.X, source.Y + 2);
                        break;
                    case GridDirection.SouthWest:
                        result = new MapCoordinate(source.X + (source.Y & 1) - 1, source.Y + 1);
                        break;
                    case GridDirection.West:
                        result = new MapCoordinate(source.X - 1, source.Y);
                        break;
                    case GridDirection.NorthWest:
                        result = new MapCoordinate(source.X + (source.Y & 1) - 1, source.Y - 1);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
                }
            }

            return true;
        }
    }
}

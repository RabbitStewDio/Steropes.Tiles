using System;

namespace Steropes.Tiles.Navigation
{
    public class IsoDiamondGridNavigator : IMapNavigator<GridDirection>
    {
        public bool NavigateTo(GridDirection direction, in MapCoordinate source, out MapCoordinate result, int steps = 1)
        {
            if (steps == 0)
            {
                result = source;
                return true;
            }

            switch (direction)
            {
                case GridDirection.None:
                    result = source;
                    break;
                case GridDirection.North:
                    result = new MapCoordinate(source.X - steps, source.Y - steps);
                    break;
                case GridDirection.NorthEast:
                    result = new MapCoordinate(source.X, source.Y - steps);
                    break;
                case GridDirection.East:
                    result = new MapCoordinate(source.X + steps, source.Y - steps);
                    break;
                case GridDirection.SouthEast:
                    result = new MapCoordinate(source.X + steps, source.Y);
                    break;
                case GridDirection.South:
                    result = new MapCoordinate(source.X + steps, source.Y + steps);
                    break;
                case GridDirection.SouthWest:
                    result = new MapCoordinate(source.X, source.Y + steps);
                    break;
                case GridDirection.West:
                    result = new MapCoordinate(source.X - steps, source.Y + steps);
                    break;
                case GridDirection.NorthWest:
                    result = new MapCoordinate(source.X - steps, source.Y);
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(direction), direction, null);
            }

            return true;
        }
    }
}

using System;

namespace Steropes.Tiles.Navigation
{
    public class LimitedRangeNavigator<T> : IMapNavigator<T>
        where T : struct
    {
        readonly ILogAdapter logger = LogProvider.CreateLogger<LimitedRangeNavigator<T>>();
        readonly int lowerX;
        readonly int lowerY;
        readonly IMapNavigator<T> parent;
        readonly int upperX;
        readonly int upperY;

        public LimitedRangeNavigator(IMapNavigator<T> parent, int lowerX, int lowerY, int upperX, int upperY)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
            this.lowerX = lowerX;
            this.lowerY = lowerY;
            this.upperX = upperX - 1;
            this.upperY = upperY - 1;
        }

        public LimitedRangeNavigator(IMapNavigator<T> parent, int upperX, int upperY) : this(parent, 0, 0, upperX, upperY)
        { }

        public bool NavigateTo(T direction, MapCoordinate source, out MapCoordinate result, int steps)
        {
            bool resultFlag = parent.NavigateTo(direction, source, out result, steps);

            var clampedX = Clamp(result.X, lowerX, upperX);
            var clampedY = Clamp(result.Y, lowerY, upperY);

            var validNavigation = clampedY == result.Y && clampedX == result.X;
            if (!validNavigation)
            {
                logger.Trace("Invalid navigation {0}, {1} vs {2}", clampedX, clampedY, result);
            }

            return resultFlag && validNavigation;
        }

        static int Clamp(int value, int lower, int upper)
        {
            return Math.Min(Math.Max(value, lower), upper);
        }
    }
}

using System;
using System.Runtime.CompilerServices;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Navigation
{
    internal class WrapAroundNavigator<T> : IMapNavigator<T>
        where T : struct
    {
        readonly int lowerX;
        readonly int lowerY;
        readonly int deltaX;
        readonly int deltaY;
        readonly IMapNavigator<T> parent;

        public WrapAroundNavigator(IMapNavigator<T> parent, Range x, Range y)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

            lowerX = Math.Min(x.Min, x.Max);
            lowerY = Math.Min(y.Min, y.Max);

            var upperX = Math.Max(x.Min, x.Max);
            var upperY = Math.Max(y.Min, y.Max);
            deltaX = upperX - lowerX;
            deltaY = upperY - lowerY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int WrapX(int value)
        {
            return ((value - lowerX) % deltaX + deltaX) % deltaX + lowerX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int WrapY(int value)
        {
            return ((value - lowerY) % deltaY + deltaY) % deltaY + lowerY;
        }

        public WrapAroundNavigator(IMapNavigator<T> parent, int upperX, int upperY) : this(parent, new Range(0, upperX), new Range(0, upperY))
        { }

        public bool NavigateTo(T direction, MapCoordinate source, out MapCoordinate result, int steps)
        {
            var resultFlag = parent.NavigateTo(direction, source, out result, steps);
            result.X = WrapX(result.X);
            result.Y = WrapY(result.Y);
            return resultFlag;
        }
    }

    internal class WrapAroundVertical<T> : IMapNavigator<T>
        where T : struct
    {
        readonly int lowerY;
        readonly int deltaY;
        readonly IMapNavigator<T> parent;

        public WrapAroundVertical(IMapNavigator<T> parent, Range y)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

            lowerY = Math.Min(y.Min, y.Max);

            var upperY = Math.Max(y.Min, y.Max);
            deltaY = upperY - lowerY;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int WrapY(int value)
        {
            return ((value - lowerY) % deltaY + deltaY) % deltaY + lowerY;
        }

        public WrapAroundVertical(IMapNavigator<T> parent, int upperY) : this(parent, new Range(0, upperY))
        { }

        public bool NavigateTo(T direction, MapCoordinate source, out MapCoordinate result, int steps)
        {
            var resultFlag = parent.NavigateTo(direction, source, out result, steps);
            result.Y = WrapY(result.Y);
            return resultFlag;
        }
    }

    internal class WrapAroundHorizontal<T> : IMapNavigator<T>
        where T : struct
    {
        readonly int lowerX;
        readonly int deltaX;
        readonly IMapNavigator<T> parent;

        public WrapAroundHorizontal(IMapNavigator<T> parent, Range x)
        {
            this.parent = parent ?? throw new ArgumentNullException(nameof(parent));

            lowerX = Math.Min(x.Min, x.Max);

            var upperX = Math.Max(x.Min, x.Max);
            deltaX = upperX - lowerX;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        int WrapX(int value)
        {
            return ((value - lowerX) % deltaX + deltaX) % deltaX + lowerX;
        }

        public WrapAroundHorizontal(IMapNavigator<T> parent, int upperX) : this(parent, new Range(0, upperX))
        { }

        public bool NavigateTo(T direction, MapCoordinate source, out MapCoordinate result, int steps)
        {
            var resultFlag = parent.NavigateTo(direction, source, out result, steps);
            result.X = WrapX(result.X);
            return resultFlag;
        }
    }

    public static class WrapAroundNavigation
    {
        public static IMapNavigator<T> Wrap<T>(this IMapNavigator<T> parent, Range? x, Range? y)
            where T : struct
        {
            if (x.HasValue && y.HasValue)
            {
                return new WrapAroundNavigator<T>(parent, x.Value, y.Value);
            }

            if (x.HasValue)
            {
                return new WrapAroundHorizontal<T>(parent, x.Value);
            }

            if (y.HasValue)
            {
                return new WrapAroundVertical<T>(parent, y.Value);
            }

            return parent;
        }

        public static IMapNavigator<T> Wrap<T>(this IMapNavigator<T> parent, int? x, int? y)
            where T : struct
        {
            if (x.HasValue && y.HasValue)
            {
                return new WrapAroundNavigator<T>(parent, x.Value, y.Value);
            }

            if (x.HasValue)
            {
                return new WrapAroundHorizontal<T>(parent, x.Value);
            }

            if (y.HasValue)
            {
                return new WrapAroundVertical<T>(parent, y.Value);
            }

            return parent;
        }
    }
}

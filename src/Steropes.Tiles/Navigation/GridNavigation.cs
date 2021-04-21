using System;

namespace Steropes.Tiles.Navigation
{
    public static class GridNavigation
    {
        public static MapCoordinate[] NavigateNeighbours(this IMapNavigator<GridDirection> nav,
                                                         MapCoordinate coord,
                                                         MapCoordinate[] retval = null)
        {
            if (retval == null || retval.Length < 8)
            {
                retval = new MapCoordinate[8];
            }

            nav.NavigateTo(GridDirection.North, coord, out retval[0]);
            nav.NavigateTo(GridDirection.NorthEast, coord, out retval[1]);
            nav.NavigateTo(GridDirection.East, coord, out retval[2]);
            nav.NavigateTo(GridDirection.SouthEast, coord, out retval[3]);
            nav.NavigateTo(GridDirection.South, coord, out retval[4]);
            nav.NavigateTo(GridDirection.SouthWest, coord, out retval[5]);
            nav.NavigateTo(GridDirection.West, coord, out retval[6]);
            nav.NavigateTo(GridDirection.NorthWest, coord, out retval[7]);
            return retval;
        }

        public static MapCoordinate[] NavigateCardinalNeighbours(this IMapNavigator<GridDirection> nav,
                                                                 MapCoordinate coord,
                                                                 MapCoordinate[] retval = null)
        {
            if (retval == null || retval.Length < 4)
            {
                retval = new MapCoordinate[4];
            }

            nav.NavigateTo(GridDirection.North, coord, out retval[0]);
            nav.NavigateTo(GridDirection.East, coord, out retval[1]);
            nav.NavigateTo(GridDirection.South, coord, out retval[2]);
            nav.NavigateTo(GridDirection.West, coord, out retval[3]);
            return retval;
        }

        public static IMapNavigator<GridDirection> CreateNavigator(RenderType type)
        {
            switch (type)
            {
                case RenderType.Grid:
                    return new GridNavigator();
                case RenderType.IsoStaggered:
                    return new IsoStaggeredGridNavigator();
                case RenderType.IsoDiamond:
                    return new IsoDiamondGridNavigator();
                case RenderType.Hex:
                    throw new ArgumentException("Hex is not a supported grid navigation schema.");
                case RenderType.HexDiamond:
                    throw new ArgumentException("Hex is not a supported grid navigation schema.");
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public static MapCoordinate NavigateUnconditionally(this IMapNavigator<GridDirection> nav,
                                                            GridDirection d,
                                                            MapCoordinate coord,
                                                            int steps = 1)
        {
            nav.NavigateTo(d, coord, out MapCoordinate results, steps);
            return results;
        }
    }
}

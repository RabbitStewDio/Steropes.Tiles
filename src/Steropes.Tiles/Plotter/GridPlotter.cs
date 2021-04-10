using System;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter
{
    /// <summary>
    ///  A grid plotter is responsible for navigating the screen space during rendering. 
    /// 
    ///  The algorithm works by establishing two linked coordinate systems that are 
    ///  traveresed in lockstep. The screen coordinate system should always be a plain navigator
    ///  with no wrap-around and is used for establishing the location of rendered tiles on the 
    ///  screen, using a coodinate system where (0,0) represents the centre of the renderable area.
    ///  The map access navigator must be provided by the caller and can be a transformed navigator
    ///  (rotation, translation) and can be configured for wrap-around on any axis.
    /// 
    ///  After establishing the extend of the renderable area from the viewport, the algorithm 
    ///  iterates over the screen and map space and generates calls to the plot operation to render 
    ///  the content for the given screen/map coordinate pair.
    /// 
    ///  Warning: Due to the effects of rotation and wrap-around there is no stable relationship
    ///  between map and screen coordinates and any mapping of coordinates  from map to screen space
    ///  should be done relative to the nearest tile location.
    /// </summary>
    public class GridPlotter : IPlotter
    {
        readonly ILogAdapter logger = LogProvider.CreateLogger<GridPlotter>();
        readonly IMapNavigator<GridDirection> mapAccessNavigator;

        /// <summary>
        ///   This screen navigator is used for navigating the rendering grid.
        ///   Its function is directly related to the ScreenCoordinateMapping functions.
        /// </summary>
        readonly IMapNavigator<GridDirection> screenNavigator;

        //readonly IPlotOperation plotOperation;
        readonly IMapRenderArea viewport;

        public GridPlotter(IMapRenderArea viewport,
                           IMapNavigator<GridDirection> navigator,
                           IMapNavigator<GridDirection> screenNavigator = null)
        {
            //this.plotOperation = plotOperation ?? throw new ArgumentNullException(nameof(plotOperation));
            this.viewport = viewport ?? throw new ArgumentNullException(nameof(viewport));
            mapAccessNavigator = navigator ?? throw new ArgumentNullException(nameof(navigator));
            this.screenNavigator = screenNavigator ?? GridNavigation.CreateNavigator(viewport.RenderType);
        }

        public void Draw(IPlotOperation plotOperation)
        {
            var area = viewport.RenderedArea;
            var insets = viewport.RenderInsets;
            var renderType = viewport.RenderType;

            var screenOrigin = new MapCoordinate(0, 0);
            screenNavigator.NavigateTo(GridDirection.North, screenOrigin, out MapCoordinate northWest, insets.Top);
            screenNavigator.NavigateTo(GridDirection.West, northWest, out northWest, insets.Left);

            screenNavigator.NavigateTo(GridDirection.South, screenOrigin, out MapCoordinate southWest, insets.Bottom);
            screenNavigator.NavigateTo(GridDirection.West, southWest, out southWest, insets.Left);

            var valid = true;
            var mapOrigin = viewport.CenterPointInMapCoordinates;
            valid &= mapAccessNavigator.NavigateTo(GridDirection.North, mapOrigin, out MapCoordinate mapRowStart, insets.Top);
            valid &= mapAccessNavigator.NavigateTo(GridDirection.West, mapRowStart, out mapRowStart, insets.Left);

            var rowStart = northWest;
            if (logger.IsTraceEnabled)
            {
                logger.Trace(
                    "Rendering frame lines from {0} -> {1} in area {2} (Insets: {3}, ScreenOrigin: {4}, MapOrigin: {5})",
                    northWest, southWest, area, insets, screenOrigin, mapOrigin);
            }

            plotOperation.StartDrawing();

            for (var y = 0; y < area.Height; y += 1)
            {
                if (renderType == RenderType.Grid)
                {
                    RenderLine(plotOperation, mapRowStart, rowStart, area.Width, valid, y);
                    screenNavigator.NavigateTo(GridDirection.South, rowStart, out rowStart);
                    valid = mapAccessNavigator.NavigateTo(GridDirection.South, mapRowStart, out mapRowStart);
                }
                else
                {
                    // In any of the two isometric modes the renderer cannot just move straight downwards.
                    // Rows are offset against each other, so the renderer has to alternate between the
                    // south-east and south-west direction.
                    //
                    // On the first line, the start and end position are outwards from the screen so
                    // the line is actually two elements longer.
                    RenderLine(plotOperation, mapRowStart, rowStart, area.Width, valid, y); // was area.Width
                    screenNavigator.NavigateTo(GridDirection.SouthEast, rowStart, out rowStart);
                    valid = mapAccessNavigator.NavigateTo(GridDirection.SouthEast, mapRowStart, out mapRowStart);
                    RenderLine(plotOperation, mapRowStart, rowStart, area.Width, valid, y); // was area.Width
                    screenNavigator.NavigateTo(GridDirection.SouthWest, rowStart, out rowStart);
                    valid = mapAccessNavigator.NavigateTo(GridDirection.SouthWest, mapRowStart, out mapRowStart);
                }
            }

            plotOperation.FinishedDrawing();
        }

        void RenderLine(IPlotOperation plotOperation,
                        MapCoordinate rowStart,
                        MapCoordinate screenRowStart,
                        int width,
                        bool firstCellValid,
                        int logicalLine)
        {
            if (logger.IsTraceEnabled)
            {
                logger.Trace("Rendering line {0} / {1}with {2} tiles and valid={3}", rowStart, screenRowStart, width, firstCellValid);
                var coordinateToScreenMapper = ScreenCoordinateMapping.CreateMapToScreenMapper(viewport.RenderType);
                var r = coordinateToScreenMapper(screenRowStart.X, screenRowStart.Y);
                logger.Trace("  - start of line on screen ({0},{1})", r.X, r.Y);
            }

            plotOperation.StartLine(logicalLine, screenRowStart);

            var currentPos = rowStart;
            var screenPos = screenRowStart;
            var valid = firstCellValid;
            // render row
            // With the supported map models, it is always safe to assume that we can simply step 
            // eastwards when rendering.
            for (var x = 0; x < width; x += 1)
            {
                if (valid)
                {
                    // render next only if the navigator allows us to visit that tile.
                    // Navigation may be prevented when we hit a map-boundary when using a limiting map navigator.
                    plotOperation.RenderAt(screenPos, currentPos);
                }
                else
                {
                    if (logger.IsTraceEnabled)
                    {
                        logger.Trace("Skipping tile at {0}", currentPos);
                    }
                }

                screenNavigator.NavigateTo(GridDirection.East, screenPos, out screenPos);
                var p = currentPos;
                valid = mapAccessNavigator.NavigateTo(GridDirection.East, currentPos, out currentPos);
                if (!valid)
                {
                    valid = mapAccessNavigator.NavigateTo(GridDirection.East, p, out currentPos);
                }
            }

            if (logger.IsTraceEnabled)
            {
                var coordinateToScreenMapper = ScreenCoordinateMapping.CreateMapToScreenMapper(viewport.RenderType);
                var r = coordinateToScreenMapper(screenRowStart.X, screenRowStart.Y);
                logger.Trace("  - end of line on screen ({0},{1})", r.X, r.Y);
            }

            plotOperation.EndLine(logicalLine, screenPos);
        }
    }
}

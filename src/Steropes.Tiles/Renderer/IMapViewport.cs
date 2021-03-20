using System.ComponentModel;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Renderer
{
    /// <summary>
    ///  A restricted viewport interface for specifying the renderable area of an grid plot.
    ///  This set of properties is not affected by rotation or translation operations.
    /// </summary>
    public interface IMapRenderArea : INotifyPropertyChanged
    {
        /// <summary>
        ///   The render-type for the current viewport. This property must be read-only and
        ///   must not change during the lifetime of this object. 
        /// </summary>
        RenderType RenderType { get; }

        MapCoordinate CenterPointInMapCoordinates { get; }

        /// <summary>
        ///  The number of rendered extra tiles in each direction. This overdraw is necessary
        ///  to allow tiles to overlap each other.
        /// </summary>
        IntInsets RenderInsets { get; }

        /// <summary>
        ///  The rendered area in map coordinates. This measure is given in map units and contains
        ///  the overdraw.
        /// </summary>
        IntDimension RenderedArea { get; }
    }

    /// <summary>
    ///  A viewport defines the a screen view over an area of the map. It is a mediator between
    ///  screen coordinates and map coordinates.
    /// </summary>
    public interface IMapViewport : IMapRenderArea
    {
        /// <summary>
        ///  The focal point for the viewport. This is a continuius coordinate to allow smooth 
        ///  scrolling.
        /// </summary>
        ContinuousViewportCoordinates CenterPoint { get; }

        /// <summary>
        ///  Returns the intra-tile of the centrepoint within the current map coordinate.
        ///  The values in both x- and y-axis are in the range of [-2, +2] and are given
        ///  in viewport coordinates. 
        ///  
        ///  Not all map coordinate systems are continuous, staggered systems have jumps
        ///  between tiles. When an element moves continuously from one tile to another,
        ///  its underlying map coordinate may change abruptly. This is the reason all
        ///  movement has to be computed in screen space.
        /// </summary>
        ContinuousViewportCoordinates CenterPointOffset { get; }

        /// <summary>
        ///  The size of the viewport in full tiles.
        /// </summary>
        IntDimension SizeInTiles { get; }

        ContinuousViewportCoordinates MapPositionToScreenPosition(DoublePoint mapPosition);
        ViewportCoordinates MapPositionToScreenPosition(MapCoordinate mapPosition);

        DoublePoint ScreenPositionToMapPosition(ContinuousViewportCoordinates screenPosition);
        MapCoordinate ScreenPositionToMapCoordinate(ViewportCoordinates screenPosition);
    }
}

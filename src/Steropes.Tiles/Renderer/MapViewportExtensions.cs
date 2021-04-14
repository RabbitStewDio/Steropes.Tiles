using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Renderer
{
    public static class MapViewportExtensions
    {
        /// <summary>
        ///  Calculates the rendering offset for the given viewport. This computes the 
        ///  translation offset from the viewport coordinate system to the render coordinate
        ///  system.
        /// 
        ///  The rendering system uses an virtual screen coordinates system that 
        ///  has its origin (0,0) in the top left corner of the screen. The viewport coordinate
        ///  system's origin is at equivalent of the the map-coordinate (0,0).
        /// </summary>
        /// <param name="viewport"></param>
        /// <returns></returns>
        public static ContinuousViewportCoordinates CalculateRenderCoordinateOffset(this IMapViewport viewport)
        {
            var size = viewport.SizeInTiles;
            var fractionalOffset = viewport.CenterPointOffset;
            return CalculateRenderCoordinateOffset(size, fractionalOffset);
        }

        public static ContinuousViewportCoordinates CalculateRenderCoordinateOffset(in IntDimension size,
                                                                                    in ContinuousViewportCoordinates fractionalOffset)
        {
            var w = ContinuousViewportCoordinates.FromTileCoordinates(size.Width) / 2.0;
            var h = ContinuousViewportCoordinates.FromTileCoordinates(size.Height) / 2.0;

            // This offset is in the range of (-0.5 to +0.5)
            // we take the factional offset into account so that smooth scrolling becomes possible.
            // This way the tile for the upper left corner will be slightly offset.
            var offsetX = w - fractionalOffset.X;
            var offsetY = h - fractionalOffset.Y;
            return new ContinuousViewportCoordinates(offsetX, offsetY);
        }
    }
}

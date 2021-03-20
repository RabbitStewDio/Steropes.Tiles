using System;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Renderer
{
    /// <summary>
    ///   Shared calculations that depend on MapViewport properties but cannot be shared sanely
    ///   as in C# one cannot alter property access rights.
    /// </summary>
    public static class MapViewportBaseCalculations
    {
        /// <summary>
        ///   Returns the size of the rendered area in full tiles.
        /// </summary>
        public static IntDimension RenderedAreaOf(IntInsets viewport)
        {
            return new IntDimension(viewport.Left + viewport.Right + 1,
                                    viewport.Top + viewport.Bottom + 1);
        }

        /// <summary>
        ///   Returns the distance in steps from the centre point to the edges of the renderable screen area.
        ///   Note that this does not include the center tile itself. So the width of the area rendered is
        ///   (1 + left + right, 1 + top + bottom).
        /// </summary>
        public static IntInsets EnsureViewportValid(IntDimension size,
                                                    IntInsets overdraw)
        {
            // Uses Floor/Ceiling to resolve partial tiles to their full coordinates.
            // Even if just a part of the tile is visible, we have to render all of it.
            var halfWidth = (int)Math.Ceiling(size.Width / 2.0);
            var halfHeight = (int)Math.Ceiling(size.Height / 2.0);

            // Compensate by one at the top to account for fractional offsets when
            // using isometric tiles. When using an Isometric rendering, half the
            // line is filled with the previous line's tiles.
            //
            // This is not needed for the horizontal or bottom line due to the way
            // the grid-plotter requests screen updates. This quick and dirty calculation
            // here is a 'good enough' balance between complexity/performance and accuracy.
            return new IntInsets(Math.Max(0, halfHeight + 1 + overdraw.Top),
                                 Math.Max(0, halfWidth + overdraw.Left),
                                 Math.Max(0, halfHeight + overdraw.Bottom),
                                 Math.Max(0, halfWidth + overdraw.Right));
        }
    }
}

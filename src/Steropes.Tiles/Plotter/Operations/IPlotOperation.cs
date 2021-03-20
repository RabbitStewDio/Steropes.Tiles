using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations
{
    public interface IPlotCacheControl
    {
        /// <summary>
        ///  Cache control. It is simply easier to require these methods for all
        ///  implementations and then leave them as NoOps than to try to patch them
        ///  through in a more minimalistic interface.
        /// </summary>
        /// <param name="mapPosition"></param>
        /// <param name="range"></param>
        void Invalidate(MapCoordinate mapPosition, int range);

        void InvalidateAll();
    }

    public interface IPlotOperation : IPlotCacheControl
    {
        void StartDrawing();
        void RenderAt(MapCoordinate screenPosition, MapCoordinate mapPosition);
        void FinishedDrawing();

        void StartLine(int logicalLine, MapCoordinate screenPos);
        void EndLine(int logicalLine, MapCoordinate screenPos);
    }

    /// <summary>
    ///   A plot operation that allows access to the underlying renderer instance.
    ///   The renderer property allows to set and get the user defined renderer,
    ///   but plots can wrap such renderers into their own pre-processors where needed.
    ///   To feed data through the render chain, feel the data to the ActiveRenderer.
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IRenderPlotOperation<TRenderTile, TContext> : IPlotOperation
    {
        IRenderCallback<TRenderTile, TContext> Renderer { get; set; }

        IRenderCallback<TRenderTile, TContext> ActiveRenderer { get; }
    }
}

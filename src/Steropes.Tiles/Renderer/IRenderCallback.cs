using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Renderer
{
    /// <summary>
    /// This callback connects the plotter to the renderer. The plotter translates map coordinates into 
    /// viewport coordinates.
    /// </summary>
    /// <typeparam name="TRenderTileType"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public interface IRenderCallback<in TRenderTileType, in TContext>
    {
        void StartDrawing();

        /// <summary>
        ///  Signals the beginning of the a line batch. The batch will be centered around the screen position given with a +- offset of 0.5.
        /// </summary>
        /// <param name="logicalLine"></param>
        /// <param name="screen"></param>
        void StartLine(int logicalLine, ContinuousViewportCoordinates screen);

        /// <summary>
        /// Draws the given tile in the actual graphics engine. This interface must be implemented
        /// by the user of this rendering engine. The method here will be called multiple times 
        /// during the render process. The renderer may render multiple tiles at the same position.
        /// All calls will made in Z-order. All units given are tile-units.
        /// </summary>
        void Draw(TRenderTileType tile, TContext context, SpritePosition pos, ContinuousViewportCoordinates screenLocation);

        void EndLine(int logicalLine, ContinuousViewportCoordinates screen);

        void FinishedDrawing();
    }

    /// <summary>
    ///  Converts tile and context information from TMatchedTile and TMatchedContext to TRenderedTile and TRenderedContext.
    ///  The Matched types are produced by the data extractors that work with the model code (the source) and the 
    ///  RenderedTile and Context are what the rendering (consumer) code expects to receive.
    /// </summary>
    /// <typeparam name="TRenderedTile"></typeparam>
    /// <typeparam name="TRenderedContext"></typeparam>
    /// <typeparam name="TMatchedTile"></typeparam>
    /// <typeparam name="TMatchedContext"></typeparam>
    public interface IRenderCallbackFilter<TMatchedTile, TMatchedContext, TRenderedTile, TRenderedContext> : IRenderCallback<TMatchedTile, TMatchedContext>
    {
        IRenderCallback<TRenderedTile, TRenderedContext> RenderTarget { get; set; }
    }
}

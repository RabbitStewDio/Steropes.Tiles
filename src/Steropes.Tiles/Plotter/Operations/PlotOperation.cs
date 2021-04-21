using System;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations
{
    /// <summary>
    ///  The plot operation handles the rendering of one tile coordinate.
    /// </summary>
    /// <typeparam name="TRenderTile"></typeparam>
    /// <typeparam name="TContext"></typeparam>
    public class PlotOperation<TRenderTile, TContext> : IRenderPlotOperation<TRenderTile, TContext>
    {
        readonly RendererAdapter<TRenderTile, TContext> adapter;
        readonly ITileMatcher<TRenderTile, TContext> matcher;
        readonly TileResultCollector<TRenderTile, TContext> onMatchFound;

        public PlotOperation(ITileMatcher<TRenderTile, TContext> matcher,
                             RenderType renderType,
                             IRenderCallback<TRenderTile, TContext> renderer = null)
        {
            this.matcher = matcher;
            adapter = new RendererAdapter<TRenderTile, TContext>(renderType, renderer);
            onMatchFound = adapter.MatchFound;
        }

        public IRenderCallback<TRenderTile, TContext> Renderer
        {
            get { return adapter.Renderer; }
            set { adapter.Renderer = value; }
        }

        public IRenderCallback<TRenderTile, TContext> ActiveRenderer => Renderer;

        public void StartDrawing()
        {
            if (Renderer == null)
            {
                throw new InvalidOperationException("Cannot perform operation without renderer.");
            }

            Renderer.StartDrawing();
        }

        public void RenderAt(in MapCoordinate screenPos, in MapCoordinate mapPosition)
        {
            adapter.NextTile(screenPos.X, screenPos.Y);
            matcher.Match(mapPosition.X, mapPosition.Y, onMatchFound);
        }

        public void StartLine(int logicalLine, in MapCoordinate screenPos)
        {
            adapter.NextTile(screenPos.X, screenPos.Y);
            Renderer.StartLine(logicalLine, adapter.Screen);
        }

        public void EndLine(int logicalLine, in MapCoordinate screenPos)
        {
            adapter.NextTile(screenPos.X, screenPos.Y);
            Renderer.EndLine(logicalLine, adapter.Screen);
        }

        public void FinishedDrawing()
        {
            Renderer.FinishedDrawing();
        }

        public void Invalidate(in MapCoordinate mapPosition, int range)
        { }

        public void InvalidateAll()
        { }
    }
}

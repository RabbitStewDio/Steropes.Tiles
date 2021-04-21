using System;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations
{
    public class ContextConverterPlotOperation<TMatchedTile,
                                               TMatchedContext,
                                               TTargetTile,
                                               TTargetContext> : IRenderPlotOperation<TTargetTile, TTargetContext>
    {
        readonly IRenderCallbackFilter<TMatchedTile, TMatchedContext, TTargetTile, TTargetContext> renderer;
        readonly IRenderPlotOperation<TMatchedTile, TMatchedContext> op;

        public ContextConverterPlotOperation(IRenderPlotOperation<TMatchedTile, TMatchedContext> op,
                                             IRenderCallbackFilter<TMatchedTile, TMatchedContext, TTargetTile, TTargetContext> convertingRenderer)
        {
            this.op = op ?? throw new ArgumentNullException();
            this.renderer = convertingRenderer ?? throw new ArgumentNullException();
            if (op.Renderer != null && op.Renderer != convertingRenderer)
            {
                throw new ArgumentException();
            }

            this.op.Renderer = convertingRenderer;
        }

        protected IRenderCallback<TMatchedTile, TMatchedContext> ConverterRenderer => renderer;

        public IRenderCallback<TTargetTile, TTargetContext> Renderer
        {
            get { return renderer.RenderTarget; }
            set { renderer.RenderTarget = value; }
        }

        public virtual IRenderCallback<TTargetTile, TTargetContext> ActiveRenderer => Renderer;

        public void StartDrawing()
        {
            op.StartDrawing();
        }

        public void RenderAt(in MapCoordinate screenPosition, in MapCoordinate mapPosition)
        {
            op.RenderAt(screenPosition, mapPosition);
        }

        public void FinishedDrawing()
        {
            op.FinishedDrawing();
        }

        public void StartLine(int logicalLine, in MapCoordinate screenPos)
        {
            op.StartLine(logicalLine, screenPos);
        }

        public void EndLine(int logicalLine, in MapCoordinate screenPos)
        {
            op.EndLine(logicalLine, screenPos);
        }

        public void Invalidate(in MapCoordinate mapPosition, int range)
        {
            op.Invalidate(mapPosition, range);
        }

        public void InvalidateAll()
        {
            op.InvalidateAll();
        }
    }

    public class ViewportPlotOperation<TTargetTile, TTargetContext> : ContextConverterPlotOperation<TTargetTile, TTargetContext, TTargetTile, TTargetContext>
    {
        public ViewportPlotOperation(IRenderPlotOperation<TTargetTile, TTargetContext> op,
                                     IMapViewport viewport) : base(op, CreateViewportRenderer(viewport))
        { }

        public override IRenderCallback<TTargetTile, TTargetContext> ActiveRenderer => ConverterRenderer;

        static ViewportRenderer<TTargetTile, TTargetContext> CreateViewportRenderer(IMapViewport viewport)
        {
            return new ViewportRenderer<TTargetTile, TTargetContext>(viewport);
        }
    }
}

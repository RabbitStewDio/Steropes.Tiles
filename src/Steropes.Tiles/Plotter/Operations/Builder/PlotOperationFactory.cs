using System;
using Steropes.Tiles.Matcher;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations.Builder
{
    public class PlotOperationFactory<TRenderTile, TContext>
    {
        readonly IViewportRenderContext sharedRenderConfig;
        readonly IRenderPlotOperation<TRenderTile, TContext> plotOperation;

        public PlotOperationFactory(IViewportRenderContext sharedRenderConfig, IRenderPlotOperation<TRenderTile, TContext> plotOperation)
        {
            this.sharedRenderConfig = sharedRenderConfig ?? throw new ArgumentNullException(nameof(sharedRenderConfig));
            this.plotOperation = plotOperation;
        }

        public PlotOperationFactory<TTargetRenderTile, TTargetRenderContext>
            WithConversion<TTargetRenderTile, TTargetRenderContext>(IRenderCallbackFilter<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext> conversionRenderer)
        {
            var c = new ContextConverterPlotOperation<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext>(plotOperation, conversionRenderer);
            return new PlotOperationFactory<TTargetRenderTile, TTargetRenderContext>(sharedRenderConfig, c);
        }

        public IRenderPlotOperation<TRenderTile, TContext> Build()
        {
            return plotOperation;
        }

        public PlotOperationFactory<TRenderTile, TContext> WithRenderer(IRenderCallback<TRenderTile, TContext> renderer)
        {
            plotOperation.Renderer = renderer;
            return this;
        }
    }

    public class PlotOperationFactory
    {
        readonly IViewportRenderContext sharedRenderConfig;

        public PlotOperationFactory(IViewportRenderContext sharedRenderConfig)
        {
            this.sharedRenderConfig = sharedRenderConfig ?? throw new ArgumentNullException(nameof(sharedRenderConfig));
        }


        public CachablePlotOperationFactory<TRenderTile, TContext> Create<TRenderTile, TContext>(ITileMatcher<TRenderTile, TContext> matcher)
        {
            var plotOperation = new PlotOperation<TRenderTile, TContext>(matcher, sharedRenderConfig.RenderType);
            return new CachablePlotOperationFactory<TRenderTile, TContext>(sharedRenderConfig, plotOperation);
        }
    }
}

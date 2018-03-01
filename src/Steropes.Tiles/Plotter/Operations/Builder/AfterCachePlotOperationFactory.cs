using System;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations.Builder
{
  public class AfterCachePlotOperationFactory<TRenderTile, TContext>
  {
    readonly IViewportRenderContext sharedRenderConfig;
    readonly IRenderPlotOperation<TRenderTile, TContext> plotOperation;

    public AfterCachePlotOperationFactory(IViewportRenderContext sharedRenderConfig, IRenderPlotOperation<TRenderTile, TContext> plotOperation)
    {
      this.sharedRenderConfig = sharedRenderConfig ?? throw new ArgumentNullException(nameof(sharedRenderConfig));
      this.plotOperation = plotOperation;
    }

    public AfterCachePlotOperationFactory<TTargetRenderTile, TTargetRenderContext>
      WithConversion<TTargetRenderTile, TTargetRenderContext>(IRenderCallbackFilter<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext> conversionRenderer)
    {
      var c = new ContextConverterPlotOperation<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext>(plotOperation, conversionRenderer);
      return new AfterCachePlotOperationFactory<TTargetRenderTile, TTargetRenderContext>(sharedRenderConfig, c);
    }

    // reduces to non caching builder 
    public PlotOperationFactory<TRenderTile,TContext> ForViewport()
    {
      var retval = new ViewportPlotOperation<TRenderTile, TContext>(plotOperation, sharedRenderConfig.Viewport);
      return new PlotOperationFactory<TRenderTile, TContext>(sharedRenderConfig, retval);
    }

    public IRenderPlotOperation<TRenderTile, TContext> BuildUnsafe()
    {
      return plotOperation;
    }

    public AfterCachePlotOperationFactory<TRenderTile, TContext> WithRenderer(IRenderCallback<TRenderTile, TContext> renderer)
    {
      plotOperation.Renderer = renderer;
      return this;
    }
  }
}
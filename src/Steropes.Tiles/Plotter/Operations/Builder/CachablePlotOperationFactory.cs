using System;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles.Plotter.Operations.Builder
{
  public class CachablePlotOperationFactory<TRenderTile, TContext> 
  {
    readonly IViewportRenderContext sharedRenderConfig;
    readonly IRenderPlotOperation<TRenderTile, TContext> plotOperation;

    public CachablePlotOperationFactory(IViewportRenderContext sharedRenderConfig, IRenderPlotOperation<TRenderTile, TContext> plotOperation)
    {
      this.sharedRenderConfig = sharedRenderConfig ?? throw new ArgumentNullException(nameof(sharedRenderConfig));
      this.plotOperation = plotOperation;
    }

    public CachablePlotOperationFactory<TTargetRenderTile, TTargetRenderContext>
      WithConversion<TTargetRenderTile, TTargetRenderContext>(IRenderCallbackFilter<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext> conversionRenderer)
    {
      var c = new ContextConverterPlotOperation<TRenderTile, TContext, TTargetRenderTile, TTargetRenderContext>(plotOperation, conversionRenderer);
      return new CachablePlotOperationFactory<TTargetRenderTile, TTargetRenderContext>(sharedRenderConfig, c);
    }

    // reduce to non caching builder 
    public AfterCachePlotOperationFactory<TRenderTile,TContext> WithCache()
    {
      var retval = new CachingPlotOperation<TRenderTile, TContext>(plotOperation,
                                                                   sharedRenderConfig.MatcherNavigator,
                                                                   sharedRenderConfig.Viewport);
      return new AfterCachePlotOperationFactory<TRenderTile, TContext>(sharedRenderConfig, retval);
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

    public CachablePlotOperationFactory<TRenderTile, TContext> WithRenderer(IRenderCallback<TRenderTile, TContext> renderer)
    {
      plotOperation.Renderer = renderer;
      return this;
    }

  }
}
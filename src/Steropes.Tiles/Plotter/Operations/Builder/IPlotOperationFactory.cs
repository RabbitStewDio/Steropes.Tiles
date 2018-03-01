namespace Steropes.Tiles.Plotter.Operations.Builder
{
  public interface IPlotOperationFactory<TRenderTile, TContext>
  {
    IRenderPlotOperation<TRenderTile, TContext> Build();
  }
}
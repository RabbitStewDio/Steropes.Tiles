namespace Steropes.Tiles.Plotter.Operations.Builder
{
  public static class PlotOperations
  {
    public static PlotOperationFactory FromContext(IViewportRenderContext sharedRenderConfig)
    {
      return new PlotOperationFactory(sharedRenderConfig);
    }
  }
}
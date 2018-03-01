using System;
using Steropes.Tiles.Plotter;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles
{
  public class RenderLayer<TRenderTile, TContext>
  {
    public IRenderCallback<TRenderTile, TContext> Renderer { get; set; }
    public IPlotOperation PlotOperation { get; set; }

    public RenderLayer(IRenderCallback<TRenderTile, TContext> renderer, 
                       IPlotOperation plotOperation)
    {
      Renderer = renderer ?? throw new ArgumentNullException(nameof(renderer));
      PlotOperation = plotOperation ?? throw new ArgumentNullException(nameof(plotOperation));
    }

    public void Draw(GridPlotter plotter)
    {
      plotter.Draw(PlotOperation);
    }
  }
}
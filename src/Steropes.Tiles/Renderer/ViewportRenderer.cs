using System;
using System.ComponentModel;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Renderer
{
  /// <summary>
  /// Translates the GridPlotter's screen coordinates into coordinates suitable for rendering.
  /// </summary>
  /// <para>
  /// The grid plotter is intentionally unaware of the actual screen layout. The coordinates 
  /// it creates assume a simple coordinate system where the map tile that is in focus is 
  /// rendered at (0,0). Tiles left and top to it have negative x or y coordinates, tiles 
  /// to the right or bottom have positive coordinates.
  /// </para>
  /// <param>
  /// This ViewportRenderer offsets these raw coordinates so that (0,0) aligns with the upper
  /// left corner of the renderable area and also takes care of partial offsets. This allows
  /// the underlying IRenderCallback to use a simplified drawing method.
  /// </param>
  /// <typeparam name="TRenderType"></typeparam>
  /// <typeparam name="TContext"></typeparam>
  public class ViewportRenderer<TRenderType, TContext> : IRenderCallbackFilter<TRenderType, TContext, TRenderType, TContext>
  {
    IRenderCallback<TRenderType, TContext> renderTarget;
    readonly IMapViewport viewport;
    ContinuousViewportCoordinates offset;

    public ViewportRenderer(IMapViewport viewport)
    {
      this.viewport = viewport ?? throw new ArgumentNullException();
      this.viewport.PropertyChanged += ViewportChanged;

      ViewportChanged(this.viewport, null);
    }

    public IRenderCallback<TRenderType, TContext> RenderTarget
    {
      get { return renderTarget; }
      set { renderTarget = value; }
    }

    public void StartDrawing()
    {
      if (renderTarget == null)
      {
        throw new InvalidOperationException("No renderer defined in viewport renderer.");
      }
      renderTarget.StartDrawing();
    }

    public void StartLine(int line, ContinuousViewportCoordinates screenPos)
    {
      renderTarget.StartLine(line, screenPos + offset);
    }

    public void Draw(TRenderType tile, TContext context, SpritePosition pos, ContinuousViewportCoordinates s)
    {
      renderTarget.Draw(tile, context, pos, s + offset);
    }

    public void EndLine(int line, ContinuousViewportCoordinates screenPos)
    {
      renderTarget.EndLine(line, screenPos + offset);
    }

    public void FinishedDrawing()
    {
      renderTarget.FinishedDrawing();
    }

    void ViewportChanged(object sender, PropertyChangedEventArgs propertyChangedEventArgs)
    {
      offset = viewport.CalculateRenderCoordinateOffset();
    }
  }

}
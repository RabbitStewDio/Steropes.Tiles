using System;
using System.ComponentModel;
using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles
{
  /// <summary>
  ///  A very lightweight pixel-screen oriented view-settings control.
  ///  This allows renderer implementations to inspect the view bounds 
  /// (given in pixel units), the render type and the tile set size.
  /// 
  /// </summary>
  public interface IRendererControl: INotifyPropertyChanged
  {
    IntDimension TileSize { get; }
    RenderType RenderType { get; }
    Rect Bounds { get; }
  }

  public static class RendererControlExtensions
  {
    /// <summary>
    ///  The map viewport only deals with full tiles. We need to update the 
    /// </summary>
    /// <returns></returns>
    public static DoublePoint CalculateTileOffset(this IRendererControl viewport)
    {
      var ctr = viewport.Bounds;
      var tileSize = viewport.TileSize;

      // Calculate the number of full tiles rendered in each direction in pixel units.
      var cw = Math.Ceiling(ctr.Width / tileSize.Width) * tileSize.Width;
      var ch = Math.Ceiling(ctr.Height / tileSize.Height) * tileSize.Height;
      // Calculate excess to the available screen space (how many pixels of overdraw do we have)
      var rmx = cw - ctr.Width;
      var rmy = ch - ctr.Height;
      // .. reduce by half to get the offset on the top/left size 
      var rdx = (int)Math.Round(rmx / 2);
      var rdy = (int)Math.Round(rmy / 2);

      return new DoublePoint(-rdx + (int)ctr.X, -rdy + (int)ctr.Y);
    }

  }
}
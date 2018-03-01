using System;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Plotter
{
  public delegate ViewportCoordinates MapToScreenMapper(int x, int y);
  public delegate ContinuousViewportCoordinates ContinuousMapToScreenMapper(double x, double y);

  public delegate MapCoordinate ScreenToMapMapper(double screenX, double screenY);
  public delegate DoublePoint ContinuousScreenToMapMapper(double screenX, double screenY);

  public static class ScreenCoordinateMapping
  {
    static readonly IsoStaggeredGridNavigator staggeredNavigator = new IsoStaggeredGridNavigator();
    static readonly IsoDiamondGridNavigator diamondNavigator = new IsoDiamondGridNavigator();

    public static ScreenToMapMapper CreateToMapMapper(RenderType type)
    {
      switch (type)
      {
        case RenderType.Grid:
          return GridScreenToMapMapper;
        case RenderType.IsoStaggered:
          return IsoStaggeredScreenToMapMapper;
        case RenderType.IsoDiamond:
          return IsoDiamondScreenToMapMapper;
        case RenderType.Hex:
          return HexStaggeredScreenToMapMapper;
        case RenderType.HexDiamond:
          return HexDiamondScreenToMapMapper;
        default:
          throw new ArgumentException();
      }
    }

    public static ContinuousScreenToMapMapper CreateToContinuousMapMapper(RenderType type)
    {
      switch (type)
      {
        case RenderType.Grid:
          return ContinuousGridScreenToMapMapper;
        case RenderType.IsoStaggered:
          return ContinuousIsoStaggeredScreenToMapMapper;
        case RenderType.IsoDiamond:
          return ContinuousIsoDiamondScreenToMapMapper;
        case RenderType.Hex:
          return ContinuousHexStaggeredScreenToMapMapper;
        case RenderType.HexDiamond:
          return ContinuousHexDiamondScreenToMapMapper;
        default:
          throw new ArgumentException();
      }
    }

    public static MapToScreenMapper CreateMapToScreenMapper(RenderType type)
    {
      switch (type)
      {
        case RenderType.Grid:
          return GridMapToScreenMapper;
        case RenderType.IsoStaggered:
          return IsoStaggeredMapToScreenMapper;
        case RenderType.IsoDiamond:
          return IsoDiamondMapToScreenMapper;
        case RenderType.Hex:
          return HexStaggeredMapToScreenMapper;
        default:
          throw new ArgumentException();
      }
    }

    public static ContinuousMapToScreenMapper CreateToContinuousMapToScreenMapper(RenderType type)
    {
      switch (type)
      {
        case RenderType.Grid:
          return ContinuousGridMapToScreenMapper;
        case RenderType.IsoStaggered:
          return ContinuousIsoStaggeredMapToScreenMapper;
        case RenderType.IsoDiamond:
          return ContinuousIsoDiamondMapToScreenMapper;
        case RenderType.Hex:
          return ContinuousHexMapToScreenMapper;
        default:
          throw new ArgumentException();
      }
    }

    public static MapCoordinate GridScreenToMapMapper(double screenX, double screenY)
    {
      // Correct for half a nominal tile width, as our screen coordinates always mark 
      // the center/anchor-points of tiles.
      //
      // Using Floor(z+0.5) so that I have a reliable rounding method that always rounds towards
      // the smaller integer value. Using normal rounding modes would create inconsistencies.
      return new MapCoordinate((int) Math.Floor((screenX + 2) / 4), (int) Math.Floor((screenY + 2) / 4));
    }

    public static DoublePoint ContinuousGridScreenToMapMapper(double screenX, double screenY)
    {
      // Correct for half a nominal tile width, as our screen coordinates always mark 
      // the center/anchor-points of tiles.
      return new DoublePoint(screenX / 4, screenY / 4);
    }

    public static ViewportCoordinates GridMapToScreenMapper(int x, int y)
    {
      return new ViewportCoordinates(x * ViewportCoordinates.Scale, y * ViewportCoordinates.Scale);
    }

    public static ContinuousViewportCoordinates ContinuousGridMapToScreenMapper(double x, double y)
    {
      return new ContinuousViewportCoordinates(x * ViewportCoordinates.Scale, y * ViewportCoordinates.Scale);
    }

    public static ViewportCoordinates IsoDiamondMapToScreenMapper(int x, int y)
    {
      var screenX = (x - y) * 2;
      var screenY = (x + y) * 2;
      return new ViewportCoordinates(screenX, screenY);
    }

    public static ContinuousViewportCoordinates ContinuousIsoDiamondMapToScreenMapper(double x, double y)
    {
      var screenX = (x - y) * 2;
      var screenY = (x + y) * 2;
      return new ContinuousViewportCoordinates(screenX, screenY);
    }

    public static MapCoordinate IsoDiamondScreenToMapMapper(double screenX, double screenY)
    {

      // get the normalized grid coordinate as integer.
      // this roughly tells us the cell we are in.
      // Note: Grid coordinates are 1-unit based, 
      //       as opposed to the 4-unit based view coordinates 
      var grid = GridScreenToMapMapper(screenX, screenY);
      var mouseGridX = (grid.Y + grid.X);
      var mouseGridY = (grid.Y - grid.X);

      // remainder now at a range of [-1;+1]
      var mouseInnerX = (screenX - grid.X * 4);
      var mouseInnerY = (screenY - grid.Y * 4);

      if (Math.Abs(mouseInnerX) > 2 - Math.Abs(mouseInnerY))
      {
        var direction = Compute(mouseInnerX, mouseInnerY);
        /*
          mouseInnerY < 0
            ? mouseInnerX < 0
              ? GridDirection.NorthWest
              : GridDirection.NorthEast
            : mouseInnerX < 0
              ? GridDirection.SouthWest
              : GridDirection.SouthEast;
              */
        diamondNavigator.NavigateTo(direction,
                                    new MapCoordinate(mouseGridX, mouseGridY),
                                    out MapCoordinate result);
        return result;
      }
      return new MapCoordinate(mouseGridX, mouseGridY);
    }

    public static DoublePoint ContinuousIsoDiamondScreenToMapMapper(double screenX, double screenY)
    {
      // get the normalized grid coordinate as integer.
      // this roughly tells us the cell we are in.
      // Note: Grid coordinates are 1-unit based, 
      //       as opposed to the 4-unit based view coordinates 
      var grid = ContinuousGridScreenToMapMapper(screenX, screenY);
      var mouseGridX = (grid.Y + grid.X);
      var mouseGridY = (grid.Y - grid.X);
      return new DoublePoint(mouseGridX, mouseGridY);
    }

    public static ViewportCoordinates IsoStaggeredMapToScreenMapper(int x, int y)
    {
      int screenX;
      if (y % 2 == 0)
      {
        screenX = x * 4;
      }
      else
      {
        screenX = x * 4 + 2;
      }
      return new ViewportCoordinates(screenX, y * 2);
    }

    public static ContinuousViewportCoordinates ContinuousIsoStaggeredMapToScreenMapper(double x, double y)
    {
      double screenX;
      int iy = (int) Math.Floor(y);
      if (iy % 2 == 0)
      {
        screenX = x * 4;
      }
      else
      {
        screenX = x * 4 + 2;
      }
      return new ContinuousViewportCoordinates(screenX, y * 2);
    }

    public static MapCoordinate IsoStaggeredScreenToMapMapper(double screenX, double screenY)
    {
      var r = ContinuousIsoStaggeredScreenToMapMapper(screenX, screenY);
      return MapCoordinate.Normalize(r);
    }

    public static DoublePoint ContinuousIsoStaggeredScreenToMapMapper(double screenX, double screenY)
    {
      // get the normalized grid coordinate as integer.
      // this roughly tells us the cell we are in.
      // Note: Grid coordinates are 1-unit based, 
      //       as opposed to the 4-unit based view coordinates 
      // 
      // For Iso-Staggered maps the grid mapper already returns the (almost) correct 
      // coordinates for the central tile.
      // We still have to check whether the coordinate given is 
      // in one of the corner regions.
      var cgrid = ContinuousGridScreenToMapMapper(screenX, screenY);
      var ngrid = MapCoordinate.Normalize(cgrid);

      // offsets within the tile are not scaled.
      var dx = cgrid.X - ngrid.X;
      var dy = cgrid.Y - ngrid.Y;

      // In Staggered maps, the underlying map is stretched on the y-axis
      // compared to normal grids. On the y-axis one map-grid-cell of 1 
      // moves the position by 2.
      ngrid.Y *= 2;

      if (Math.Abs(dx) > 0.5 - Math.Abs(dy))
      {
        // This is one of the corner regions.
        var direction = Compute(dx, dy);
        staggeredNavigator.NavigateTo(direction, ngrid, out MapCoordinate result);
        var d = (0.5 - Math.Abs(dx));
        double rdx = -Math.Sign(dx) * d;
        double rdy = -Math.Sign(dy) * (0.5 - Math.Abs(dy));
        return new DoublePoint(result.X + rdx, result.Y + rdy);
      }
      return new DoublePoint(ngrid.X + dx, ngrid.Y + dy);

    }

    static GridDirection Compute(double dx, double dy)
    {
      var ypos = (dy < 0) ? 0 : 2;
      var xpos = (dx < 0) ? 0 : 1;
      var idx = ypos + xpos;
      switch (idx)
      {
        case 0:
        {
          // x and y are negative
          return GridDirection.NorthWest;
        }
        case 1:
        {
          // y negative, x positive
          return GridDirection.NorthEast;
        }
        case 2:
        {
          // y positive, x negative
          return GridDirection.SouthWest;
        }
        default:
        {
          // y positive, x positive
          return GridDirection.SouthEast;
        }
      }
    }

    public static ViewportCoordinates HexStaggeredMapToScreenMapper(int x, int y)
    {
      // todo: Placeholder code. Not correct.
      return IsoStaggeredMapToScreenMapper(x, y);
    }

    public static ContinuousViewportCoordinates ContinuousHexMapToScreenMapper(double x, double y)
    {
      // todo: Placeholder code. Not correct.
      return ContinuousIsoStaggeredMapToScreenMapper(x, y);
    }

    /// <summary>
    ///   Todo: Does not work. Must subdivide the cell so that hex-top/bottom area is 1/4 of the total height each, and
    ///   the long side area is 1/2.
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static MapCoordinate HexStaggeredScreenToMapMapper(double screenX, double screenY)
    {
      var mouseGridX = (int) Math.Floor(screenX + 0.5);
      var mouseGridY = (int) Math.Floor(screenY + 0.75 / 1.5);

      // remainder now at a range of [-1;+1]
      var mouseInnerX = (screenX - mouseGridX) * 2;
      // y is in the range of [-0.75;+0.75]
      // [-0.75;-0.5] is the first slope section (NE, or NW)
      // [-0.5;0] is the even-row hex tile
      // [0;0.25] is the lower slope section (SW or SE)
      // [0.25;0.75] is the odd-row hex tile section (SW or SE)
      var mouseInnerY = screenY - mouseGridY * 1.5;

      if (mouseInnerY < -0.5)
      {
        // upper slopes 
        var threshold = mouseInnerY * 4 / 3; // now at [-1;0]
        // in one of the corner areas ..
        if (Math.Abs(mouseInnerX) > threshold)
        {
          var direction = mouseInnerX < 0 ? GridDirection.NorthWest : GridDirection.NorthEast;
          // northwest
          staggeredNavigator.NavigateTo(direction,
                                        new MapCoordinate(mouseGridX, mouseGridY),
                                        out MapCoordinate result);
          return result;
        }
        return new MapCoordinate(mouseGridX, mouseGridY);
      }

      if (mouseInnerY < 0)
      {
        // centre
        return new MapCoordinate(mouseGridX, mouseGridY);
      }

      // maps the x-position to the threshold where we'd leave the central tile.
      //
      // Provides a simple comparison against inner-Y. This is possible because 
      // inner-x is normalized to (-1,+1) and avoids some div/0 checks.
      var v = -0.25 * Math.Abs(mouseInnerX) + 0.25;
      if (v < mouseInnerY)
      {
        // central
        return new MapCoordinate(mouseGridX, mouseGridY);
      }
      {
        var direction = mouseInnerX < 0 ? GridDirection.SouthWest : GridDirection.SouthEast;
        staggeredNavigator.NavigateTo(direction,
                                      new MapCoordinate(mouseGridX, mouseGridY),
                                      out MapCoordinate result);
        return result;
      }
    }
    /// <summary>
    ///   Todo: Does not work. Must subdivide the cell so that hex-top/bottom area is 1/4 of the total height each, and
    ///   the long side area is 1/2.
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static DoublePoint ContinuousHexStaggeredScreenToMapMapper(double screenX, double screenY)
    {
      var mouseGridX = (int) Math.Floor(screenX + 0.5);
      var mouseGridY = (int) Math.Floor(screenY + 0.75 / 1.5);

      // remainder now at a range of [-1;+1]
      var mouseInnerX = (screenX - mouseGridX) * 2;
      // y is in the range of [-0.75;+0.75]
      // [-0.75;-0.5] is the first slope section (NE, or NW)
      // [-0.5;0] is the even-row hex tile
      // [0;0.25] is the lower slope section (SW or SE)
      // [0.25;0.75] is the odd-row hex tile section (SW or SE)
      var mouseInnerY = screenY - mouseGridY * 1.5;

      if (mouseInnerY < -0.5)
      {
        // upper slopes 
        var threshold = mouseInnerY * 4 / 3; // now at [-1;0]
        // in one of the corner areas ..
        if (Math.Abs(mouseInnerX) > threshold)
        {
          var direction = mouseInnerX < 0 ? GridDirection.NorthWest : GridDirection.NorthEast;
          // northwest
          staggeredNavigator.NavigateTo(direction,
                                        new MapCoordinate(mouseGridX, mouseGridY),
                                        out MapCoordinate result);
          return new DoublePoint(result.X, result.Y);
        }
        return new DoublePoint(mouseGridX, mouseGridY);
      }

      if (mouseInnerY < 0)
      {
        // centre
        return new DoublePoint(mouseGridX, mouseGridY);
      }

      // maps the x-position to the threshold where we'd leave the central tile.
      //
      // Provides a simple comparison against inner-Y. This is possible because 
      // inner-x is normalized to (-1,+1) and avoids some div/0 checks.
      var v = -0.25 * Math.Abs(mouseInnerX) + 0.25;
      if (v < mouseInnerY)
      {
        // central
        return new DoublePoint(mouseGridX, mouseGridY);
      }
      {
        var direction = mouseInnerX < 0 ? GridDirection.SouthWest : GridDirection.SouthEast;
        staggeredNavigator.NavigateTo(direction,
                                      new MapCoordinate(mouseGridX, mouseGridY),
                                      out MapCoordinate result);
        return new DoublePoint(result.X, result.Y);
      }
    }

    /// <summary>
    ///   Todo: Does not work. Must subdivide the cell so that hex-top/bottom area is 1/4 of the total height each, and
    ///   the long side area is 1/2.
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static MapCoordinate HexDiamondScreenToMapMapper(double screenX, double screenY)
    {
      var mouseGridX = (int) Math.Floor(screenX + 0.5);
      var mouseGridY = (int) Math.Floor(screenY + 0.75 / 1.5);

      // remainder now at a range of [-1;+1]
      var mouseInnerX = (screenX - mouseGridX) * 2;
      // y is in the range of [-0.75;+0.75]
      // [-0.75;-0.5] is the first slope section (NE, or NW)
      // [-0.5;0] is the even-row hex tile
      // [0;0.25] is the lower slope section (SW or SE)
      // [0.25;0.75] is the odd-row hex tile section (SW or SE)
      var mouseInnerY = screenY - mouseGridY * 1.5;

      if (mouseInnerY < -0.5)
      {
        // upper slopes 
        var threshold = mouseInnerY * 4 / 3; // now at [-1;0]
        // in one of the corner areas ..
        if (Math.Abs(mouseInnerX) > threshold)
        {
          var direction = mouseInnerX < 0 ? GridDirection.NorthWest : GridDirection.NorthEast;
          // northwest
          diamondNavigator.NavigateTo(direction,
                                        new MapCoordinate(mouseGridX, mouseGridY),
                                        out MapCoordinate result);
          return result;
        }
        return new MapCoordinate(mouseGridX, mouseGridY);
      }

      if (mouseInnerY < 0)
      {
        // centre
        return new MapCoordinate(mouseGridX, mouseGridY);
      }

      // maps the x-position to the threshold where we'd leave the central tile.
      //
      // Provides a simple comparison against inner-Y. This is possible because 
      // inner-x is normalized to (-1,+1) and avoids some div/0 checks.
      var v = -0.25 * Math.Abs(mouseInnerX) + 0.25;
      if (v < mouseInnerY)
      {
        // central
        return new MapCoordinate(mouseGridX, mouseGridY);
      }
      {
        var direction = mouseInnerX < 0 ? GridDirection.SouthWest : GridDirection.SouthEast;
        diamondNavigator.NavigateTo(direction,
                                      new MapCoordinate(mouseGridX, mouseGridY),
                                      out MapCoordinate result);
        return result;
      }
    }

    /// <summary>
    ///   Todo: Does not work. Must subdivide the cell so that hex-top/bottom area is 1/4 of the total height each, and
    ///   the long side area is 1/2.
    /// </summary>
    /// <param name="screenX"></param>
    /// <param name="screenY"></param>
    /// <param name="x"></param>
    /// <param name="y"></param>
    public static DoublePoint ContinuousHexDiamondScreenToMapMapper(double screenX, double screenY)
    {
      var mouseGridX = (int) Math.Floor(screenX + 0.5);
      var mouseGridY = (int) Math.Floor(screenY + 0.75 / 1.5);

      // remainder now at a range of [-1;+1]
      var mouseInnerX = (screenX - mouseGridX) * 2;
      // y is in the range of [-0.75;+0.75]
      // [-0.75;-0.5] is the first slope section (NE, or NW)
      // [-0.5;0] is the even-row hex tile
      // [0;0.25] is the lower slope section (SW or SE)
      // [0.25;0.75] is the odd-row hex tile section (SW or SE)
      var mouseInnerY = screenY - mouseGridY * 1.5;

      if (mouseInnerY < -0.5)
      {
        // upper slopes 
        var threshold = mouseInnerY * 4 / 3; // now at [-1;0]
        // in one of the corner areas ..
        if (Math.Abs(mouseInnerX) > threshold)
        {
          var direction = mouseInnerX < 0 ? GridDirection.NorthWest : GridDirection.NorthEast;
          // northwest
          diamondNavigator.NavigateTo(direction,
                                        new MapCoordinate(mouseGridX, mouseGridY),
                                        out MapCoordinate result);
          return new DoublePoint(result.X, result.Y);
        }
        return new DoublePoint(mouseGridX, mouseGridY);
      }

      if (mouseInnerY < 0)
      {
        // centre
        return new DoublePoint(mouseGridX, mouseGridY);
      }

      // maps the x-position to the threshold where we'd leave the central tile.
      //
      // Provides a simple comparison against inner-Y. This is possible because 
      // inner-x is normalized to (-1,+1) and avoids some div/0 checks.
      var v = -0.25 * Math.Abs(mouseInnerX) + 0.25;
      if (v < mouseInnerY)
      {
        // central
        return new DoublePoint(mouseGridX, mouseGridY);
      }
      {
        var direction = mouseInnerX < 0 ? GridDirection.SouthWest : GridDirection.SouthEast;
        diamondNavigator.NavigateTo(direction,
                                      new MapCoordinate(mouseGridX, mouseGridY),
                                      out MapCoordinate result);
        return new DoublePoint(result.X, result.Y);
      }
    }
  }
}
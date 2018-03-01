using Steropes.Tiles.DataStructures;

namespace Steropes.Tiles.Matcher.Sprites
{
  /// <summary>
  /// Defines the relative sprite render position. This system assumes that a 
  /// tile has a size of 1x1 units and that fractional units are allowed. The
  /// position provides the anchor alignment point for the tile. 
  /// </summary>
  public enum SpritePosition
  {
    /// <summary>
    ///  Render at 0, 0. 
    ///  <para/>
    ///  This is the normal position without any offset correction.
    /// </summary>
    Whole = 0,

    /// <summary>
    ///  Iso: 0, -0.25.
    ///  Grid: -0.25, -0.25.
    ///  <para/>
    ///  Render the northern/ northwestern quadrant of the tile cell.
    /// </summary>
    Up = 1,

    /// <summary>
    ///  Iso: 0.25, 0.
    ///  Grid: 0.25, -0.25.
    ///  <para/>
    ///  Render the eastern/ northeastern quadrant of the tile cell.
    /// </summary>
    Right = 2,
    /// <summary>
    ///  Iso: 0, 0.25
    ///  Grid: 0.25, 0.25.
    ///  <para/>
    ///  Render the southern/ Southeast quadrant of the tile cell.
    /// </summary>
    Down = 3,
    /// <summary>
    ///  Iso: -0.25, 0
    ///  Grid: -0.25, +0.25.
    ///  <para/>
    ///  Render the west / south west quadrant of the tile cell.
    /// </summary>
    Left = 4,

    /// <summary>
    ///  Iso: Render at 0, -0.5. 
    ///  Grid: Render at -0.5, 0.5.
    ///  <para/>
    ///  This aligns the tile centre point with the point where the four northern 
    /// </summary>
    CellMap = 5,
  }

  public static class SpritePositionExtensions
  {
    public static ViewportCoordinates[] OffsetsFor(RenderType t)
    {
      var p = new ViewportCoordinates[6];
      p[(int) SpritePosition.Whole] = OffsetFor(SpritePosition.Whole, t);
      p[(int) SpritePosition.Up] = OffsetFor(SpritePosition.Up, t);
      p[(int) SpritePosition.Right] = OffsetFor(SpritePosition.Right, t);
      p[(int) SpritePosition.Down] = OffsetFor(SpritePosition.Down, t);
      p[(int) SpritePosition.Left] = OffsetFor(SpritePosition.Left, t);
      p[(int) SpritePosition.CellMap] = OffsetFor(SpritePosition.CellMap, t);
      return p;
    }

    public static ViewportCoordinates OffsetFor(this SpritePosition p, RenderType t)
    {
      ViewportCoordinates result;
      OffsetFor(p, t, out result);
      return result;
    }

    public static void OffsetFor(this SpritePosition p, RenderType t, out ViewportCoordinates result)
    {
      if (t.IsIsometric())
      {
        switch (p)
        {
          case SpritePosition.CellMap:
            result = new ViewportCoordinates(0, 2);
            break;
          case SpritePosition.Whole:
            result = new ViewportCoordinates(0, 0);
            break;
          case SpritePosition.Up:
            result = new ViewportCoordinates(0, -1);
            break;
          case SpritePosition.Right:
            result = new ViewportCoordinates(1, 0);
            break;
          case SpritePosition.Down:
            result = new ViewportCoordinates(0, 1);
            break;
          case SpritePosition.Left:
            result = new ViewportCoordinates(-1, 0);
            break;
          default:
            result = new ViewportCoordinates(0, 0);
            break;
        }
      }
      else
      {
        switch (p)
        {
          case SpritePosition.CellMap:
            result = new ViewportCoordinates(2, -2);
            break;
          case SpritePosition.Whole:
            result = new ViewportCoordinates(0, 0);
            break;
          case SpritePosition.Up:
            result = new ViewportCoordinates(-1, -1);
            break;
          case SpritePosition.Right:
            result = new ViewportCoordinates(1, -1);
            break;
          case SpritePosition.Down:
            result = new ViewportCoordinates(1, 1);
            break;
          case SpritePosition.Left:
            result = new ViewportCoordinates(-1, 1);
            break;
          default:
            result = new ViewportCoordinates(0, 0);
            break;
        }
      }
    }
  }
}
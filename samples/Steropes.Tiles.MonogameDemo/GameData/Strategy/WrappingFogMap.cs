using System;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.MonogameDemo.GameData.Strategy
{
  public class WrappingFogMap: IFogMap
  {
    readonly IMapNavigator<GridDirection> navigator;
    readonly IFogMap parent;

    public WrappingFogMap(IFogMap parent, IMapNavigator<GridDirection> navigator)
    {
      this.navigator = navigator;
      this.parent = parent ?? throw new ArgumentNullException(nameof(parent));
    }

    public int Width
    {
      get { return parent.Width; }
    }

    public int Height
    {
      get { return parent.Height; }
    }

    public FogState this[int x, int y]
    {
      get
      {
        navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
        return parent[c.X, c.Y];
      }
    }

    public void MarkExplored(int x, int y)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.MarkExplored(c.X, c.Y);
    }

    public void MarkUnexplored(int x, int y)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.MarkUnexplored(c.X, c.Y);
    }

    public void MarkRangeExplored(int x, int y, int radius)
    {
      var upperX = x + radius;
      var upperY = y + radius;

      for (int ty = y - radius; ty <= upperY; ty += 1)
      {
        for (int tx = x - radius; tx <= upperX; tx += 1)
        {
          UpdateExplored(tx, ty, true);
        }
      }
      FireMapDataChanged(x, y, radius);
    }

    public void MarkRangeUnexplored(int x, int y, int radius)
    {
      var upperX = x + radius;
      var upperY = y + radius;

      for (int ty = y - radius; ty <= upperY; ty += 1)
      {
        for (int tx = x - radius; tx <= upperX; tx += 1)
        {
          UpdateExplored(tx, ty, false);
        }
      }
      FireMapDataChanged(x, y, radius);
    }

    public void UpdateExplored(int x, int y, bool val)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.UpdateExplored(c.X, c.Y, val);
    }

    public void MarkVisible(int x, int y)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.MarkVisible(c.X, c.Y);
      FireMapDataChanged(x, y);
    }

    public void MarkInvisible(int x, int y)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.MarkInvisible(c.X, c.Y);
      FireMapDataChanged(x, y);
    }

    /// <summary>
    ///  Marks any tile visible that can be reached with ${radius} moves.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    public void MarkRangeVisible(int x, int y, int radius)
    {
      var upperX = x + radius;
      var upperY = y + radius;

      for (int ty = y - radius; ty <= upperY; ty += 1)
      {
        for (int tx = x - radius; tx <= upperX; tx += 1)
        {
          UpdateVisible(tx, ty, 1);
        }
      }
      FireMapDataChanged(x, y, radius);
    }

    /// <summary>
    ///  Marks any tile visible that can be reached with ${radius} moves.
    /// </summary>
    /// <param name="x"></param>
    /// <param name="y"></param>
    /// <param name="radius"></param>
    public void MarkRangeInvisible(int x, int y, int radius)
    {
      var upperX = x + radius;
      var upperY = y + radius;

      for (int ty = y - radius; ty <= upperY; ty += 1)
      {
        for (int tx = x - radius; tx <= upperX; tx += 1)
        {
          UpdateVisible(tx, ty, -1);
        }
      }
      FireMapDataChanged(x, y, radius);
    }

    public void UpdateVisible(int x, int y, short val)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.UpdateVisible(c.X, c.Y, val);
    }

    public event EventHandler<MapDataChangedEventArgs> MapDataChanged
    {
      add { parent.MapDataChanged += value; }
      remove { parent.MapDataChanged -= value; }
    }

    public void FireMapDataChanged(int x, int y, int range = 1)
    {
      navigator.NavigateTo(GridDirection.None, new MapCoordinate(x, y), out MapCoordinate c);
      parent.FireMapDataChanged(c.X, c.Y, range);
    }
  }
}
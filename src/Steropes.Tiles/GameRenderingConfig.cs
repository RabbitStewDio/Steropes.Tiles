using JetBrains.Annotations;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;
using Steropes.Tiles.Renderer;

namespace Steropes.Tiles
{
  public interface IViewportRenderContext
  {
    IMapViewport Viewport { get; }
    RenderType RenderType { get; }
    IMapNavigator<GridDirection> MatcherNavigator { get; }
  }

  public class GameRenderingConfig : IViewportRenderContext, INotifyPropertyChanged
  {
    readonly RotationGridNavigator navigator;
    readonly TransformingMapViewport transformingMapViewport;
    readonly MapViewport viewport;
    int rotationSteps;

    public GameRenderingConfig(RenderType renderType,
                               Range? wrapAroundX = null,
                               Range? wrapAroundY = null,
                               Range? limitX = null,
                               Range? limitY = null)
    {
      WrapAroundX = wrapAroundX;
      WrapAroundY = wrapAroundY;
      LimitX = limitX;
      LimitY = limitY;
      RenderType = renderType;

      viewport = new MapViewport(RenderType)
      {
        SizeInTiles = new IntDimension(10, 10),
        Overdraw = new IntInsets(0, 0, 1, 0)
      };

      transformingMapViewport = new TransformingMapViewport(viewport);
      transformingMapViewport.PropertyChanged += OnViewportPropertyChanged;
      navigator = CreateNavigator(renderType, true);
      NonWrapAroundNavigator = CreateNavigator(renderType, false);
      MatcherNavigator = CreateMatcherNavigator();
    }


    public RotationGridNavigator NonWrapAroundNavigator { get; }

    public Range? WrapAroundX { get; }
    public Range? WrapAroundY { get; }
    public Range? LimitX { get; }
    public Range? LimitY { get; }

    public IMapNavigator<GridDirection> MatcherNavigator { get; }

    public RenderType RenderType { get; }

    public int RotationSteps
    {
      get { return rotationSteps; }
      set
      {
        if (rotationSteps == value)
        {
          return;
        }

        rotationSteps = value % 4;
        transformingMapViewport.RotationDeg = -90 * RotationSteps;
        navigator.RotationSteps = 2 * RotationSteps;
        OnPropertyChanged();
      }
    }

    public IntInsets Overdraw
    {
      get { return transformingMapViewport.Overdraw; }
      set { transformingMapViewport.Overdraw = value; }
    }

    public MapCoordinate CenterPointInMapCoordinates
    {
      get { return transformingMapViewport.CenterPointInMapCoordinates; }
      set { transformingMapViewport.CenterPointInMapCoordinates = value; }
    }

    public ContinuousViewportCoordinates CenterPoint
    {
      get { return transformingMapViewport.CenterPoint; }
      set { transformingMapViewport.CenterPoint = value; }
    }

    public IntDimension Size
    {
      get { return transformingMapViewport.SizeInTiles; }
      set { transformingMapViewport.SizeInTiles = value; }
    }

    public IMapViewport Viewport
    {
      get { return transformingMapViewport; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    void OnViewportPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(transformingMapViewport.Overdraw) ||
          e.PropertyName == nameof(transformingMapViewport.CenterPointInMapCoordinates) ||
          e.PropertyName == nameof(transformingMapViewport.CenterPoint))
      {
        OnPropertyChanged(e.PropertyName);
      }
      else if (e.PropertyName == nameof(transformingMapViewport.SizeInTiles))
      {
        OnPropertyChanged(nameof(Size));
      }
    }

    public GridPlotter CreatePlotter()
    {
      return new GridPlotter(Viewport, navigator);
    }

    RotationGridNavigator CreateNavigator(RenderType renderType, bool wrap)
    {
      var baseNav = GridNavigation.CreateNavigator(renderType);
      if (LimitX != null || LimitY != null)
      {
        var limitX = LimitX ?? new Range(int.MinValue, int.MinValue);
        var limitY = LimitY ?? new Range(int.MinValue, int.MinValue);
        baseNav = new LimitedRangeNavigator<GridDirection>(baseNav, limitX.Min, limitY.Min, limitX.Max, limitY.Max);
      }
      else if (!wrap && (WrapAroundX != null || WrapAroundY != null))
      {
        var limitX = WrapAroundX ?? new Range(int.MinValue, int.MinValue);
        var limitY = WrapAroundY ?? new Range(int.MinValue, int.MinValue);
        baseNav = new LimitedRangeNavigator<GridDirection>(baseNav, limitX.Min, limitY.Min, limitX.Max, limitY.Max);
      }

      if (wrap)
      {
        var wrapNav = baseNav.Wrap(WrapAroundX, WrapAroundY);
        return new RotationGridNavigator(wrapNav, RotationSteps * 2);
      }

      return new RotationGridNavigator(baseNav, RotationSteps * 2);
    }

    IMapNavigator<GridDirection> CreateMatcherNavigator()
    {
      if (RenderType.IsIsometric())
      {
        // Cardinality matches always match against the visual major axes. 
        // For iso/hex systems, this is NE, NW, SE, SW. For plain grids it
        // is N,E,S,W in map coordinates.
        return new FixedOffsetRotationGridNavigator(navigator, 1);
      }
      return navigator;
    }

    public IMapViewport BaseViewport()
    {
      return viewport;
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.Xna.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using Steropes.Tiles.Plotter;
using Steropes.Tiles.Plotter.Operations;
using Steropes.Tiles.Properties;

namespace Steropes.Tiles.Monogame
{
  public class GameRendering : DrawableGameComponent, IViewportControl, IReadOnlyList<IPlotOperation>
  {
    readonly GameRenderingConfig config;
    readonly IPlotter gridPlotter;
    readonly List<IPlotOperation> plotters;

    public GameRendering(Game game,
                         GameRenderingConfig config,
                         RendererControl renderControl) : this(game, config, renderControl, config.CreatePlotter())
    {
    }

    public GameRendering(Game game,
                         GameRenderingConfig config,
                         RendererControl renderControl,
                         IPlotter gridPlotter) : base(game)
    {
      this.gridPlotter = gridPlotter ?? throw new ArgumentNullException(nameof(gridPlotter));
      this.config = config ?? throw new ArgumentNullException(nameof(config));
      config.PropertyChanged += OnRenderingConfigPropertyChanged;

      RenderControl = renderControl ?? throw new ArgumentNullException(nameof(renderControl));
      RenderControl.PropertyChanged += OnRenderPropertyChanged;

      if (config.RenderType != renderControl.RenderType)
      {
        throw new ArgumentException("RenderConfig render type and renderControl render type must match.");
      }

      RotationSteps = 0;
      CenterPointInMapCoordinates = new MapCoordinate(0, 0);

      plotters = new List<IPlotOperation>();
    }

    public RendererControl RenderControl { get; }

    public IntDimension Size
    {
      get { return config.Size; }
      set
      {
        var oldBounds = RenderControl.Bounds;
        RenderControl.Bounds = new Rect(oldBounds.X, oldBounds.Y, value.Width, value.Height);
      }
    }

    public Rect Bounds
    {
      get { return RenderControl.Bounds; }
      set { RenderControl.Bounds = value; }
    }

    public RenderType RenderType
    {
      get { return config.RenderType; }
    }

    public IntInsets Overdraw
    {
      get { return config.Overdraw; }
      set { config.Overdraw = value; }
    }

    public IntDimension TileSize
    {
      get { return RenderControl.TileSize; }
    }

    public int RotationSteps
    {
      get { return config.RotationSteps; }
      set { config.RotationSteps = value; }
    }

    public ContinuousViewportCoordinates CenterPoint
    {
      get { return config.CenterPoint; }
      set { config.CenterPoint = value; }
    }

    public MapCoordinate CenterPointInMapCoordinates
    {
      get { return config.CenterPointInMapCoordinates; }
      set { config.CenterPointInMapCoordinates = value; }
    }

    public ContinuousViewportCoordinates MapPositionToScreenPosition(DoublePoint mapPosition)
    {
      return config.Viewport.MapPositionToScreenPosition(mapPosition);
    }

    public ViewportCoordinates MapPositionToScreenPosition(MapCoordinate mapPosition)
    {
      return config.Viewport.MapPositionToScreenPosition(mapPosition);
    }

    public DoublePoint ScreenPositionToMapPosition(ContinuousViewportCoordinates screenPosition)
    {
      return config.Viewport.ScreenPositionToMapPosition(screenPosition);
    }

    public MapCoordinate ScreenPositionToMapCoordinate(ViewportCoordinates screenPosition)
    {
      return config.Viewport.ScreenPositionToMapCoordinate(screenPosition);
    }

    public IMapNavigator<GridDirection> MapNavigator
    {
      get { return config.MatcherNavigator; }
    }

    public event PropertyChangedEventHandler PropertyChanged;

    void OnRenderingConfigPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      OnPropertyChanged(e.PropertyName);
    }

    void OnRenderPropertyChanged(object sender, PropertyChangedEventArgs e)
    {
      if (e.PropertyName == nameof(IRendererControl.Bounds))
      {
        config.Size = CalculateViewportSize(RenderControl.Bounds);
        OnPropertyChanged(nameof(Bounds));
      }
    }

    public override void Draw(GameTime gameTime)
    {
      var state = Game.GraphicsDevice.SaveState();

      base.Draw(gameTime);
      foreach (var plotter in plotters)
      {
        gridPlotter.Draw(plotter);
      }
      state.RestoreState();
    }

    public void AddLayers(IEnumerable<IPlotOperation> ops)
    {
      foreach (var op in ops)
      {
        plotters.Add(op);
      }
    }

    public void AddLayer(IPlotOperation op)
    {
      plotters.Add(op);
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public IEnumerator<IPlotOperation> GetEnumerator()
    {
      return plotters.GetEnumerator();
    }

    public IPlotOperation this[int idx]
    {
      get { return plotters[idx]; }
    }

    public int Count => plotters.Count;

    IntDimension CalculateViewportSize(Rect r)
    {
      return new IntDimension((int) Math.Ceiling(r.Width / TileSize.Width),
                              (int) Math.Ceiling(r.Height / TileSize.Height));
    }

    [NotifyPropertyChangedInvocator]
    protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
    {
      PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
    }
  }
}
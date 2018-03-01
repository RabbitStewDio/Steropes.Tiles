using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles
{
  public interface IViewportControl: IRendererControl
  {
    int RotationSteps { get; set; }
    IntInsets Overdraw { get; set; }

    /// <summary>
    ///  The current center of the view (where the camera is focused at) in 
    ///  screen tile coordinates.
    /// </summary>
    ContinuousViewportCoordinates CenterPoint { get; set; }

    MapCoordinate CenterPointInMapCoordinates { get; set; }

    ContinuousViewportCoordinates MapPositionToScreenPosition(DoublePoint mapPosition);
    ViewportCoordinates MapPositionToScreenPosition(MapCoordinate mapPosition);

    DoublePoint ScreenPositionToMapPosition(ContinuousViewportCoordinates screenPosition);
    MapCoordinate ScreenPositionToMapCoordinate(ViewportCoordinates screenPosition);

    IMapNavigator<GridDirection> MapNavigator { get; }
  }
}
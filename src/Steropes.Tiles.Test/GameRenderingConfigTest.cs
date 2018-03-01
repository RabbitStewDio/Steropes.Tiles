using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test
{
  public class GameRenderingConfigTest
  {
    /// <summary>
    ///  Validates that Isometric navigators match against tile edges. In Isometric
    ///  view the map is rotated by 45 degrees, so a cardinal matcher's north direction 
    ///  would be executed as NorthEast, and so on. This is not what we want when we
    ///  match tiles for rendering.
    /// </summary>
    [Test]
    public void MatchNavigator_Fits_Cardinals()
    {
      var c = new GameRenderingConfig(RenderType.IsoDiamond);
      var nav = c.MatcherNavigator;
      var origin = new MapCoordinate(0, 0);
      var res = nav.NavigateCardinalNeighbours(origin);

      var referenceNav = GridNavigation.CreateNavigator(RenderType.Grid);
      res[(int)CardinalIndex.North].Should().Be(referenceNav.NavigateUnconditionally(GridDirection.North, origin));
      res[(int)CardinalIndex.East].Should().Be(referenceNav.NavigateUnconditionally(GridDirection.East, origin));
      res[(int)CardinalIndex.South].Should().Be(referenceNav.NavigateUnconditionally(GridDirection.South, origin));
      res[(int)CardinalIndex.West].Should().Be(referenceNav.NavigateUnconditionally(GridDirection.West, origin));
    }
  }
}
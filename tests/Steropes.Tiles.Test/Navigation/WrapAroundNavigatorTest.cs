using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test.Navigation
{

  [TestFixture]
  public class WrapAroundNavigatorTest
  {
    [Test]
    public void UpperLimitTest()
    {
      var nav = Substitute.For<IMapNavigator<GridDirection>>();
      var input = new MapCoordinate(99, 0);
      nav.NavigateTo(GridDirection.East, input, out MapCoordinate dummy, 1).Returns(x =>
      {
        x[2] = new MapCoordinate(100, 0);
        return true;
      });

      var w = nav.Wrap(new Range(0, 100), new Range(0, 100));

      w.NavigateTo(GridDirection.East, input, out MapCoordinate m, 1).Should().BeTrue();
      m.Should().Be(new MapCoordinate(0, 0));
    }

    [Test]
    public void LowerLimitTest()
    {
      var nav = Substitute.For<IMapNavigator<GridDirection>>();
      var input = new MapCoordinate(99, 0);
      nav.NavigateTo(GridDirection.North, input, out MapCoordinate dummy, 1).Returns(x =>
      {
        x[2] = new MapCoordinate(99, -1);
        return true;
      });

      var w = nav.Wrap(new Range(0, 100), new Range(0, 100));

      w.NavigateTo(GridDirection.North, input, out MapCoordinate m, 1).Should().BeTrue();
      m.Should().Be(new MapCoordinate(99, 99));
    }
  }
}
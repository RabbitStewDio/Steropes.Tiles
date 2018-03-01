using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test.Navigation
{
  public class RotationNavigatorTest
  {
    [Test]
    public static void RotateZero()
    {
      RotationGridNavigator.RotateBy(GridDirection.None, 0).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, 0).Should().Be(GridDirection.North);
    }

    [Test]
    public static void RotateMultiple()
    {
      RotationGridNavigator.RotateBy(GridDirection.None, 8).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, 8).Should().Be(GridDirection.North);
      RotationGridNavigator.RotateBy(GridDirection.None, 16).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, 16).Should().Be(GridDirection.North);
      RotationGridNavigator.RotateBy(GridDirection.None, 17).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, 17).Should().Be(GridDirection.NorthEast);
    }

    [Test]
    public static void RotateClockWise()
    {
      RotationGridNavigator.RotateBy(GridDirection.None, 1).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, 1).Should().Be(GridDirection.NorthEast);
      RotationGridNavigator.RotateBy(GridDirection.NorthWest, 1).Should().Be(GridDirection.North);
      RotationGridNavigator.RotateBy(GridDirection.West, 1).Should().Be(GridDirection.NorthWest);
      RotationGridNavigator.RotateBy(GridDirection.SouthWest, 1).Should().Be(GridDirection.West);
      RotationGridNavigator.RotateBy(GridDirection.South, 1).Should().Be(GridDirection.SouthWest);
      RotationGridNavigator.RotateBy(GridDirection.SouthEast, 1).Should().Be(GridDirection.South);
      RotationGridNavigator.RotateBy(GridDirection.East, 1).Should().Be(GridDirection.SouthEast);
      RotationGridNavigator.RotateBy(GridDirection.NorthEast, 1).Should().Be(GridDirection.East);
    }

    [Test]
    public static void RotateCounterClockWise()
    {
      RotationGridNavigator.RotateBy(GridDirection.None, -1).Should().Be(GridDirection.None);
      RotationGridNavigator.RotateBy(GridDirection.North, -1).Should().Be(GridDirection.NorthWest);
      RotationGridNavigator.RotateBy(GridDirection.NorthWest, -1).Should().Be(GridDirection.West);
      RotationGridNavigator.RotateBy(GridDirection.West, -1).Should().Be(GridDirection.SouthWest);
      RotationGridNavigator.RotateBy(GridDirection.SouthWest, -1).Should().Be(GridDirection.South);
      RotationGridNavigator.RotateBy(GridDirection.South, -1).Should().Be(GridDirection.SouthEast);
      RotationGridNavigator.RotateBy(GridDirection.SouthEast, -1).Should().Be(GridDirection.East);
      RotationGridNavigator.RotateBy(GridDirection.East, -1).Should().Be(GridDirection.NorthEast);
      RotationGridNavigator.RotateBy(GridDirection.NorthEast, -1).Should().Be(GridDirection.North);
    }
  }
}
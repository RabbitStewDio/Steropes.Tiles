using System.Collections.Generic;
using NUnit.Framework;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test.Navigation
{
  public class GridNavigationTest
  {
    public static IEnumerable<NavigationTestCase> TestData
    {
      get
      {
        var data = new NavigationTestDataGenerator(new MapCoordinate(0, 0));
        yield return data.CaseData(GridDirection.NorthWest, -1, -1);
        yield return data.CaseData(GridDirection.North, 0, -1);
        yield return data.CaseData(GridDirection.NorthEast, 1, -1);
        yield return data.CaseData(GridDirection.East, 1, 0);
        yield return data.CaseData(GridDirection.SouthEast, 1, 1);
        yield return data.CaseData(GridDirection.South, 0, 1);
        yield return data.CaseData(GridDirection.SouthWest, -1, 1);
        yield return data.CaseData(GridDirection.West, -1, 0);
        yield return data.CaseData(GridDirection.None, 0, 0);
      }
    }

    [Test]
    public void ValidateNavigation()
    {
      var nav = new GridNavigator();
      TestData.ValidateAll(nav);
    }
  }
}
using System.Collections.Generic;
using NUnit.Framework;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test.Navigation
{
  public class IsoStaggeredGridNavigationTest
  {
    public static IEnumerable<NavigationTestCase> EvenColsEvenRowsTestData
    {
      get
      {
        var data = new NavigationTestDataGenerator(new MapCoordinate(0, 0));
        yield return data.CaseData(GridDirection.NorthWest, -1, -1);
        yield return data.CaseData(GridDirection.North, 0, -2);
        yield return data.CaseData(GridDirection.NorthEast, 0, -1);
        yield return data.CaseData(GridDirection.East, 1, 0);
        yield return data.CaseData(GridDirection.SouthEast, 0, 1);
        yield return data.CaseData(GridDirection.South, 0, 2);
        yield return data.CaseData(GridDirection.SouthWest, -1, 1);
        yield return data.CaseData(GridDirection.West, -1, 0);
        yield return data.CaseData(GridDirection.None, 0, 0);
      }
    }

    public static IEnumerable<NavigationTestCase> EvenColsOddRowsTestData
    {
      get
      {
        var data = new NavigationTestDataGenerator(new MapCoordinate(0, 1));
        yield return data.CaseData(GridDirection.NorthWest, 0, 0);
        yield return data.CaseData(GridDirection.North, 0, -1);
        yield return data.CaseData(GridDirection.NorthEast, 1, 0);
        yield return data.CaseData(GridDirection.East, 1, 1);
        yield return data.CaseData(GridDirection.SouthEast, 1, 2);
        yield return data.CaseData(GridDirection.South, 0, 3);
        yield return data.CaseData(GridDirection.SouthWest, 0, 2);
        yield return data.CaseData(GridDirection.West, -1, 1);
        yield return data.CaseData(GridDirection.None, 0, 1);
      }
    }

    public static IEnumerable<NavigationTestCase> OddColEvenRowsTestData
    {
      get
      {
        var data = new NavigationTestDataGenerator(new MapCoordinate(1, 0));
        yield return data.CaseData(GridDirection.NorthWest, 0, -1);
        yield return data.CaseData(GridDirection.North, 1, -2);
        yield return data.CaseData(GridDirection.NorthEast, 1, -1);
        yield return data.CaseData(GridDirection.East, 2, 0);
        yield return data.CaseData(GridDirection.SouthEast, 1, 1);
        yield return data.CaseData(GridDirection.South, 1, 2);
        yield return data.CaseData(GridDirection.SouthWest, 0, 1);
        yield return data.CaseData(GridDirection.West, 0, 0);
        yield return data.CaseData(GridDirection.None, 1, 0);
      }
    }

    public static IEnumerable<NavigationTestCase> OddColOddRowsTestData
    {
      get
      {
        var data = new NavigationTestDataGenerator(new MapCoordinate(0, 1));
        yield return data.CaseData(GridDirection.NorthWest, 0, 0);
        yield return data.CaseData(GridDirection.North, 0, -1);
        yield return data.CaseData(GridDirection.NorthEast, 1, 0);
        yield return data.CaseData(GridDirection.East, 1, 1);
        yield return data.CaseData(GridDirection.SouthEast, 1, 2);
        yield return data.CaseData(GridDirection.South, 0, 3);
        yield return data.CaseData(GridDirection.SouthWest, 0, 2);
        yield return data.CaseData(GridDirection.West, -1, 1);
        yield return data.CaseData(GridDirection.None, 0, 1);
      }
    }

    [Test]
    public void ValidateNavigationOnEvenRows()
    {
      var nav = new IsoStaggeredGridNavigator();
      OddColEvenRowsTestData.ValidateAll(nav);
      EvenColsEvenRowsTestData.ValidateAll(nav);
      OddColOddRowsTestData.ValidateAll(nav);
      EvenColsOddRowsTestData.ValidateAll(nav);
    }
  }
}
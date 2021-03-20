using NUnit.Framework;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.Test.Navigation
{
    public class IsoDiamondNavigationTest
    {
        public static IEnumerable<NavigationTestCase> TestData
        {
            get
            {
                var data = new NavigationTestDataGenerator(new MapCoordinate(0, 0));
                yield return data.CaseData(GridDirection.NorthWest, -1, 0);
                yield return data.CaseData(GridDirection.North, -1, -1);
                yield return data.CaseData(GridDirection.NorthEast, 0, -1);
                yield return data.CaseData(GridDirection.East, 1, -1);
                yield return data.CaseData(GridDirection.SouthEast, 1, 0);
                yield return data.CaseData(GridDirection.South, 1, 1);
                yield return data.CaseData(GridDirection.SouthWest, 0, 1);
                yield return data.CaseData(GridDirection.West, -1, 1);
                yield return data.CaseData(GridDirection.None, 0, 0);
            }
        }

        [Test]
        public void ValidateNavigation()
        {
            var nav = new IsoDiamondGridNavigator();
            TestData.ValidateAll(nav);
        }
    }
}

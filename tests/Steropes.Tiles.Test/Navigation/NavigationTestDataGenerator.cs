using FluentAssertions;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.Test.Navigation
{
    public class NavigationTestDataGenerator
    {
        readonly MapCoordinate origin;

        public NavigationTestDataGenerator(MapCoordinate origin)
        {
            this.origin = origin;
        }

        public NavigationTestCase CaseData(GridDirection gridDirection, int x, int y)
        {
            return new NavigationTestCase(gridDirection, origin).Expect(new MapCoordinate(x, y)).Returns(true);
        }
    }

    public static class NavigationTestCaseExtensions
    {
        public static void ValidateAll(this IEnumerable<NavigationTestCase> en, IMapNavigator<GridDirection> nav)
        {
            foreach (var testCase in en)
            {
                testCase.Validate(nav);
            }
        }
    }

    public class NavigationTestCase
    {
        public NavigationTestCase(GridDirection direction, MapCoordinate origin)
        {
            Direction = direction;
            Origin = origin;
            Result = true;
            Steps = 1;
        }

        public GridDirection Direction { get; }
        public MapCoordinate Origin { get; }
        public MapCoordinate ExpectedPosition { get; private set; }
        public bool Result { get; private set; }
        public int Steps { get; set; }

        public NavigationTestCase WithSteps(int steps)
        {
            Steps = steps;
            return this;
        }

        public NavigationTestCase Returns(bool result)
        {
            Result = result;
            return this;
        }

        public NavigationTestCase Expect(MapCoordinate expected)
        {
            ExpectedPosition = expected;
            return this;
        }

        public void Validate(IMapNavigator<GridDirection> navigator)
        {
            var br = navigator.NavigateTo(Direction, Origin, out var result, Steps);
            br.Should().Be(Result);
            result.Should().Be(ExpectedPosition, "we navigated from origin {0} in direction {1}", Origin, Direction, ExpectedPosition);
        }
    }
}

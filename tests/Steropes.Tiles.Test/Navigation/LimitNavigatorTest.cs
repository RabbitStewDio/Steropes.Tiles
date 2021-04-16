using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.Test.Navigation
{
    class TestNavigator: IMapNavigator<GridDirection>
    {
        readonly Dictionary<(GridDirection, MapCoordinate, int), (bool, MapCoordinate)> expectedCalls;

        public TestNavigator()
        {
            expectedCalls = new Dictionary<(GridDirection, MapCoordinate, int), (bool, MapCoordinate)>();
        }

        public void Expect((GridDirection direction, MapCoordinate source, int steps) parameter, (bool, MapCoordinate) result)
        {
            expectedCalls[parameter] = result;
        }
            
        public bool NavigateTo(GridDirection direction, in MapCoordinate source, out MapCoordinate result, int steps = 1)
        {
            var key = (direction, source, steps);
            if (expectedCalls.TryGetValue(key, out var r))
            {
                result = r.Item2;
                return r.Item1;
            }

            throw new AssertionException("Unexpected call");
        }
    }
        
    [TestFixture]
    public class LimitNavigatorTest
    {
        [Test]
        public void UpperLimitTest()
        {
            var input = new MapCoordinate(99, 0);
            var nav = new TestNavigator();
            nav.Expect((GridDirection.East, input, 1), 
                       (true, new MapCoordinate(100, 0)));

            var w = new LimitedRangeNavigator<GridDirection>(nav, 0, 0, 100, 100);

            w.NavigateTo(GridDirection.East, input, out var m, 1).Should().BeFalse();
            m.Should().Be(new MapCoordinate(100, 0));
        }
    }
}

using FluentAssertions;
using NSubstitute;
using NUnit.Framework;
using Steropes.Tiles.Navigation;

namespace Steropes.Tiles.Test.Navigation
{
    [TestFixture]
    public class LimitNavigatorTest
    {
        [Test]
        public void UpperLimitTest()
        {
            var nav = Substitute.For<IMapNavigator<GridDirection>>();
            var input = new MapCoordinate(99, 0);
            nav.NavigateTo(GridDirection.East, input, out var dummy)
               .Returns(x =>
               {
                   x[2] = new MapCoordinate(100, 0);
                   return true;
               });

            var w = new LimitedRangeNavigator<GridDirection>(nav, 0, 0, 100, 100);

            w.NavigateTo(GridDirection.East, input, out var m, 1).Should().BeFalse();
            m.Should().Be(new MapCoordinate(100, 0));
        }
    }
}

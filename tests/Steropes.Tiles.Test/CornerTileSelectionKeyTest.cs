using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.Matcher.Registry;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Test
{
    public class CornerTileSelectionKeyTest
    {
        [Test]
        public void TestEquals()
        {
            var r = CornerTileSelectionKey.ValueOf(Direction.Up, false, false, false);
            var l = CornerTileSelectionKey.ValueOf(Direction.Left, false, false, false);

            r.Equals(l).Should().BeFalse();
        }
    }
}

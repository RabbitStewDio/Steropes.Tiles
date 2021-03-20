using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using static Steropes.Tiles.Renderer.MapViewportBaseCalculations;

namespace Steropes.Tiles.Test.Renderer
{
    public class MapViewportTest
    {
        [Test]
        public void ValidateMapAreaCalculation()
        {
            RenderedAreaOf(new IntInsets()).Should().Be(new IntDimension(1, 1));
            RenderedAreaOf(new IntInsets(1, 2, 3, 4)).Should().Be(new IntDimension(2 + 4 + 1, 1 + 3 + 1));
        }

        [Test]
        public void ValidateRenderInsets()
        {
            EnsureViewportValid(new IntDimension(3, 3), new IntInsets()).Should().Be(new IntInsets(2 + 1, 2, 2, 2));
            EnsureViewportValid(new IntDimension(4, 4), new IntInsets()).Should().Be(new IntInsets(2 + 1, 2, 2, 2));
        }
    }
}

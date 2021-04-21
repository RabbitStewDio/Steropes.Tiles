using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Renderer;
using static Steropes.Tiles.Renderer.MapViewportExtensions;

namespace Steropes.Tiles.Test
{
    public class ViewportRendererTest
    {
        IMapViewport Create(RenderType t, double cx, double cy)
        {
            var v = new MapViewport(t)
            {
                CenterPoint = new ContinuousViewportCoordinates(cx, cy)
            };
            return v;
        }

        static IntDimension SizeOf(int w, int h)
        {
            return new IntDimension(w, h);
        }

        static ContinuousViewportCoordinates OffsetOf(double x, double y)
        {
            return new ContinuousViewportCoordinates(x, y);
        }

        [Test]
        public void ValidateFractionalTileOffset()
        {
            Create(RenderType.IsoDiamond, 10, 10).CenterPointOffset.Should().Be(OffsetOf(0, 0));
            Create(RenderType.IsoDiamond, 10.2, 9.6).CenterPointOffset.Should().Be(OffsetOf(0.2, -0.4));
        }

        [Test]
        public void ValidateRenderCoordinateOffset()
        {
            CalculateRenderCoordinateOffset(SizeOf(10, 10), OffsetOf(0, -0.5)).Should().Be(OffsetOf(20, 20.5));
            CalculateRenderCoordinateOffset(SizeOf(0, 0), OffsetOf(0.1, 0.2)).Should().Be(OffsetOf(-0.1, -0.2));
        }
    }
}

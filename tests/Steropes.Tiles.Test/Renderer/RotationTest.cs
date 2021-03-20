using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Renderer;
using System;

namespace Steropes.Tiles.Test.Renderer
{
    public class RotationTest
    {
        [Test]
        public void Test()
        {
            var viewport = new MapViewport(RenderType.IsoDiamond)
            {
                SizeInTiles = new IntDimension(100, 100),
                Overdraw = new IntInsets(0, 0, 1, 0)
            };

            var transformingMapViewport = new TransformingMapViewport(viewport)
            {
                RotationDeg = -90,
                Translation = new ContinuousViewportCoordinates(101, 15),
                CenterPoint = new ContinuousViewportCoordinates(2.5, 2.5)
            };


            var x = 10.0;
            for (var y = 10.0; y < 13; y += 0.1)
            {
                var p = transformingMapViewport.MapPositionToScreenPosition(new DoublePoint(x, y));
                Console.WriteLine($"{x}, {y} = {p}");
            }
        }
    }
}

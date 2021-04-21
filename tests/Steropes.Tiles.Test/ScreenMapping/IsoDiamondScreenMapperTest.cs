using FluentAssertions;
using NUnit.Framework;
using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.Test.ScreenMapping
{
    public class IsoDiamondScreenMapperTest : ScreenMapperTestBase
    {
        protected override RenderType RenderType => RenderType.IsoDiamond;

        public override IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> DirectlyEquivalent
        {
            get
            {
                yield return ViewCoordinate(0, 4).IsSameAs(MapCoordinate(1, 1)); // center

                // 8 tiles, counter-clockwise from origin starting at North.
                yield return ViewCoordinate(0, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(2, 2).IsSameAs(MapCoordinate(1, 0));
                yield return ViewCoordinate(4, 4).IsSameAs(MapCoordinate(2, 0));
                yield return ViewCoordinate(2, 6).IsSameAs(MapCoordinate(2, 1));
                yield return ViewCoordinate(0, 8).IsSameAs(MapCoordinate(2, 2));
                yield return ViewCoordinate(-2, 6).IsSameAs(MapCoordinate(1, 2));
                yield return ViewCoordinate(-4, 4).IsSameAs(MapCoordinate(0, 2));
                yield return ViewCoordinate(-2, 2).IsSameAs(MapCoordinate(0, 1));

                // extra line to track odd rows
                yield return ViewCoordinate(4, 0).IsSameAs(MapCoordinate(1, -1));
                yield return ViewCoordinate(6, 2).IsSameAs(MapCoordinate(2, -1));
                yield return ViewCoordinate(8, 4).IsSameAs(MapCoordinate(3, -1));
            }
        }

        public override IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> Fractional
        {
            get
            {
                yield return ViewCoordinate(-3, 0).IsSameAs(MapCoordinate(-1, 1));
                yield return ViewCoordinate(-2, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(-1, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(1, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(2, 0).IsSameAs(MapCoordinate(1, -1));
                yield return ViewCoordinate(3, 0).IsSameAs(MapCoordinate(1, -1));
                yield return ViewCoordinate(4, 0).IsSameAs(MapCoordinate(1, -1));

                yield return ViewCoordinate(0, -3).IsSameAs(MapCoordinate(-1, -1));
                yield return ViewCoordinate(0, -2).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, -1).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, 1).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, 2).IsSameAs(MapCoordinate(1, 1));
                yield return ViewCoordinate(0, 3).IsSameAs(MapCoordinate(1, 1));
                yield return ViewCoordinate(0, 4).IsSameAs(MapCoordinate(1, 1));
            }
        }

        public override IEnumerable<ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>> ContDirectlyEquivalent
        {
            get
            {
                foreach (var c in Transform(DirectlyEquivalent))
                {
                    yield return c;
                }
            }
        }

        public override IEnumerable<ScreenMapperCase<DoublePoint, ContinuousViewportCoordinates>> ContFractional
        {
            get
            {
                yield return ContViewCoordinate(-3, 0).IsSameAs(MapPosition(-0.75, 0.75));
                yield return ContViewCoordinate(-2, 0).IsSameAs(MapPosition(-0.5, 0.5));
                yield return ContViewCoordinate(-1, 0).IsSameAs(MapPosition(-0.25, 0.25));
                yield return ContViewCoordinate(0, 0).IsSameAs(MapPosition(0, 0));
                yield return ContViewCoordinate(1, 0).IsSameAs(MapPosition(0.25, -0.25));
                yield return ContViewCoordinate(2, 0).IsSameAs(MapPosition(0.5, -0.5));
                yield return ContViewCoordinate(3, 0).IsSameAs(MapPosition(0.75, -0.75));
                yield return ContViewCoordinate(4, 0).IsSameAs(MapPosition(1, -1));

                yield return ContViewCoordinate(0, -3).IsSameAs(MapPosition(-0.75, -0.75));
                yield return ContViewCoordinate(0, -2).IsSameAs(MapPosition(-0.5, -0.5));
                yield return ContViewCoordinate(0, -1).IsSameAs(MapPosition(-0.25, -0.25));
                yield return ContViewCoordinate(0, 0).IsSameAs(MapPosition(0, 0));
                yield return ContViewCoordinate(0, 1).IsSameAs(MapPosition(0.25, 0.25));
                yield return ContViewCoordinate(0, 2).IsSameAs(MapPosition(0.5, 0.5));
                yield return ContViewCoordinate(0, 3).IsSameAs(MapPosition(0.75, 0.75));
                yield return ContViewCoordinate(0, 4).IsSameAs(MapPosition(1, 1));

                yield return ContViewCoordinate(1, 0).IsSameAs(MapPosition(0.25, -0.25));
                yield return ContViewCoordinate(1, 1).IsSameAs(MapPosition(0.5, 0));
                yield return ContViewCoordinate(1, 2).IsSameAs(MapPosition(0.75, 0.25));
                yield return ContViewCoordinate(1, 3).IsSameAs(MapPosition(1, 0.5));
                yield return ContViewCoordinate(1, 4).IsSameAs(MapPosition(1.25, 0.75));

                yield return ContViewCoordinate(2, 0).IsSameAs(MapPosition(0.5, -0.5));
                yield return ContViewCoordinate(2, 1).IsSameAs(MapPosition(0.75, -0.25));
                yield return ContViewCoordinate(2, 2).IsSameAs(MapPosition(1, 0));
                yield return ContViewCoordinate(2, 3).IsSameAs(MapPosition(1.25, 0.25));
                yield return ContViewCoordinate(2, 4).IsSameAs(MapPosition(1.5, 0.5));

                yield return ContViewCoordinate(3, 0).IsSameAs(MapPosition(0.75, -0.75));
                yield return ContViewCoordinate(3, 1).IsSameAs(MapPosition(1, -0.5));
                yield return ContViewCoordinate(3, 2).IsSameAs(MapPosition(1.25, -0.25));
                yield return ContViewCoordinate(3, 3).IsSameAs(MapPosition(1.5, 0));
                yield return ContViewCoordinate(3, 4).IsSameAs(MapPosition(1.75, 0.25));

                // For completeness, lets test some fractionals as well ..
                yield return ContViewCoordinate(0.25, 0.25).IsSameAs(MapPosition(0.125, 0));
                yield return ContViewCoordinate(1.5, 1.75).IsSameAs(MapPosition(0.8125, 0.0625));
            }
        }

        [Test]
        public void TestSingle()
        {
            var screenMapperCase = ViewCoordinate(2, 2).IsSameAs(MapCoordinate(1, 0));

            var mc = ScreenToMap(screenMapperCase.ViewCoordinates.X, screenMapperCase.ViewCoordinates.Y);
            mc.Should().Be(screenMapperCase.MapCoordinate, "the view coordinate was {0}", screenMapperCase.ViewCoordinates);
        }
    }
}

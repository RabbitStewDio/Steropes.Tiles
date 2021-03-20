using Steropes.Tiles.DataStructures;
using Steropes.Tiles.Navigation;
using System.Collections.Generic;

namespace Steropes.Tiles.Test.ScreenMapping
{
    public class GridScreenMapperTest : ScreenMapperTestBase
    {
        protected override RenderType RenderType => RenderType.Grid;

        public override IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> DirectlyEquivalent
        {
            get
            {
                yield return ViewCoordinate(0, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(4, 0).IsSameAs(MapCoordinate(1, 0));
                yield return ViewCoordinate(4, 4).IsSameAs(MapCoordinate(1, 1));
                yield return ViewCoordinate(0, 4).IsSameAs(MapCoordinate(0, 1));
            }
        }

        public override IEnumerable<ScreenMapperCase<MapCoordinate, ViewportCoordinates>> Fractional
        {
            get
            {
                yield return ViewCoordinate(-3, 0).IsSameAs(MapCoordinate(-1, 0));
                yield return ViewCoordinate(-2, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(-1, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(0, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(1, 0).IsSameAs(MapCoordinate(0, 0));
                yield return ViewCoordinate(2, 0).IsSameAs(MapCoordinate(1, 0));
                yield return ViewCoordinate(3, 0).IsSameAs(MapCoordinate(1, 0));
                yield return ViewCoordinate(4, 0).IsSameAs(MapCoordinate(1, 0));
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
                yield return ContViewCoordinate(-3, 0).IsSameAs(MapPosition(-0.75, 0));
                yield return ContViewCoordinate(-2, 0).IsSameAs(MapPosition(-0.5, 0));
                yield return ContViewCoordinate(-1, 0).IsSameAs(MapPosition(-0.25, 0));
                yield return ContViewCoordinate(0, 0).IsSameAs(MapPosition(0, 0));
                yield return ContViewCoordinate(1, 0).IsSameAs(MapPosition(0.25, 0));
                yield return ContViewCoordinate(2, 0).IsSameAs(MapPosition(0.5, 0));
                yield return ContViewCoordinate(3, 0).IsSameAs(MapPosition(0.75, 0));
                yield return ContViewCoordinate(4, 0).IsSameAs(MapPosition(1, 0));
            }
        }
    }
}

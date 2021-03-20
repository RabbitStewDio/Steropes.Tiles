namespace Steropes.Tiles.TemplateGenerator.Model
{
    public enum MatcherType
    {
        Basic = 0,
        CardinalFlags = 1,
        CardinalIndex = 2,
        CellMap = 3,
        Corner = 4,
        DiagonalFlags = 5,
        NeighbourIndex = 6
    }

    public static class MatcherTypeExtensions
    {
        public static bool CanAddTiles(this MatcherType m)
        {
            return m == MatcherType.Basic;
        }
    }
}

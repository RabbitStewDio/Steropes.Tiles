using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Sprites
{
    public interface ICellMatcher
    {
        bool Match(int x, int y, out ITileTagEntrySelection result);
        int Cardinality { get; }
        ITileTagEntrySelectionFactory Owner { get; }
    }
}

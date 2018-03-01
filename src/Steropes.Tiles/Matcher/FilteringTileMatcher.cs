using System;

namespace Steropes.Tiles.Matcher
{
  public class FilteringTileMatcher<TRenderTile, TContext>: ITileMatcher<TRenderTile, TContext>
  {
    readonly Func<int, int, bool> filterCondition;
    readonly ITileMatcher<TRenderTile, TContext> matcher;

    public FilteringTileMatcher(Func<int, int, bool> filterCondition, ITileMatcher<TRenderTile, TContext> matcher)
    {
      this.filterCondition = filterCondition ?? throw new ArgumentNullException(nameof(filterCondition));
      this.matcher = matcher ?? throw new ArgumentNullException(nameof(matcher));
    }

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound)
    {
      if (filterCondition(x, y))
      {
        return matcher.Match(x, y, onMatchFound);
      }
      return false;
    }
  }
}
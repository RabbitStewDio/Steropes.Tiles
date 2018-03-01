using System.Collections.Generic;
using Steropes.Tiles.Matcher.TileTags;

namespace Steropes.Tiles.Matcher.Sprites
{
  public interface ICellMatcher
  {
    bool Match(int x, int y, out ITileTagEntrySelection result);
    int Cardinality { get; }
    ITileTagEntrySelectionFactory Owner { get; }
  }

  public static class CellMatchers
  {
    class UniformCelLMatcher<TSelector> : ICellMatcher
    {
      readonly Dictionary<ITileTagEntrySelection<TSelector>, GridMatcher> matchers;

      public UniformCelLMatcher(ITileTagEntrySelectionFactory<TSelector> owner,
                                Dictionary<ITileTagEntrySelection<TSelector>, GridMatcher> matchers)
      {
        Owner = owner;
        Cardinality = owner.Count;
        this.matchers = matchers;
      }

      public ITileTagEntrySelectionFactory Owner { get; }

      public bool Match(int x, int y, out ITileTagEntrySelection selection)
      {
        foreach (var pair in matchers)
        {
          if (pair.Value(x, y))
          {
            selection = pair.Key;
            return true;
          }
        }

        selection = default(ITileTagEntrySelection);
        return false;
      }

      public int Cardinality { get; }
    }

    public static ICellMatcher FromGridMatcher<TSelector>(ITileTagEntrySelectionFactory<TSelector> owner,
                                                                     Dictionary<ITileTagEntrySelection<TSelector>,
                                                                       GridMatcher> matchers)
    {
      return new UniformCelLMatcher<TSelector>(owner, matchers);
    }
  }
}
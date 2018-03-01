using System.Collections;
using System.Collections.Generic;

namespace Steropes.Tiles.Matcher
{
  /// <summary>
  ///   Tests every tile matcher in this aggregate matcher against the tile data.
  ///   This operation scales with the number of tile matchers added and is not
  ///   recommended for large sets.
  /// </summary>
  /// <typeparam name="TRenderTile"></typeparam>
  public class AggregateTileMatcher<TRenderTile, TContext> : ITileMatcher<TRenderTile, TContext>, IEnumerable<ITileMatcher<TRenderTile, TContext>>
  {
    readonly List<ITileMatcher<TRenderTile, TContext>> matchers;

    public AggregateTileMatcher(List<ITileMatcher<TRenderTile, TContext>> matchers)
    {
      this.matchers = new List<ITileMatcher<TRenderTile, TContext>>(matchers);
    }

    public AggregateTileMatcher(params ITileMatcher<TRenderTile, TContext>[] matchers)
    {
      this.matchers = new List<ITileMatcher<TRenderTile, TContext>>(matchers);
    }



    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> resultCollector)
    {
      var result = false;
      for (var i = 0; i < matchers.Count; i++)
      {
        var m = matchers[i];
        result |= m.Match(x, y, resultCollector);
      }
      return result;
    }

    public void Add(ITileMatcher<TRenderTile, TContext> m)
    {
      matchers.Add(m);
    }

    public void Add(IEnumerable<ITileMatcher<TRenderTile, TContext>> m)
    {
      matchers.AddRange(m);
    }

    public IEnumerator<ITileMatcher<TRenderTile, TContext>> GetEnumerator()
    {
      return matchers.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }
  }
}
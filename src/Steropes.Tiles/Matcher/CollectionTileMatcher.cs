using System;
using System.Collections.Generic;
using Steropes.Tiles.Matcher.Sprites;

namespace Steropes.Tiles.Matcher
{
  public class CollectionTileMatcher<TRenderTile, TContext, TData>: ITileMatcher<TRenderTile, TContext>
  {
    readonly Func<int, int, IEnumerable<TData>> keyFn;
    readonly Mapper<TData, TRenderTile, TContext> mapper;

    public CollectionTileMatcher(Func<int, int, IEnumerable<TData>> keyFn, Mapper<TData, TRenderTile, TContext> mapper)
    {
      this.keyFn = keyFn ?? throw new ArgumentNullException(nameof(keyFn));
      this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public bool Match(int x, int y, TileResultCollector<TRenderTile, TContext> onMatchFound)
    {
      var en = keyFn(x, y);
      if (en == null)
      {
        return false;
      }

      bool found = false;
      foreach (var data in en)
      {
        if (mapper(data, out TRenderTile result, out TContext context))
        {
          onMatchFound(SpritePosition.Whole, result, context);
          found = true;
        }
      }
      return found;
    }
  }
}
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Steropes.Tiles.Matcher.Registry
{
  public class BasicTileRegistry<TRenderTile> : ITileRegistry<TRenderTile>,
                                                IEnumerable<KeyValuePair<string, TRenderTile>>
  {
    readonly Dictionary<string, TRenderTile> tilesByName;

    public BasicTileRegistry()
    {
      tilesByName = new Dictionary<string, TRenderTile>();
    }

    public IEnumerator<KeyValuePair<string, TRenderTile>> GetEnumerator()
    {
      return tilesByName.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
      return GetEnumerator();
    }

    public bool TryFind(string tag, out TRenderTile tile)
    {
      return tilesByName.TryGetValue(tag, out tile);
    }

    public void Add(string name, TRenderTile tile)
    {
      tilesByName.Add(name, tile);
    }
  }
}
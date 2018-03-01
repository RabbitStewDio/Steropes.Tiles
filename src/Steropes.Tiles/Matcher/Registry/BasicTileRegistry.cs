using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

namespace Steropes.Tiles.Matcher.Registry
{
  public class BasicTileRegistry<TRenderTile> : ITileRegistry<TRenderTile>,
                                                IEnumerable<KeyValuePair<string, TRenderTile>>
  {
    readonly TRenderTile fallback;
    readonly Dictionary<string, TRenderTile> tilesByName;
    readonly TraceSource logger = TileRegistryTracing.MissingTilesTracer;

    public BasicTileRegistry(TRenderTile fallback)
    {
      this.fallback = fallback;
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

    public TRenderTile Find(string tag)
    {
      if (tilesByName.TryGetValue(tag, out TRenderTile tile))
      {
        return tile;
      }

      logger.TraceEvent(TraceEventType.Warning, 0, "Missing tile in registry for tag ({0})", tag);
      return fallback;
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
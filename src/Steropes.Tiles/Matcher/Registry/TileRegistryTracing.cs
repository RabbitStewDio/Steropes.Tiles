using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Steropes.Tiles.Matcher.Registry
{
  public class TileRegistryTracing
  {
    TileRegistryTracing()
    {
    }

    public static readonly TraceSource MissingTilesTracer = TracingUtil.Create<TileRegistryTracing>();

    public static void EmitMissingTileWarning(string tag)
    {
      MissingTilesTracer.TraceEvent(TraceEventType.Warning, 0, "Missing tile in registry for tag ({0})", tag);
    }

    public static void EmitMissingTileWarning(string tag, IReadOnlyList<string> alts)
    {
      if (alts == null || alts.Count == 0)
      {
        EmitMissingTileWarning(tag);
      }
      else if (MissingTilesTracer.Switch.ShouldTrace(TraceEventType.Warning))
      {
        var t = string.Join(",", alts);
        MissingTilesTracer.TraceEvent(TraceEventType.Warning, 0, "Missing tile in registry for tag ({0}) or alternative tags {1}", tag, t);
      }
    }
  }
}